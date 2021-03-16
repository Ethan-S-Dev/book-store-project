using System;

namespace BookStore.BLL.util
{
    public class UserLoggedInException : Exception
    {

        public UserLoggedInException() : base("There is no user logged in to the system.")
        {

        }
    }
    public class UserAccessException : Exception
    {
        public UserAccessException(string message) : base(message)
        {

        }

        public UserAccessException()
        {

        }
    }
    public class ViewNotExistException : Exception
    {
        public ViewNotExistException(Type itemtype) : base($"View doesnt exist for Type {itemtype.Name}.")
        {

        }
    }
    public class UpdatedItemNotMachingException : Exception
    {
        public UpdatedItemNotMachingException(Type updated,Type item) : base($"The Item is of type: {item.Name}, And you gave me a {updated.Name}.")
        {

        }
    }
    public class PrimeryKeyAllReadyExistException : Exception
    {
    }
    public class WorkerUpdateFailedException : Exception
    {

    }
    public class WhatException : Exception
    {

    }

}
