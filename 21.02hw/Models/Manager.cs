using System;
using System.Collections.Generic;

namespace _21._02hw.Models;

public partial class Manager
{
    public int ManagerID { get; set; }

    public string FullName { get; set; } = null!;

    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();
}
