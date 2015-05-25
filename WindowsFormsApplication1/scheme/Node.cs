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
        private List<Wire> lw;
        private Point ptm;

        public Node(Point location, object parent)
        {
            Parent = (Control)parent;
            color = Color.Black;
            Width = 5;
            Height = 5;
            MouseDown += new MouseEventHandler(EventMouseDown);
            MouseEnter += new EventHandler(EventMouseEnter);
            MouseLeave += new EventHandler(EventMouseLeave);
            //repaint();
            Refresh();
            lw = new List<Wire>();

            Size = new Size(2 * ptm.X, 2 * ptm.Y);
            Location = new Point(location.X - (int)(Size.Width / 2.0F), location.Y - (int)(Size.Height / 2.0F));
        }

        //public void repaint()
        protected override void OnPaint(PaintEventArgs pe)
        {
            /*GraphicsPath gp = new GraphicsPath();

            Bitmap flag = new Bitmap(Width, Height);
            this.Image = flag;
            Graphics gfx = Graphics.FromImage(this.Image);
            gfx.Clear(color);

            gp.AddEllipse(0, 0, 5, 5);
            Region = new Region(gp);
            Refresh();*/

            GraphicsPath gp = new GraphicsPath();

            if (SchemePicture.useGetPixelSizePerMM == 1)
            {
                SizeF tptm = SchemePicture.GetPixelSizePerMM();
                ptm = new Point((int)tptm.Width, (int)tptm.Height);
            }
            else
                ptm = new Point((int)(pe.Graphics.DpiX / 25.4F), (int)(pe.Graphics.DpiY / 25.4F));

            gp.AddEllipse(0, 0, Size.Width, Size.Height);
            Region = new Region(gp);

            pe.Graphics.Clear(color);
        }

        public void EventMouseDown(object sender, MouseEventArgs e)
        {
            Point tloc = ((SchemePicture)Parent).PointToMask(new Point(e.X + Location.X, e.Y + Location.Y));
            Point loc;
            if (tloc.X - (int)(Size.Width / 2.0F) == Location.X && tloc.Y - (int)(Size.Height / 2.0F) == Location.Y)
                loc = tloc;
            else if (Math.Abs(((int)(Size.Width / 2.0F) + Location.X - tloc.X)) < Math.Abs((int)(Size.Height / 2.0F) + Location.Y - tloc.Y))
                loc = new Point((int)(Size.Width / 2.0F) + Location.X, tloc.Y);
            else
                loc = new Point(tloc.X, (int)(Size.Height / 2.0F) + Location.Y);
            //((SchemePicture)Parent).EventMouseDown(sender, new MouseEventArgs(e.Button, e.Clicks, e.X + Location.X, e.Y + Location.Y, e.Delta));
            ((SchemePicture)Parent).EventMouseDown(sender, new MouseEventArgs(e.Button, e.Clicks, loc.X, loc.Y, e.Delta));
        }

        public void EventMouseLeave(object sender, EventArgs e)
        {
            color = Color.Black;
            //repaint();
            Refresh();
        }

        public void EventMouseEnter(object sender, EventArgs e)
        {
            color = Color.MediumVioletRed;
            //repaint();
            Refresh();
        }

        public void addWire(Wire w)
        {
            lw.Add(w);
        }

        public void BeginDispose()
        {
            List<Wire> tlw = new List<Wire>();
            foreach (Wire w in lw)
            {
                if (!w.IsDisposed && !w.Disposing)
                    tlw.Add(w);
            }
            if (tlw.Count == 2)
            {
                for (int i = tlw[1].pl.Count - 2; i >= 0; i--)
                    tlw[0].addPoint(tlw[1].pl[i]);
                tlw[0].eel = tlw[1].sel;
                if (object.ReferenceEquals(tlw[0].eel.GetType(), typeof(ElemPictureBox)))
                    ((ElemPictureBox)tlw[0].eel).addWire(tlw[0]);
                if (object.ReferenceEquals(tlw[0].eel.GetType(), typeof(Node)))
                    ((Node)tlw[0].eel).addWire(tlw[0]);

                tlw[1].eel = null;
                tlw[1].Dispose();
                this.Dispose();
            }
        }
    }
}
