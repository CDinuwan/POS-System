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
    public partial class frmStore : Form
    {
        SqlConnection con;
        SqlCommand cm;
        SqlDataReader dr;
        DBConnection db = new DBConnection();
        public frmStore()
        {
            InitializeComponent();
            con = new SqlConnection(db.MyCon());
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        public void LoadRecords()
        {
            con.Open();
            cm = new SqlCommand("select * from tblStore",con);
            dr = cm.ExecuteReader();
            dr.Read();
            if(dr.HasRows)
            {
                txtAddress.Text = dr["address"].ToString();
                txtStore.Text = dr["store"].ToString();
            }
            else
            {
                txtStore.Clear();
                txtStore.Clear();
            }
            dr.Close();
            con.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Save store Info?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    int count;
                    con.Open();
                    cm = new SqlCommand("select count(*) from tblStore", con);
                    count = int.Parse(cm.ExecuteScalar().ToString());
                    con.Close();
                    if(count>0)
                    {
                        con.Open();
                        cm = new SqlCommand("update tblStore set store=@store,address=@address", con);
                        cm.Parameters.AddWithValue("@store", txtStore.Text);
                        cm.Parameters.AddWithValue("@address", txtAddress.Text);
                        cm.ExecuteNonQuery();
                    }
                    else
                    {
                        con.Open();
                        cm = new SqlCommand("insert into tblStore (store,address)values(@store,@address)", con);
                        cm.Parameters.AddWithValue("@store", txtStore.Text);
                        cm.Parameters.AddWithValue("@address", txtAddress.Text);
                        cm.ExecuteNonQuery();
                    }
                    MessageBox.Show("Store details has been successfully saved!", "Save Record", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
            }catch(Exception er)
            {
                con.Close();
                MessageBox.Show(er.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        
    }
}
