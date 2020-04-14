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
    public partial class frmChangePassword : Form
    {
        SqlConnection con = new SqlConnection();
        SqlCommand cm;
        DBConnection db = new DBConnection();
        frmPOS f;
        public frmChangePassword(frmPOS frm)
        {
            InitializeComponent();
            con = new SqlConnection(db.MyCon());
            f = frm;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string _oldpass = db.GetPassword(f.lblUser.Text);
                if(_oldpass !=txtOld.Text)
                {
                    MessageBox.Show("Old password did not matched!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if(txtNew.Text!=txtConfirm.Text)
                {
                    MessageBox.Show("Confirm new password did not matched!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    if(MessageBox.Show("Change Password!","Confirm",MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.Yes)
                    {
                        con.Open();
                        cm = new SqlCommand("update tbluser set password=@password where username=@username", con);
                        cm.Parameters.AddWithValue("@password",txtNew.Text);
                        cm.Parameters.AddWithValue("@username", f.lblUser.Text);
                        cm.ExecuteNonQuery();
                        con.Close();
                        MessageBox.Show("Password has been successfully saved", "Save Changes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Dispose();
                    }
                }
            }catch(Exception er)
            {
                con.Close();
                MessageBox.Show(er.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
