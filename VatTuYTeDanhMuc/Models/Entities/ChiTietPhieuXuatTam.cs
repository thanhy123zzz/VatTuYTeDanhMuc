using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace VatTuYTeDanhMuc.Models.Entities
{
    public partial class ChiTietPhieuXuatTam
    {
        public int Id { get; set; }
        public string Host { get; set; }
        public int? Idpx { get; set; }
        public int? Idhh { get; set; }
        public int? Iddvt { get; set; }
        public double? Slg { get; set; }
        public double? DonGia { get; set; }
        public int? Idctpn { get; set; }
        public double? Cktm { get; set; }
        public double? Thue { get; set; }
        public int? Nvtao { get; set; }
        public DateTime? NgayTao { get; set; }
    }
}
