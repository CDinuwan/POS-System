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
    public partial class Brandlist : Form
    {
        SqlConnection con = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnection db = new DBConnection();
        SqlDataReader dr;
        public Brandlist()
        {
            InitializeComponent();
            con = new SqlConnection(db.MyCon());
            LoadRecords();
        }
        public void LoadRecords()
        {
            int c = 0;
            dataGridView1.Rows.Clear();
            con.Open();
            cm = new SqlCommand("SELECT * FROM tblBrand order by brand", con);
            dr = cm.ExecuteReader();
            while(dr.Read())
            {
                c += 1;
                dataGridView1.Rows.Add(c,dr["id"].ToString(),dr["brand"].ToString());
            }
            dr.Close();
            con.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick_2(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dataGridView1.Columns[e.ColumnIndex].Name;
            if (colName == "Edit")
            {
                Brand frm = new Brand(this);
                frm.lblID.Text = dataGridView1[1, e.RowIndex].Value.ToString();
                frm.txtBrand.Text = dataGridView1[2, e.RowIndex].Value.ToString();
                frm.btnSave.Enabled = false;
                frm.btnupdate.Enabled = true;
                frm.ShowDialog();

            }
            else if(colName == "ColDelete")
            {
                if (MessageBox.Show("Are you sure you want to delete this item?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    con.Open();
                    cm = new SqlCommand("DELETE FROM tblBrand where id like'" + dataGridView1[1, e.RowIndex].Value.ToString() + "'", con);
                    cm.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Brand has been successfully deleted", "POS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadRecords();
                }
            }
            else
            {

            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Brand brn = new Brand(this);
            brn.ShowDialog();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
