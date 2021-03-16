using BookStore.BLL;
using BookStore.ViewModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using BookStore.ViewModel.Controls;

namespace BookStore.Client.Pages
{
    /// <summary>
    /// Interaction logic for InventoryPage.xaml
    /// </summary>
    public partial class InventoryPage : Page
    {

        public InventoryPage()
        {
            InitializeComponent();
            listView.ItemsSource = Logic.Instance.GetStoreItems();
            Logic.Instance.ItemAdded += (s,e) => listView.ItemsSource = Logic.Instance.GetStoreItems();
            Logic.Instance.ItemRemoved += (s, e) => listView.ItemsSource = Logic.Instance.GetStoreItems();
            Logic.Instance.ItemUpdated += (s, e) => listView.ItemsSource = Logic.Instance.GetStoreItems();
            for (int i = 0; i < (int)SearchCata.Length; i++)
            {
                searchKataBox.Items.Add(EnumToString((SearchCata)i));
            }
            searchKataBox.SelectedIndex = 0;
        }

        private void SearchBar_Searched(res.UserControls.SearchBar sender, res.UserControls.SearchEventArgs args)
        {
            Predicate<StoreItemView> condition;

            if (args.SearchedText.ToLower() == string.Empty)
            {
                listView.ItemsSource = Logic.Instance.GetStoreItems();
                return;
            }

            switch (searchKataBox.SelectedIndex)
            {
                case 0:
                    condition = (i) => i.Name.ToLower().Contains(args.SearchedText.ToLower());
                    break;
                case 1:
                    condition = (i) => i.ISBN.Contains(args.SearchedText);
                    break;
                case 2:
                    condition = (i) =>
                    {
                        return i.PublishedDate.ToString("dd/MM/yyyy").Contains(args.SearchedText) ||
                        i.PublishedDate.ToString("d/M/yy").Contains(args.SearchedText);
                    };
                    break;
                case 3:
                    condition = (i) =>
                    {
                        BookView item = i as BookView;
                        if (item == null) return false;
                        return item.Author.ToLower().Contains(args.SearchedText.ToLower());
                    };
                    break;
                case 4:
                    condition = (i) => {
                        BookView item = i as BookView;
                        if (item == null) return false;
                        return item.Publisher.ToLower().Contains(args.SearchedText.ToLower());
                    };
                    break;
                case 5:
                    condition = (i) => {
                        BookView item = i as BookView;
                        if (item == null) return false;

                        LinkedList<GenreView> gens = new LinkedList<GenreView>();

                        foreach (var genre in item.Genres)
                        {
                            if(genre.Name.ToLower().Contains(args.SearchedText.ToLower()))
                            {
                                gens.AddLast(genre);
                            }
                        }

                        foreach (var gen in gens)
                        {
                            if (item.Genres[gen.Id].Active)
                                return true;
                        }

                        return false;

                    };
                    break;
                case 6:
                    condition = (i) =>
                    {
                        BookView item = i as BookView;
                        if (item == null) return false;
                        int num;
                        if (!int.TryParse(args.SearchedText, out num)) return false;

                        return item.CatalogNumber == num;
                    };
                    break;
                case 7:
                    condition = (i) => 
                    {
                        BookView item = i as BookView;
                        if (item == null) return false;

                        return item.Edition.Contains(args.SearchedText.ToLower());
                    };
                    break;
                default:
                    return;
            }

            listView.ItemsSource = Logic.Instance.GetStoreItems(condition);

        }
        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(((ListView)sender).SelectedItem == null) return;

            StoreItemView selectedItem = ((ViewModel.Controls.StoreItemControl)((ListView)sender).SelectedItem).ItemView;
            BookView book = selectedItem as BookView;
            if(book != null)
            {
                updateForm.Navigate(new BookForm(book));
                return;
            }

            JournalView journal = selectedItem as JournalView;
            if (journal != null)
            {
                updateForm.Navigate(new JournalForm(journal));
                return;
            }

        }

        enum SearchCata
        {
            Name =0,
            ISBN,
            Published_Date,
            Author,
            Publisher,
            Genres,
            Catalog_Num,
            Edition,
            Length
        }

        private string EnumToString(Enum e)
        {
            string name = Enum.GetName(e.GetType(),e);

            string[] arr = name.Split('_');

            if (arr.Length > 1)
                return $"{arr[0]} {arr[1]}";

            return arr[0];
        }

        private void advanced_Click(object sender, RoutedEventArgs e)
        {
            if(!advanceSearch.IsOpen)
                advanceSearch.IsOpen = true;
        }

