using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PhotoDiary
{
    public partial class Form2 : Form
    {
        Form1 form1 = null;
        byte[] image = null;
        string imgLocation;
        public Form2(Form1 form1)
        {
            InitializeComponent();
            this.form1 = form1;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            

            openFileDialog1.Title = "Select image to be upload.";
            openFileDialog1.Filter = "Image Only(*.jpg; *.jpeg; *.gif; *.bmp; *.png)|*.jpg; *.jpeg; *.gif; *.bmp; *.png";
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (openFileDialog1.CheckFileExists)
                {
                    imgLocation = openFileDialog1.FileName.ToString();
                    pictureBox1.Image = new Bitmap(openFileDialog1.FileName);
                    pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                }
            }
            else
            {
                MessageBox.Show("Please Upload image.");
            }
        }

        private void Button2_Click(object sender, EventArgs e)// form 2 submit button
        {
            if (imgLocation == null)
            {
                MessageBox.Show("Please select a valid image.");
            }
            else
            {
                FileStream Stream = new FileStream(imgLocation, FileMode.Open, FileAccess.Read);//
                byte[] bImageData = new byte[Stream.Length];
                Stream.Read(bImageData, 0, System.Convert.ToInt32(Stream.Length));// for byte conversion
                Stream.Close();
                DateTime time = DateTime.Now;
                string date1 = time.ToString();
                string query = "INSERT INTO PhotoDir VALUES(@images,@title,@date,@dec)";
                form1.conn.Open();
                SqlCommand cmd = new SqlCommand(query, form1.conn);
                cmd.Parameters.AddWithValue("@images", bImageData);
                cmd.Parameters.AddWithValue("@date", date1);
                cmd.Parameters.AddWithValue("@title", textBox1.Text);
                cmd.Parameters.AddWithValue("@dec", textBox2.Text);
                cmd.ExecuteNonQuery();
                form1.conn.Close();
                MessageBox.Show("Image uploaded successfully.");
                form1.ds.Clear();
                this.Close();
            }
        }
    }
}
