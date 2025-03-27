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
    public class LuongDAL
    {
        private SqlConnection _conn;
        public LuongDAL() // kết nối với csdl = cách lấy chuỗi kết nối sẳn từ DataHelper
        {
            _conn = new SqlConnection(DataHelper.Instance.ConnectionString);
        }
        public List<LuongDTO> LoadLuongList()
        {
            List<LuongDTO> luongList = new List<LuongDTO>();
            string query = "USP_GetLuongWithNhanVien"; // Gọi Stored Procedure

            DataTable data = DataHelper.Instance.ExecuteQuery(query);
            foreach (DataRow row in data.Rows)
            {
                LuongDTO luong = new LuongDTO(row);
                luongList.Add(luong);
            }

            return luongList;
        }
        public DataTable LayTenNhanVienList()
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
        public bool TinhLuong(DateTime ngayBD, DateTime ngayKT, int soNgayNghi, int luongNgay, string idNV)
        {
            bool isSuccess = false;
            try
            {
                _conn.Open();

                // Tính số ngày làm việc
                int soNgayLamViec = (ngayKT - ngayBD).Days + 1 - soNgayNghi;
                if (soNgayLamViec < 0) soNgayLamViec = 0;

                // Tính tổng lương
                int tongLuong = soNgayLamViec * luongNgay;

                // Chèn dữ liệu vào bảng LUONG
                string query = "INSERT INTO LUONG (NGAYBD, NGAYKT, SONGAYNGHI, TONGLUONG, IDNV) " +
                               "VALUES (@NGAYBD, @NGAYKT, @SONGAYNGHI, @TONGLUONG, @IDNV)";
                SqlCommand cmd = new SqlCommand(query, _conn);
                cmd.Parameters.AddWithValue("@NGAYBD", ngayBD);
                cmd.Parameters.AddWithValue("@NGAYKT", ngayKT);
                cmd.Parameters.AddWithValue("@SONGAYNGHI", soNgayNghi);
                cmd.Parameters.AddWithValue("@TONGLUONG", tongLuong);
                cmd.Parameters.AddWithValue("@IDNV", idNV);

                int rowsAffected = cmd.ExecuteNonQuery();
                isSuccess = rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tính lương: " + ex.Message);
            }
            finally
            {
                _conn.Close();
            }
            return isSuccess;
        }
        public bool UpdateLuong(int idLuong, DateTime ngayBD, DateTime ngayKT, int soNgayNghi, int luongNgay, string idNV)
        {
            bool isSuccess = false;
            try
            {
                _conn.Open();

                // Tính số ngày làm việc
                int soNgayLamViec = (ngayKT - ngayBD).Days + 1 - soNgayNghi;
                if (soNgayLamViec < 0) soNgayLamViec = 0;

                // Tính tổng lương
                int tongLuong = soNgayLamViec * luongNgay;

                // Cập nhật dữ liệu trong bảng LUONG
                string query = "UPDATE LUONG " +
                               "SET NGAYBD = @NGAYBD, NGAYKT = @NGAYKT, SONGAYNGHI = @SONGAYNGHI, " +
                               "TONGLUONG = @TONGLUONG, IDNV = @IDNV " +
                               "WHERE IDLUONG = @IDLUONG";
                using (SqlCommand cmd = new SqlCommand(query, _conn))
                {
                    cmd.Parameters.AddWithValue("@IDLUONG", idLuong);
                    cmd.Parameters.AddWithValue("@NGAYBD", ngayBD);
                    cmd.Parameters.AddWithValue("@NGAYKT", ngayKT);
                    cmd.Parameters.AddWithValue("@SONGAYNGHI", soNgayNghi);
                    cmd.Parameters.AddWithValue("@TONGLUONG", tongLuong);
                    cmd.Parameters.AddWithValue("@IDNV", idNV);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    isSuccess = rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi cập nhật lương: " + ex.Message);
            }
            finally
            {
                _conn.Close();
            }
            return isSuccess;
        }
        public bool DeleteLuong(int idLuong)
        {
            string query = "DELETE FROM LUONG WHERE IDLUONG = @IDLUONG";

            using (SqlCommand cmd = new SqlCommand(query, _conn))
            {
                cmd.Parameters.AddWithValue("@IDLUONG", idLuong);

                _conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                _conn.Close();

                return rowsAffected > 0;
            }
        }
        public DataTable GetLuongByTenID(string timKiem)
        {
            string query = @"SELECT L.IDLUONG, L.NGAYBD, L.NGAYKT, L.SONGAYNGHI, 
                            L.TONGLUONG, NV.TENNV, NV.LUONG
                     FROM LUONG L
                     JOIN NHANVIEN NV ON L.IDNV = NV.IDNV
                     WHERE NV.TENNV LIKE @TimKiem OR NV.IDNV LIKE @TimKiem";

            using (SqlCommand cmd = new SqlCommand(query, _conn))
            {
                cmd.Parameters.Add("@TimKiem", SqlDbType.NVarChar).Value = "%" + timKiem + "%"; // Tìm kiếm gần đúng

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.SelectCommand.Connection = _conn; // Chỉ định Connection
                    da.Fill(dt);
                    return dt;
                }
            }
        }


    }
}
