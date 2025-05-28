using System;
using System.Collections.Generic;

namespace Restaurant.Models;

public partial class ClientTable
{
    public int ClientId { get; set; }

    public int TableId { get; set; }

    public DateTime? DatetimeFrom { get; set; }

    public DateTime? DatetimeTo { get; set; }

    public virtual Client Client { get; set; } = null!;

    public virtual Table Table { get; set; } = null!;
}
