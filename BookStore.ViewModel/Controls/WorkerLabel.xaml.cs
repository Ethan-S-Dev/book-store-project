using BookStore.ViewModel.Models;
using System.Text;
using System.Windows.Controls;

namespace BookStore.ViewModel.Controls
{
    /// <summary>
    /// Interaction logic for WorkerLable.xaml
    /// </summary>
    public partial class WorkerLabel : UserControl
    {
        public string FirstName { get { return fName.Text; } set { fName.Text = value; } }
        public string LastName { get { return lName.Text; } set { lName.Text = value; } }
        public string UserName { get { return uName.Text; } set { uName.Text = value; } }

        public WorkerView LabelWorker { get { return worker; }
            set
            {
                this.worker = value;
                StringBuilder st = new StringBuilder();

                st.Append(worker.FirstName.Substring(0, 1).ToUpper());
                st.Append(worker.FirstName.Substring(1));
                FirstName = st.ToString();

                st.Clear();
                st.Append(worker.LastName.Substring(0, 1).ToUpper());
                st.Append(worker.LastName.Substring(1));
                LastName = st.ToString();

                st.Clear();
                st.Append(worker.Username.Substring(0, 1).ToUpper());
                st.Append(worker.Username.Substring(1));
                UserName = st.ToString();
            }
        }
        public WorkerLabel(WorkerView worker) : this()
        {            
            LabelWorker = worker;
        }
        public WorkerLabel()
        {
            InitializeComponent();
        }
        
        WorkerView worker;
    }
}
