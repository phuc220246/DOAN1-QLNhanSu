using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using DUANQLNS.DTO;
using DUANQLNS.DAL;

namespace DUANQLNS.DAL
{
    public class PhongBanDAL
    {
        private SqlConnection _conn;
        public PhongBanDAL() // kết nối với csdl = cách lấy chuỗi kết nối sẳn từ DataHelper
        {
            _conn = new SqlConnection(DataHelper.Instance.ConnectionString);
        }
        public List<PhongBanDTO> LoadPhongBanList()
        {
            List<PhongBanDTO> phongBanList = new List<PhongBanDTO>();
            DataTable data = DataHelper.Instance.ExecuteQuery("SELECT * FROM PHONGBAN");
            foreach (DataRow row in data.Rows)
            {
                PhongBanDTO phongban = new PhongBanDTO(row);
                phongBanList.Add(phongban);
            }

            return phongBanList;
        }
        public void AddPhongBan(string idPhongBan, string tenPhongBan, string soluongNVPB, string mota)
        {
            string query = "INSERT INTO PHONGBAN (IDPB, TENPB, SOLUONGNVPB, MOTAPB) VALUES (@IDPB, @TENPB, @SOLUONGNVPB, @MOTAPB)";
            using (SqlCommand cmd = new SqlCommand(query, _conn))
            {
                cmd.Parameters.AddWithValue("@IDPB", idPhongBan);
                cmd.Parameters.AddWithValue("@TENPB", tenPhongBan);
                cmd.Parameters.AddWithValue("@SOLUONGNVPB", soluongNVPB);
                cmd.Parameters.AddWithValue("@MOTAPB", mota);

                _conn.Open();
                cmd.ExecuteNonQuery();
                _conn.Close();
            }
        }
        public bool UpdatePhongBan(string idPhongBan, string tenPhongBan, string soluongNVPB, string mota)
        {
            string query = "UPDATE PHONGBAN SET TENPB = @TENPB, SOLUONGNVPB = @SOLUONGNVPB, MOTAPB = @MOTAPB WHERE IDPB = @IDPB";
            using (SqlCommand cmd = new SqlCommand(query, _conn))
            {
                cmd.Parameters.AddWithValue("@IDPB", idPhongBan);
                cmd.Parameters.AddWithValue("@TENPB", tenPhongBan);
                cmd.Parameters.AddWithValue("@SOLUONGNVPB", soluongNVPB);
                cmd.Parameters.AddWithValue("@MOTAPB", mota);
                _conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                _conn.Close();

                return rowsAffected > 0;
            }
        }
        public bool DeletePhongBan(string idPhongBan)
        {
            string query = "DELETE FROM PHONGBAN WHERE IDPB = @IDPB";
            using (SqlCommand cmd = new SqlCommand(query, _conn))
            {
                cmd.Parameters.AddWithValue("@IDPB", idPhongBan);

                _conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                _conn.Close();

                return rowsAffected > 0;
            }
        }
        public DataTable GetPhongBanByTenID(string timKiem)
        {
            string query = "SELECT IDPB, TENPB, SOLUONGNVPB, MOTAPB FROM PHONGBAN WHERE TENPB LIKE @TENPB + '%' OR IDPB LIKE @IDPB + '%'";
            using (SqlCommand cmd = new SqlCommand(query, _conn))
            {
                cmd.Parameters.AddWithValue("@TENPB", timKiem + "%");
                cmd.Parameters.AddWithValue("@IDPB", timKiem + "%");

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

    }
}
