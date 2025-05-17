using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBookStore.Models;
using PagedList;
using PagedList.Mvc;
using System.IO;
using System.Data.Entity;
using System.Web.Security;

namespace WebBookStore.Controllers
{
    public class AdminController : Controller
    {
        //Tọa dôi tượng quản lý
        dbQLBookstore db = new dbQLBookstore();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        //public ActionResult Sach(int? page)
        //{
        //    int pageNumber = (page ?? 1);
        //    int pageSize = 10;
        //    return View(db.SACHes.ToList().OrderBy(n => n.MaSach).ToPagedList(pageNumber, pageSize));
        //}
        public ActionResult Sach(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 10;
            var sachList = db.SACHes.ToList().OrderBy(n => n.MaSach).ToPagedList(pageNumber, pageSize);
            return View(sachList);
        }
        [HttpPost]
        public ActionResult Logout()
        {
            // Xóa thông tin phiên làm việc của người dùng
            Session.Clear();

            // Đăng xuất người dùng
            FormsAuthentication.SignOut();

            // Chuyển hướng về trang đăng nhập
            return RedirectToAction("login", "Admin");
        }

        // GET: Admin
        public ActionResult login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult login(FormCollection collection)
        {
            var tendn = collection["username"];
            var matkhau = collection["password"];
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Tên đăng không được để trống";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Mật khẩu không được để trống";
            }
            else
            {
                //gán giá trị vào
                ADMIN admin = db.ADMINs.SingleOrDefault(n => n.UserAdmin == tendn && n.PassAdmin == matkhau);
                if (admin != null)
                {
                    ViewBag.ThongBao = "Bạn đã đăng nhập thành công";
                    Session["TaiKhoan"] = admin;
                    return RedirectToAction("Index", "Admin");
                }
                else
                    ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return View();
        }
        [HttpGet]
        public ActionResult ThemmoiSach()
        {
            ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChude");
            ViewBag.MaNXB = new SelectList(db.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemmoiSach(SACH sach, HttpPostedFileBase fileupload)
        {
            ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChude");
            ViewBag.MaNXB = new SelectList(db.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");
            if (fileupload == null)
            {
                ViewBag.Thongbao = "Chọn ảnh thêm vào";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    //Luu teen file
                    var fileName = Path.GetFileName(fileupload.FileName);
                    //luu dương dẫn
                    var path = Path.Combine(Server.MapPath("~/product_imgs"), fileName);
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    }
                    else
                    {
                        fileupload.SaveAs(path);
                    }
                    sach.AnhBia = fileName;
                    //luu file
                    db.SACHes.Add(sach);
                    db.SaveChanges();

                }
                return RedirectToAction("Sach");
            }

        }
        [HttpGet]
        public ActionResult Xoasach(int id)
        {
            //lấy dối tượng cần xóa
            SACH sach = db.SACHes.SingleOrDefault(n => n.MaSach == id);
            ViewBag.Masach = sach.MaSach;
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sach);
        }
        [HttpPost, ActionName("Xoasach")]
        public ActionResult Xacnhanxoa(int id)
        {
            //lấy dối tượng cần xóa
            SACH sach = db.SACHes.SingleOrDefault(n => n.MaSach == id);
            ViewBag.Masach = sach.MaSach;
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.SACHes.Remove(sach);
            db.SaveChanges();
            return RedirectToAction("Sach");
        }
        [HttpGet]
        public ActionResult Suasach(int id)
        {
            //lấy dối tượng cần xóa
            SACH sach = db.SACHes.SingleOrDefault(n => n.MaSach == id);
            ViewBag.Masach = sach.MaSach;
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChude", sach.MaCD);
            ViewBag.MaNXB = new SelectList(db.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB", sach.MaNXB);
            return View(sach);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SuaSach(SACH sach, HttpPostedFileBase fileupload)
        {
            ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChude");
            ViewBag.MaNXB = new SelectList(db.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");
            if (fileupload == null)
            {
                ViewBag.Thongbao = "Chọn ảnh thêm vào";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    //Luu teen file
                    var fileName = Path.GetFileName(fileupload.FileName);
                    //luu dương dẫn
                    var path = Path.Combine(Server.MapPath("~/product_imgs"), fileName);
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    }
                    else
                    {
                        fileupload.SaveAs(path);
                    }
                    sach.AnhBia = fileName;
                    //luu file
                    UpdateModel(sach);
                    db.SaveChanges();

                }
                return RedirectToAction("Sach");
            }

        }
        ////////chi tiết sách
        public ActionResult chitietSACH(int id)
        {
            // Retrieve the book by its ID
            SACH sach = db.SACHes.SingleOrDefault(n => n.MaSach == id);
            if (sach == null)
            {
                // If no book is found, return a 404 status code
                Response.StatusCode = 404;
                return null;
            }
            return View(sach);
        }

    }
}
/////////////////////////zzzzzzzzzzimmmmmmmmmmmmmmmmmmmmmmmm