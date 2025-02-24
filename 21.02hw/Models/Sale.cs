using System;
using System.Collections.Generic;

namespace _21._02hw.Models;

public partial class Sale
{
    public int SaleID { get; set; }

    public DateTime SaleDate { get; set; }

    public int ManagerID { get; set; }

    public int FirmID { get; set; }

    public virtual Firm Firm { get; set; } = null!;

    public virtual Manager Manager { get; set; } = null!;

    public virtual ICollection<SaleDetail> SaleDetails { get; set; } = new List<SaleDetail>();
}
