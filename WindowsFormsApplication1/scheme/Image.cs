using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Globalization;

namespace MainWindow
{
    public class Image
    {
        public Image(Image img)
        {
            id = img.id;
            ffs = img.ffs;

            arcs = new List<Arc>();
            foreach (Arc arc in img.arcs)
                arcs.Add(new Arc(arc));
            
            lines = new List<Line>();
            foreach (Line line in img.lines)
                lines.Add(new Line(line));

            arrows = new List<Arrow>();
            foreach (Arrow arrow in img.arrows)
                arrows.Add(new Arrow(arrow));
            
            strings = new List<Text>();
            foreach (Text text in img.strings)
                strings.Add(new Text(text));
            
            rectangles = new List<Rectangle>();
            foreach (Rectangle rect in img.rectangles)
                rectangles.Add(new Rectangle(rect));

            joins = new List<Join>();
            foreach (Join join in img.joins)
                joins.Add(new Join(join));

            border = new Rectangle(img.border);
            
            isLoad = img.isLoad;
            Height = img.Height;
            Width = img.Width;
            pointRotate = new PointF(img.pointRotate.X, img.pointRotate.Y);
            TopLeft = new Point(img.TopLeft.X, img.TopLeft.Y);

            RefreshRegion();
        }

        public void RefreshRegion()
        {
            GraphicsPath newReg = new GraphicsPath();
            /*foreach (Line line in lines)
            {
                newReg.AddLine(line.x1 + 10, line.y1 + 10, line.x2 + 10, line.y2 + 10);
                newReg.AddLine(line.x2 + 10, line.y2 + 10, line.x1 + 10, line.y1 + 10);
            }*/
            foreach (Rectangle rect in rectangles)
                newReg.AddRectangle(new System.Drawing.Rectangle(rect.x+10, rect.y+10, rect.width, rect.height));
            region = new Region(newReg);
        }

        public Image(int _id)
        {
            id = _id;
            load();
        }

        private class Arc
        {
            public float x, y, width, height;
            public float startAngle, sweepAngle;
            //public PointF pos, wh;
            public RectangleF rect;
            public Arc(string str)
            {
                if (str.Equals(""))
                    return;
                string[] tstr = str.Split(',');
                x = float.Parse(tstr[0], CultureInfo.InvariantCulture.NumberFormat);
                y = float.Parse(tstr[1], CultureInfo.InvariantCulture.NumberFormat);
                width = float.Parse(tstr[2], CultureInfo.InvariantCulture.NumberFormat);
                height = float.Parse(tstr[3], CultureInfo.InvariantCulture.NumberFormat);
                startAngle = float.Parse(tstr[4], CultureInfo.InvariantCulture.NumberFormat);
                sweepAngle = float.Parse(tstr[5], CultureInfo.InvariantCulture.NumberFormat);
            }
            public Arc(Arc arc)
            {
                x = arc.x;
                y = arc.y;
                width = arc.width;
                height = arc.height;
                startAngle = arc.startAngle;
                sweepAngle = arc.sweepAngle;
            }
            public void Paint(Pen pen, Graphics gr, PointF ptm)
            {
                //gr.DrawRectangle(pen, x + 1, y + 1, width, height);
                //gr.DrawArc(pen, x + 1, y + 1, width, height, startAngle, sweepAngle);
                gr.DrawArc(pen, x * ptm.X + 1, y * ptm.Y + 1, width * ptm.X, height * ptm.Y, startAngle, sweepAngle);
                //gr.DrawRectangle(
            }
            public void Rotate(int param, PointF o)
            {
                PointF[] p;
                PointF bp;
                p = new PointF[4];
                p[0] = new PointF(x, y);
                p[1] = new PointF(x + width, y);
                p[2] = new PointF(x + width, y + height);
                p[3] = new PointF(x, y + height);
                float sin, cos;
                switch (param)
                {
                    case 1:
                        p[3] = new PointF(-p[3].Y + o.Y + o.Y, p[3].X);
                        x = (int)p[3].X;
                        y = (int)p[3].Y;

                        width += height;
                        height = width - height;
                        width -= height;
                        //angle
                        return;
                    case 2:
                        p[1] = new PointF(p[1].Y, -p[1].X + o.X + o.X);
                        x = (int)p[1].X;
                        y = (int)p[1].Y;

                        width += height;
                        height = width - height;
                        width -= height;
                        //angle
                        return;
                    case 3:
                        p[3] = new PointF(p[3].X, -p[3].Y + o.Y + o.Y);
                        x = (int)p[3].X;
                        y = (int)p[3].Y;
                        return;
                    case 4:
                        p[1] = new PointF(-p[1].X + o.X + o.X, p[1].Y);
                        x = (int)p[1].X;
                        y = (int)p[1].Y;
                        return;
                    default:
                        return;
                }
                /*Console.WriteLine("====================================");
                Console.WriteLine("p1=" + p[0]);
                Console.WriteLine("p2=" + p[1]);
                Console.WriteLine("p3=" + p[2]);
                Console.WriteLine("p4=" + p[3]);
                Console.WriteLine("w=" + width.ToString());
                Console.WriteLine("h=" + height.ToString());*/
                //sp = new PointF(((sp.X - o.X) * cos + (sp.Y - o.Y) * sin) + o.X, -(sp.X - o.X) * sin + (sp.Y - o.Y) * cos + o.Y);
                //ep = new PointF(((ep.X - o.X) * cos + (ep.Y - o.Y) * sin) + o.X, -(ep.X - o.X) * sin + (ep.Y - o.Y) * cos + o.Y);
            }
        }

