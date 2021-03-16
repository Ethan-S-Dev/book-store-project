using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Imaging;

namespace BookStore.ViewModel.Models
{
    public class WorkerView
    {
        public int Id{ get;}
        public string FirstName { get;}
        public string LastName { get;}
        public string Username { get;}
        public Rank WorkerRank { get;}
        public byte[] ProfileImage { get; }

        public WorkerView(int id,string firstName,string lastName,string userName,Rank rank,byte[] img)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Username = userName;
            WorkerRank = rank;
            ProfileImage = img;
        }
        public BitmapImage GetUserImage()
        {
            BitmapImage img;
            if (ProfileImage != null)
            {
                using (var ms = new System.IO.MemoryStream(ProfileImage))
                {
                    img = new BitmapImage();
                    img.BeginInit();
                    img.CacheOption = BitmapCacheOption.OnLoad; // here
                    img.StreamSource = ms;
                    img.EndInit();
                }
            }
            else
            {
                img = AssetPool.Instance.Default_UserImg;
            }
            return img;
        }
        public bool Equals(WorkerView other)
        {
            if (other == null) return false;
            if (other.Username.ToLower() != Username.ToLower()) return false;
            if (other.Id != Id) return false;
            return true;
        }
        public enum Rank
        {
            Manager,
            Worker
        }
    }

    
}
