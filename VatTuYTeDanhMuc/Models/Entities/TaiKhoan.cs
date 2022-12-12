using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace VatTuYTeDanhMuc.Models.Entities
{
    public partial class TaiKhoan
    {
        public TaiKhoan()
        {
            KhachHang = new HashSet<KhachHang>();
            VaiTro = new HashSet<VaiTro>();
        }

        public int Id { get; set; }
        public string TenTaiKhoan { get; set; }
        public string MatKhau { get; set; }
        public bool? Loai { get; set; }
        public int? Nvtao { get; set; }
        public DateTime? NgayTao { get; set; }
        public int? Nvsua { get; set; }
        public DateTime? NgaySua { get; set; }
        public bool? Active { get; set; }

        public virtual NhanVien NhanVien { get; set; }
        public virtual ICollection<KhachHang> KhachHang { get; set; }
        public virtual ICollection<VaiTro> VaiTro { get; set; }
    }
}
