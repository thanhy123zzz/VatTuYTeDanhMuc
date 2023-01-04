using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace VatTuYTeDanhMuc.Models.Entities
{
    public partial class SoThuTu
    {
        public int Id { get; set; }
        public DateTime? Ngay { get; set; }
        public string Loai { get; set; }
        public int? Stt { get; set; }
    }
}
