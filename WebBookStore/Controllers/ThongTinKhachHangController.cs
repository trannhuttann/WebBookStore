using System;
using System.Linq;
using System.Web.Mvc;
using WebBookStore.Models;

namespace WebBookStore.Controllers
{
    public class ThongTinKhachHangController : Controller
    {
        private dbQLBookstore db = new dbQLBookstore();

        // GET: ThongTinKhachHang
        public ActionResult Index()
        {
            // Kiểm tra xem người dùng đã đăng nhập chưa
            var kh = Session["TaiKhoan"] as KHACHHANG;
            if (kh == null)
            {
                // Nếu chưa đăng nhập, chuyển hướng về trang đăng nhập
                return RedirectToAction("DangNhap", "Nguoidung");
            }

            // Tìm thông tin khách hàng từ cơ sở dữ liệu
            var thongTinKhachHang = db.KHACHHANGs.SingleOrDefault(n => n.MaKH == kh.MaKH);
            if (thongTinKhachHang == null)
            {
                // Nếu không tìm thấy thông tin, có thể thông báo lỗi hoặc chuyển hướng
                return HttpNotFound();
            }

            // Trả về view và truyền thông tin khách hàng vào view
            return View(thongTinKhachHang);
        }
        // GET: ThongTinKhachHang/Edit
        public ActionResult Edit()
        {
            // Kiểm tra xem người dùng đã đăng nhập chưa
            var kh = Session["TaiKhoan"] as KHACHHANG;
            if (kh == null)
            {
                // Nếu chưa đăng nhập, chuyển hướng về trang đăng nhập
                return RedirectToAction("DangNhap", "Nguoidung");
            }

            // Tìm thông tin khách hàng từ cơ sở dữ liệu
            var thongTinKhachHang = db.KHACHHANGs.SingleOrDefault(n => n.MaKH == kh.MaKH);
            if (thongTinKhachHang == null)
            {
                // Nếu không tìm thấy thông tin, có thể thông báo lỗi hoặc chuyển hướng
                return HttpNotFound();
            }

            // Trả về view và truyền thông tin khách hàng vào view
            return View(thongTinKhachHang);
        }
        // POST: ThongTinKhachHang/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(KHACHHANG model)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem người dùng đã đăng nhập chưa
                var kh = Session["TaiKhoan"] as KHACHHANG;
                if (kh == null)
                {
                    // Nếu chưa đăng nhập, chuyển hướng về trang đăng nhập
                    return RedirectToAction("DangNhap", "Nguoidung");
                }

                // Tìm thông tin khách hàng từ cơ sở dữ liệu
                var thongTinKhachHang = db.KHACHHANGs.SingleOrDefault(n => n.MaKH == kh.MaKH);
                if (thongTinKhachHang == null)
                {
                    // Nếu không tìm thấy thông tin, có thể thông báo lỗi hoặc chuyển hướng
                    return HttpNotFound();
                }

                // Cập nhật thông tin khách hàng
                thongTinKhachHang.HoTen = model.HoTen;
                thongTinKhachHang.Email = model.Email;
                thongTinKhachHang.DiachiKH = model.DiachiKH;
                thongTinKhachHang.DienThoaiKH = model.DienThoaiKH;
                thongTinKhachHang.NgaySinh = model.NgaySinh;

                // Lưu thay đổi vào cơ sở dữ liệu
                db.SaveChanges();

                // Cập nhật thông tin khách hàng trong session
                Session["TaiKhoan"] = thongTinKhachHang;

                // Chuyển hướng về trang thông tin khách hàng
                return RedirectToAction("Index");
            }

            // Nếu có lỗi, hiển thị lại form chỉnh sửa
            return View(model);
        }


    }

}
