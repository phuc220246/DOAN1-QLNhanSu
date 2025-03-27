using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DUANQLNS.DAL;
using DUANQLNS.DTO;
using DUANQLNS.BLL;
namespace DUANQLNS.BLL
{
    public class NhanVienBLL
    {
        private NhanVienDAL nvDAL;

        public NhanVienBLL()
        {
            nvDAL = new NhanVienDAL();
        }
        public List<NhanVienDTO> GetNhanVienList()
        {
            List<NhanVienDTO> nhanVienList = nvDAL.LoadNhanVienList();

            // Kiểm tra lại xem danh sách có dữ liệu không
            if (nhanVienList == null || nhanVienList.Count == 0)
            {
                Console.WriteLine("Danh sách nhân viên trống.");
            }

            return nhanVienList;
        }
        public DataTable GetTenPhongBanList()
        {

            return nvDAL.LayTenPhongBanList(); // Gọi DAL để lấy dữ liệu 
        }
        public DataTable GetTenDuAnList()
        {
            return nvDAL.LayTenDuAnList();
        }
        public DataTable GetDanhSachNhanVien()
        {
            return nvDAL.LayDanhSachNhanVien();
        }

        public void AddNhanVien(string idNhanVien, string tenNhanVien, DateTime nsNV, string dcNV, string luongNV, string idPhongBan, string idDuAn, string vaiTro)
        {
            if (string.IsNullOrWhiteSpace(idNhanVien))
                throw new ArgumentException("ID nhân viên không được để trống.");
            if (string.IsNullOrWhiteSpace(tenNhanVien))
                throw new ArgumentException("Tên nhân viên không được để trống.");
            //  kiểm tra DateTime có giá trị hợp lệ không
            if (nsNV == DateTime.MinValue)
                throw new ArgumentException("Ngày sinh nhân viên không được để trống hoặc không hợp lệ.");
            if (string.IsNullOrWhiteSpace(dcNV))
                throw new ArgumentException("Địa chỉ không được để trống.");
            if (string.IsNullOrWhiteSpace(luongNV))
                throw new ArgumentException("Lương không được để trống.");
            if (string.IsNullOrWhiteSpace(idPhongBan))
                throw new ArgumentException("Tên phòng ban không được để trống.");
            if (string.IsNullOrWhiteSpace(idDuAn))
                throw new ArgumentException("Tên dự án không được để trống.");
            if (string.IsNullOrWhiteSpace(vaiTro))
                throw new ArgumentException("Vai trò không được để trống.");

            nvDAL.AddNhanVien(idNhanVien, tenNhanVien, nsNV, dcNV, luongNV, idPhongBan, idDuAn, vaiTro);
        }
        public bool UpdateNhanVien(string idNhanVien, string tenNhanVien, DateTime nsNV, string dcNV, string luongNV, string idPhongBan, string idDuAn, string vaiTro)
        {
            if (string.IsNullOrWhiteSpace(idNhanVien))
                throw new ArgumentException("ID nhân viên không được để trống.");
            if (string.IsNullOrWhiteSpace(tenNhanVien))
                throw new ArgumentException("Tên nhân viên không được để trống.");
            //  kiểm tra DateTime có giá trị hợp lệ không
            if (nsNV == DateTime.MinValue)
                throw new ArgumentException("Ngày sinh nhân viên không được để trống hoặc không hợp lệ.");
            if (string.IsNullOrWhiteSpace(dcNV))
                throw new ArgumentException("Địa chỉ không được để trống.");
            if (string.IsNullOrWhiteSpace(luongNV))
                throw new ArgumentException("Lương không được để trống.");
            if (string.IsNullOrWhiteSpace(idPhongBan))
                throw new ArgumentException("Tên phòng ban không được để trống.");
            if (string.IsNullOrWhiteSpace(idDuAn))
                throw new ArgumentException("Tên dự án không được để trống.");
            if (string.IsNullOrWhiteSpace(vaiTro))
                throw new ArgumentException("Vai trò không được để trống.");

            return nvDAL.UpdateNhanVien(idNhanVien, tenNhanVien, nsNV, dcNV, luongNV, idPhongBan, idDuAn, vaiTro);
        }
        public bool DeleteNhanVien(string idNhanVien)
        {

            if (string.IsNullOrEmpty(idNhanVien))
            {
                throw new ArgumentException("Mã nhân viên không được để trống.");
            }

            // gọi  DAL để xóa 
            return nvDAL.DeleteNhanVien(idNhanVien);
        }
        public DataTable TimKiemNhanVien(string timKiem)
        {

            if (string.IsNullOrEmpty(timKiem))
            {
                throw new ArgumentException("Nội dung tìm kiếm không được để trống.");
            }

            // gọi DAL để tìm kiếm
            return nvDAL.GetNhanVienByTenID(timKiem);
        }
    }
}
