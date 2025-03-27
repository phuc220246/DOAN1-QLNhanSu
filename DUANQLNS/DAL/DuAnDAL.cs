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
    public class DuAnDAL
    {
        private SqlConnection _conn;
        public DuAnDAL() // kết nối với csdl = cách lấy chuỗi kết nối sẳn từ DataHelper
        {
            _conn = new SqlConnection(DataHelper.Instance.ConnectionString);
        }
        public List<DuAnDTO> LoadDuAnList()
        {
            List<DuAnDTO> duAnList = new List<DuAnDTO>();
            DataTable data = DataHelper.Instance.ExecuteQuery("SELECT * FROM DUAN");
            foreach (DataRow row in data.Rows)
            {
                DuAnDTO duan = new DuAnDTO(row);
                duAnList.Add(duan);
            }

            return duAnList;

        }
        public void AddDuAn(string idDuAn, string tenDuAn, string soluongNVDA, string motaDA)
        {
            string query = "INSERT INTO DUAN (IDDA, TENDA, SOLUONGNVDA, MOTADA) VALUES (@IDDA, @TENDA, @SOLUONGNVDA, @MOTADA)";
            using (SqlCommand cmd = new SqlCommand(query, _conn))
            {
                cmd.Parameters.AddWithValue("@IDDA", idDuAn);
                cmd.Parameters.AddWithValue("@TENDA", tenDuAn);
                cmd.Parameters.AddWithValue("@SOLUONGNVDA", soluongNVDA);
                cmd.Parameters.AddWithValue("@MOTADA", motaDA);

                _conn.Open();
                cmd.ExecuteNonQuery();
                _conn.Close();
            }
        }
        public bool UpdateDuAn(string idDuAn, string tenDuAn, string soluongNVDA, string motaDA)
        {
            string query = "UPDATE DUAN SET TENDA = @TENDA, SOLUONGNVDA = @SOLUONGNVDA, MOTADA = @MOTADA WHERE IDDA = @IDDA";
            using (SqlCommand cmd = new SqlCommand(query, _conn))
            {
                cmd.Parameters.AddWithValue("@IDDA", idDuAn);
                cmd.Parameters.AddWithValue("@TENDA", tenDuAn);
                cmd.Parameters.AddWithValue("@SOLUONGNVDA", soluongNVDA);
                cmd.Parameters.AddWithValue("@MOTADA", motaDA);
                _conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                _conn.Close();

                return rowsAffected > 0;
            }
        }
        public bool DeleteDuAn(string idDuAn)
        {
            string query = "DELETE FROM DUAN WHERE IDDA = @IDDA";
            using (SqlCommand cmd = new SqlCommand(query, _conn))
            {
                cmd.Parameters.AddWithValue("@IDDA", idDuAn);

                _conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                _conn.Close();

                return rowsAffected > 0;
            }
        }
        public DataTable GetDuAnByTenID(string timKiem)
        {
            string query = "SELECT IDDA, TENDA, SOLUONGNVDA, MOTADA FROM DUAN WHERE TENDA LIKE @TENDA + '%' OR IDDA LIKE @IDDA + '%'";
            using (SqlCommand cmd = new SqlCommand(query, _conn))
            {
                cmd.Parameters.AddWithValue("@TENDA", timKiem + "%");
                cmd.Parameters.AddWithValue("@IDDA", timKiem + "%");

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

    }
}
