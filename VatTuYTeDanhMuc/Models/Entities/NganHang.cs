using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace VatTuYTeDanhMuc.Models.Entities
{
    public partial class NganHang
    {
        public NganHang()
        {
            KhachHangNganHang = new HashSet<KhachHangNganHang>();
            NccNganHang = new HashSet<NccNganHang>();
        }

        public int Id { get; set; }
        public string MaNh { get; set; }
        public string TenNh { get; set; }
        public string GhiChu { get; set; }
        public int? Nvtao { get; set; }
        public DateTime? NgayTao { get; set; }
        public int? Nvsua { get; set; }
        public DateTime? NgaySua { get; set; }
        public bool? Active { get; set; }
        public int? Idcn { get; set; }
        public int? Idhttt { get; set; }

        public virtual Httt IdhtttNavigation { get; set; }
        public virtual ICollection<KhachHangNganHang> KhachHangNganHang { get; set; }
        public virtual ICollection<NccNganHang> NccNganHang { get; set; }
    }
}
