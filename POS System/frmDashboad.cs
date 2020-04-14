using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Windows.Forms.DataVisualization.Charting;

namespace POS_System
{
    public partial class frmDashboad : Form
    {
        SqlConnection con;
        SqlCommand cm;
        DBConnection db = new DBConnection();
        public frmDashboad()
        {
            InitializeComponent();
            con = new SqlConnection();
            con.ConnectionString = db.MyCon();
            LoadChart();
        }

        private void frmDashboad_Load(object sender, EventArgs e)
        {

        }

        private void frmDashboad_Resize(object sender, EventArgs e)
        {
            panel1.Left = (this.Width = panel1.Width) / 2;
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }
        public void LoadChart()
        {
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter("select Year(sdate) as year,isnull(sum(total),0.0) as total from tblCart where status like 'Sold' group by Year(sdate)",con);
            DataSet ds = new DataSet();

            da.Fill(ds, "Sales");
            chart1.DataSource = ds.Tables["Sales"];
            Series series1 = chart1.Series["Series1"];
            series1.ChartType = SeriesChartType.Doughnut;

            series1.Name = "Sales";

            var chart = chart1;
            chart.Series[series1.Name].XValueMember = "year";
            chart.Series[series1.Name].YValueMembers = "total";
            chart.Series[0].IsValueShownAsLabel = true;
            //chart.Series[0].LegendText

            con.Close();
        }
    }
}
