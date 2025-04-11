using System;
using System.Collections.Generic;

namespace Restaurant.Models;

public partial class Client
{
    public int ClientId { get; set; }

    public string? ClientFirstname { get; set; }

    public string? ClientName { get; set; }

    public string? ClientEmail { get; set; }

    public string ClientPhone { get; set; } = null!;

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    public ICollection<ClientTable> ClientTables { get; set; } = new List<ClientTable>();
}
