using DUANQLNS.DAL;
using DUANQLNS.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUANQLNS.BLL
{
    public class HopDongBLL
    {
        private HopDongDAL hopDongDAL;

        public HopDongBLL()
        {
            hopDongDAL = new HopDongDAL();
        }

        public List<HopDongDTO> GetHopDongList()
        {
            List<HopDongDTO> hopDongList = hopDongDAL.LoadHopDongList();

            // Kiểm tra nếu danh sách rỗng
            if (hopDongList == null || hopDongList.Count == 0)
            {
                Console.WriteLine("Danh sách hợp đồng trống.");
            }

            return hopDongList;
        }
        public DataTable GetTenLoaiHDList()
        {

            return hopDongDAL.LayTenLoaiHDList(); // Gọi DAL để lấy dữ liệu 
        }
        public DataTable GetTrangThaiHDList()
        {

            return hopDongDAL.LayTrangThaiHDList(); // Gọi DAL để lấy dữ liệu 
        }
        public void AddHopDong(string idHopDong, DateTime ngayBD, DateTime ngayKT, string moTa, string idNhanVien, string idLoaiHD, string idTrangThai)
        {
            // Kiểm tra dữ liệu đầu vào
            if (string.IsNullOrWhiteSpace(idHopDong) || string.IsNullOrWhiteSpace(moTa) || string.IsNullOrWhiteSpace(idNhanVien) ||
                string.IsNullOrWhiteSpace(idLoaiHD) || string.IsNullOrWhiteSpace(idTrangThai))
            {
                throw new ArgumentException("Vui lòng nhập đầy đủ thông tin hợp đồng.");
            }

            // Kiểm tra ngày bắt đầu và ngày kết thúc
            if (ngayBD > ngayKT)
            {
                throw new ArgumentException("Ngày bắt đầu không thể lớn hơn ngày kết thúc.");
            }

            // Gọi DAL để thêm hợp đồng
            hopDongDAL.AddHopDong(idHopDong, ngayBD, ngayKT, moTa, idNhanVien, idLoaiHD, idTrangThai);
        }
        public bool UpdateHopDong(string idHopDong, DateTime ngayBD, DateTime ngayKT, string moTa, string idNhanVien, string idLoaiHD, string idTrangThai)
        {
            // Kiểm tra dữ liệu hợp lệ trước khi cập nhật
            if (string.IsNullOrEmpty(idHopDong))
            {
                throw new Exception("ID hợp đồng không được để trống.");
            }
            if (ngayBD > ngayKT)
            {
                throw new Exception("Ngày bắt đầu không thể lớn hơn ngày kết thúc.");
            }
            if (string.IsNullOrEmpty(idNhanVien))
            {
                throw new Exception("Vui lòng chọn nhân viên.");
            }
            if (string.IsNullOrEmpty(idLoaiHD))
            {
                throw new Exception("Vui lòng chọn loại hợp đồng.");
            }
            if (string.IsNullOrEmpty(idTrangThai))
            {
                throw new Exception("Vui lòng chọn trạng thái hợp đồng.");
            }

            return hopDongDAL.UpdateHopDong(idHopDong, ngayBD, ngayKT, moTa, idNhanVien, idLoaiHD, idTrangThai);
        }
        public bool DeleteHopDong(string idHopDong)
        {
            if (string.IsNullOrEmpty(idHopDong))
            {
                throw new Exception("ID hợp đồng không hợp lệ!");
            }

            return hopDongDAL.DeleteHopDong(idHopDong);
        }
        public DataTable TimKiemHopDong(string tuKhoa)
        {
            if (string.IsNullOrWhiteSpace(tuKhoa))
            {
                throw new Exception("Vui lòng nhập từ khóa tìm kiếm!");
            }

            return hopDongDAL.GetHopDongByTenID(tuKhoa);
        }

    }
}
