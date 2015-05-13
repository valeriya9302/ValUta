using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MainWindow.scheme
{
    public partial class EditForm : Form
    {
        public string name { set; get; }
        public bool HiddenName { set; get; }
        public EditForm(string name, bool hiddenName)
        {
            InitializeComponent();
            textBox1.Text = name;
            HiddenName = hiddenName;
            checkBox1.Checked = HiddenName;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            name = textBox1.Text;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            HiddenName = checkBox1.Checked;
        }
    }
}
