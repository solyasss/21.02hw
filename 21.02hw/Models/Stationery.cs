using System;
using System.Collections.Generic;

namespace _21._02hw.Models;

public partial class Stationery
{
    public int StationeryID { get; set; }

    public string Name { get; set; } = null!;

    public int TypeID { get; set; }

    public int Quantity { get; set; }

    public decimal CostPrice { get; set; }

    public virtual ICollection<SaleDetail> SaleDetails { get; set; } = new List<SaleDetail>();

    public virtual StationeryType Type { get; set; } = null!;
}
