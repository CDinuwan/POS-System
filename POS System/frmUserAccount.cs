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
    public partial class frmUserAccount : Form
    {

        SqlConnection con = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnection db = new DBConnection();
        SqlDataReader dr;
        Form1 f;
        public frmUserAccount(Form1 f)
        {
            InitializeComponent();
            con = new SqlConnection(db.MyCon());
            this.f = f;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void frmUserAccount_Resize(object sender, EventArgs e)
        {
            metroTabControl1.Left = (this.Width - metroTabControl1.Width) / 2;
            metroTabControl1.Top = (this.Height - metroTabControl1.Height)/2;

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void frmUserAccount_Load(object sender, EventArgs e)
        {
            
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if(txtPassword.Text!=txtRepass.Text)
                {
                    MessageBox.Show("Password did not match!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                con.Open();
                cm = new SqlCommand("insert into tblUser(username,password,role,name)values(@username,@password,@role,@name)", con);
                cm.Parameters.AddWithValue("@username",txtUsername.Text);
                cm.Parameters.AddWithValue("@password", txtPassword.Text);
                cm.Parameters.AddWithValue("@role", cboRole.Text);
                cm.Parameters.AddWithValue("@name", txtName.Text);
                cm.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("New account has saved");

            }catch(Exception er)
            {
                con.Close();
                MessageBox.Show("Error");
            }
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
        private void Clear()
        {
            txtName.Clear();
            txtPassword.Clear();
            txtRepass.Clear();
            txtUsername.Clear();
            cboRole.Text = "";
            txtUsername.Focus();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if(txtOld1.Text!=f._pass)
                {
                    MessageBox.Show("Old password not matched!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (txtNew1.Text != txtRetype1.Text)
                {
                    MessageBox.Show("New password not matched!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                con.Open();
                cm = new SqlCommand("update tblUser set password=@password where username=@username",con);
                cm.Parameters.AddWithValue("@password", txtNew1.Text);
                cm.Parameters.AddWithValue("@username", txtUser1.Text);
                cm.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Password has been successfully changed!", "Changed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtUser1.Clear();
                txtRetype1.Clear();
                txtNew1.Clear();
                txtOld1.Clear();
            }
            catch(Exception er)
            {
                MessageBox.Show("Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                cm = new SqlCommand("select * from tblUser where username=@username",con);
                cm.Parameters.AddWithValue("@username",txtUser2.Text);
                dr = cm.ExecuteReader();
                dr.Read();
                if(dr.HasRows)
                {
                    checkBox1.Checked = bool.Parse(dr["Isactive"].ToString());
                }
                else
                {
                    checkBox1.Checked = false;
                }
                dr.Close();
                con.Close();
            }catch(Exception er)
            {
                con.Close();
                MessageBox.Show("Error", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                Boolean found = true;
                con.Open();
                cm = new SqlCommand("select * from tblUser where username=@username", con);
                cm.Parameters.AddWithValue("@username", txtUser2.Text);
                dr = cm.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    found = true;
                }
                else
                {
                    found = false;
                }
                dr.Close();
                con.Close();

                if(found==true)
                {
                    con.Open();
                    cm = new SqlCommand("update tblUser set isactive=@isactive where username=@username", con);
                    cm.Parameters.AddWithValue("@isactive", checkBox1.Checked.ToString());
                    cm.Parameters.AddWithValue("@username", txtUser2.Text);
                    cm.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Account status has been successfully updated!", "Update Status", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtUser2.Clear();
                    checkBox1.Checked = false;
                }
                else
                {
                    MessageBox.Show("Account not exist!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                
            }catch(Exception er)
            {
                con.Close();
                MessageBox.Show("Error", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
