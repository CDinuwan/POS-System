using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;


namespace POS_System
{
    public class DBConnection
    {
        SqlConnection con = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        private double dailySales;
        private string cn;
        private int productLine;
        private int stockonhand;
        public string MyCon()
        {
            cn = @"Data Source=DESKTOP-OUGSDHL\SQLEXPRESS;Initial Catalog=POS;Integrated Security=True";
            return cn;
        }
        public double GetVal()
        {
            double vat = 0 ;
            con.ConnectionString = MyCon();
            con.Open();
            cm = new SqlCommand("select * from tblVat", con);
            dr = cm.ExecuteReader();
            while(dr.Read())
            {
                vat = Double.Parse(dr["vat"].ToString());
            }
            dr.Close();
            con.Close();
            return vat;
        }
        public double dailySale()
        {
            string sdate = DateTime.Now.ToShortDateString();
            con = new SqlConnection();
            con.ConnectionString=cn;
            con.Open();
            cm = new SqlCommand("select isnull(sum(total),0) as total from tblCart where sdate between'" + sdate + "'and'" + sdate + "'and status like 'Sold'", con);
            dailySales = double.Parse(cm.ExecuteScalar().ToString());
            con.Close();
            return dailySales;
        }
        public int ProductLine()
        {
            con = new SqlConnection();
            con.ConnectionString = cn;
            con.Open();
            cm = new SqlCommand("select count(*) from tblProduct", con);
            productLine = int.Parse(cm.ExecuteScalar().ToString());
            con.Close();
            return productLine;
        }
        public int StockOnHand()
        {
            con = new SqlConnection();
            con.ConnectionString = cn;
            con.Open();
            cm = new SqlCommand("select isnull(sum(qty),0) as qty from tblProduct", con);
            stockonhand = int.Parse(cm.ExecuteScalar().ToString());
            con.Close();
            return stockonhand;
        }
        public String GetPassword(string user)
        {
            string password="";
            con.ConnectionString = MyCon();
            con.Open();
            cm = new SqlCommand("select * from tblUser where username=@username", con);
            cm.Parameters.AddWithValue("@username", user);
            dr = cm.ExecuteReader();
            dr.Read();
            if(dr.HasRows)
            {
                password = dr["password"].ToString();
            }
            dr.Close();
            con.Close();
            return password;
        }
    }
    
}
