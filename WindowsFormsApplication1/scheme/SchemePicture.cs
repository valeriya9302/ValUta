using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using MainWindow.sheme;

namespace MainWindow
{
    class SchemePicture : PictureBox
    {
        public SchemePicture()
        {
            MouseMove += new MouseEventHandler(EventMouseMove);
            MouseDown += new MouseEventHandler(EventMouseDown);
            MouseLeave += new EventHandler(EventMouseLeave);
            MouseEnter += new EventHandler(EventMouseEnter);
            SizeChanged += new EventHandler(EventSizeChanged);
            lepb = new List<ElemPictureBox>();
            //gr = CreateGraphics();
            cursorMode = new int[2]; 
            cursorMode[0] = 0;
            cursorMode[1] = 0;
            maskSize = 10;
            //rePaint();
        }

        //private Graphics gr;
        //private List<Elems> imgs = new List<Elems>();
        /**
         * Режим курсора
         * 0 - Указатель
         * 1 - Расстановка новых элементов
         * 2 - Установка проводников
         * 3 - Добавление точек проводнику
         **/
        private int[] cursorMode;
        /**
         * Размер маски в пикселях
         **/
        public int maskSize { set; get; }

        private ElemPictureBox epb;
        private Elems timg;
        private List<ElemPictureBox> lepb;
        private Wire twire;
        private List<Wire> lwpb;
        

        public void setCursor(int cursor)
        {
            if (cursor == 2 && cursorMode[0] == 3)
                return;
            if (cursorMode[0] != 0)
            {
                cursorMode[1] = cursorMode[0];
            }
            cursorMode[0] = cursor;
        }

        public void SetImage(Elems img)
        {
            timg = img;
            cursorMode[0] = 1;
            /*epb = new ElemPictureBox(img, this);
            Controls.Add(epb);*/
            //Cursor.Hide();
        }

        public void EventMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                if (cursorMode[0] == 3)
                {
                    if (twire.removeLast())
                        return;
                    else
                    {
                        cursorMode[0] = 0;
                        Controls.Remove(twire);
                    }
                }
                if (cursorMode[0] == 1)
                {
                    Controls.Remove(epb);
                    epb.Dispose();
                }
                //cursorMode[0] = 0;
                /*if (cursorMode[0] == 1)
                    Controls.RemoveAt(Controls.Count - 1);*/
                Refresh();
                if (cursorMode[0] == 0)
                    return;
                cursorMode[1] = cursorMode[0];
                cursorMode[0] = 0;
                return;
            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                switch (cursorMode[0])
                {
                    case 0:
                        cursorMode[0] = cursorMode[1];
                        cursorMode[1] = 0;
                        if (cursorMode[0] == 1)
                        {
                            epb = new ElemPictureBox(timg, this);
                            //Controls.Add(epb);
                        }
                        return;
                    case 1:
                        epb.setDone();
                        lepb.Add(epb);
                        cursorMode[1] = cursorMode[0];
                        cursorMode[0] = 0;
                        /*//MessageBox.Show("this");
                        ElemPictureBox temp = new ElemPictureBox(new Elems(timg), this);
                        temp.setLocation(new Point((int)(e.X / maskSize) * maskSize, (int)(e.Y / maskSize) * maskSize - timg.image.Height / 2));
                        lepb.Add(temp);
                        Controls.Add(temp);
                        //temp.Parent = this;
                        temp.BringToFront();
                        Refresh();
                        //Controls.Add(epb);
                        cursorMode[1] = cursorMode[0];
                        cursorMode[0] = 0;*/
                        return;
                    case 2:
                        //create wire
                        //MessageBox.Show("Провод!!!" + e.X.ToString() + "|" + e.Y.ToString());
                        twire = new Wire(new Point((int)(e.X / maskSize) * maskSize, (int)(e.Y / maskSize) * maskSize), this);
                        Controls.Add(twire);
                        //twire.Parent = this;
                        twire.BringToFront();
                        Refresh();
                        cursorMode[0] = 3;
                        return;
                    case 3:
                        if (object.ReferenceEquals(sender.GetType(), typeof(ElemPictureBox)))
                        {
                            //end wire
                            twire.addPoint(new Point((int)(e.X / maskSize) * maskSize, (int)(e.Y / maskSize) * maskSize));
                            twire.setDone();
                            cursorMode[0] = 0;
                            return;
                        }
                        if (object.ReferenceEquals(sender.GetType(), typeof(Wire)) && ((Wire)sender).isDone)
                        {
                            //replace point
                            //get last point
                            Point p = twire.replacePoint(new Point((int)Math.Round((double)(e.X / maskSize)) * maskSize, (int)Math.Round((double)(e.Y / maskSize)) * maskSize));
                            //split from last point
                            List<Wire> tlw = ((Wire)sender).split(p);
                            if (tlw == null)
                                return;
                            twire.setDone();
                            //create node from last point
                            Node tnode = new Node(p, this);
                            Controls.Add(tnode);
                            tnode.BringToFront();
                            //необходимо разрезать существующий провод в точке узла на 2 части и поместить в контрол
                            //twire.addPoint(new Point((int)Math.Round((double)(e.X / maskSize)) * maskSize, (int)Math.Round((double)(e.Y / maskSize)) * maskSize));
                            //List<Wire> tlw = ((Wire)sender).split(new Point((int)Math.Round((double)(e.X / maskSize)) * maskSize, (int)Math.Round((double)(e.Y / maskSize)) * maskSize));
                            //Controls.Remove((Wire)sender);
                            ((Wire)sender).Dispose();
                            foreach (Wire wire in tlw)
                            {
                                Controls.Add(wire);
                                wire.BringToFront();
                            }
                            tnode.BringToFront();
                            cursorMode[0] = 0;
                            Refresh();
                            return;
                        }
                        if (object.ReferenceEquals(sender.GetType(), typeof(Node)))
                        {
                            Point p = twire.replacePoint(new Point((int)Math.Round((double)(e.X / maskSize)) * maskSize, (int)Math.Round((double)(e.Y / maskSize)) * maskSize));
                            if (!p.Equals(new Point((int)Math.Round((double)(e.X / maskSize)) * maskSize, (int)Math.Round((double)(e.Y / maskSize)) * maskSize)))
                                return;
                            twire.setDone();
                            ((Node)sender).BringToFront();
                            cursorMode[0] = 0;
                            Refresh();
                            return;
                        }
                        //add point to wire
                        twire.addPoint(new Point((int)(e.X / maskSize) * maskSize, (int)(e.Y / maskSize) * maskSize));
                        return;
                }
            }

