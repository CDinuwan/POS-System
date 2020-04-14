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
    public partial class LookUp : Form
    {
        frmPOS f;
        SqlConnection con = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnection db = new DBConnection();
        SqlDataReader dr;
        string sittle = "Simple POS System";
        public LookUp(frmPOS frm)
        {
            InitializeComponent();
            con = new SqlConnection(db.MyCon());
            f = frm;
            this.KeyPreview = true;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void flowLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }
        public void LoadRecords()
        {
            int i = 0;
            dataGridView1.Rows.Clear();
            con.Open();
            cm = new SqlCommand("Select p.pcode,p.barcode,p.pdsec,b.brand,c.category,p.price,p.qty from tblProduct as p inner join tblBrand as b on b.id=p.brandid inner join tblCategory as c on c.id=p.cid WHERE p.pdsec like '" + txtSearch.Text + "%'", con);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dataGridView1.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), dr[6].ToString());
            }
            dr.Close();
            con.Close();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadRecords();
        }

        private void txtSearch_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dataGridView1.Columns[e.ColumnIndex].Name;
            if(colName=="Select")
            {
                frmQty frm = new frmQty(f);
                frm.productDetails(dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString(), Double.Parse(dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString()), f.lblTransno.Text);
                frm.ShowDialog(); ;
            }
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void LookUp_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Escape)
            {
                this.Dispose();
            }
        }
    }
}
