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
        public int posX { get; set; }
        public int posY { get; set; }
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
            //image_id = img_id;
            //FIXME
            List<List<string>> img = Query.SendQuerySelect("SELECT IMAGE_ID FROM [image_manager] WHERE (EID = " + eid + ")");
            if (img.Count != 0 && img[0].Count != 0)
                image_id = Convert.ToInt32(img[0][0]);
            image = new Image(image_id);
            //image = new Image();
        }

        public Elems(Elems elem)
        {
            id = elem.id;
            eid = elem.eid;
            name = elem.name;
            prefix = elem.prefix;
            param = elem.param;
            image_id = elem.image_id;
            image = new Image(elem.image);
        }

        public string toString()
        {
            return id + " " + name + " " + prefix;
        }

        public void Paint(Pen pen, Graphics gr, int offsetX = 0, int offsetY = 0)
        {
            image.id = image_id;
            image.Paint(pen, gr, offsetX + posX, offsetY + posY);
        }
    }
}
