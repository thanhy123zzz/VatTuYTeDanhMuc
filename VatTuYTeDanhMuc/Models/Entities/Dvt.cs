﻿using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace VatTuYTeDanhMuc.Models.Entities
{
    public partial class Dvt
    { 
        public Dvt()
        {
            ChiTietPhieuXuat = new HashSet<ChiTietPhieuXuat>();
            ChiTietPhieuXuatNoiBo = new HashSet<ChiTietPhieuXuatNoiBo>();
            ChiTietPhieuXuatTamNoiBo = new HashSet<ChiTietPhieuXuatTamNoiBo>();
            GiaTheoKhachHang = new HashSet<GiaTheoKhachHang>();
            HangHoa = new HashSet<HangHoa>();
            HhDvt = new HashSet<HhDvt>();
        }

        public int Id { get; set; }
        public string MaDvt { get; set; }
        public string TenDvt { get; set; }
        public int? Nvtao { get; set; }
        public DateTime? NgayTao { get; set; }
        public int? Nvsua { get; set; }
        public DateTime? NgaySua { get; set; }
        public bool? Active { get; set; }
        public int? Idcn { get; set; }

        public virtual ICollection<ChiTietPhieuXuat> ChiTietPhieuXuat { get; set; }
        public virtual ICollection<ChiTietPhieuXuatNoiBo> ChiTietPhieuXuatNoiBo { get; set; }
        public virtual ICollection<ChiTietPhieuXuatTamNoiBo> ChiTietPhieuXuatTamNoiBo { get; set; }
        public virtual ICollection<GiaTheoKhachHang> GiaTheoKhachHang { get; set; }
        public virtual ICollection<HangHoa> HangHoa { get; set; }
        public virtual ICollection<HhDvt> HhDvt { get; set; }
    }
}
