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
    
    public partial class Category : Form
    {
        SqlConnection con = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnection db = new DBConnection();
        CategoryList flist;
        public Category(CategoryList frm)
        {
            InitializeComponent();
            con = new SqlConnection(db.MyCon());
            flist = frm;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        public void Clear()
        {
            btnSave.Enabled = true;
            btnupdate.Enabled = false;
            txtCategory.Clear();
            txtCategory.Focus();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if(MessageBox.Show("Are you sure you want  to save this category?","Saving Record",MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.Yes)
                {
                    con.Open();
                    cm = new SqlCommand("INSERT INTO tblCategory(category) VALUES(@Category)", con);
                    cm.Parameters.AddWithValue("@category",txtCategory.Text);
                    cm.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Category has been successfully saved.");
                    Clear();
                    flist.LoadCategory();
                }
            }catch(Exception er)
            {
                con.Close();
                MessageBox.Show(er.Message);
            }
        }

        private void txtCategory_TextChanged(object sender, EventArgs e)
        {

        }

        private void Category_Load(object sender, EventArgs e)
        {

        }

        private void btnupdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Are you sure you want to update this brand?", "Update Category", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    con.Open();
                    cm = new SqlCommand("UPDATE tblCategory set category=@category where id like '" + lblID.Text + "'", con);
                    cm.Parameters.AddWithValue("@category", txtCategory.Text);
                    cm.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Brand has been successfully updated.");
                    Clear();
                    flist.LoadCategory();
                    this.Dispose();
                }
            }
            catch (Exception err)
            {
                con.Close();
                MessageBox.Show("Error");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Clear();
        }
    }
}
