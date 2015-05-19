using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace MainWindow.scheme
{
    class CategPictureBox : PictureBox
    {
        private Elems baseElem;
        private Pen pen;
        private new Size DefaultSize;

        public CategPictureBox(Elems el)
        {
            baseElem = el;
            //elem = new Elems(el);
            BackColor = Color.DeepPink;
            pen = new Pen(Color.Black);
            DefaultSize = new Size(baseElem.image.Width, baseElem.image.Height + 2);
            Size = DefaultSize;
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            DefaultSize = new Size(baseElem.image.Width, baseElem.image.Height + 2);
            Size = DefaultSize;
            //newReg - Задать регион объекта
            // Сначала требуется продумать рисование фигуры

            //Region = new Region(elem.image.getRegion().GetRegionData());

            //Bitmap flag = new Bitmap(DefaultSize.Width, DefaultSize.Height);
            if (pe != null)
            {
                pe.Graphics.Clear(Color.Azure);
                //pe.Graphics.BeginContainer(new RectangleF(), new RectangleF(), GraphicsUnit.Pixel);
                //pe.Graphics.EndContainer(pe.Graphics.BeginContainer(new RectangleF(0, 0, 1.4F, 1.4F), new RectangleF(), GraphicsUnit.Pixel));
                pe.Graphics.ScaleTransform((float)Width / (float)DefaultSize.Width, (float)Height / (float)DefaultSize.Height);
                //pe.Graphics.RotateTransform(angle);
                baseElem.Paint(pen, pe.Graphics);
                //pe.Graphics.PageScale = Width / DefaultSize.Width;
                return;
            }
            Bitmap flag = new Bitmap(Width, Height);
            this.Image = flag;
            Graphics gfx = Graphics.FromImage(this.Image);
            gfx.Clear(Color.Azure);
            baseElem.Paint(pen, gfx);
            //gfx.ScaleTransform(Width / DefaultSize.Width, Height / DefaultSize.Height);
            //this.BackgroundImage = flag;
            //BringToFront();
        }

        public System.Drawing.Image getImage()
        {
            DefaultSize = new Size(baseElem.image.Width, baseElem.image.Height + 2);
            Size = DefaultSize;
            Bitmap flag = new Bitmap(Width, Height);
            this.Image = flag;
            Graphics gfx = Graphics.FromImage(this.Image);
            gfx.Clear(Color.Azure);
            baseElem.Paint(pen, gfx);
            return Image;
        }
    }
}
