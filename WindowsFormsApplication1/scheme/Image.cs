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
    class Image
    {
        public Image(Image img)
        {
            id = img.id;
            ffs = img.ffs;
            ////arcs
            
            lines = new List<Line>();
            foreach (Line line in img.lines)
                lines.Add(new Line(line));
            
            ////arrows
            ////strings
            
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

        private struct Arc
        {
        }

        private class Line
        {
            //public int x1, y1, x2, y2;
            public PointF sp, ep;
            public Line(string str)
            {
                string[] tstr = str.Split(',');
                sp = new PointF(float.Parse(tstr[0], CultureInfo.InvariantCulture.NumberFormat), float.Parse(tstr[1], CultureInfo.InvariantCulture.NumberFormat) + 1.0F);
                ep = new PointF(float.Parse(tstr[2], CultureInfo.InvariantCulture.NumberFormat), float.Parse(tstr[3], CultureInfo.InvariantCulture.NumberFormat) + 1.0F);
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
            public void Paint(Pen pen, Graphics gr, int offsetX = 0, int offsetY = 0)
            {
                //gr.DrawLine(pen, x1 + offsetX, y1 + offsetY, x2 + offsetX, y2 + offsetY);
                gr.DrawLine(pen, sp, ep);
            }
            public void rotate(double angle)
            {
                /*int x1o = x1;
                int x2o = x2;
                x1 = (int)((x1o) * Math.Cos(angle) - (y1) * Math.Sin(angle));
                x2 = (int)((x2o) * Math.Cos(angle) - (y2) * Math.Sin(angle));
                y1 = (int)((x1o) * Math.Sin(angle) + (y1) * Math.Cos(angle));
                y2 = (int)((x2o) * Math.Sin(angle) + (y2) * Math.Cos(angle));*/
            }
        }

        private struct Arrow
        {
        }

        private struct Text
        {
        }

        private class Rectangle
        {
            public int x, y, width, height;
            public RectangleF rect;
            public Rectangle(string str)
            {
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
            public void Paint(Pen pen, Graphics gr, int offsetX = 0, int offsetY = 0)
            {
                gr.DrawRectangle(pen, x, y + 1, width, height);
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
        }

        public class Join
        {
            public PointF p;
            public Join(string str)
            {
                string[] tstr = str.Split(',');
                p = new PointF(float.Parse(tstr[0], CultureInfo.InvariantCulture.NumberFormat), float.Parse(tstr[1], CultureInfo.InvariantCulture.NumberFormat) + 1.0F);
            }
            public Join(Join join)
            {
                p = new PointF(join.p.X, join.p.Y);
            }
            public void tPaint(Pen pen, Graphics gr, int offsetX = 0, int offsetY = 0)
            {
                gr.DrawRectangle(new Pen(Color.Red), p.X - 1.0F, p.Y - 1.0F, 2.0F, 2.0F);
            }
        }

        public int id;
        string ffs;
        //List<Arc> arcs;
        List<Line> lines;
        //List<Arrow> arrows;
        //List<Text> strings;
        List<Rectangle> rectangles;
        public List<Join> joins;
        Rectangle border;
        public bool isLoad = false;
        public int Height { set; get; }
        public int Width { set; get; }
        private Region region;

        public void Paint(Pen pen, Graphics gr, int offsetX = 0, int offsetY = 0)
        {
            if (!isLoad)
                load();

            foreach (Line line in lines)
                line.Paint(pen, gr, offsetX + border.width / 2, offsetY + border.height / 2);
            foreach (Rectangle rect in rectangles)
                rect.Paint(pen, gr, offsetX + border.width / 2, offsetY + border.height / 2);
            //foreach (Join join in joins)
            //    join.tPaint(pen, gr, offsetX + border.width / 2, offsetY + border.height / 2);

            //border.Paint(pen, gr, offsetX - border.width / 2, offsetY - border.height / 2);
        }
        public void jPaint(Pen pen, Graphics gr, int offsetX = 0, int offsetY = 0)
        {
            if (!isLoad)
                load();

            foreach (Join join in joins)
                join.tPaint(pen, gr, offsetX + border.width / 2, offsetY + border.height / 2);
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

                i++;
                //arcs = new List<Arc>();

                lines = new List<Line>();
                tstr = str[i++].Split(';'); //for lines
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


                i++;

                i++;

                rectangles = new List<Rectangle>();
                tstr = str[i++].Split(';'); //for rectangle
                for (int ti = 0; ti < tstr.Length; ti++)
                {
                    Rectangle rect = new Rectangle(tstr[ti]);
                    maxX = (maxX < rect.x + rect.width) ? rect.x + rect.width : maxX;
                    maxY = (maxY < rect.y + rect.height) ? rect.y + rect.height : maxY; 
                    rectangles.Add(rect);
                }

                joins = new List<Join>();
                tstr = str[i++].Split(';'); //for rectangle
                for (int ti = 0; ti < tstr.Length; ti++)
                    joins.Add(new Join(tstr[ti]));

                border = new Rectangle(0, 0, maxX - minX, maxY);
                Height = maxY + 2;
                Width = maxX + 1;

                isLoad = true;
            }
        }

        public void LeftRotate()
        {
        }

        public void RightRotate()
        {
            double angle = Math.PI * 90.0 / 180.0;
            Console.WriteLine(angle);
            /*foreach(Line line in lines)
            {
                line.rotate(angle);
            }*/
            foreach (Rectangle rect in rectangles)
                rect.rotate(1);
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
    }
}
