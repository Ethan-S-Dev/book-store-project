using System;

namespace BookStore.ViewModel.Models
{
    public class JournalView : StoreItemView
    {
        public JournalView(int id, string name, decimal unitPrice, float discount, string isbn, int unitsInStock,string field,int voulmeNum,int issueNum ,DateTime publishedDate,byte[] image = null) : base(id, name, unitPrice, discount, isbn, unitsInStock, publishedDate, image)
        {
            Field = field;
            VolumeNumber = voulmeNum;
            IssueNumber = issueNum;
        }

        public string Field { get; set; }
        public int VolumeNumber { get; set; }
        public int IssueNumber { get; set; }
    }
}
