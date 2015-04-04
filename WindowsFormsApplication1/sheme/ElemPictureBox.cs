using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace MainWindow.sheme
{
    /**
     * Класс для рисования одного элемента на схеме
     **/
    class ElemPictureBox : PictureBox
    {
        private Elems elem;
        private Point oldPos;

        public ElemPictureBox(Elems el)
        {
            elem = el;
            //MouseDown += new MouseEventHandler(EventMouseDown);
            //MouseMove += new MouseEventHandler(EventMouseMove);
            MouseClick += new MouseEventHandler(EventMouseClick);
            BackColor = Color.DeepPink;
        }

        public void setLocation(Point pos)
        {
            Location = pos;
            //Refresh();
            repaint();
            Refresh();
        }

        public void repaint()
        {
            Bitmap flag = new Bitmap(100,100);
            this.Image = flag;
            Graphics gfx = Graphics.FromImage(this.Image);
            gfx.Clear(Color.DeepPink);
            elem.Paint(new Pen(Color.Black), gfx,20,20);
            //BringToFront();
        }

        public void EventMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
                oldPos = new Point(e.X, e.Y);
        }

        public void EventMouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
                setLocation(e.Location);
        }

        public void EventMouseClick(object sender, MouseEventArgs e)
        {
            //MessageBox.Show("trololo");
            Console.WriteLine(e.X);
        }
    }
}
