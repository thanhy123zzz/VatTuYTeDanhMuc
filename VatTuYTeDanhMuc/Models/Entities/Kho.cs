﻿using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace VatTuYTeDanhMuc.Models.Entities
{
    public partial class Kho
    {
        public Kho()
        {
            PhieuXuatNoiBo = new HashSet<PhieuXuatNoiBo>();
        }

        public int Id { get; set; }
        public string MaKho { get; set; }
        public string TenKho { get; set; }
        public string Sdt { get; set; }
        public string DiaChi { get; set; }
        public string Email { get; set; }
        public string GhiChu { get; set; }
        public int? Nvtao { get; set; }
        public DateTime? NgayTao { get; set; }
        public int? Nvsua { get; set; }
        public DateTime? NgaySua { get; set; }
        public bool? Active { get; set; }

        public virtual ICollection<PhieuXuatNoiBo> PhieuXuatNoiBo { get; set; }
    }
}