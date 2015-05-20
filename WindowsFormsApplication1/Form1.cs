using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MainWindow.scheme;

namespace MainWindow
{
    public partial class Form1 : Form
    {
        private elements db = new elements();
        //private CategPictureBox cpb;
        Favorites fav;
        //private List<
        //private Images _images = new Images();

        private class Favorites
        {
            public List<ElemPictureBox> epb;
            public List<ToolStripItem> tsi;
            private Elems elem;
            public int CurrentIndex { set; get; }
            public Favorites()
            {
                epb = new List<ElemPictureBox>();
                tsi = new List<ToolStripItem>();
            }
            public void SetPretendent(Elems el)
            {
                elem = el;
                elem.Model = true;
            }
            public System.Drawing.Image AddPretendent()
            {
                epb.Add(new ElemPictureBox(elem));
                return epb[epb.Count - 1].getImage();
            }
            public void Remove()
            {
                epb.RemoveAt(CurrentIndex);
            }
            public ElemPictureBox get()
            {
                return epb[CurrentIndex];
            }
        }

        public Form1()
        {
            InitializeComponent();
            // Заполнение элементов с бд
            //elements db = new elements();
            db.load();
            List<int> ind = new List<int>();
            ind.Add(0);
            loadDataElems(treeView1.Nodes, ind);
            fav = new Favorites();
        }

        private void loadDataElems(TreeNodeCollection _treeView, List<int> _ind)
        {
            //foreach (string temp in db.GetLevel(ind))
            List<string> temp = db.GetLevel(_ind);
            if (temp == null)
                return;
            List<int> ind = new List<int>(_ind);
            ind.Add(0);
            for(int i = 0; i < temp.Count; i++)    
            {
                //Console.WriteLine(temp[i]);
                _treeView.Add(temp[i]);
                ind[ind.Count - 1] = i;
                loadDataElems(_treeView[i].Nodes, ind);
            }
        }

        private void CreateToolStripButton_Click(object sender, EventArgs e)
        {
            //Создание нового поля
            tabControl1.SelectedIndex = AddNewDiagramBox();
        }

        public int AddNewDiagramBox()
        {
            //новая вкладка
            int new_last = tabControl1.TabPages.Count;
            tabControl1.TabPages.Add(String.Format("default {0}", new_last + 1));
            //tabControl1.TabPages[new_last].Controls.Add(panel1);
            //Делаем её активной
            tabControl1.SelectedIndex = new_last;
            return new_last;
        }

        //Запуск
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            //Проверка на ошбки
            //Если ошибок нет, проверяем работу
            //Если есть, то выводим экран с ошибками
        }

