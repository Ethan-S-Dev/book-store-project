using BookStore.BLL;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace BookStore.Client.Windows
{
    /// <summary>
    /// Interaction logic for BookStoreWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            close_btn.Click += (s, e) => Close();
            min_btn.Click += (s, e) => WindowState = WindowState.Minimized;
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string userName = userBox.Text;
            byte[] pass = SHA256.Create().ComputeHash(Encoding.Unicode.GetBytes(passBox.Password));
            if (!Logic.Instance.Login(userName, pass))
            {
                errorBox.Pop("Username or Password is incorrect..");
            }
            else
                errorBox.Hide();

            passBox.Password = string.Empty;
        }
    }
}
