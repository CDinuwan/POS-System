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
    public partial class frmQty : Form
    {
        SqlConnection con = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnection db = new DBConnection();
        SqlDataReader dr;

        private String pcode;
        private double price;
        private string transno;

        string sittle = "Simple POS system";
        frmPOS fpos;
        public frmQty(frmPOS frmpos)
        {
            InitializeComponent();
            con = new SqlConnection(db.MyCon());
            fpos = frmpos;
        }

        private void frmQty_Load(object sender, EventArgs e)
        {

        }
        public void productDetails(String pcode,double price,String transno)
        {
            this.pcode = pcode;
            this.price = price;
            this.transno = transno;
        }

        private void txtQty_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            if((e.KeyChar==13)&&(txtQty.Text!=String.Empty))
            {
                string id="";
                bool found = false;
                con.Open();
                cm = new SqlCommand("select * from tblCart where transno=@transno and pcode=@pcode", con);
                cm.Parameters.AddWithValue("@transno", fpos.lblTransno.Text);
                cm.Parameters.AddWithValue("@pcode", pcode);
                dr = cm.ExecuteReader();
                dr.Read();
                if(dr.HasRows)
                {
                    found = true;
                    id = dr["id"].ToString();
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
                    cm = new SqlCommand("update tblCart set qty=(qty+"+int.Parse(txtQty.Text)+")where id='"+id+"'", con);
                    cm.ExecuteNonQuery();
                    con.Close();

                    fpos.txtSearch.Clear();
                    fpos.txtSearch.Focus();
                    fpos.LoadCart();
                    this.Dispose();
                }
                else
                {
                    con.Open();
                    cm = new SqlCommand("insert into tblCart(transno,pcode,price,qty,sdate,cashier)values(@transno,@pcode,@price,@qty,@sdate,@cashier)", con);
                    cm.Parameters.AddWithValue("@transno", transno);
                    cm.Parameters.AddWithValue("@pcode", pcode);
                    cm.Parameters.AddWithValue("@price", price);
                    cm.Parameters.AddWithValue("@qty", int.Parse(txtQty.Text));
                    cm.Parameters.AddWithValue("@sdate", DateTime.Now);
                    cm.Parameters.AddWithValue("@cashier", fpos.lblUser.Text);
                    cm.ExecuteNonQuery();
                    con.Close();

                    fpos.txtSearch.Clear();
                    fpos.txtSearch.Focus();
                    fpos.LoadCart();
                    this.Dispose();
                }
            }

        }
    }
}
