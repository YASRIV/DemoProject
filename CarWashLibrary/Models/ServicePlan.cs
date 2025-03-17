using System;
using System.Collections.Generic;

namespace Car_Wash_Library.Models;

public partial class ServicePlan
{
    public int ServiceId { get; set; }

    public string ServiceName { get; set; } = null!;

    public string ServiceDescription { get; set; } = null!;

    public double Price { get; set; }

    public bool IsActive { get; set; }
}
