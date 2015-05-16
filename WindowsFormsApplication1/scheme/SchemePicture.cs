using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using MainWindow.scheme;

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
            //SizeChanged += new EventHandler(EventSizeChanged);
            MouseClick += new MouseEventHandler(EventMouseClick);
            //Parent.KeyDown += new KeyEventHandler(EventKeyDown);
            
            MouseWheel += new MouseEventHandler(EventMouseWheel);
            lepb = new List<ElemPictureBox>();
            DefaultSize = new Size(1500, 1000);
            Size = DefaultSize;
            //gr = CreateGraphics();
            cursorMode = new int[2]; 
            cursorMode[0] = 0;
            cursorMode[1] = 0;
            maskSize = 10;
            ndriver = new NameDriver();
            pressedControl = false;
            scale = 1;
            UpdateMask();
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
        private NameDriver ndriver;
        private new Size DefaultSize;
        public bool pressedControl { set; get; }
        private float scale;
        

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
            if (epb != null && !epb.Disposing)
            {
                //MessageBox.Show("this");
                epb.Dispose();
                epb = null;
            }
            /*epb = new ElemPictureBox(img, this);
            Controls.Add(epb);*/
            //Cursor.Hide();
        }

        public Point PointToMask(Point p)
        {
            return new Point((int)(p.X / maskSize) * maskSize, (int)(p.Y / maskSize) * maskSize);
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
                    //Controls.Remove(epb);
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
                        epb.setName(ndriver.getFreeName(epb.getPrefix()));
                        epb.setDone();
                        lepb.Add(epb);
                        epb = null;
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
                        if (object.ReferenceEquals(sender.GetType(), typeof(ElemPictureBox)))
                            twire = new Wire(e.Location, this);
                        else
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
                            //twire.addPoint(e.Location);
                            twire.r2Point(e.Location);
                            twire.setDone();
                            cursorMode[0] = 0;
                            return;
                        }
                        if (object.ReferenceEquals(sender.GetType(), typeof(Wire)) && ((Wire)sender).isDone)
                        {
                            //replace point
                            //get last point
                            //Point p = twire.replacePoint(new Point((int)Math.Round((double)(e.X / maskSize)) * maskSize, (int)Math.Round((double)(e.Y / maskSize)) * maskSize));
                            Point p = twire.r2Point(e.Location);
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
                            //Point p = twire.replacePoint(new Point((int)Math.Round((double)(e.X / maskSize)) * maskSize, (int)Math.Round((double)(e.Y / maskSize)) * maskSize));
                            Point p = twire.r2Point(e.Location);
                            //if (!p.Equals(new Point((int)Math.Round((double)(e.X / maskSize)) * maskSize, (int)Math.Round((double)(e.Y / maskSize)) * maskSize)))
                            if (!p.Equals(e.Location))
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

        public void EventMouseClick(object sender, MouseEventArgs e)
        {
            //MessageBox.Show("Width:" + Width + "|Height:" + Height);
            //UpdateMask();
        }

        public new void Scale(float scale)
        {
            /*if (scale + ds - 1.0F < 0.1F || scale + ds - 1.0F > 2.0F)
                return;
            scale += ds - 1.0F;*/
            //maskSize += (int)(maskSize * scale);
            //EventSizeChanged(this, null);
            Width = (int)(DefaultSize.Width * scale);
            Height = (int)(DefaultSize.Height * scale);
            UpdateMask();
            foreach (Control control in Controls)
                control.Scale(scale, scale);
            //Bitmap flag = new Bitmap(Size.Width, Size.Height);
            //this.Image = flag;
            //Graphics gfx = Graphics.FromImage(this.Image);
            //gfx.ResetTransform();
            //gfx.ScaleTransform(1.3F, 1.3F);
        }

        public void EventMouseEnter(object sender, EventArgs e)
        {
            //if(epb!=null)
            //MessageBox.Show(epb.Disposing.ToString());
            if (cursorMode[0] == 1 && timg != null && (epb == null || epb.Disposing))
            {
                epb = new ElemPictureBox(timg, this);
                
                //Controls.Add(epb);
                //Cursor.Hide();
            }
            Focus();
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

        public void EventMouseWheel(object sender, MouseEventArgs e)
        {
            if (ModifierKeys.HasFlag(Keys.Control))
            {
                //var element as
                

                //float delta = e.Delta / 1200F;
                float delta = e.Delta >= 0 ? 1.1F : (1.0F / 1.1F);
                if (Width * delta < DefaultSize.Width * 0.1F || Width * delta > DefaultSize.Width * 2.0F)
                    return;
                /*if (scale + delta < 0.1F || scale + delta > 2.0F)
                    return;
                scale += delta;*/

                /*if (scale + delta - 1.0F < 0.1F || scale + delta - 1.0F > 2.0F)
                    return;
                scale += delta - 1.0F;*/
                //maskSize = (int)(10 * scale);
                /*foreach (ElemPictureBox t in lepb)
                {
                    //t.setScale(scale);
                    t.Scale(new SizeF(scale, scale));
                }*/
                SizeF scale = new SizeF(delta, delta);
                this.ScaleControl(scale, BoundsSpecified.Size);
                foreach (Control control in Controls)
                    control.Scale(scale);
                //Scale(new SizeF(delta, delta));
                //Location = new Point(0, 0);
                UpdateMask();
                //ScaleControl(new SizeF(scale, scale), BoundsSpecified.All);
                //Scale(scale);
            }
        }

        public void EventMouseMove(object sender, MouseEventArgs e)
        {
            if (cursorMode[0] == 1)
            {
                //Refresh();
                //if (object.ReferenceEquals(sender.GetType(), typeof(ElemPictureBox)))
                  //  epb.setLocation(new Point(e.X + ((ElemPictureBox)sender).Location.X, e.Y + ((ElemPictureBox)sender).Location.Y));
                //else
                epb.setLocation(new Point((int)(e.X / maskSize) * maskSize - timg.image.TopLeft.X, (int)(e.Y / maskSize) * maskSize - timg.image.TopLeft.Y));
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

        public void UpdateMask()
        {
            //MessageBox.Show("Width:" + Size.Width + "|Height:" + Size.Height);

            Bitmap flag = new Bitmap(Size.Width, Size.Height);
            maskSize = (int)(10 * (float)Size.Width / (float)DefaultSize.Width);
            this.Image = flag;
            Graphics gfx = Graphics.FromImage(this.Image);
            gfx.Clear(Color.MintCream);
            Pen pen = new Pen(Color.LightGray);
            if (maskSize == 0)
                return;
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
