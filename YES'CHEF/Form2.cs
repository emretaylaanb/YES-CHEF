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

        public DataTable dt = new DataTable();
        SqlConnection con = new SqlConnection("Data Source=EMRET;Initial Catalog=Mekan_Days;Integrated Security=True");
        Object nesne;

        private void Form2_Load(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("SELECT  Masa_İsmi ,Durum FROM Masalar", con);
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(dt);

            btn_renk();
            dataGridView1.DataSource = dt;//silinecek
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
               DialogResult result = MessageBox.Show("Yeni adisyon açılsın mı ?", "YES'CHEF", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if(result == DialogResult.Yes)
                {
                    SqlConnection cons = new SqlConnection("Data Source=EMRET;Initial Catalog=Mekan_Days;Integrated Security=True");
                    (sender as Button).BackColor = Color.Lime;
                    cons.Open();
                    SqlCommand kod  = new SqlCommand("Update Masalar Set Durum = 1 Where Masa_İsmi = '"+(sender as Button).Text+"'",cons);
                    kod.ExecuteNonQuery();
                    Form3 form3 = new Form3();
                    form3.msismi = (sender as Button).Text;
                    form3.ShowDialog();
                    for (int i = 0; i<dt.Rows.Count; i++)
                    {
                        DataRow dtrow = dt.Rows[i];
                        if (dtrow["Masa_İsmi"]==(sender as Button).Text)
                        {
                            dtrow["Durum"] = 1;
                        }
                    }
                    cons.Close();
                    cons.Dispose();
                }
            }
            else
            {
                Form3 form3 = new Form3();
                form3.msismi = (sender as Button).Text;
                form3.ShowDialog();
                
            }
           
            
        }

        private void btn_Enter(object sender, EventArgs e)
        {
            nesne = sender;    // bulundu yeni yol
        }


        public void btn_renk() // çalışıyor fakat hantallaştırıyor programı
        {
            flowLayoutPanel2.Controls.Clear();
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

        private void masaSilToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // koşul eklendi
            if ((nesne as Button).BackColor == Color.Lime)
            {
                DialogResult resultt = MessageBox.Show("Silmek istediğiniz masanın adisyonu bulunmaktadır. Silmek istediğinize emin misiniz ?", "YES'CHEF", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (resultt == DialogResult.Yes)
                {
                    masasil();
                }
            }
            else
            {
                masasil();
            }
            
        }

        private void yenidenAdlandırToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form4 fmr4 = new Form4();
            fmr4.isim = (nesne as Button).Text;
            fmr4.masa_acik = true;
            fmr4.ShowDialog();
        }

        public void masasil()
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("delete from Masalar Where Masa_İsmi = @veri", con);
            cmd.Parameters.AddWithValue("@veri", (nesne as Button).Text);
            cmd.ExecuteNonQuery();
            cmd.CommandText = "delete from Masa where masa_ismi = @vveri";
            cmd.Parameters.AddWithValue("@vveri", (nesne as Button).Text); // optimizasyon yapılacak
            cmd.ExecuteNonQuery();
            con.Close();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow row = dt.Rows[i];
                if (row["Masa_İsmi"] == (nesne as Button).Text)
                {
                    row.Delete();
                    dt.AcceptChanges();
                }
            }

            flowLayoutPanel2.Controls.Remove((Control)nesne);
        }

        private void aÇToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btn_Click(nesne,e);
        }

        private void masayıKapatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((nesne as Button).BackColor == Color.Lime)
            {
                DialogResult dd= MessageBox.Show("Masayı kapatmak istediğinize emin misiniz ? Masa kapandıktan sonra adisyon silinecektir .","YES'CHEF",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
                if (dd == DialogResult.Yes)
                {
                  
                    con.Open();

                    //mevcut masanın ürünlerini silme
                    SqlCommand cmd = new SqlCommand("delete from Masa where masa_ismi = @veri", con);
                    cmd.Parameters.AddWithValue("@veri", (nesne as Button).Text);
                    cmd.ExecuteNonQuery();

                    //Masa durumunu düzeltme
                    cmd.CommandText = "Update Masalar Set Durum = 0 Where Masa_İsmi = '" + (nesne as Button).Text + "'";
                    cmd.ExecuteNonQuery();

                    con.Close();

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dtrow = dt.Rows[i];
                        if (dtrow["Masa_İsmi"] == (nesne as Button).Text)
                        {
                            dtrow["Durum"] = 0;
                            btn_renk();
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Masa kapalı.", "YES'CHEF", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void masalarıBirleştirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form5 form5 = new Form5();
            form5.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form6 form6 = new Form6();
            form6.ShowDialog();
        }
    }
}