            /*epb.setLocation(e.Location);
            Controls.Add(epb);
            epb.BringToFront();
            epb.Paint();*/
            /*if (timg == null)
                return;
            imgs.Add(new Elems(timg));
            imgs[imgs.Count - 1].posX = e.X;
            imgs[imgs.Count - 1].posY = e.Y;
            rePaint();*/
        }

        public void EventMouseLeave(object sender, EventArgs e)
        {
            /*if (cursorMode[0] != 1 || epb.isMouseEnter)
                return;
            Cursor.Show();
             */
            //MessageBox.Show(e.X);
            if (cursorMode[0] == 1)
            {
                //Refresh();
                //Controls.RemoveAt(Controls.Count - 1);
                //epb.Dispose();
                //Cursor.Show();
            }
        }

        public void EventMouseEnter(object sender, EventArgs e)
        {
            if (cursorMode[0] == 1 && timg != null && (epb == null || epb.Disposing))
            {
                epb = new ElemPictureBox(timg, this);
                //Controls.Add(epb);
                //Cursor.Hide();
            }
            /*Console.WriteLine("qwe");
            if (cursorMode[0] == 1)
            {
                epb = new ElemPictureBox(timg);
                //epb.MouseUp += new MouseEventHandler(EventMouseClick);
                Controls.Add(epb);
                epb.MouseMove += new MouseEventHandler(EventMouseMove);
                //epb.BringToFront();
                //epb.MouseUp += new MouseEventHandler(EventMouseClick);
            }*/
        }

        public void EventMouseMove(object sender, MouseEventArgs e)
        {
            if (cursorMode[0] == 1)
            {
                //Refresh();
                //if (object.ReferenceEquals(sender.GetType(), typeof(ElemPictureBox)))
                  //  epb.setLocation(new Point(e.X + ((ElemPictureBox)sender).Location.X, e.Y + ((ElemPictureBox)sender).Location.Y));
                //else
                epb.setLocation(new Point((int)(e.X / maskSize) * maskSize, (int)(e.Y / maskSize) * maskSize - timg.image.Height / 2));
                //Refresh();
            }
            if (cursorMode[0] == 3)
            {
                twire.replacePoint(new Point((int)(e.X / maskSize) * maskSize, (int)(e.Y / maskSize) * maskSize));
                Refresh();
                return;
            }
            if (cursorMode[0] != 1 || timg == null)
                return;
            //Refresh();
            //rePaint();
            //Refresh();
            //timg.Paint(new Pen(Color.Black), this.CreateGraphics(), (int)(e.X / maskSize) * maskSize, 
            //    (int)(e.Y / maskSize) * maskSize - timg.image.Height / 2 + 2);
            //timg.image.Height / 2
            //this.Cursor = new Cursor
        }

        public void EventSizeChanged(object sender, EventArgs e)
        {
            Bitmap flag = new Bitmap(Size.Width, Size.Height);
            this.Image = flag;
            Graphics gfx = Graphics.FromImage(this.Image);
            gfx.Clear(Color.MintCream);
            Pen pen = new Pen(Color.LightGray);
            for (int i = 0; i < Size.Height; i += maskSize)
            {
                gfx.DrawLine(pen, 0, i, Size.Width, i);
            }
            for (int i = 0; i < Size.Width; i += maskSize)
                gfx.DrawLine(pen, i, 0, i, Size.Height);
        }

        /*public void rePaint()
        {
            //foreach (Elems el in imgs)
            //    el.Paint(new Pen(Color.Black), this.CreateGraphics());
            Bitmap flag = new Bitmap(Size.Width, Size.Height);
            this.Image = flag;
            Graphics gfx = Graphics.FromImage(this.Image);
            gfx.Clear(Color.MintCream);
            Pen pen = new Pen(Color.Gray);
            for (int i = 0; i < Size.Height; i += maskSize)
            {
                gfx.DrawLine(pen, 0, i, Size.Width, i);
            }
            for (int i = 0; i < Size.Width; i += maskSize)
                gfx.DrawLine(pen, i, 0, i, Size.Height);
        }*/

        /*public void Clear()
        {
            this.CreateGraphics().Clear(BackColor);
            rePaint();
        }*/
    }
}
