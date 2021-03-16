using System.Windows;
using System.Windows.Controls;

namespace BookStore.Client.res.UserControls
{
    /// <summary>
    /// Interaction logic for ErrorMessage.xaml
    /// </summary>
    public partial class ErrorMessage : UserControl
    {
        public ErrorMessage()
        {
            InitializeComponent();
        }

        public void Pop(string message)
        {
            errorBox.Visibility = Visibility.Visible;
            errorTxt.Text = message;
        }

        public void Hide()
        {
            errorBox.Visibility = Visibility.Collapsed;
            errorTxt.Text = string.Empty;
        }
    }
}
