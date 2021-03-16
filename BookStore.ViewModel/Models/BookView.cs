using System;
using System.Collections.Generic;

namespace BookStore.ViewModel.Models
{
    public class BookView : StoreItemView
    {
        public string Author { get; }
        public string Publisher { get; }
        public GenresView Genres { get;}       
        public string Edition { get;}
        public string Summary { get;}
        public int CatalogNumber { get;}

        public BookView(int id, string name,string edition,int catalogNumber, string summary, decimal unitPrice, float discount, string isbn, int unitsInStock, string author, string publisher, List<ViewGenres> genres, DateTime publishedDate, byte[] image = null) : base(id, name, unitPrice, discount, isbn, unitsInStock, publishedDate, image)
        {
            Author = author;
            Publisher = publisher;
            Genres = new GenresView(genres);
            Edition = edition;
            CatalogNumber = catalogNumber;
            Summary = summary;
        }
        public BookView(int id, string name, string edition, int catalogNumber, string summary, decimal unitPrice, float discount, string isbn, int unitsInStock, string author, string publisher, GenresView genres, DateTime publishedDate, byte[] image = null) : base(id, name, unitPrice, discount, isbn, unitsInStock, publishedDate, image)
        {
            Author = author;
            Publisher = publisher;
            Genres = genres;
            Edition = edition;
            CatalogNumber = catalogNumber;
            Summary = summary;
        }
    }
}
