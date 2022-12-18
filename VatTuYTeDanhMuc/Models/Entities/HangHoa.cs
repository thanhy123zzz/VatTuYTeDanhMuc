using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace VatTuYTeDanhMuc.Models.Entities
{
    public partial class HangHoa
    {
        public HangHoa()
        {
            ChiTietPhieuNhap = new HashSet<ChiTietPhieuNhap>();
            ChiTietPhieuXuat = new HashSet<ChiTietPhieuXuat>();
            HhDvt = new HashSet<HhDvt>();
            HinhAnhHangHoa = new HashSet<HinhAnhHangHoa>();
        }

        public int Id { get; set; }
        public string MaHh { get; set; }
        public string TenHh { get; set; }
        public string Avatar { get; set; }
        public int? Idnhh { get; set; }
        public int? Idhsx { get; set; }
        public int? Idnsx { get; set; }
        public int? Nvtao { get; set; }
        public DateTime? NgayTao { get; set; }
        public int? Nvsua { get; set; }
        public DateTime? NgaySua { get; set; }
        public bool? Active { get; set; }
        public int? Idcn { get; set; }
        public int? Iddvtchinh { get; set; }

        public virtual Dvt IddvtchinhNavigation { get; set; }
        public virtual HangSanXuat IdhsxNavigation { get; set; }
        public virtual NhomHangHoa IdnhhNavigation { get; set; }
        public virtual NuocSanXuat IdnsxNavigation { get; set; }
        public virtual ICollection<ChiTietPhieuNhap> ChiTietPhieuNhap { get; set; }
        public virtual ICollection<ChiTietPhieuXuat> ChiTietPhieuXuat { get; set; }
        public virtual ICollection<HhDvt> HhDvt { get; set; }
        public virtual ICollection<HinhAnhHangHoa> HinhAnhHangHoa { get; set; }
    }
}
