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
        private Pen pen;

        public ElemPictureBox(Elems el)
        {
            elem = el;
            MouseDown += new MouseEventHandler(EventMouseDown);
            MouseMove += new MouseEventHandler(EventMouseMove);
            //MouseClick += new MouseEventHandler(EventMouseClick);
            MouseEnter += new EventHandler(EventMouseEnter);
            MouseLeave += new EventHandler(EventMouseLeave);
            BackColor = Color.DeepPink;
            pen = new Pen(Color.Black);
            Height = elem.image.Height;
            Width = elem.image.Width;
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
            Bitmap flag = new Bitmap(Width, Height);
            this.Image = flag;
            Graphics gfx = Graphics.FromImage(this.Image);
            gfx.Clear(Color.White);
            elem.Paint(pen, gfx, Width / 2, Height / 2);
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
                setLocation(new Point(Location.X - oldPos.X + e.X, Location.Y - oldPos.Y + e.Y));
        }

        public void EventMouseClick(object sender, MouseEventArgs e)
        {
            //MessageBox.Show("trololo");
            Console.WriteLine(e.X);
        }

        public void EventMouseEnter(object sender, EventArgs e)
        {
            pen.Color = Color.MediumVioletRed;
            repaint();
        }

        public void EventMouseLeave(object sender, EventArgs e)
        {
            pen.Color = Color.Black;
            repaint();
        }
    }
}
