using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace VatTuYTeDanhMuc.Models.Entities
{
    public partial class VaiTro
    {
        public VaiTro()
        {
            ChucNang = new HashSet<ChucNang>();
        }

        public int Id { get; set; }
        public string MaVt { get; set; }
        public string TenVt { get; set; }
        public int? Idcn { get; set; }
        public int? Idtk { get; set; }
        public int? Nvtao { get; set; }
        public DateTime? NgayTao { get; set; }
        public int? Nvsua { get; set; }
        public DateTime? NgaySua { get; set; }
        public bool? Active { get; set; }

        public virtual ChiNhanh IdcnNavigation { get; set; }
        public virtual TaiKhoan IdtkNavigation { get; set; }
        public virtual ICollection<ChucNang> ChucNang { get; set; }
    }
}
