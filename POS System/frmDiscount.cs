using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace POS_System
{
    public partial class frmDiscount : Form
    {
        SqlConnection con = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnection db = new DBConnection();
        SqlDataReader dr;
        string sittle = "Simple POS System";
        frmPOS f;
        public frmDiscount(frmPOS frm)
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

        private void frmDiscount_Load(object sender, EventArgs e)
        {

        }

        private void txtPrecent_TextChanged(object sender, EventArgs e)
        {
            try
            {
                double discount = Double.Parse(txtPrice.Text) * Double.Parse(txtPrecent.Text);
                txtAmount.Text = discount.ToString("#,##0.00");
            }catch(Exception er)
            {
                txtAmount.Text = "0.00";
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if(MessageBox.Show("Add discount? Click yes to confirm",sittle,MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.Yes)
                {
                    con.Open();
                    cm = new SqlCommand("update tblCart set disc=@disc , disc_precent=@disc_precent where id=@id", con);
                    cm.Parameters.AddWithValue("@disc", Double.Parse(txtAmount.Text));
                    cm.Parameters.AddWithValue("@disc_precent", Double.Parse(txtPrecent.Text));
                    cm.Parameters.AddWithValue("@id", int.Parse(lblD.Text));
                    cm.ExecuteNonQuery();
                    con.Close();
                    f.LoadCart();
                    this.Dispose();
                }
            }catch(Exception er)
            {
                con.Close();
                MessageBox.Show(er.Message);
            }
        }

        private void frmDiscount_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Escape)
            {
                this.Dispose();
            }
        }
    }
}
