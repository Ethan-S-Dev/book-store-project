using BookStore.ViewModel.Models;
using System;
using System.Text;
using System.Windows.Controls;

namespace BookStore.ViewModel.Controls
{
    /// <summary>
    /// Interaction logic for WorkerControl.xaml
    /// </summary>
    public partial class WorkerControl : UserControl
    {
        public event EventHandler<WorkerView> Click;
        public bool PressEnabled { get; set; }
        public WorkerView User
        {
            get
            {
                return user;
            }
            set
            {
                StringBuilder st = new StringBuilder();
                user = value;
                st.Append(user.FirstName.Substring(0, 1).ToUpper());
                st.Append(user.FirstName.Substring(1));
                st.Append(" ");
                st.Append(user.LastName.Substring(0, 1).ToUpper());
                st.Append(user.LastName.Substring(1));
                nameBlock.Text = st.ToString();

                st.Clear();
                st.Append(user.Username.Substring(0, 1).ToUpper());
                st.Append(user.Username.Substring(1));
                userBlock.Text = st.ToString();

                titleBox.Text = user.WorkerRank.ToString();

                ProfileImag.Source = user.GetUserImage();
            }
        }
        public WorkerControl()
        {
            PressEnabled = true;
            InitializeComponent();
            profile_box.MouseLeftButtonDown += (s, e) => { if (PressEnabled) pressed = true; };
            profile_box.MouseLeftButtonUp += (s, e) => { if (PressEnabled) { if (pressed) { pressed = false; Click?.Invoke(this, user); } } };
        }
        public WorkerControl(WorkerView worker) : this()
        {        
            User = worker;           
        }

        private bool pressed = false;
        private WorkerView user;       
    }
}
