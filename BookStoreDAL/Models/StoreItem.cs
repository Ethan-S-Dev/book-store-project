using System;
using BookStore.DAL.Interfaces;

namespace BookStore.DAL.Models
{
    /// <summary>
    /// Base class for Inventory items
    /// </summary>
    [Serializable]
    public abstract class StoreItem : IIdable<StoreItem>
    {
        public StoreItem(int id,string name,decimal unitPrice,float discount,string isbn,DateTime publishedDate,int unitsInStock,byte[] image)
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
        public int Id { get { return id; } set 
            { 
                if (id != 0) return;
                id = value;
            } 
        }
        public string Name { get; set; }
        public decimal UnitPrice { get; set; }
        public float Discount { get; set; }

        /// <summary>
        /// ISBN or ISSN
        /// </summary>
        public string ISBN { get; set; }      
        public int UnitsInStock { get; set; }
        public byte[] DisplayImage { get; set; }
        public DateTime PublishedDate { get; set; }
        public bool ComperPrimeryKey(StoreItem other)
        {
            return (this.ISBN.Equals(other.ISBN));
        }

        private int id = 0;        
    }
}
