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
    public partial class frmPOS : Form
    {
        String id;
        String price;
        SqlConnection con = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnection db = new DBConnection();
        SqlDataReader dr;
        string sittle = "Simple POS System";
        frmSecurity f;
        public frmPOS(frmSecurity frm)
        {
            InitializeComponent();
            lblDate.Text = DateTime.Now.ToLongDateString();
            con = new SqlConnection(db.MyCon());
            this.KeyPreview = true;
            f = frm;
        }

        private void frmPOS_Load(object sender, EventArgs e)
        {
             
        }
        public void GetTransNo()
        {
            try
            {
                string date = DateTime.Now.ToString("yyyyMMdd");
                string transno;
                int count;
                con.Open();
                cm = new SqlCommand("select top 1 transno from tblCart where transno like '" + date + "%'order by id desc", con);
                dr = cm.ExecuteReader();
                dr.Read();
                if(dr.HasRows)
                {
                    transno = dr[0].ToString();
                    count = int.Parse(transno.Substring(8, 4));
                    lblTransno.Text = date +(count + 1);
                }
                else
                {
                    transno = date + "1001";
                    lblTransno.Text = transno;
                }
                dr.Close();
                con.Close();
            }catch(Exception er)
            {
                con.Close();
                MessageBox.Show(er.Message,sittle,MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            if(dataGridView1.Rows.Count>0)
            {
                return;
            }
            GetTransNo();
            txtSearch.Enabled = true;
            txtSearch.Focus();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            if(dataGridView1.Rows.Count>0)
            {
                MessageBox.Show("Unable to logout! Please cancel the transaction.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if(MessageBox.Show("Logout Application?","Logout",MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.Yes)
            {
                this.Hide();
                frmSecurity frm = new frmSecurity();
                frm.ShowDialog();
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {

        }

        private void txtSearch_Click(object sender, EventArgs e)
        {
           
        }
        public void GetCartTotal()
        {
            double discount = Double.Parse(lblDiscount.Text);
            double sales = Double.Parse(lblTotal.Text);
            double vat = sales*db.GetVal();
            double vatable = sales-vat;
            lblVat.Text = vat.ToString("#,#00.00");
            lblVatable.Text = vatable.ToString("#,#00.00");
            lblDisplayTot.Text=sales.ToString("#,#00.00");
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtSearch.Text == String.Empty)
                {
                    return;
                }
                else
                {
                    con.Open();
                    cm = new SqlCommand("select * from tblProduct where barcode like'" + txtSearch.Text + "'", con);
                    dr = cm.ExecuteReader();
                    dr.Read();
                    if (dr.HasRows)
                    {
                        frmQty frm = new frmQty(this);
                        frm.productDetails(dr["pcode"].ToString(), double.Parse(dr["price"].ToString()), lblTransno.Text);
                        dr.Close();
                        con.Close();
                        frm.ShowDialog();
                    }
                    dr.Close();
                    con.Close();
                }
            }
            catch (Exception er)
            {
                con.Close();
                MessageBox.Show(er.Message, sittle, MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }

        }
        public void LoadCart()
        {
            try
            {
                Boolean hasrecord = false;
                dataGridView1.Rows.Clear();
                int i = 0;
                double total = 0;
                double discount = 0;
                con.Open();
                cm = new SqlCommand("select c.id,c.pcode,p.pdsec,c.price,c.qty,c.disc,c.total from tblCart as c inner join tblproduct as p on c.pcode=p.pcode where transno like'" + lblTransno.Text + "'and status like'pending'", con);
                dr = cm.ExecuteReader();
                while(dr.Read())
                {
                    i++;
                    total += Double.Parse(dr["total"].ToString());
                    discount += Double.Parse(dr["disc"].ToString());
                    dataGridView1.Rows.Add(i, dr["id"].ToString(),dr["pcode"].ToString(), dr["pdsec"].ToString(), dr["price"].ToString(), dr["qty"].ToString(), dr["disc"].ToString(), Double.Parse(dr["total"].ToString()).ToString("#,##0.00"));
                    hasrecord = true;
                }
                dr.Close();
                con.Close();
                lblTotal.Text = total.ToString("#,##0.00");
                lblDiscount.Text = discount.ToString("#,##0.00");
                GetCartTotal();
                if(hasrecord=true)
                {
                    btnSettle.Enabled = true;
                    btnDiscount.Enabled = true;
                    btnCancel.Enabled = true;
                }
                else
                {
                    btnSettle.Enabled = false;
                    btnDiscount.Enabled = false;
                    btnCancel.Enabled = false;
                }
            }catch(Exception er) 
            { 
                con.Close();
                MessageBox.Show(er.Message, sittle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        public void MyDashBoad()
        {
            frmDashboad frm = new frmDashboad();
            frm.TopLevel = false;
            panel3.Controls.Add(frm);
            frm.BringToFront();
            frm.Show();
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dataGridView1.Columns[e.ColumnIndex].Name;
            if(colName=="Delete")
            {
                if(MessageBox.Show("Remove this item?","Remove Item",MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.Yes)
                {
                    con.Open();
                    cm = new SqlCommand("delete from tblCart where id like '" + dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString() + "'", con);
                    cm.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Item has been successfully removed",sittle,MessageBoxButtons.OK,MessageBoxIcon.Information);
                    LoadCart();
                }
            }
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if(lblTransno.Text=="00000000000000")
            {
                return;
            }
            LookUp frm = new LookUp(this);
            frm.LoadRecords();
            frm.ShowDialog();
        }

        private void btnDiscount_Click(object sender, EventArgs e)
        {
            frmDiscount frm = new frmDiscount(this);
            frm.lblD.Text = id;
            frm.txtPrice.Text = price;
            frm.ShowDialog();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            int i=dataGridView1.CurrentRow.Index;
            id = dataGridView1[1, i].Value.ToString();
            price = dataGridView1[7, i].Value.ToString();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            
        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            
        }

        private void timer1_Tick_2(object sender, EventArgs e)
        {
            lblTime.Text = DateTime.Now.ToString("hh:mm:ss tt");
            lblDate1.Text = DateTime.Now.ToLongDateString();
        }

        private void lblTime_Click(object sender, EventArgs e)
        {

        }

        private void btnSettle_Click(object sender, EventArgs e)
        {
            frmSettle frm = new frmSettle(this);
            frm.txtSale.Text = lblDisplayTot.Text;
            frm.ShowDialog();
        }

        private void btnSales_Click(object sender, EventArgs e)
        {
            frmSolditems frm = new frmSolditems();
            frm.dt1.Enabled = false;
            frm.dt2.Enabled = false;
            frm.suser = lblUser.Text;
            frm.cboCashier.Enabled = false;
            frm.cboCashier.Text = lblUser.Text;
            frm.ShowDialog();        
        }

        private void lblDisplayTot_Click(object sender, EventArgs e)
        {

        }

        private void frmPOS_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.F1)
            {
                btnNew_Click(sender, e);
            }
            else if(e.KeyCode==Keys.F2)
            {
                btnSearch_Click(sender, e);
            }
            else if(e.KeyCode==Keys.F3)
            {
                btnDiscount_Click(sender, e);
            }
            else if(e.KeyCode==Keys.F4)
            {
                btnSettle_Click(sender, e);
            }
            else if(e.KeyCode==Keys.F6)
            {
                btnSales_Click(sender, e);
            }
            else if(e.KeyCode==Keys.F7)
            {
                btnChangepw_Click(sender, e);
            }
            else if(e.KeyCode==Keys.F5)
            {
                btnCancel_Click(sender, e);
            }
            else if(e.KeyCode==Keys.F10)
            {
                btnLogout_Click(sender, e);
            }
        }

        private void btnChangepw_Click(object sender, EventArgs e)
        {
            frmChangePassword frm = new frmChangePassword(this);
            frm.ShowDialog();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Remove all items from cart?","Confirm",MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.Yes)
            {
                con.Open();
                cm = new SqlCommand("delete from tblCart where transno like'" + lblTransno.Text+"'", con);
                cm.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("All items has been successfully remove!", "Remove Item", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadCart();
            }
        }
    }
}
