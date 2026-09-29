using System;
using System.Collections.Generic;

namespace HotelManagement.DBContext;

public partial class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? ImageName { get; set; }

    public string? ResetToken { get; set; }

    public DateTime? ResetTokenExpiry { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    public virtual User? CreatedByNavigation { get; set; }

    public virtual ICollection<User> InverseCreatedByNavigation { get; set; } = new List<User>();

    public virtual ICollection<User> InverseUpdatedByNavigation { get; set; } = new List<User>();

    public virtual ICollection<ItemList> ItemListCreatedByNavigations { get; set; } = new List<ItemList>();

    public virtual ICollection<ItemList> ItemListUpdatedByNavigations { get; set; } = new List<ItemList>();

    public virtual ICollection<OrderItemList> OrderItemListCreatedByNavigations { get; set; } = new List<OrderItemList>();

    public virtual ICollection<OrderItemList> OrderItemListUpdatedByNavigations { get; set; } = new List<OrderItemList>();

    public virtual ICollection<OrderList> OrderListCreatedByNavigations { get; set; } = new List<OrderList>();

    public virtual ICollection<OrderList> OrderListUpdatedByNavigations { get; set; } = new List<OrderList>();

    public virtual ICollection<TableList> TableListCreatedByNavigations { get; set; } = new List<TableList>();

    public virtual ICollection<TableList> TableListUpdatedByNavigations { get; set; } = new List<TableList>();

    public virtual User? UpdatedByNavigation { get; set; }
}
