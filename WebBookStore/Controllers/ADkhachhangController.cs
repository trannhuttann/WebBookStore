using System;
using System.Linq;
using System.Web.Mvc;
using WebBookStore.Models;

namespace WebBookStore.Controllers
{
    public class ADKhachHangController : Controller
    {
        private dbQLBookstore DbContext = new dbQLBookstore();

        // GET: ADKhachHang/Index
        public ActionResult Index()
        {
            var khachHangs = DbContext.KHACHHANGs.ToList();
            return View(khachHangs);
        }

        // GET: ADKhachHang/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ADKhachHang/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "HoTen,TaiKhoan,MatKhau,Email,DiachiKH,DienThoaiKH,NgaySinh")] KHACHHANG khachHang)
        {
            if (string.IsNullOrWhiteSpace(khachHang.HoTen))
            {
                ModelState.AddModelError("HoTen", "Họ tên không được để trống.");
            }
            if (string.IsNullOrWhiteSpace(khachHang.TaiKhoan))
            {
                ModelState.AddModelError("TaiKhoan", "Tài khoản không được để trống.");
            }
            if (string.IsNullOrWhiteSpace(khachHang.MatKhau))
            {
                ModelState.AddModelError("MatKhau", "Mật khẩu không được để trống.");
            }
            if (string.IsNullOrWhiteSpace(khachHang.Email))
            {
                ModelState.AddModelError("Email", "Email không được để trống.");
            }
            if (string.IsNullOrWhiteSpace(khachHang.DiachiKH))
            {
                ModelState.AddModelError("DiachiKH", "Địa chỉ không được để trống.");
            }
            if (string.IsNullOrWhiteSpace(khachHang.DienThoaiKH))
            {
                ModelState.AddModelError("DienThoaiKH", "Điện thoại không được để trống.");
            }
            if (khachHang.NgaySinh == default(DateTime))
            {
                ModelState.AddModelError("NgaySinh", "Ngày sinh không được để trống.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    DbContext.KHACHHANGs.Add(khachHang);
                    DbContext.SaveChanges(); // Lưu thay đổi vào cơ sở dữ liệu
                    return RedirectToAction("Index"); // Chuyển hướng về danh sách khách hàng
                }
                catch (Exception ex)
                {
                    // Ghi log lỗi hoặc hiển thị thông báo lỗi
                    ModelState.AddModelError("", "Lỗi khi lưu khách hàng: " + ex.Message);
                }
            }

            // Nếu có lỗi, quay lại view Create và hiển thị lỗi
            return View(khachHang);
        }

        // GET: ADKhachHang/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            KHACHHANG khachHang = DbContext.KHACHHANGs.Find(id);
            if (khachHang == null)
            {
                return HttpNotFound();
            }

            return View(khachHang);
        }

        // POST: ADKhachHang/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaKH,HoTen,TaiKhoan,MatKhau,Email,DiachiKH,DienThoaiKH,NgaySinh")] KHACHHANG khachHang)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    DbContext.Entry(khachHang).State = System.Data.Entity.EntityState.Modified;
                    DbContext.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi khi cập nhật khách hàng: " + ex.Message);
                }
            }

            return View(khachHang);
        }    

    // GET: ADKhachHang/Delete/5
    public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            KHACHHANG khachHang = DbContext.KHACHHANGs.Find(id);
            if (khachHang == null)
            {
                return HttpNotFound();
            }

            return View(khachHang);
        }

        // POST: ADKhachHang/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                KHACHHANG khachHang = DbContext.KHACHHANGs.Find(id);
                if (khachHang != null)
                {
                    DbContext.KHACHHANGs.Remove(khachHang);
                    DbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                // Ghi log lỗi hoặc hiển thị thông báo lỗi
                ModelState.AddModelError("", "Lỗi khi xóa khách hàng: " + ex.Message);
            }

            return RedirectToAction("Index");
        }
    }
}
