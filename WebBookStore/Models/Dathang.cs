using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WebBookStore.Models
{
    public class Dathang
    {
        public DbSet<DONHANG> DONHANGs { get; set; }
        public DbSet<CHITIETDONHANG> CHITIETDONHANGs { get; set; }
        public DbSet<KHACHHANG> KHACHHANGs { get; set; }
        public DbSet<SACH> SACHs { get; set; }
    }
    public class DONHANG
    {
        public int MaDonHang { get; set; }
        public int MaKH { get; set; }
        public DateTime NgayDatHang { get; set; }
        public DateTime NgayGiaoHang { get; set; }
        public double TongTien { get; set; }
        public string TinhTrang { get; set; }
        public virtual KHACHHANG KHACHHANG { get; set; }
        public virtual ICollection<CHITIETDONHANG> CHITIETDONHANGs { get; set; }
    }

    public class CHITIETDONHANG
    {
        public int MaCTDH { get; set; }
        public int MaDonHang { get; set; }
        public int MaSach { get; set; }
        public int SoLuong { get; set; }
        public double DonGia { get; set; }
        public virtual DONHANG DONHANG { get; set; }
        public virtual SACH SACH { get; set; }
    }


}