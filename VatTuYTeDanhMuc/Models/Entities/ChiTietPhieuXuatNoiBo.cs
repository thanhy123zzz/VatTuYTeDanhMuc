﻿using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace VatTuYTeDanhMuc.Models.Entities
{
    public partial class ChiTietPhieuXuatNoiBo
    { 
        public int Id { get; set; }
        public int? Idpxnb { get; set; }
        public int? Idhh { get; set; }
        public int? Iddvt { get; set; }
        public double? Slg { get; set; }
        public double? DonGia { get; set; }
        public int? Idctpn { get; set; }
        public int? Nvtao { get; set; }
        public DateTime? NgayTao { get; set; }
        public int? Nvsua { get; set; }
        public DateTime? NgaySua { get; set; }
        public bool? Active { get; set; }

        public virtual ChiTietPhieuNhap IdctpnNavigation { get; set; }
        public virtual Dvt IddvtNavigation { get; set; }
        public virtual HangHoa IdhhNavigation { get; set; }
        public virtual PhieuXuatNoiBo IdpxnbNavigation { get; set; }
    }
}
