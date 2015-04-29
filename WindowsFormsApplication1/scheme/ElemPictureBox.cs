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
        private List<Text> text;
        private bool sendParent = false;
        private bool isDone;
        public bool isMouseEnter;
        private string name;

        public ElemPictureBox(Elems el, object parent)
        {
            elem = el;
            Parent = (Control)parent;
            MouseDown += new MouseEventHandler(EventMouseDown);
            MouseMove += new MouseEventHandler(EventMouseMove);
            //MouseMove += new MouseEventHandler(((SchemePicture)Parent).EventMouseMove);
            //MouseClick += new MouseEventHandler(EventMouseClick);
            //MouseEnter += new EventHandler(EventMouseEnter);
            //MouseEnter += new EventHandler(((SchemePicture)Parent).EventMouseEnter);
            //MouseLeave += new EventHandler(EventMouseLeave);
            Disposed += new EventHandler(EventDisposed);
            BackColor = Color.DeepPink;
            pen = new Pen(Color.Black);
            Height = elem.image.Height + 2;
            Width = elem.image.Width;
            isDone = false;
            sendParent = false;
            text = new List<Text>();
            name = elem.prefix;
            /*foreach (Image.Join join in elem.image.joins)
            {
                Controls.Add(join);
                join.Parent = this;
                join.BackColor = Color.Red;
                join.BringToFront();
            }*/
        }

        public void EventDisposed(object sender, EventArgs e)
        {
            //MouseDown -= MouseDown;
            //MessageBox.Show("Disposed!");
            MouseDown -= EventMouseDown;
            MouseMove -= EventMouseMove;
            Disposed -= EventDisposed;
            MouseClick -= EventMouseClick;
            MouseEnter -= EventMouseEnter;
            MouseLeave -= EventMouseLeave;
            text.Clear();
        }

        public void setLocation(Point pos)
        {
            Location = pos;
            //Refresh();
            repaint();
            Refresh();
        }

        public void setDone()
        {
            isDone = true;
            MouseClick += new MouseEventHandler(EventMouseClick);
            MouseEnter += new EventHandler(EventMouseEnter);
            MouseLeave += new EventHandler(EventMouseLeave);

            text.Add(new Text(elem.getTextParam()));
            text[0].Parent = Parent;
            text[0].setLocation(Location);
            text.Add(new Text(name));
            text[1].Parent = Parent;
            text[1].setLocation(text[0].Location);
            /*text[0].setLocation(new Point(Location.X + Width / 2, Location.Y));
            //text.Add(new sheme.Text(null, Parent, new Point(text[0].Location.X + Width / 2, Location.Y)));
            //text = new Text(null, Parent, new Point(Location.X + Width / 2, Location.Y));
            Parent.Controls.Add(text[0]);
            text[0].BringToFront();*/
            //text[0].Parent = Parent;
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
            //MessageBox.Show("this");
            if (!isDone)
            {
                ((SchemePicture)Parent).EventMouseDown(sender, new MouseEventArgs(e.Button, e.Clicks, Location.X + e.X, Location.Y + e.Y, e.Delta));
                return;
            }
            if (sendParent)
            {
                MouseEventArgs ne = new MouseEventArgs(e.Button, e.Clicks, oldPos.X + Location.X, oldPos.Y + Location.Y, e.Delta);
                ((SchemePicture)Parent).setCursor(2);
                ((SchemePicture)Parent).EventMouseDown(sender, ne);
                return;
            }
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
                oldPos = new Point(e.X, e.Y);
            //text[0].EventMouseDown(sender, e);
        }

        public void EventMouseMove(object sender, MouseEventArgs e)
        {
            if (!isDone)
            {
                ((SchemePicture)Parent).EventMouseMove(sender, new MouseEventArgs(e.Button, e.Clicks, Location.X + e.X, Location.Y + e.Y, e.Delta));
                return;
            }
            if (sendParent)
                return;
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                Point oldLocation = Location;
                setLocation(new Point(
                    (int)((Location.X - oldPos.X + e.X) / ((SchemePicture)(Parent)).maskSize) * ((SchemePicture)(Parent)).maskSize,
                    (int)((Location.Y - oldPos.Y + e.Y) / ((SchemePicture)(Parent)).maskSize) * ((SchemePicture)(Parent)).maskSize - elem.image.Height / 2));
                //text[0].EventMouseMove(sender, e);
                text[0].offsetLocation(new Point(Location.X - oldLocation.X, Location.Y - oldLocation.Y));
                text[1].offsetLocation(new Point(Location.X - oldLocation.X, Location.Y - oldLocation.Y));
                /*text[0].offsetLocation(new Point(
                    (int)((Location.X - oldPos.X + e.X) / ((SchemePicture)(Parent)).maskSize) * ((SchemePicture)(Parent)).maskSize,
                    (int)((Location.Y - oldPos.Y + e.Y) / ((SchemePicture)(Parent)).maskSize) * ((SchemePicture)(Parent)).maskSize - elem.image.Height / 2));*/
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
            isMouseEnter = true;
            if (!isDone)
                return;
            pen.Color = Color.MediumVioletRed;
            repaint();
        }

        public void EventMouseLeave(object sender, EventArgs e)
        {
            isMouseEnter = false;
            if (!isDone)
            {
                //((SchemePicture)Parent).EventMouseLeave(sender, e);
                return;
            }
            pen.Color = Color.Black;
            sendParent = false;
            repaint();
        }
    }
}
