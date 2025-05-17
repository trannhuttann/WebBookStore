using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq;
using WebBookStore.Models;
using WebBookStore.Controllers;
using System.Net;

namespace WebBookStore.Controllers
{
    public class NguoidungController : Controller
    {
        //Tọa dôi tượng quản lý
        dbQLBookstore db = new dbQLBookstore();
        // GET: Nguoidung
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Dangky()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Dangky(FormCollection collection, KHACHHANG kh)
        {
            // Lấy giá trị từ form
            var hoten = collection["HotenKH"];
            var tendn = collection["TenDN"];
            var matkhau = collection["Matkhau"];
            var matkhaunhaplai = collection["Matkhaunhaplai"];
            var email = collection["Email"];
            var diachi = collection["DiaChi"];
            var dienthoai = collection["DienThoai"];
            var ngaysinh = collection["Ngaysinh"];

            // Kiểm tra dữ liệu nhập vào
            if (String.IsNullOrEmpty(hoten))
            {
                ViewData["Loi1"] = "Họ tên khách hàng không được để trống";
            }
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi2"] = "Tên đăng ký không được để trống";
            }
            if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi3"] = "Mật khẩu không được để trống";
            }
            if (matkhau != matkhaunhaplai)
            {
                ViewData["Loi4"] = "Mật khẩu nhập lại không khớp";
            }
            if (String.IsNullOrEmpty(email))
            {
                ViewData["Loi5"] = "Email không được để trống";
            }
            if (String.IsNullOrEmpty(dienthoai))
            {
                ViewData["Loi6"] = "Điện thoại không được để trống";
            }

            // Nếu không có lỗi, lưu dữ liệu vào cơ sở dữ liệu
            if (ModelState.IsValid && ViewData["Loi1"] == null && ViewData["Loi2"] == null && ViewData["Loi3"] == null && ViewData["Loi4"] == null && ViewData["Loi5"] == null && ViewData["Loi6"] == null)
            {
                try
                {
                    // Kiểm tra xem tài khoản đã tồn tại chưa
                    var existingUser = db.KHACHHANGs.SingleOrDefault(n => n.TaiKhoan == tendn);
                    if (existingUser != null)
                    {
                        ViewData["Loi2"] = "Tên đăng ký đã tồn tại. Vui lòng chọn tên khác.";
                        return View();
                    }

                    kh.HoTen = hoten;
                    kh.TaiKhoan = tendn;
                    kh.MatKhau = matkhau;
                    kh.Email = email;
                    kh.DiachiKH = diachi;
                    kh.DienThoaiKH = dienthoai;
                    kh.NgaySinh = DateTime.Parse(ngaysinh);

                    // Thêm khách hàng vào cơ sở dữ liệu
                    db.KHACHHANGs.Add(kh);
                    db.SaveChanges();

                    // Đặt thông báo thành công
                    TempData["SuccessMessage"] = "Đăng ký thành công!";
                    return RedirectToAction("DangNhap");
                }
                catch (Exception ex)
                {
                    // Xử lý lỗi
                    ViewBag.ErrorMessage = "Đã xảy ra lỗi khi lưu dữ liệu: " + ex.Message;
                }
            }

            // Trả về view nếu có lỗi
            return View();
        }



        [HttpGet]
        public ActionResult Dangnhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Dangnhap(FormCollection collection)
        {
            var tendn = collection["TenDN"];
            var matkhau = collection["Matkhau"];
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Tên đăng nhập không được để trống";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Nhập mật khẩu vào";
            }
            else
            {
                // Gán giá trị vào
                KHACHHANG kh = db.KHACHHANGs.SingleOrDefault(n => n.TaiKhoan == tendn && n.MatKhau == matkhau);
                if (kh != null)
                {
                    ViewBag.ThongBao = "Bạn đã đăng nhập thành công";
                    Session["TaiKhoan"] = kh;
                    ViewBag.Username = kh.HoTen;
                    return RedirectToAction("Index", "BookStore");
                }
                else
                    ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return View();
        }
    

    // GET: Nguoidung/QuenMatKhau
    [HttpGet]
        public ActionResult QuenMatKhau()
        {
            return View();
        }
        public ActionResult Logout()
        {
            // Xóa session hoặc cookie đăng nhập
            Session.Clear();
            Session.Abandon();

            // Chuyển hướng về trang chính hoặc trang đăng nhập
            return RedirectToAction("Index", "Home");
        }
        // POST: Nguoidung/QuenMatKhau
        [HttpPost]
        public ActionResult QuenMatKhau(FormCollection collection)
        {
            var email = collection["Email"];
            if (String.IsNullOrEmpty(email))
            {
                ViewData["Loi"] = "Email không được để trống";
            }
            else
            {
                var kh = db.KHACHHANGs.SingleOrDefault(n => n.Email == email);
                if (kh != null)
                {
                    // Gửi email với mật khẩu (hoặc liên kết để đặt lại mật khẩu)
                    // Ví dụ gửi mật khẩu qua email (cần cấu hình SMTP)
                    string subject = "Khôi phục mật khẩu";
                    string body = $"Mật khẩu của bạn là: {kh.MatKhau}";

                    // Sử dụng phương thức gửi email (phương thức gửi email cụ thể sẽ phụ thuộc vào cấu hình của bạn)
                    // EmailHelper.SendEmail(email, subject, body);

                    ViewBag.ThongBao = "Mật khẩu của bạn đã được gửi đến email của bạn.";
                }
                else
                {
                    ViewData["Loi"] = "Email không tồn tại trong hệ thống";
                }
            }
            return View();
        }
     


    }
}

//zzzzzzzzzziiiiiiiiiiiiiiiimmmmmmmmmmmmmmmmmmm