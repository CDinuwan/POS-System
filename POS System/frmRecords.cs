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
    
    public partial class frmRecords : Form
    {
        SqlConnection con = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnection db = new DBConnection();
        SqlDataReader dr;
        public frmRecords()
        {
            InitializeComponent();
            con = new SqlConnection(db.MyCon());
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if(cboSelect.Text==String.Empty)
            {
                MessageBox.Show("Please select from the dropdown list", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            LoadRecord();
            LoadChartTopSelling();
        }
        public void LoadRecord()
        {
            dataGridView1.Rows.Clear();
            int i = 0;
            con.Open();
            if(cboSelect.Text=="SORT BY QTY")
            {
                cm = new SqlCommand("select top 20 pcode,pdsec,isnull(sum(qty),0) as qty,isnull(sum(total),0) as total from vwSoldItems where sdate between '" + dateTimePicker1.Value.ToString() + "' and '" + dateTimePicker2.Value.ToString() + "'and status like 'Sold' group by pcode,pdsec order by qty desc", con);
            }
            else if(cboSelect.Text=="SORT BY TOTAL")
            {
                cm = new SqlCommand("select top 20 pcode,pdsec,isnull(sum(qty),0) as qty,isnull(sum(total),0) as total from vwSoldItems where sdate between '" + dateTimePicker1.Value.ToString() + "' and '" + dateTimePicker2.Value.ToString() + "'and status like 'Sold' group by pcode,pdsec order by total desc", con);
            }
            
            dr = cm.ExecuteReader();
            while(dr.Read())
            {
                i++;
                dataGridView1.Rows.Add(i, dr["pcode"].ToString(), dr["pdsec"].ToString(), dr["qty"].ToString(),Double.Parse(dr["total"].ToString()).ToString("#,##0.00"));
            }
            dr.Close();
            con.Close();
        }

        public void CancelledOrders()
        {
            int i = 0;
            dataGridView4.Rows.Clear();
            con.Open();
            cm = new SqlCommand("select * from vwCancelledOrder where sdate between'"+dateTimePicker6.Value.ToString()+"' and '"+dateTimePicker5.Value.ToString()+"'",con);
            dr = cm.ExecuteReader();
            while(dr.Read())
            {
                i++;
                dataGridView4.Rows.Add(i,dr["transno"].ToString(), dr["pcode"].ToString(), dr["pdsec"].ToString(), dr["price"].ToString(), dr["qty"].ToString(), dr["total"].ToString(), dr["sdate"].ToString(), dr["voidby"].ToString(), dr["reason"].ToString(), dr["action"].ToString());
            }
            dr.Close();
            con.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                {
                    dataGridView2.Rows.Clear();
                    int i = 0;
                    con.Open();
                    cm = new SqlCommand("select c.pcode,c.paymentby,p.pdsec,sum(c.qty) as tot_qty,sum(c.disc) as tot_disc,sum(c.total) as total from tblCart as c inner join tblProduct as p on c.pcode = p.pcode where status like 'Sold'and sdate between'"+dateTimePicker4.Value.ToString()+"'and '"+dateTimePicker3.Value.ToString()+"'group by c.pcode, p.pdsec, p.price,c.paymentby ", con);
                    dr = cm.ExecuteReader();
                    while (dr.Read())
                    {
                        i++;
                        dataGridView2.Rows.Add(i, dr["pcode"].ToString(), dr["pdsec"].ToString(),dr["paymentby"].ToString(),dr["tot_qty"].ToString(),dr["tot_disc"].ToString(),dr["total"].ToString()).ToString("#,##0.00"); 
                    }
                    dr.Close();
                    con.Close();

                    String x;
                    con.Open();
                    cm = new SqlCommand("select isnull(sum(total),0) from tblCart where status like 'Sold' and sdate between '" + dateTimePicker4.Value.ToString() + "' and '" + dateTimePicker3.Value.ToString() + "'", con);
                    label4.Text = Double.Parse(cm.ExecuteScalar().ToString()).ToString("#,##0.00");
                    con.Close();
                }
            }
            catch(Exception er)
            {
                MessageBox.Show("Error","Warning",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }
        public void LoadInventory()
        {
            int i = 0;
            dataGridView3.Rows.Clear();
            con.Open();
            cm = new SqlCommand("select p.pcode,p.barcode,p.pdsec,b.brand,c.category,p.price,p.qty from tblProduct as p inner join tblBrand as b on p.brandid=b.id inner join tblCategory as c on p.cid=c.id", con);
            dr = cm.ExecuteReader();
            while(dr.Read())
            {
                i++;
                dataGridView3.Rows.Add(i,dr["pcode"].ToString(), dr["barcode"].ToString(), dr["pdsec"].ToString(), dr["brand"].ToString(), dr["category"].ToString(), dr["price"].ToString(), dr["qty"].ToString());
            }
            dr.Close();
            con.Close();
        }
        public void LoadChartTopSelling()
        {
            SqlDataAdapter da=new SqlDataAdapter();
            con.Open();
            if (cboSelect.Text == "SORT BY QTY")
            {
                da = new SqlDataAdapter("select top 20 pcode,isnull(sum(qty),0) as qty,isnull(sum(total),0) as total from vwSoldItems where sdate between '" + dateTimePicker1.Value.ToString() + "' and '" + dateTimePicker2.Value.ToString() + "'and status like 'Sold' group by pcode order by qty desc", con);
            }
            else if (cboSelect.Text == "SORT BY TOTAL")
            {
                da= new SqlDataAdapter("select top 20 pcode,isnull(sum(qty),0) as qty,isnull(sum(total),0) as total from vwSoldItems where sdate between '" + dateTimePicker1.Value.ToString() + "' and '" + dateTimePicker2.Value.ToString() + "'and status like 'Sold' group by pcode order by total desc", con);
            }
            DataSet ds = new DataSet();
            da.Fill(ds, "TOPSELLING");
            chart1.DataSource = ds.Tables["TOPSELLING"];
            Series series = chart1.Series[0];
            series.ChartType = SeriesChartType.Doughnut;

            series.Name = "TOP SELLING";
            var chart = chart1;
            chart.Series[0].XValueMember = "pcode";
            if (cboSelect.Text == "SORT BY QTY") { chart.Series[0].YValueMembers = "qty"; }
            if (cboSelect.Text == "SORT BY TOTAL") { chart.Series[0].YValueMembers = "total"; }
            chart.Series[0].IsValueShownAsLabel = true;
            if (cboSelect.Text == "SORT BY QTY") { chart1.Series[0].LabelFormat = "{#,##0.00}"; }
            if (cboSelect.Text == "SORT BY QTY") { chart1.Series[0].LabelFormat = "{#,##0}"; }
            con.Close();
        }

        private void dataGridView3_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
             
        }

        private void dataGridView4_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            CancelledOrders();
        }
        public void LoadStockInHistory()
        {
            int i = 0;
            dataGridView5.Rows.Clear();
            con.Open();
            cm = new SqlCommand("Select * FROM vwStockin where cast(sdate as date) between '"+dateTimePicker8.Value.ToString()+"'and'"+dateTimePicker7.Value.ToString()+"'", con);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dataGridView5.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), DateTime.Parse(dr[5].ToString()).ToShortDateString(), dr[6].ToString());
            }
            dr.Close();
            con.Close();
        }

        private void dataGridView5_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            LoadStockInHistory();
        }

        private void label11_Click(object sender, EventArgs e)
        {
            frmInventory frm = new frmInventory();
            frm.LoadReport();
            frm.ShowDialog();
        }

        private void label10_Click(object sender, EventArgs e)
        {
            frmInventory frm = new frmInventory();
            frm.LoadTopSelling("select pcode,pdsec,sum(qty) as qty from vwSoldItems where sdate between '" + dateTimePicker1.Value.ToString() + "' and '" + dateTimePicker2.Value.ToString() + "'and status like 'Sold' group by pcode,pdsec order by qty desc");
            frm.ShowDialog();
        }

        private void label9_Click(object sender, EventArgs e)
        {
            frmInventory frm = new frmInventory();
            frm.LoadSoldItems("select c.pcode, p.pdsec, sum(c.qty) as tot_qty, sum(c.disc) as tot_disc, sum(c.total) as total from tblCart as c inner join tblProduct as p on c.pcode = p.pcode where status like 'Sold'and sdate between'"+dateTimePicker4.Value.ToString()+"'and '"+dateTimePicker3.Value.ToString()+"'group by c.pcode, p.pdsec, p.price ");
            frm.ShowDialog();
        }

        private void cboSelect_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {
            frmInventory frm = new frmInventory();
            frm.LoadStockInReport("Select * FROM vwStockin where cast(sdate as date) between '" + dateTimePicker8.Value.ToString() + "'and'" + dateTimePicker7.Value.ToString() + "'");
            frm.ShowDialog();
        }

        private void label8_Click(object sender, EventArgs e)
        {
            frmInventory frm = new frmInventory();
            frm.LoadCancelledOrder("select * from vwCancelledOrder where sdate between'" + dateTimePicker6.Value.ToString() + "' and '" + dateTimePicker5.Value.ToString() + "'");
            frm.ShowDialog();
        }

    }
}
