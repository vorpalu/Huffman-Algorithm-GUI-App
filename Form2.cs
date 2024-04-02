using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace WindowsFormsApp1
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        public string CompressionInfo
        {
            get { return label2.Text; }
            set { label2.Text = value; }
        }


        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Закрываем текущую форму
            this.Hide();
            // После закрытия второй формы, закрываем приложение
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label2.Text = CompressionInfo;
        }

    }
}
