using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VatTuYTeDanhMuc.Models.Entities;

namespace VatTuYTeDanhMuc.Models
{
  public class PhieuTraNoModel
  {
    public PhieuTraNo phieuTraNo { get; set; }

    public NccNganHang nccNganHang { get; set; }

    public Httt httt { get; set; }

    public List<ChiTietTraNo> chiTietTraNos { get; set; }
  }
}
