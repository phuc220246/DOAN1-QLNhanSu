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
    public partial class RPNhanVien : Form
    {
        private string _option;
        public RPNhanVien(string option)
        {
            InitializeComponent();
            _option = option;
        }

        private void RPNhanVien_Load(object sender, EventArgs e)
        {
            if(_option == "XemDSNV")
            {
                try
                {
                    reportViewer1.LocalReport.ReportEmbeddedResource = "DUANQLNS.Report.RPNhanVien.rdlc";
                    string queryNhanVien = @"USP_GetNhanVienWithPhongBanAndDuAn;";
                    var reportDatasource1 = new ReportDataSource()
                    {
                        Name = "DataSetNV",
                        Value = DataHelper.Instance.ExecuteQuery(queryNhanVien)
                    };
                    
                    reportViewer1.LocalReport.DataSources.Add(reportDatasource1);
                    
                }
                catch( Exception ex) 
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
                this.reportViewer1.RefreshReport();
            }
            this.reportViewer1.RefreshReport();
        }
    }
}
