using BookStore.ViewModel.Models;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace BookStore.Client.res.UserControls
{
    /// <summary>
    /// Interaction logic for MultiSelectionBox.xaml
    /// </summary>
    public partial class MultiSelectionBox : UserControl
    {
        private GenresView genres;
        public GenresView Geners { get { return genres; } set 
            {
                genres = value;
                BindGenresDropDown();
                BindListBOX();
            } }

        public ListBox SelectedList => testListbox; // test

        public ComboBox Combo => ddlGenres; // test

        public MultiSelectionBox(GenresView genres)
        {
            InitializeComponent();
            Geners = genres;          
        }

        public MultiSelectionBox():this(GenresView.AllGenres)
        {

        }
        private void BindGenresDropDown()
        {
            ddlGenres.ItemsSource = Geners;
        }
        private void Genres_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Genres_TextChanged(object sender, TextChangedEventArgs e)
        {
            ddlGenres.ItemsSource = Geners.Where(x => x.Name.StartsWith(ddlGenres.Text.Trim()));
        }

        private void AllCheckbocx_CheckedAndUnchecked(object sender, RoutedEventArgs e)
        {
            BindListBOX();
        }

        private void BindListBOX()
        {
            testListbox.Items.Clear();
            for (int i = 0; i < Geners.Length; i++)
            {
                if (Geners[i].Active)
                {
                    testListbox.Items.Add(Geners[i].ToString());
                }
            }
        }

        public List<ViewGenres> GetGenres()
        {
            List<ViewGenres> genres = new List<ViewGenres>();
            for (int i = 0; i < Geners.Length; i++)
            {
                if (Geners[i].Active)
                    genres.Add((ViewGenres)Geners[i].Id);
            }

            return genres;
        }

    }

}
