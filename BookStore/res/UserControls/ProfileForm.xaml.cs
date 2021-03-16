using BookStore.BLL;
using BookStore.ViewModel.Models;
using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using BookStore.BLL.util;
using System.Security.Cryptography;

namespace BookStore.Client.res.UserControls
{
    /// <summary>
    /// Interaction logic for ProfileForm.xaml
    /// </summary>
    public partial class ProfileForm : UserControl
    {
        private WorkerView worker;
        private bool createNew;
        public WorkerView Worker
        {
            get { return worker; }
            set
            {
                worker = value;
                createNew = false;
                add_Btn.Visibility = Visibility.Collapsed;
                passBtn.Visibility = Visibility.Visible;
                newPassBlock.Visibility = Visibility.Collapsed;
                rem_Btn.Visibility = Visibility.Visible;
                upd_Btn.Visibility = Visibility.Visible;

                if (Logic.Instance.IsCurrentUser(Worker))
                    passGridRow.Visibility = Visibility.Visible;
                else
                    passGridRow.Visibility = Visibility.Collapsed;

                if (Logic.Instance.UserRank == WorkerView.Rank.Manager)
                {
                    if (Logic.Instance.IsCurrentUser(Worker))
                    {
                        passGridRow.Visibility = Visibility.Visible;
                        upd_Btn.Visibility = Visibility.Visible;
                        rem_Btn.Visibility = Visibility.Collapsed;
                    }
                    
                    rankField.Visibility = Visibility.Visible;
                   
                    rankField.SelectedIndex = (int)worker?.WorkerRank;
                    rankTextBlock.Visibility = Visibility.Hidden;
                }
                else
                {
                    if (Logic.Instance.IsCurrentUser(Worker))
                    {
                        rankField.Visibility = Visibility.Hidden;                        
                        rankTextBlock.Visibility = Visibility.Visible;
                        rem_Btn.Visibility = Visibility.Collapsed;
                        rankText.Text = worker.WorkerRank.ToString();
                    }
                    else
                        throw new UserAccessException();
                }



                firstField.Value = worker.FirstName;
                lastField.Value = worker.LastName;
                userField.Value = worker.Username;

                imgField.Image = worker.GetUserImage();
                imgField.ImageBytes = worker.ProfileImage;

            }
        }

        public ProfileForm(WorkerView worker) : this()
        {
            Worker = worker;
        }

        public ProfileForm()
        {
            InitializeComponent();
            errorBox.Hide();
            createNew = true;
            add_Btn.Visibility = Visibility.Visible;
            upd_Btn.Visibility = Visibility.Collapsed;
            rem_Btn.Visibility = Visibility.Collapsed;
            passBtn.Visibility = Visibility.Collapsed;
            passBoxFields.Visibility = Visibility.Collapsed;
            newPassBlock.Visibility = Visibility.Visible;
            rankField.Visibility = Visibility.Visible;
            rankTextBlock.Visibility = Visibility.Collapsed;

            rankField.Items.Add(WorkerView.Rank.Manager);
            rankField.Items.Add(WorkerView.Rank.Worker);
            
        }

        private void Update_Button(object sender, RoutedEventArgs e)
        {
            if (!createNew)
            {
                errorBox.Hide();

                if (firstField.Value == string.Empty)
                {
                    errorBox.Pop("First name can't be Empty.");
                    return;
                }
                if (lastField.Value == string.Empty)
                {
                    errorBox.Pop("Last name can't be Empty.");
                    return;
                }
                if (userField.Value == string.Empty)
                {
                    errorBox.Pop("Username can't be Empty.");
                    return;
                }

                if (Logic.Instance.IsCurrentUser(Worker))
                {
                    WorkerView updated = new WorkerView(0, firstField.Value, lastField.Value, userField.Value, Worker.WorkerRank, imgField.ImageBytes);
                    try
                    {
                        Logic.Instance.UpdateWorker(updated, Worker);
                        Worker = Logic.Instance.GetUserView(worker.Id);
                    }
                    catch (WorkerUpdateFailedException ex)
                    {
                        Logger.Exception(ex);
                        errorBox.Pop("Some thing with the Id went wrong..");
                    }
                    catch (PrimeryKeyAllReadyExistException ex)
                    {
                        Logger.Exception(ex);
                        errorBox.Pop("Username All Ready Taken.");
                    }

                    return;
                }

                if (rankField.SelectedItem == null)
                {
                    errorBox.Pop("Worker rank must be set.");
                    return;
                }

                if (Logic.Instance.UserRank == WorkerView.Rank.Manager)
                {
                    WorkerView.Rank? rank = rankField.SelectedItem as WorkerView.Rank?;
                    if (rank == null) throw new WhatException();

                    WorkerView updated = new WorkerView(0, firstField.Value, lastField.Value, userField.Value, (WorkerView.Rank)(rankField.SelectedItem), imgField.ImageBytes);
                    try
                    {
                        Logic.Instance.UpdateWorker(updated, Worker);
                    }
                    catch (WorkerUpdateFailedException ex)
                    {
                        Logger.Exception(ex);
                        errorBox.Pop("Some thing with the Id went wrong..");
                    }
                    catch (PrimeryKeyAllReadyExistException ex)
                    {
                        Logger.Exception(ex);
                        errorBox.Pop("Username All Ready Taken.");
                    }
                }
            }
        }

