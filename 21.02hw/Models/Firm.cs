using System;
using System.Collections.Generic;

namespace _21._02hw.Models;

public partial class Firm
{
    public int FirmId { get; set; }

    public string FirmName { get; set; } = null!;

    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();
}
