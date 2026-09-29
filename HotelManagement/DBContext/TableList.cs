using System;
using System.Collections.Generic;

namespace HotelManagement.DBContext;

public partial class TableList
{
    public int TableId { get; set; }

    public int TableNumber { get; set; }

    public string TableName { get; set; } = null!;

    public string DisplayTableName { get; set; } = null!;

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? UpdatedBy { get; set; }

    public virtual User? CreatedByNavigation { get; set; }

    public virtual ICollection<OrderList> OrderLists { get; set; } = new List<OrderList>();

    public virtual User? UpdatedByNavigation { get; set; }
}
