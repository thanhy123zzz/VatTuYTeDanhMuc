using System.Collections.Generic;
using VatTuYTeDanhMuc.Models.Entities;

namespace VatTuYTeDanhMuc.Models
{
    public class PhieuNhapKhoModel
    {
        public PhieuNhap phieuNhap { get; set; }
        public List<ChiTietPhieuNhap> chiTietPhieuNhaps { get; set; }
        public List<ChiTietTraNo> chiTietTraNos { get; set; }
    }
}
