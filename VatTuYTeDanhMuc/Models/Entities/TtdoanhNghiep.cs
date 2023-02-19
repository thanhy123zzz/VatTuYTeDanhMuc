using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace VatTuYTeDanhMuc.Models.Entities
{
    public partial class TtdoanhNghiep
    {
        public int Id { get; set; }
        public string Ten { get; set; }
        public string DiaChi { get; set; }
        public string Sdt { get; set; }
        public string Email { get; set; }
        public string MaSoThue { get; set; }
        public string Stk { get; set; }
        public string TenNh { get; set; }
        public string ChuTk { get; set; }
    }
}
