using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace VatTuYTeDanhMuc.Models.Entities
{
    public partial class ChiTietTraNo
    {
        public int Id { get; set; }
        public int? Idpn { get; set; }
        public decimal? SoTien { get; set; }
        public int? Nvtao { get; set; }
        public DateTime? NgayTao { get; set; }
        public int? Nvsua { get; set; }
        public DateTime? NgaySua { get; set; }
        public bool? Active { get; set; }
        public int? Idptn { get; set; }

        public virtual PhieuNhap IdpnNavigation { get; set; }
        public virtual PhieuTraNo IdptnNavigation { get; set; }
    }
}
