using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace MainWindow.scheme
{
    class Text : PictureBox
    {
        private string str;
        private int theight;
        public string Str
        {
            set
            {
                Point loc = new Point(Location.X, Location.Y + Height);
                str = value;
                Width = 0;
                setLocation(loc);
            }
            get
            {
                return str;
            }
        }
        private Color color;
        private Point oldPos;

        public Text(string str)
        {
            //Parent = (Control)parent;
            this.str = str;
            Size = new Size(0, 0);
            color = Color.Black;
            //Location = new Point(50, 50);
            //repaint();
            MouseEnter += new EventHandler(EventMouseEnter);
            MouseLeave += new EventHandler(EventMouseLeave);
            MouseDown += new MouseEventHandler(EventMouseDown);
            MouseMove += new MouseEventHandler(EventMouseMove);
        }

        public void setLocation(Point location)
        {
            if (Width == 0)
            {
                Bitmap flag = new Bitmap(100, 50);
                this.Image = flag;
                Graphics gfx = Graphics.FromImage(this.Image);
                Size = gfx.MeasureString(str, new Font("Arial", 7)).ToSize();
            }
            Location = new Point(location.X, location.Y - Height);
            repaint();
        }

        public void offsetLocation(Point dp)
        {
            //Location.Offset(dp);
            Location = new Point(Location.X + dp.X, Location.Y + dp.Y);
            repaint();
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
            //SizeF size = gfx.MeasureString("123", new Font("Arial", 7));
            //Height = (int)size.Height;
            //Width = (int)size.Width;
            //Location = new Point(Location.X - (int)(size.Width / 2), Location.Y - (int)size.Height - 16);
            gfx.DrawString(str, new Font("Arial", 7), new SolidBrush(color), new PointF(0, 0));
            //BringToFront();
        }

        public void EventMouseEnter(object sender, EventArgs e)
        {
            color = Color.MediumVioletRed;
            repaint();
        }

        public void EventMouseLeave(object sender, EventArgs e)
        {
            color = Color.Black;
            repaint();
        }

        public void EventMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
                oldPos = new Point(e.X, e.Y);
        }

        public void EventMouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                setLocation(new Point(Location.X - oldPos.X + e.X, Location.Y - oldPos.Y + e.Y));
                return;
            }
        }
    }
}
