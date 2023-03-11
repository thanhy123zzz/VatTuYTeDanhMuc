using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace VatTuYTeDanhMuc.Models.Entities
{
    public partial class ChiTietPhieuNhap
    {
        public ChiTietPhieuNhap()
        {
            ChiTietPhieuXuat = new HashSet<ChiTietPhieuXuat>();
            ChiTietPhieuXuatNoiBo = new HashSet<ChiTietPhieuXuatNoiBo>();
            ChiTietPhieuXuatTamNoiBo = new HashSet<ChiTietPhieuXuatTamNoiBo>();
            SoLuongHhcon = new HashSet<SoLuongHhcon>();
        }

        public int Id { get; set; }
        public int? Idpn { get; set; }
        public int? Idhh { get; set; }
        public int? Idbh { get; set; }
        public double? Slg { get; set; }
        public double? DonGia { get; set; }
        public double? Cktm { get; set; }
        public double? Thue { get; set; }
        public string SoLo { get; set; }
        public DateTime? Nsx { get; set; }
        public DateTime? Hsd { get; set; }
        public int? Tgbh { get; set; }
        public string GhiChu { get; set; }
        public int? Nvtao { get; set; }
        public DateTime? NgayTao { get; set; }
        public int? Nvsua { get; set; }
        public DateTime? NgaySua { get; set; }
        public bool? Active { get; set; }

        public virtual BaoHanh IdbhNavigation { get; set; }
        public virtual HangHoa IdhhNavigation { get; set; }
        public virtual PhieuNhap IdpnNavigation { get; set; }
        public virtual ICollection<ChiTietPhieuXuat> ChiTietPhieuXuat { get; set; }
        public virtual ICollection<ChiTietPhieuXuatNoiBo> ChiTietPhieuXuatNoiBo { get; set; }
        public virtual ICollection<ChiTietPhieuXuatTamNoiBo> ChiTietPhieuXuatTamNoiBo { get; set; }
        public virtual ICollection<SoLuongHhcon> SoLuongHhcon { get; set; }
    }
}
