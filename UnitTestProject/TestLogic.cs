using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using BookStore.BLL;
using BookStore.BLL.Convert;
using System.Security.Cryptography;
using System.Text;
using BookStore.ViewModel.Models;
using BookStore.DAL.Models;
using System.Collections.Generic;
using BookStore.BLL.util;

namespace UnitTestProject
{
    [TestClass]
    public class TestLogic
    {
        Random rnd = new Random();
        Book book1 = new Book("The Alchemyst: The secrets of the immortal Nicholas Flamel", "First", 234, 0.1f, 1, "978-234767-4", 213, "Michael Scott", "Keter", new List<Genres> { Genres.Adventure, Genres.Fantasy }, new DateTime(2009, 1, 1), "asdasdasd\nasdasd\nasdasd", null);
        Book book2 = new Book("Diary of a wimpy kid: Dog days", "First", 234, 0.1f, 23, "978-965235-7", 213, "Jeff Kinney", "Kinneret", new List<Genres> { Genres.Comic_Book }, new DateTime(2010, 1, 1), "asdasdasd\nasdasd\nasdasd", null);
        Book boker;

        public TestLogic()
        {
            boker = new Book("Being There", "asds", 234, 0.1f, 4, RandomISBN(), 213, "Jerzy Kosinski", "Moden", new List<Genres> { Genres.Biographies }, new DateTime(1970, 1, 1), "asdasdasd\nasdasd\nasdasd", null);
        }

        private string RandomISBN()
        {
            string[] nums = { rnd.Next(100, 999).ToString(), rnd.Next(100, 999).ToString(), rnd.Next(100, 999).ToString(), rnd.Next(100, 999).ToString(), rnd.Next(100, 999).ToString() };

            return string.Join("-", nums);
        }

        [TestMethod]
        public void TestLogin()
        {
            string user = "Admin";
            string pass = "123";

            Assert.IsTrue(Logic.Instance.Login(user, SHA256.Create().ComputeHash(Encoding.Unicode.GetBytes(pass))));
        }

        [TestMethod]
        public void TestAddStoreItem()
        {
            TestLogin();


            StoreItemView item = ViewConverter.CreateStoreItemView(book1);
            try
            {
                Logic.Instance.AddStoreItem(item);
            }catch(PrimeryKeyAllReadyExistException e)
            {
                Logger.Exception(e,"Testing add item.");
                Assert.Fail("Created an item with an all ready taken ISBN");
            }           
        }

        [TestMethod]
        public string TestAddWorker(out string password)
        {
            TestLogin(); 
            password = $"password{rnd.Next(10000000)}";

            byte[] pass = SHA256.Create().ComputeHash(Encoding.Unicode.GetBytes(password));

            string uname = $"random{rnd.Next(10000000)}";

            WorkerView worker = new WorkerView(0,"Yooosss","Yolo", uname, WorkerView.Rank.Worker,null);

            try
            {
                Logic.Instance.AddWorker(worker,pass);

            }catch(PrimeryKeyAllReadyExistException e)
            {
                Logger.Exception(e, "Testing AddWorker.");
                Assert.Fail("Created a worker with an all ready taken Username");
            }

            return uname;
        }

       
    }
}
