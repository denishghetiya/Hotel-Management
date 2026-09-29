using System.ComponentModel.DataAnnotations;

namespace HotelManagement.ViewModels
{
    public class OrderViewModel
    {
        public int OrderId { get; set; }
        public string CustomerName { get; set; } 
        public int TableId { get; set; }
        public string DisplayTableName { get; set; }
        public decimal BillAmount { get; set; }
        public bool BillPayed { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
        public List<OrderItemViewModel> OrderItems { get; set; } = new List<OrderItemViewModel>();
        public List<CreateTableViewModel> Tables { get; set; } = new List<CreateTableViewModel>();
        public List<CreateItemViewModel> ItemList { get; set; } = new List<CreateItemViewModel>();
    }
    public class OrderItemViewModel
    {
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal FinalPrice { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedBy { get; set; }
    }

}
