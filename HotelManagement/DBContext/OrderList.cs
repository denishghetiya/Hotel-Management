using System;
using System.Collections.Generic;

namespace HotelManagement.DBContext;

public partial class OrderList
{
    public int OrderId { get; set; }

    public string CustomerName { get; set; } = null!;

    public int TableId { get; set; }

    public decimal BillAmount { get; set; }

    public bool BillPayed { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }

    public virtual User? CreatedByNavigation { get; set; }

    public virtual ICollection<OrderItemList> OrderItemLists { get; set; } = new List<OrderItemList>();

    public virtual TableList Table { get; set; } = null!;

    public virtual User? UpdatedByNavigation { get; set; }
}
