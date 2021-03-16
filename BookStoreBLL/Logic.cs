using BookStore.BLL.Convert;
using BookStore.BLL.util;
using BookStore.DAL.Models;
using BookStore.DAL.Repositorys;
using BookStore.ViewModel.Controls;
using BookStore.ViewModel.Models;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace BookStore.BLL
{
    /// <summary>
    /// Main logic for the book store,
    /// Singleton class.
    /// </summary>
    public sealed class Logic 
    {
        /// <summary>
        /// Happens when the user logged in successfully.
        /// </summary>
        public event EventHandler<UserLoggedInEventArgs> LoggedIn;
        /// <summary>
        /// Happens when the BookStore Closes.
        /// </summary>
        public event EventHandler OnClose;
        /// <summary>
        /// Happens when a new item was add successfully.
        /// </summary>
        public event EventHandler<StoreItemView> ItemAdded;
        /// <summary>
        /// Happens when an item was removed successfully.
        /// </summary>
        public event EventHandler<StoreItemView> ItemRemoved;
        /// <summary>
        /// Happens when an item was updated successfully.
        /// </summary>
        public event EventHandler<StoreItemView> ItemUpdated;
        /// <summary>
        /// Happens when one of the workers where updated.
        /// </summary>
        public event EventHandler<WorkerView> WorkerUpdated;
        /// <summary>
        /// Happens when a new worker is add successfully.
        /// </summary>
        public event EventHandler<WorkerView> WorkerAdded;
        /// <summary>
        /// Happens when a worker is removed successfully.
        /// </summary>
        public event EventHandler<WorkerView> WorkerRemoved;

        /// <summary>
        /// The singleton instance of the Logic class.
        /// </summary>
        public static Logic Instance
        {
            get
            {
                if (instance != null) return instance;
                else
                    instance = new Logic();
                return instance;
            }
        } //Singleton object
        public bool IsUserLoggedIn { get { return currentUser != null; } }
        public WorkerView.Rank? UserRank { get { if (IsUserLoggedIn) return currentUser.WorkerRank; else return null; } }
        public void Shutdown() => Shutdown(0);
        public void Shutdown(int exitcode)
        {
            try
            {
                storeItems.Close();
                if (workers.IsOpen)
                    workers.Close();
            }catch(RepositorySaveFailedExceptiom e)
            {
                log.Exception(e, "Failed on tring to add a new item.");
                log.Fetal("Fetal Error, App shuting down.");
            }
            OnClose?.Invoke(this, new EventArgs());
            log.Log($"Book Store Exited with code: {exitcode} .");
            instance = null;
        }
        public IEnumerable<StoreItemControl> GetStoreItems()
        {
            List<StoreItemControl> list = new List<StoreItemControl>();
            foreach (var item in storeItems.GetAll())
            {
                list.Add(ViewConverter.CreateStoreItemControl(item));
            }
            return list;
        }
        public IEnumerable<StoreItemControl> GetStoreItems(params Predicate<StoreItemView>[] predicates)
        {
            List<StoreItemControl> list = new List<StoreItemControl>();
            foreach (var item in storeItems.GetAll())
            {
                var itemView = ViewConverter.CreateStoreItemView(item);
                bool fined = true;
                foreach (var pred in predicates)
                {
                    if (!pred(itemView))
                    {
                        fined = false;
                        break;
                    }

                }
                if (fined)
                    list.Add(ViewConverter.CreateStoreItemControl(item));
            }
            return list;
        }
        public IEnumerable<WorkerLabel> GetWorkers()
        {
            if (UserRank == WorkerView.Rank.Manager)
            {
                try
                {
                    workers.Open();
                }catch(RepositoryOpenFailedExceptiom e)
                {
                    log.Exception(e, "Failed on tring to get all the users.");
                    log.Fetal("Fetal Error, App shuting down.");
                    Shutdown(1);
                }
                List<WorkerLabel> list = new List<WorkerLabel>();
                foreach (var item in workers.GetAll())
                    list.Add(ViewConverter.CreateWorkerLabel(item));
                try
                {

                workers.Close();
                }catch(RepositorySaveFailedExceptiom e)
                {
                    log.Exception(e, "Failed on tring to get all the users.");
                    log.Fetal("Fetal Error, App shuting down.");
                    Shutdown(1);
                }

                return list;
            }
            else
                throw new UserAccessException("You dont have access to Workes Reposetory.");
        }
        public IEnumerable<WorkerLabel> GetWorkers(params Predicate<WorkerView>[] predicates)
        {
            if (UserRank == WorkerView.Rank.Manager)
            {
                try
                {
                    workers.Open();
                }
                catch (RepositoryOpenFailedExceptiom e)
                {
                    log.Exception(e, "Failed on tring to get users.");
                    log.Fetal("Fetal Error, App shuting down.");
                    Shutdown(1);
                }
                List<WorkerLabel> list = new List<WorkerLabel>();
                foreach (var item in workers.GetAll())
                {
                    var itemView = ViewConverter.CreateWorkerView(item);
                    bool found = false;
                    foreach (var pred in predicates)
                        if (pred(itemView))
                        {
                            found = true;
                            break;
                        }

                    if (found)
                        list.Add(ViewConverter.CreateWorkerLabel(itemView));
                }
                try
                {

                    workers.Close();
                }
                catch (RepositorySaveFailedExceptiom e)
                {
                    log.Exception(e, "Failed on tring to get all the users.");
                    log.Fetal("Fetal Error, App shuting down.");
                    Shutdown(1);
                }
                return list;
            }
            else
                throw new UserAccessException("You dont have access to Workes Reposetory.");

        }
        public void AddStoreItem(StoreItemView item)
        {
            if (!IsUserLoggedIn) throw new UserLoggedInException();

            if (currentUser.WorkerRank != WorkerView.Rank.Manager) throw new UserAccessException($"{WorkerView.Rank.Manager} Access needed for that action, your Access is {currentUser.WorkerRank}.");

            try
            {
                if (item is BookView)
                {
                    storeItems.Add(ViewConverter.CreateBook((BookView)item));
                    ItemAdded?.Invoke(this, item);
                }
                else if (item is JournalView)
                {
                    storeItems.Add(ViewConverter.CreateJournal((JournalView)item));
                    ItemAdded?.Invoke(this, item);
                }
                storeItems.Save();
            }
            catch (DAL.Repositorys.PrimeryKeyAllReadyExistException e)
            {
                log.Exception(e);
                throw new util.PrimeryKeyAllReadyExistException();
            }catch(RepositorySaveFailedExceptiom e)
            {
                log.Exception(e, "Failed on tring to add a new item.");
                log.Fetal("Fetal Error, App shuting down.");
                Shutdown(1);
            }
            catch(Exception e)
            {
                log.Exception(e);
                log.Fetal("Fetal Error Closing down app...");
                Shutdown(1);
            }


        }
        public void AddWorker(WorkerView work,byte[] hashedpass)
        {
            if (!IsUserLoggedIn) throw new UserLoggedInException();

            if (currentUser.WorkerRank != WorkerView.Rank.Manager) throw new UserAccessException($"{WorkerView.Rank.Manager} Access needed for that action, your Access is {currentUser.WorkerRank}.");

            try
            {
                workers.Open();
            }
            catch (RepositoryOpenFailedExceptiom e)
            {
                log.Exception(e, "Failed on tring to add user.");
                log.Fetal("Fetal Error, App shuting down.");
                Shutdown(1);
            }
            Worker w = ViewConverter.CreateWorker(work);
            try
            {
                w.Password = SHA256.Create().ComputeHash(hashedpass);
                workers.Add(w);
                workers.Close();
                WorkerAdded?.Invoke(this,work);
            }
            catch(DAL.Repositorys.PrimeryKeyAllReadyExistException e)
            {
                log.Exception(e);
                throw new util.PrimeryKeyAllReadyExistException();
            }catch(RepositorySaveFailedExceptiom e)
            {
                log.Exception(e, "Failed on tring to add user.");
                log.Fetal("Fetal Error, App shuting down.");
                Shutdown(1);
            }
            catch(Exception e)
            {
                log.Exception(e);
                log.Fetal("Fetal Error Closing down app...");
                Shutdown(1);
            }

        }
        public bool Login(string userName, byte[] hashedPass)
        {
            Worker user;
            byte[] doubleHashpass;
            try
            {
                doubleHashpass = SHA256.Create().ComputeHash(hashedPass);
            }
            catch (Exception e)
            {
                log.Exception(e);
                return false;
            }
            try
            {

                workers.Open();
            }
            catch (RepositoryOpenFailedExceptiom e)
            {
                log.Exception(e, "Failed on tring to login.");
                log.Fetal("Fetal Error, App shuting down.");
                Shutdown(1);
            }
            if (workers.GetFirstBy((w) => w.Username.ToLower() == userName.ToLower(), out user))
            {

                try
                {

                    workers.Close();
                }
                catch (RepositorySaveFailedExceptiom e)
                {
                    log.Exception(e, "Failed on tring to login.");
                    log.Fetal("Fetal Error, App shuting down.");
                    Shutdown(1);
                }
                if (workers.ComperPass(doubleHashpass, user.Password))
                {
                    LoggedIn?.Invoke(this, new UserLoggedInEventArgs(user));
                    return true;
                }
                return false;
            }
            try
            {

                workers.Close();
            }
            catch (RepositorySaveFailedExceptiom e)
            {
                log.Exception(e, "Failed on tring to login.");
                log.Fetal("Fetal Error, App shuting down.");
                Shutdown(1);
            }
            return false;
        }
        public bool UserExist(string userName)
        {

            try
            {
                workers.Open();
            }
            catch (RepositoryOpenFailedExceptiom e)
            {
                log.Exception(e, "Failed on tring to check if user exist.");
                log.Fetal("Fetal Error, App shuting down.");
                Shutdown(1);
            }
            bool exist = workers.FindBy((w) => w.FirstName.ToLower() == userName.ToLower());
            try
            {

                workers.Close();
            }
            catch (RepositorySaveFailedExceptiom e)
            {
                log.Exception(e, "Failed on check if user exist.");
                log.Fetal("Fetal Error, App shuting down.");
                Shutdown(1);
            }
            return exist;
        }
        public void UpdateStoreItem(StoreItemView updated, StoreItemView toUpdate)
        {
            if (!IsUserLoggedIn) throw new UserLoggedInException();

            if (currentUser.WorkerRank != WorkerView.Rank.Manager) throw new UserAccessException($"{WorkerView.Rank.Manager} Access needed for that action, your Access is {currentUser.WorkerRank}.");

            StoreItem storeItem = storeItems.GetById(toUpdate.Id);

            BookView updatedBook = updated as BookView;
            if (updatedBook != null)
            {
                Book upBook = ViewConverter.CreateBook(updatedBook);
                Book bookItem = storeItem as Book;
                if (bookItem == null) throw new UpdatedItemNotMachingException(updated.GetType(), toUpdate.GetType());

                try
                {
                    storeItems.UpdateBook(upBook, bookItem);
                }
                catch (DAL.Repositorys.PrimeryKeyAllReadyExistException)
                {
                    throw new util.PrimeryKeyAllReadyExistException();
                }
            }
            else
            {

                JournalView updatedJournal = updated as JournalView;

                if (updatedJournal != null)
                {
                    Journal upJour = ViewConverter.CreateJournal(updatedJournal);
                    Journal jourItem = storeItem as Journal;
                    if (jourItem == null) throw new UpdatedItemNotMachingException(updated.GetType(), toUpdate.GetType());

                    try
                    {
                        storeItems.UpdateJournal(upJour, jourItem);
                    }
                    catch (DAL.Repositorys.PrimeryKeyAllReadyExistException)
                    {
                        throw new util.PrimeryKeyAllReadyExistException();
                    }
                }
            }
            try
            {
                storeItems.Save();
            }catch(RepositorySaveFailedExceptiom e)
            {
                log.Exception(e, "Failed on tring to update item.");
                log.Fetal("Fetal Error, App shuting down.");
                Shutdown(1);
            }
            ItemUpdated?.Invoke(this, updated);
        }
        public void UpdateWorker(WorkerView updated, WorkerView toUpdate)
        {
            if (!IsUserLoggedIn) throw new UserLoggedInException();

            Worker upTo = ViewConverter.CreateWorker(updated);

            try
            {
                workers.Open();
                if (!workers.UpdateWorker(toUpdate.Id, upTo))
                {
                    workers.Close();
                    throw new WorkerUpdateFailedException();
                }
                if (currentUser.Id == toUpdate.Id)
                    WorkerUpdated?.Invoke(this, ViewConverter.CreateWorkerView(workers.GetById(currentUser.Id)));
                workers.Close();
            }
            catch (DAL.Repositorys.PrimeryKeyAllReadyExistException e)
            {
                log.Exception(e);
                throw new util.PrimeryKeyAllReadyExistException();
            }catch(RepositoryOpenFailedExceptiom e)
            {
                log.Exception(e, "Failed on tring to update a user.");
                log.Fetal("Fetal Error, App shuting down.");
                Shutdown(1);
                return;
            }catch(RepositorySaveFailedExceptiom e)
            {
                log.Exception(e, "Failed on tring to update a user.");
                log.Fetal("Fetal Error, App shuting down.");
                Shutdown(1);
                return;
            }
            catch (Exception e)
            {
                log.Exception(e, "Unknown exception, Closing App.");
                Shutdown(1);
                return;
            }
            finally
            {
                workers.Close();
            }

        }
        public bool ChangePassword(int id, byte[] oldhashedPass, byte[] newHashedPass)
        {
            byte[] doubleHashpass;
            try
            {
                doubleHashpass = SHA256.Create().ComputeHash(oldhashedPass); 
            }
            catch (Exception e)
            {
                log.Exception(e);
                return false;
            }
            byte[] newDoubleHashpass;
            try
            {
                newDoubleHashpass = SHA256.Create().ComputeHash(newHashedPass);
            }
            catch (Exception e)
            {
                log.Exception(e);
                return false;
            }

            if (currentUser.Id == id)
            {
                try
                {
                    workers.Open();
                }
                catch (RepositoryOpenFailedExceptiom e)
                {
                    log.Exception(e, "Failed on tring to change password.");
                    log.Fetal("Fetal Error, App shuting down.");
                    Shutdown(1);
                }
                if (workers.UpdatePass(id, doubleHashpass, newDoubleHashpass))
                {
                    try
                    {

                        workers.Close();
                    }
                    catch (RepositorySaveFailedExceptiom e)
                    {
                        log.Exception(e, "Failed on tring to login.");
                        log.Fetal("Fetal Error, App shuting down.");
                        Shutdown(1);
                        return false;
                    }
                    return true;
                }
                else
                {
                    try
                    {

                        workers.Close();
                    }
                    catch (RepositorySaveFailedExceptiom e)
                    {
                        log.Exception(e, "Failed on tring to login.");
                        log.Fetal("Fetal Error, App shuting down.");
                        Shutdown(1);
                        return false;
                    }
                    return false;
                }
            }
            else
                throw new UserAccessException("No Access To The User Password.");
        }
        public bool IsCurrentUser(WorkerView other)
        {
            if (!IsUserLoggedIn) throw new UserLoggedInException();

            return currentUser.Equals(other);
        }
        public WorkerView GetUserView(int i)
        {

            if (currentUser.Id == i)
            {
                try
                {
                    workers.Open();
                    Worker w = workers.GetById(i);
                    workers.Close();
                    return ViewConverter.CreateWorkerView(w);
                }
                catch (ItemNotFoundException e)
                {
                    log.Exception(e);
                }catch(RepositoryOpenFailedExceptiom e)
                {
                    log.Exception(e, "Failed on tring to get a user view.");
                    log.Fetal("Fetal Error, App shuting down.");
                    Shutdown(1);
                }catch(RepositorySaveFailedExceptiom e)
                {
                    log.Exception(e, "Failed on tring to get user.");
                    log.Fetal("Fetal Error, App shuting down.");
                    Shutdown(1);
                }
                catch (RepositoryCloesedException e)
                {
                    log.Exception(e);
                }
                finally
                {
                    try
                    {

                        workers.Close();
                    }
                    catch (RepositorySaveFailedExceptiom e)
                    {
                        log.Exception(e, "Failed on tring to get user.");
                        log.Fetal("Fetal Error, App shuting down.");
                        Shutdown(1);
                    }
                }

            }
            if (currentUser.WorkerRank == WorkerView.Rank.Manager)
            {

                try
                {
                    workers.Open();
                    Worker w = workers.GetById(i);
                    try
                    {

                        workers.Close();
                    }
                    catch (RepositorySaveFailedExceptiom e)
                    {
                        log.Exception(e, "Failed on tring to get user.");
                        log.Fetal("Fetal Error, App shuting down.");
                        Shutdown(1);
                    }
                    return ViewConverter.CreateWorkerView(w);

                }
                catch (ItemNotFoundException e)
                {
                    log.Exception(e);
                }
                catch (RepositoryCloesedException e)
                {
                    log.Exception(e);
                }
                catch (Exception e)
                {
                    log.Exception(e, "What just Hapend?");
                    Shutdown(1);
                }
                finally
                {
                    try
                    {

                        workers.Close();
                    }
                    catch (RepositorySaveFailedExceptiom e)
                    {
                        log.Exception(e, "Failed on tring to get user.");
                        log.Fetal("Fetal Error, App shuting down.");
                        Shutdown(1);
                    }
                }
;
            }

            throw new UserAccessException("You dont have access to that user.");
        }
        public void RemoveStorItem(int id)
        {
            StoreItem item;
            if (storeItems.RemoveById(id, out item))
            {
                ItemRemoved?.Invoke(this, ViewConverter.CreateStoreItemView(item));
            }

        }
        public void RemoveWorker(int id)
        {
            if (!IsUserLoggedIn) throw new UserLoggedInException();

            if (currentUser.WorkerRank != WorkerView.Rank.Manager) throw new UserAccessException($"{WorkerView.Rank.Manager} Access needed for that action, your Access is {currentUser.WorkerRank}.");

            Worker worker;
            try
            {

            workers.Open();
            }catch(RepositoryOpenFailedExceptiom e)
            {
                log.Exception(e, "Failed on tring to update a user.");
                log.Fetal("Fetal Error, App shuting down.");
                Shutdown(1);
            }
            if(workers.RemoveById(id,out worker))
            {
                try
                {

                    workers.Close();
                }
                catch (RepositorySaveFailedExceptiom e)
                {
                    log.Exception(e, "Failed on tring to get user.");
                    log.Fetal("Fetal Error, App shuting down.");
                    Shutdown(1);
                }
                WorkerRemoved?.Invoke(this,ViewConverter.CreateWorkerView(worker));
            }
        }

        internal DAL.Logger.Logger Log => log;

        private static Logic instance;
        private DAL.Logger.Logger log;
        private StoreItemRepository storeItems;
        private WorkerRepository workers;
        private WorkerView currentUser;
        private Logic()
        {
            log = new DAL.Logger.Logger();
            storeItems = new StoreItemRepository();
            workers = new WorkerRepository();
            LoggedIn += User_LoggedIn;
        }
        private void User_LoggedIn(object sender, UserLoggedInEventArgs args)
        {
            currentUser = args.Worker;
            try
            {
                storeItems.Open();
            }catch(RepositoryOpenFailedExceptiom e)
            {
                log.Exception(e, "Failed on user login.");
                log.Fetal("Fetal Error, App shuting down.");
                Shutdown(1);
            }
            
        }
    }
}
