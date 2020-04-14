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
    public partial class Stockln : Form
    {
        SqlConnection con = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnection db = new DBConnection();
        SqlDataReader dr;
        string sittle = "simple POS System";
        public Stockln()
        {
            InitializeComponent();
            con = new SqlConnection(db.MyCon());
        }
        
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }
        public void LoadStockIn()
        {
            int i=0;
            dataGridView2.Rows.Clear();
            con.Open();
            cm = new SqlCommand("Select * FROM vwStockin WHERE refno like '"+txtRefNo.Text+ "' and status like 'pending' ", con);
            dr = cm.ExecuteReader();
            while(dr.Read())
            {
                i++;
                dataGridView2.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), dr[6].ToString());
            }
            dr.Close();

            con.Close();
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string ColName = dataGridView2.Columns[e.ColumnIndex].Name;
            if(ColName== "ColDelete")
            {
                if (MessageBox.Show("Are you sure you want to delete this record?", sittle, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    con.Open();
                    cm = new SqlCommand("DELETE FROM tblStockin WHERE id like'" + dataGridView2.Rows[e.RowIndex].Cells[1].Value.ToString() + "'", con);
                    cm.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Item was successfully deleted",sittle,MessageBoxButtons.OK);
                    LoadStockIn();
                }

            }
           
        }
        private void LoadStockInHistory()
        {
            int i = 0;
            dataGridView1.Rows.Clear();
            con.Open();
            cm = new SqlCommand("Select * FROM vwStockin", con);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {   
                i++;
                dataGridView1.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), DateTime.Parse(dr[5].ToString()).ToShortDateString(), dr[6].ToString());
            }
            dr.Close();

            con.Close();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void Stockln_Load(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SearchProduct_Instock frm = new SearchProduct_Instock(this);
            frm.LoadProduct();
            frm.ShowDialog();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
        public void Clear()
        {
            txtBy.Clear();
            txtRefNo.Clear();
            dt1.Value = DateTime.Now;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if(dataGridView2.Rows.Count>0)
                {
                    if(MessageBox.Show("Are you sure you want to update this record?",sittle,MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.Yes)
                    { 
                    for(int i=0;i<dataGridView2.Rows.Count;i++)
                    {
                        //update product qty
                        con.Open();
                        cm = new SqlCommand("update tblProduct set qty=qty +'" + int.Parse(dataGridView2.Rows[i].Cells[5].Value.ToString()) + "' where pcode like '" + dataGridView2.Rows[i].Cells[3].Value.ToString() + "'",con);
                        cm.ExecuteNonQuery();
                        con.Close();

                        con.Open();
                        cm = new SqlCommand("update tblStockin set qty=qty +'" + int.Parse(dataGridView2.Rows[i].Cells[5].Value.ToString()) + "',status='Done' where id like '" + dataGridView2.Rows[i].Cells[i].Value.ToString() + "'", con);
                        cm.ExecuteNonQuery();
                        con.Close();
                    }

                    Clear();
                    LoadStockIn();
                    }
                }
            }catch(Exception er)
            {
                con.Close();
                MessageBox.Show(er.Message,sittle,MessageBoxButtons.OK,MessageBoxIcon.Warning);

            }
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnload_Click(object sender, EventArgs e)
        {
            LoadStockInHistory();
        }
    }
}
