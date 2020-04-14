using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS_System
{
    public partial class frmCancelDetail : Form
    {
        frmSolditems f;
        public frmCancelDetail(frmSolditems frm)
        {
            InitializeComponent();
            f = frm;
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void frmCancelDetail_Load(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }
        public void RefreshList()
        {
            f.LoadRecord();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void cboAction_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if((cboAction.Text!=String.Empty) && (txtReason.Text!=String.Empty) && (txtCancelQty.Text!=String.Empty))
                {
                    if(int.Parse(txtQty.Text)==int.Parse(txtCancelQty.Text))
                    {
                        frmVoid frm = new frmVoid(this);
                        frm.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("Please add correct quantity");
                    }
                        
                }
                
            }catch(Exception er)
            {
                MessageBox.Show(er.Message,"Error",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
        }

        private void txtCancelQty_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
