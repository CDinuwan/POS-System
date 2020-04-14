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
    public partial class CategoryList : Form
    {
        SqlConnection con = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnection db = new DBConnection();
        SqlDataReader dr;
        public CategoryList()
        {
            InitializeComponent();
            con = new SqlConnection(db.MyCon());
            LoadCategory();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        public void LoadCategory()
        {
            int i=0;
            dataGridView1.Rows.Clear();
            con.Open();
            cm = new SqlCommand("SELECT * FROM tblCategory order by category", con);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dataGridView1.Rows.Add(i,dr[0].ToString(),dr[1].ToString());
            }
            dr.Close();
            con.Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Category frm = new Category(this);
            frm.btnSave.Enabled = true;
            frm.btnupdate.Enabled = false;
            frm.ShowDialog();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dataGridView1.Columns[e.ColumnIndex].Name;
            if(colName=="Edit")
            {
                Category frm = new Category(this);
                frm.txtCategory.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                frm.lblID.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                frm.btnSave.Enabled = false;
                frm.btnupdate.Enabled = true;
                frm.ShowDialog();

            }else if(colName=="ColDelete")
            {
                if(MessageBox.Show("Are you sure you want to delete this category","Delete Category", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    con.Open();
                    cm = new SqlCommand("DELETE FROM tblCategory where id like'" + dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString()+"'",con);
                    cm.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Brand has been successfully deleted.");
                    LoadCategory();
                }
            }
        }
    } 
}
