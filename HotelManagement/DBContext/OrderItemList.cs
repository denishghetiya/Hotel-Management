using System;
using System.Collections.Generic;

namespace HotelManagement.DBContext;

public partial class OrderItemList
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

    public virtual User? CreatedByNavigation { get; set; }

    public virtual ItemList Item { get; set; } = null!;

    public virtual OrderList Order { get; set; } = null!;

    public virtual User? UpdatedByNavigation { get; set; }
}
