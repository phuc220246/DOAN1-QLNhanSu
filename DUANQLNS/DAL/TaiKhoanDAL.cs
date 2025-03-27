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
    public class TaiKhoanDAL

    {
        private SqlConnection _conn;
        public TaiKhoanDAL() // kết nối với csdl = cách lấy chuỗi kết nối sẳn từ DataHelper
        {
            _conn = new SqlConnection(DataHelper.Instance.ConnectionString);
        }

        public List<TaiKhoanDTO> LoadTaiKhoanList()
        {
            List<TaiKhoanDTO> taiKhoanList = new List<TaiKhoanDTO>();
            DataTable data = DataHelper.Instance.ExecuteQuery("SELECT * FROM TAIKHOAN");
            foreach (DataRow row in data.Rows)
            {
                TaiKhoanDTO taiKhoan = new TaiKhoanDTO(row);
                taiKhoanList.Add(taiKhoan);
            }

            return taiKhoanList;
        }

        public void AddTaiKhoan(string idTaiKhoan, string tenTaiKhoan, string matKhau)
        {
            string query = "INSERT INTO TAIKHOAN (IDTAIKHOAN, TENTAIKHOAN, MATKHAU) VALUES (@IDTAIKHOAN, @TENTAIKHOAN, @MATKHAU)";
            using (SqlCommand cmd = new SqlCommand(query, _conn))
            {
                cmd.Parameters.AddWithValue("@IDTAIKHOAN", idTaiKhoan);
                cmd.Parameters.AddWithValue("@TENTAIKHOAN", tenTaiKhoan);
                cmd.Parameters.AddWithValue("@MATKHAU", matKhau);

                _conn.Open();
                cmd.ExecuteNonQuery();
                _conn.Close();
            }
        }

        public bool UpdateTaiKhoan(string idTaiKhoan, string tenTaiKhoan, string matKhau)
        {
            string query = "UPDATE TAIKHOAN SET TENTAIKHOAN = @TENTAIKHOAN, MATKHAU = @MATKHAU WHERE IDTAIKHOAN = @IDTAIKHOAN";
            using (SqlCommand cmd = new SqlCommand(query, _conn))
            {
                cmd.Parameters.AddWithValue("@IDTAIKHOAN", idTaiKhoan);
                cmd.Parameters.AddWithValue("@TENTAIKHOAN", tenTaiKhoan);
                cmd.Parameters.AddWithValue("@MATKHAU", matKhau);

                _conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                _conn.Close();

                return rowsAffected > 0;
            }
        }

        public bool DeleteTaiKhoan(string maTaiKhoan)
        {
            string query = "DELETE FROM TAIKHOAN WHERE IDTAIKHOAN = @IDTAIKHOAN";
            using (SqlCommand cmd = new SqlCommand(query, _conn))
            {
                cmd.Parameters.AddWithValue("@IDTAIKHOAN", maTaiKhoan);

                _conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                _conn.Close();

                return rowsAffected > 0;
            }
        }

        public DataTable GetTaiKhoanByTenID(string timKiem)
        {
            string query = "SELECT IDTAIKHOAN, TENTAIKHOAN, MATKHAU FROM TAIKHOAN WHERE TENTAIKHOAN LIKE @TENTAIKHOAN + '%' OR IDTAIKHOAN LIKE @IDTAIKHOAN + '%'";
            using (SqlCommand cmd = new SqlCommand(query, _conn))
            {
                cmd.Parameters.AddWithValue("@TENTAIKHOAN", timKiem + "%");
                cmd.Parameters.AddWithValue("@IDTAIKHOAN", timKiem + "%");

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }


        public bool DangNhap(string username, string password)
        {
            string query = "SELECT COUNT(*) FROM TAIKHOAN WHERE TENTAIKHOAN = @username AND MATKHAU = @password"; // truy vấn sql check đăng nhập
            using (SqlCommand cmd = new SqlCommand(query, _conn)) // thực thi câu lệnh 
            {
                cmd.Parameters.AddWithValue("@username", username); // tránh lỗi sql injection 
                cmd.Parameters.AddWithValue("@password", password);

                _conn.Open();
                int count = (int)cmd.ExecuteScalar(); // thực hiênj truy vấn và lấy kq
                _conn.Close();

                return count > 0; //trả kq kiểm tra
            }
        }
    }

}
