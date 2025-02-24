using System;
using System.Collections.Generic;

namespace _21._02hw.Models;

public partial class SaleDetail
{
    public int SaleDetailID { get; set; }

    public int SaleID { get; set; }

    public int StationeryID { get; set; }

    public int QuantitySold { get; set; }

    public decimal PriceEach { get; set; }

    public virtual Sale Sale { get; set; } = null!;

    public virtual Stationery Stationery { get; set; } = null!;
}
