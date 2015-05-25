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
    public partial class AddForm : Form
    {
        List<ElemPictureBox> limg;
        ElemPictureBox cimg;
        List<Label> ll;
        List<TextBox> ltb;
        List<CheckBox> lcb;
        string tableName;
        int eid, cbsi;

        public AddForm(int image_id, string tname)
        {
            InitializeComponent();
            tableName = tname;
            limg = new List<ElemPictureBox>();
            ll = new List<Label>();
            ltb = new List<TextBox>();
            lcb = new List<CheckBox>();
            cbsi = -1;
            List<List<string>> fstr = Query.SendQuerySelect("SELECT id FROM [image]");
            foreach (List<string> str in fstr)
            {
                int img = Convert.ToInt32(str[0]);
                limg.Add(new ElemPictureBox(new Elems(img)));
                comboBox1.Items.Add("Элемент " + img);
                if (img == image_id)
                    cbsi = comboBox1.Items.Count - 1;
            }
            List<string> nstr = Query.getColNames(tname);
            int si = 5;
            for (int i = si; i < nstr.Count; i++)
            {
                Label l = new Label();
                l.AutoSize = true;
                l.Location = new System.Drawing.Point(4, 32 + 23 * (i - si));
                l.Name = "label4";
                l.Size = new System.Drawing.Size(35, 13);
                l.TabIndex = 3 * (i - si) + 1;
                l.Text = nstr[i];
                ll.Add(l);

                TextBox tb = new TextBox();
                tb.Location = new System.Drawing.Point(92, 29 + 23 * (i - si));
                tb.Name = "textBox3";
                tb.Size = new System.Drawing.Size(157, 20);
                tb.TabIndex = 3 * (i - si) + 2;
                ltb.Add(tb);

                CheckBox cb = new CheckBox();
                cb.AutoSize = true;
                cb.Location = new System.Drawing.Point(255, 32 + 23 * (i - si));
                cb.Name = "checkBox1";
                cb.Size = new System.Drawing.Size(88, 17);
                cb.TabIndex = 3 * (i - si) + 3;
                cb.Text = "Отображать";
                cb.UseVisualStyleBackColor = true;
                lcb.Add(cb);
                
                panel1.Controls.Add(l);
                panel1.Controls.Add(tb);
                panel1.Controls.Add(cb);
            }

            fstr = Query.SendQuerySelect("SELECT eid FROM [image_manager]");
            foreach (List<string> str in fstr)
            {
                int teid = Convert.ToInt32(str[0]);
                if (teid > eid)
                    eid = teid;
            }
            eid++;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cimg != null && !cimg.Disposing)
                cimg.Dispose();
            cimg = new ElemPictureBox(limg[comboBox1.SelectedIndex].elem);
            pictureBox1.Controls.Add(cimg);
            cimg.Visible = true;
            cimg.Refresh();
            cimg.Location = new Point((pictureBox1.Width - cimg.Width) / 2, (pictureBox1.Height - cimg.Height) / 2);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string disp = "";
            if (textBox1.Text.Equals("") || textBox2.Text.Equals(""))
            {
                MessageBox.Show("Заполните имя и префикс элемента");
                return;
            }
            foreach(TextBox tb in ltb)
                if (tb.Text.Equals(""))
                {
                    MessageBox.Show("Заполните все параметры элемента");
                    return;
                }
            for (int i = 0; i < lcb.Count; i++)
                if (lcb[i].Checked)
                {
                    if (!disp.Equals(""))
                        disp += ",";
                    disp += (i + 1).ToString();
                }
            if (disp.Equals(""))
                disp += "0";

            string colName = "EID, NAME, PREFIX, DISP";
            foreach (Label l in ll)
                colName += ", " + l.Text;

            string values = eid + ", '" + textBox1.Text + "', '" + textBox2.Text + "', '" + disp + "'";
            foreach (TextBox tb in ltb)
                values += ", '" + tb.Text + "'";


            //Добавление в таблицу элемента
            string queryString = "INSERT " + tableName + " (" + colName + ") VALUES (" + values + ")";
            int q = Query.SendQueryInsert(queryString);

            //Связка с картинкой
            queryString = "INSERT image_manager (eid, image_id) VALUES (" + eid + ", " + limg[comboBox1.SelectedIndex].elem.image_id + ")";
            q = Query.SendQueryInsert(queryString);

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void AddForm_Shown(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = cbsi;
        }
    }
}
