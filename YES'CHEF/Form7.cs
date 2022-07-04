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
    public partial class Form7 : Form
    {
        public Form7()
        {
            InitializeComponent();
        }
        DataTable datatable = new DataTable();
        SqlConnection conn = new SqlConnection("Data Source=EMRET;Initial Catalog=Mekan_Days;Integrated Security=True");
        private void flowLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Form7_Load(object sender, EventArgs e)
        {
            
            SqlCommand cmd = new SqlCommand("Select id,  İsmi , Ücret , Kategori  FROM İçerikler order by id", conn);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
            sqlDataAdapter.SelectCommand = cmd;
            sqlDataAdapter.Fill(datatable);

            dataGridView1.DataSource = datatable;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.CellBorderStyle=DataGridViewCellBorderStyle.Sunken;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.ColumnHeadersHeight = 50;
            for (int i = 0; i < datatable.Rows.Count; i++)
            {
                DataRow idatarow = datatable.Rows[i];
                if (comboBox1.Items.IndexOf(idatarow["Kategori"]) == -1)
                {
                    comboBox1.Items.Add(idatarow["Kategori"].ToString());
                }
            }
            textBox1.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            textBox2.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            comboBox1.SelectedItem = dataGridView1.CurrentRow.Cells[3].Value.ToString();


        }

       

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
        
            textBox1.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            textBox2.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            comboBox1.SelectedItem = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            

        }

      

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != dataGridView1.CurrentRow.Cells[1].Value.ToString() || textBox2.Text != dataGridView1.CurrentRow.Cells[2].Value.ToString() || comboBox1.Text != dataGridView1.CurrentRow.Cells[3].Value.ToString())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("UPDATE İçerikler SET İsmi = @veri1 , Ücret = @veri2 , Kategori = @veri3 Where id = @verid", conn);
                cmd.Parameters.AddWithValue("@veri1", textBox1.Text);
                cmd.Parameters.AddWithValue("@veri2", textBox2.Text);
                cmd.Parameters.AddWithValue("@veri3", comboBox1.Text);
                cmd.Parameters.AddWithValue("@verid", dataGridView1.CurrentRow.Cells[0].Value);
                cmd.ExecuteNonQuery();
                conn.Close();
                datatable.PrimaryKey = new DataColumn[] { datatable.Columns["id"] };
                DataRow dtr = datatable.Rows.Find(dataGridView1.CurrentRow.Cells[0].Value);
                dtr["İsmi"] = textBox1.Text;
                dtr["Ücret"]= textBox2.Text;
                dtr["Kategori"]=comboBox1.Text;
                MessageBox.Show("İçerik güncellenmiştir", "YES'CHEF", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Veriler güncel", "YES'CHEF", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            comboBox1.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            datatable.PrimaryKey = new DataColumn[] { datatable.Columns["İsmi"] };
            DataRow dtr = datatable.Rows.Find(textBox1.Text);
            if (dtr == null)
            {
                conn.Open();
                SqlCommand cmdx = new SqlCommand(" INSERT INTO İçerikler(İsmi, Ücret,Kategori) VALUES('" + textBox1.Text + "', '" + textBox2.Text + "' , '"+comboBox1.Text+"')", conn);
                cmdx.ExecuteNonQuery();
                conn.Close();
                DataRow dataRow = datatable.NewRow();
                dataRow["id"] = datatable.Rows.Count+1;
                dataRow["İsmi"] = textBox1.Text;
                dataRow["Ücret"] = textBox2.Text;
                dataRow["Kategori"] = comboBox1.Text;
                datatable.Rows.Add(dataRow);
                MessageBox.Show("Ürün başarıyla eklnedi","YES'CHEF",MessageBoxButtons.OK, MessageBoxIcon.Information);
                textBox1.Clear();
                textBox2.Clear();
                comboBox1.SelectedItem = 0;
                textBox1.Focus();
            }
            else
            {
                MessageBox.Show("Bu isimle ürün bulunmaktadır", "YES'CHEF", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult dg = MessageBox.Show(dataGridView1.CurrentRow.Cells[1].Value + "silmek istediğinize emin misiniz ?", "YES'CHEF", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if(dg == DialogResult.Yes)
            {
                conn.Open();
                SqlCommand sqlCommand = new SqlCommand("delete from İçerikler where id = '"+ dataGridView1.CurrentRow.Cells[0].Value.ToString() + "' ",conn);
                sqlCommand.ExecuteNonQuery();
                conn.Close();

                datatable.PrimaryKey = new DataColumn[] { datatable.Columns["id"] };
                DataRow dtr = datatable.Rows.Find(dataGridView1.CurrentRow.Cells[0].Value.ToString());
                dtr.Delete();
                datatable.AcceptChanges();

                dataGridView1_CellClick(sender,null);
                MessageBox.Show("Ürün başarıyla silindi ", "YES'CHEF", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
           
        }
    }
}
