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

namespace DUANQLNS.Report
{
    public partial class RPHopDong : Form
    {
        private string _option;
        public RPHopDong(string option)
        {
            InitializeComponent();
            _option = option;
        }

        private void RPHopDong_Load(object sender, EventArgs e)
        {
            if (_option == "XemDSHD")
            {
                try
                {
                    reportViewer1.LocalReport.ReportEmbeddedResource = "DUANQLNS.Report.RPHopDong.rdlc";
                    string queryHopDong = @"USP_GetHopDongWithDetails;";
                    var reportDatasource1 = new ReportDataSource()
                    {
                        Name = "DataSetHD",
                        Value = DataHelper.Instance.ExecuteQuery(queryHopDong)
                    };

                    reportViewer1.LocalReport.DataSources.Add(reportDatasource1);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }

                this.reportViewer1.RefreshReport();
            }
            this.reportViewer1.RefreshReport();
        }
    }
}
