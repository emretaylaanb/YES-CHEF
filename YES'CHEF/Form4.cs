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
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }
        public string isim;
        public bool masa_acik = false;
        
        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Form4_Load(object sender, EventArgs e)
        {
            textBox1.Text = isim;
            label3.Text = isim;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (textBox1.Text == isim)
            {
                this.Close();
            }
            else
            {

                Form2 form2 = (Form2)Application.OpenForms["Form2"];
                form2.dt.PrimaryKey = new DataColumn[] { form2.dt.Columns["Masa_İsmi"] };
                DataRow dtr = form2.dt.Rows.Find(textBox1.Text);

                if (dtr != null)
                {
                    textBox1.Clear();
                    MessageBox.Show("Bu isimle masa bulunmaktadır.", "YES'CHEF", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    SqlConnection con = new SqlConnection("Data Source=EMRET;Initial Catalog=Mekan_Days;Integrated Security=True");
                    con.Open();
                    SqlCommand cmd = new SqlCommand("UPDATE Masalar SET Masa_İsmi = @yveri WHERE Masa_İsmi = @everi ", con);
                    cmd.Parameters.AddWithValue("@yveri", textBox1.Text);
                    cmd.Parameters.AddWithValue("@everi", isim);
                    if (masa_acik)
                    {
                        SqlCommand cmd2 = new SqlCommand("UPDATE Masa SET masa_ismi = @veri Where masa_ismi = @eeveri", con);
                        cmd2.Parameters.AddWithValue("@veri", textBox1.Text);
                        cmd2.Parameters.AddWithValue("@eeveri", isim);
                        cmd2.ExecuteNonQuery();
                    }

                    cmd.ExecuteNonQuery();
                    con.Close();



                    for (int i = 0; i < form2.dt.Rows.Count; i++)
                    {
                        DataRow dtrow = form2.dt.Rows[i];
                        if (dtrow["Masa_İsmi"] == isim)
                        {
                            dtrow["Masa_İsmi"] = textBox1.Text;
                            form2.btn_renk();
                        }
                    }

                    this.Close();
                }
            }
            
        }
    }
}
