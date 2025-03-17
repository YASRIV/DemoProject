using System;
using System.Collections.Generic;

namespace Car_Wash_Library.Models;

public partial class Payment
{

    public int PaymentId { get; set; }
    public int TransactionId { get; set; }

    public DateTime TransactionDate { get; set; }

    public double Amount { get; set; }

    public string PaymentMethod { get; set; } = null!;

    public string PaymentStatus { get; set; } = null!;

    public bool IsActive { get; set; }
}
