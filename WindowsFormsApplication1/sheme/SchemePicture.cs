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
            lepb = new List<ElemPictureBox>();
            gr = CreateGraphics();
            cursorMode = new int[2]; 
            cursorMode[0] = 0;
            cursorMode[1] = 0;
        }

        private Graphics gr;
        private List<Elems> imgs = new List<Elems>();
        private Elems timg;
        /**
         * Режим курсора
         * 0 - Указатель
         * 1 - Расстановка новых элементов
         **/
        private int[] cursorMode;

        //test
        private ElemPictureBox epb;
        private List<ElemPictureBox> lepb;
        

        public void setCursor(int cursor)
        {
            
        }

        public void SetImage(Elems img)
        {
            timg = img;
            cursorMode[0] = 1;
            //epb = new ElemPictureBox(img);
            //Controls.Add(epb);
        }

        public void EventMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
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
                        return;
                    case 1:
            //MessageBox.Show("this");
            ElemPictureBox temp = new ElemPictureBox(timg);
            temp.setLocation(e.Location);
            lepb.Add(temp);
            Controls.Add(temp);
            Refresh();
                        //Controls.Add(epb);
                        cursorMode[1] = cursorMode[0];
                        cursorMode[0] = 0;
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
            if (cursorMode[0] == 1)
            {
                Clear();
                //Controls.RemoveAt(Controls.Count - 1);
                //epb.Dispose();
            }
        }

        public void EventMouseEnter(object sender, EventArgs e)
        {
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

        private void EventMouseMove(object sender, MouseEventArgs e)
        {
            /*if (cursorMode[0] == 1)
            {
                //Refresh();
                epb.setLocation(new Point(e.X, e.Y));
                Refresh();
            }*/
            if (cursorMode[0] != 1 || timg == null)
                return;
            Clear();
            //rePaint();
            //Refresh();
            timg.Paint(new Pen(Color.Black), this.CreateGraphics(), e.X, e.Y); 
            //this.Cursor = new Cursor
        }

        public void rePaint()
        {
            foreach (Elems el in imgs)
                el.Paint(new Pen(Color.Black), this.CreateGraphics());
        }

        public void Clear()
        {
            this.CreateGraphics().Clear(BackColor);
        }
    }
}
