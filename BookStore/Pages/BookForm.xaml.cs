using BookStore.BLL;
using BookStore.BLL.util;
using BookStore.ViewModel.Models;
using System;
using System.Text.RegularExpressions;
using BookStore.Client.res.UserControls;
using System.Windows.Controls;
using System.Windows.Documents;

namespace BookStore.Client.Pages
{
    /// <summary>
    /// Interaction logic for StoreItemForm.xaml
    /// </summary>
    public partial class BookForm : Page
    {
        private bool CreateNew;

        private BookView item;

        private MultiSelectionBox genresField;

        public BookForm(BookView book) : this()
        {
            CreateNew = false;

            item = book;

            nameField.Value = book.Name;

            isbnField.Value = book.ISBN;

            priceField.Value = book.UnitPrice.ToString();

            int disc = (int)(book.Discount * 100);

            discountField.Value = $"{disc}%";

            authorField.Value = book.Author;

            publisherField.Value = book.Publisher;

            if (book.CatalogNumber > 0)
                cataNumField.Value = book.CatalogNumber.ToString();

            editionField.Value = book.Edition;


            imageField.Image = book.GetDisplayImage();
            imageField.ImageBytes = book.DisplayImage;


            unitInStockField.NumValue = book.UnitsInStock;

            dateField.SelectedDate = book.PublishedDate;

            genresField = new res.UserControls.MultiSelectionBox(book.Genres);
            genresField.Margin = new System.Windows.Thickness(5, 0, 0, 0);
            genresField.VerticalAlignment = System.Windows.VerticalAlignment.Center;

            generBox.Child = genresField;

            FlowDocument mcFlowDoc = new FlowDocument();
            Paragraph para = new Paragraph();
            para.Inlines.Add(new Run(book.Summary));
            mcFlowDoc.Blocks.Add(para);
            summaryField.Document = mcFlowDoc;

            submitBtnTxt.Text = "Update";
            removeBtn.Visibility = System.Windows.Visibility.Visible;


        }
        public BookForm()
        {
            const int boxSize = 150;
            CreateNew = true;
            InitializeComponent();


            genresField = new res.UserControls.MultiSelectionBox();
            genresField.Margin = new System.Windows.Thickness(5, 0, 0, 0);
            genresField.VerticalAlignment = System.Windows.VerticalAlignment.Center;

            generBox.Child = genresField;

            removeBtn.Visibility = System.Windows.Visibility.Collapsed;

            nameField.Text = "Name:";
            nameField.BoxWidth = boxSize;

            isbnField.Text = "ISBN:";
            isbnField.BoxWidth = boxSize;
            isbnField.PreviewInput += IsbnField_PreviewInput;

            priceField.Text = "Price:";
            priceField.BoxWidth = boxSize;
            priceField.PreviewInput += PriceField_PreviewInput;

            discountField.Text = "Discount:";
            discountField.BoxWidth = boxSize;
            discountField.PreviewInput += DiscountField_PreviewInput;
            discountField.InputLostFocus += DiscountField_InputLostFocus;

            authorField.Text = "Author:";
            authorField.BoxWidth = boxSize;

            publisherField.Text = "Publisher:";
            publisherField.BoxWidth = boxSize;

            cataNumField.Text = "Catalog Number:";
            cataNumField.BoxWidth = 40;
            cataNumField.PreviewInput += CataNumField_TextInput;

            editionField.Text = "Edition";
            editionField.BoxWidth = boxSize;
        }

        private void CleanBook()
        {
            nameField.Value = "";
            priceField.Value = "";
            discountField.Value = "";
            isbnField.Value = "";
            unitInStockField.NumValue = 0;
            imageField.ImageBytes = null;
            imageField.Image = null;
            dateField.SelectedDate = null;


            authorField.Value = "";
            publisherField.Value = "";
            editionField.Value = "";
            cataNumField.Value = "";
            summaryField.Document = new FlowDocument();
            generBox.Child = new MultiSelectionBox();

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

        private void CataNumField_TextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
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
                    BookView bonk;
                    if (BuildBook(out bonk))
                    {
                        Logic.Instance.AddStoreItem(bonk);
                        CleanBook();
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
                    errorBox.Pop("ISBN All Ready In Use.");
                }
                catch (Exception ex)
                {
                    Logger.Exception(ex);
                    Logger.Fetal("Unknown Exception on Book Submitting.");
                    Logic.Instance.Shutdown(1);
                }
            }
            else
            {
                if (item == null)
                {
                    errorBox.Pop("Item Was All Ready Deleted.");
                    return;
                }

                try
                {
                    BookView bonk;
                    if (BuildBook(out bonk))
                        Logic.Instance.UpdateStoreItem(bonk, item);
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

        private bool BuildBook(out BookView book)
        {
            book = null;

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
            if (publishedDate > DateTime.Now) { errorBox.Pop("Invalid Date."); return false; }

            string author = authorField.Value;
            if (author == "" || author == null) { errorBox.Pop("Invalid Author."); return false; }

            string publisher = publisherField.Value;
            if (publisher == "" || publisher == null) { errorBox.Pop("Invalid Publisher."); return false; }

            GenresView genres = genresField.Geners;
            if (genres.Count == 0) { errorBox.Pop("Must Choose at Least One Genre"); return false; }

            string summary = null;
            TextRange text = new TextRange(summaryField.Document.ContentStart, summaryField.Document.ContentEnd);
            if (text.Text != "")
                summary = text.Text;

            int cataNum = -1;
            if (cataNumField.Value != null && cataNumField.Value != "")
                if (!int.TryParse(cataNumField.Value, out cataNum)) { errorBox.Pop("Invalid Catalog Num."); return false; }

            string edition = editionField.Value;

            book = new BookView(0, name, edition, cataNum, summary, price, discount, isbn, uIS, author, publisher, genres, publishedDate.GetValueOrDefault(), image);

            return true;
        }

        private void removeBtn_Click(object sender, System.Windows.RoutedEventArgs e)
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
