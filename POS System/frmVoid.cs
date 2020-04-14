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
    public partial class frmVoid : Form
    {
        SqlConnection con = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnection db = new DBConnection();
        SqlDataReader dr;
        frmCancelDetail f;
        public frmVoid(frmCancelDetail frm)
        {
            InitializeComponent();
            con = new SqlConnection(db.MyCon());
            f = frm;
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtPass.Text != String.Empty)
                {
                    string user;
                    con.Open();
                    cm = new SqlCommand("select * from tblUser where username = @username and password=@password",con);
                    cm.Parameters.AddWithValue("@username", txtUser.Text);
                    cm.Parameters.AddWithValue("@password", txtPass.Text);
                    dr = cm.ExecuteReader();
                    dr.Read();
                    if (dr.HasRows)
                    { 
                        user = dr["username"].ToString();
                        dr.Close();
                        con.Close();

                        SaveCancelOrder(user);
                        if(f.cboAction.Text=="Yes")
                        {
                            UpdateData("update tblProduct set qty=qty +" + int.Parse(f.txtCancelQty.Text) + " where pcode ='" + f.txtPCode.Text + "'");
                        }
                        UpdateData("delete from tblCart where transno like '"+f.txtTransNo.Text+"'");

                        MessageBox.Show("Order transaction successfully cancelled!","Cancel order",MessageBoxButtons.OK,MessageBoxIcon.Information);
                        this.Dispose();
                        f.RefreshList();
                        f.Dispose();
                    }
                    dr.Close();
                    con.Close();
                }
            }catch(Exception er)
            {
                con.Close();
                MessageBox.Show(er.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        public void SaveCancelOrder(string user)
        {
            con.Open();
            cm = new SqlCommand("insert into tblCancel (transno,pcode,price,qty,sdate,voidby,cancelledby,reason,action)values(@transno,@pcode,@price,@qty,@sdate,@voidby,@cancelledby,@reason,@action)",con);
            cm.Parameters.AddWithValue("@transno", f.txtTransNo.Text);
            cm.Parameters.AddWithValue("@pcode", f.txtPCode.Text);
            cm.Parameters.AddWithValue("@price", double.Parse(f.txtPrice.Text));
            cm.Parameters.AddWithValue("@qty", int.Parse(f.txtCancelQty.Text));
            cm.Parameters.AddWithValue("@sdate", DateTime.Now);
            cm.Parameters.AddWithValue("@voidby", user);
            cm.Parameters.AddWithValue("@cancelledby", f.txtCancel.Text);
            cm.Parameters.AddWithValue("@reason", f.txtReason.Text);
            cm.Parameters.AddWithValue("@action", f.cboAction.Text);
            cm.ExecuteNonQuery();
            con.Close();
        }
        public void UpdateData(string sql)
        {
            con.Open();
            cm = new SqlCommand(sql,con);
            cm.ExecuteNonQuery();
            con.Close(); 
        }

        private void frmVoid_Load(object sender, EventArgs e)
        {

        }
    }
}
