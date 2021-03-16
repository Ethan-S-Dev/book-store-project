using System;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BookStore.ViewModel.Models;

namespace BookStore.ViewModel.Controls
{
    /// <summary>
    /// Interaction logic for StoreItemControl.xaml
    /// </summary>
    public partial class StoreItemControl : UserControl
    {
        public event EventHandler Click;
        public StoreItemView ItemView => item;
        private bool pressed;
        public double LableWidth => lableGrid.ActualWidth;
        public StoreItemControl(StoreItemView item)
        {                       
            InitializeComponent();
            Click += StoreItemControl_Click;
            BookImag.Source = item.GetDisplayImage();
            ItemName.Text = item.Name;
            ItemISBN.ToolTip = item.ISBN;
            priceBlock.Text = item.UnitPrice.ToString("C",CultureInfo.CurrentCulture);
            unitsBlock.Text = item.UnitsInStock.ToString();
            discBlock.Text = item.Discount.ToString();
            bookFields.Visibility = Visibility.Collapsed;
            journalFields.Visibility = Visibility.Collapsed;
            nameBlock.Text = item.Name;
            this.item = item;

            BookView book = item as BookView;
            if(book != null)
            {
                typeBlock.Text = "Book";
                bookFields.Visibility = Visibility.Visible;
                summBlock.Text = book.Summary;
                authorBlock.Text = book.Author;
                publishBlock.Text = book.Publisher;
                dateBlock.Text = book.PublishedDate.ToString("dd/MM/yyyy");
                if(book.CatalogNumber > 0)
                    cataBlock.Text = book.CatalogNumber.ToString();
                editionBlock.Text = book.Edition;
                StringBuilder st = new StringBuilder();
                for (int i = 0; i < book.Genres.Length; i++)
                {
                    if (book.Genres[i].Active)
                        st.Append($"{book.Genres[i].GenreToString}\n");
                }

                genresBlock.Text = st.ToString();
                return;
            }

            JournalView journal = item as JournalView;
            if(journal != null)
            {

                typeBlock.Text = "Journal";
                journalFields.Visibility = Visibility.Visible;

                volumBlock.Text = journal.VolumeNumber.ToString();
                issueBlock.Text = journal.IssueNumber.ToString();
                fielBlock.Text = journal.Field;
            }

        }

        private void StoreItemControl_Click(object sender, EventArgs e)
        {
            if (popInfo.IsOpen)
                popInfo.IsOpen = false;
            else
                popInfo.IsOpen = true;
        }
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            pressed = true;
        }
        private void Border_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if(pressed)
            {
                pressed = false;
                Click?.Invoke(this, new EventArgs());
            }
        }
        
        StoreItemView item;
    }
}
