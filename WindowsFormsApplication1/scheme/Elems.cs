using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace MainWindow.scheme
{
    public class Elems
    {
        private int id;
        public int eid { get; set; }
        public string name;
        public string prefix;
        private List<int> disp;
        private List<string> param;
        public int image_id { get; set; }
        public int posX { get; set; }
        public int posY { get; set; }
        public Image image;

        public Elems(int img_id)
        {
            disp = new List<int>();
            param = new List<string>();
            this.image_id = img_id;
            image = new Image(image_id);
        }

        public Elems(List<string> str, int img_id = -1)
        {
            int i = 0;
            param = new List<string>();
            id = Convert.ToInt32(str[i++]);
            eid = Convert.ToInt32(str[i++]);
            name = str[i++];
            prefix = str[i++];
            disp = new List<int>();
            string[] tstr = str[i++].Split(',');
            if (!tstr[0].Equals(""))
                for (int ti = 0; ti < tstr.Length; ti++)
                    disp.Add(Convert.ToInt32(tstr[ti]));
            //disp = 0; i++;// Convert.ToInt32(str[i++]); FIXME
            param.Add(name);
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
            disp = new List<int>();
            foreach (int d in elem.disp)
                disp.Add(d);
        }

        public string toString()
        {
            //return id + " " + name + " " + prefix;
            return getTextParam();
        }

        public void Paint(Pen pen, Graphics gr, int offsetX = 0, int offsetY = 0)
        {
            image.id = image_id;
            //image.Paint(pen, gr, offsetX + posX, offsetY + posY);
            image.Paint(pen, gr, offsetX, offsetY);
        }

        public void Rotate(int param)
        {
            image.Rotate(param);
        }

        public string getTextParam()
        {
            string res = "";
            foreach (int d in disp)
                res += " " + param[d];
            return res;
        }
    }
}
