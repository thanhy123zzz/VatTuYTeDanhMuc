using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using VatTuYTeDanhMuc.Models.Entities;

namespace VatTuYTeDanhMuc.Controllers
{
    public class GiaBanChungNHHController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult table()
        {
            ViewData["Title"] = "Cài đặt tỉ lệ theo nhóm hàng hóa";
            return View("tableGiaBanChungNHH");
        }
        [HttpPost("/addNewRowGBCNHH")]
        public IActionResult addNewRowGBCNHH(int id)
        {
            webContext context = new webContext();
            ViewBag.nhh = context.GiaTheoNhomHangHoa.FirstOrDefault(x =>x.Id == id && x.Active == true);

            return PartialView();
        }
        [HttpPost("/UpdateGBCNHH")]
        public string UpdateGBCNHH(float giatrimin, float giatrimax, float tilesi, float tilele, int id)
        {
            webContext context = new webContext();
            
            var ListGiaTheoNhomHh = context.GiaTheoNhomHangHoa.Where(x => x.Id == id && x.Active == true).ToList();
            if (giatrimin >= giatrimax)
            {
                return "Giá trị Max phải lớn hơn giá trị min";
            }
            foreach (GiaTheoNhomHangHoa g in ListGiaTheoNhomHh)
            {
                if ((giatrimin - g.Min >= 0 && giatrimin - g.Max <= 0) || (giatrimax - g.Max <= 0 && giatrimax - g.Min >= 0))
                {
                    return "Giá trị Min và Max không hợp lệ";
                }

            }
            GiaTheoNhomHangHoa h = context.GiaTheoNhomHangHoa.Find(id);
            int idUser = int.Parse(User.Claims.ElementAt(3).Type);
            h.Nvsua = idUser;
            h.NgaySua = DateTime.Now;
            h.Id = id;
            h.Min = giatrimin;
            h.Max = giatrimax;
            h.TiLeSi = Math.Round(tilesi,2);
            h.TiLeLe = Math.Round(tilele,2);

            context.GiaTheoNhomHangHoa.Update(h);
            context.SaveChanges();
            return "Sửa thành công";
        }
        [HttpPost("loadTableGBCNHH")]
        public IActionResult loadTableGBCNHH(int id)
        {
            webContext context = new webContext();
            ViewBag.nhh = context.NhomHangHoa.Where(x => x.Active == true);
            return PartialView();
        }

        [HttpPost("loadTableGTNHH")]
        public IActionResult loadTableGTNHH(int idnhh)
        {
            webContext context = new webContext();
            ViewBag.NHH = context.GiaTheoNhomHangHoa.Where(x =>x.Idnhh==idnhh && x.Active==true);
            ViewBag.idnhh = idnhh;
            return PartialView();
        }

        [HttpPost("addNewRowGBCNHH_NewRow")]
        public IActionResult addNewRowGBCNHH_NewRow(int idnhh)
        {
            ViewBag.idnhh = idnhh;
            return PartialView();
        }
        [HttpPost("addGBCNHH")]
        public string addGBCNHH(float max, float min, float tilesi, float tilele,int idnhh)
        {
            webContext context = new webContext();
            GiaTheoNhomHangHoa h = new GiaTheoNhomHangHoa();
            var ListGiaTheoNhomHh = context.GiaTheoNhomHangHoa.Where(x => x.Idnhh == idnhh && x.Active == true).ToList();
            if (min >= max)
            {
                return "Giá trị max phải lớn hơn giá trị min";
            }
            foreach (GiaTheoNhomHangHoa g in ListGiaTheoNhomHh){
                if((min - g.Min >= 0 && min - g.Max <= 0) || (max - g.Max <= 0 && max - g.Min >= 0)){
                    return "Giá trị Min và Max không hợp lệ";
                        }

            }
            int idUser = int.Parse(User.Claims.ElementAt(3).Type);
            h.Nvtao = idUser;
            h.NgayTao = DateTime.Now;
            h.Idnhh = idnhh;
                h.Max = max;
                h.Min = min;
                h.TiLeSi = Math.Round(tilesi,2);
                h.TiLeLe = Math.Round(tilele, 2);
               
                h.Active = true;
                context.GiaTheoNhomHangHoa.Add(h);
                context.SaveChanges();
                return "Thêm thành công";
            
        }
        [HttpPost("/deleteHGBCNHH")]
        public string deleteGBCNHH(int id)
        {
            webContext context = new webContext();
            GiaTheoNhomHangHoa h = context.GiaTheoNhomHangHoa.Find(id);
            int idUser = int.Parse(User.Claims.ElementAt(3).Type);
            h.NgaySua = DateTime.Now;
            h.Nvsua = idUser;
            h.Active = false;
            context.Update(h);
            context.SaveChanges();
            return "Xoá thành công!";
        }
        [HttpPost("/searchTableGBCNHH")]
        public IActionResult searchTableGBCNHH( string key)
        {
            webContext context = new webContext();
            
            
                ViewBag.NHH = context.NhomHangHoa.FromSqlRaw("select*from nhomhanghoa where concat_ws(' ',MaNHH,TenNHH) LIKE N'%" + key + "%' and active='true';").ToList();
            
            
            
            return PartialView("loadTableNHH");
        }
    }
}
