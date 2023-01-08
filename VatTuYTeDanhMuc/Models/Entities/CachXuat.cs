﻿using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace VatTuYTeDanhMuc.Models.Entities
{
    public partial class CachXuat
    {
        public int Id { get; set; }
        public bool? TheoHsd { get; set; }
        public bool? TheoTgnhap { get; set; }
    }
}
