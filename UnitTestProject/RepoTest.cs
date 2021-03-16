using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using System.Linq;
using BookStore.DAL.Repositorys;
using BookStore.DAL.Models;
using System.Diagnostics;

namespace UnitTestProject
{
    [TestClass]
    public class RepoTest
    {
        Book book1 = new Book("The Alchemyst: The secrets of the immortal Nicholas Flamel", "First",234, 0.1f,1 ,"978-965-07-1767-4", 213, "Michael Scott", "Keter", new List<Genres> { Genres.Adventure, Genres.Fantasy }, new DateTime(2009, 1, 1),"asdasdasd\nasdasd\nasdasd",null);
        Book book2 = new Book("Diary of a wimpy kid: Dog days","First", 234, 0.1f,23 ,"978-965-517-765-7", 213, "Jeff Kinney", "Kinneret", new List<Genres> { Genres.Comic_Book }, new DateTime(2010, 1, 1), "asdasdasd\nasdasd\nasdasd", null);
        Book book3 = new Book("Being There", "asds",234, 0.1f, 4,"978-965-234-765-7", 213, "Jerzy Kosinski", "Moden", new List<Genres> { Genres.Biographies }, new DateTime(1970, 1, 1), "asdasdasd\nasdasd\nasdasd", null);

        [TestMethod]       
        public void TestAddItemToStoreRepo()
        {
            StoreItemRepository storeRepos = new StoreItemRepository();
            storeRepos.Open();
            storeRepos.Add(book1);
            storeRepos.Add(book2);    
            storeRepos.Close();
            storeRepos.Open();
            storeRepos.Add(book3);
        }

        [TestMethod]
        public void TestGetItemList()
        {
            StoreItemRepository storeRepos = new StoreItemRepository();
            List<StoreItem> list;
            storeRepos.Open();
            list = storeRepos.GetAll().ToList();
            for (int i = 0; i < list.Count; i++)
            {
                Debug.WriteLine(list[i].Name);
            }
            Debug.WriteLine(list.Count);
            storeRepos.Close();
        }

        [TestMethod]
        public void TestRemoveItem()
        {
            StoreItemRepository storeRepos = new StoreItemRepository();
            List<StoreItem> list;
            IEnumerable<StoreItem> outItems;
            storeRepos.Open();
            list = storeRepos.GetAll().ToList();
            for (int i = 0; i < list.Count; i++)
            {
                Debug.WriteLine(list[i].Name);
            }
            Debug.WriteLine(list.Count);

            storeRepos.RemoveBy((i) => i.Name.ToLower().Contains("The"),out outItems);

            foreach (var item in outItems)
            {
                Debug.WriteLine(item.Name);
            }

            Debug.WriteLine(list.Count);

            storeRepos.Close();
        }
    }
}
