using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace VatTuYTeDanhMuc.Models.Entities
{
    public partial class PhieuThuNo
    {
        public PhieuThuNo()
        {
            ChiTietThuNo = new HashSet<ChiTietThuNo>();
        }

        public int Id { get; set; }
        public string SoPhieu { get; set; }
        public int? Idkh { get; set; }
        public int? Idhttt { get; set; }
        public DateTime? NgayTra { get; set; }
        public decimal? TongTien { get; set; }
        public int? Nvtao { get; set; }
        public DateTime? NgayTao { get; set; }
        public int? Nvsua { get; set; }
        public DateTime? NgaySua { get; set; }
        public bool? Active { get; set; }
        public int? Idnv { get; set; }
        public int? Idcn { get; set; }

        public virtual Httt IdhtttNavigation { get; set; }
        public virtual NhanVien IdnvNavigation { get; set; }
        public virtual ICollection<ChiTietThuNo> ChiTietThuNo { get; set; }
    }
}
