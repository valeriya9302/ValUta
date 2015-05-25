using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using MainWindow.scheme;
using System.Runtime.InteropServices;

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
            
            lepb = new List<ElemPictureBox>();
            Size = new Size(1500, 1500);
            //gr = CreateGraphics();
            cursorMode = new int[2]; 
            cursorMode[0] = 0;
            cursorMode[1] = 0;
            //maskSize = 10;
            ndriver = new NameDriver();
            pressedControl = false;
            scale = 1;
            UpdateMask();
            //rePaint();
            cms = new ContextMenuStrip();
            wcms = new ContextMenuStrip();
            tsmi = new ToolStripMenuItem[6];
            for (int i = 0; i < 6; i++)
            {
                tsmi[i] = new ToolStripMenuItem();
                tsmi[i].Name = "rot" + (i + 1).ToString();
                tsmi[i].Size = new System.Drawing.Size(154, 22);
            }
            wtsmi = new ToolStripMenuItem();
            wtsmi.Name = "del".ToString();
            wtsmi.Size = new System.Drawing.Size(154, 22);
            cms.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                tsmi[0],
                tsmi[1],
                tsmi[2],
                tsmi[3],
                tsmi[4],
                tsmi[5]});
            wcms.Items.AddRange(new ToolStripItem[] {
                wtsmi});
            cms.Name = "cms_elem";
            wcms.Name = "cms_wire";
            cms.Size = new System.Drawing.Size(155, 92);
            wcms.Size = new System.Drawing.Size(155, 30);
            tsmi[0].Text = "Повернуть по часовой";
            tsmi[0].Click += new EventHandler(CmsEventClick0);
            tsmi[1].Text = "Повернуть против часовой";
            tsmi[1].Click += new EventHandler(CmsEventClick1);
            tsmi[2].Text = "Отразить по горизонтали";
            tsmi[2].Click += new EventHandler(CmsEventClick2);
            tsmi[3].Text = "Отразить по вертикали";
            tsmi[3].Click += new EventHandler(CmsEventClick3);
            tsmi[4].Text = "Удалить";
            tsmi[4].Click += new EventHandler(CmsEventClick4);
            tsmi[5].Text = "Свойства";
            tsmi[5].Click += new EventHandler(CmsEventClick5);
            wtsmi.Click += new EventHandler(CmsEventClickDel);
            wtsmi.Text = "Удалить";
        }

        void CmsEventClick0(object sender, EventArgs e)
        {
            cepb.Rotate(1);
        }
        void CmsEventClick1(object sender, EventArgs e)
        {
            cepb.Rotate(2);
        }
        void CmsEventClick2(object sender, EventArgs e)
        {
            cepb.Rotate(3);
        }
        void CmsEventClick3(object sender, EventArgs e)
        {
            cepb.Rotate(4);
        }
        void CmsEventClick4(object sender, EventArgs e)
        {
            cepb.Dispose();
        }
        void CmsEventClick5(object sender, EventArgs e)
        {
            cepb.ShowEditForm();
        }
        void CmsEventClickDel(object sender, EventArgs e)
        {
            cwire.Dispose();
        }

        const int HORZSIZE = 4;
        const int VERTSIZE = 6;

        const int HORZRES = 8;
        const int VERTRES = 10;

        [DllImport("gdi32.dll", SetLastError = true)]
        static extern int GetDeviceCaps(
            IntPtr hDc,
            int index
            );

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        public static SizeF GetPixelSizePerMM()
        {
            IntPtr hDC = GetDC(IntPtr.Zero);
            float wM = (float)GetDeviceCaps(hDC, HORZRES) / (float)GetDeviceCaps(hDC, HORZSIZE);
            float hM = (float)GetDeviceCaps(hDC, VERTRES) / (float)GetDeviceCaps(hDC, VERTSIZE);
            ReleaseDC(IntPtr.Zero, hDC);

            return new SizeF(2.0F * wM, 2.0F * hM);
        }

        public const int useGetPixelSizePerMM = 0;

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
         * Размер маски в миллиметрах
         **/
        public Point maskSize { set; get; }

        private ElemPictureBox epb, cepb;
        private Elems timg;
        private Image trimg;
        private List<ElemPictureBox> lepb;
        private Wire twire, cwire;
        private List<Wire> lwpb;
        private NameDriver ndriver;
        public bool pressedControl { set; get; }
        private float scale;
        private ContextMenuStrip cms, wcms;
        private ToolStripMenuItem[] tsmi;
        private ToolStripMenuItem wtsmi;
        

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

        public void SetImage(Elems img, Image rimg)
        {
            timg = img;
            trimg = rimg;
            cursorMode[0] = 1;
            if (epb != null && !epb.Disposing)
            {
                //MessageBox.Show("this");
                epb.Dispose();
                epb = null;
            }
            if (timg == null)
                cursorMode[0] = 0;
            /*epb = new ElemPictureBox(img, this);
            Controls.Add(epb);*/
            //Cursor.Hide();
        }

        public Point PointToMask(Point p)
        {
            return new Point((int)((int)(p.X / maskSize.X) * maskSize.X), (int)((int)(p.Y / maskSize.Y) * maskSize.Y));
        }

        public void EventMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                if (object.ReferenceEquals(sender.GetType(), typeof(ElemPictureBox)) && ((ElemPictureBox)sender).isDone)
                {
                    cepb = (ElemPictureBox)sender;
                    cms.Show(cepb, cepb.Width, cepb.Height);
                    return;
                }
                if (object.ReferenceEquals(sender.GetType(), typeof(Wire)) && ((Wire)sender).isDone)
                {
                    cwire = (Wire)sender;
                    wcms.Show(this, e.X, e.Y);
                    return;
                }
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
                        if (timg == null)
                            return;
                        cursorMode[0] = cursorMode[1];
                        cursorMode[1] = 0;
                        if (cursorMode[0] == 1)
                        {
                            if (trimg != null)
                                epb = new ElemPictureBox(timg, trimg, this);
                            else
                                epb = new ElemPictureBox(timg, this);
                            //Controls.Add(epb);
                        }
                        return;
                    case 1:
                        epb.setName(ndriver.getFreeName(epb.getPrefix()));
                        if (!epb.setDone())
                            return;
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
                        {
                            twire = new Wire(e.Location, this);
                            ((ElemPictureBox)sender).addWire(twire);
                            twire.sel = (ElemPictureBox)sender;
                        }
                        else
                            twire = new Wire(PointToMask(e.Location), this);
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
                            ((ElemPictureBox)sender).addWire(twire);
                            twire.eel = (ElemPictureBox)sender;
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
                            tlw[0].eel = tnode;
                            tnode.addWire(tlw[0]);
                            tlw[1].eel = tnode;
                            tnode.addWire(tlw[1]);
                            twire.eel = tnode;
                            tnode.addWire(twire);
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
                            twire.eel = sender;
                            ((Node)sender).BringToFront();
                            ((Node)sender).addWire(twire);
                            cursorMode[0] = 0;
                            Refresh();
                            return;
                        }
                        //add point to wire
                        twire.addPoint(PointToMask(e.Location));
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

        public void EventMouseEnter(object sender, EventArgs e)
        {
            //if(epb!=null)
            //MessageBox.Show(epb.Disposing.ToString());
            if (cursorMode[0] == 1 && timg != null && (epb == null || epb.Disposing))
            {
                if (trimg != null)
                    epb = new ElemPictureBox(timg, trimg, this);
                else
                    epb = new ElemPictureBox(timg, this);
                
                //Controls.Add(epb);
                //Cursor.Hide();
            }
            //Focus();
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
                //FIXME//epb.setLocation(new Point((int)(e.X / maskSize) * maskSize - timg.image.TopLeft.X, (int)(e.Y / maskSize) * maskSize - timg.image.TopLeft.Y));
                epb.setLocation(new Point((int)((int)(e.X / maskSize.X) * maskSize.X - epb.TopLeft.X),
                    (int)((int)(e.Y / maskSize.Y) * maskSize.Y - epb.TopLeft.Y)));
                Console.WriteLine(epb.Location.ToString());
                //Refresh();
            }
            if (cursorMode[0] == 3)
            {
                twire.replacePoint(PointToMask(e.Location));
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
            this.Image = flag;
            Graphics gfx = Graphics.FromImage(this.Image);
            gfx.Clear(Color.MintCream);
            //gfx.PageUnit = GraphicsUnit.Millimeter;
            SizeF ptm;
            if (SchemePicture.useGetPixelSizePerMM == 1)
                ptm = SchemePicture.GetPixelSizePerMM();
            else
                ptm = new SizeF(gfx.DpiX / 25.4F, gfx.DpiY / 25.4F);
            //maskSize = new Point(5*(int)(1.0F * gfx.DpiX / 25.4F), 5*(int)(1.0F * gfx.DpiY / 25.4F));
            maskSize = new Point(5 * (int)(ptm.Width), 5 * (int)(ptm.Height));
            Pen pen = new Pen(new SolidBrush(Color.LightGray), 1.0F);
            for (int i = 0; i < Size.Height; i += (int)maskSize.Y)
            {
                gfx.DrawLine(pen, 0, i, Size.Width, i);
            }
            for (int i = 0; i < Size.Width; i += (int)maskSize.X)
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