        private class Line
        {
            //public int x1, y1, x2, y2;
            public PointF sp, ep;
            public Line(string str)
            {
                string[] tstr = str.Split(',');
                //sp = new PointF(float.Parse(tstr[0], CultureInfo.InvariantCulture.NumberFormat), float.Parse(tstr[1], CultureInfo.InvariantCulture.NumberFormat) + 1.0F);
                //ep = new PointF(float.Parse(tstr[2], CultureInfo.InvariantCulture.NumberFormat), float.Parse(tstr[3], CultureInfo.InvariantCulture.NumberFormat) + 1.0F);
                sp = new PointF(float.Parse(tstr[0], CultureInfo.InvariantCulture.NumberFormat), float.Parse(tstr[1], CultureInfo.InvariantCulture.NumberFormat));
                ep = new PointF(float.Parse(tstr[2], CultureInfo.InvariantCulture.NumberFormat), float.Parse(tstr[3], CultureInfo.InvariantCulture.NumberFormat));
                /*x1 = Convert.ToInt32(tstr[0]);
                y1 = Convert.ToInt32(tstr[1]);
                x2 = Convert.ToInt32(tstr[2]);
                y2 = Convert.ToInt32(tstr[3]);*/
            }
            public Line(Line line)
            {
                sp = new PointF(line.sp.X, line.sp.Y);
                ep = new PointF(line.ep.X, line.ep.Y);
                /*x1 = line.x1;
                y1 = line.y1;
                x2 = line.x2;
                y2 = line.y2;*/
            }
            public PointF maxPoint()
            {
                return new PointF(maxFloat(sp.X, ep.X), maxFloat(sp.Y, ep.Y));
            }
            public void Paint(Pen pen, Graphics gr, PointF ptm)
            {
                //gr.DrawLine(pen, x1 + offsetX, y1 + offsetY, x2 + offsetX, y2 + offsetY);
                //gr.DrawLine(pen, sp.X + 1.0F, sp.Y + 1.0F, ep.X + 1.0F, ep.Y + 1.0F);
                gr.DrawLine(pen, sp.X * ptm.X + 1, sp.Y * ptm.Y + 1, ep.X * ptm.X + 1, ep.Y * ptm.Y + 1);
            }
            public void Rotate(int param, PointF o)
            {
                float sin, cos;
                switch (param)
                {
                    case 1:
                        sp = new PointF(-sp.Y + o.Y + o.Y, sp.X);
                        ep = new PointF(-ep.Y + o.Y + o.Y, ep.X);
                        return;
                    case 2:
                        sp = new PointF(sp.Y, -sp.X + o.X + o.X);
                        ep = new PointF(ep.Y, -ep.X + o.X + o.X);
                        return;
                    case 3:
                        sp = new PointF(sp.X, -sp.Y + o.Y + o.Y);
                        ep = new PointF(ep.X, -ep.Y + o.Y + o.Y);
                        return;
                    case 4:
                        sp = new PointF(-sp.X + o.X + o.X, sp.Y);
                        ep = new PointF(-ep.X + o.X + o.X, ep.Y);
                        return;
                    default:
                        return;
                }
            }
        }

