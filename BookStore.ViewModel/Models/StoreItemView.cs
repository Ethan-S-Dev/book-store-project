using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace BookStore.ViewModel.Models
{
    public abstract class StoreItemView
    {
        public int Id { get; }       
        public string Name { get; }
        public decimal UnitPrice { get; }
        public float Discount { get;  }
        public string ISBN { get; }
        public int UnitsInStock { get; }
        public byte[] DisplayImage { get; }
        public DateTime PublishedDate { get; }

        public StoreItemView(int id, string name, decimal unitPrice, float discount, string isbn, int unitsInStock,DateTime publishedDate, byte[] image)
        {
            Id = id;
            Name = name;
            UnitPrice = unitPrice;
            Discount = discount;
            ISBN = isbn;
            UnitsInStock = unitsInStock;
            DisplayImage = image;
            PublishedDate = publishedDate;
        }
        public BitmapImage GetDisplayImage()
        {
            BitmapImage img;
            if (DisplayImage != null)
            {
                using (var ms = new MemoryStream(DisplayImage))
                {
                    img = new BitmapImage();
                    img.BeginInit();
                    img.CacheOption = BitmapCacheOption.OnLoad; // here
                    img.StreamSource = ms;
                    img.EndInit();                   
                }
            }
            else
            {
                img = AssetPool.Instance.Default_BookImg;
            }
            return img;
        }
    }
}
