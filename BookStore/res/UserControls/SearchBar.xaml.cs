using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BookStore.Client.res.UserControls
{
    /// <summary>
    /// Interaction logic for SerchBar.xaml
    /// </summary>
    public partial class SearchBar : UserControl
    {
        public delegate void SearchEventHandler(SearchBar sender, SearchEventArgs args);
        public SearchBar()
        {
            InitializeComponent();
        }

        public event SearchEventHandler Searched;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (searchBox.Text == null)
                Searched?.Invoke(this, new SearchEventArgs(string.Empty));
            else
                Searched?.Invoke(this, new SearchEventArgs(searchBox.Text));
        }

        private void searchBox_KeyDown(object sender, KeyEventArgs e)
        {
            TextBox tb = sender as TextBox;

            if (tb.IsFocused)
            {              
                if (e.Key == Key.Enter)
                {
                    if (searchBox.Text == null)                   
                        Searched?.Invoke(this, new SearchEventArgs(string.Empty));
                    else
                        Searched?.Invoke(this, new SearchEventArgs(searchBox.Text));
                    btn.Focus();
                }
            }
        }
    }

    public class SearchEventArgs : EventArgs
    {
        public string SearchedText { get; }
        public SearchEventArgs(string searchText)
        {
            SearchedText = searchText;
        }
    }
}
