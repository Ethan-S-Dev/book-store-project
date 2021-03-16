using System.Collections.Generic;
using BookStore.DAL.Models;
using BookStore.ViewModel.Models;
using BookStore.ViewModel.Controls;
using BookStore.BLL.util;

namespace BookStore.BLL.Convert
{
    /// <summary>
    /// Converter for ViewModel to DAL models and back
    /// </summary>
    public static class ViewConverter 
    {
        public static StoreItemControl CreateStoreItemControl(StoreItem book)
        {
            return new StoreItemControl(CreateStoreItemView(book));
        }
        public static StoreItemView CreateStoreItemView(StoreItem item)
        {
            Book bok = item as Book;
            if (bok != null)
            {
                List<ViewGenres> genres = new List<ViewGenres>();
                foreach (var it in bok.Genres)
                {
                    genres.Add((ViewGenres)(int)it);
                }
                return new BookView(bok.Id, bok.Name, bok.Edition, bok.CatalogNumber, bok.Summary, bok.UnitPrice, bok.Discount, bok.ISBN, bok.UnitsInStock, bok.Author, bok.Publisher, genres, bok.PublishedDate, bok.DisplayImage);
            }

            Journal jur = item as Journal;
            if (jur != null)
            {
                return new JournalView(jur.Id, jur.Name, jur.UnitPrice, jur.Discount, jur.ISBN, jur.UnitsInStock, jur.Field, jur.VolumeNumber, jur.IssueNumber, jur.PublishedDate, jur.DisplayImage);
            }

            throw new ViewNotExistException(item.GetType());
        }
        public static Book CreateBook(BookView book)
        {
            List<Genres> gen = new List<Genres>();
            for (int i = 0; i < book.Genres.Length; i++)
            {
                if (book.Genres[i].Active)
                    gen.Add((Genres)book.Genres[i].Id);
            }

            return new Book(book.Name, book.Edition, book.UnitPrice, book.Discount, book.CatalogNumber, book.ISBN, book.UnitsInStock, book.Author, book.Publisher, gen, book.PublishedDate, book.Summary, book.DisplayImage, book.Id);
        }
        public static Journal CreateJournal(JournalView journal)
        {
            return new Journal(journal.Name, journal.UnitPrice, journal.Discount, journal.ISBN, journal.UnitsInStock, journal.VolumeNumber, journal.Field, journal.PublishedDate, journal.IssueNumber, journal.DisplayImage, journal.Id);
        }
        public static WorkerView CreateWorkerView(Worker worker)
        {
            return new WorkerView(worker.Id, worker.FirstName, worker.LastName, worker.Username, (WorkerView.Rank)(int)worker.WorkerRank, worker.ProfileImage);
        }
        public static Worker CreateWorker(WorkerView worker)
        {
            return new Worker(worker.FirstName, worker.LastName, null, worker.Username, (Worker.Rank)(int)worker.WorkerRank, worker.ProfileImage, worker.Id);
        }
        public static WorkerLabel CreateWorkerLabel(WorkerView work)
        {
            return new WorkerLabel(work);
        }
        public static WorkerLabel CreateWorkerLabel(Worker work)
        {
            return new WorkerLabel(CreateWorkerView(work));
        }
    }
}
