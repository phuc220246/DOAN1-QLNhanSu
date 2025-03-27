using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUANQLNS.DTO
{
    public class LuongDTO
    {
        public int IDLuong { get; set; }
        public DateTime NgayBD { get; set; }
        public DateTime NgayKT { get; set; }
        public int SoNgayNghi { get; set; }
        public int TongLuong { get; set; }
        public string TenNhanVien { get; set; }  
        public int Luong { get; set; } 

        public LuongDTO() { }

        public LuongDTO(int idLuong, DateTime ngayBD, DateTime ngayKT, int soNgayNghi, int tongLuong, string tenNhanVien, int luong)
        {
            IDLuong = idLuong;
            NgayBD = ngayBD.Date;
            NgayKT = ngayKT.Date;
            SoNgayNghi = soNgayNghi;
            TongLuong = tongLuong;
            TenNhanVien = tenNhanVien;
            Luong = luong;
        }

        public LuongDTO(DataRow row)
        {
            IDLuong = Convert.ToInt32(row["IDLUONG"]);
            NgayBD = Convert.ToDateTime(row["NGAYBD"]);
            NgayKT = Convert.ToDateTime(row["NGAYKT"]);
            SoNgayNghi = Convert.ToInt32(row["SONGAYNGHI"]);
            TongLuong = Convert.ToInt32(row["TONGLUONG"]);
            TenNhanVien = row["TENNV"].ToString(); 
            Luong = Convert.ToInt32(row["LUONG"]); // Lương theo ngày
        }
    }

}

