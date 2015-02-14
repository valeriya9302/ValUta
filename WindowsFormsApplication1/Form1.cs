using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MainWindow
{
    public partial class Form1 : Form
    {
        private elements db = new elements();

        public Form1()
        {
            InitializeComponent();
            // Заполнение элементов с бд
            //elements db = new elements();
            db.load();
            List<int> ind = new List<int>();
            ind.Add(0);
            loadDataElems(treeView1.Nodes, ind);
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
            tabControl1.TabPages[new_last].Controls.Add(panel1);
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
            treeView1.SelectedNode = treeView1.SelectedNode.Nodes.Add("new group");
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
                    cms_el_gr.Show(treeView1, p);
                }
            }
        }

        private void treeView1_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            MessageBox.Show(e.Label);
        }
    }
}
