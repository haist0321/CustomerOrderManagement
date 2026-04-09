using System;
using System.Collections.Generic;

namespace CustomerOrderManagement.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int CustomerId { get; set; }

    public string OrderType { get; set; } = null!;

    public DateTime? OrderDate { get; set; }

    public decimal TotalAmount { get; set; }

    public decimal Discount { get; set; }

    public decimal FinalAmount { get; set; }

    public virtual Customer Customer { get; set; } = null!;
}
