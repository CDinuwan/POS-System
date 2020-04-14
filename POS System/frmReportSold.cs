using Microsoft.Reporting.WinForms;
using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace POS_System
{
    public partial class frmReportSold : Form
    {
        SqlConnection con = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnection db = new DBConnection();
        SqlDataReader dr;
        frmSolditems f;
        public frmReportSold(frmSolditems frm)
        {
            InitializeComponent();
            con = new SqlConnection(db.MyCon());
            f = frm;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void frmReportSold_Load(object sender, EventArgs e)
        {


        }

        private void reportViewer1_Load(object sender, EventArgs e)
        {

        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
       

        public void LoadReport()
        {
            try
            {
                ReportDataSource rptDS;

                this.reportViewer1.LocalReport.ReportPath = Application.StartupPath + @"\Reports\Report2.rdlc";
                this.reportViewer1.LocalReport.DataSources.Clear();

                DataSet1 ds = new DataSet1();
                SqlDataAdapter da = new SqlDataAdapter();

                con.Open();
                if (f.cboCashier.Text == "All Cashier")
                {
                    da.SelectCommand = new SqlCommand("select c.id,c.transno,c.pcode,p.pdsec,c.price,c.qty,c.disc as discount,c.total from tblCart as c inner join tblProduct as p on c.pcode=p.pcode where status like 'Sold' and sdate between '" + f.dt1.Value + "' and '" + f.dt2.Value + "'", con);
                }
                else
                {
                    da.SelectCommand = new SqlCommand("select c.id,c.transno,c.pcode,p.pdsec,c.price,c.qty,c.disc as discount,c.total from tblCart as c inner join tblProduct as p on c.pcode=p.pcode where status like 'Sold' and sdate between '" + f.dt1.Value + "' and '" + f.dt2.Value + "'and cashier like '" + f.cboCashier.Text + "'", con);
                }
                da.Fill(ds.Tables["dtSoldReport"]);
                con.Close();

                rptDS = new ReportDataSource("DataSet1", ds.Tables["dtSoldReport"]);
                reportViewer1.LocalReport.DataSources.Add(rptDS);
                reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);
                reportViewer1.ZoomMode = ZoomMode.Percent;
                reportViewer1.ZoomPercent = 100;


            }
            catch (Exception er)
            {
                con.Close();
                MessageBox.Show(er.Message);
            }
        }
    }
}
