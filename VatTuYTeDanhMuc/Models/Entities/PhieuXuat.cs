using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace VatTuYTeDanhMuc.Models.Entities
{
    public partial class PhieuXuat
    {
        public PhieuXuat()
        {
            ChiTietPhieuXuat = new HashSet<ChiTietPhieuXuat>();
            ChiTietThuNo = new HashSet<ChiTietThuNo>();
        }

        public int Id { get; set; }
        public int? Idcn { get; set; }
        public string SoPx { get; set; }
        public int? Nvtao { get; set; }
        public DateTime? NgayTao { get; set; }
        public int? Nvsua { get; set; }
        public DateTime? NgaySua { get; set; }
        public bool? Active { get; set; }
        public int? Idkh { get; set; }
        public int? Idnv { get; set; }
        public int? Idtt { get; set; }

        public virtual KhachHang IdkhNavigation { get; set; }
        public virtual NhanVien IdnvNavigation { get; set; }
        public virtual TrangThai IdttNavigation { get; set; }
        public virtual ICollection<ChiTietPhieuXuat> ChiTietPhieuXuat { get; set; }
        public virtual ICollection<ChiTietThuNo> ChiTietThuNo { get; set; }
    }
}
