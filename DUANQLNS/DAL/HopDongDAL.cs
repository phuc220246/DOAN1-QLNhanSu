using DUANQLNS.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUANQLNS.DAL
{
    public class HopDongDAL
    {
        private SqlConnection _conn;
        public HopDongDAL() // kết nối với csdl = cách lấy chuỗi kết nối sẳn từ DataHelper
        {
            _conn = new SqlConnection(DataHelper.Instance.ConnectionString);
        }
        public List<HopDongDTO> LoadHopDongList()
        {
            List<HopDongDTO> hopDongList = new List<HopDongDTO>();
            string query = "USP_GetHopDongWithDetails"; // Gọi Stored Procedure

            DataTable data = DataHelper.Instance.ExecuteQuery(query);
            foreach (DataRow row in data.Rows)
            {
                HopDongDTO hopDong = new HopDongDTO(row);
                hopDongList.Add(hopDong);
            }

            return hopDongList;
        }
        public DataTable LayTenLoaiHDList()
        {
            string query = "SELECT IDLOAIHD, TENLOAIHD FROM LOAIHOPDONG";
            SqlCommand cmd = new SqlCommand(query, _conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            _conn.Open();
            da.Fill(dt);
            _conn.Close();
            return dt;
        }
        public DataTable LayTrangThaiHDList()
        {
            string query = "SELECT IDTRANGTHAI, TENTRANGTHAI FROM TRANGTHAIHOPDONG";
            SqlCommand cmd = new SqlCommand(query, _conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            _conn.Open();
            da.Fill(dt);
            _conn.Close();
            return dt;
        }
        public void AddHopDong(string idHopDong, DateTime ngayBD, DateTime ngayKT, string moTa, string idNhanVien, string idLoaiHD, string idTrangThai)
        {
            string query = "INSERT INTO HOPDONG (IDHD, NGAYBDHD, NGAYKTHD, MOTAHD, IDNV, IDLOAIHD, IDTRANGTHAI) " +
                           "VALUES (@IDHD, @NGAYBDHD, @NGAYKTHD, @MOTAHD, @IDNV, @IDLOAIHD, @IDTRANGTHAI)";

            using (SqlCommand cmd = new SqlCommand(query, _conn))
            {
                cmd.Parameters.AddWithValue("@IDHD", idHopDong);
                cmd.Parameters.AddWithValue("@NGAYBDHD", ngayBD.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@NGAYKTHD", ngayKT.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@MOTAHD", moTa);
                cmd.Parameters.AddWithValue("@IDNV", idNhanVien);
                cmd.Parameters.AddWithValue("@IDLOAIHD", idLoaiHD);
                cmd.Parameters.AddWithValue("@IDTRANGTHAI", idTrangThai);

                _conn.Open();
                cmd.ExecuteNonQuery();
                _conn.Close();
            }
        }
        public bool UpdateHopDong(string idHopDong, DateTime ngayBD, DateTime ngayKT, string moTa, string idNhanVien, string idLoaiHD, string idTrangThai)
        {
            string query = "UPDATE HOPDONG SET NGAYBDHD = @NGAYBDHD, NGAYKTHD = @NGAYKTHD, MOTAHD = @MOTAHD, " +
                           "IDNV = @IDNV, IDLOAIHD = @IDLOAIHD, IDTRANGTHAI = @IDTRANGTHAI WHERE IDHD = @IDHD";

            using (SqlCommand cmd = new SqlCommand(query, _conn))
            {
                cmd.Parameters.AddWithValue("@IDHD", idHopDong);
                cmd.Parameters.AddWithValue("@NGAYBDHD", ngayBD.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@NGAYKTHD", ngayKT.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@MOTAHD", moTa);
                cmd.Parameters.AddWithValue("@IDNV", idNhanVien);
                cmd.Parameters.AddWithValue("@IDLOAIHD", idLoaiHD);
                cmd.Parameters.AddWithValue("@IDTRANGTHAI", idTrangThai);

                _conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                _conn.Close();

                return rowsAffected > 0; // Trả về true nếu có ít nhất 1 dòng bị ảnh hưởng
            }
        }
        public bool DeleteHopDong(string idHopDong)
        {
            string query = "DELETE FROM HOPDONG WHERE IDHD = @IDHD";

            using (SqlCommand cmd = new SqlCommand(query, _conn))
            {
                cmd.Parameters.AddWithValue("@IDHD", idHopDong);

                _conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                _conn.Close();

                return rowsAffected > 0; // Nếu xóa thành công, trả về true
            }
        }
        public DataTable GetHopDongByTenID(string timKiem)
        {
            string query = @"SELECT HD.IDHD, HD.NGAYBDHD, HD.NGAYKTHD, HD.MOTAHD, 
                                NV.TENNV, NV.IDNV, LHD.TENLOAIHD, TT.TENTRANGTHAI
                         FROM HOPDONG HD
                         JOIN NHANVIEN NV ON HD.IDNV = NV.IDNV
                         JOIN LOAIHOPDONG LHD ON HD.IDLOAIHD = LHD.IDLOAIHD
                         JOIN TRANGTHAIHOPDONG TT ON HD.IDTRANGTHAI = TT.IDTRANGTHAI
                         WHERE HD.IDHD LIKE @TimKiem OR NV.TENNV LIKE @TimKiem OR NV.IDNV LIKE @TimKiem";

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
