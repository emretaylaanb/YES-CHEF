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

namespace YES_CHEF
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        DataTable dt = new DataTable();
        SqlConnection con = new SqlConnection("Data Source=EMRET;Initial Catalog=Mekan_Days;Integrated Security=True");
        Object nesne;

        private void Form2_Load(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("SELECT  Masa_İsmi ,Durum FROM Masalar ", con);
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(dt);
            Color a = Color.Gray;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                if (Convert.ToInt32(dr["Durum"]) == 1)
                {
                    a = Color.Lime;
                }
                else
                {
                    a = Color.Gray;
                }
                içerikler(dr["Masa_İsmi"].ToString(), a, i);
            }
        }

        void içerikler(string isim, Color s, int id)
        {
            Button btn = new Button();
            btn.Width = 157;
            btn.Height = 225;
            btn.Text = isim; // değişti
            btn.TextAlign = ContentAlignment.MiddleCenter;
            btn.Name = "button_" + id; //değişti
            btn.Click += new EventHandler(btn_Click);
            btn.MouseEnter += new EventHandler(btn_Enter);
            btn.BackColor = s;
            btn.ContextMenuStrip = contextMenuStrip1;
            flowLayoutPanel2.Controls.Add(btn); //Tahoma; 20,25pt; style=Bold

        }

        private void btn_Click(object sender, EventArgs e)
        {
            if ((sender as Button).BackColor == Color.Gray)
            {
                MessageBox.Show("Yeni adisyon açılsın mı ?", "YES'CHEF", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            }
            Form3 form3 = new Form3();
            form3.msismi = (sender as Button).Text;
            form3.ShowDialog();
            
        }

        private void btn_Enter(object sender, EventArgs e)
        {
            nesne = sender;    // YENİ BİR YOL LAZM
        }




    }
}
