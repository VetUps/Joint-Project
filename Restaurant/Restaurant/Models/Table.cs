using System;
using System.Collections.Generic;

namespace Restaurant.Models;

public partial class Table
{
    public int TableId { get; set; }

    public int? TableCapacity { get; set; }

    public string? TableLocation { get; set; }

    public string? TableStatus { get; set; }

    public virtual ICollection<ClientTable> ClientTables { get; set; } = new List<ClientTable>();
}
