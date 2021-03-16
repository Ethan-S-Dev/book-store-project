using System;
using System.Windows;
using System.Windows.Controls;
using BookStore.ViewModel.Models;
using BookStore.BLL;
using BookStore.ViewModel.Controls;
using BookStore.Client.res.UserControls;

namespace BookStore.Client.Windows
{
    /// <summary>
    /// Interaction logic for TestWindow.xaml
    /// </summary>
    
    public partial class MainWindow : Window
    {
        public MainWindow(WorkerView user)
        {
            
            InitializeComponent();
            
            
            WorkerControl theUser = new WorkerControl(user);
            Pages.ProfilePage userProfile = new Pages.ProfilePage(user);
            
            
            StateChanged += TestWindow_StateChanged;
            close_btn.Click += (s, e) => Close();
            max_btn.Click += (s,e) => WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
            min_btn.Click += (s, e) => WindowState = WindowState.Minimized;
            page_Frame.NavigationUIVisibility = System.Windows.Navigation.NavigationUIVisibility.Hidden;
            userBox.Child = theUser;
            Page[] pages;
            if (user.WorkerRank == WorkerView.Rank.Manager)
            {
                pages = new Page[] { new Pages.InventoryPage(), new Pages.MangeWorkersPage(), userProfile };
                
            }else
            {
                pages = new Page[] { userProfile };
                theUser.Click += (s, u) => menu.Change_Selection(0);
            }
            
            menu.ItemSourse = pages;

            Logic.Instance.WorkerUpdated += (s,e)=> userBox.Child = new WorkerControl(Logic.Instance.GetUserView(user.Id));
        }

        private void TestWindow_StateChanged(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                full_win_Border.Padding = new Thickness(7);
                Border bord = (Border)close_btn.Template.FindName("btn_border", close_btn);
                bord.CornerRadius = new CornerRadius();
                window_border.CornerRadius = new CornerRadius();
                titlebar_border.CornerRadius = new CornerRadius();
                ((TextBlock)max_btn.Template.FindName("max_btn_text", max_btn)).Text = "\xE73F";

            }
            else if(WindowState == WindowState.Normal)
            {
                full_win_Border.Padding = new Thickness(0);
                Border bord = (Border)close_btn.Template.FindName("btn_border", close_btn);
                bord.CornerRadius = new CornerRadius(0, 10, 0, 0);
                window_border.CornerRadius = new CornerRadius(10, 10, 10, 10);
                titlebar_border.CornerRadius = new CornerRadius(10, 10, 0, 0);
                ((TextBlock)max_btn.Template.FindName("max_btn_text", max_btn)).Text = "\xE740";
            }
        }

        private void MenuList_SelectionChanged(object sender, SelectionEventArgs e)
        {
            page_Frame.Navigate(e.SelectedItem);
        }

        public void UpdateUserProfile()
        {
            userBox.Child = new WorkerControl();
        }
    }
}