        private class Arrow
        {
            public PointF sp, ep;
            private PointF[] pa;
            private float k;
            public Arrow(string str)
            {
                string[] tstr = str.Split(',');
                //sp = new PointF(float.Parse(tstr[0], CultureInfo.InvariantCulture.NumberFormat), float.Parse(tstr[1], CultureInfo.InvariantCulture.NumberFormat) + 1.0F);
                //ep = new PointF(float.Parse(tstr[2], CultureInfo.InvariantCulture.NumberFormat), float.Parse(tstr[3], CultureInfo.InvariantCulture.NumberFormat) + 1.0F);
                sp = new PointF(float.Parse(tstr[0], CultureInfo.InvariantCulture.NumberFormat), float.Parse(tstr[1], CultureInfo.InvariantCulture.NumberFormat));
                ep = new PointF(float.Parse(tstr[2], CultureInfo.InvariantCulture.NumberFormat), float.Parse(tstr[3], CultureInfo.InvariantCulture.NumberFormat));
                float h = float.Parse(tstr[4], CultureInfo.InvariantCulture.NumberFormat);
                PointF tv = new PointF(sp.X - ep.X, sp.Y - ep.Y);
                float norm = (float)Math.Sqrt(tv.X * tv.X + tv.Y * tv.Y);
                float cos = (float)Math.Cos(30.0F * Math.PI / 180);
                float sin = (float)Math.Sin(30.0F * Math.PI / 180);
                k = h / norm / cos;
                tv = new PointF(tv.X * k, tv.Y * k);
                PointF v1 = new PointF(tv.X * cos + tv.Y * sin, -tv.X * sin + tv.Y * cos);
                PointF v2 = new PointF(tv.X * cos - tv.Y * sin, tv.X * sin + tv.Y * cos);
                pa = new PointF[3];
                pa[0] = ep;
                pa[1] = new PointF(ep.X + v1.X, ep.Y + v1.Y);
                pa[2] = new PointF(ep.X + v2.X, ep.Y + v2.Y);
                /*x1 = Convert.ToInt32(tstr[0]);
                y1 = Convert.ToInt32(tstr[1]);
                x2 = Convert.ToInt32(tstr[2]);
                y2 = Convert.ToInt32(tstr[3]);*/
            }
            public Arrow(Arrow arrow)
            {
                sp = new PointF(arrow.sp.X, arrow.sp.Y);
                ep = new PointF(arrow.ep.X, arrow.ep.Y);
                pa = new PointF[3];
                pa[0] = ep;
                pa[1] = new PointF(arrow.pa[1].X, arrow.pa[1].Y);
                pa[2] = new PointF(arrow.pa[2].X, arrow.pa[2].Y);
                k = arrow.k;
                //h = arrow.h;
                /*x1 = line.x1;
                y1 = line.y1;
                x2 = line.x2;
                y2 = line.y2;*/
            }
            public PointF maxPoint()
            {
                return new PointF(maxFloat(sp.X, ep.X), maxFloat(sp.Y, ep.Y));
            }
            public void Paint(Pen pen, Graphics gr, PointF ptm)
            {
                //gr.DrawLine(pen, x1 + offsetX, y1 + offsetY, x2 + offsetX, y2 + offsetY);
                /*gr.DrawLine(pen, sp.X + 1.0F, sp.Y + 1.0F, ep.X + 1.0F, ep.Y + 1.0F);
                PointF[] tpa = new PointF[3];
                for (int i = 0; i < 3; i++)
                    tpa[i] = new PointF(pa[i].X + 1.0F, pa[i].Y + 1.0F);
                gr.FillPolygon(new SolidBrush(pen.Color), tpa);*/
                gr.DrawLine(pen, sp.X * ptm.X + 1, sp.Y * ptm.Y + 1, ep.X * ptm.X + 1, ep.Y * ptm.Y + 1);
                PointF[] tpa = new PointF[3];
                for (int i = 0; i < 3; i++)
                    tpa[i] = new PointF(pa[i].X * ptm.X + 1, pa[i].Y * ptm.Y + 1);
                gr.FillPolygon(new SolidBrush(pen.Color), tpa);
            }
            public void Rotate(int param, PointF o)
            {
                switch (param)
                {
                    case 1:
                        sp = new PointF(-sp.Y + o.Y + o.Y, sp.X);
                        ep = new PointF(-ep.Y + o.Y + o.Y, ep.X);
                        pa[0] = ep;
                        for (int i = 1; i < 3; i++)
                            pa[i] = new PointF(-pa[i].Y + o.Y + o.Y, pa[i].X);
                        return;
                    case 2:
                        sp = new PointF(sp.Y, -sp.X + o.X + o.X);
                        ep = new PointF(ep.Y, -ep.X + o.X + o.X);
                        pa[0] = ep;
                        for(int i = 1; i < 3; i++)
                            pa[i] = new PointF(pa[i].Y - o.Y + o.Y, -pa[i].X + o.X + o.X);
                        return;
                    case 3:
                        sp = new PointF(sp.X , - sp.Y + o.Y + o.Y);
                        ep = new PointF(ep.X , - ep.Y + o.Y + o.Y);
                        pa[0] = ep;
                        for (int i = 1; i < 3; i++)
                            pa[i] = new PointF(pa[i].X, -pa[i].Y + o.Y + o.Y);
                        return;
                    case 4:
                        sp = new PointF(-sp.X + o.X + o.X, sp.Y);
                        ep = new PointF(-ep.X + o.X + o.X, ep.Y);
                        pa[0] = ep;
                        for(int i = 1; i < 3; i++)
                            pa[i] = new PointF(-pa[i].X + o.X + o.X, pa[i].Y);
                        return;
                    default:
                        return;
                }
            }
        }

