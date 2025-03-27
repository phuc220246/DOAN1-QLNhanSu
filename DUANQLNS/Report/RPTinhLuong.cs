using DUANQLNS.DAL;
using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace DUANQLNS.Report
{
    public partial class RPTinhLuong : Form
    {
        private string _option;
        public RPTinhLuong(string option)
        {
            InitializeComponent();
            _option = option;
        }

        private void RPTinhLuong_Load(object sender, EventArgs e)
        {
            if (_option == "XemDSTL")
            {
                try
                {
                    reportViewer2.LocalReport.ReportEmbeddedResource = "DUANQLNS.Report.RPTinhLuong.rdlc";
                    string queryTinhLuong = @"USP_GetLuongWithNhanVien;";
                    var reportDatasource1 = new ReportDataSource()
                    {
                        Name = "DataSetTL",
                        Value = DataHelper.Instance.ExecuteQuery(queryTinhLuong)
                    };

                    reportViewer2.LocalReport.DataSources.Add(reportDatasource1);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }

                this.reportViewer2.RefreshReport();
            }
            this.reportViewer2.RefreshReport();
        }
    }
}
