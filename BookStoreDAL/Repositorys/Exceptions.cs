using System;

namespace BookStore.DAL.Repositorys
{
    public class ItemNotFoundException : Exception
    {
        public ItemNotFoundException(int id) : base($"Item with the Id: {id} not found.")
        {

        }

    }
    public class RepositoryCloesedException : Exception
    {
        public RepositoryCloesedException(string filePath) : base($"The repository in {filePath} is closed.")
        {

        }
    }
    public class OrderIdNotFoundException : Exception
    {
        public OrderIdNotFoundException(string orderRepoName) : base($"The OrderId of the item doesn't much any order in the repository {orderRepoName}")
        {

        }
    }
    public class PrimeryKeyAllReadyExistException: Exception
    {
    }
    public class RepositoryOpenFailedExceptiom : Exception
    {
        public RepositoryOpenFailedExceptiom(string message,Exception inner) : base(message,inner)
        {

        }
    }
    public class RepositorySaveFailedExceptiom : Exception
    {
        public RepositorySaveFailedExceptiom(string message, Exception inner) : base(message, inner)
        {

        }
    }
}