        private void AdvSearch_Click(object sender, RoutedEventArgs e)
        {

            string name = sNameField.Value.ToLower();
            string iSBN = sISBNField.Value;
            string author = sAuthorField.Value.ToLower();
            string publisher = sPublisherField.Value.ToLower();
            string edition = sEditionField.Value.ToLower();
            string Cata = sCataField.Value;
            GenresView genres = sGenerBox.Geners;
            DateTime? date = sDateField.SelectedDate;

            List<Predicate<StoreItemView>> predics = new List<Predicate<StoreItemView>>(8);

            if (name != string.Empty) predics.Add((i) => i.Name.ToLower().Contains(name));
            if (iSBN != string.Empty) predics.Add((i) => i.ISBN.Contains(iSBN));
            if (author != string.Empty) predics.Add((i) =>
            {
                BookView item = i as BookView;
                if (item == null) return false;
                return item.Author.ToLower().Contains(author);
            });
            if (publisher != string.Empty) predics.Add((i) => {
                BookView item = i as BookView;
                if (item == null) return false;
                return item.Publisher.ToLower().Contains(publisher);
            });
            if (edition != string.Empty) predics.Add((i) =>
            {
                BookView item = i as BookView;
                if (item == null) return false;

                return item.Edition.Contains(edition);
            });
            if (Cata != string.Empty) predics.Add((i) =>
            {
                BookView item = i as BookView;
                if (item == null) return false;
                int num;
                if (!int.TryParse(Cata, out num)) return false;

                return item.CatalogNumber == num;
            });
            if (genres.Count != 0) predics.Add((i) => {
                BookView item = i as BookView;
                if (item == null) return false;

                bool contain = false;
                for (int j = 0; j < GenresView.AllGenres.Length; j++)
                {
                    if (genres[j].Active)
                    {
                        if (item.Genres[j].Active)
                            contain = true;
                        else
                            return false;
                    }
                }
                return contain;
             
            });
            if(date != null) predics.Add((i) => i.PublishedDate.Equals(date));

            if (predics.Count == 0) return;

            listView.ItemsSource = Logic.Instance.GetStoreItems(predics.ToArray());

            advanceSearch.IsOpen = false;
        }

        private void Close_Pop(object sender, RoutedEventArgs e)
        {
            advanceSearch.IsOpen = false;
        }

        bool sortUpName = false;
        bool sortUpPrice = false;
        bool sortUpUnits = false;
        bool sortUpISBN = false;

        private void Sort_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if(btn != null)
            {
                List<StoreItemControl> toList = listView.ItemsSource.OfType<StoreItemControl>().ToList();
                switch (btn.Name)
                {
                    case "sortNameBtn":
                        {
                            if (sortUpName)
                            {
                                toList.Sort((one, sec) => sec.ItemView.Name.CompareTo(one.ItemView.Name));
                                sortUpName = false;
                            }
                            else
                            {
                                toList.Sort((one, sec) => one.ItemView.Name.CompareTo(sec.ItemView.Name));
                                sortUpName = true;
                            }
                            listView.ItemsSource = toList;
                        }
                        break;
                    case "sortPriceBtn":
                        {
                            if(sortUpPrice)
                            {
                                toList.Sort((one, sec) => sec.ItemView.UnitPrice.CompareTo(one.ItemView.UnitPrice));
                                sortUpPrice = false;
                            }
                            else
                            {
                                toList.Sort((one, sec) => one.ItemView.UnitPrice.CompareTo(sec.ItemView.UnitPrice));
                                sortUpPrice = true;
                            }
                            listView.ItemsSource = toList;
                        }
                        break;
                    case "sortUnitsBtn":
                        {
                            if(sortUpUnits)
                            {
                                toList.Sort((one, sec) => sec.ItemView.UnitsInStock.CompareTo(one.ItemView.UnitsInStock));
                                sortUpUnits = false;
                            }
                            else
                            {
                                toList.Sort((one, sec) => one.ItemView.UnitsInStock.CompareTo(sec.ItemView.UnitsInStock));
                                sortUpUnits = true;
                            }
                            
                            listView.ItemsSource = toList;
                        }
                        break;
                    case "sortISBNBtn":
                        {
                            if(sortUpISBN)
                            {
                                toList.Sort((one, sec) => sec.ItemView.ISBN.CompareTo(one.ItemView.ISBN));
                                sortUpISBN = false;
                            }
                            else
                            {
                                toList.Sort((one, sec) => one.ItemView.ISBN.CompareTo(sec.ItemView.ISBN));
                                sortUpISBN = true;
                            }                          
                            listView.ItemsSource = toList;
                        }
                        break;
                    default:
                        break;
                }

            }
        }
    }
}
