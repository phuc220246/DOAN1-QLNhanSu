using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUANQLNS.DTO
{
    public class HopDongDTO
    {
        public string IDHopDong { get; set; }
        public DateTime NgayBatDau { get; set; }
        public DateTime NgayKetThuc { get; set; }
        public string MoTa { get; set; }
        public string TenNhanVien { get; set; }  // Lấy TÊN thay vì ID
        public string LoaiHopDong { get; set; }  // Lấy TÊN thay vì ID
        public string TrangThaiHopDong { get; set; }  // Lấy TÊN thay vì ID

        public HopDongDTO() { }

        public HopDongDTO(string idHopDong, DateTime ngayBatDau, DateTime ngayKetThuc, string moTa,
                          string tenNhanVien, string loaiHopDong, string trangThaiHopDong)
        {
            IDHopDong = idHopDong;
            NgayBatDau = ngayBatDau.Date;
            NgayKetThuc = ngayKetThuc.Date;
            MoTa = moTa;
            TenNhanVien = tenNhanVien;
            LoaiHopDong = loaiHopDong;
            TrangThaiHopDong = trangThaiHopDong;
        }

        public HopDongDTO(DataRow row)
        {
           
            IDHopDong = row["IDHD"].ToString();
            NgayBatDau = Convert.ToDateTime(row["NGAYBDHD"]);
            NgayKetThuc = Convert.ToDateTime(row["NGAYKTHD"]);
            MoTa = row["MOTAHD"].ToString();
            TenNhanVien = row["TenNhanVien"].ToString();  // Đúng tên cột từ SQL
            LoaiHopDong = row["LoaiHopDong"].ToString();  // Đúng tên cột từ SQL
            TrangThaiHopDong = row["TrangThaiHopDong"].ToString();  // Đúng tên cột từ SQL
        }
    }

}
