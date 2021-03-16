using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BookStore.Client.res.UserControls
{
    /// <summary>
    /// Interaction logic for FieldBox.xaml
    /// </summary>
    public partial class FieldBox : UserControl
    {
        public double BoxWidth
        {
            get { return fieldBox.Width; }
            set 
            { 
                fieldBox.Width = value; 
            }
        }
        public string Text
        {
            get { return fieldName.Text; }
            set 
            { 
                fieldName.Text = value; 
            }
        }     
        public string Value { get
            {
                return fieldBox.Text;
            }
            set
            {
                fieldBox.Text = value;
            }
        }

        public event TextChangedEventHandler TextChanged;
        public event TextCompositionEventHandler PreviewInput;
        public event RoutedEventHandler InputLostFocus;

        public FieldBox()
        {
            InitializeComponent();
            fieldBox.TextChanged += (s, e) => TextChanged?.Invoke(s, e);
            fieldBox.PreviewTextInput += (s, e) => PreviewInput?.Invoke(s,e);
            fieldBox.LostFocus += (s, e) => InputLostFocus?.Invoke(s, e);
        }
    }
}
