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
    public class PhongBanBLL
    {
        private PhongBanDAL pbDAL;

        public PhongBanBLL()
        {
            pbDAL = new PhongBanDAL();
        }

        public List<PhongBanDTO> GetPhongBanList()
        {
            List<PhongBanDTO> phongBanList = pbDAL.LoadPhongBanList();

            // Kiểm tra lại xem danh sách có dữ liệu không
            if (phongBanList == null || phongBanList.Count == 0)
            {
                Console.WriteLine("Danh sách phòng ban trống.");
            }

            return phongBanList;
        }
        public void AddPhongBan(string idPhongBan, string tenPhongBan, string soluongNVPB, string mota)
        {
            if (string.IsNullOrWhiteSpace(idPhongBan))
                throw new ArgumentException("ID phòng ban không được để trống.");
            if (string.IsNullOrWhiteSpace(tenPhongBan))
                throw new ArgumentException("Tên phòng ban không được để trống.");
            if (string.IsNullOrWhiteSpace(soluongNVPB))
                throw new ArgumentException("Số lượng nhân viên không được để trống.");
            if (string.IsNullOrWhiteSpace(mota))
                throw new ArgumentException("Mô tả không được để trống.");

            pbDAL.AddPhongBan(idPhongBan, tenPhongBan, soluongNVPB, mota);
        }
        public bool UpdatePhongBan(string idPhongBan, string tenPhongBan, string soluongNVPB, string mota)
        {
            if (string.IsNullOrWhiteSpace(idPhongBan))
                throw new ArgumentException("ID phòng ban không được để trống.");
            if (string.IsNullOrWhiteSpace(tenPhongBan))
                throw new ArgumentException("Tên phòng ban không được để trống.");
            if (string.IsNullOrWhiteSpace(soluongNVPB))
                throw new ArgumentException("Số lượng nhân viên không được để trống.");
            if (string.IsNullOrWhiteSpace(mota))
                throw new ArgumentException("Mô tả không được để trống.");

           return  pbDAL.UpdatePhongBan(idPhongBan, tenPhongBan, soluongNVPB, mota);
        }
        public bool DeletePhongBan(string idPhongBan)
        {

            if (string.IsNullOrEmpty(idPhongBan))
            {
                throw new ArgumentException("Mã phòng ban không được để trống.");
            }

            // gọi  DAL để xóa 
            return pbDAL.DeletePhongBan(idPhongBan);
        }
        public DataTable TimKiemPhongBan(string timKiem)
        {

            if (string.IsNullOrEmpty(timKiem))
            {
                throw new ArgumentException("Nội dung tìm kiếm không được để trống.");
            }

            // gọi DAL để tìm kiếm
            return pbDAL.GetPhongBanByTenID(timKiem);
        }
    }
}
