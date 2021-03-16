using BookStore.BLL.Convert;
using BookStore.DAL.Models;
using BookStore.ViewModel.Models;
using System;

namespace BookStore.BLL
{
    public class UserLoggedInEventArgs : EventArgs
    {
        public WorkerView Worker { get; }
        public DateTime LoggedInTime { get; }
        public bool IsManager { get; }
        public UserLoggedInEventArgs(Worker work)
        {
            Worker = ViewConverter.CreateWorkerView(work);
            IsManager = (Worker.WorkerRank == ViewModel.Models.WorkerView.Rank.Manager);
            LoggedInTime = DateTime.Now;
        }
    }

}
