using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DUANQLNS.DAL;
using DUANQLNS.DTO;

namespace DUANQLNS.BLL
{
    public class TaiKhoanBLL
    {
        private TaiKhoanDAL tkDAL;

        public TaiKhoanBLL()
        {
            tkDAL = new TaiKhoanDAL();
        }

        public List<TaiKhoanDTO> GetTaiKhoanList()
        {
            List<TaiKhoanDTO> taiKhoanList = tkDAL.LoadTaiKhoanList();

            // Kiểm tra lại xem danh sách có dữ liệu không
            if (taiKhoanList == null || taiKhoanList.Count == 0)
            {
                Console.WriteLine("Danh sách tài khoản trống.");
            }

            return taiKhoanList;
        }


        public void AddTaiKhoan(string idTaiKhoan, string tenTaiKhoan, string matKhau)
        {
            if (string.IsNullOrWhiteSpace(idTaiKhoan))
                throw new ArgumentException("ID tài khoản không được để trống.");
            if (string.IsNullOrWhiteSpace(tenTaiKhoan))
                throw new ArgumentException("Tên tài khoản không được để trống.");
            if (string.IsNullOrWhiteSpace(matKhau))
                throw new ArgumentException("Mật khẩu không được để trống.");

            tkDAL.AddTaiKhoan(idTaiKhoan, tenTaiKhoan, matKhau);
        }


        public bool UpdateTaiKhoan(string idTaiKhoan, string tenTaiKhoan, string matKhau)
        {
            if (string.IsNullOrWhiteSpace(idTaiKhoan))
                throw new ArgumentException("ID tài khoản không được để trống!");

            if (string.IsNullOrWhiteSpace(tenTaiKhoan))
                throw new ArgumentException("Tên tài khoản không được để trống!");

            if (string.IsNullOrWhiteSpace(matKhau))
                throw new ArgumentException("Mật khẩu không được để trống!");

            return tkDAL.UpdateTaiKhoan(idTaiKhoan, tenTaiKhoan, matKhau);
        }
        public bool DeleteTaiKhoan(string maTaiKhoan)
        {

            if (string.IsNullOrEmpty(maTaiKhoan))
            {
                throw new ArgumentException("Mã tài khoản không được để trống.");
            }

            // gọi  DAL để xóa 
            return tkDAL.DeleteTaiKhoan(maTaiKhoan);
        }

        public DataTable TimKiemTaiKhoan(string timKiem)
        {

            if (string.IsNullOrEmpty(timKiem))
            {
                throw new ArgumentException("Nội dung tìm kiếm không được để trống.");
            }

            // gọi DAL để tìm kiếm
            return tkDAL.GetTaiKhoanByTenID(timKiem);
        }


        public bool DangNhap(string username, string password)
        {

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("Tài khoản và mật khẩu không được để trống.");
            }
            // Gọi tầng DAL để kiểm tra đăng nhập
            return tkDAL.DangNhap(username, password);
        }

    }
}
