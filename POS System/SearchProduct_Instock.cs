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
    
    public partial class SearchProduct_Instock : Form
    {
        SqlConnection con = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnection db = new DBConnection();
        SqlDataReader dr;
        string sittle = "Simple POS System";
        Stockln slist;
        public SearchProduct_Instock(Stockln flist)
        {
            InitializeComponent();
            con = new SqlConnection(db.MyCon());
            slist = flist;
        }
        public void LoadProduct()
        {
            int i = 0;
            dataGridView1.Rows.Clear();
            con.Open();
            cm = new SqlCommand("SELECT pcode,pdsec,qty FROM tblProduct WHERE pdsec like '%" + txtSearch.Text + "%'order by pdsec", con);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dataGridView1.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString());
            }
            dr.Close();
            con.Close();
        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
       
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (slist.txtRefNo.Text == String.Empty)
            {
                MessageBox.Show("Please Enter Reference No", sittle, MessageBoxButtons.OK, MessageBoxIcon.Warning);slist.txtRefNo.Focus();
                return;
            }

            if (slist.txtBy.Text == String.Empty)
            {
                MessageBox.Show("Please Enter Stock In By", sittle, MessageBoxButtons.OK, MessageBoxIcon.Warning); slist.txtBy.Focus();
                return;
            }
            string colName = dataGridView1.Columns[e.ColumnIndex].Name;
            if (colName == "Select")
            {
                if (MessageBox.Show("Add this item", sittle, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    con.Open();
                    cm = new SqlCommand("INSERT INTO tblStockin(refno,pcode,sdate,stockinby)VALUES(@refno,@pcode,@sdate,@stockinby)", con);
                    cm.Parameters.AddWithValue("@refno", slist.txtRefNo.Text);
                    cm.Parameters.AddWithValue("@pcode", dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString());
                    cm.Parameters.AddWithValue("@sdate", slist.dt1.Value);
                    cm.Parameters.AddWithValue("@stockinby", slist.txtBy.Text);
                    cm.ExecuteNonQuery();
                    con.Close();

                    MessageBox.Show("Successfully Added", sittle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    slist.LoadStockIn();
                }
            }
        }

        private void txtSearch_Click(object sender, EventArgs e)
        {

        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadProduct();
        }
    }
}
