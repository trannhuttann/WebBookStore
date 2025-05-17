using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebBookStore.Models;

namespace WebBookStore.Models
{
    public class Giohang
    {

        //Tọa dôi tượng
        dbQLBookstore DbContext = new dbQLBookstore();
        public int iMasach { set; get; }
        public String sTensach { set; get; }
        public String sAnhbia { set; get; }
        public Double dDongia { set; get; }
        public int iSoluong { set; get; }
        public Double dThanhtien
        {
            get { return iSoluong * dDongia; }
        }
        public Giohang (int Masach)
        {
            iMasach = Masach;
            SACH sach = DbContext.SACHes.Single(n => n.MaSach == iMasach);
            sTensach = sach.TenSach;
            sAnhbia = sach.AnhBia;
            dDongia = double.Parse(sach.GiaBan.ToString());
            iSoluong = 1;
        }
       

    }
}