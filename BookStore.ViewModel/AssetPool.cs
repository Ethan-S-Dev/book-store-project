using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace BookStore.ViewModel
{
    public class AssetPool
    {
        private static AssetPool instance;
        public static AssetPool Instance
        {
            get
            {
                if (instance == null)
                    instance = new AssetPool();
                return instance;
            }
        }
        public BitmapImage Default_BookImg { get; }
        public BitmapImage Default_UserImg { get; }
        private AssetPool()
        {
            Default_BookImg = new BitmapImage();
            Default_BookImg.BeginInit();
            Default_BookImg.CacheOption = BitmapCacheOption.OnLoad; // here
            Default_BookImg.UriSource = new Uri("Assets/Images/default-book.png", UriKind.Relative);
            Default_BookImg.EndInit();
            Default_UserImg = new BitmapImage();
            Default_UserImg.BeginInit();
            Default_UserImg.CacheOption = BitmapCacheOption.OnLoad; // here
            Default_UserImg.UriSource = new Uri("Assets/Images/default_profile.png", UriKind.Relative);
            Default_UserImg.EndInit();
        }
    }
}
