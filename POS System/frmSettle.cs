﻿using System;
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
    public partial class frmSettle : Form
    {
        SqlConnection con = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnection db = new DBConnection();
        SqlDataReader dr;
        frmPOS fpos;
        public frmSettle(frmPOS fp)
        {
            InitializeComponent();
            fpos = fp;
            con = new SqlConnection(db.MyCon());
            this.KeyPreview = true;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Dispose(); 
        }

        private void txtCash_TextChanged(object sender, EventArgs e)
        {
            try
            {
                double sale = double.Parse(txtSale.Text);
                double cash = double.Parse(txtCash.Text);
                double change = cash - sale;
                txtChange.Text = change.ToString("#,##0.00");
            }catch(Exception er)
            {
                txtChange.Text = "0.00";
            }
        }

        private void btn7_Click(object sender, EventArgs e)
        {
            txtCash.Text += btn7.Text;
        }

        private void btn8_Click(object sender, EventArgs e)
        {
            txtCash.Text += btn8.Text;
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            txtCash.Text += btn1.Text;
        }

        private void btn2_Click(object sender, EventArgs e)
        {
            txtCash.Text += btn2.Text;
        }

        private void btn3_Click(object sender, EventArgs e)
        {
            txtCash.Text += btn3.Text;
        }

        private void btn4_Click(object sender, EventArgs e)
        {
            txtCash.Text += btn4.Text;
        }

        private void btn5_Click(object sender, EventArgs e)
        {
            txtCash.Text += btn5.Text;
        }

        private void btn6_Click(object sender, EventArgs e)
        {
            txtCash.Text += btn6.Text;
        }

        private void btn9_Click(object sender, EventArgs e)
        {
            txtCash.Text += btn9.Text;
        }

        private void btnC_Click(object sender, EventArgs e)
        {
            txtCash.Clear();
            txtCash.Focus();
        }

        private void btn0_Click(object sender, EventArgs e)
        {
            txtCash.Text += btn0.Text;
        }

        private void btn00_Click(object sender, EventArgs e)
        {
            txtCash.Text += btn00.Text;
        }

        private void btnEnter_Click(object sender, EventArgs e)
        {
            try
            {
                if((double.Parse(txtChange.Text)<0) || (txtCash.Text==String.Empty))
                {
                    MessageBox.Show("Insufficient Amount","Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;                
                }
                else
                {
                    for(int i=0;i<fpos.dataGridView1.Rows.Count; i++)
                    {
                        con.Open();
                        cm = new SqlCommand("update tblProduct set qty=qty-"+int.Parse(fpos.dataGridView1.Rows[i].Cells[5].Value.ToString())+"where pcode ='"+ fpos.dataGridView1.Rows[i].Cells[2].Value.ToString() + "'",con);
                        cm.ExecuteNonQuery();
                        con.Close();

                        con.Open();
                        cm = new SqlCommand("update tblCart set status ='Sold' where id = '" + fpos.dataGridView1.Rows[i].Cells[1].Value.ToString() + "'",con);
                        cm.ExecuteNonQuery();
                        con.Close();
                    }
                    frmReceipt frm = new frmReceipt(fpos);
                    frm.LoadReport(txtCash.Text,txtChange.Text);
                    frm.ShowDialog();

                    MessageBox.Show("payment successfully saved.", "payment", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    fpos.GetTransNo();
                    fpos.LoadCart();
                    this.Dispose();
                }
                
            }catch(Exception er)
            {
                MessageBox.Show("Insufficient Amount!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void frmSettle_Load(object sender, EventArgs e)
        {

        }

        private void frmSettle_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Escape)
            {
                this.Dispose();
            }
            else if(e.KeyCode==Keys.Enter)
            {
                btnEnter_Click(sender, e);
            }
        }
    }
}
