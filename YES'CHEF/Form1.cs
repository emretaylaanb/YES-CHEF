using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YES_CHEF
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text== "admin" && textBox2.Text == "admin")
            {
                Form2 fomr2 = new Form2();
                fomr2.Show();
                this.Hide();
            }
            else
            {
                textBox2.Clear();
                MessageBox.Show("Kullanıcı adı veya şifre yanlış","YES'CHEF",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.ActiveControl = textBox2;
        }
    }
}
