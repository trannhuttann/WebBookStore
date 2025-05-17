using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBookStore.Models;
using PagedList;
using PagedList.Mvc;

namespace WebBookStore.Controllers
{
    public class BookStoreController : Controller
    {

        //Tọa dôi tượng
         dbQLBookstore DbContext = new dbQLBookstore();
        private List<SACH> Laysachmoi(int count)
        {
            //sap xep
            return DbContext.SACHes.OrderByDescending(a => a.NgayCapNhat).Take(count).ToList();
        }
        // GET: BookStore
        public ActionResult Index(int ? page)
        {
            //tạo biến quy định số lượng sp
            //doi so trang o day
            int pageSize = 6;
            //tao bien so trang
            int pageNum = (page ?? 1);
            var sachmoi = Laysachmoi(50);
            return View(sachmoi.ToPagedList(pageNum,pageSize));
        }
        public ActionResult Chude()
        {
            var chude = from cd in DbContext.CHUDEs select cd;
            return PartialView(chude);
        }
        public ActionResult Nhasanxuat()
        {
            var nhasanxuat = from cd in DbContext.NHAXUATBANs select cd;
            return PartialView(nhasanxuat);
        }
        

        public ActionResult SPTheochude(int id)
        {
            var sach = from s in DbContext.SACHes where s.MaCD == id select s;
            return View(sach);
        }

        public ActionResult SPtheoNhaXuatBan(int id)
        {
            var sach = from s in DbContext.SACHes where s.MaNXB == id select s;
            return View(sach);
        }
        public ActionResult Details(int id)
        {
            var sach = from s in DbContext.SACHes where s.MaSach == id select s;
            return View(sach.Single());
        }

        public ActionResult Search(string keyword, int? page)
        {
            // Nếu từ khóa rỗng, trả về danh sách rỗng
            if (string.IsNullOrEmpty(keyword))
            {
                return View(new List<SACH>().ToPagedList(1, 6));
            }

            // Tìm kiếm sách dựa trên từ khóa
            var result = DbContext.SACHes
                           .Where(s => s.TenSach.Contains(keyword) ||
                                       s.MoTa.Contains(keyword))
                           .OrderBy(s => s.TenSach)
                           .ToList();

            int pageSize = 6;
            int pageNum = (page ?? 1);

            // Trả về view với kết quả tìm kiếm và từ khóa
            ViewBag.Keyword = keyword; // Lưu từ khóa vào ViewBag để sử dụng trong view
            return View(result.ToPagedList(pageNum, pageSize));
        }


    }
}