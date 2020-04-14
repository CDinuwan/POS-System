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
    public partial class Product : Form
    {
        SqlConnection con = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnection db = new DBConnection();
        SqlDataReader dr;
        productlist flist;
        public Product(productlist frmm)
        {
            InitializeComponent();
            con = new SqlConnection(db.MyCon());
            flist = frmm;
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        public void LoadCategory()
        {
            cboCategory.Items.Clear();
            con.Open();
            cm = new SqlCommand("SELECT category FROM tblCategory", con);
            dr = cm.ExecuteReader();
            dr.Read();

            while (dr.Read())
            {
                cboCategory.Items.Add(dr[0].ToString());
            }
            dr.Close();
            con.Close();

        }
        public void LoadBrand()
        {
            cboBrand.Items.Clear();
            con.Open();
            cm = new SqlCommand("SELECT brand FROM tblBrand", con);
            dr = cm.ExecuteReader();

            while (dr.Read())
            {
                cboBrand.Items.Add(dr[0].ToString());
            }
            dr.Close();
            con.Close();

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Product_Load(object sender, EventArgs e)
        {
            
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string brandid = "";
                string cid="";
                con.Open();
                cm = new SqlCommand("SELECT id FROM tblBrand WHERE brand like '" + cboBrand.Text + "'", con);
                dr = cm.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    brandid = dr[0].ToString();
                }
                dr.Close();
                con.Close();

                con.Open();
                cm = new SqlCommand("SELECT id FROM tblCategory WHERE category like '" + cboCategory.Text + "'", con);
                dr = cm.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    cid = dr[0].ToString();
                }
                dr.Close();
                con.Close();

                if (MessageBox.Show("Are you sure you want to save this product?","Save Product",MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.Yes)
                {
                    con.Open();
                    cm = new SqlCommand("INSERT INTO tblProduct(pcode,barcode,pdsec,brandid,cid,price) VALUES(@pcode,@barcode,@pdsec,@brandid,@cid,@price)", con);
                    cm.Parameters.AddWithValue("@pcode",txtPcode.Text);
                    cm.Parameters.AddWithValue("@pdsec", txtPdesc.Text);
                    cm.Parameters.AddWithValue("@barcode", txtBarcode.Text);
                    cm.Parameters.AddWithValue("@brandid", brandid);
                    cm.Parameters.AddWithValue("@cid", cid);
                    cm.Parameters.AddWithValue("@price", txtPrice.Text);
                    cm.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Product has been successfully saved.");
                    Clear(); 
                    flist.LoadRecords();
                }
            }catch(Exception er)
            {
                con.Close();
                MessageBox.Show(er.Message);
            }
        }
        public void Clear()
        {
            txtPrice.Clear();
            txtPdesc.Clear();
            txtBarcode.Clear();
            txtPcode.Clear();
            cboBrand.Text = "";
            cboCategory.Text = "";
            txtPcode.Focus();
            btnSave.Enabled = true;
            btnupdate.Enabled = false;
        }

        private void btnupdate_Click(object sender, EventArgs e)
        {
            try
            {
                string brandid = "";
                string cid = "";
                con.Open();
                cm = new SqlCommand("SELECT id FROM tblBrand WHERE brand like '" + cboBrand.Text + "'", con);
                dr = cm.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    brandid = dr[0].ToString();
                }
                dr.Close();
                con.Close();

                con.Open();
                cm = new SqlCommand("SELECT id FROM tblCategory WHERE category like '" + cboCategory.Text + "'", con);
                dr = cm.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    cid = dr[0].ToString();
                }
                dr.Close();
                con.Close();

                if (MessageBox.Show("Are you sure you want to update this product?", "Save Product", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    con.Open();
                    cm = new SqlCommand("UPDATE tblProduct SET pdsec=@pdsec,barcode=@barcode,brandid=@brandid,cid=@cid,price=@price WHERE pcode like @pcode", con);
                    cm.Parameters.AddWithValue("@pcode", txtPcode.Text);
                    cm.Parameters.AddWithValue("@barcode", txtBarcode.Text);
                    cm.Parameters.AddWithValue("@pdsec", txtPdesc.Text);
                    cm.Parameters.AddWithValue("@brandid", brandid);
                    cm.Parameters.AddWithValue("@cid", cid);
                    cm.Parameters.AddWithValue("@price", txtPrice.Text);
                    cm.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Product has been successfully updated.");
                    Clear();
                    flist.LoadRecords();
                    this.Dispose();
                }
            }
            catch (Exception er)
            {
                con.Close();
                MessageBox.Show(er.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void txtPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar)&& e.KeyChar!='.')
            {
                e.Handled = true;
            }
        }

        private void txtPrice_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
