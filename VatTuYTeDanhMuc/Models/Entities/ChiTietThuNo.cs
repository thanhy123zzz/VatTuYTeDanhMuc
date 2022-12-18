using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace VatTuYTeDanhMuc.Models.Entities
{
    public partial class ChiTietThuNo
    {
        public int Id { get; set; }
        public int? Idpx { get; set; }
        public int? Idptn { get; set; }
        public double? SoTien { get; set; }
        public int? Nvtao { get; set; }
        public DateTime? NgayTao { get; set; }
        public int? Nvsua { get; set; }
        public DateTime? NgaySua { get; set; }
        public bool? Active { get; set; }

        public virtual PhieuThuNo IdptnNavigation { get; set; }
        public virtual PhieuXuat IdpxNavigation { get; set; }
    }
}
