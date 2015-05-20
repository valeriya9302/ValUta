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
        public Elems el { set; get; }
        public bool HiddenEl { set; get; }
        private List<Elems> lelem;
        public EditForm(string name, bool hiddenName, Elems elem, bool hiddenEl)
        {
            InitializeComponent();
            textBox1.Text = name;
            HiddenName = hiddenName;
            checkBox1.Checked = HiddenName;

            el = elem;
            HiddenEl = hiddenEl;
            checkBox2.Checked = hiddenEl;

            lelem = new List<Elems>();
            List<List<string>> fstr = Query.SendQuerySelect("SELECT * FROM [" + el.TableName + "]");
            foreach (List<string> str in fstr)
            {
                Elems tValue = new Elems(str, el.TableName);
                comboBox1.Items.Add(tValue.toString());
                lelem.Add(tValue);
                if (!el.Model && tValue.id == el.id)
                {
                    comboBox1.SelectedIndex = comboBox1.Items.Count - 1;
                    button2.Enabled = true;
                }
            }
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

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            HiddenEl = checkBox2.Checked;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            el = lelem[comboBox1.SelectedIndex];
            button2.Enabled = true;
        }
    }
}
