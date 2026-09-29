using System.ComponentModel.DataAnnotations;

namespace HotelManagement.ViewModels
{
    public class CreateItemViewModel
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; } 
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedBy { get; set; }
    }

}
