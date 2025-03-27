using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUANQLNS.DTO
{
    public class TaiKhoanDTO
    {
        //private DataRow row;

        // Thuộc tính ID tài khoản
        public string IDTaiKhoan { get; set; }

        // Thuộc tính tên tài khoản
        public string TenTaiKhoan { get; set; }


        public string MatKhau { get; set; }

        public TaiKhoanDTO() { }

        // Constructor để khởi tạo trực tiếp các giá trị
        public TaiKhoanDTO(string idTaiKhoan, string tenTaiKhoan, string matKhau)
        {
            IDTaiKhoan = idTaiKhoan;
            TenTaiKhoan = tenTaiKhoan;
            MatKhau = matKhau;
        }


        public TaiKhoanDTO(DataRow row)
        {
            IDTaiKhoan = row["IDTAIKHOAN"].ToString();
            TenTaiKhoan = row["TENTAIKHOAN"].ToString();
            MatKhau = row["MATKHAU"].ToString();
        }
    }
}