        private void группуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Создание группы элементов в подгруппе
            treeView1.SelectedNode = treeView1.SelectedNode.Nodes.Add("new group (in group)");
            treeView1.SelectedNode.BeginEdit();
        }

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            treeView1.SelectedNode.Remove();
        }

        private void добавитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Создание группы элементов в корне
            treeView1.SelectedNode = treeView1.Nodes.Add("new group");
            treeView1.SelectedNode.BeginEdit();
        }

        private void добавитьЭлементToolStripMenuItem_Click(object sender, EventArgs e)
        {
            treeView1.Nodes.Add("new element");//.ContextMenuStrip = cms_el_gr;
        }

        private void treeView1_MouseUp(object sender, MouseEventArgs e)
        {
            comboBox1.Items.Clear();
            //pictureBox1.CreateGraphics().Clear(Color.Black);
            comboBox1.SelectedIndex = -1;
            elemPreview1.reset();
            schemePicture1.SetImage((Elems)null);
            // Show menu only if the right mouse button is clicked.
            if (e.Button == MouseButtons.Right)
            {
                // Point where the mouse is clicked.
                Point p = new Point(e.X, e.Y);

                // Get the node that the user has clicked.
                TreeNode node = treeView1.GetNodeAt(p);
                if (node != null)
                {
                    treeView1.SelectedNode = node;
                    if (node.Nodes.Count == 0)
                    {
                        List<Elems> lst = db.getElements(node.FullPath.Split('/'));
                        int timgid = lst[0].image_id;
                        bool eql = true;
                        foreach (Elems temp in lst)
                        {
                            eql = eql && temp.image_id == timgid;
                            comboBox1.Items.Add(temp.toString());
                        }
                        добавитьВИзбранноеToolStripMenuItem.Enabled = eql;
                        if (eql)
                            //cpb = new CategPictureBox(lst[0]);
                            fav.SetPretendent(lst[0]); //претендент
                        cms_el_gr.Show(treeView1, p); 
                    }
                }
                return;
            }
            // Load elements from database for category
            if (e.Button == MouseButtons.Left)
            {
                // Point where the mouse is clicked.
                Point p = new Point(e.X, e.Y);

                // Get the node that the user has clicked.
                TreeNode node = treeView1.GetNodeAt(p);
                if (node != null && node.Nodes.Count == 0)
                {
                    treeView1.SelectedNode = node;
                    //MessageBox.Show(db.getEtname(node.FullPath.Split('/')));
                    List<Elems> lst = db.getElements(node.FullPath.Split('/'));
                    foreach (Elems temp in lst)
                        comboBox1.Items.Add(temp.toString());
                    //cms_el_gr.Show(treeView1, p);
                }
                return;
            }
        }

        private void treeView1_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            MessageBox.Show(e.Label);
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            //Pen pen = new Pen(Color.Red, 3);
            //e.Graphics.DrawRectangle(pen, 3, 3, 10, 30);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //MessageBox.Show(comboBox1.SelectedIndex.ToString());
            //MessageBox.Show(db._elems[comboBox1.SelectedIndex].image_id.ToString());
            //Тут рисуем картинкку на панели, при переключении перезагрузка
            //_images.at(db._elems[comboBox1.SelectedIndex].image_id).Paint(new Pen(Color.White), pictureBox1.CreateGraphics());
            //_images.Paint(new Pen(Color.White), pictureBox1.CreateGraphics(), db._elems[comboBox1.SelectedIndex].image_id);
            //Graphics gr = pictureBox1.CreateGraphics();
            //db._elems[comboBox1.SelectedIndex].Paint(new Pen(Color.White), gr, pictureBox1.Width / 2, pictureBox1.Height / 2);
            this.elemPreview1.setElement(db._elems[comboBox1.SelectedIndex]);
            schemePicture1.SetImage(db._elems[comboBox1.SelectedIndex]);
            //MessageBox.Show("x="+pictureBox1.Width.ToString());
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            //Повернуть по часовой
            if (comboBox1.SelectedIndex == -1)
                return;
            //FIXME//db._elems[comboBox1.SelectedIndex].image.RightRotate();
            //pictureBox1.CreateGraphics().Clear(Color.Black);
            //db._elems[comboBox1.SelectedIndex].Paint(new Pen(Color.White), pictureBox1.CreateGraphics(), pictureBox1.Width / 2, pictureBox1.Height / 2);
        }

        private void tabPage1_Resize(object sender, EventArgs e)
        {
            //schemePicture1.MinimumSize = ((TabPage)sender).Size;
        }

        private void добавитьВИзбранноеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //добавление претендента
            toolStrip2.Items.Add(fav.AddPretendent());
            //toolStrip2.Items.Add(cpb.getImage());
            //toolStrip2.Items[toolStrip2.Items.Count - 1].Click += new EventHandler(toolStrip2_Click);
            toolStrip2.Items[toolStrip2.Items.Count - 1].MouseDown += new MouseEventHandler(toolStrip2_MouseDown);
        }

        private void добавитьЭлементToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            AddForm af = new AddForm(db.getElements(treeView1.SelectedNode.FullPath.Split('/'))[0].image_id, db.getEtname(treeView1.SelectedNode.FullPath.Split('/')));
            af.ShowDialog(this);
        }

        private void toolStrip2_MouseDown(object sender, MouseEventArgs e)
        {
            fav.CurrentIndex = toolStrip2.Items.IndexOf((ToolStripItem)sender);
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (((ToolStripItem)sender).BackColor == System.Drawing.SystemColors.Control)
                {
                    schemePicture1.SetImage(fav.get().elem);
                    ((ToolStripItem)sender).BackColor = System.Drawing.SystemColors.ControlDark;
                }
                else
                {
                    ((ToolStripItem)sender).BackColor = System.Drawing.SystemColors.Control;
                    schemePicture1.SetImage((Elems)null);
                }
            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                contextMenuStrip1.Show(toolStrip2, e.Location);
            }
        }

        private void удалитьToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            toolStrip2.Items.RemoveAt(fav.CurrentIndex);
            fav.Remove();
        }
    }
}
