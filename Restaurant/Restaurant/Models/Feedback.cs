using System;
using System.Collections.Generic;

namespace Restaurant.Models;

public partial class Feedback
{
    public int FeedbackId { get; set; }

    public int? ClientId { get; set; }

    public int? Rating { get; set; }

    public string? FeedbackText { get; set; }

    public DateOnly? FeedbackDate { get; set; }

    public virtual Client? Client { get; set; }
}
