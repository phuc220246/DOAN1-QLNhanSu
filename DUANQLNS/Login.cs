using DUANQLNS.BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DUANQLNS.BLL;


namespace DUANQLNS
{
    public partial class Login : Form
    {
        private TaiKhoanBLL _taiKhoanBLL;
        public Login()
        {
            InitializeComponent(); // Tạo và sắp xếp các điều khiển trên form theo thiết kế của VS
            this.StartPosition = FormStartPosition.CenterScreen; // Thiết lập cửa sổ đăng nhập xuất hiện ở trung tâm màn hình.
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {

            string username = txtUsername.Text; // lấy dữ liệu từ ng dùng nhập 
            string password = txtPassword.Text;

            try
            {
                TaiKhoanBLL taiKhoanBLL = new TaiKhoanBLL();
                bool isLoggedIn = taiKhoanBLL.DangNhap(username, password); //gọi DangNhap từ DAL 

                if (isLoggedIn)
                {
                    MessageBox.Show("Đăng nhập thành công!");
                    Main fm = new Main();
                    fm.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Sai mật khẩu hoặc tài khoản!");
                }
            }
            catch (ArgumentException ex)
            {
                //báo lỗi
                MessageBox.Show(ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked ) 
            {
                txtPassword.UseSystemPasswordChar = false; // hiện
            }
            else
            {
                txtPassword.UseSystemPasswordChar = true; // ẩn 
            }
        }
    }
}
