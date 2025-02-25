using System;
using System.Collections.Generic;

namespace _21._02hw.Models;

public partial class StationeryType
{
    public int TypeId { get; set; }

    public string TypeName { get; set; } = null!;

    public virtual ICollection<Stationery> Stationeries { get; set; } = new List<Stationery>();
}
