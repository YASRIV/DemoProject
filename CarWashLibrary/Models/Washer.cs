using System;
using System.Collections.Generic;

namespace Car_Wash_Library.Models;

public partial class Washer
{
    public int WasherId { get; set; }

    public string WasherName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PhoneNo { get; set; } = null!;

    public bool IsActive { get; set; }
}
