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
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
        }
        Form2 form2 = (Form2)Application.OpenForms["Form2"];
        private void Form5_Load(object sender, EventArgs e)
        {
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
            for (int i =0; i< form2.dt.Rows.Count; i++)
            {
                DataRow idatarow = form2.dt.Rows[i];
                
                if (Convert.ToInt32(idatarow["Durum"]) == 1)
                {
                    comboBox1.Items.Add(idatarow["Masa_İsmi"].ToString());  
                }
            }
            



        }
        SqlConnection sqlConnection = new SqlConnection("Data Source=EMRET;Initial Catalog=Mekan_Days;Integrated Security=True");
        private void button1_Click(object sender, EventArgs e)
        {
            

                sqlConnection.Open();
                if (checkBox1.Checked == true)
                {
                    işlem();
                    form2.masasil();
                }
                else
                {
                    işlem();
                    for (int i = 0; i < form2.dt.Rows.Count; i++)
                    {
                        DataRow dtrow = form2.dt.Rows[i];
                        if (dtrow["Masa_İsmi"] == comboBox1.SelectedItem.ToString())
                        {
                            dtrow["Durum"] = 0;

                        }

                    }

                }
                form2.btn_renk();
                sqlConnection.Close();
                this.Close();
            
        }

        void işlem()
        {
            SqlCommand cmd = new SqlCommand("update Masa Set masa_ismi = '" + comboBox2.SelectedItem.ToString() + "' where masa_ismi = '" + comboBox1.SelectedItem.ToString() + "'", sqlConnection);
            cmd.ExecuteNonQuery();
            cmd.CommandText = "update Masalar set Durum = 0 where Masa_İsmi = '" + comboBox1.SelectedItem.ToString() + "'";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "update Masalar set Durum = 1 where Masa_İsmi = '" + comboBox2.SelectedItem.ToString() + "'";
            cmd.ExecuteNonQuery();
            for (int i = 0; i < form2.dt.Rows.Count; i++)
            {
                DataRow dtrow = form2.dt.Rows[i];
                if (dtrow["Masa_İsmi"] == comboBox2.SelectedItem.ToString())
                {
                    dtrow["Durum"] = 1;

                }
            } 
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.Items.Clear();
            for (int i = 0; i < form2.dt.Rows.Count; i++)
            {
                DataRow idatarow = form2.dt.Rows[i];
                if (idatarow["Masa_İsmi"] != comboBox1.SelectedItem)
                {
                    comboBox2.Items.Add(idatarow["Masa_İsmi"].ToString());
                }
    
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
