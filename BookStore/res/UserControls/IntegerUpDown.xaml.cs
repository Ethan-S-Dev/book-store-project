using System.Windows;
using System.Windows.Controls;

namespace BookStore.Client.res.UserControls
{
    /// <summary>
    /// Interaction logic for IntegerUpDown.xaml
    /// </summary>
    public partial class IntegerUpDown : UserControl
    {

        private int _numValue = 0;

        public int NumValue
        {
            get { return _numValue; }
            set
            {
                _numValue = value;
                txtNum.Text = value.ToString();
            }
        }

        public IntegerUpDown()
        {
            InitializeComponent();
            txtNum.Text = _numValue.ToString();
        }         

        private void cmdUp_Click(object sender, RoutedEventArgs e)
        {
            NumValue++;
        }

        private void cmdDown_Click(object sender, RoutedEventArgs e)
        {
            if(NumValue>0)
                NumValue--;
        }

        private void txtNum_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtNum == null)
            {
                return;
            }

            if (!int.TryParse(txtNum.Text, out _numValue))
                txtNum.Text = _numValue.ToString();
        }
    }
}
