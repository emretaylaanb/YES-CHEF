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
        private void btn2x_Click(object sender, EventArgs e)
        {
            button((sender as Button));
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            label1.Text = msismi;
            // veri çekme
            SqlCommand cmd = new SqlCommand("SELECT  masa_ismi ,ürünler_id ,İsmi , tane  , Ücret*tane As 'Fiyat' FROM Masa,İçerikler where  ürünler_id = id and masa_ismi = @veri ", con);
            cmd.Parameters.AddWithValue("@veri", label1.Text);// değişti
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(dt);
            

            //içerikleri çekme
            SqlCommand cmd2 = new SqlCommand("Select * from İçerikler", con);
            SqlDataAdapter da2 = new SqlDataAdapter();
            da2.SelectCommand = cmd2;
            da2.Fill(dt2);



            //comboboxa kategori girme 
            string eklenen = "";
            for (int i = 0; i < dt2.Rows.Count; i++)
            {

                DataRow idatarow = dt2.Rows[i];
                if (i == 0) { eklenen = idatarow["Kategori"].ToString(); comboBox1.Items.Add(idatarow["Kategori"].ToString()); }

                if (idatarow["Kategori"].ToString() != eklenen)
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
    }
}