using System;
using System.Collections.Generic;

namespace BookStore.DAL.Models
{
    /// <summary>
    /// DAL Book model drives from StoreItem
    /// </summary>
    [Serializable]
    public class Book : StoreItem
    {   
        public Book(string name, string edition, decimal unitPrice, float discount,int catalogNumber, string isbn, int unitsInStock,string author,string publisher,List<Genres> genres,DateTime publishedDate ,string summary,byte[] image, int id=0) : base(id,name, unitPrice, discount, isbn, publishedDate, unitsInStock,image)
        {
            Author = author;
            Publisher = publisher;
            Genres = genres;
            PublishedDate = publishedDate;
            Summary = summary;
            Edition = edition;
            CatalogNumber = catalogNumber;
        }
        public string Author { get; set; }
        public string Publisher { get; set; }
        public List<Genres> Genres { get; set; }
        public string Summary { get; set; }
        public int CatalogNumber { get; set; }
        public string Edition { get; set; }
    }
}