        private class Text
        {
            private string str;
            private PointF pos;
            public Text(string str)
            {
                if (str.Equals(""))
                    return;
                string[] tstr = str.Split(',');
                pos = new PointF(float.Parse(tstr[0], CultureInfo.InvariantCulture.NumberFormat), float.Parse(tstr[1], CultureInfo.InvariantCulture.NumberFormat));
                this.str = tstr[2];
            }
            public Text(Text text)
            {
                pos = new PointF(text.pos.X, text.pos.Y);
                str = text.str;
            }
            public void Paint(Pen pen, Graphics gr, PointF ptm)
            {
                gr.DrawString(str, new Font("Arial", 2 * ptm.Y), new SolidBrush(pen.Color), new PointF(pos.X * ptm.X, pos.Y * ptm.Y));
            }
        }

        private class Rectangle
        {
            public int x, y, width, height;
            //public PointF pos, wh;
            public RectangleF rect;
            public Rectangle(string str)
            {
                if (str.Equals(""))
                    return;
                string[] tstr = str.Split(',');
                x = Convert.ToInt32(tstr[0]);
                y = Convert.ToInt32(tstr[1]);
                width = Convert.ToInt32(tstr[2]);
                height = Convert.ToInt32(tstr[3]);
            }
            public Rectangle(int _x, int _y, int _w, int _h)
            {
                x = _x;
                y = _y;
                width = _w;
                height = _h;
            }
            public Rectangle(Rectangle rect)
            {
                x = rect.x;
                y = rect.y;
                width = rect.width;
                height = rect.height;
            }
            public void Paint(Pen pen, Graphics gr, PointF ptm)
            {
                //FIXgr.DrawRectangle(pen, x + 1, y + 1, width, height);
                gr.DrawRectangle(pen, x * ptm.X + 1, y * ptm.Y + 1, width * ptm.X, height * ptm.Y);
                //gr.DrawRectangle(
            }
            public void rotate(int rot)
            {
                if (rot == 0)
                    return;
                int x0 = x;
                int y0 = y;
                if (rot > 0)
                {
                    x = -y0 - height;
                    y = x0;
                    width += height;
                    height = width - height;
                    width -= height;
                }
                else
                {
                }
            }
            public void Rotate(int param, PointF o)
            {
                PointF[] p;
                PointF bp;
                p = new PointF[4];
                p[0] = new PointF(x, y);
                p[1] = new PointF(x + width, y);
                p[2] = new PointF(x + width, y + height);
                p[3] = new PointF(x, y + height);
                switch (param)
                {
                    case 1:
                        p[3] = new PointF(-p[3].Y + o.Y + o.Y, p[3].X);
                        x = (int)p[3].X;
                        y = (int)p[3].Y;

                        width += height;
                        height = width - height;
                        width -= height;
                        //angle
                        return;
                    case 2: 
                        p[1] = new PointF(p[1].Y, -p[1].X + o.X + o.X);
                        x = (int)p[1].X;
                        y = (int)p[1].Y;

                        width += height;
                        height = width - height;
                        width -= height;
                        //angle
                        return;
                    case 3:
                        p[3] = new PointF(p[3].X, -p[3].Y + o.Y + o.Y);
                        x = (int)p[3].X;
                        y = (int)p[3].Y;
                        return;
                    case 4:
                        p[1] = new PointF(-p[1].X + o.X + o.X, p[1].Y);
                        x = (int)p[1].X;
                        y = (int)p[1].Y;
                        return;
                    default:
                        return;
                }
            }
        }

