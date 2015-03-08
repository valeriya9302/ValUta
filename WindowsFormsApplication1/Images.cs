using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace MainWindow
{
    class Images
    {
        public static int offsetX;
        public static int offsetY;

        public struct Arc
        {
        }

        public struct Line
        {
            int x1, y1, x2, y2;
            public Line(string str)
            {
                string[] tstr = str.Split(',');
                x1 = Convert.ToInt32(tstr[0]);
                y1 = Convert.ToInt32(tstr[1]);
                x2 = Convert.ToInt32(tstr[2]);
                y2 = Convert.ToInt32(tstr[3]);
            }
            public void Paint(Pen pen, Graphics gr)
            {
                gr.DrawLine(pen, x1 + offsetX, y1 + offsetY, x2 + offsetX, y2 + offsetY);
            }
        }

        public struct Arrow
        {
        }

        public struct Text
        {
        }

        public struct Rectangle
        {
            int x, y, width, height;
            public Rectangle(string str)
            {
                string[] tstr = str.Split(',');
                x = Convert.ToInt32(tstr[0]);
                y = Convert.ToInt32(tstr[1]);
                width = Convert.ToInt32(tstr[2]);
                height = Convert.ToInt32(tstr[3]);
            }
            public void Paint(Pen pen, Graphics gr)
            {
                gr.DrawRectangle(pen, x + offsetX, y + offsetY, width, height);
            }
        }

        public struct Join
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
            public void Paint(Pen pen, Graphics gr)
            {
                gr.DrawEllipse(pen, x + offsetX, y + offsetY, width, height);
            }
        }

        public struct Struct
        {
            public int id;
            string ffs;
            //List<Arc> arcs;
            List<Line> lines;
            //List<Arrow> arrows;
            //List<Text> strings;
            List<Rectangle> rectangles;
            List<Join> joins;
            public Struct(List<string> str)
            {
                string[] tstr;
                int i = 0;
                id = Convert.ToInt32(str[i++]);
                ffs = str[i++];
                
                i++;
                //arcs = new List<Arc>();

                lines = new List<Line>();
                tstr = str[i++].Split(';'); //for lines
                for (int ti = 0; ti < tstr.Length; ti++)
                    lines.Add(new Line(tstr[ti]));

                i++;
                
                i++;

                rectangles = new List<Rectangle>();
                tstr = str[i++].Split(';'); //for rectangle
                for (int ti = 0; ti < tstr.Length; ti++)
                    rectangles.Add(new Rectangle(tstr[ti]));

                joins = new List<Join>();
                tstr = str[i++].Split(';'); //for rectangle
                for (int ti = 0; ti < tstr.Length; ti++)
                    joins.Add(new Join(tstr[ti]));
            }
            public void Paint(Pen pen, Graphics gr)
            {
                foreach (Line line in lines)
                    line.Paint(pen, gr);
                foreach (Rectangle rect in rectangles)
                    rect.Paint(pen, gr);
                foreach (Join join in joins)
                    join.Paint(pen, gr);
            }
        }

        public List<Struct> _images;

        public Images()
        {
            _images = new List<Struct>();
            List<List<string>> fstr = Query.SendQuerySelect("SELECT * FROM [image]");
            foreach (List<string> str in fstr)
            {
                Struct tValue = new Struct(str);
                _images.Add(tValue);
            }
        }

        public void Paint(Pen pen, Graphics gr, int index)
        {
            if (index == -1)
                return;
            gr.PageUnit = GraphicsUnit.Millimeter;
            offsetX = 10;
            offsetY = 10;
            //gr.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality
            foreach (Struct img in _images)
            {
                if (img.id == index)
                {
                    img.Paint(pen, gr);
                    return;
                }
            }
        }
    }
}
