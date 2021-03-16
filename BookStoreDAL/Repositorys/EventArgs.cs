using System;
using System.Collections.Generic;

namespace BookStore.DAL.Repositorys
{
    public class RepositoryClosedEventArgs : EventArgs
    {
        public DateTime ClosedDateTime { get;private set; }

        public RepositoryClosedEventArgs()
        {
            ClosedDateTime = DateTime.Now;
        }
    }
    public class RepositoryOpenedEventArgs : EventArgs
    {
        public DateTime OpenedDateTime { get; private set; }       

        public RepositoryOpenedEventArgs()
        {
            OpenedDateTime = DateTime.Now;
        }
    }
    public class RepositorySavedEventArgs : EventArgs
    {
        public DateTime SavedDateTime { get; private set; }
        public RepositorySavedEventArgs()
        {
            SavedDateTime = DateTime.Now;
        }
    }
    public class RepositoryCreatedEventArgs<T> : EventArgs
    {
        public DateTime SavedDateTime { get; }
        public List<T> CreatedTable; 
        public RepositoryCreatedEventArgs(List<T> table)
        {
            SavedDateTime = DateTime.Now;
            CreatedTable = table;
        }
    }
}
