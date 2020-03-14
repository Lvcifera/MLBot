using System;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            textBox1.Text = "Tiksi";
            textBox2.Text = "F4ATj";
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Authorization auth = new Authorization();
            var main=auth.getMain();
            var vers = auth.FindVersion(main);
            var salt = auth.getSalt();
            var hash = auth.getSaltPasswordHash(salt, textBox2.Text);
            var res = auth.Authorize(textBox1.Text, hash, vers);
            label4.Text = salt;
            label5.Text = hash;
            label6.Text = res;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

    }
}