        public class Join
        {
            public PointF p;
            public Join(string str)
            {
                string[] tstr = str.Split(',');
                //p = new PointF(float.Parse(tstr[0], CultureInfo.InvariantCulture.NumberFormat), float.Parse(tstr[1], CultureInfo.InvariantCulture.NumberFormat) + 1.0F);
                p = new PointF(float.Parse(tstr[0], CultureInfo.InvariantCulture.NumberFormat), float.Parse(tstr[1], CultureInfo.InvariantCulture.NumberFormat));
            }
            public Join(Join join)
            {
                p = new PointF(join.p.X, join.p.Y);
            }
            public void tPaint(Pen pen, Graphics gr, PointF ptm)
            {
                //gr.DrawRectangle(new Pen(Color.Red), p.X - 1.0F, p.Y - 1.0F, 2.0F, 2.0F);
                //gr.DrawRectangle(new Pen(Color.Red), p.X * ptm.X - 1.0F, p.Y * ptm.Y - 1.0F, 4.5F, 4.5F);
                float r = (float)Math.Round(2.0F * ptm.X / 2.0F);
                gr.DrawEllipse(new Pen(Color.Red, pen.Width), p.X * ptm.X - r + 1.0F, p.Y * ptm.Y - r + 1.0F, 2.0F * r, 2.0F * r);
            }
            public void pPaint(Pen pen, Graphics gr, PointF ptm)
            {
                gr.DrawRectangle(pen, p.X * ptm.X, p.Y * ptm.Y, 1.0F * ptm.X, 1.0F * ptm.Y);
                //gr.DrawLine(pen, p, p);
            }

            public void Rotate(int param, PointF o)
            {
                float sin, cos;
                switch (param)
                {
                    case 1:
                        p = new PointF(-p.Y + o.Y + o.Y, p.X);
                        return;
                    case 2:
                        p = new PointF(p.Y, -p.X + o.X + o.X);
                        return;
                    case 3:
                        p = new PointF(p.X, -p.Y + o.Y + o.Y);
                        return;
                    case 4:
                        p = new PointF(-p.X + o.X + o.X, p.Y);
                        return;
                    default:
                        return;
                }
            }
        }

