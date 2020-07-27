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
    public partial class Brand : Form
    {
        SqlConnection con = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnection db = new DBConnection();
        Brandlist frmlist;
        public Brand(Brandlist flist)
        {
            InitializeComponent();
            con = new SqlConnection(db.MyCon());
            frmlist = flist;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnupdate_Click(object sender, EventArgs e)
        {
            try
            {
                if(MessageBox.Show("Are you sure you want to update this brand?","Update Record",MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.Yes)
                {
                    con.Open();
                    cm = new SqlCommand("UPDATE tblBrand set brand=@brand where id like '" + lblID.Text + "'", con);
                    cm.Parameters.AddWithValue("@brand", txtBrand.Text);
                    cm.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Brand has been successfully updated.");
                    Clear();
                    frmlist.LoadRecords();
                    this.Dispose();
                }
            }catch(Exception err)
            {
                con.Close();
                MessageBox.Show("Error");
            }
        }
        private void Clear()
        {
            btnSave.Enabled = true;
            btnupdate.Enabled = false;
            txtBrand.Clear();
            txtBrand.Focus();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Are you sure you want to save this brand?","",MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.Yes)
                { 
                    con.Open();
                    cm = new SqlCommand("INSERT INTO tblBrand(brand)VALUES(@brand)", con);
                    cm.Parameters.AddWithValue("@Brand", txtBrand.Text);
                    cm.ExecuteNonQuery();

                    con.Close();
                    MessageBox.Show("Record has been succesfully saved.");
                    Clear();
                    frmlist.LoadRecords();
                }
            }catch(Exception ex)
            {
                MessageBox.Show("Error");
            }
        }

        private void Brand_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
