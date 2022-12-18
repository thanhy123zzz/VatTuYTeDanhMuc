using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace VatTuYTeDanhMuc.Models.Entities
{
    public partial class PhieuTraNo
    {
        public PhieuTraNo()
        {
            ChiTietTraNo = new HashSet<ChiTietTraNo>();
        }

        public int Id { get; set; }
        public string SoPhieu { get; set; }
        public int? Idncc { get; set; }
        public int? Idhttt { get; set; }
        public DateTime? NgayTra { get; set; }
        public double? TongTien { get; set; }
        public int? Nvtao { get; set; }
        public DateTime? NgayTao { get; set; }
        public int? Nvsua { get; set; }
        public DateTime? NgaySua { get; set; }
        public bool? Active { get; set; }
        public int? Idnv { get; set; }
        public int? Idcn { get; set; }

        public virtual Httt IdhtttNavigation { get; set; }
        public virtual NhaCungCap IdnccNavigation { get; set; }
        public virtual NhanVien IdnvNavigation { get; set; }
        public virtual ICollection<ChiTietTraNo> ChiTietTraNo { get; set; }
    }
}
