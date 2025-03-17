using System;
using System.Collections.Generic;

namespace Car_Wash_Library.Models;

public partial class ServiceOrder
{
    public int ServiceOrderId { get; set; }

    public int ServiceId { get; set; }

    public int OrderId { get; set; }

    public double Price { get; set; }
}
