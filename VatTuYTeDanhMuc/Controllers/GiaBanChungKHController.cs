using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using VatTuYTeDanhMuc.Models.Entities;

namespace VatTuYTeDanhMuc.Controllers
{
	public class GiaBanChungKHController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}

        //hiển thị giao diện giá bán theo khách hàng
		public IActionResult table()
		{
            ViewData["Title"] = "Giá bán theo khách hàng";
            return View("TableGiaBanChungKH");
		}

        //load table giá bán chung theo khách hàng(sau khi chọn nhóm hàng hóa và khách hàng)
        [HttpPost("/loadTableGiaBanChungKH")]
        public IActionResult loadTableGiaBanChungKH(int nhomHH, int idkh)
        {
            webContext context = new webContext();
            ViewBag.HangHoa = context.HangHoa.Where(x => x.Idnhh == nhomHH &&x.Active == true);
            ViewBag.KH = context.GiaTheoKhachHang.Where(x => x.Idkh == idkh);
            ViewBag.IDKH = idkh;
            ViewBag.IDNHH = nhomHH;
            return PartialView();
        }

        //load table giá theo khách hàng (sau khi nhấn chọn hàng hóa cần xem)
        [HttpPost("/loadTableGTKH")]
        public IActionResult loadTableGTKH(int idkh, int iddvt, int idhh)
        {
            webContext context = new webContext();

            ViewBag.GTKH = context.GiaTheoKhachHang.FirstOrDefault(x => x.Idkh == idkh && x.Idhh == idhh && x.Iddvt == iddvt);
            var Idkh = context.GiaTheoKhachHang.Select(x => x.Idkh == idkh);
            var Idhh = context.GiaTheoKhachHang.Select(x => x.Idhh == idhh);
            var Iddvt = context.GiaTheoKhachHang.Select(x => x.Iddvt == iddvt);
            if (ViewBag.GTKH == null)
            {
                ViewBag.idkh = null;
                ViewBag.iddvt = null;
                ViewBag.idhh = null;
            }else
            ViewBag.idkh = idkh;
            ViewBag.iddvt = iddvt;
            ViewBag.idhh = idhh;
            return PartialView();
        }
        //hiện dòng sau khi nhấn nút sửa cho đơn vị tính chính
        [HttpPost("/addNewRowGTKH")]
        public IActionResult addNewRowGTKH(int id)
        {
            webContext context = new webContext();
            
            if(id == 0)
            {
                return ViewBag.ID = null;
            }
            else
            ViewBag.GTKH = context.GiaTheoKhachHang.Find(id);
            ViewBag.ID = id; 
            return PartialView();
        }
        //hiện dòng sau khi nhấn nút sửa cho các đơn vị tính phụ
        [HttpPost("/addNewRowGTKH1")]
        public IActionResult addNewRowGTKH1(int id)
        {
            webContext context = new webContext();

            if (id == 0)
            {
                return ViewBag.ID = null;
            }
            else
                ViewBag.GTKH = context.GiaTheoKhachHang.Find(id);
            ViewBag.ID = id;
            return PartialView();
        }

        //cập nhập lại giá khách hàng theo đơn vị tính 
        [HttpPost("/updateGTKH")]
        public string updateGTKH(float giabansi, float giabanle, float tilesi, float tilele, int id)
        {
            webContext context = new webContext();
            GiaTheoKhachHang h = context.GiaTheoKhachHang.Find(id);
            int idUser = int.Parse(User.Claims.ElementAt(2).Type);
            h.Nvsua = idUser;
            h.NgaySua = DateTime.Now;
            h.TiLeLe = Math.Round(tilele,2);
            h.TiLeSi = Math.Round(tilesi,2);
            h.GiaBanLe = giabanle;
            h.GiaBanSi = giabansi;
            h.Active = true;

            context.GiaTheoKhachHang.Update(h);
            context.SaveChanges();
            return "Sửa thành công";
        }
        //thêm tỉ lệ, giá bán cho khách hàng 
        [HttpPost("/AddGTKH")]
        public string addGTKH(float giabansi, float giabanle, float tilesi, float tilele, int idhh, int iddvt, int idkh)
        {
            webContext context = new webContext();
            GiaTheoKhachHang h = new GiaTheoKhachHang();
            int idUser = int.Parse(User.Claims.ElementAt(2).Type);
            int idCN = int.Parse(User.Claims.ElementAt(4).Value);
            h.Idcn = idCN; 
            h.Nvtao = idUser;
            h.NgayTao = DateTime.Now;
            h.TiLeLe = Math.Round(tilele,2);
            h.TiLeSi = Math.Round(tilesi,2);
            h.GiaBanLe = giabanle;
            h.GiaBanSi = giabansi;
            h.Idhh = idhh;
            h.Iddvt = iddvt;
            h.Idkh = idkh;
            h.Active = true;

            context.GiaTheoKhachHang.Add(h);
            context.SaveChanges();
            return "Thêm thành công";
        }
       // Hiển thị bảng đơn vị tính phụ
        [HttpPost("/loadTableGBCKH_DVT")]
        public IActionResult loadTableGBCKH_DVT(int idhh)
        {

            webContext context = new webContext();
            ViewBag.hhdvt = context.HhDvt.Where(x => x.Idhh == idhh && x.Active == true);
            return PartialView();
        }
        [HttpPost("/Viewnull")]
        public IActionResult Viewnull()
        {
            return PartialView();
        }

        //hiển thị bảng giá theo khách hàng theo các đơn vị tính phụ
        
        [HttpPost("/loadTableGTKH_DVT")]
        public IActionResult loadTableGTKH_DVT(int idkh, int iddvt, int idhh)
        {
            webContext context = new webContext();

            ViewBag.GTKH = context.GiaTheoKhachHang.FirstOrDefault(x => x.Idkh == idkh && x.Idhh == idhh && x.Iddvt == iddvt && x.Active == true);
            var Idkh = context.GiaTheoKhachHang.Select(x => x.Idkh == idkh);
            var Idhh = context.GiaTheoKhachHang.Select(x => x.Idhh == idhh);
            var Iddvt = context.GiaTheoKhachHang.Select(x => x.Iddvt == iddvt);
            if (ViewBag.GTKH == null)
            {
                ViewBag.idkh = null;
                ViewBag.iddvt = null;
                ViewBag.idhh = null;
            }
            else
                ViewBag.idkh = idkh;
            ViewBag.iddvt = iddvt;
            ViewBag.idhh = idhh;
            return PartialView();
        }
        //xóa tỉ lệ, giá bán đã được áp dụng cho khách
        [HttpPost("/Delete")]
        public string Delete(int id)
        {
            webContext context = new webContext();
            GiaTheoKhachHang h = context.GiaTheoKhachHang.Find(id);
            int idUser = int.Parse(User.Claims.ElementAt(2).Type);
            h.NgaySua = DateTime.Now;
            h.Nvsua = idUser;
            h.Active = false;
            context.Update(h);
            context.SaveChanges();
          
            return "Xóa thành công";
        }
        //tìm kiếm hàng hóa
        [HttpPost("/searchTableGBCKH")]
        public IActionResult searchTableGBCKH(int nhomHH, string key, int nhomKH)
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
            ViewBag.IDKH = nhomKH;
            return PartialView("loadTableGiaBanChungKH");
        }
    }
}
