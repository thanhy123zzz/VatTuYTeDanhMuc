﻿using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace VatTuYTeDanhMuc.Models.Entities
{
    public partial class KhachHangNganHang
    {
        public int Id { get; set; }
        public int? Idkh { get; set; }
        public int? Idnh { get; set; }
        public string Stk { get; set; }
        public int? Nvtao { get; set; }
        public DateTime? NgayTao { get; set; }
        public int? Nvsua { get; set; }
        public DateTime? NgaySua { get; set; }
        public bool? Active { get; set; }

        public virtual KhachHang IdkhNavigation { get; set; }
        public virtual NganHang IdnhNavigation { get; set; }
    }
}
