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
    public partial class Form1 : Form
    {
        SqlConnection con = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnection db = new DBConnection();
        public string _pass,_user;
        
        public Form1()
        {
            InitializeComponent();
            con = new SqlConnection(db.MyCon());
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
        public void MyDashBoad()
        {
            frmDashboad frm = new frmDashboad();
            frm.TopLevel = false;
            panel3.Controls.Add(frm);
            frm.lblDailySales.Text = db.dailySale().ToString("#,##0.00");
            frm.lblProduct.Text = db.ProductLine().ToString("#,##0");
            frm.lblStockOnHand.Text = db.StockOnHand().ToString("#,##0");
            frm.BringToFront();
            frm.Show();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            MyDashBoad();
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void btnBrand_Click(object sender, EventArgs e)
        {
            
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            
        }

        private void btnProduct_Click(object sender, EventArgs e)
        { 
            
        }

        private void btnStockln_Click(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }


        private void btnProduct_Click_1(object sender, EventArgs e)
        { 
            productlist frm = new productlist();
            frm.TopLevel = false;
            panel3.Controls.Add(frm);
            frm.BringToFront();
            frm.Show();
        }

        private void btnBrand_Click_1(object sender, EventArgs e)
        {
            Brandlist frm = new Brandlist();
            frm.TopLevel = false;
            panel3.Controls.Add(frm);
            frm.BringToFront();
            frm.Show();
        }

        private void btnCategory_Click(object sender, EventArgs e)
        {
            CategoryList frm = new CategoryList();
            frm.TopLevel = false;
            panel3.Controls.Add(frm);
            frm.BringToFront();
            frm.Show();
        }

        private void btnStockln_Click_1(object sender, EventArgs e)
        {
            Stockln frm = new Stockln();
            frm.LoadStockIn();
            frm.ShowDialog();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Logout Application?", "Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Hide();
                frmSecurity frm = new frmSecurity();
                frm.ShowDialog();
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            frmUserAccount frm = new frmUserAccount(this);
            frm.txtUser1.Text = _user;
            frm.ShowDialog();
        }

        private void btnSalesHistory_Click(object sender, EventArgs e)
        {
            frmSolditems frm = new frmSolditems();
            frm.ShowDialog();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            frmRecords frm = new frmRecords();
            frm.TopLevel = false;
            frm.LoadInventory();
            frm.CancelledOrders();
            frm.LoadStockInHistory();
            panel3.Controls.Add(frm);
            frm.BringToFront();
            frm.Show();
        }

        private void button2_Click_2(object sender, EventArgs e)
        {
            frmStore frm = new frmStore();
            frm.LoadRecords();
            frm.ShowDialog();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            MyDashBoad(); 
        }
    }
}
  