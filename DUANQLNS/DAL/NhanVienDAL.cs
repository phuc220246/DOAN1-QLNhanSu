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
using System.Globalization;

namespace DUANQLNS.DAL
{
    public class NhanVienDAL
    {
        private SqlConnection _conn;
        public NhanVienDAL() // kết nối với csdl = cách lấy chuỗi kết nối sẳn từ DataHelper
        {
            _conn = new SqlConnection(DataHelper.Instance.ConnectionString);
        }
        public List<NhanVienDTO> LoadNhanVienList()
        {
            List<NhanVienDTO> nhanVienList = new List<NhanVienDTO>();
            string query = @"EXEC USP_GetNhanVienWithPhongBanAndDuAn"; // Gọi Stored Procedure

            DataTable data = DataHelper.Instance.ExecuteQuery(query);
            foreach (DataRow row in data.Rows)
            {
                NhanVienDTO nhanvien = new NhanVienDTO(row);
                nhanVienList.Add(nhanvien);
            }

            return nhanVienList;
        }
        public DataTable LayTenPhongBanList()
        {
            string query = "SELECT IDPB, TENPB FROM PHONGBAN ";
            SqlCommand cmd = new SqlCommand(query, _conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            _conn.Open();
            da.Fill(dt);
            _conn.Close();
            return dt;
        }
        public DataTable LayTenDuAnList()
        {
            string query = "SELECT IDDA, TENDA FROM DUAN ";
            SqlCommand cmd = new SqlCommand(query, _conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            _conn.Open();
            da.Fill(dt);
            _conn.Close();
            return dt;
        }
        public void AddNhanVien(string idNhanVien, string tenNhanVien, DateTime nsNV, string dcNV, string luongNV, string idPhongBan, string idDuAn, string vaiTro)
        {
            string query = "INSERT INTO NHANVIEN (IDNV, TENNV, NGAYSINH, DIACHI, LUONG, IDPB, IDDA) " +
                           "VALUES (@IDNV, @TENNV, @NGAYSINH, @DIACHI, @LUONG, @IDPB, @IDDA)";

            string queryPhanCong = "INSERT INTO PHANCONG (IDNV, IDDA, VAITRO) VALUES (@IDNV, @IDDA, @VAITRO)";
            using (SqlCommand cmd = new SqlCommand(query, _conn))
            using (SqlCommand cmdPhanCong = new SqlCommand(queryPhanCong, _conn))
            {
                cmd.Parameters.AddWithValue("@IDNV", idNhanVien);
                cmd.Parameters.AddWithValue("@TENNV", tenNhanVien);
                cmd.Parameters.AddWithValue("@NGAYSINH", nsNV.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@DIACHI", dcNV);
                cmd.Parameters.AddWithValue("@LUONG", luongNV);
                cmd.Parameters.AddWithValue("@IDPB", idPhongBan);
                cmd.Parameters.AddWithValue("@IDDA", idDuAn);

                cmdPhanCong.Parameters.AddWithValue("@IDNV", idNhanVien);
                cmdPhanCong.Parameters.AddWithValue("@IDDA", idDuAn);
                cmdPhanCong.Parameters.AddWithValue("@VAITRO", vaiTro);

                _conn.Open();
                cmd.ExecuteNonQuery();
                cmdPhanCong.ExecuteNonQuery();
                _conn.Close();
            }
        }
        public bool UpdateNhanVien(string idNhanVien, string tenNhanVien, DateTime nsNV, string dcNV, string luongNV, string idPhongBan, string idDuAn, string vaiTro)
        {
            string query = "UPDATE NHANVIEN SET TENNV = @TENNV, NGAYSINH = @NGAYSINH, DIACHI = @DIACHI, LUONG = @LUONG, IDPB = @IDPB, IDDA = @IDDA WHERE IDNV = @IDNV";
            string queryPhanCong = "UPDATE PHANCONG SET VAITRO = @VAITRO WHERE IDNV = @IDNV AND IDDA = @IDDA";

            using (SqlCommand cmd = new SqlCommand(query, _conn))
            using (SqlCommand cmdPhanCong = new SqlCommand(queryPhanCong, _conn))
            {
                cmd.Parameters.AddWithValue("@IDNV", idNhanVien);
                cmd.Parameters.AddWithValue("@TENNV", tenNhanVien);
                cmd.Parameters.AddWithValue("@NGAYSINH", nsNV.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@DIACHI", dcNV);
                cmd.Parameters.AddWithValue("@LUONG", luongNV);
                cmd.Parameters.AddWithValue("@IDPB", idPhongBan);
                cmd.Parameters.AddWithValue("@IDDA", idDuAn);

                cmdPhanCong.Parameters.AddWithValue("@IDNV", idNhanVien);
                cmdPhanCong.Parameters.AddWithValue("@IDDA", idDuAn);
                cmdPhanCong.Parameters.AddWithValue("@VAITRO", vaiTro);

                _conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                int rowsAffected2 = cmdPhanCong.ExecuteNonQuery();
                _conn.Close();

                return rowsAffected > 0;
            }
        }
        public bool DeleteNhanVien(string idNhanVien)
        {
            string queryPhanCong = "DELETE FROM PHANCONG WHERE IDNV = @IDNV";
            string query = "DELETE FROM NHANVIEN WHERE IDNV = @IDNV";

            using (SqlCommand cmdPhanCong = new SqlCommand(queryPhanCong, _conn))
            using (SqlCommand cmd = new SqlCommand(query, _conn))
            {
                cmdPhanCong.Parameters.AddWithValue("@IDNV", idNhanVien);
                cmd.Parameters.AddWithValue("@IDNV", idNhanVien);

                _conn.Open();
                cmdPhanCong.ExecuteNonQuery();
                int rowsAffected = cmd.ExecuteNonQuery();
                _conn.Close();

                return rowsAffected > 0;
            }
        }
        public DataTable GetNhanVienByTenID(string timKiem)
        {
            string query = @"SELECT NV.IDNV, NV.TENNV, NV.NGAYSINH, NV.DIACHI, NV.LUONG, 
                            PB.TENPB, DA.TENDA, PC.VAITRO
                     FROM NHANVIEN NV
                     JOIN PHONGBAN PB ON NV.IDPB = PB.IDPB
                     JOIN DUAN DA ON NV.IDDA = DA.IDDA
                     JOIN PHANCONG PC ON NV.IDNV = PC.IDNV
                     WHERE NV.TENNV LIKE @TimKiem OR NV.IDNV LIKE @TimKiem";

            using (SqlCommand cmd = new SqlCommand(query, _conn))
            {
                cmd.Parameters.Add("@TimKiem", SqlDbType.NVarChar).Value = timKiem + "%"; // Sửa lỗi truyền tham số

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.SelectCommand.Connection = _conn; // Đảm bảo chỉ định Connection
                    da.Fill(dt);
                    return dt;
                }
            }
        }
        public DataTable LayDanhSachNhanVien()
        {
            string query = "SELECT IDNV, TENNV, LUONG FROM NHANVIEN";
            SqlCommand cmd = new SqlCommand(query, _conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            _conn.Open();
            da.Fill(dt);
            _conn.Close();
            return dt;
        }

    }
}
