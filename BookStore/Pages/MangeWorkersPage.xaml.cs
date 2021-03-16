using BookStore.BLL;
using BookStore.ViewModel.Controls;
using BookStore.ViewModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BookStore.Client.Pages
{
    /// <summary>
    /// Interaction logic for MangeWorkersPage.xaml
    /// </summary>
    public partial class MangeWorkersPage : Page
    {
        public MangeWorkersPage()
        {
            InitializeComponent();
            listView.ItemsSource = Logic.Instance.GetWorkers();

            for (int i = 0; i < (int)SearchCata.Length; i++)
            {
                searchKataBox.Items.Add(EnumToString((SearchCata)i));
            }
            searchKataBox.SelectedIndex = 0;

            Logic.Instance.WorkerAdded += (s,e)=> listView.ItemsSource = Logic.Instance.GetWorkers();
            Logic.Instance.WorkerRemoved += (s, e) => listView.ItemsSource = Logic.Instance.GetWorkers();
            Logic.Instance.WorkerUpdated += (s, e) => listView.ItemsSource = Logic.Instance.GetWorkers();
        }

        private enum SearchCata
        {
            First_Name = 0,
            Last_Name ,
            Username,
            Length
        }

        private string EnumToString(Enum e)
        {
            string name = Enum.GetName(e.GetType(), e);

            string[] arr = name.Split('_');

            if (arr.Length > 1)
                return $"{arr[0]} {arr[1]}";

            return arr[0];
        }

        bool sortUpFName = false;
        bool sortUpLName = false;
        bool sortUpUName = false;

        private void Sort_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                List<WorkerLabel> toList = listView.ItemsSource.OfType<WorkerLabel>().ToList();
                switch (btn.Name)
                {
                    case "sortFNameBtn":
                        {
                            if (sortUpFName)
                            {
                                toList.Sort((one, sec) => sec.LabelWorker.FirstName.CompareTo(one.LabelWorker.FirstName));
                                sortUpFName = false;
                            }
                            else
                            {
                                toList.Sort((one, sec) => one.LabelWorker.FirstName.CompareTo(sec.LabelWorker.FirstName));
                                sortUpFName = true;
                            }
                            listView.ItemsSource = toList;
                        }
                        break;
                    case "sortLNameBtn":
                        {
                            if (sortUpLName)
                            {
                                toList.Sort((one, sec) => sec.LabelWorker.LastName.CompareTo(one.LabelWorker.LastName));
                                sortUpLName = false;
                            }
                            else
                            {
                                toList.Sort((one, sec) => one.LabelWorker.LastName.CompareTo(sec.LabelWorker.LastName));
                                sortUpLName = true;
                            }
                            listView.ItemsSource = toList;
                        }
                        break;
                    case "sortUserNameBtn":
                        {
                            if (sortUpUName)
                            {
                                toList.Sort((one, sec) => sec.LabelWorker.Username.CompareTo(one.LabelWorker.Username));
                                sortUpUName = false;
                            }
                            else
                            {
                                toList.Sort((one, sec) => one.LabelWorker.Username.CompareTo(sec.LabelWorker.Username));
                                sortUpUName = true;
                            }

                            listView.ItemsSource = toList;
                        }
                        break;                    
                    default:
                        break;
                }
            }
        }

        private void SearchBar_Searched(res.UserControls.SearchBar sender, res.UserControls.SearchEventArgs args)
        {
            Predicate<WorkerView> condition;

            if (args.SearchedText.ToLower() == string.Empty)
            {
                listView.ItemsSource = Logic.Instance.GetWorkers();
                return;
            }

            switch (searchKataBox.SelectedIndex)
            {
                case 0:
                    condition = (i) => i.FirstName.ToLower().Contains(args.SearchedText.ToLower());
                    break;
                case 1:
                    condition = (i) => i.LastName.ToLower().Contains(args.SearchedText.ToLower());
                    break;
                case 2:
                    condition = (i) => i.Username.ToLower().Contains(args.SearchedText.ToLower());
                    break;               
                default:
                    return;
            }

            listView.ItemsSource = Logic.Instance.GetWorkers(condition);
        }

        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ListView)sender).SelectedItem == null) return;

            updateForm.Worker = ((WorkerLabel)((ListView)sender).SelectedItem).LabelWorker;
        }
    }
}
