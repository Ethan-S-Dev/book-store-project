using BookStore.BLL;
using BookStore.ViewModel.Models;
using System.Windows.Controls;

namespace BookStore.Client.Pages
{
    /// <summary>
    /// Interaction logic for ProfilePage.xaml
    /// </summary>
    public partial class ProfilePage : Page
    {
        WorkerView worker;
        public ProfilePage(WorkerView user)
        {
            InitializeComponent();
            worker = user;
            profileImage.Source = worker.GetUserImage();
            Logic.Instance.WorkerUpdated += (s, e) =>
            {
                profileImage.Source = e.GetUserImage();
                worker = e;
                profileForm.Worker = e;
            };
            profileForm.Worker = user;
        }

    }
}
