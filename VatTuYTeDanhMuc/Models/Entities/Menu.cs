using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace VatTuYTeDanhMuc.Models.Entities
{
    public partial class Menu
    {
        public Menu()
        {
            ChucNang = new HashSet<ChucNang>();
        }

        public int Id { get; set; }
        public string MaMenu { get; set; }
        public string TenMenu { get; set; }
        public int? Nvtao { get; set; }
        public DateTime? NgayTao { get; set; }
        public int? Nvsua { get; set; }
        public DateTime? NgaySua { get; set; }
        public bool? Active { get; set; }
        public int? Idphan { get; set; }

        public virtual Phan IdphanNavigation { get; set; }
        public virtual ICollection<ChucNang> ChucNang { get; set; }
    }
}
