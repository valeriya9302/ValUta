using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace MainWindow.scheme
{
    class Node : PictureBox
    {
        private Color color;

        public Node(Point location, object parent)
        {
            Location = new Point(location.X - 2, location.Y - 2);
            Parent = (Control)parent;
            color = Color.Black;
            Width = 5;
            Height = 5;
            MouseDown += new MouseEventHandler(EventMouseDown);
            MouseEnter += new EventHandler(EventMouseEnter);
            MouseLeave += new EventHandler(EventMouseLeave);
            repaint();
        }

        public void repaint()
        {
            GraphicsPath gp = new GraphicsPath();

            Bitmap flag = new Bitmap(Width, Height);
            this.Image = flag;
            Graphics gfx = Graphics.FromImage(this.Image);
            gfx.Clear(color);

            gp.AddEllipse(0, 0, 5, 5);
            Region = new Region(gp);
            Refresh();
        }

        public void EventMouseDown(object sender, MouseEventArgs e)
        {
            Point tloc = ((SchemePicture)Parent).PointToMask(new Point(e.X + Location.X, e.Y + Location.Y));
            Point loc;
            if (tloc.X - 2 == Location.X && tloc.Y - 2 == Location.Y)
                loc = tloc;
            else if (Math.Abs((2 + Location.Y - tloc.X)) < Math.Abs(2 + Location.Y - tloc.Y))
                loc = new Point(2 + Location.X, tloc.Y);
            else
                loc = new Point(tloc.X, 2 + Location.Y);
            //((SchemePicture)Parent).EventMouseDown(sender, new MouseEventArgs(e.Button, e.Clicks, e.X + Location.X, e.Y + Location.Y, e.Delta));
            ((SchemePicture)Parent).EventMouseDown(sender, new MouseEventArgs(e.Button, e.Clicks, loc.X, loc.Y, e.Delta));
        }

        public void EventMouseLeave(object sender, EventArgs e)
        {
            color = Color.Black;
            repaint();
        }

        public void EventMouseEnter(object sender, EventArgs e)
        {
            color = Color.MediumVioletRed;
            repaint();
        }
    }
}
