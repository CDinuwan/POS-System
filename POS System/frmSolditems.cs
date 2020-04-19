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

namespace POS_System
{
    public partial class frmSolditems : Form
    {
        SqlConnection con = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnection db = new DBConnection();
        SqlDataReader dr;
        public string suser;
        public frmSolditems()
        {
            InitializeComponent();
            con = new SqlConnection(db.MyCon());
            dt1.Value = DateTime.Now;
            dt2.Value = DateTime.Now;
            LoadRecord();
            LoadCashier();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        public void LoadRecord()
        {
            double total = 0;
            int i = 0;
            dataGridView1.Rows.Clear();
            con.Open();
            if (cboCashier.Text == "All Cashier")
            {
                cm = new SqlCommand("select c.id,c.transno,c.pcode,p.pdsec,c.price,c.qty,c.disc,c.total,c.paymentby from tblCart as c inner join tblProduct as p on c.pcode=p.pcode where status like 'Sold' and sdate between '" + dt1.Value + "' and '" + dt2.Value + "'", con);

            }
            else
            {
                cm = new SqlCommand("select c.id,c.transno,c.pcode,p.pdsec,c.price,c.qty,c.disc,c.total,c.paymentby from tblCart as c inner join tblProduct as p on c.pcode=p.pcode where status like 'Sold' and sdate between '" + dt1.Value + "' and '" + dt2.Value + "'and cashier like'" + cboCashier.Text + "'", con);
            }
            dr = cm.ExecuteReader();
            while(dr.Read())
            {
                total += double.Parse(dr["total"].ToString());
                i += 1;
                dataGridView1.Rows.Add(i, dr["id"].ToString(), dr["transno"].ToString(), dr["pcode"].ToString(), dr["pdsec"].ToString(), dr["price"].ToString(), dr["qty"].ToString(), dr["disc"].ToString(), dr["total"].ToString(),dr["paymentby"].ToString());
            }
            dr.Close();
            con.Close();
            txtTotal.Text = total.ToString("#,##0.00");
        }
        private void frmSolditems_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dataGridView1.Columns[e.ColumnIndex].Name;
            if(colName=="Cancel")
            {
                frmCancelDetail f = new frmCancelDetail(this);
                f.txtID.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                f.txtTransNo.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                f.txtPCode.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
                f.txtDescription.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
                f.txtPrice.Text = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
                f.txtQty.Text = dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString();
                f.txtDiscount.Text = dataGridView1.Rows[e.RowIndex].Cells[7].Value.ToString();
                f.txtTotal.Text = dataGridView1.Rows[e.RowIndex].Cells[8].Value.ToString();
                f.txtCancel.Text = suser;
                f.ShowDialog();
            }
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dt1_ValueChanged(object sender, EventArgs e)
        {
            LoadRecord();
        }

        private void dt2_ValueChanged(object sender, EventArgs e)
        {
            LoadRecord();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            frmReportSold frm = new frmReportSold(this);
            frm.LoadReport();
            frm.ShowDialog();
        }

        private void cboCashier_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        public void LoadCashier()
        {
            cboCashier.Items.Clear();
            cboCashier.Items.Add("All Cashier");
            con.Open();
            cm = new SqlCommand("select * from tblUser where role like 'Cashier'", con);
            dr=cm.ExecuteReader();
            while(dr.Read())
            {
                cboCashier.Items.Add(dr["username"].ToString());
            }
            dr.Close();
            con.Close();
        }

        private void cboCashier_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadRecord();
        }
    }
}
