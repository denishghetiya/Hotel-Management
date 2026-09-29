using System;
using System.Collections.Generic;

namespace HotelManagement.DBContext;

public partial class ItemList
{
    public int ItemId { get; set; }

    public string ItemName { get; set; } = null!;

    public decimal Price { get; set; }

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? UpdatedBy { get; set; }

    public virtual User? CreatedByNavigation { get; set; }

    public virtual ICollection<OrderItemList> OrderItemLists { get; set; } = new List<OrderItemList>();

    public virtual User? UpdatedByNavigation { get; set; }
}
