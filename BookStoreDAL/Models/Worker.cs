using System;
using BookStore.DAL.Interfaces;

namespace BookStore.DAL.Models
{
    /// <summary>
    /// DAL model for the workers
    /// </summary>
    [Serializable]
    public class Worker : IIdable<Worker>
    {
        public enum Rank
        {
            Manager,
            Worker
        }
        public Worker(string firstName,string lastName, byte[] password,string userName,Rank rank,byte[] img = null,int id = 0)
        {
            FirstName = firstName;
            LastName = lastName;
            Password = password;
            Username = userName;
            WorkerRank = rank;
            ProfileImage = img;
        } 
        public int Id { get { return id; } set {
                if (id != 0) return;
                id = value;
            } }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public byte[] Password { get; set; }
        public string Username { get; set; }
        public byte[] ProfileImage { get; set; }
        public Rank WorkerRank { get; set; }
        
        public bool ComperPrimeryKey(Worker other)
        {
            return this.Username.ToLower().Equals(other.Username.ToLower());
        }

        private int id = 0;
    }    
}
