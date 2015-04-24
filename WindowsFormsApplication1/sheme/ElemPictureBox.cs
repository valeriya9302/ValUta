using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
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
        private bool sendParent = false;

        public ElemPictureBox(Elems el)
        {
            elem = el;
            MouseDown += new MouseEventHandler(EventMouseDown);
            MouseMove += new MouseEventHandler(EventMouseMove);
            MouseClick += new MouseEventHandler(EventMouseClick);
            MouseEnter += new EventHandler(EventMouseEnter);
            MouseLeave += new EventHandler(EventMouseLeave);
            BackColor = Color.DeepPink;
            pen = new Pen(Color.Black);
            Height = elem.image.Height + 2;
            Width = elem.image.Width;
            /*foreach (Image.Join join in elem.image.joins)
            {
                Controls.Add(join);
                join.Parent = this;
                join.BackColor = Color.Red;
                join.BringToFront();
            }*/
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
            //newReg - Задать регион объекта, тогда все будет заебись
            // Сначала требуется продумать рисование фигуры

            //Region = new Region(elem.image.getRegion().GetRegionData());

            Bitmap flag = new Bitmap(Width, Height);
            this.Image = flag;
            Graphics gfx = Graphics.FromImage(this.Image);
            gfx.Clear(Color.Azure);
            elem.Paint(pen, gfx);
            //BringToFront();
        }

        public void EventMouseDown(object sender, MouseEventArgs e)
        {
            if (sendParent)
            {
                MouseEventArgs ne = new MouseEventArgs(e.Button, e.Clicks, oldPos.X + Location.X, oldPos.Y + Location.Y, e.Delta);
                ((SchemePicture)Parent).setCursor(2);
                ((SchemePicture)Parent).EventMouseDown(sender, ne);
                return;
            }
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
                oldPos = new Point(e.X, e.Y);
        }

        public void EventMouseMove(object sender, MouseEventArgs e)
        {
            if (sendParent)
                return;
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                setLocation(new Point(
                    (int)((Location.X - oldPos.X + e.X) / ((SchemePicture)(Parent)).maskSize) * ((SchemePicture)(Parent)).maskSize,
                    (int)((Location.Y - oldPos.Y + e.Y) / ((SchemePicture)(Parent)).maskSize) * ((SchemePicture)(Parent)).maskSize - elem.image.Height / 2));
                Parent.Refresh();
                return;
            }
            else
            {
                /*foreach (Image.Join join in elem.image.joins)
                    if (Math.Abs(join.x - e.X) < 3 && Math.Abs(join.y - e.Y) < 3)
                        MessageBox.Show("321");*/
                foreach (Image.Join join in elem.image.joins)
                {
                    if (Math.Abs(join.p.X - e.X) < 3.0F && Math.Abs(join.p.Y - e.Y) < 3.0F)
                    {
                        Graphics gfx = Graphics.FromImage(this.Image);
                        join.tPaint(new Pen(Color.Red), gfx);
                        oldPos = new Point((int)join.p.X, (int)join.p.Y);
                        sendParent = true;
                    }
                }
            }
        }

        public void EventMouseClick(object sender, MouseEventArgs e)
        {
            //MessageBox.Show("trololo");
            //Console.WriteLine(e.X);
            //set current
        }

        public void EventMouseEnter(object sender, EventArgs e)
        {
            pen.Color = Color.MediumVioletRed;
            repaint();
        }

        public void EventMouseLeave(object sender, EventArgs e)
        {
            pen.Color = Color.Black;
            sendParent = false;
            repaint();
        }
    }
}
