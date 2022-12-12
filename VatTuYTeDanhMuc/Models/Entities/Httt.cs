using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace VatTuYTeDanhMuc.Models.Entities
{
    public partial class Httt
    {
        public Httt()
        {
            NganHang = new HashSet<NganHang>();
            PhieuThuNo = new HashSet<PhieuThuNo>();
            PhieuTraNo = new HashSet<PhieuTraNo>();
        }

        public int Id { get; set; }
        public string MaHttt { get; set; }
        public string TenHttt { get; set; }
        public string GhiChu { get; set; }
        public int? Nvtao { get; set; }
        public DateTime? NgayTao { get; set; }
        public int? Nvsua { get; set; }
        public DateTime? NgaySua { get; set; }
        public bool? Active { get; set; }
        public int? Idcn { get; set; }

        public virtual ICollection<NganHang> NganHang { get; set; }
        public virtual ICollection<PhieuThuNo> PhieuThuNo { get; set; }
        public virtual ICollection<PhieuTraNo> PhieuTraNo { get; set; }
    }
}
