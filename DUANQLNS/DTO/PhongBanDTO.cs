using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUANQLNS.DTO
{
    public class PhongBanDTO
    {
        public string IDPhongBan { get; set; }

       
        public string TenPhongBan { get; set; }


        public string SoLuongNVPB { get; set; }

        public string MoTa { get; set; }

        public PhongBanDTO() { }

        // Constructor để khởi tạo trực tiếp các giá trị
        public PhongBanDTO(string idPhongBan, string tenPhongBan, string soluongNVPB, string mota)
        {
            IDPhongBan = idPhongBan;
            TenPhongBan = tenPhongBan;
            SoLuongNVPB = soluongNVPB;
            MoTa = mota;
        }


        public PhongBanDTO(DataRow row)
        {
            IDPhongBan = row["IDPB"].ToString();
            TenPhongBan = row["TENPB"].ToString();
            SoLuongNVPB = row["SOLUONGNVPB"].ToString();
            MoTa = row["MOTAPB"].ToString();
        }
    }
}
