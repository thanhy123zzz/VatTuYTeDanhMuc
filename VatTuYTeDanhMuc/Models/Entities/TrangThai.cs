using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace VatTuYTeDanhMuc.Models.Entities
{
    public partial class TrangThai
    {
        public TrangThai()
        {
            PhieuXuat = new HashSet<PhieuXuat>();
            PhieuXuatNoiBo = new HashSet<PhieuXuatNoiBo>();
        }

        public int Id { get; set; }
        public string MaTt { get; set; }
        public string TenTt { get; set; }
        public int? Idnvxk { get; set; }
        public DateTime? NgayXk { get; set; }
        public int? Iddvvc { get; set; }
        public DateTime? NgayBdvc { get; set; }
        public DateTime? NgayGiao { get; set; }
        public string TenNvgh { get; set; }
        public string Sdtnvgh { get; set; }
        public int? Idnvtt { get; set; }
        public DateTime? NgayTt { get; set; }
        public int? Nvtao { get; set; }
        public DateTime? NgayTao { get; set; }
        public int? Nvsua { get; set; }
        public DateTime? NgaySua { get; set; }
        public bool? Active { get; set; }
        public int? Idcn { get; set; }

        public virtual Dvvc IddvvcNavigation { get; set; }
        public virtual ICollection<PhieuXuat> PhieuXuat { get; set; }
        public virtual ICollection<PhieuXuatNoiBo> PhieuXuatNoiBo { get; set; }
    }
}
