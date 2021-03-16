using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace BookStore.Client.res.UserControls
{
    /// <summary>
    /// Interaction logic for MenuItem.xaml
    /// </summary>
    public partial class MenuItem : UserControl
    {
        public event EventHandler Click;
        private bool pressed = false;
        private bool iaSelected = false;
        private Page navegateTo;
        public Page NavegateTo { get
            {
                return navegateTo;
            }
            set
            {
                navegateTo = value;
                AdditionalContent = navegateTo.ToString();
            } 
        }
        public int Index { get; set; }
        public bool IsSelected
        {
            get
            {
                return iaSelected;
            }
            set
            {
                iaSelected = value;
                if(iaSelected)
                    itemBorder.Background = new SolidColorBrush(Color.FromArgb(200, 200, 200, 200));
                else
                    itemBorder.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
            }
        }
        public object AdditionalContent
        {
            get { return (object)GetValue(AdditionalContentProperty); }
            set { SetValue(AdditionalContentProperty, value); }
        }
        public static readonly DependencyProperty AdditionalContentProperty =
            DependencyProperty.Register("AdditionalContent", typeof(object), typeof(MenuItem),
              new PropertyMetadata(null));
        public MenuItem()
        {
            InitializeComponent();
            itemName.MouseEnter += (s, e) => { itemBorder.Background = new SolidColorBrush(Color.FromArgb(180,200,200,200)); Cursor = Cursors.Hand; };
            itemName.MouseLeave += (s, e) => { if(!iaSelected) itemBorder.Background = new SolidColorBrush(Color.FromArgb(0,0,0,0)); ; Cursor = Cursors.Arrow; };
            itemName.MouseLeftButtonDown += (s, e) => { if (!pressed) pressed = true; else pressed = false;};
            itemName.MouseLeftButtonUp += (s, e) => { if (pressed) { pressed = false; Click?.Invoke(this,new EventArgs()); } };
            
        }

        public MenuItem(object content) : this()
        {
            Page page = content as Page;
            if (page != null)
            {
                NavegateTo = page;
                AdditionalContent = page.Title;
            }
            else
            {
                AdditionalContent = content;
                navegateTo = null;
            }
        }
    }
}
