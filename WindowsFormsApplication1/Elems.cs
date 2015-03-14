using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace MainWindow
{
    class Elems
    {
        private int id;
        public int eid { get; set; }
        private string name;
        private string prefix;
        //public List<int> disp;
        private List<string> param;
        public int image_id { get; set; }
        public Image image;

        public Elems(List<string> str, int img_id = -1)
        {
            int i = 0;
            param = new List<string>();
            id = Convert.ToInt32(str[i++]);
            eid = Convert.ToInt32(str[i++]);
            name = str[i++];
            prefix = str[i++];
            i++;
            for (int ii = i; ii < str.Count; ii++)
            {
                param.Add(str[ii]);
            }
            image_id = img_id;
            image = new Image();
        }

        public string toString()
        {
            return id + " " + name + " " + prefix;
        }

        public void Paint(Pen pen, Graphics gr, int offsetX = 0, int offsetY = 0)
        {
            image.id = image_id;
            image.Paint(pen, gr, offsetX, offsetY);
        }
    }
}
