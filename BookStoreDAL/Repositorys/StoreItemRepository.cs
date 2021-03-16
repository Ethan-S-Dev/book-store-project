using System;
using System.Collections.Generic;
using BookStore.DAL.Models;

namespace BookStore.DAL.Repositorys
{
    public class StoreItemRepository : Repository<StoreItem>
    {
        public StoreItemRepository(string fileName,string path):base(fileName,path)
        {
            
        }    
        public StoreItemRepository() : this("StoreItemRepo.data", "./Data/Repos")
        {

        }
        public IEnumerable<StoreItem> GetBy(params Predicate<StoreItem>[] func)
        {
            if (IsOpen)
            {
                LinkedList<StoreItem> list = new LinkedList<StoreItem>();
                foreach (var item in table)
                {
                    bool fit = true;
                    foreach (Predicate<StoreItem> predicate in func)
                    {
                        if (!predicate(item))
                        {
                            fit = false;
                            break;
                        }
                    }

                    if (fit)
                        list.AddLast(item);
                }

                return list;
            }
            else
                throw new RepositoryCloesedException(FullPath);
        }
        public IEnumerable<Book> GetAllBooks()
        {
            LinkedList<Book> books = new LinkedList<Book>();
            foreach(var item in table)
            {
                if(item is Book)
                {
                    books.AddLast((Book)item);
                }
            }

            return books;
        }
        public IEnumerable<Journal> GetAllJurnals()
        {
            LinkedList<Journal> jurnals = new LinkedList<Journal>();
            foreach (var item in table)
            {
                if (item is Journal)
                {
                    jurnals.AddLast((Journal)item);
                }
            }

            return jurnals;
        }
        public void UpdateBook(Book updated,Book toUpdate)
        {
            if (updated.ISBN != toUpdate.ISBN)
            {
                foreach (var item in table)
                {
                    if (item.ComperPrimeryKey(updated))
                        throw new PrimeryKeyAllReadyExistException();

                }
                toUpdate.ISBN = updated.ISBN;
            }

            toUpdate.Name = updated.Name;
            toUpdate.PublishedDate = updated.PublishedDate;
            toUpdate.Publisher = updated.Publisher;
            toUpdate.Summary = updated.Summary;
            toUpdate.UnitPrice = updated.UnitPrice;
            toUpdate.UnitsInStock = updated.UnitsInStock;
            toUpdate.Genres = updated.Genres;
            toUpdate.Edition = updated.Edition;
            toUpdate.DisplayImage = updated.DisplayImage;
            toUpdate.Discount = updated.Discount;
            toUpdate.CatalogNumber = updated.CatalogNumber;
            toUpdate.Author = updated.Author;
        }
        public void UpdateJournal(Journal updated, Journal toUpdate)
        {
            if (updated.ISBN != toUpdate.ISBN)
            {
                foreach (var item in table)
                {
                    if (item.ComperPrimeryKey(updated))
                        throw new PrimeryKeyAllReadyExistException();

                }
                toUpdate.ISBN = updated.ISBN;
            }

            toUpdate.Name = updated.Name;
            toUpdate.PublishedDate = updated.PublishedDate;
            toUpdate.UnitPrice = updated.UnitPrice;
            toUpdate.UnitsInStock = updated.UnitsInStock;
            toUpdate.DisplayImage = updated.DisplayImage;
            toUpdate.Discount = updated.Discount;

            toUpdate.Field = updated.Field;
            toUpdate.IssueNumber = updated.IssueNumber;
            toUpdate.VolumeNumber = updated.VolumeNumber;
        }
    }
}
