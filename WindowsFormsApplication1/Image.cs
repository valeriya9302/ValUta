using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace MainWindow
{
    class Image
    {

        private struct Arc
        {
        }

        private class Line
        {
            public int x1, y1, x2, y2;
            public Line(string str)
            {
                string[] tstr = str.Split(',');
                x1 = Convert.ToInt32(tstr[0]);
                y1 = Convert.ToInt32(tstr[1]);
                x2 = Convert.ToInt32(tstr[2]);
                y2 = Convert.ToInt32(tstr[3]);
            }
            public void Paint(Pen pen, Graphics gr, int offsetX = 0, int offsetY = 0)
            {
                gr.DrawLine(pen, x1 + offsetX, y1 + offsetY, x2 + offsetX, y2 + offsetY);
            }
            public void rotate(double angle)
            {
                int x1o = x1;
                int x2o = x2;
                x1 = (int)((x1o) * Math.Cos(angle) - (y1) * Math.Sin(angle));
                x2 = (int)((x2o) * Math.Cos(angle) - (y2) * Math.Sin(angle));
                y1 = (int)((x1o) * Math.Sin(angle) + (y1) * Math.Cos(angle));
                y2 = (int)((x2o) * Math.Sin(angle) + (y2) * Math.Cos(angle));
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
            public void Paint(Pen pen, Graphics gr, int offsetX = 0, int offsetY = 0)
            {
                gr.DrawRectangle(pen, x + offsetX, y + offsetY, width, height);
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

        private struct Join
        {
            int x, y, width, height;
            public Join(string str)
            {
                string[] tstr = str.Split(',');
                x = Convert.ToInt32(tstr[0]) - 1;
                y = Convert.ToInt32(tstr[1]) - 1;
                width = 2;
                height = 2;
            }
            public void Paint(Pen pen, Graphics gr, int offsetX = 0, int offsetY = 0)
            {
                gr.DrawEllipse(pen, x + offsetX, y + offsetY, width, height);
            }
        }

        public int id;
        string ffs;
        //List<Arc> arcs;
        List<Line> lines;
        //List<Arrow> arrows;
        //List<Text> strings;
        List<Rectangle> rectangles;
        List<Join> joins;
        Rectangle border;
        public bool isLoad = false;

        public void Paint(Pen pen, Graphics gr, int offsetX = 0, int offsetY = 0)
        {
            if (!isLoad)
                load();

            foreach (Line line in lines)
                line.Paint(pen, gr, offsetX - border.width / 2, offsetY - border.height / 2);
            foreach (Rectangle rect in rectangles)
                rect.Paint(pen, gr, offsetX - border.width / 2, offsetY - border.height / 2);
            //foreach (Join join in joins)
            //    join.Paint(pen, gr);

            //border.Paint(pen, gr, offsetX - border.width / 2, offsetY - border.height / 2);
        }

        public void load()
        {
            List<List<string>> fstr = Query.SendQuerySelect("SELECT * FROM [image] WHERE (id=" + id.ToString() + ")");
            foreach (List<string> str in fstr)
            {
                string[] tstr;
                int i = 0;
                int maxX = 0, maxY = 0;
                id = Convert.ToInt32(str[i++]);
                ffs = str[i++];

                i++;
                //arcs = new List<Arc>();

                lines = new List<Line>();
                tstr = str[i++].Split(';'); //for lines
                for (int ti = 0; ti < tstr.Length; ti++)
                {
                    Line line = new Line(tstr[ti]);
                    maxX = (maxX < line.x1) ? line.x1 : maxX;
                    maxX = (maxX < line.x2) ? line.x2 : maxX;
                    maxY = (maxY < line.x1) ? line.y1 : maxY;
                    maxY = (maxY < line.x2) ? line.y2 : maxY;
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

                border = new Rectangle(0, 0, maxX, maxY);

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
    }
}
