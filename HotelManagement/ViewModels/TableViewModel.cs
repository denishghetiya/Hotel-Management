using System.ComponentModel.DataAnnotations;

namespace HotelManagement.ViewModels
{
    public class CreateTableViewModel
    {
        public int TableId { get; set; }
        public int TableNumber { get; set; }
        public string TableName { get; set; } 
        public string DisplayTableName { get; set; } 
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedBy { get; set; }
    }
}
