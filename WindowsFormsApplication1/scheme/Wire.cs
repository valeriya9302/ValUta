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
    public class Wire : PictureBox
    {
        public List<Point> pl { private set; get; }
        public bool isDone;
        private Color color;
        private PointF ptm { set; get; }
        public object sel, eel;
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
            //repaint();
            Refresh();
            isDone = false;
            //Visible = false;
        }

        public Wire(List<Point> points, object parent)
        {
            //Visible = false;
            pl = new List<Point>(points.AsEnumerable());
            Location = new Point(0, 0);
            MouseMove += new MouseEventHandler(EventMouseMove);
            MouseDown += new MouseEventHandler(EventMouseDown);
            Parent = (Control)parent;
            Height = Parent.Height;
            Width = Parent.Width;
            color = Color.Black;
            updateRegion();
            //repaint();
            Refresh();
            setDone();
        }

        public void setDone()
        {
            isDone = true;
            MouseEnter += new EventHandler(EventMouseEnter);
            MouseLeave += new EventHandler(EventMouseLeave);
            MouseClick += new MouseEventHandler(EventMouseClick);
            Disposed += new EventHandler(EventDisposed);
        }

        void EventDisposed(object sender, EventArgs e)
        {
            if (sel != null && object.ReferenceEquals(sel.GetType(), typeof(Node)))
                ((Node)sel).BeginDispose();
            if (eel != null && object.ReferenceEquals(eel.GetType(), typeof(Node)))
                ((Node)eel).BeginDispose();
            MouseEnter -= EventMouseEnter;
            MouseLeave -= EventMouseLeave;
            MouseClick -= EventMouseClick;
            Disposed -= EventDisposed;
            MouseMove -= EventMouseMove;
            MouseDown -= EventMouseDown;
        }

        public void EventMouseEnter(object sender, EventArgs e)
        {
            color = Color.MediumVioletRed;
            //repaint();
            Refresh();
        }

        public void EventMouseLeave(object sender, EventArgs e)
        {
            color = Color.Black;
            //repaint();
            Refresh();
        }

        public void addPoint(Point point)
        {
            pl.Add(new Point(point.X, point.Y));
            updateRegion();
            //repaint();
            Refresh();
        }

        public Point r2Point(Point point)
        {
            if (point.X == pl[pl.Count - 2].X || point.Y == pl[pl.Count - 2].Y)
                pl[pl.Count - 1] = new Point(point.X, point.Y);
            else if (pl.Count == 2)
            {
                if (Math.Abs((point.X - pl[pl.Count - 2].X)) > Math.Abs(point.Y - pl[pl.Count - 2].Y))
                {
                    //pl[pl.Count - 2] = new Point(point.X, pl[pl.Count - 2].Y);
                    pl[pl.Count - 1] = new Point(point.X, pl[pl.Count - 2].Y);
                }
                else
                {
                    //pl[pl.Count - 2] = new Point(pl[pl.Count - 2].X, point.Y);
                    pl[pl.Count - 1] = new Point(pl[pl.Count - 2].X, point.Y);
                }
            }
            /*else if (((SchemePicture)Parent).PointToMask(pl[pl.Count - 2]) != pl[pl.Count - 2])
            {
                Point p = ((SchemePicture)Parent).PointToMask(pl[pl.Count - 2]);
                if (Math.Abs((p.X - pl[pl.Count - 2].X)) < Math.Abs(p.Y - pl[pl.Count - 2].Y))
                {
                    //pl[pl.Count - 2] = new Point(point.X, pl[pl.Count - 2].Y);
                    pl[pl.Count - 1] = new Point(point.X, pl[pl.Count - 2].Y);
                }
                else
                {
                    //pl[pl.Count - 2] = new Point(pl[pl.Count - 2].X, point.Y);
                    pl[pl.Count - 1] = new Point(pl[pl.Count - 2].X, point.Y);
                }
            }*/
            else if (Math.Abs((point.X - pl[pl.Count - 2].X)) < Math.Abs(point.Y - pl[pl.Count - 2].Y))
            {
                pl[pl.Count - 2] = new Point(point.X, pl[pl.Count - 2].Y);
                pl[pl.Count - 1] = new Point(point.X, point.Y);
            }
            else
            {
                pl[pl.Count - 2] = new Point(pl[pl.Count - 2].X, point.Y);
                pl[pl.Count - 1] = new Point(point.X, point.Y);
            }
            updateRegion();
            //repaint();
            Refresh();
            return pl[pl.Count - 1];
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
            //repaint();
            Refresh();
            return tp;
        }

        public bool removeLast()
        {
            if (pl.Count == 2)
                return false;
            pl.RemoveAt(pl.Count - 1);
            return true;
        }

        //public void repaint()
        protected override void OnPaint(PaintEventArgs pe)
        {
            /*Bitmap flag = new Bitmap(Width, Height);
            this.Image = flag;
            Graphics gfx = Graphics.FromImage(this.Image);
            ptm = new PointF(pe.Graphics.DpiX / 25.4F, pe.Graphics.DpiY / 25.4F);
            gfx.Clear(color);*/
            //gfx.DrawLines(new Pen(Color.Black), pl.ToArray());

            //Refresh();
            if (SchemePicture.useGetPixelSizePerMM == 1)
            {
                SizeF tptm = SchemePicture.GetPixelSizePerMM();
                ptm = new PointF(tptm.Width, tptm.Height);
            }
            else
                ptm = new PointF(pe.Graphics.DpiX / 25.4F, pe.Graphics.DpiY / 25.4F);
            pe.Graphics.Clear(Color.MintCream);
            pe.Graphics.DrawLines(new Pen(color, (int)(Math.Round(ptm.X / 2.0F))), pl.ToArray());
            //pe.Graphics.Clear(color);
        }

        private void updateRegion()
        {
            Refresh();
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

                dx = (int)(Math.Round(dx * ptm.X / 2.0F));
                dy = (int)(Math.Round(dy * ptm.Y / 2.0F));

                rect.Add(new Point(minX + dx, minY - dy));
                rect.Add(new Point(maxX + dx, maxY - dy));
                rect.Add(new Point(maxX - dx, maxY + dy));
                rect.Add(new Point(minX - dx, minY + dy));
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
            Visible = true;
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
            if (sel != null)
            {
                if (object.ReferenceEquals(sel.GetType(), typeof(ElemPictureBox)))
                    ((ElemPictureBox)sel).addWire(list[0]);
                if (object.ReferenceEquals(sel.GetType(), typeof(Node)))
                    ((Node)sel).addWire(list[0]);
                list[0].sel = sel;
            }
            list.Add(new Wire(list2g, Parent));
            if (eel != null)
            {
                if (object.ReferenceEquals(eel.GetType(), typeof(ElemPictureBox)))
                    ((ElemPictureBox)eel).addWire(list[1]);
                if (object.ReferenceEquals(eel.GetType(), typeof(Node)))
                    ((Node)eel).addWire(list[1]);
                list[1].sel = eel;
            }
            return list;
        }

        public void EventMouseMove(object sender, MouseEventArgs e)
        {
            if (!isDone)
                ((SchemePicture)Parent).EventMouseMove(sender, e);
        }
        public void EventMouseDown(object sender, MouseEventArgs e)
        {
            //MessageBox.Show("down");
            if (!isDone)
                ((SchemePicture)Parent).EventMouseDown(sender, e);
            else if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                Point minOfset = new Point(int.MaxValue, int.MaxValue);
                for (int i = 1; i < pl.Count; i++)
                {
                    if (pl[i - 1].X == pl[i].X)
                        if ((pl[i - 1].Y >= e.Y && pl[i].Y <= e.Y) || (pl[i - 1].Y <= e.Y && pl[i].Y >= e.Y))
                            if (Math.Abs(minOfset.X) > Math.Abs(pl[i].X - e.X))
                                minOfset.X = pl[i].X - e.X;
                    if (pl[i - 1].Y == pl[i].Y)
                        if ((pl[i - 1].X >= e.X && pl[i].X <= e.X) || (pl[i - 1].X <= e.X && pl[i].X >= e.X))
                            if (Math.Abs(minOfset.Y) > Math.Abs(pl[i].Y - e.Y))
                                minOfset.Y = pl[i].Y - e.Y;
                }
                Point te;
                if (Math.Abs(minOfset.X) > Math.Abs(minOfset.Y))
                    te = new Point(e.X, e.Y + minOfset.Y);
                else
                    te = new Point(e.X + minOfset.X, e.Y);

                Point tloc = ((SchemePicture)Parent).PointToMask(te);
                Point loc = new Point();
                for (int i = 1; i < pl.Count; i++)
                {
                    if ((pl[i - 1].X == pl[i].X && pl[i].X == tloc.X) || (pl[i - 1].Y == pl[i].Y && pl[i].Y == tloc.Y))
                    {
                        loc = tloc;
                        break;
                    }
                }
                if (loc != tloc)
                {
                    if (Math.Abs((te.X - tloc.X)) > Math.Abs(te.Y - tloc.Y))
                        loc = new Point(te.X, tloc.Y);
                    else
                        loc = new Point(tloc.X, te.Y);
                }
                ((SchemePicture)Parent).EventMouseDown(sender, new MouseEventArgs(e.Button, e.Clicks, loc.X, loc.Y, e.Delta));
            }
            else
            {
                ((SchemePicture)Parent).EventMouseDown(sender, e);
            }
        }

        public void EventMouseClick(object sender, MouseEventArgs e)
        {
        }
    }
}