        public int id;
        string ffs;
        List<Arc> arcs;
        List<Line> lines;
        List<Arrow> arrows;
        List<Text> strings;
        List<Rectangle> rectangles;
        public List<Join> joins;
        Rectangle border;
        public bool isLoad = false;
        public int Height { set; get; }
        public int Width { set; get; }
        private Region region;
        private PointF pointRotate;
        public Point TopLeft
        {
            private set;
            get;
        }

        public void Paint(Pen pen, Graphics gr, PointF ptm, int offsetX = 0, int offsetY = 0)
        {
            if (!isLoad)
                load();

            foreach (Arc arc in arcs)
                arc.Paint(pen, gr, ptm);
            foreach (Line line in lines)
                line.Paint(pen, gr, ptm);
            foreach (Arrow arrow in arrows)
                arrow.Paint(pen, gr, ptm);
            foreach (Rectangle rect in rectangles)
                rect.Paint(pen, gr, ptm);
            //foreach (Join join in joins)
                //join.tPaint(pen, gr, ptm);

            foreach(Text text in strings)
                text.Paint(pen, gr, ptm);

            //border.Paint(pen, gr, offsetX - border.width / 2, offsetY - border.height / 2);
        }
        public void jPaint(Pen pen, Graphics gr, PointF ptm)
        {
            if (!isLoad)
                load();

            foreach (Join join in joins)
                join.tPaint(pen, gr, ptm);
        }

        public void load()
        {
            List<List<string>> fstr = Query.SendQuerySelect("SELECT * FROM [image] WHERE (id=" + id.ToString() + ")");
            foreach (List<string> str in fstr)
            {
                string[] tstr;
                int i = 0;
                int maxX = 0, maxY = 0, minX = 0;
                id = Convert.ToInt32(str[i++]);
                ffs = str[i++];

                arcs = new List<Arc>();
                tstr = str[i++].Split(';');
                if (!tstr[0].Equals(""))
                    for (int ti = 0; ti < tstr.Length; ti++)
                    {
                        Arc arc = new Arc(tstr[ti]);
                        maxX = (maxX < arc.x + arc.width) ? (int)(arc.x + arc.width) : maxX;
                        maxY = (maxY < arc.y + arc.height) ? (int)(arc.y + arc.height) : maxY;
                        arcs.Add(arc);
                    }

                lines = new List<Line>();
                tstr = str[i++].Split(';'); //for lines
                if (!tstr[0].Equals(""))
                    for (int ti = 0; ti < tstr.Length; ti++)
                    {
                        Line line = new Line(tstr[ti]);
                        maxX = (int)maxFloat(maxX, line.maxPoint().X);
                        maxY = (int)maxFloat(maxY, line.maxPoint().Y);
                        /*maxX = (maxX < line.x1) ? line.x1 : maxX;
                        maxX = (maxX < line.x2) ? line.x2 : maxX;
                        maxY = (maxY < line.y1) ? line.y1 : maxY;
                        maxY = (maxY < line.y2) ? line.y2 : maxY;
                        minX = (minX > line.x1) ? line.x1 : minX;
                        minX = (minX > line.x2) ? line.x2 : minX;*/
                        lines.Add(line);
                    }

                arrows = new List<Arrow>();
                tstr = str[i++].Split(';');
                if(!tstr[0].Equals(""))
                    for (int ti = 0; ti < tstr.Length; ti++)
                    {
                        Arrow arrow = new Arrow(tstr[ti]);
                        maxX = (int)maxFloat(maxX, arrow.maxPoint().X);
                        maxY = (int)maxFloat(maxY, arrow.maxPoint().Y);
                        /*maxX = (maxX < line.x1) ? line.x1 : maxX;
                        maxX = (maxX < line.x2) ? line.x2 : maxX;
                        maxY = (maxY < line.y1) ? line.y1 : maxY;
                        maxY = (maxY < line.y2) ? line.y2 : maxY;
                        minX = (minX > line.x1) ? line.x1 : minX;
                        minX = (minX > line.x2) ? line.x2 : minX;*/
                        arrows.Add(arrow);
                    }

                //Add strings
                strings = new List<Text>();
                tstr = str[i++].Split(';');
                for (int ti = 0; ti < tstr.Length; ti++)
                {
                    Text text = new Text(tstr[ti]);
                    strings.Add(text);
                }

                rectangles = new List<Rectangle>();
                tstr = str[i++].Split(';'); //for rectangle
                for (int ti = 0; ti < tstr.Length; ti++)
                {
                    Rectangle rect = new Rectangle(tstr[ti]);
                    maxX = (maxX < rect.x + rect.width) ? rect.x + rect.width : maxX;
                    maxY = (maxY < rect.y + rect.height) ? rect.y + rect.height : maxY; 
                    rectangles.Add(rect);
                }

                TopLeft = new Point(int.MaxValue, int.MaxValue);

                joins = new List<Join>();
                tstr = str[i++].Split(';'); //for rectangle
                for (int ti = 0; ti < tstr.Length; ti++)
                {
                    Join temp = new Join(tstr[ti]);
                    if (temp.p.X + 1 < TopLeft.X)
                        TopLeft = new Point((int)temp.p.X + 1, TopLeft.Y);
                    if (temp.p.Y + 1 < TopLeft.Y)
                        TopLeft = new Point(TopLeft.X, (int)temp.p.Y + 1);
                    joins.Add(temp);
                }

                border = new Rectangle(0, 0, maxX - minX, maxY);
                //Height = maxY + 2;
                Height = maxY + 1;
                //Width = maxX + 3;
                Width = maxX + 1;

                pointRotate = new PointF(maxX, maxY);

                isLoad = true;
            }
        }

