using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBookStore.Models;

namespace WebBookStore.Controllers
{
    public class ADNXBController : Controller
    {
        dbQLBookstore DbContext = new dbQLBookstore();

        // GET: ADNXB
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult QLChuDe()
        {
            var chude = DbContext.CHUDEs.ToList();
            return View(chude);
        }

        // GET: ChuDe/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            CHUDE chude = DbContext.CHUDEs.Find(id);
            if (chude == null)
            {
                return HttpNotFound();
            }
            return View(chude);
        }

        // POST: ChuDe/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaCD,TenChuDe")] CHUDE chude)
        {
            if (ModelState.IsValid)
            {
                DbContext.Entry(chude).State = System.Data.Entity.EntityState.Modified;
                DbContext.SaveChanges();
                return RedirectToAction("QLChuDe");
            }
            return View(chude);
        }

        // GET: ChuDe/Create
        public ActionResult CreateChuDe()
        {
            return View();
        }

        // POST: ChuDe/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateChuDe([Bind(Include = "TenChuDe")] CHUDE chude)
        {
            if (string.IsNullOrWhiteSpace(chude.TenChuDe))
            {
                ModelState.AddModelError("TenChuDe", "Vui lòng nhập tên chủ đề.");
            }

            if (ModelState.IsValid)
            {
                DbContext.CHUDEs.Add(chude);
                DbContext.SaveChanges();
                return RedirectToAction("QLChuDe");
            }

            return View(chude);
        }


        // GET: ChuDe/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            CHUDE chude = DbContext.CHUDEs.Find(id);
            if (chude == null)
            {
                return HttpNotFound();
            }
            return View(chude);
        }

        // POST: ChuDe/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CHUDE chude = DbContext.CHUDEs.Find(id);
            DbContext.CHUDEs.Remove(chude);
            DbContext.SaveChanges();
            return RedirectToAction("QLChuDe");
        }
        // GET: SACH/Create
        public ActionResult Create()
        {
            ViewBag.MaCD = new SelectList(DbContext.CHUDEs, "MaCD", "TenChuDe");
            ViewBag.MaNXB = new SelectList(DbContext.NHAXUATBANs, "MaNXB", "TenNXB");
            return View();
        }

        // POST: SACH/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaSach,TenSach,GiaBan,MoTa,SoLuongTon,MaCD,MaNXB")] SACH sach, HttpPostedFileBase AnhBia)
        {
            // Kiểm tra dữ liệu đầu vào
            if (string.IsNullOrEmpty(sach.TenSach))
            {
                ModelState.AddModelError("TenSach", "Tên sách không được để trống.");
            }
            if (sach.GiaBan <= 0)
            {
                ModelState.AddModelError("GiaBan", "Giá bán phải lớn hơn 0.");
            }
            if (string.IsNullOrEmpty(sach.MoTa))
            {
                ModelState.AddModelError("MoTa", "Mô tả không được để trống.");
            }
            if (sach.SoLuongTon < 0)
            {
                ModelState.AddModelError("SoLuongTon", "Số lượng tồn không được âm.");
            }
            if (sach.MaCD == null)
            {
                ModelState.AddModelError("MaCD", "Vui lòng chọn chủ đề.");
            }
            if (sach.MaNXB == null)
            {
                ModelState.AddModelError("MaNXB", "Vui lòng chọn nhà xuất bản.");
            }
            if (AnhBia == null || AnhBia.ContentLength == 0)
            {
                ModelState.AddModelError("AnhBia", "Vui lòng chọn ảnh bìa cho sách.");
            }

            // Nếu dữ liệu hợp lệ, tiến hành lưu sách và ảnh bìa
            if (ModelState.IsValid)
            {
                // Đặt ngày cập nhật cho sách
                sach.NgayCapNhat = DateTime.Now;

                // Kiểm tra và lưu ảnh bìa nếu tồn tại
                if (AnhBia != null && AnhBia.ContentLength > 0)
                {
                    // Đảm bảo thư mục 'product_imgs' tồn tại
                    string pathToFolder = Server.MapPath("~/product_imgs");
                    if (!Directory.Exists(pathToFolder))
                    {
                        Directory.CreateDirectory(pathToFolder);
                    }

                    // Tạo đường dẫn đầy đủ cho ảnh bìa
                    var fileName = Path.GetFileName(AnhBia.FileName);
                    var path = Path.Combine(pathToFolder, fileName);

                    // Lưu ảnh vào thư mục
                    AnhBia.SaveAs(path);

                    // Gán đường dẫn ảnh vào thuộc tính AnhBia của sách
                    sach.AnhBia = "" + fileName;
                }

                // Thêm sách vào cơ sở dữ liệu
                DbContext.SACHes.Add(sach);
                DbContext.SaveChanges();

                // Điều hướng tới trang chi tiết của sách vừa thêm
                return RedirectToAction("Details", new { id = sach.MaSach });
            }

            // Nếu dữ liệu không hợp lệ, hiển thị lại form cùng thông báo lỗi
            ViewBag.MaCD = new SelectList(DbContext.CHUDEs, "MaCD", "TenChuDe", sach.MaCD);
            ViewBag.MaNXB = new SelectList(DbContext.NHAXUATBANs, "MaNXB", "TenNXB", sach.MaNXB);
            return View(sach);
        }


        // Thêm action Details để hiển thị thông tin sách sau khi tạo
        public ActionResult Details(int id)
        {
            var sach = DbContext.SACHes.Find(id);
            if (sach == null)
            {
                return HttpNotFound();
            }
            return View(sach);
        }
    }
}
//zinnnnnnnnnnnnnnnnnnnn