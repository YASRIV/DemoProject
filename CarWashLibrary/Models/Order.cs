using System;
using System.Collections.Generic;

namespace Car_Wash_Library.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int CustomerId { get; set; }

    public DateTime OrderDate { get; set; }

    public DateTime WashDate { get; set; }

    public int? WasherId { get; set; }

    public int? TransactionId { get; set; }

    public int? RequestId { get; set; }

    public int? ServiceOrderId { get; set; }

    public string OrderStatus { get; set; } = null!;

    public double TotalPrice { get; set; }

    public bool? IsInvoiceGenerated { get; set; }

    public bool IsActive { get; set; }
}
