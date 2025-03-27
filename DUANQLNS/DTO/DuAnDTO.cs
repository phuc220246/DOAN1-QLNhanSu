using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUANQLNS.DTO
{
    public class DuAnDTO
    {
        public string IDDuAn { get; set; }


        public string TenDuAn { get; set; }


        public string SoLuongNVDA { get; set; }

        public string MoTaDA { get; set; }

        public DuAnDTO() { }

        // Constructor để khởi tạo trực tiếp các giá trị
        public DuAnDTO(string idDuAn, string tenDuAn, string soluongNVDA, string motaDA)
        {
            IDDuAn = idDuAn;
            TenDuAn = tenDuAn;
            SoLuongNVDA = soluongNVDA;
            MoTaDA = motaDA;
        }


        public DuAnDTO(DataRow row)
        {
            IDDuAn = row["IDDA"].ToString();
            TenDuAn = row["TENDA"].ToString();
            SoLuongNVDA = row["SOLUONGNVDA"].ToString();
            MoTaDA = row["MOTADA"].ToString();
        }
    }
}

