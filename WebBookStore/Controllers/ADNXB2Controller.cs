using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBookStore.Models;

namespace WebBookStore.Controllers
{
    public class ADNXB2Controller : Controller
    {
        private dbQLBookstore DbContext = new dbQLBookstore();

        // GET: ADNXB2
        public ActionResult Index()
        {
            return View();
        }

        // GET: ADNXB2/QLNhaXuatBan
        public ActionResult QLNhaXuatBan()
        {
            var nhaxuatban = DbContext.NHAXUATBANs.ToList();
            return View(nhaxuatban);
        }

        // GET: ADNXB2/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ADNXB2/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TenNXB,DiaChi,DienThoai")] NHAXUATBAN nhaxuatban)
        {
            // Kiểm tra và thêm lỗi vào ModelState nếu các trường bắt buộc bị bỏ trống
            if (string.IsNullOrWhiteSpace(nhaxuatban.TenNXB))
            {
                ModelState.AddModelError("TenNXB", "Tên nhà xuất bản không được để trống.");
            }

            if (string.IsNullOrWhiteSpace(nhaxuatban.DiaChi))
            {
                ModelState.AddModelError("DiaChi", "Địa chỉ không được để trống.");
            }

            if (string.IsNullOrWhiteSpace(nhaxuatban.DienThoai))
            {
                ModelState.AddModelError("DienThoai", "Điện thoại không được để trống.");
            }

            // Kiểm tra tính hợp lệ của ModelState
            if (ModelState.IsValid)
            {
                DbContext.NHAXUATBANs.Add(nhaxuatban);
                DbContext.SaveChanges();
                return RedirectToAction("QLNhaXuatBan");
            }

            // Nếu không hợp lệ, trả về view với thông tin và lỗi
            return View(nhaxuatban);
        }


        // GET: ADNXB2/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            NHAXUATBAN nhaxuatban = DbContext.NHAXUATBANs.Find(id);
            if (nhaxuatban == null)
            {
                return HttpNotFound();
            }
            return View(nhaxuatban);
        }

        // POST: ADNXB2/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaNXB,TenNXB,DiaChi,DienThoai")] NHAXUATBAN nhaxuatban)
        {
            if (ModelState.IsValid)
            {
                DbContext.Entry(nhaxuatban).State = System.Data.Entity.EntityState.Modified;
                DbContext.SaveChanges();
                return RedirectToAction("QLNhaXuatBan");
            }
            return View(nhaxuatban);
        }

        // GET: ADNXB2/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            NHAXUATBAN nhaxuatban = DbContext.NHAXUATBANs.Find(id);
            if (nhaxuatban == null)
            {
                return HttpNotFound();
            }
            return View(nhaxuatban);
        }

        // POST: ADNXB2/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NHAXUATBAN nhaxuatban = DbContext.NHAXUATBANs.Find(id);
            DbContext.NHAXUATBANs.Remove(nhaxuatban);
            DbContext.SaveChanges();
            return RedirectToAction("QLNhaXuatBan");
        }
    }
}
