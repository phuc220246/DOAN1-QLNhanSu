using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DUANQLNS.BLL;
using DUANQLNS.DAL;
using DUANQLNS.DTO;
using DUANQLNS.Report;
using Microsoft.ReportingServices.ReportProcessing.ExprHostObjectModel;


namespace DUANQLNS
{
    public partial class Main : Form
    {
        private TaiKhoanBLL _taiKhoanBLL;
        private PhongBanBLL _phongBanBLL;
        private DuAnBLL _duAnBLL;
        private NhanVienBLL _nhanVienBLL;
        private LuongBLL _luongBLL;
        private HopDongBLL _hopDongBLL;

        public Main()
        {
            InitializeComponent();
            _taiKhoanBLL = new TaiKhoanBLL();
            _phongBanBLL = new PhongBanBLL();
            _duAnBLL = new DuAnBLL();
            _nhanVienBLL = new NhanVienBLL();
            _luongBLL = new LuongBLL();
            _hopDongBLL = new HopDongBLL();
            LoadTaiKhoan();
            LoadPhongBan();
            LoadDuAn();
            LayTenPhongBan();
            LayTenDuAn();
            LoadNhanVien();
            LoadLuong();
            LayTenNhanVien();
            cmbLuong.SelectedIndexChanged += cboTenNhanVien_SelectedIndexChanged; // (tự động load lương theo tên nhân viên)
            LoadHopDong();
            LoadComboBoxNhanVien();
            LoadComboBoxLoaiHD();
            LoadComboBoxTrangThaiHD();
        }
        #region tab TaiKhoan
        private void LoadTaiKhoan()
        {
            _taiKhoanBLL = new TaiKhoanBLL();
            List<TaiKhoanDTO> taiKhoanList = _taiKhoanBLL.GetTaiKhoanList();
            dataGridViewtk.DataSource = taiKhoanList;
            dataGridViewtk.Columns[0].HeaderText = "Mã tài khoản";
            dataGridViewtk.Columns[1].HeaderText = "Tên tài khoản";
            dataGridViewtk.Columns[2].HeaderText = "Mật khẩu";
            dataGridViewtk.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewtk.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 10, FontStyle.Bold);
        }
        private void btnThemTK_Click(object sender, EventArgs e)
        {
            try
            {
                string idTaiKhoan = txtMaTK.Text;
                string tenTaiKhoan = txtTenTK.Text;
                string matKhau = txtMatkhauTK.Text;

                _taiKhoanBLL.AddTaiKhoan(idTaiKhoan, tenTaiKhoan, matKhau);

                // Cập nhật danh sách tài khoản sau khi thêm
                LoadTaiKhoan();
                MessageBox.Show("Thêm tài khoản thành công!");
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void dataGridViewtk_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int index;
            index = e.RowIndex;
            DataGridViewRow selectedrow = dataGridViewtk.Rows[index];
            txtMaTK.Text = selectedrow.Cells[0].Value.ToString();
            txtTenTK.Text = selectedrow.Cells[1].Value.ToString();
            txtMatkhauTK.Text = selectedrow.Cells[2].Value.ToString();
            txtMaTK.Enabled = false;
            btnThemTK.Enabled = false;
        }
        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtMaTK.Text = "";
            txtTenTK.Text = "";
            txtMatkhauTK.Text = "";
            txtTimkiemTK.Text = " ";
            txtMaTK.Enabled = true;
            btnThemTK.Enabled = true;
        }
        private void btnSuaTK_Click(object sender, EventArgs e)
        {
            TaiKhoanBLL taiKhoanBLL = new TaiKhoanBLL();

            // Lấy dữ liệu từ các TextBox
            string idTaiKhoan = txtMaTK.Text;
            string tenTaiKhoan = txtTenTK.Text;
            string matKhau = txtMatkhauTK.Text;

            // Gọi phương thức BLL để cập nhật tài khoản
            bool isUpdated = taiKhoanBLL.UpdateTaiKhoan(idTaiKhoan, tenTaiKhoan, matKhau);

            if (isUpdated)
            {
                MessageBox.Show("Sửa thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadTaiKhoan(); // Cập nhật lại dữ liệu hiển thị trên giao diện
            }
            else
            {
                MessageBox.Show("Sửa thất bại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnXoaTK_Click_1(object sender, EventArgs e)
        {
            string maTaiKhoan = txtMaTK.Text;
            string tenTaiKhoan = txtTenTK.Text;
            DialogResult result = MessageBox.Show(
                $"Bạn chắc chắn muốn xóa tài khoản '{tenTaiKhoan}'?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result == DialogResult.Yes)
            {
                TaiKhoanBLL taiKhoanBLL = new TaiKhoanBLL();
                bool isDeleted = taiKhoanBLL.DeleteTaiKhoan(maTaiKhoan);

                if (isDeleted)
                {
                    LoadTaiKhoan();
                    MessageBox.Show("Xóa thành công!", "Thông báo");
                }
            }
            else
            {
                MessageBox.Show("Hủy thao tác xóa.", "Thông báo");
            }
        }
        private void btnTimkiemtk_Click(object sender, EventArgs e)
        {
            string timKiem = txtTimkiemTK.Text;

            // Gọi lớp BLL để tìm kiếm tài khoản
            TaiKhoanBLL taiKhoanBLL = new TaiKhoanBLL();
            DataTable dt = taiKhoanBLL.TimKiemTaiKhoan(timKiem);

            // Hiển thị dữ liệu lên DataGridView
            dataGridViewtk.DataSource = dt;

            // Đặt lại tiêu đề cột và định dạng DataGridView
            dataGridViewtk.Columns["IDTAIKHOAN"].HeaderText = "Mã tài khoản";
            dataGridViewtk.Columns["TENTAIKHOAN"].HeaderText = "Tên tài khoản";
            dataGridViewtk.Columns["MATKHAU"].HeaderText = "Mật khẩu";
            dataGridViewtk.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewtk.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 10, FontStyle.Bold);
        }
        private void btnShowAllTaiKhoan_Click(object sender, EventArgs e)
        {
            LoadTaiKhoan();
        }
        #endregion tab tai khoan

        #region tab PhongBan
        private void LoadPhongBan()
        {
            _phongBanBLL = new PhongBanBLL();
            List<PhongBanDTO> phongBanList = _phongBanBLL.GetPhongBanList();
            dataGridViewpb.DataSource = phongBanList;
            dataGridViewpb.Columns[0].HeaderText = "Mã Phòng Ban";
            dataGridViewpb.Columns[1].HeaderText = "Tên Phòng Ban";
            dataGridViewpb.Columns[2].HeaderText = "Số lượng nhân viên";
            dataGridViewpb.Columns[3].HeaderText = "Mô Tả";
            dataGridViewpb.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewpb.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 10, FontStyle.Bold);
        }
        private void btnThemPB_Click(object sender, EventArgs e)
        {
            try
            {
                string idPhongBan = txtMaPB.Text;
                string tenPhongBan = txtTenPB.Text;
                string soluongNVPB = txtSlnvPB.Text;
                string mota = txtMoTaPB.Text;

                _phongBanBLL.AddPhongBan(idPhongBan, tenPhongBan, soluongNVPB, mota);

                // Cập nhật danh sách phòng ban sau khi thêm
                LoadPhongBan();
                MessageBox.Show("Thêm phòng ban thành công!");
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void dataGridViewpb_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int index;
            index = e.RowIndex;
            DataGridViewRow selectedrow = dataGridViewpb.Rows[index];
            txtMaPB.Text = selectedrow.Cells[0].Value.ToString();
            txtTenPB.Text = selectedrow.Cells[1].Value.ToString();
            txtSlnvPB.Text = selectedrow.Cells[2].Value.ToString();
            txtMoTaPB.Text = selectedrow.Cells[3].Value.ToString();
            txtMaPB.Enabled = false;
            btnThemPB.Enabled = false;
        }
        private void btnLamMoiPB_Click(object sender, EventArgs e)
        {
            txtMaPB.Text = "";
            txtTenPB.Text = "";
            txtSlnvPB.Text = "";
            txtMoTaPB.Text = "";
            txtTimKiemPB.Text = " ";
            txtMaPB.Enabled = true;
            btnThemPB.Enabled = true;
        }
        private void btnSuaPB_Click(object sender, EventArgs e)
        {
            PhongBanBLL phongBanBLL = new PhongBanBLL();

            // Lấy dữ liệu từ các TextBox
            string idPhongBan = txtMaPB.Text;
            string tenPhongBan = txtTenPB.Text;
            string soluongNVPB = txtSlnvPB.Text;
            string mota = txtMoTaPB.Text;

            // Gọi phương thức BLL để cập nhật tài khoản
            bool isUpdated = phongBanBLL.UpdatePhongBan(idPhongBan, tenPhongBan, soluongNVPB, mota);

            if (isUpdated)
            {
                MessageBox.Show("Sửa thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadPhongBan(); // Cập nhật lại dữ liệu hiển thị trên giao diện
            }
            else
            {
                MessageBox.Show("Sửa thất bại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnXoaPB_Click(object sender, EventArgs e)
        {
            string idPhongBan = txtMaPB.Text;
            string tenPhongBan = txtTenPB.Text;
            DialogResult result = MessageBox.Show(
                $"Bạn chắc chắn muốn xóa phòng ban '{tenPhongBan}'?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result == DialogResult.Yes)
            {
                PhongBanBLL phongBanBLL = new PhongBanBLL();
                bool isDeleted = phongBanBLL.DeletePhongBan(idPhongBan);

                if (isDeleted)
                {
                    LoadPhongBan();
                    MessageBox.Show("Xóa thành công!", "Thông báo");
                }
            }
            else
            {
                MessageBox.Show("Hủy thao tác xóa.", "Thông báo");
            }
        }
        private void btnTimKiemPB_Click(object sender, EventArgs e)
        {
            string timKiem = txtTimKiemPB.Text;

            // Gọi lớp BLL để tìm kiếm tài khoản
            PhongBanBLL phongBanBLL = new PhongBanBLL();
            DataTable dt = phongBanBLL.TimKiemPhongBan(timKiem);

            // Hiển thị dữ liệu lên DataGridView
            dataGridViewpb.DataSource = dt;

            // Đặt lại tiêu đề cột và định dạng DataGridView
            dataGridViewpb.Columns["IDPB"].HeaderText = "Mã Phòng ban";
            dataGridViewpb.Columns["TENPB"].HeaderText = "Tên phòng ban";
            dataGridViewpb.Columns["SOLUONGNVPB"].HeaderText = "Số lượng nhân viên";
            dataGridViewpb.Columns["MOTAPB"].HeaderText = "Mô tả";
            dataGridViewpb.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewpb.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 10, FontStyle.Bold);
        }
        private void btnShowAllPB_Click(object sender, EventArgs e)
        {
            LoadPhongBan();
        }
        #endregion tab PhongBan

        #region tab DuAn 
        private void LoadDuAn()
        {
            _duAnBLL = new DuAnBLL();
            List<DuAnDTO> duAnList = _duAnBLL.GetDuAnList();
            dataGridViewda.DataSource = duAnList;
            dataGridViewda.Columns[0].HeaderText = "Mã Dự án";
            dataGridViewda.Columns[1].HeaderText = "Tên Dự án";
            dataGridViewda.Columns[2].HeaderText = "Số lượng nhân viên";
            dataGridViewda.Columns[3].HeaderText = "Mô Tả dự án";
            dataGridViewda.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewda.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 10, FontStyle.Bold);
        }
        private void btnThemDA_Click(object sender, EventArgs e)
        {
            try
            {
                string idDuAn = txtMaDA.Text;
                string tenDuAn = txtTenDA.Text;
                string soluongNVDA = txtSlnvDA.Text;
                string motaDA = txtMoTaDA.Text;

                _duAnBLL.AddDuAn(idDuAn, tenDuAn, soluongNVDA, motaDA);

                // Cập nhật danh sách phòng ban sau khi thêm
                LoadDuAn();
                MessageBox.Show("Thêm dự án thành công!");
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void dataGridViewda_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int index;
            index = e.RowIndex;
            DataGridViewRow selectedrow = dataGridViewda.Rows[index];
            txtMaDA.Text = selectedrow.Cells[0].Value.ToString();
            txtTenDA.Text = selectedrow.Cells[1].Value.ToString();
            txtSlnvDA.Text = selectedrow.Cells[2].Value.ToString();
            txtMoTaDA.Text = selectedrow.Cells[3].Value.ToString();
            txtMaDA.Enabled = false;
            btnThemDA.Enabled = false;
        }
        private void btnLamMoiDA_Click(object sender, EventArgs e)
        {
            txtMaDA.Text = "";
            txtTenDA.Text = "";
            txtSlnvDA.Text = "";
            txtMoTaDA.Text = "";
            txtTimKiemDA.Text = " ";
            txtMaDA.Enabled = true;
            btnThemDA.Enabled = true;
        }
        private void btnSuaDA_Click(object sender, EventArgs e)
        {
            DuAnBLL duAnBLL = new DuAnBLL();

            // Lấy dữ liệu từ các TextBox
            string idDuAn = txtMaDA.Text;
            string tenDuAn = txtTenDA.Text;
            string soluongNVDA = txtSlnvDA.Text;
            string motaDA = txtMoTaDA.Text;

            // Gọi phương thức BLL để cập nhật tài khoản
            bool isUpdated = duAnBLL.UpdateDuAn(idDuAn, tenDuAn, soluongNVDA, motaDA);

            if (isUpdated)
            {
                MessageBox.Show("Sửa thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDuAn(); // Cập nhật lại dữ liệu hiển thị trên giao diện
            }
            else
            {
                MessageBox.Show("Sửa thất bại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnXoaDA_Click(object sender, EventArgs e)
        {
            string idDuAn = txtMaDA.Text;
            string tenDuAn = txtTenDA.Text;
            DialogResult result = MessageBox.Show(
                $"Bạn chắc chắn muốn xóa dự án '{tenDuAn}'?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result == DialogResult.Yes)
            {
                DuAnBLL duAnBLL = new DuAnBLL();
                bool isDeleted = duAnBLL.DeleteDuAn(idDuAn);

                if (isDeleted)
                {
                    LoadDuAn();
                    MessageBox.Show("Xóa thành công!", "Thông báo");
                }
            }
            else
            {
                MessageBox.Show("Hủy thao tác xóa.", "Thông báo");
            }
        }
        private void btnTimKiemDA_Click(object sender, EventArgs e)
        {
            string timKiem = txtTimKiemDA.Text;

            // Gọi lớp BLL để tìm kiếm tài khoản
            DuAnBLL duAnBLL = new DuAnBLL();
            DataTable dt = duAnBLL.TimKiemDuAn(timKiem);

            // Hiển thị dữ liệu lên DataGridView
            dataGridViewda.DataSource = dt;

            // Đặt lại tiêu đề cột và định dạng DataGridView
            dataGridViewda.Columns["IDDA"].HeaderText = "Mã dự án";
            dataGridViewda.Columns["TENDA"].HeaderText = "Tên dự án";
            dataGridViewda.Columns["SOLUONGNVDA"].HeaderText = "Số lượng nhân viên";
            dataGridViewda.Columns["MOTADA"].HeaderText = "Mô tả";
            dataGridViewda.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewda.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 10, FontStyle.Bold);
        }
        private void btnShowAllDA_Click(object sender, EventArgs e)
        {
            LoadDuAn();
        }
        #endregion tab DuAn

        #region tab NhanVien
        private void LoadNhanVien()
        {
            _nhanVienBLL = new NhanVienBLL();
            List<NhanVienDTO> nhanVienList = _nhanVienBLL.GetNhanVienList();
            dataGridViewnv.DataSource = nhanVienList;
            dataGridViewnv.Columns[0].HeaderText = "Mã nhân viên";
            dataGridViewnv.Columns[1].HeaderText = "Họ tên ";
            dataGridViewnv.Columns[2].HeaderText = "Ngày sinh";
            dataGridViewnv.Columns[3].HeaderText = "Địa chỉ";
            dataGridViewnv.Columns[4].HeaderText = "Lương";
            dataGridViewnv.Columns[5].HeaderText = "Tên phòng ban";
            dataGridViewnv.Columns[6].HeaderText = "Tên dự án";
            dataGridViewnv.Columns[7].HeaderText = "Vai trò";
            dataGridViewnv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewnv.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 10, FontStyle.Bold);
        }
        private void LayTenPhongBan()
        {
            _nhanVienBLL = new NhanVienBLL();
            DataTable dt = _nhanVienBLL.GetTenPhongBanList();

            DataRow newRow = dt.NewRow();
            newRow["IDPB"] = DBNull.Value;
            newRow["TENPB"] = "--Chọn phòng--";
            dt.Rows.InsertAt(newRow, 0);
            cmbPB.DataSource = dt;
            cmbPB.DisplayMember = "TENPB";
            cmbPB.ValueMember = "IDPB";
            cmbPB.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbPB.SelectedIndex = 0;

        }
        private void LayTenDuAn()
        {
            _nhanVienBLL = new NhanVienBLL();
            DataTable dt = _nhanVienBLL.GetTenDuAnList();

            DataRow newRow = dt.NewRow();
            newRow["IDDA"] = DBNull.Value;
            newRow["TENDA"] = "--Chọn dự án--";
            dt.Rows.InsertAt(newRow, 0);
            cmbDA.DataSource = dt;
            cmbDA.DisplayMember = "TENDA";
            cmbDA.ValueMember = "IDDA";
            cmbDA.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbDA.SelectedIndex = 0;

        }
        private void btnThemNV_Click_1(object sender, EventArgs e)
        {
            try
            {
                string idNhanVien = txtMaNV.Text;
                string tenNhanVien = txtTenNV.Text;
                DateTime nsNV = dtpNgaySinhNV.Value;
                string dcNV = txtDiaChiNV.Text;
                string luongNV = txtLuongNV.Text;
                string tenPhongBan = cmbPB.SelectedValue?.ToString();
                string tenDuAn = cmbDA.SelectedValue?.ToString();
                string vaiTro = txtVaiTroNV.Text;


                if (string.IsNullOrEmpty(tenPhongBan) || tenPhongBan == "--Chọn phòng--")
                {
                    MessageBox.Show("Vui lòng chọn phòng ban hợp lệ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (string.IsNullOrEmpty(tenDuAn) || tenDuAn == "--Chọn dự án--")
                {
                    MessageBox.Show("Vui lòng chọn dự án hợp lệ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (string.IsNullOrWhiteSpace(vaiTro))
                {
                    MessageBox.Show("Vai trò không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (nsNV == DateTime.MinValue)
                {
                    MessageBox.Show("Ngày sinh không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Hiển thị ngày sinh để kiểm tra trước khi lưu
                MessageBox.Show($"Ngày sinh được chọn: {nsNV.ToString("yyyy-MM-dd HH:mm:ss")}", "Thông tin kiểm tra", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _nhanVienBLL.AddNhanVien(idNhanVien, tenNhanVien, nsNV, dcNV, luongNV, tenPhongBan, tenDuAn, vaiTro);

                LoadNhanVien();



                MessageBox.Show("Thêm nhân viên thành công!");

            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void dataGridViewnv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int index;
            index = e.RowIndex;
            DataGridViewRow selectedrow = dataGridViewnv.Rows[index];
            txtMaNV.Text = selectedrow.Cells[0].Value?.ToString();
            txtTenNV.Text = selectedrow.Cells[1].Value?.ToString();

            if (selectedrow.Cells[2].Value != null)
                dtpNgaySinhNV.Value = Convert.ToDateTime(selectedrow.Cells[2].Value);

            txtDiaChiNV.Text = selectedrow.Cells[3].Value?.ToString();
            txtLuongNV.Text = selectedrow.Cells[4].Value?.ToString();

            // Lấy danh sách phòng ban từ BLL
            DataTable dtPhongBan = _nhanVienBLL.GetTenPhongBanList();

            // Tìm IDPB tương ứng với tên phòng ban
            if (selectedrow.Cells[5].Value != null)
            {
                DataRow rowPB = dtPhongBan.AsEnumerable()
                    .FirstOrDefault(row => row["TENPB"].ToString() == selectedrow.Cells[5].Value.ToString());

                if (rowPB != null)
                    cmbPB.SelectedValue = rowPB["IDPB"].ToString();
            }

            DataTable dtDuAn = _nhanVienBLL.GetTenDuAnList();
            if (selectedrow.Cells[6].Value != null)
            {
                DataRow rowDA = dtDuAn.AsEnumerable()
                    .FirstOrDefault(row => row["TENDA"].ToString() == selectedrow.Cells[6].Value.ToString());

                if (rowDA != null)
                    cmbDA.SelectedValue = rowDA["IDDA"].ToString();
            }
            txtVaiTroNV.Text = selectedrow.Cells[7].Value?.ToString();
            txtMaNV.Enabled = false;
            btnThemNV.Enabled = false;
        }
        private void btnLamMoiNV_Click(object sender, EventArgs e)
        {
            txtMaNV.Text = "";
            txtTenNV.Text = "";
            txtDiaChiNV.Text = "";
            txtLuongNV.Text = "";
            txtTimKiemNV.Text = "";
            txtVaiTroNV.Text = "";

            // Đặt lại giá trị mặc định cho DateTimePicker (ngày hiện tại)
            dtpNgaySinhNV.Value = DateTime.Now;

            // Đặt lại ComboBox về trạng thái mặc định (item đầu tiên)
            if (cmbPB.Items.Count > 0)
                cmbPB.SelectedIndex = 0;

            if (cmbDA.Items.Count > 0)
                cmbDA.SelectedIndex = 0;

            // Bật lại trường nhập mã nhân viên và nút Thêm
            txtMaNV.Enabled = true;
            btnThemNV.Enabled = true;
        }
        private void btnSuaNV_Click(object sender, EventArgs e)
        {
            // Khởi tạo BLL để thao tác với dữ liệu nhân viên
            NhanVienBLL nhanVienBLL = new NhanVienBLL();

            // Lấy dữ liệu từ các TextBox và ComboBox
            string idNhanVien = txtMaNV.Text;
            string tenNhanVien = txtTenNV.Text;
            DateTime nsNV = dtpNgaySinhNV.Value;
            string dcNV = txtDiaChiNV.Text;
            string luongNV = txtLuongNV.Text;
            string tenPhongBan = cmbPB.SelectedValue?.ToString();
            string tenDuAn = cmbDA.SelectedValue?.ToString();
            string vaiTro = txtVaiTroNV.Text.Trim(); // Kiểm tra vai trò

            // Kiểm tra nếu vai trò bị trống
            if (string.IsNullOrWhiteSpace(vaiTro))
            {
                MessageBox.Show("Vai trò không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtVaiTroNV.Focus(); // Đưa con trỏ vào ô Vai Trò
                return;
            }

            // Kiểm tra dữ liệu hợp lệ (có thể thêm kiểm tra rỗng nếu cần)
            if (string.IsNullOrWhiteSpace(idNhanVien) || string.IsNullOrWhiteSpace(tenNhanVien) ||
                string.IsNullOrWhiteSpace(dcNV) || string.IsNullOrWhiteSpace(luongNV) ||
                string.IsNullOrWhiteSpace(tenPhongBan) || string.IsNullOrWhiteSpace(tenDuAn))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Gọi phương thức BLL để cập nhật nhân viên
            bool isUpdated = nhanVienBLL.UpdateNhanVien(idNhanVien, tenNhanVien, nsNV, dcNV, luongNV, tenPhongBan, tenDuAn, vaiTro);

            if (isUpdated)
            {
                MessageBox.Show("Sửa thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadNhanVien(); // Cập nhật lại dữ liệu hiển thị trên giao diện
            }
            else
            {
                MessageBox.Show("Sửa thất bại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnXoaNV_Click(object sender, EventArgs e)
        {
            // Lấy ID nhân viên và tên nhân viên từ TextBox
            string idNhanVien = txtMaNV.Text;
            string tenNhanVien = txtTenNV.Text;

            // Hiển thị hộp thoại xác nhận trước khi xóa
            DialogResult result = MessageBox.Show(
                $"Bạn chắc chắn muốn xóa nhân viên '{tenNhanVien}'?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result == DialogResult.Yes)
            {
                // Gọi BLL để thực hiện xóa
                NhanVienBLL nhanVienBLL = new NhanVienBLL();
                bool isDeleted = nhanVienBLL.DeleteNhanVien(idNhanVien);

                if (isDeleted)
                {
                    LoadNhanVien(); // Cập nhật lại danh sách nhân viên
                    MessageBox.Show("Xóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Xóa thất bại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Hủy thao tác xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void btnTimKiemNV_Click(object sender, EventArgs e)
        {
            // Lấy dữ liệu từ ô tìm kiếm
            string timKiem = txtTimKiemNV.Text;

            // Gọi lớp BLL để tìm kiếm nhân viên
            NhanVienBLL nhanVienBLL = new NhanVienBLL();
            DataTable dt = nhanVienBLL.TimKiemNhanVien(timKiem);

            // Hiển thị dữ liệu lên DataGridView
            dataGridViewnv.DataSource = dt;

            // Đặt lại tiêu đề cột và định dạng DataGridView
            dataGridViewnv.Columns["IDNV"].HeaderText = "Mã nhân viên";
            dataGridViewnv.Columns["TENNV"].HeaderText = "Tên nhân viên";
            dataGridViewnv.Columns["NGAYSINH"].HeaderText = "Ngày sinh";
            dataGridViewnv.Columns["DIACHI"].HeaderText = "Địa chỉ";
            dataGridViewnv.Columns["LUONG"].HeaderText = "Lương";
            dataGridViewnv.Columns["TENPB"].HeaderText = "Phòng ban";
            dataGridViewnv.Columns["TENDA"].HeaderText = "Dự án";
            dataGridViewnv.Columns["VAITRO"].HeaderText = "Vai trò";
            

            // Định dạng DataGridView
            dataGridViewnv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewnv.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 10, FontStyle.Bold);
        }
        private void btnShowAllNV_Click(object sender, EventArgs e)
        {
            LoadNhanVien();
            LayTenPhongBan();
            LayTenDuAn();
        }
        private void btnXemDSNV_Click(object sender, EventArgs e)
        {
            RPNhanVien rpnv = new RPNhanVien("XemDSNV");
            rpnv.Show();
        }
        
        #endregion tab NhanVien

        #region tab TinhLuong
        private void LoadLuong()
        {
            List<LuongDTO> luongList = _luongBLL.GetLuongList();
            dataGridViewL.DataSource = luongList;

            dataGridViewL.Columns[0].HeaderText = "Mã lương";      // IDLUONG
            dataGridViewL.Columns[1].HeaderText = "Ngày bắt đầu";  // NGAYBD
            dataGridViewL.Columns[2].HeaderText = "Ngày kết thúc"; // NGAYKT
            dataGridViewL.Columns[3].HeaderText = "Số ngày nghỉ";  // SONGAYNGHI
            dataGridViewL.Columns[4].HeaderText = "Tổng lương";    // TONGLUONG
            dataGridViewL.Columns[5].HeaderText = "Tên nhân viên"; // TENNV
            dataGridViewL.Columns[6].HeaderText = "Lương/ngày";    // LUONG (Đúng cột)

            dataGridViewL.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewL.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 10, FontStyle.Bold);
        }
        private void LayTenNhanVien()
        {
            NhanVienBLL nhanVienBLL = new NhanVienBLL();
            DataTable dtnv = nhanVienBLL.GetDanhSachNhanVien();

            // Thêm dòng "--Chọn nhân viên--"
            DataRow newRow = dtnv.NewRow();
            newRow["IDNV"] = DBNull.Value;
            newRow["TENNV"] = "--Chọn nhân viên--";
            dtnv.Rows.InsertAt(newRow, 0);

            cmbLuong.DataSource = dtnv;
            cmbLuong.DisplayMember = "TENNV"; // Hiển thị tên
            cmbLuong.ValueMember = "IDNV";    // Lưu ID
            cmbLuong.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbLuong.SelectedIndex = 0;
        }
        private void cboTenNhanVien_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbLuong.SelectedIndex != -1)
            {
                DataRowView drv = (DataRowView)cmbLuong.SelectedItem;
                txtLuong.Text = drv["LUONG"].ToString(); // Lấy lương từ cột LUONG
            }
        }
        private void btnTinhL_Click(object sender, EventArgs e)
        {

            try
            {
                string idNV = cmbLuong.SelectedValue?.ToString();
                if (string.IsNullOrWhiteSpace(idNV))
                {
                    MessageBox.Show("Vui lòng chọn nhân viên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DateTime ngayBD = dtpNgayBD.Value;
                DateTime ngayKT = dtpNgayKT.Value;

                if (ngayBD > ngayKT)
                {
                    MessageBox.Show("Ngày bắt đầu không thể lớn hơn ngày kết thúc!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!int.TryParse(txtSoNgayNghi.Text, out int soNgayNghi) || soNgayNghi < 0)
                {
                    MessageBox.Show("Số ngày nghỉ không hợp lệ! Vui lòng nhập số nguyên không âm.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!int.TryParse(txtLuong.Text, out int luongNgay) || luongNgay <= 0)
                {
                    MessageBox.Show("Lương không hợp lệ! Vui lòng nhập số nguyên dương.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Truyền đúng kiểu dữ liệu
                bool isSuccess = _luongBLL.TinhLuong(ngayBD, ngayKT, soNgayNghi, luongNgay, idNV);

                if (isSuccess)
                {
                    MessageBox.Show("Tính lương thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadLuong();
                }
                else
                {
                    MessageBox.Show("Tính lương thất bại! Vui lòng kiểm tra lại dữ liệu.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void dataGridViewL_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex >= 0) // Kiểm tra chỉ số hàng hợp lệ
            {
                DataGridViewRow selectedRow = dataGridViewL.Rows[e.RowIndex];

                dtpNgayBD.Value = selectedRow.Cells[1].Value != DBNull.Value ? Convert.ToDateTime(selectedRow.Cells[1].Value) : DateTime.Now; // Ngày bắt đầu
                dtpNgayKT.Value = selectedRow.Cells[2].Value != DBNull.Value ? Convert.ToDateTime(selectedRow.Cells[2].Value) : DateTime.Now; // Ngày kết thúc

                // Kiểm tra và lấy số ngày nghỉ
                if (selectedRow.Cells[3].Value != null && selectedRow.Cells[3].Value != DBNull.Value)
                {
                    txtSoNgayNghi.Text = selectedRow.Cells[3].Value.ToString(); // Hiển thị số ngày nghỉ
                }
                else
                {
                    txtSoNgayNghi.Text = "0"; // Nếu bị null, hiển thị 0
                }

                cmbLuong.Text = selectedRow.Cells[5].Value?.ToString(); // Tên nhân viên
                txtLuong.Text = selectedRow.Cells[6].Value?.ToString(); // Lương/ngày

                btnTinhL.Enabled = false;
                

            }
        }
        private void btnSuaL_Click(object sender, EventArgs e)
        {
            try
            {
                // Khởi tạo BLL để thao tác với dữ liệu lương
                LuongBLL luongBLL = new LuongBLL();

                // Kiểm tra xem có dòng nào được chọn không
                if (dataGridViewL.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Vui lòng chọn một bản ghi để sửa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Lấy dữ liệu từ DataGridView
                int idLuong = Convert.ToInt32(dataGridViewL.SelectedRows[0].Cells[0].Value);
                DateTime ngayBD = dtpNgayBD.Value;
                DateTime ngayKT = dtpNgayKT.Value;
                int soNgayNghi = int.Parse(txtSoNgayNghi.Text);
                int luongNgay = int.Parse(txtLuong.Text);
                string idNV = cmbLuong.SelectedValue?.ToString();

                // Kiểm tra dữ liệu hợp lệ
                if (ngayBD > ngayKT)
                {
                    MessageBox.Show("Ngày bắt đầu không thể lớn hơn ngày kết thúc!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (soNgayNghi < 0)
                {
                    MessageBox.Show("Số ngày nghỉ không thể nhỏ hơn 0!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (luongNgay <= 0)
                {
                    MessageBox.Show("Lương/ngày phải lớn hơn 0!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (string.IsNullOrEmpty(idNV))
                {
                    MessageBox.Show("Vui lòng chọn nhân viên!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Gọi phương thức BLL để cập nhật thông tinh lương
                bool isUpdated = luongBLL.UpdateLuong(idLuong, ngayBD, ngayKT, soNgayNghi, luongNgay, idNV);

                if (isUpdated)
                {
                    MessageBox.Show("Cập nhật lương thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadLuong(); // Cập nhật lại DataGridView
                }
                else
                {
                    MessageBox.Show("Cập nhật thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnLamMoiL_Click(object sender, EventArgs e)
        {
            // Xóa dữ liệu trong các ô nhập liệu
            dtpNgayBD.Value = DateTime.Now;
            dtpNgayKT.Value = DateTime.Now;
            txtSoNgayNghi.Text = "";
            txtLuong.Text = "";

            // Đặt lại ComboBox nhân viên về trạng thái mặc định (item đầu tiên)
            if (cmbLuong.Items.Count > 0)
                cmbLuong.SelectedIndex = 0;

            // Bật lại các trường nhập liệu và nút Tính
            btnTinhL.Enabled = true;
            btnSuaL.Enabled = false;
            btnXoaL.Enabled = false;

            // Làm mới lại bảng dữ liệu
            LoadLuong();
        }
        private void btnXoaL_Click(object sender, EventArgs e)
        {
            // Kiểm tra nếu chưa chọn dòng nào
            if (dataGridViewL.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn một bản ghi cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Lấy ID lương từ dòng được chọn trong DataGridView
            int idLuong = Convert.ToInt32(dataGridViewL.SelectedRows[0].Cells[0].Value);

            // Xác nhận lại với người dùng trước khi xóa
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa ?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                // Gọi phương thức từ BLL để xóa
                LuongBLL luongBLL = new LuongBLL();
                bool isDeleted = luongBLL.DeleteLuong(idLuong);

                if (isDeleted)
                {
                    MessageBox.Show("Xóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadLuong(); // Cập nhật lại bảng dữ liệu
                }
                else
                {
                    MessageBox.Show("Xóa thất bại. Vui lòng thử lại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void btnTimKiemL_Click(object sender, EventArgs e)
        {
            string keyword = txtTimKiemL.Text.Trim(); // Lấy giá trị nhập vào

            // Kiểm tra nếu ô tìm kiếm rỗng
            if (string.IsNullOrEmpty(keyword))
            {
                MessageBox.Show("Vui lòng nhập từ khóa tìm kiếm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Gọi BLL để tìm kiếm dữ liệu
            LuongBLL luongBLL = new LuongBLL();
            DataTable dt = luongBLL.SearchLuong(keyword);

            // Kiểm tra nếu không có kết quả
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("Không tìm thấy kết quả phù hợp!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            // Hiển thị kết quả lên DataGridView
            dataGridViewL.DataSource = dt;
            dataGridViewL.Columns[0].HeaderText = "Mã lương";      // IDLUONG
            dataGridViewL.Columns[1].HeaderText = "Ngày bắt đầu";  // NGAYBD
            dataGridViewL.Columns[2].HeaderText = "Ngày kết thúc"; // NGAYKT
            dataGridViewL.Columns[3].HeaderText = "Số ngày nghỉ";  // SONGAYNGHI
            dataGridViewL.Columns[4].HeaderText = "Tổng lương";    // TONGLUONG
            dataGridViewL.Columns[5].HeaderText = "Tên nhân viên"; // TENNV
            dataGridViewL.Columns[6].HeaderText = "Lương/ngày";    // LUONG (Đúng cột)

            dataGridViewL.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewL.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 10, FontStyle.Bold);
        }
        private void btnShowAllL_Click(object sender, EventArgs e)
        {
            LoadLuong();
            LayTenNhanVien();
        }
        private void btnXemDSTL_Click(object sender, EventArgs e)
        {
            RPTinhLuong rptl = new RPTinhLuong("XemDSTL");
            rptl.Show();
        }
        #endregion tab TinhLuong       

        #region tab HopDong
        private void LoadHopDong()
        {
            _hopDongBLL = new HopDongBLL();
            List<HopDongDTO> hopDongList = _hopDongBLL.GetHopDongList();
            dataGridViewHD.DataSource = hopDongList;

            // Đặt tên cột hiển thị
            dataGridViewHD.Columns[0].HeaderText = "Mã hợp đồng";
            dataGridViewHD.Columns[1].HeaderText = "Ngày bắt đầu";
            dataGridViewHD.Columns[2].HeaderText = "Ngày kết thúc";
            dataGridViewHD.Columns[3].HeaderText = "Mô tả hợp đồng";
            dataGridViewHD.Columns[4].HeaderText = "Tên nhân viên";
            dataGridViewHD.Columns[5].HeaderText = "Loại hợp đồng";
            dataGridViewHD.Columns[6].HeaderText = "Trạng thái hợp đồng";

            // Căn chỉnh DataGridView
            dataGridViewHD.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewHD.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 10, FontStyle.Bold);
        }
        private void LoadComboBoxNhanVien()
        {
            NhanVienBLL nhanVienBLL = new NhanVienBLL();
            DataTable dtnv = nhanVienBLL.GetDanhSachNhanVien();

            // Thêm dòng "--Chọn nhân viên--"
            DataRow newRow = dtnv.NewRow();
            newRow["IDNV"] = DBNull.Value;
            newRow["TENNV"] = "--Chọn nhân viên--";
            dtnv.Rows.InsertAt(newRow, 0);

            cmbNhanVienHD.DataSource = dtnv;
            cmbNhanVienHD.DisplayMember = "TENNV"; // Hiển thị tên
            cmbNhanVienHD.ValueMember = "IDNV";    // Lưu ID
            cmbNhanVienHD.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbNhanVienHD.SelectedIndex = 0;
        }
        private void LoadComboBoxLoaiHD()
        {
            HopDongBLL hopDongBLL = new HopDongBLL();
            DataTable dtnv = hopDongBLL.GetTenLoaiHDList();

            // Thêm dòng "--Chọn nhân viên--"
            DataRow newRow = dtnv.NewRow();
            newRow["IDLOAIHD"] = DBNull.Value;
            newRow["TENLOAIHD"] = "--Chọn loại--";
            dtnv.Rows.InsertAt(newRow, 0);

            cmbLoaiHD.DataSource = dtnv;
            cmbLoaiHD.DisplayMember = "TENLOAIHD"; // Hiển thị tên
            cmbLoaiHD.ValueMember = "IDLOAIHD";    // Lưu ID
            cmbLoaiHD.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbLoaiHD.SelectedIndex = 0;
        }
        private void LoadComboBoxTrangThaiHD()
        {
            HopDongBLL hopDongBLL = new HopDongBLL();
            DataTable dtnv = hopDongBLL.GetTrangThaiHDList();

            // Thêm dòng "--Chọn nhân viên--"
            DataRow newRow = dtnv.NewRow();
            newRow["IDTRANGTHAI"] = DBNull.Value;
            newRow["TENTRANGTHAI"] = "--Chọn trạng thái--";
            dtnv.Rows.InsertAt(newRow, 0);

            cmbTrangThaiHD.DataSource = dtnv;
            cmbTrangThaiHD.DisplayMember = "TENTRANGTHAI"; // Hiển thị tên
            cmbTrangThaiHD.ValueMember = "IDTRANGTHAI";    // Lưu ID
            cmbTrangThaiHD.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTrangThaiHD.SelectedIndex = 0;
        }
        private void btnThemHD_Click(object sender, EventArgs e)
        {
            try
            {
                string idHopDong = txtMaHD.Text.Trim();
                DateTime ngayBD = dtpNgayBDHD.Value;
                DateTime ngayKT = dtpNgayKTHD.Value;
                string moTa = txtMoTaHD.Text.Trim();
                string idNhanVien = cmbNhanVienHD.SelectedValue?.ToString();
                string idLoaiHD = cmbLoaiHD.SelectedValue?.ToString();
                string idTrangThai = cmbTrangThaiHD.SelectedValue?.ToString();

                if (string.IsNullOrWhiteSpace(idHopDong) || string.IsNullOrWhiteSpace(moTa) ||
                    string.IsNullOrWhiteSpace(idNhanVien) || string.IsNullOrWhiteSpace(idLoaiHD) || string.IsNullOrWhiteSpace(idTrangThai))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                HopDongBLL hopDongBLL = new HopDongBLL();
                hopDongBLL.AddHopDong(idHopDong, ngayBD, ngayKT, moTa, idNhanVien, idLoaiHD, idTrangThai);

                LoadHopDong();

                MessageBox.Show("Thêm hợp đồng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void dataGridViewHD_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Kiểm tra chỉ số hàng hợp lệ
            {
                DataGridViewRow selectedRow = dataGridViewHD.Rows[e.RowIndex];

                // Gán dữ liệu từ DataGridView vào các điều khiển
                txtMaHD.Text = selectedRow.Cells[0].Value?.ToString(); // ID Hợp đồng
                dtpNgayBDHD.Value = selectedRow.Cells[1].Value != DBNull.Value ? Convert.ToDateTime(selectedRow.Cells[1].Value) : DateTime.Now; // Ngày bắt đầu
                dtpNgayKTHD.Value = selectedRow.Cells[2].Value != DBNull.Value ? Convert.ToDateTime(selectedRow.Cells[2].Value) : DateTime.Now; // Ngày kết thúc
                txtMoTaHD.Text = selectedRow.Cells[3].Value?.ToString(); // Mô tả hợp đồng
                cmbNhanVienHD.Text = selectedRow.Cells[4].Value?.ToString(); // Tên nhân viên
                cmbLoaiHD.Text = selectedRow.Cells[5].Value?.ToString(); // Loại hợp đồng
                cmbTrangThaiHD.Text = selectedRow.Cells[6].Value?.ToString(); // Trạng thái hợp đồng

                // Vô hiệu hóa nút thêm để tránh trùng lặp ID khi chỉnh sửa
                btnThemHD.Enabled = false;
                txtMaHD.Enabled = false;
            }   
        }
        private void btnSuaHD_Click(object sender, EventArgs e)
        {
            try
            {
                string idHopDong = txtMaHD.Text;
                DateTime ngayBD = dtpNgayBDHD.Value;
                DateTime ngayKT = dtpNgayKTHD.Value;
                string moTa = txtMoTaHD.Text;
                string idNhanVien = cmbNhanVienHD.SelectedValue?.ToString();
                string idLoaiHD = cmbLoaiHD.SelectedValue?.ToString();
                string idTrangThai = cmbTrangThaiHD.SelectedValue?.ToString();

                bool result = _hopDongBLL.UpdateHopDong(idHopDong, ngayBD, ngayKT, moTa, idNhanVien, idLoaiHD, idTrangThai);
                if (result)
                {
                    MessageBox.Show("Cập nhật hợp đồng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadHopDong(); // Tải lại danh sách hợp đồng
                }
                else
                {
                    MessageBox.Show("Cập nhật hợp đồng thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnLamMoiHD_Click(object sender, EventArgs e)
        {
            // Xóa dữ liệu trong các ô nhập liệu
            txtMaHD.Text = "";
            dtpNgayBDHD.Value = DateTime.Now;
            dtpNgayKTHD.Value = DateTime.Now;
            txtMoTaHD.Text = ""; // Xóa nội dung mô tả hợp đồng
            txtTimKiemHD.Text = "";

            // Đặt lại ComboBox Nhân Viên, Loại Hợp Đồng, Trạng Thái về mặc định (item đầu tiên)
            if (cmbNhanVienHD.Items.Count > 0)
                cmbNhanVienHD.SelectedIndex = 0;

            if (cmbLoaiHD.Items.Count > 0)
                cmbLoaiHD.SelectedIndex = 0;

            if (cmbTrangThaiHD.Items.Count > 0)
                cmbTrangThaiHD.SelectedIndex = 0;

            // Bật lại các nút và ô nhập liệu khi làm mới
            txtMaHD.Enabled = true;
            btnThemHD.Enabled = true;
            

            // Làm mới lại bảng dữ liệu hợp đồng
            LoadHopDong();
        }
        private void btnXoaHD_Click(object sender, EventArgs e)
        {
            if (dataGridViewHD.SelectedRows.Count > 0) // Kiểm tra có dòng nào được chọn không
            {
                DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa hợp đồng này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    string idHopDong = dataGridViewHD.SelectedRows[0].Cells[0].Value.ToString(); // Lấy ID hợp đồng từ dòng được chọn

                    try
                    {
                        if (_hopDongBLL.DeleteHopDong(idHopDong)) // Gọi BLL để xóa
                        {
                            MessageBox.Show("Xóa hợp đồng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadHopDong(); // Làm mới lại bảng dữ liệu sau khi xóa
                        }
                        else
                        {
                            MessageBox.Show("Xóa hợp đồng thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một hợp đồng để xóa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void btnTimKiemHD_Click(object sender, EventArgs e)
        {
            try
            {
                string tuKhoa = txtTimKiemHD.Text.Trim(); // Lấy dữ liệu từ ô nhập

                if (string.IsNullOrWhiteSpace(tuKhoa))
                {
                    MessageBox.Show("Vui lòng nhập từ khóa tìm kiếm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DataTable dt = _hopDongBLL.TimKiemHopDong(tuKhoa);

                if (dt.Rows.Count > 0)
                {
                    dataGridViewHD.DataSource = dt; // Hiển thị kết quả tìm kiếm
                    dataGridViewHD.Columns[0].HeaderText = "Mã hợp đồng";
                    dataGridViewHD.Columns[1].HeaderText = "Ngày bắt đầu";
                    dataGridViewHD.Columns[2].HeaderText = "Ngày kết thúc";
                    dataGridViewHD.Columns[3].HeaderText = "Mô tả hợp đồng";
                    dataGridViewHD.Columns[4].HeaderText = "Tên nhân viên";
                    dataGridViewHD.Columns[5].HeaderText = "Mã nhân viên";
                    dataGridViewHD.Columns[6].HeaderText = "Loại hợp đồng";
                    dataGridViewHD.Columns[7].HeaderText = "Trạng thái hợp đồng";
                }
                else
                {
                    MessageBox.Show("Không tìm thấy hợp đồng nào!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dataGridViewHD.DataSource = null; // Xóa dữ liệu cũ
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnShowAllHD_Click(object sender, EventArgs e)
        {
            LoadHopDong();
            LoadComboBoxNhanVien();
            LoadComboBoxLoaiHD();
            LoadComboBoxTrangThaiHD();
        }
        private void btnXemDSHD_Click(object sender, EventArgs e)
        {
            RPHopDong rphd = new RPHopDong("XemDSHD");
            rphd.Show();
        }

        #endregion tab HopDong

        #region btntab

        private void btntabTK_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPageTK;
        }

        private void btntabNV_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPageNV;
        }

        private void btntabPB_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPagePB;
        }

        private void btntabDA_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPageDA;
        }

        private void btntabTL_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPageTL;
        }

        private void btntabHD_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPageHD;
        }

        private void btnOut_Click(object sender, EventArgs e)
        {
            this.Hide();
            Login login = new Login();
            login.Show();   
        }
        private void ptAD_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPageTK;
        }
        private void ptNV_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPageNV;
        }
        private void ptPB_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPagePB;
        }
        private void ptDA_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPageDA;
        }
        private void ptTL_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPageTL;
        }
        private void ptHD_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPageHD;
        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Login login = new Login();
            login.Show();
        }










        #endregion btntab

        
    }
}




