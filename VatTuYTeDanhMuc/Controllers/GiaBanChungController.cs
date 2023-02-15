using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using VatTuYTeDanhMuc.Models.Entities;

namespace VatTuYTeDanhMuc.Controllers
{
    public class GiaBanChungController : Controller
    {
        //hiển thị giao diện giá bán, tỉ lệ chung
        public IActionResult table()
        {
            ViewData["title"] = "Tỉ lệ giá bán chung ";
            webContext context = new webContext();
            TempData["Menu"] = context.Menu.Where(x => x.MaMenu == "GiaBanChung").FirstOrDefault().Id;
            return View("TableGiaBanChung");
        }
        //hiển thị table hàng hóa khi chọn nhóm hàng hóa 
        [HttpPost("/loadTableGiaBanChung")]
        public IActionResult loadTableGiaBanChung(int nhomHH)
        {
            webContext context = new webContext();
            ViewBag.HangHoa = context.HangHoa.Where(x => x.Idnhh == nhomHH && x.Active == true);
            return PartialView();
        }
        //hiển thị dòng sửa đơn vị tính chính khi nhấn nút sửa
        [HttpPost("/addNewRowHH")]
        public IActionResult addNewRowHH(int idDVT, int sl, int id)
        {
            webContext context = new webContext();
            if (idDVT == 0)
                ViewBag.idHH = null;
            else
            {
                ViewBag.idHH = idDVT;
            }
            ViewBag.idHH = idDVT;
            ViewBag.ID = id;
            ViewBag.HH = context.HangHoa.Find(id);
            return PartialView();
        }
        //tìm kiếm hàng hóa
        [HttpPost("/searchTableGBC")]
        public IActionResult searchTableGBC(int nhomHH, string key)
        {
            webContext context = new webContext();
            if (nhomHH == 0)
            {
                ViewBag.HangHoa = context.HangHoa.FromSqlRaw("select*from HangHoa where concat_ws(' ',MaHH,TenHH) LIKE N'%" + key + "%' and active='true';").ToList();
            }
            else
            {
                ViewBag.HangHoa = context.HangHoa.FromSqlRaw("select*from HangHoa where IdNhh = '" + nhomHH + "' and concat_ws(' ',MaHH,TenHH) LIKE N'%" + key + "%' and active='true';").ToList();
            }
            return PartialView("loadTableGiaBanChung");
        }
        //hiển thị dòng sửa đơn vị tính phụ khi nhấn nút sửa
        [HttpPost("/addNewRowHHDVT")]
        public IActionResult addNewRowHHDVT(int idDVT, int sl, int id)
        {
            webContext context = new webContext();
            if (idDVT == 0)
                ViewBag.idHH = null;
            else
            {
                ViewBag.idHH = idDVT;
            }
            ViewBag.idHH = idDVT;
            ViewBag.ID = id;
            ViewBag.HHdvt = context.HhDvt.Find(id);
            return PartialView();
        }

        //cập nhập đơn vị tính chính
        [HttpPost("/updateTLHH")]
        public string updateTLHH(float giabansi, float giabanle, float tilesi, float tilele, int id)
        {
            webContext context = new webContext();
            HangHoa h = context.HangHoa.Find(id);
            int idUser = int.Parse(User.Claims.ElementAt(2).Type);
            h.Nvsua = idUser;
            h.NgaySua = DateTime.Now;
            h.TiLeLe = Math.Round(tilele, 2);
            h.TiLeSi = Math.Round(tilesi, 2);
            h.GiaBanLe = giabanle;
            h.GiaBanSi = giabansi;

            context.HangHoa.Update(h);
            context.SaveChanges();
            return "Sửa thành công ";
        }
        //cập nhập đơn vị tính phụ
        [HttpPost("/updateTLHH_DVT")]
        public string updateTLHH_DVT(float giabansi, float giabanle, float tilesi, float tilele, int id, int sl)
        {
            webContext context = new webContext();
            HhDvt h = context.HhDvt.Find(id);
            int idUser = int.Parse(User.Claims.ElementAt(2).Type);
            h.Nvsua = idUser;
            h.NgaySua = DateTime.Now;
            h.TiLeLe = Math.Round(tilele, 2);
            h.TiLeSi = Math.Round(tilesi, 2);
            h.GiaBanLe = giabanle;
            h.GiaBanSi = giabansi;
            h.SlquyDoi = sl;

            context.HhDvt.Update(h);
            context.SaveChanges();
            return "Sửa thành công";
        }

        //Thêm đơn vị tính phụ
        [HttpPost("/addTLHH_DVT")]
        public string addTLHH_DVT(float giabansi, float giabanle, float tilesi, float tilele, float sl, int idhh, int iddvt)
        {
            webContext context = new webContext();
            HhDvt h = new HhDvt();
            int idUser = int.Parse(User.Claims.ElementAt(2).Type);
            int idCN = int.Parse(User.Claims.ElementAt(4).Value);
            var hhdvt = context.HhDvt.FirstOrDefault(x => x.Idhh == idhh && x.Iddvt == iddvt && x.Active == true);
            var hhdvt1 = context.HangHoa.FirstOrDefault(x => x.Id == idhh && x.Iddvtchinh == iddvt && x.Active == true);
            if (sl == 0)
            {
                return "Số lượng quy đổi không hợp lệ";
            }
            if (hhdvt != null || hhdvt1 != null)
            {
                return "Đơn vị tính này đã được sử dụng";

            }
            else

                h.Nvtao = idUser;
            h.Idhh = idhh;
            h.Iddvt = iddvt;
            h.SlquyDoi = sl;
            h.NgayTao = DateTime.Now;
            h.TiLeLe = Math.Round(tilele, 2);
            h.TiLeSi = Math.Round(tilesi, 2);
            h.GiaBanLe = giabanle;
            h.GiaBanSi = giabansi;

            context.HhDvt.Add(h);
            context.SaveChanges();
            return "Thêm thành công";
        }

        [HttpPost("/loadTableGiaBanChungDVT")]
        public IActionResult loadTableGiaBanChungDVT(int idhh)
        {
            webContext context = new webContext();
            ViewBag.HHDVT = context.HhDvt.Where(x => x.Idhh == idhh && x.Active == true);
            ViewBag.ID = idhh;
            return PartialView();
        }

        //hiển thị table các đơn vị tính phụ sau khi nhấn nút more
        [HttpPost("/loadTableGBC_DVT")]
        public IActionResult loadTableGBC_DVT(int idhh)
        {
            webContext context = new webContext();
            ViewBag.HHDVT = context.HhDvt.Where(x => x.Idhh == idhh && x.Active == true);
            ViewBag.IDNHH = context.HangHoa.Find(idhh).Idnhh;
            ViewBag.ID = idhh;
            return PartialView();
        }
        //hiện dòng mới để thêm đơn vị tính phụ
        [HttpPost("/addNewRowHH_DVT_GBC")]
        public IActionResult addNewRowHH_DVT_GBC(int idhh)
        {
            webContext context = new webContext();
            ViewBag.IDHH = idhh;
            return PartialView();
        }

        //xóa đơn vị tính phụ
        [HttpPost("/deleteHHDVT")]
        public string deleteHHDVT(int id)
        {
            webContext context = new webContext();
            HhDvt h = context.HhDvt.Find(id);
            int idUser = int.Parse(User.Claims.ElementAt(3).Type);
            h.NgaySua = DateTime.Now;
            h.Nvsua = idUser;
            h.Active = false;
            context.Update(h);
            context.SaveChanges();
            return "Xoá thành công!";
        }

    }
}
