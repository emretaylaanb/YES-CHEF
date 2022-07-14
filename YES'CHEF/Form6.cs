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
    public partial class Form6 : Form
    {
        public Form6()
        {
            InitializeComponent();
        }
        Form2 form2 = (Form2)Application.OpenForms["Form2"];
        SqlConnection sqlConnection = new SqlConnection("Data Source =EMRET; Initial Catalog = Mekan_Days; Integrated Security = True");
        private void button1_Click(object sender, EventArgs e)
        {

            form2.dt.PrimaryKey = new DataColumn[] { form2.dt.Columns["Masa_İsmi"]};
            DataRow dtr = form2.dt.Rows.Find(textBox1.Text.ToUpper());
            if (dtr != null)
            {
                textBox1.Clear();
                MessageBox.Show("Bu isimde masa bulunmaktadır.", "YES'CHEF ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                
            }
            else
            {
                sqlConnection.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO Masalar (Masa_İsmi  ,Durum  ) VALUES ('" + textBox1.Text.ToUpper() + "',0)", sqlConnection);
                cmd.ExecuteNonQuery();
                sqlConnection.Close();
                DataRow dataRow = form2.dt.NewRow();
                dataRow["Masa_İsmi"] = textBox1.Text.ToUpper();
                dataRow["Durum"] = 0;
                form2.dt.Rows.Add(dataRow);
                form2.btn_renk();
                this.Close();
            }
             

        }

        private void Form6_Load(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
