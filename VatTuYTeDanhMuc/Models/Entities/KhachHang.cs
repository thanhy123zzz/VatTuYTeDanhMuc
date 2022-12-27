using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace VatTuYTeDanhMuc.Models.Entities
{
    public partial class KhachHang
    {
        public KhachHang()
        {
            GiaTheoKhachHang = new HashSet<GiaTheoKhachHang>();
            KhachHangNganHang = new HashSet<KhachHangNganHang>();
            PhieuXuat = new HashSet<PhieuXuat>();
        }

        public int Id { get; set; }
        public string MaKh { get; set; }
        public string TenKh { get; set; }
        public string DiaChi { get; set; }
        public string Sdt { get; set; }
        public string Email { get; set; }
        public string MaSoThue { get; set; }
        public string GhiChu { get; set; }
        public int? Nvsale { get; set; }
        public string TenTaiKhoan { get; set; }
        public int? Nvtao { get; set; }
        public DateTime? NgayTao { get; set; }
        public int? Nvsua { get; set; }
        public DateTime? NgaySua { get; set; }
        public bool? Active { get; set; }
        public int? Idcn { get; set; }
        public int? Idnv { get; set; }

        public virtual NhanVien IdnvNavigation { get; set; }
        public virtual TaiKhoan TenTaiKhoanNavigation { get; set; }
        public virtual ICollection<GiaTheoKhachHang> GiaTheoKhachHang { get; set; }
        public virtual ICollection<KhachHangNganHang> KhachHangNganHang { get; set; }
        public virtual ICollection<PhieuXuat> PhieuXuat { get; set; }
    }
}
