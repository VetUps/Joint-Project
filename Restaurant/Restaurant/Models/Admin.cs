using System;
using System.Collections.Generic;

namespace Restaurant.Models;

public partial class Admin
{
    public int AdminId { get; set; }

    public string? AdminLogin { get; set; }

    public string? AdminPassword { get; set; }
}
