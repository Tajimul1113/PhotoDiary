using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;

namespace PhotoDiary
{
    public partial class Form1 : Form
    {
        string cs = ConfigurationManager.ConnectionStrings["photoGal"].ConnectionString; //database connrcetion string
        public SqlConnection conn;
        SqlCommand cmd;
        public DataSet ds;// for get the autometic list
        public Form1()
        {
            InitializeComponent();
            conn = new SqlConnection(cs);
            ds = new DataSet();
            string query = "select eventName from PhotoDir";
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                adapter.Fill(ds);
                listBox1.DisplayMember = "eventName";
                listBox1.DataSource = ds.Tables[0];
                conn.Close();
            }
        }



        private void ListBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            DataRowView dr = (DataRowView)listBox1.SelectedValue;//table view
            string imgName;
            if (dr != null)
            {
                imgName = dr["eventName"].ToString();
            }
            else
            {
                imgName = null;
            }
            string query = "select img,evedec,lastdate from PhotoDir where eventName='"+imgName+"'";
            if (conn.State == ConnectionState.Closed && imgName!=null)
            {
                conn.Open();
                SqlCommand comn = new SqlCommand(query, conn);
                SqlDataReader reader = comn.ExecuteReader();
                while (reader.Read())
                {
                    byte[] img = (byte[])reader["img"];//convert
                    MemoryStream ms = new MemoryStream(img);
                    pictureBox1.Image = new Bitmap(ms);//convert
                    pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                    label4.Text = (string)reader["evedec"];//title
                    label2.Text = imgName;
                    label5.Text = (string)reader["lastdate"];//date
                }
                conn.Close();
            }  
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2(this);
            form2.ShowDialog();// open the form 2
            conn.Open();
            
            string query = "select eventName from PhotoDir";
            SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
            //ds.Clear();
            adapter.Fill(ds);
            listBox1.DisplayMember = "eventName";
            listBox1.DataSource = ds.Tables[0];
            conn.Close();
        }
    }
}
