using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBookStore.Models;

namespace WebBookStore.Controllers
{
    public class GioHangController : Controller
    {
        //Tọa dôi tượng
        dbQLBookstore DbContext = new dbQLBookstore();
        // GET: GioHang
        public List<Giohang> LayGiohang()
        {
            List<Giohang> lstGiohang = Session["Giohang"] as List<Giohang>;
            if (lstGiohang == null)
            {
                lstGiohang = new List<Giohang>();
                Session["Giohang"] = lstGiohang;
            }
            return lstGiohang;
        }
        public ActionResult ThemGiohang(int iMasach, string strURL)
        {
            List<Giohang> lstGiohang = LayGiohang();
            Giohang sanpham = lstGiohang.Find(n => n.iMasach == iMasach);
            if (sanpham == null)
            {
                sanpham = new Giohang(iMasach);
                lstGiohang.Add(sanpham);
                return Redirect(strURL);
            }
            else
            {
                sanpham.iSoluong++;
                return Redirect(strURL);
            }
        }
        //tong sl
        private int TongSoLuong()
        {
            int iTongSoLuong = 0;
            List<Giohang> lstGiohang = Session["Giohang"] as List<Giohang>;
            if (lstGiohang != null)
            {
                iTongSoLuong = lstGiohang.Sum(n => n.iSoluong);
            }
            return iTongSoLuong;
        }
        //tinh tổng
        private double TongTien()
        {
            List<Giohang> lstGiohang = Session["Giohang"] as List<Giohang>;
            double iTongTien = 0;
            if (lstGiohang != null)
            {
                iTongTien = lstGiohang.Sum(n => n.dThanhtien);
            }
            return iTongTien;
        }
        //tạo giỏ hàng
        public ActionResult Giohang()
        {
            List<Giohang> lstGiohang = LayGiohang();
            if (lstGiohang.Count == 0)
            {
                return RedirectToAction("Index", "BookStore");
            }

            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return View(lstGiohang);

        }
        public ActionResult GiohangPartial()
        {
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return PartialView();
        }

        //xóa giỏ hàng
        public ActionResult XoaGiohang(int iMaSP)
        {
            List<Giohang> lstGiohang = LayGiohang();
            Giohang sanpham = lstGiohang.SingleOrDefault(n => n.iMasach == iMaSP);
            if (lstGiohang != null)
            {
                lstGiohang.RemoveAll(n => n.iMasach == iMaSP);
                return RedirectToAction("Giohang");
            }
            if (lstGiohang.Count == 0)
            {
                return RedirectToAction("Index", "BookStore");
            }
            return RedirectToAction("Giohang");
        }
        //cap nhat gio hang
        public ActionResult CapnhatGiohang(int iMaSP, FormCollection f)
        {
            List<Giohang> lstGiohang = LayGiohang();
            Giohang sanpham = lstGiohang.SingleOrDefault(n => n.iMasach == iMaSP);
            if (sanpham != null)
            {
                sanpham.iSoluong = int.Parse(f["txtSoluong"].ToString());

            }
            return RedirectToAction("Giohang");
        }
        [HttpGet]
        public ActionResult Dathang()
        {
            //Kiểm tra việc đăng nhập
            if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
            {
                return RedirectToAction("Dangnhap", "Nguoidung");
            }
            if (Session["Giohang"] == null)
            {
                return RedirectToAction("Index", "BookStore");
            }
            //Lay gio hang
            List<Giohang> lstGiohang = LayGiohang();
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return View(lstGiohang);
        }
        [HttpPost]
        public ActionResult Dathang(FormCollection collection)
        {
            // Create a new order
            DONDATHANG ddh = new DONDATHANG
            {
              
                MaKH = ((KHACHHANG)Session["Taikhoan"]).MaKH,
                NgayDH = DateTime.Now,
                TinhTrangGiaoHang = false,
                DaThanhToan = false,
                NgayGiao = collection["NgayGiao"]

            };

            // Update customer information
            KHACHHANG kh = DbContext.KHACHHANGs.Find(ddh.MaKH);
            if (kh != null)
            {
                kh.HoTen = collection["HoTen"];
                //kh.NgaySinh = DateTime.Parse(collection["NgaySinh"]);
                kh.DiachiKH = collection["DiaChi"];

                // Save the updated customer information
                DbContext.Entry(kh).State = System.Data.Entity.EntityState.Modified;
            }

            // Add the order to the database
            DbContext.DONDATHANGs.Add(ddh);
            DbContext.SaveChanges(); // Save changes to generate SoDH

            // Retrieve the newly created order ID
            int orderId = ddh.SoDH;

            // Add order details
            List<Giohang> gh = LayGiohang();
            foreach (var item in gh)
            {
                ChiTietDH ctdh = new ChiTietDH
                {   HoTen= kh.HoTen,
                    //NgayGiao= collection["NgayGiao"],
                    DienThoaiKH = kh.DienThoaiKH,
                    DiaChi = kh.DiachiKH ,
                    NgaySinh=kh.NgaySinh,
                    MaDH = orderId.ToString(), // Correct: Convert int to string
                    MaSach = item.iMasach,
                    SoLuong = item.iSoluong,
                    GiaBan = (int)item.dThanhtien / item.iSoluong,
                    // Correct: Cast to int
                    
                };

                DbContext.ChiTietDHs.Add(ctdh);
            }

            // Save changes for order details
            DbContext.SaveChanges();

            // Clear the shopping cart
            Session["Giohang"] = null;

            return RedirectToAction("Xacnhandonhang");
        }


        [HttpGet]
        
        public ActionResult Xacnhandonhang()
        {
            return View();
        }
    }
}
//zimmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmnnnnnnnnnnnnnnnnnnnnnnnn