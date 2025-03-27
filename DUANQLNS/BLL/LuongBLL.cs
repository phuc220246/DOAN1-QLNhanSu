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
    public class LuongBLL
    {
        private LuongDAL luongDAL;

        public LuongBLL()
        {
            luongDAL = new LuongDAL();
        }

        public List<LuongDTO> GetLuongList()
        {
            List<LuongDTO> luongList = luongDAL.LoadLuongList();

            // Kiểm tra nếu danh sách rỗng
            if (luongList == null || luongList.Count == 0)
            {
                Console.WriteLine("Danh sách lương trống.");
            }

            return luongList;
        }
        public DataTable GetTenNhanVienList()
        {

            return luongDAL.LayTenNhanVienList(); // Gọi DAL để lấy dữ liệu 
        }
        public bool TinhLuong(DateTime ngayBD, DateTime ngayKT, int soNgayNghi, int luongNgay, string idNV)
        {
            {
                if (ngayBD == DateTime.MinValue)
                    throw new ArgumentException("Ngày bắt đầu không hợp lệ.");

                if (ngayKT == DateTime.MinValue)
                    throw new ArgumentException("Ngày kết thúc không hợp lệ.");

                if (ngayKT < ngayBD)
                    throw new ArgumentException("Ngày kết thúc không được nhỏ hơn ngày bắt đầu.");

                if (soNgayNghi < 0)
                    throw new ArgumentException("Số ngày nghỉ không thể là số âm.");

                if (luongNgay <= 0)
                    throw new ArgumentException("Lương ngày phải lớn hơn 0.");
                return luongDAL.TinhLuong(ngayBD, ngayKT, soNgayNghi, luongNgay, idNV);
            }
        }
        public bool UpdateLuong(int idLuong, DateTime ngayBD, DateTime ngayKT, int soNgayNghi, int luongNgay, string idNV)
        {
            // Kiểm tra hợp lệ trước khi cập nhật
            if (ngayBD > ngayKT)
            {
                throw new Exception("Ngày bắt đầu không thể lớn hơn ngày kết thúc.");
            }
            if (soNgayNghi < 0)
            {
                throw new Exception("Số ngày nghỉ không thể nhỏ hơn 0.");
            }
            if (luongNgay < 0)
            {
                throw new Exception("Lương/ngày không hợp lệ.");
            }
            if (string.IsNullOrEmpty(idNV))
            {
                throw new Exception("Vui lòng chọn nhân viên.");
            }

            return luongDAL.UpdateLuong(idLuong, ngayBD, ngayKT, soNgayNghi, luongNgay, idNV);
        }
        public bool DeleteLuong(int idLuong)
        {
            return luongDAL.DeleteLuong(idLuong);
        }
        public DataTable SearchLuong(string keyword)
        {
            return luongDAL.GetLuongByTenID(keyword);
        }
    }

}