        public Region getRegion()
        {
            RefreshRegion();
            return region;
        }
        public static float maxFloat(float x1, float x2)
        {
            return (x1 > x2) ? x1 : x2;
        }

        /**
         * 1 - по часовой
         * 2 - против
         * 3 - отражение по горизонтали
         * 4 - отражение по вертикали
         **/
        public void Rotate(int param)
        {
            foreach (Arc arc in arcs)
                arc.Rotate(param, new PointF(pointRotate.X / 2.0F, pointRotate.Y / 2.0F));

            foreach (Line line in lines)
                line.Rotate(param, new PointF(pointRotate.X / 2.0F, pointRotate.Y / 2.0F));

            foreach (Arrow arrow in arrows)
                arrow.Rotate(param, new PointF(pointRotate.X / 2.0F, pointRotate.Y / 2.0F));

            /*foreach (Text text in strings)
                strings.Add(new Text(text));*/

            foreach (Rectangle rect in rectangles)
                rect.Rotate(param, new PointF(pointRotate.X / 2.0F, pointRotate.Y / 2.0F));

            TopLeft = new Point(int.MaxValue, int.MaxValue);

            foreach (Join join in joins)
            {
                join.Rotate(param, new PointF(pointRotate.X / 2.0F, pointRotate.Y / 2.0F));
                if (join.p.X + 1 < TopLeft.X)
                    TopLeft = new Point((int)join.p.X + 1, TopLeft.Y);
                if (join.p.Y + 1 < TopLeft.Y)
                    TopLeft = new Point(TopLeft.X, (int)join.p.Y + 1);
            }

            /*border = new Rectangle(img.border);

            isLoad = img.isLoad;
            Height = img.Height;
            Width = img.Width;

            RefreshRegion();*/
            if (param == 1 || param == 2)
            {
                Height = (int)pointRotate.X + 1;
                Width = (int)pointRotate.Y + 1;
                pointRotate = new PointF(pointRotate.Y, pointRotate.X);
            }
        }
    }
}
