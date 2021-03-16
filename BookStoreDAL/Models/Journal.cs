using System;

namespace BookStore.DAL.Models
{
    /// <summary>
    /// DAL Journal model drives from StoreItem
    /// </summary>
    [Serializable]
    public class Journal : StoreItem
    {
        public Journal(string name, decimal unitPrice, float discount, string isbn, int unitsInStock,int volumeNumber,string field,DateTime publishedDate,int issueNumber, byte[] image,int id =0) : base(id,name, unitPrice, discount, isbn, publishedDate, unitsInStock, image)
        {
            Field = field;
            VolumeNumber = volumeNumber;
            IssueNumber = issueNumber;
        }
        public string Field { get; set; }
        public int VolumeNumber { get; set; }
        public int IssueNumber { get; set; }      
    }
}
