using System;
using System.Collections.Generic;

namespace BookStore.DAL.Interfaces
{
    /// <summary>
    /// Interface for basic functionality of a Repository.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    interface IRepository<T> 
    {       
        void Add(T item);
        T GetById(int id);
        bool GetFirstBy(Predicate<T> func,out T item);
        IEnumerable<T> GetAll();
        IEnumerable<T> GetBy(Predicate<T> func);
        bool FindBy(Predicate<T> func);
        bool Remove(T item);
        bool RemoveById(int id,out T item);
        bool RemoveBy(Predicate<T> func, out IEnumerable<T> items);
    }
}
