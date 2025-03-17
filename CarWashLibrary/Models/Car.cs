using System;
using System.Collections.Generic;

namespace Car_Wash_Library.Models;

public partial class Car
{
    public int CarId { get; set; }

    public int CustomerId { get; set; }

    public string CarModel { get; set; } = null!;

    public string CarMake { get; set; } = null!;

    public int CarYear { get; set; }

    public bool IsActive { get; set; }

}
