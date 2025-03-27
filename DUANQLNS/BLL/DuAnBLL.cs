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
    public class DuAnBLL
    {
        private DuAnDAL daDAL;

        public DuAnBLL()
        {
            daDAL = new DuAnDAL();
        }

        public List<DuAnDTO> GetDuAnList()
        {
            List<DuAnDTO> duAnList = daDAL.LoadDuAnList();

            // Kiểm tra lại xem danh sách có dữ liệu không
            if (duAnList == null || duAnList.Count == 0)
            {
                Console.WriteLine("Danh sách dự án trống.");
            }

            return duAnList;
        }
        public void AddDuAn(string idDuAn, string tenDuAn, string soluongNVDA, string motaDA)
        {
            if (string.IsNullOrWhiteSpace(idDuAn))
                throw new ArgumentException("ID dự án không được để trống.");
            if (string.IsNullOrWhiteSpace(tenDuAn))
                throw new ArgumentException("Tên dự án không được để trống.");
            if (string.IsNullOrWhiteSpace(soluongNVDA))
                throw new ArgumentException("Số lượng nhân viên không được để trống.");
            if (string.IsNullOrWhiteSpace(motaDA))
                throw new ArgumentException("Mô tả không được để trống.");

            daDAL.AddDuAn(idDuAn, tenDuAn, soluongNVDA, motaDA);
        }
        public bool UpdateDuAn(string idDuAn, string tenDuAn, string soluongNVDA, string motaDA)
        {
            if (string.IsNullOrWhiteSpace(idDuAn))
                throw new ArgumentException("ID dự án không được để trống.");
            if (string.IsNullOrWhiteSpace(tenDuAn))
                throw new ArgumentException("Tên dự án không được để trống.");
            if (string.IsNullOrWhiteSpace(soluongNVDA))
                throw new ArgumentException("Số lượng nhân viên không được để trống.");
            if (string.IsNullOrWhiteSpace(motaDA))
                throw new ArgumentException("Mô tả không được để trống.");

            return daDAL.UpdateDuAn(idDuAn, tenDuAn, soluongNVDA, motaDA);
        }
        public bool DeleteDuAn(string idDuAn)
        {

            if (string.IsNullOrEmpty(idDuAn))
            {
                throw new ArgumentException("Mã dự án không được để trống.");
            }

            // gọi  DAL để xóa 
            return daDAL.DeleteDuAn(idDuAn);
        }
        public DataTable TimKiemDuAn(string timKiem)
        {

            if (string.IsNullOrEmpty(timKiem))
            {
                throw new ArgumentException("Nội dung tìm kiếm không được để trống.");
            }

            // gọi DAL để tìm kiếm
            return daDAL.GetDuAnByTenID(timKiem);
        }
    }
}
