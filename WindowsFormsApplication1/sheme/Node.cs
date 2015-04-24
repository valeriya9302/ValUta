using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace MainWindow.sheme
{
    class Node : PictureBox
    {
        public Node(Point location, object parent)
        {
            Location = new Point(location.X - 2, location.Y - 2);
            Parent = (Control)parent;
            Width = 5;
            Height = 5;
            repaint();
        }

        public void repaint()
        {
            GraphicsPath gp = new GraphicsPath();

            Bitmap flag = new Bitmap(Width, Height);
            this.Image = flag;
            Graphics gfx = Graphics.FromImage(this.Image);
            gfx.Clear(Color.Black);

            gp.AddEllipse(0, 0, 5, 5);
            Region = new Region(gp);
            Refresh();
        }
    }
}
