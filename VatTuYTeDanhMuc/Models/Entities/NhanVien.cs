using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace VatTuYTeDanhMuc.Models.Entities
{
    public partial class NhanVien
    {
        public NhanVien()
        {
            HinhAnhNhanVien = new HashSet<HinhAnhNhanVien>();
            KhachHang = new HashSet<KhachHang>();
            PhieuNhap = new HashSet<PhieuNhap>();
            PhieuThuNo = new HashSet<PhieuThuNo>();
            PhieuTraNo = new HashSet<PhieuTraNo>();
            PhieuXuat = new HashSet<PhieuXuat>();
        }

        public int Id { get; set; }
        public string MaNv { get; set; }
        public string TenNv { get; set; }
        public bool? GioiTinh { get; set; }
        public DateTime? NgaySinh { get; set; }
        public string DiaChi { get; set; }
        public string QueQuan { get; set; }
        public string Sdt { get; set; }
        public string Email { get; set; }
        public string Cccd { get; set; }
        public string Avatar { get; set; }
        public int? Idnnv { get; set; }
        public string TenTaiKhoan { get; set; }
        public int? Nvtao { get; set; }
        public DateTime? NgayTao { get; set; }
        public int? Nvsua { get; set; }
        public DateTime? NgaySua { get; set; }
        public bool? Active { get; set; }
        public int? Idcn { get; set; }

        public virtual NhomNhanVien IdnnvNavigation { get; set; }
        public virtual TaiKhoan TenTaiKhoanNavigation { get; set; }
        public virtual ICollection<HinhAnhNhanVien> HinhAnhNhanVien { get; set; }
        public virtual ICollection<KhachHang> KhachHang { get; set; }
        public virtual ICollection<PhieuNhap> PhieuNhap { get; set; }
        public virtual ICollection<PhieuThuNo> PhieuThuNo { get; set; }
        public virtual ICollection<PhieuTraNo> PhieuTraNo { get; set; }
        public virtual ICollection<PhieuXuat> PhieuXuat { get; set; }
    }
}
