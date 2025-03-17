using System;
using System.Collections.Generic;

namespace Car_Wash_Library.Models;

public partial class Customer
{
    public int CustomerId { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string City { get; set; } = null!;

    public int ZipCode { get; set; }

    public bool IsActive { get; set; }
    
}
