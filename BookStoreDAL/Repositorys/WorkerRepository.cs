using System.Security.Cryptography;
using System.Text;
using BookStore.DAL.Models;

namespace BookStore.DAL.Repositorys
{
    public class WorkerRepository : Repository<Worker>
    {
        public WorkerRepository(string fileName,string path) : base(fileName,path)
        {
            Created += WorkerRepository_Created;
        }
        public WorkerRepository() : this("WorkersRepo.data", "./Data/Repos")
        {

        }
        public bool ComperPass(byte[] pass1,byte[] pass2)
        {
            if (pass1.Length != pass2.Length) return false;
            for (int i = 0; i < pass1.Length; i++)
            {
                if (pass1[i] != pass2[i])
                    return false;
            }
            return true;
        } 

        private void WorkerRepository_Created(object sender, RepositoryCreatedEventArgs<Worker> args)
        {
            string pass = "123";
            byte[] hashed = SHA256.Create().ComputeHash(Encoding.Unicode.GetBytes(pass));
            byte[] twoXHashed = SHA256.Create().ComputeHash(hashed);
            Add(new Worker("Admin", "User", twoXHashed, "Admin", Worker.Rank.Manager));
        }

        public bool UpdateWorker(int id,Worker updated)
        {
            Worker worker = GetById(id);

            if(id == worker.Id)
            {
                if(worker.Username != updated.Username)
                {
                    foreach (var item in table)
                    {
                        if (item.ComperPrimeryKey(updated))
                            throw new PrimeryKeyAllReadyExistException();
                    }

                    worker.Username = updated.Username;
                }

                worker.FirstName = updated.FirstName;
                worker.LastName = updated.LastName;
                worker.ProfileImage = updated.ProfileImage;
                worker.WorkerRank = updated.WorkerRank;

                return true;
            }

            return false;
        }

        public bool UpdatePass(int userId, byte[] oldPass, byte[] newPass)
        {
            Worker user = GetById(userId);
            if(ComperPass(oldPass, user.Password))
            {
                user.Password = newPass;
                return true;
            }

            return false;
        }
    }
}
