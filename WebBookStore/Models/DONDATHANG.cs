//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebBookStore.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class DONDATHANG
    {
        public int SoDH { get; set; }
        public Nullable<int> MaKH { get; set; }
        public Nullable<System.DateTime> NgayDH { get; set; }
        public string NgayGiao { get; set; }
        public Nullable<bool> DaThanhToan { get; set; }
        public Nullable<bool> TinhTrangGiaoHang { get; set; }
        public string DatHang { get; set; }
    
        public virtual KHACHHANG KHACHHANG { get; set; }
        public virtual ChiTietDH ChiTietDH { get; set; }
    }
}
