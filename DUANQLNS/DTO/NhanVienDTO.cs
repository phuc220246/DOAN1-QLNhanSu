using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUANQLNS.DTO
{
    public class NhanVienDTO
    {
        public string IDNhanVien { get; set; }


        public string TenNhanVien { get; set; }


        public DateTime NgaySinh { get; set; }

        public string DiaChi { get; set; }

        public string Luong {  get; set; }

        public string TenPhongBan { get; set; }  
        public string TenDuAn { get; set; }
        public string VaiTro { get; set; }

        public NhanVienDTO() { }

        // Constructor để khởi tạo trực tiếp các giá trị
        public NhanVienDTO(string idNhanVien, string tenNhanVien, DateTime ngaySinh, string diaChi, string luong, string tenPhongBan, string tenDuAn, string vaiTro)
        {
            IDNhanVien = idNhanVien;
            TenNhanVien = tenNhanVien;
            NgaySinh = ngaySinh;
            DiaChi = diaChi;
            Luong = luong;
            TenPhongBan = tenPhongBan;
            TenDuAn = tenDuAn;
            VaiTro = vaiTro;
        }


        public NhanVienDTO(DataRow row)
        {
            
            IDNhanVien = row["IDNV"].ToString();
            TenNhanVien = row["TENNV"].ToString();
            NgaySinh = Convert.ToDateTime(row["NGAYSINH"]);
            DiaChi = row["DIACHI"].ToString();
            Luong = row["LUONG"].ToString();
            TenPhongBan = row["TENPB"].ToString();
            TenDuAn = row["TENDA"].ToString();
            VaiTro = row["VAITRO"].ToString();


        }
    }
}
