using System;
using System.Collections.Generic;
using System.IO;
using BookStore.DAL.Interfaces;
using System.Runtime.Serialization.Formatters.Binary;

namespace BookStore.DAL.Repositorys
{
    public abstract class Repository<T> : IRepository<T> where T : IIdable<T>
    {
        public event EventHandler<RepositoryClosedEventArgs> Closed;
        public event EventHandler<RepositoryOpenedEventArgs> Opened;
        public event EventHandler<RepositorySavedEventArgs> Saved;
        public event EventHandler<RepositoryCreatedEventArgs<T>> Created;
        
        public Repository(string fileName, string path)
        {
            this.fileName = fileName;
            this.path = path;
            Directory.CreateDirectory(path);
        }

        public string Path { get { return path; } }
        public string FileName { get { return fileName; } }
        public string FullPath { get { return $"{Path}/{FileName}"; } }
        public virtual bool IsOpen => table != null;

        public virtual void Add(T item)
        {
            if (IsOpen)
            {
                foreach (var it in table)
                {
                    if (it.ComperPrimeryKey(item))
                        throw new PrimeryKeyAllReadyExistException();
                }
                item.Id = nextId;
                nextId++;
                table.Add(item);
            }
            else
                throw new RepositoryCloesedException(FullPath);
        }
        public virtual void Update(T item)
        {
            if (IsOpen)
            {
                foreach (var it in table)
                {
                    if (it.ComperPrimeryKey(item))
                    {

                    }
                        

                        throw new PrimeryKeyAllReadyExistException();
                }               
            }
            else
                throw new RepositoryCloesedException(FullPath);
        }
        public virtual IEnumerable<T> GetAll()
        {
            if (IsOpen)
            {
                return table;
            }
            else
                throw new RepositoryCloesedException(FullPath);
        }
        public virtual void Open()
        {
            if (!IsOpen)
            {
                if (File.Exists(FullPath))
                {
                    try
                    {
                        using (Stream repStream = new FileStream(FullPath, FileMode.Open))
                        {
                            var formatter = new BinaryFormatter();
                            table = formatter.Deserialize(repStream) as List<T>;
                        }
                    }catch(Exception e)
                    {
                        throw new RepositoryOpenFailedExceptiom($"Failed to open {fileName}",e);
                    }
                    

                    int bigestId = 0;

                    foreach (var item in table)
                    {
                        if (item.Id > bigestId)
                            bigestId = item.Id;
                    }

                    nextId = bigestId + 1;
                }
                else
                {
                    nextId = 1;
                    table = new List<T>();
                    Created?.Invoke(this, new RepositoryCreatedEventArgs<T>(table));
                    using (Stream repStream = new FileStream(FullPath, FileMode.Create))
                    {
                        var formatter = new BinaryFormatter();
                        formatter.Serialize(repStream, table);
                    }
                }

                Opened?.Invoke(this, new RepositoryOpenedEventArgs());
            }

        }
        public virtual void Save()
        {
            if (File.Exists(FullPath))
            {
                try
                {
                    using (Stream repStream = new FileStream(FullPath, FileMode.Open))
                    {
                        var formatter = new BinaryFormatter();
                        formatter.Serialize(repStream, table);
                    }
                }catch(Exception e)
                {
                    throw new RepositorySaveFailedExceptiom($"Field to save to {fileName}", e);
                }
                
                Saved?.Invoke(this, new RepositorySavedEventArgs());
            }
        }
        public virtual void Close()
        {
            if (IsOpen)
            {
                Save();
                table = null;
                Closed?.Invoke(this, new RepositoryClosedEventArgs());
            }
        }
        public virtual IEnumerable<T> GetBy(Predicate<T> func)
        {
            if (IsOpen)
            {
                LinkedList<T> list = new LinkedList<T>();
                foreach (var item in table)
                {
                    if (func(item))
                        list.AddLast(item);
                }

                return list;
            }
            else
                throw new RepositoryCloesedException(FullPath);
        }
        public virtual T GetById(int id)
        {
            if (IsOpen)
            {
                foreach (var item in table)
                {
                    if (item.Id == id)
                        return item;
                }

                throw new ItemNotFoundException(id);
            }
            else
                throw new RepositoryCloesedException(FullPath);
        }
        public virtual bool FindBy(Predicate<T> func)
        {
            if (IsOpen)
            {
                foreach (var i in table)
                {
                    if (func(i))
                        return true;
                }
                return false;
            }
            else
                throw new RepositoryCloesedException(FullPath);
        }
        public virtual bool Remove(T item)
        {
            if (IsOpen)
            {
                return (table.RemoveAll((other) => item.ComperPrimeryKey(other)) > 0);
            }
            else
                throw new RepositoryCloesedException(FullPath);
        }
        public virtual bool RemoveById(int id, out T itemRemoved)
        {
            if (IsOpen)
            {
                itemRemoved = default(T);
                foreach (var item in table)
                {
                    if (item.Id == id)
                    {
                        itemRemoved = item;
                        table.Remove(item);
                        return true;
                    }
                }
                return false;
            }
            else
                throw new RepositoryCloesedException(FullPath);
        }
        public virtual bool RemoveBy(Predicate<T> func, out IEnumerable<T> items)
        {
            if (IsOpen)
            {


                LinkedList<T> removedList = new LinkedList<T>();
                bool removed = false;
                foreach (var item in table)
                {
                    if (func(item))
                    {
                        removedList.AddLast(item);
                        table.Remove(item);
                        removed = true;
                    }
                }

                items = removedList;
                return removed;
            }
            else
                throw new RepositoryCloesedException(FullPath);
        }
        public bool GetFirstBy(Predicate<T> func, out T item)
        {
            if (IsOpen)
            {
                item = default;
                foreach (var i in table)
                {
                    if (func(i))
                    {
                        item = i;
                        return true;
                    }
                }
                return false;
            }
            else
                throw new RepositoryCloesedException(FullPath);
        }

        protected List<T> table;
        protected int nextId;
        
        private string path;
        private string fileName;
        ~Repository()
        {
            if (IsOpen)
            {
                try
                {
                    Close();
                }catch
                {
                    return;
                }
            }
        }
    }
}
