using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace MainWindow.scheme
{
    /** 
     * Класс для рисования проводов на схеме
     **/
    class Wire : PictureBox
    {
        private List<Point> pl;
        public bool isDone;
        private Color color;
        public Wire(Point startPoint, object parent)
        {
            pl = new List<Point>();
            pl.Add(startPoint);
            pl.Add(startPoint);
            Location = new Point(0, 0);
            MouseMove += new MouseEventHandler(EventMouseMove);
            MouseDown += new MouseEventHandler(EventMouseDown);
            Parent = (Control)parent;
            Height = Parent.Height;
            Width = Parent.Width;
            Padding = new System.Windows.Forms.Padding(0);
            Margin = new System.Windows.Forms.Padding(0);
            color = Color.Black;
            updateRegion();
            repaint();
            isDone = false;
        }

        public Wire(List<Point> points, object parent)
        {
            pl = new List<Point>(points.AsEnumerable());
            Location = new Point(0, 0);
            MouseMove += new MouseEventHandler(EventMouseMove);
            MouseDown += new MouseEventHandler(EventMouseDown);
            Parent = (Control)parent;
            Height = Parent.Height;
            Width = Parent.Width;
            color = Color.Black;
            updateRegion();
            repaint();
            setDone();
        }


        public void setDone()
        {
            isDone = true;
            MouseEnter += new EventHandler(EventMouseEnter);
            MouseLeave += new EventHandler(EventMouseLeave);
            MouseClick += new MouseEventHandler(EventMouseClick);
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

        public void addPoint(Point point)
        {
            pl.Add(new Point(point.X, point.Y));
            updateRegion();
            repaint();
        }

        public Point replacePoint(Point point)
        {
            //pl[pl.Count - 1] = new Point(point.X - Location.X, point.Y - Location.Y);
            //pl[pl.Count - 1] = point;
            Point tp;
            if (Math.Abs((point.X - pl[pl.Count - 2].X)) < Math.Abs(point.Y - pl[pl.Count - 2].Y))
                tp = new Point(pl[pl.Count - 2].X, point.Y);
            else
                tp = new Point(point.X, pl[pl.Count - 2].Y);
            pl[pl.Count - 1] = tp;
            updateRegion();
            repaint();
            return tp;
        }

        public bool removeLast()
        {
            if (pl.Count == 2)
                return false;
            pl.RemoveAt(pl.Count - 1);
            return true;
        }

        public void repaint()
        {
            Bitmap flag = new Bitmap(Width, Height);
            this.Image = flag;
            Graphics gfx = Graphics.FromImage(this.Image);
            gfx.Clear(color);
            //gfx.DrawLines(new Pen(Color.Black), pl.ToArray());

            Refresh();
        }

        private void updateRegion()
        {
            GraphicsPath gp = new GraphicsPath();

            List<Point> rect = new List<Point>();
            for (int i = 1; i < pl.Count; i++)
            {
                int minX = Math.Min(pl[i - 1].X, pl[i].X);
                int maxX = Math.Max(pl[i - 1].X, pl[i].X);
                int minY = Math.Min(pl[i - 1].Y, pl[i].Y);
                int maxY = Math.Max(pl[i - 1].Y, pl[i].Y);
                int dx = (minX == maxX) ? 1 : 0;
                int dy = (minY == maxY) ? 1 : 0;

                rect.Add(new Point(minX + dx, minY));
                rect.Add(new Point(maxX + dx, maxY));
                rect.Add(new Point(maxX, maxY + dy));
                rect.Add(new Point(minX, minY + dy));
                //if (dx == 0)
                //    break;
                //Math method
                /*double a = Math.Atan2(maxY - minY, maxX - minX);
                rect.Add(new Point((int)(minX * Math.Cos(a) + minY * Math.Sin(a)), (int)(-minX * Math.Sin(a) + minY * Math.Cos(a) - dx)));
                rect.Add(new Point((int)(maxX * Math.Cos(a) + maxY * Math.Sin(a)), (int)(-maxX * Math.Sin(a) + maxY * Math.Cos(a) - dx)));
                rect.Add(new Point((int)(maxX * Math.Cos(a) + maxY * Math.Sin(a)), (int)(-maxX * Math.Sin(a) + maxY * Math.Cos(a) + dy)));
                rect.Add(new Point((int)(minX * Math.Cos(a) + minY * Math.Sin(a)), (int)(-minX * Math.Sin(a) + minY * Math.Cos(a) + dy)));

                for (int j = 0; j < rect.Count; j++)
                {
                    rect[j] = new Point((int)(rect[j].X * Math.Cos(-a) + rect[j].Y * Math.Sin(-a)), (int)(-rect[j].X * Math.Sin(-a) + rect[j].Y * Math.Cos(-a)));
                }*/
                //gfx.DrawLines(new Pen(Color.Red), rect.ToArray());
                //gfx.DrawPolygon(new Pen(Color.Red), rect.ToArray());
                gp.AddPolygon(rect.ToArray());
                rect.Clear();
            }
            Region = new Region(gp);
        }

        public List<Wire> split(Point point)
        {
            List<Wire> list = new List<Wire>();
            List<Point> list1g = new List<Point>();
            List<Point> list2g = new List<Point>();
            //add points to arrays
            int i;
            list1g.Add(pl[0]);
            for(i = 1; i < pl.Count; i++)
            {
                if (pl[i - 1].X == pl[i].X && pl[i].X == point.X)
                {
                    int maxy = (int)Math.Max(pl[i - 1].Y, pl[i].Y);
                    int miny = (int)Math.Min(pl[i - 1].Y, pl[i].Y);
                    if (point.Y >= miny && point.Y <= maxy)
                        break;
                }
                else if (pl[i - 1].Y == pl[i].Y && pl[i].Y == point.Y)
                {
                    int maxx = (int)Math.Max(pl[i - 1].X, pl[i].X);
                    int minx = (int)Math.Min(pl[i - 1].X, pl[i].X);
                    if (point.X >= minx && point.X <= maxx)
                        break;
                }
                list1g.Add(pl[i]);
            }
            if (i == pl.Count)
                return null;
            list1g.Add(point);
            list2g.Add(point);
            for (; i < pl.Count; i++)
                list2g.Add(pl[i]);

            list2g.Reverse();
            list.Add(new Wire(list1g, Parent));
            list.Add(new Wire(list2g, Parent));
            return list;
        }

        public void EventMouseMove(object sender, MouseEventArgs e)
        {
            if (!isDone)
                ((SchemePicture)Parent).EventMouseMove(sender, e);
        }
        public void EventMouseDown(object sender, MouseEventArgs e)
        {
            MessageBox.Show("down");
            if (!isDone)
                ((SchemePicture)Parent).EventMouseDown(sender, e);
            else
                ((SchemePicture)Parent).EventMouseDown(sender, e);
        }

        public void EventMouseClick(object sender, MouseEventArgs e)
        {
        }
    }
}
