using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace VatTuYTeDanhMuc.Models.Entities
{
    public partial class ChucNang
    {
        public int Id { get; set; }
        public string MaCnang { get; set; }
        public bool? CaNhan { get; set; }
        public bool? Nhap { get; set; }
        public bool? Sua { get; set; }
        public bool? Xoa { get; set; }
        public bool? In { get; set; }
        public bool? Xuat { get; set; }
        public int? Nvtao { get; set; }
        public DateTime? NgayTao { get; set; }
        public int? Nvsua { get; set; }
        public DateTime? NgaySua { get; set; }
        public bool? Active { get; set; }
        public int? Idvt { get; set; }
        public int? Idcn { get; set; }
        public int? Idmenu { get; set; }

        public virtual Menu IdmenuNavigation { get; set; }
        public virtual VaiTro IdvtNavigation { get; set; }
    }
}
