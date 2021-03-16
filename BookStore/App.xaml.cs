using System.Windows;
using BookStore.BLL;
using BookStore.Client.Windows;

namespace BookStore.Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Logic.Instance.LoggedIn += Instance_LoggedIn;
            Logic.Instance.ItemAdded += (s, e) => MessageBox.Show($"{e.Name} Was Added.");
            Logic.Instance.ItemRemoved += (s, e) => MessageBox.Show($"{e.Name} Was Removed.");
            Logic.Instance.OnClose += (s, e) => Current.Shutdown();
        }

        private void Instance_LoggedIn(object sender, UserLoggedInEventArgs args)
        {
            Window priv = Current.MainWindow;
            Current.MainWindow = new MainWindow(args.Worker);
            Current.MainWindow.Show();
            priv.Close();
        }
    }
}