        private void passSaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!createNew)
            {
                passErrorBox.Hide();
                if (passOldField.Password != string.Empty && passNewField.Password != string.Empty)
                {
                    byte[] hashedoldPass;
                    byte[] newHashedPass;
                    byte[] confirmHashedPass;
                    try
                    {
                        hashedoldPass = SHA256.Create().ComputeHash(Encoding.Unicode.GetBytes(passOldField.Password));
                        newHashedPass = SHA256.Create().ComputeHash(Encoding.Unicode.GetBytes(passNewField.Password));
                        confirmHashedPass = SHA256.Create().ComputeHash(Encoding.Unicode.GetBytes(passConfirmField.Password));
                        passOldField.Password = string.Empty;
                        passNewField.Password = string.Empty;
                        passConfirmField.Password = string.Empty;
                    }
                    catch (Exception ex)
                    {
                        Logger.Exception(ex, "Unknown Exception, Closing App.");
                        Logic.Instance.Shutdown(1);
                        return;
                    }

                    if (ComprePass(newHashedPass, confirmHashedPass))
                    {
                        try
                        {
                            if (Logic.Instance.ChangePassword(Worker.Id, hashedoldPass, newHashedPass))
                            {
                                passErrorBox.Pop("Password Set Successfully!");
                            }
                            else
                            {
                                passErrorBox.Pop("Password is Incorrect.");
                            }
                        }
                        catch (UserAccessException ec)
                        {
                            Logger.Exception(ec);
                        }
                        catch (Exception ec)
                        {
                            Logger.Exception(ec, "Unknown Exception, Closing App.");
                            Logic.Instance.Shutdown(1);
                        }
                    }
                    else
                        passErrorBox.Pop("Passwords Are Not The Same.");


                }
                else
                    passErrorBox.Pop("Password Field Is Empty.");
            }

        }

        private void Close_Pass_Click(object sender, RoutedEventArgs e)
        {
            if (!createNew)
            {
                passBoxFields.Visibility = Visibility.Collapsed;
                passBtn.Visibility = Visibility.Visible;
                passErrorBox.Hide();
            }
        }

        private void passBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!createNew)
            {
                passBoxFields.Visibility = Visibility.Visible;
                passBtn.Visibility = Visibility.Collapsed;
                passErrorBox.Hide();
            }
        }

        private bool ComprePass(byte[] pass1, byte[] pass2)
        {
            if (pass1.Length != pass2.Length) return false;
            for (int i = 0; i < pass1.Length; i++)
            {
                if (pass1[i] != pass2[i])
                    return false;
            }
            return true;
        }

        private void Remove_Button(object sender, RoutedEventArgs e)
        {
            if (!Logic.Instance.IsCurrentUser(Worker))
            {
                Logic.Instance.RemoveWorker(Worker.Id);
                CleanFields();
                Worker = null;
            }
        }

        private void Add_Button(object sender, RoutedEventArgs e)
        {
            if(createNew)
            {
                string fName = firstField.Value;
                string lName = lastField.Value;
                string uName = userField.Value;


                byte[] img = imgField.ImageBytes;

                WorkerView.Rank? rank = rankField.SelectedItem as WorkerView.Rank?;

                if(string.IsNullOrWhiteSpace(fName))
                {
                    errorBox.Pop("First Name must be set.");
                    return;
                }
                if (string.IsNullOrWhiteSpace(lName))
                {
                    errorBox.Pop("Last Name must be set.");
                    return;
                }
                if (string.IsNullOrWhiteSpace(uName))
                {
                    errorBox.Pop("Username must be set.");
                    return;
                }

                if(rank == null)
                {
                    errorBox.Pop("Rank must be set.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(passBoxFieldNew.Password))
                {
                    errorBox.Pop("Password must be set.");
                    return;
                }

                byte[] newPass = SHA256.Create().ComputeHash(Encoding.Unicode.GetBytes(passBoxFieldNew.Password));
                byte[] confPass = SHA256.Create().ComputeHash(Encoding.Unicode.GetBytes(passBoxFieldConf.Password));
                if(!ComprePass(newPass, confPass))
                {
                    errorBox.Pop("Passwords are not the same.");
                    return;
                }

                WorkerView newWorker = new WorkerView(0, fName, lName, uName, rank.GetValueOrDefault(), img);

                try
                {

                    Logic.Instance.AddWorker(newWorker, newPass);
                     CleanFields();
                    
                }
                catch (UserLoggedInException ex)
                {
                    Logger.Exception(ex);
                }
                catch (UserAccessException ex)
                {
                    Logger.Exception(ex);
                }
                catch (PrimeryKeyAllReadyExistException ex)
                {
                    Logger.Exception(ex);
                    errorBox.Pop("ISBN All Ready In Use.");
                }
                catch (Exception ex)
                {
                    Logger.Exception(ex);
                    Logger.Fetal("Unknown Exception on Book Submitting.");
                    Logic.Instance.Shutdown(1);
                }

            }
        }

        private void CleanFields()
        {
            firstField.Value = string.Empty;
            lastField.Value = string.Empty;
            userField.Value = string.Empty;

            imgField.ImageBytes = null;
            rankField.SelectedItem = null;

            passBoxFieldNew.Password = string.Empty;
            passBoxFieldConf.Password = string.Empty;

            passOldField.Password = string.Empty;
            passNewField.Password = string.Empty;
            passConfirmField.Password = string.Empty;
        }
    }
}
