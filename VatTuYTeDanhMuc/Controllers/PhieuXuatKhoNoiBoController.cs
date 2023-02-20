using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using VatTuYTeDanhMuc.Models.Entities;

namespace VatTuYTeDanhMuc.Controllers
{
	public class PhieuXuatKhoNoiBoController : Controller
	{
        [Route("/PhieuXuatKhoNoiBo")]

        public IActionResult Index()
        {
            return View("PhieuXuatKhoNoiBo");
        }
        [HttpPost("/loadListDVT_NB")]
        public IActionResult loadListDVT_NB(int idHH)
        {
            webContext context = new webContext();
            double? GiaBan;

            HangHoa hh = context.HangHoa.Include(x => x.IddvtchinhNavigation).FirstOrDefault(x => x.Id == idHH);

            var dvt = context.HhDvt.Include(x => x.IddvtNavigation).Where(x => x.Idhh == idHH).ToList();

            //Lấy các đơn vị tính của Hàng hoá
            string options = "<option selected value = '" + hh.Iddvtchinh + "'>" + hh.IddvtchinhNavigation.TenDvt + "</option>";
            foreach (HhDvt d in dvt)
            {
                options += "<option value = '" + d.Iddvt + "'>" + d.IddvtNavigation.TenDvt + "</option>";
            }

            //lấy giá theo khách hàng
           

            // lấy giá nhập của hàng hoá
            SoLuongHhcon GiaHangTon;
            var cachXuat = context.CachXuat.Find(1);
            if (cachXuat.TheoTgnhap == true)
            {
                GiaHangTon = context.SoLuongHhcon
                    .Include(x => x.IdctpnNavigation)
                    .Where(x => x.IdctpnNavigation.Idhh == idHH)
                    .OrderBy(x => x.NgayNhap).
                    FirstOrDefault();
            }
            else
            {
                GiaHangTon = context.SoLuongHhcon
                    .Include(x => x.IdctpnNavigation)
                    .Where(x => x.IdctpnNavigation.Idhh == idHH)
                    .OrderBy(x => x.IdctpnNavigation.Hsd)
                    .FirstOrDefault();
            }
            if (GiaHangTon == null)
            {
                return Json(new
                {
                    options = options,
                    giaBan = 0,
                    slCon = getSLCon(idHH, (int)hh.Iddvtchinh),
                    hinhAnh = hh.Avatar
                });
            }


            return Json(new
            {
                options = options,
                giaBan = 0,
                slCon = getSLCon(idHH, (Int32)hh.Iddvtchinh),
                hinhAnh = hh.Avatar
            });

        }
        double? getSLCon(int idHH, int idDVT)
        {
            webContext context = new webContext();
            HangHoa hh = context.HangHoa.FirstOrDefault(x => x.Id == idHH);

            double? a = context.SoLuongHhcon.Include(x => x.IdctpnNavigation)
                .Where(x => x.IdctpnNavigation.Idhh == idHH)
                .Sum(x => x.Slcon);

            if (hh.Iddvtchinh == idDVT)
            {
                return a;
            }
            else
            {
                var sl = (double)context.HhDvt.Where(x => x.Idhh == idHH && x.Iddvt == idDVT).FirstOrDefault().SlquyDoi;
                return Math.Round(a.Value / sl, 2);
            }
        }
        double? getGiaBan(double? tiLe, double? GiaNhap, double? Vat)
        {
            double tl = Math.Round((tiLe.Value / 100) + 1, 2);
            double thue = Math.Round(Vat.Value / 100, 2);
            return Math.Round(tl * (GiaNhap.Value + (GiaNhap.Value * thue)), 2);
        }
    }
}
