using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace VatTuYTeDanhMuc.Models.Entities
{
    public partial class QuyDinhMa
    {
        public int Id { get; set; }
        public int? DoDai { get; set; }
        public string TiepDauNgu { get; set; }
        public int? Idcn { get; set; }
    }
}
