using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace VatTuYTeDanhMuc.Models.Entities
{
    public partial class PhieuXuatNoiBo
    {
        public PhieuXuatNoiBo()
        {
            ChiTietPhieuXuatNoiBo = new HashSet<ChiTietPhieuXuatNoiBo>();
            ChiTietPhieuXuatTamNoiBo = new HashSet<ChiTietPhieuXuatTamNoiBo>();
            ChiTietThuNo = new HashSet<ChiTietThuNo>();
        }

        public int Id { get; set; }
        public string SoPxnb { get; set; }
        public string SoHd { get; set; }
        public DateTime? NgayHd { get; set; }
        public string GhiChu { get; set; }
        public int? Idnv { get; set; }
        public DateTime? NgayTao { get; set; }
        public int? Nvsua { get; set; }
        public DateTime? NgaySua { get; set; }
        public bool? Active { get; set; }
        public int? Idcn { get; set; }
        public int? Idk { get; set; }
        public int? Idtt { get; set; }

        public virtual Kho IdkNavigation { get; set; }
        public virtual NhanVien IdnvNavigation { get; set; }
        public virtual TrangThai IdttNavigation { get; set; }
        public virtual ICollection<ChiTietPhieuXuatNoiBo> ChiTietPhieuXuatNoiBo { get; set; }
        public virtual ICollection<ChiTietPhieuXuatTamNoiBo> ChiTietPhieuXuatTamNoiBo { get; set; }
        public virtual ICollection<ChiTietThuNo> ChiTietThuNo { get; set; }
    }
}
