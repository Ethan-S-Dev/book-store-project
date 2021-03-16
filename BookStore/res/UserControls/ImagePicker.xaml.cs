using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BookStore.Client.res.UserControls
{
    /// <summary>
    /// Interaction logic for ImagePicker.xaml
    /// </summary>
    public partial class ImagePicker : UserControl
    {
        private ImageSource image;
        public bool IsDefault => ImageBytes == null;
        public ImageSource Image { get { return image; } 
            set 
            {
                image = value;
                bookImg.Source = image;
            } 
        }
        public byte[] ImageBytes { get;set; }

        public ImageSource DefaultImage
        {
            get { return (ImageSource)GetValue(DefaultImageProperty); }
            set { SetValue(DefaultImageProperty, value); }
        }
        public static readonly DependencyProperty DefaultImageProperty =
            DependencyProperty.Register("DefaultImage", typeof(ImageSource), typeof(ImagePicker),
              new PropertyMetadata(null));

        public ImagePicker()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Title = "Upload Book Image";
            fileDialog.CheckFileExists = true;
            fileDialog.CheckPathExists = true;
            fileDialog.Multiselect = false;
            fileDialog.Filter = "PNG Files (*.png)|*.png|JPEG Files (*.jpeg)|*.jpeg|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";
            fileDialog.DefaultExt = ".png";

            bool? result = fileDialog.ShowDialog();
            if (result == true)
            {
                using (Stream file = fileDialog.OpenFile())
                {
                    using(MemoryStream memo = new MemoryStream())
                    {
                        file.CopyTo(memo);
                        ImageBytes = memo.ToArray();
                    }
                    file.Position = 0;
                    BitmapImage img = new BitmapImage();
                    img.BeginInit();
                    img.CacheOption = BitmapCacheOption.OnLoad;
                    img.StreamSource = file;
                    img.EndInit();
                    Image = img;
                }
            }
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            Image = DefaultImage;
            ImageBytes = null;
        }
    }
}
