using System;
using System.Collections.Generic;

namespace Car_Wash_Library.Models;

public partial class WashRequest
{
    public int RequestId { get; set; }

    public int WasherId { get; set; }

    public int OrderId { get; set; }

    public string RequestStatus { get; set; } = null!;

    public bool IsActive { get; set; }
}
