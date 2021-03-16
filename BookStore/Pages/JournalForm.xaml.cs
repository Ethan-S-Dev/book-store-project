using BookStore.BLL;
using BookStore.BLL.util;
using BookStore.ViewModel.Models;
using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace BookStore.Client.Pages
{
    /// <summary>
    /// Interaction logic for JournalForm.xaml
    /// </summary>
    public partial class JournalForm : Page
    {
        private bool CreateNew;
        private JournalView item;
        public JournalForm(JournalView journal) : this()
        {
            CreateNew = false;
            item = journal;
            submitBtnTxt.Text = "Update";

            nameField.Value = journal.Name;

            isbnField.Value = journal.ISBN;

            priceField.Value = journal.UnitPrice.ToString();

            int disc = (int)(journal.Discount * 100);

            discountField.Value = $"{disc}%";


            issueField.Value = journal.IssueNumber.ToString();

            fieldField.Value = journal.Field;


            imageField.Image = journal.GetDisplayImage();
            imageField.ImageBytes = journal.DisplayImage;


            unitInStockField.NumValue = journal.UnitsInStock;

            dateField.SelectedDate = journal.PublishedDate;

            volumeField.Value = journal.VolumeNumber.ToString();

            submitBtnTxt.Text = "Update";
            removeBtn.Visibility = System.Windows.Visibility.Visible;
        }
        public JournalForm()
        {
            const int boxSize = 150;
            CreateNew = true;


            InitializeComponent();
            removeBtn.Visibility = System.Windows.Visibility.Collapsed;
            nameField.Text = "Name:";
            nameField.BoxWidth = boxSize;

            isbnField.Text = "ISSN:";
            isbnField.BoxWidth = boxSize;
            isbnField.PreviewInput += IsbnField_PreviewInput;

            priceField.Text = "Price:";
            priceField.BoxWidth = boxSize;
            priceField.PreviewInput += PriceField_PreviewInput;


            discountField.Text = "Discount:";
            discountField.BoxWidth = boxSize;
            discountField.PreviewInput += DiscountField_PreviewInput;
            discountField.InputLostFocus += DiscountField_InputLostFocus;

            fieldField.Text = "Field:";
            fieldField.BoxWidth = boxSize;

            volumeField.Text = "Volume Number:";
            volumeField.BoxWidth = 40;
            volumeField.PreviewInput += NumField_TextInput;

            issueField.Text = "Issue Number:";
            issueField.BoxWidth = 40;
            issueField.PreviewInput += NumField_TextInput;
        }

        private void IsbnField_PreviewInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            TextBox box = (TextBox)sender;
            Regex regex = new Regex(@"[^0-9-xX]");

            if (e.Text == "\b") return;

            if (regex.IsMatch(e.Text))
            {
                e.Handled = true;
            }
        }

        private void NumField_TextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            TextBox box = (TextBox)sender;
            Regex regex = new Regex(@"[^0-9]");

            if (e.Text == "\b") return;

            if (regex.IsMatch(e.Text))
            {
                e.Handled = true;
            }

        }

        private void DiscountField_InputLostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            TextBox box = (TextBox)sender;
            if (!box.Text.Contains("%"))
            {
                int val;
                if (int.TryParse(box.Text, out val))
                    if (val > 100)
                        box.Text = "100%";
                    else
                        box.Text = $"{box.Text}%";
            }
        }

        private void DiscountField_PreviewInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            TextBox box = (TextBox)sender;
            Regex regex = new Regex(@"[^0-9%]");

            if (e.Text == "\b") return;

            if (!regex.IsMatch(e.Text))
            {
                if (e.Text.Contains("%"))
                {
                    if (box.Text.Contains("%"))
                    {
                        e.Handled = true;
                    }

                    string[] splited = box.Text.Split('%');
                    int val;
                    if (int.TryParse(splited[0], out val))
                        if (val > 100)
                        {
                            box.Text = "100%";
                            e.Handled = true;
                        }
                }
                else
                {
                    string[] splited = box.Text.Split('%');
                    if (splited.Length > 1)
                        e.Handled = true;
                }
            }
            else
                e.Handled = true;

        }

        private void PriceField_PreviewInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            TextBox box = (TextBox)sender;
            Regex regex = new Regex(@"[^0-9.]");
            if (!regex.IsMatch(e.Text))
            {
                if (e.Text.Contains("."))
                {
                    if (box.Text.Contains("."))
                    {
                        e.Handled = true;
                    }
                }
                else
                {
                    string[] splited = box.Text.Split('.');
                    if (splited.Length > 1)
                    {
                        if (splited[1].Length > 1)
                        {
                            e.Handled = true;
                        }
                    }
                }
            }
            else
                e.Handled = true;
        }

        private void submitBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            errorBox.Hide();
            if (CreateNew)
            {


                try
                {
                    JournalView journ;
                    if (BuildJournal(out journ))
                    {
                        Logic.Instance.AddStoreItem(journ);
                        CleanJurnal();
                    }
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
                    errorBox.Pop("ISSN All Ready In Use.");
                }
                catch (Exception ex)
                {
                    Logger.Exception(ex);
                    Logger.Fetal("Unknown Exception on Journal Submitting.");
                    Logic.Instance.Shutdown(1);
                }
                
            }else
            {
                if (item == null)
                {
                    errorBox.Pop("Item Was All Ready Deleted.");
                    return;
                }

                try
                {
                    JournalView journ;
                    if (BuildJournal(out journ))                    
                        Logic.Instance.UpdateStoreItem(journ, item);

                    
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

        private bool BuildJournal(out JournalView item)
        {
            item = null;

            string name = nameField.Value;
            if (name == "" || name == null) { errorBox.Pop("Invalid Name."); return false; }

            decimal price;
            if (!decimal.TryParse(priceField.Value, out price)) { errorBox.Pop("Invalid Price."); return false; }

            int precent = 0;
            if (!string.IsNullOrWhiteSpace(discountField.Value))
            {
                string[] split = discountField.Value.Split('%');
                if (!int.TryParse(split[0], out precent)) { errorBox.Pop("Invalid Discount."); return false; }
            }
            float discount = precent / 100.0f;

            string isbn = isbnField.Value;
            if (isbn == "" || isbn == null) { errorBox.Pop("Invalid ISBN."); return false; }

            int uIS = unitInStockField.NumValue;

            byte[] image = null;
            if (!imageField.IsDefault)
                image = imageField.ImageBytes;

            DateTime? publishedDate = dateField.SelectedDate;
            if (publishedDate == null) { errorBox.Pop("Invalid Date."); return false; }
            if(publishedDate > DateTime.Now) { errorBox.Pop("Invalid Date."); return false; }


            string field = fieldField.Value;
            if (field == "" || field == null) { errorBox.Pop("Field cannot be empty."); return false; }

            int volume;
            if (!int.TryParse(volumeField.Value, out volume)) { errorBox.Pop("Invalid Volume Number."); return false; }

            int issue;
            if (!int.TryParse(issueField.Value, out issue)) { errorBox.Pop("Invalid Issue Number."); return false; }

            item = new JournalView(0, name, price, discount, isbn, uIS, field, volume, issue, publishedDate.GetValueOrDefault(), image);

            return true;
        }

        private void CleanJurnal()
        {
            nameField.Value = "";
            priceField.Value = "";
            discountField.Value = "";
            isbnField.Value = "";
            unitInStockField.NumValue = 0;
            imageField.ImageBytes = null;
            imageField.Image = null;
            dateField.SelectedDate = null;


            fieldField.Value = "";
            volumeField.Value = "";
            issueField.Value = "";
        }


        private void removeBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!CreateNew)
            {
                if (item == null)
                {
                    errorBox.Pop("Item Is All Ready Deleted.");
                    return;
                }
                
                Logic.Instance.RemoveStorItem(item.Id);
                item = null;
                
            }
        }
    }
}
