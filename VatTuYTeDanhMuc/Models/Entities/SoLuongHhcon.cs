using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace VatTuYTeDanhMuc.Models.Entities
{
    public partial class SoLuongHhcon
    {
        public int Id { get; set; }
        public int? Idctpn { get; set; }
        public DateTime? NgayNhap { get; set; }
        public double? Slcon { get; set; }
        public int? Idcn { get; set; }
        public double? GiaNhap { get; set; }
        public double? Thue { get; set; }
        public double? Cktm { get; set; }

        public virtual ChiTietPhieuNhap IdctpnNavigation { get; set; }
    }
}
