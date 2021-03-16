using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace BookStore.Client.res.UserControls
{
    /// <summary>
    /// Interaction logic for MenuControl.xaml
    /// </summary>
    public partial class MenuList : UserControl
    {

        public event EventHandler<SelectionEventArgs> SelectionChanged;

        private MenuItem selectedItem;
        public object SelectedItem
        {
            get
            {
                return selectedItem.AdditionalContent;
            }
        }

        public MenuList()
        {
            InitializeComponent();
        }

        public IEnumerable<object> ItemSourse
        {
            get
            {
                LinkedList<object> list = new LinkedList<object>();
                foreach (var item in listPanel.Children)
                {
                    MenuItem menuItem = item as MenuItem;
                    list.AddLast(menuItem.AdditionalContent);
                }
                return list;
            }
            set
            {
                listPanel.Children.Clear();
                int count = 0;
                foreach (var item in value)
                {
                    
                    MenuItem mi = new MenuItem(item);
                    mi.Index = count++;
                    mi.IsSelected = false;
                    mi.Click += Change_Selection;
                    listPanel.Children.Add(mi);
                }
            }
        }

        private void Change_Selection(object sender, EventArgs e)
        {
            MenuItem prev = selectedItem;
            selectedItem = (MenuItem)sender;
            if(prev != null)
                prev.IsSelected = false;
            selectedItem.IsSelected = true;
            SelectionChanged?.Invoke(this, new SelectionEventArgs(selectedItem, prev));
        }

        public void Change_Selection(int index)
        {
            MenuItem prev = selectedItem;
            MenuItem next = null;
            int count = 0;
            foreach (var item in listPanel.Children)
            {
                if(index == count++)
                {
                    next = (MenuItem)item;
                    break;
                }
            }

            if (next == null) throw new IndexOutOfRangeException($"The menu Dosent contine an item in insex:{index}");

            selectedItem = next;
            if (prev != null)
                prev.IsSelected = false;
            selectedItem.IsSelected = true;
            SelectionChanged?.Invoke(this, new SelectionEventArgs(selectedItem, prev));
        }
    }

    public class SelectionEventArgs : EventArgs
    {
        public object SelectedItem { get; }
        public object PreviousItem { get; }
        public int SelectedIndex { get; }
        public DateTime ChangedTime { get; }
        public SelectionEventArgs(MenuItem selected, MenuItem previous)
        {
            ChangedTime = DateTime.Now;
            SelectedItem = selected.NavegateTo;
            if (previous != null)
                PreviousItem = previous.NavegateTo;
            else
                PreviousItem = null;
            SelectedIndex = selected.Index;
        }
    }
}
