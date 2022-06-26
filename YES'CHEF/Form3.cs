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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        SqlConnection con = new SqlConnection("Data Source=EMRET;Initial Catalog=Mekan_Days;Integrated Security=True");
        DataTable dt = new DataTable();
        DataTable dt2 = new DataTable();
        int adet = 1;
        bool ikram = false;
        public string msismi = "";
        public object masa_butn;
        
         //FormIlk nesnesini tanımlıyoruz. Burda önemli olan new diye yeni bir nesne değil, Application.OpenForms komutuyla açık olan Formlar arasından tanımlıyoruz


        private void btn2x_Click(object sender, EventArgs e)
        {
            button((sender as Button));
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            label1.Text = msismi;
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;//sadece girilen verilerin görüntülenmesini sağlar kullanıcı textbox özelliğini kulllanamaz 
            // veri çekme
            SqlCommand cmd = new SqlCommand("SELECT  masa_ismi ,ürünler_id ,İsmi , tane  , Ücret*tane As 'Fiyat' FROM Masa,İçerikler where  ürünler_id = id and masa_ismi = @veri ", con);
            cmd.Parameters.AddWithValue("@veri", msismi);// değişti
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(dt);
            

            //içerikleri çekme
            SqlCommand cmd2 = new SqlCommand("Select * from İçerikler", con);
            SqlDataAdapter da2 = new SqlDataAdapter();
            da2.SelectCommand = cmd2;
            da2.Fill(dt2);

            

            //comboboxa kategori girme 

            for (int i = 0; i < dt2.Rows.Count; i++)
            {
                DataRow idatarow = dt2.Rows[i];
                if (comboBox1.Items.IndexOf(idatarow["Kategori"]) == -1)
                {
                    comboBox1.Items.Add(idatarow["Kategori"].ToString());
                }
            }

            oluşturma();
            con.Close();
            con.Dispose();
            listeleme();
        }




        void listeleme()
        {
            listBox1.Items.Clear();
            //listbox yazdırma
            int ucrt = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i]; //tanım
                string ürün = "";
                if (Convert.ToInt32(dr["tane"]) > 90)
                {
                    dr["Fiyat"] = 0;
                    ürün = String.Format("{0}$ \t {1} (İkram)   \t {2}x", dr["Fiyat"], dr["İsmi"], Convert.ToInt32(dr["tane"]) - 90);
                }
                else
                {
                    ürün = String.Format("{0}$ \t {1}               \t {2}x", dr["Fiyat"], dr["İsmi"], dr["tane"]);
                }

                listBox1.Items.Add(ürün);
                ucrt += Convert.ToInt32(dr["Fiyat"]);
                label2.Text = ucrt.ToString() + "$";
            }
            adet = 1;
        }


        void içerikler(int id, string isim)
        {
            Button btn = new Button();
            btn.Width = 64;
            btn.Height = 80;
            btn.Text = isim; // değişti
            btn.TextAlign = ContentAlignment.BottomCenter;
            btn.Name = "button_" + id; //değişti
            btn.Click += new EventHandler(btn_Click);
            flowLayoutPanel2.Controls.Add(btn);

        }

        private void btn_Click(object sender, EventArgs e)
        {
            DataRow dataRow = dt.NewRow();
            dataRow["masa_ismi"] = label1.Text; //değişecek
            dataRow["ürünler_id"] = (sender as Button).Name.Substring(7);
            dataRow["İsmi"] = (sender as Button).Text;
            if (ikram == true)
            {
                dataRow["tane"] = adet + 90;
                dataRow["Fiyat"] = 0;
                ikram = false;
            }
            else
            {
                dataRow["tane"] = adet;
                dataRow["Fiyat"] = sorgu(Convert.ToInt32((sender as Button).Name.Substring(7))) * adet; //değişti (sender as Button).Name.Substring(7)
            }


            dt.Rows.Add(dataRow);
            listeleme();
            adet_temizle();
        }



        public int sorgu(int sorgu2)
        {
            for (int i = 0; i < dt2.Rows.Count; i++)
            {
                DataRow dddrow = dt2.Rows[i];
                if (Convert.ToInt32(dddrow["id"]) == sorgu2)
                {
                    return Convert.ToInt32(dddrow["Ücret"]);
                }
            }
            return -1;

        }

        void oluşturma()
        {
            for (int i = 0; i < dt2.Rows.Count; i++)
            {
                DataRow drow = dt2.Rows[i];
                if (comboBox1.SelectedItem.ToString() == "Hepsi")
                {
                    içerikler(Convert.ToInt32(drow["id"]), drow["İsmi"].ToString());
                }
                else
                {
                    if (drow["Kategori"].ToString() == comboBox1.SelectedItem.ToString())
                    {
                        içerikler(Convert.ToInt32(drow["id"]), drow["İsmi"].ToString());
                    }
                }
            }
        }


        void button(Control name) // adet ekleme
        {

            if (name.BackColor == Color.Lime)
            {
                name.BackColor = Color.FromArgb(224, 224, 224);
                adet = 1;
            }
            else
            {
                adet_temizle();
                name.BackColor = Color.Lime;
                adet = Convert.ToInt32(name.Name.Substring(3, 1));
            }
        }

        public void adet_temizle() // diğer adetleri temizleme
        {
            btn2x.BackColor = Color.FromArgb(224, 224, 224);
            btn3x.BackColor = Color.FromArgb(224, 224, 224);
            btn4x.BackColor = Color.FromArgb(224, 224, 224);
            btn5x.BackColor = Color.FromArgb(224, 224, 224);
            btn6x.BackColor = Color.FromArgb(224, 224, 224);
            if (ikram == false) { btn7x.BackColor = Color.FromArgb(224, 224, 224); }

        }

        void kayıt()
        {

            SqlConnection cone = new SqlConnection("Data Source=EMRET;Initial Catalog=Mekan_Days;Integrated Security=True");
            cone.Open();
            SqlCommand cmds = new SqlCommand("delete from Masa where masa_ismi = @veri", cone);
            cmds.Parameters.AddWithValue("@veri", label1.Text);
            cmds.ExecuteNonQuery();
            //silme

            //veri girme 
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i]; //tanım 
                SqlCommand cmd = new SqlCommand("INSERT INTO Masa (masa_ismi , ürünler_id , tane) VALUES ('" + dr["masa_ismi"] + "' ,'" + dr["ürünler_id"] + "' , '" + dr["tane"] + "' )", cone); // gönderim
                cmd.ExecuteNonQuery();
            }
            //bağlantı kapatma
            cone.Close();
            cone.Dispose();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            flowLayoutPanel2.Controls.Clear();
            oluşturma();
        }

        private void button1_Click(object sender, EventArgs e)
        {
           kayıt();
           this.Close();
        }

        private void btn7x_Click(object sender, EventArgs e)
        {
            if (btn7x.BackColor == Color.Lime)
            {
                btn7x.BackColor = Color.FromArgb(224, 224, 224);
                ikram = false;
            }
            else
            {

                ikram = true;
                btn7x.BackColor = Color.Lime;
            }
        }

        private void sİLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                DataRow drow = dt.Rows[listBox1.SelectedIndex];
                drow.Delete();
                dt.AcceptChanges();
                listeleme();
            }
        }

        
        private void button3_Click_1(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Ödeme alındı mı ?", "YES'CHEF Masa Kapanış", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                //Masa Silme
                SqlConnection cone = new SqlConnection("Data Source=EMRET;Initial Catalog=Mekan_Days;Integrated Security=True");
                cone.Open();
                SqlCommand cmd = new SqlCommand("delete from Masa where masa_ismi = @veri", cone);
                cmd.Parameters.AddWithValue("@veri", label1.Text);
                cmd.ExecuteNonQuery();

                //LOG girme
                cmd.CommandText = "INSERT INTO Kayıt (Masa_Num , Ödeme_Türü , Ödenen_Tutar , Tarih) VALUES ('" + label1.Text + "' ,'" + (sender as Button).Text + "' , '" + Convert.ToInt32(label2.Text.Substring(0,label2.Text.Length-1)) + "' , '"+(DateTime.Now.Hour+":"+DateTime.Now.Minute+"-"+DateTime.Now.Day+"."+DateTime.Now.Month+"."+DateTime.Now.Year).ToString()+"')"; // gönderim
                cmd.ExecuteNonQuery();

                //Masa durumunu düzeltme
                cmd.CommandText= "Update Masalar Set Durum = 0 Where Masa_İsmi = '" + label1.Text + "'";
                cmd.ExecuteNonQuery();

                //bağlantı kapanış
                cone.Close();
                cone.Dispose();

                
                Form2 form2 = (Form2)Application.OpenForms["Form2"];
               
                for (int i = 0; i < form2.dt.Rows.Count; i++)
                {
                    DataRow dtrow = form2.dt.Rows[i];
                    if (dtrow["Masa_İsmi"] == msismi)
                    {
                        dtrow["Durum"] = 0;
                        form2.btn_renk();
                    }
                }

                this.Close();
                
   
            }


        }
    }
}
