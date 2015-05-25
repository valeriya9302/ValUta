using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace MainWindow.scheme
{
    /**
     * Класс для рисования одного элемента на схеме
     **/
    public class ElemPictureBox : PictureBox
    {
        public Elems elem { private set; get; }
        public Image image;
        private Point oldPos;
        private Pen pen;
        private List<Text> text;
        private bool sendParent = false;
        public bool isDone;
        public bool isMouseEnter;
        private Point DefaultLocation;
        private Image.Join Paintjoin;
        public Point TopLeft { private set; get; }
        private List<Wire> wire;
        private PointF ptm;

        public ElemPictureBox(Elems el, Image img, object parent)
        {
            elem = el;
            image = new MainWindow.Image(img);
            //elem = new Elems(el);
            Parent = (Control)parent;
            MouseDown += new MouseEventHandler(EventMouseDown);
            MouseMove += new MouseEventHandler(EventMouseMove);
            //MouseMove += new MouseEventHandler(((SchemePicture)Parent).EventMouseMove);
            //MouseClick += new MouseEventHandler(EventMouseClick);
            //MouseEnter += new EventHandler(EventMouseEnter);
            //MouseEnter += new EventHandler(((SchemePicture)Parent).EventMouseEnter);
            //MouseLeave += new EventHandler(EventMouseLeave);
            Disposed += new EventHandler(EventDisposed);
            Resize += new EventHandler(EventResize);
            //ResizeRedraw = true;
            BackColor = Color.DeepPink;
            pen = new Pen(Color.Black);
            //Height = elem.image.Height + 2;
            //Width = elem.image.Width;
            //FIXME//DefaultSize = new Size(elem.image.Width, elem.image.Height + 2);
            Size = new Size(image.Width, image.Height + 2);
            isDone = false;
            sendParent = false;
            text = new List<Text>();
            wire = new List<Wire>();
            //name = elem.prefix;
            /*foreach (Image.Join join in elem.image.joins)
            {
                Controls.Add(join);
                join.Parent = this;
                join.BackColor = Color.Red;
                join.BringToFront();
            }*/
        }

        public ElemPictureBox(Elems el)
        {
            elem = el;
            image = new MainWindow.Image(elem.image_id);
            //elem = new Elems(el);
            BackColor = Color.DeepPink;
            pen = new Pen(Color.Black);
            //FIXME//DefaultSize = new Size(elem.image.Width, elem.image.Height + 2);
            Size = new Size(image.Width, image.Height + 2);
            wire = new List<Wire>();
        }

        public ElemPictureBox(Elems el, object parent)
        {
            elem = el;
            image = new MainWindow.Image(elem.image_id);
            //elem = new Elems(el);
            Parent = (Control)parent;
            MouseDown += new MouseEventHandler(EventMouseDown);
            MouseMove += new MouseEventHandler(EventMouseMove);
            //MouseMove += new MouseEventHandler(((SchemePicture)Parent).EventMouseMove);
            //MouseClick += new MouseEventHandler(EventMouseClick);
            //MouseEnter += new EventHandler(EventMouseEnter);
            //MouseEnter += new EventHandler(((SchemePicture)Parent).EventMouseEnter);
            //MouseLeave += new EventHandler(EventMouseLeave);
            Disposed += new EventHandler(EventDisposed);
            Resize += new EventHandler(EventResize);
            //ResizeRedraw = true;
            BackColor = Color.DeepPink;
            pen = new Pen(Color.Black);
            //Height = elem.image.Height + 2;
            //Width = elem.image.Width;
            //FIXME//DefaultSize = new Size(elem.image.Width, elem.image.Height + 2);
            Size = new Size(image.Width, image.Height + 2);
            isDone = false;
            sendParent = false;
            text = new List<Text>();
            wire = new List<Wire>();
            //name = elem.prefix;
            /*foreach (Image.Join join in elem.image.joins)
            {
                Controls.Add(join);
                join.Parent = this;
                join.BackColor = Color.Red;
                join.BringToFront();
            }*/
        }

        public void EventDisposed(object sender, EventArgs e)
        {
            //MouseDown -= MouseDown;
            //MessageBox.Show("Disposed!");
            MouseDown -= EventMouseDown;
            MouseMove -= EventMouseMove;
            Disposed -= EventDisposed;
            MouseEnter -= EventMouseEnter;
            MouseLeave -= EventMouseLeave;
            foreach (Text t in text)
                t.Dispose();
            text.Clear();
            foreach (Wire w in wire)
                w.Dispose();
        }

        public void setLocation(Point pos)
        {
            Location = pos;
            DefaultLocation = Location;
            //Refresh();
            //OnPaint(null);
            Refresh();
        }

        public bool setDone()
        {
            if (elem.Model)
            {
                text.Add(new Text(elem.getTextParam()));
                //text[0].Parent = Parent;
                //text[0].setLocation(Location);
                text.Add(new Text(Name));
                //text[1].Parent = Parent;
                //text[1].setLocation(text[0].Location);

                EditForm ef = new EditForm(text[1].Str, !text[1].Visible, elem, !text[0].Visible);
                if (ef.ShowDialog(this) == DialogResult.OK)
                {
                    isDone = true;
                    elem = new Elems(elem);
                    MouseDoubleClick += new MouseEventHandler(EventMouseDoubleClick);
                    MouseEnter += new EventHandler(EventMouseEnter);
                    MouseLeave += new EventHandler(EventMouseLeave);

                    text[1].Str = ef.name;
                    text[1].Visible = !ef.HiddenName;

                    elem = ef.el;
                    text[0].Str = elem.getTextParam();
                    text[0].Visible = !ef.HiddenEl;
                    
                    text[0].Parent = Parent;
                    text[0].setLocation(Location);
                    text[1].Parent = Parent;
                    text[1].setLocation(text[0].Location);
                    return true;
                }
                return false;
            }
            isDone = true;
            elem = new Elems(elem);
            MouseDoubleClick += new MouseEventHandler(EventMouseDoubleClick);
            MouseEnter += new EventHandler(EventMouseEnter);
            MouseLeave += new EventHandler(EventMouseLeave);

            text.Add(new Text(elem.getTextParam()));
            text[0].Parent = Parent;
            text[0].setLocation(Location);
            text.Add(new Text(Name));
            text[1].Parent = Parent;
            text[1].setLocation(text[0].Location);

            return true;
            
            /*text[0].setLocation(new Point(Location.X + Width / 2, Location.Y));
            //text.Add(new sheme.Text(null, Parent, new Point(text[0].Location.X + Width / 2, Location.Y)));
            //text = new Text(null, Parent, new Point(Location.X + Width / 2, Location.Y));
            Parent.Controls.Add(text[0]);
            text[0].BringToFront();*/
            //text[0].Parent = Parent;
        }

        void EventMouseDoubleClick(object sender, MouseEventArgs e)
        {
            ShowEditForm();
        }

        public void ShowEditForm()
        {
            EditForm ef = new EditForm(text[1].Str, !text[1].Visible, elem, !text[0].Visible);
            if (ef.ShowDialog(this) == DialogResult.OK)
            {
                text[1].Str = ef.name;
                text[1].Visible = !ef.HiddenName;

                elem = ef.el;
                text[0].Str = elem.getTextParam();
                text[0].Visible = !ef.HiddenEl;
            }
        }

        public string getPrefix()
        {
            return elem.prefix;
        }

        public void setName(string name)
        {
            this.Name = name;
            //OnPaint(
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            if (pe == null)
                return;

            if (SchemePicture.useGetPixelSizePerMM == 1)
            {
                SizeF tptm = SchemePicture.GetPixelSizePerMM();
                ptm = new PointF(tptm.Width, tptm.Height);
            }
            else
                ptm = new PointF(pe.Graphics.DpiX / 25.4F, pe.Graphics.DpiY / 25.4F);
            ptm = new PointF((int)(ptm.X), (int)(ptm.Y));
            //ptm = new PointF((int)(25.4F / pe.Graphics.DpiX), (int)(25.4F / pe.Graphics.DpiY));
            TopLeft = new Point((int)((image.TopLeft.X-1) * ptm.X + 1), (int)((image.TopLeft.Y-1) * ptm.Y + 1));
            Size = new Size((int)(image.Width * ptm.X), (int)((image.Height + 2) * ptm.Y));
            
            //pe.Graphics.PageUnit = GraphicsUnit.Millimeter;
            //pe.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            pe.Graphics.Clear(Color.Azure);
            image.Paint(new Pen(pen.Color, (int)(Math.Round(ptm.X / 2.0F))), pe.Graphics, ptm);
            if (Paintjoin != null)
                Paintjoin.tPaint(pen, pe.Graphics, ptm);

            return;
        }

        public System.Drawing.Image getImage()
        {
            //FIXME//DefaultSize = new Size(baseElem.image.Width, baseElem.image.Height + 2);
            //Size = DefaultSize;
            Bitmap flag = new Bitmap(Width, Height);
            this.Image = flag;
            Graphics gfx = Graphics.FromImage(this.Image);
            
            gfx.Clear(Color.Azure);
            image.Paint(pen, gfx, new PointF(1.0F, 1.0F));
            
            return Image;
        }

        public void EventMouseDown(object sender, MouseEventArgs e)
        {
            //MessageBox.Show("this");
            if (!isDone)
            {
                ((SchemePicture)Parent).EventMouseDown(sender, new MouseEventArgs(e.Button, e.Clicks, Location.X, Location.Y, e.Delta));
                return;
            }
            if (sendParent && e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                //MouseEventArgs ne = new MouseEventArgs(e.Button, e.Clicks, oldPos.X + Location.X, oldPos.Y + Location.Y, e.Delta);
                if (Paintjoin == null)
                    return;
                MouseEventArgs ne = new MouseEventArgs(e.Button, e.Clicks, Location.X + (int)(Math.Round(Paintjoin.p.X * ptm.X) + 1), Location.Y + (int)(Math.Round(Paintjoin.p.Y * ptm.Y) + 1), e.Delta);
                ((SchemePicture)Parent).setCursor(2);
                ((SchemePicture)Parent).EventMouseDown(sender, ne);
                return;
            }
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
                oldPos = new Point(e.X, e.Y);
            else if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                MouseEventArgs ne = new MouseEventArgs(e.Button, e.Clicks, Location.X, Location.Y, e.Delta);
                ((SchemePicture)Parent).EventMouseDown(sender, ne);
            }
            //text[0].EventMouseDown(sender, e);
        }

        public void EventMouseMove(object sender, MouseEventArgs e)
        {
            if (!isDone)
            {
                ((SchemePicture)Parent).EventMouseMove(sender, new MouseEventArgs(e.Button, e.Clicks, Location.X + e.X, Location.Y + e.Y, e.Delta));
                return;
            }
            if (sendParent)
                return;
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                Point oldLocation = Location;
                Point tloc = ((SchemePicture)Parent).PointToMask(new Point(Location.X - oldPos.X + e.X, Location.Y - oldPos.Y + e.Y));
                setLocation(new Point(tloc.X - TopLeft.X, tloc.Y - TopLeft.Y));
                /*!!!setLocation(new Point(
                    //FIXME//(int)((Location.X - oldPos.X + e.X) / ((SchemePicture)(Parent)).maskSize) * ((SchemePicture)(Parent)).maskSize - elem.image.TopLeft.X,
                    //FIXME//(int)((Location.Y - oldPos.Y + e.Y) / ((SchemePicture)(Parent)).maskSize) * ((SchemePicture)(Parent)).maskSize - elem.image.TopLeft.Y));
                    (int)((Location.X - oldPos.X + e.X) / ((SchemePicture)(Parent)).maskSize) * ((SchemePicture)(Parent)).maskSize - image.TopLeft.X,
                    (int)((Location.Y - oldPos.Y + e.Y) / ((SchemePicture)(Parent)).maskSize) * ((SchemePicture)(Parent)).maskSize - image.TopLeft.Y));
                *///text[0].EventMouseMove(sender, e);
                text[0].offsetLocation(new Point(Location.X - oldLocation.X, Location.Y - oldLocation.Y));
                text[1].offsetLocation(new Point(Location.X - oldLocation.X, Location.Y - oldLocation.Y));
                /*text[0].offsetLocation(new Point(
                    (int)((Location.X - oldPos.X + e.X) / ((SchemePicture)(Parent)).maskSize) * ((SchemePicture)(Parent)).maskSize,
                    (int)((Location.Y - oldPos.Y + e.Y) / ((SchemePicture)(Parent)).maskSize) * ((SchemePicture)(Parent)).maskSize - elem.image.Height / 2));*/
                Parent.Refresh();
                return;
            }
            //else
            {
                /*foreach (Image.Join join in elem.image.joins)
                    if (Math.Abs(join.x - e.X) < 3 && Math.Abs(join.y - e.Y) < 3)
                        MessageBox.Show("321");*/
                //FIXME//foreach (Image.Join join in elem.image.joins)
                foreach (Image.Join join in image.joins)
                {
                    //Graphics gfx = Graphics.FromImage(this.Image);
                    if (Math.Abs((join.p.X + 1.0F) * ptm.X - e.X) < 4.0F && Math.Abs((join.p.Y + 1.0F) * ptm.Y - e.Y) < 4.0F)
                    {
                        Paintjoin = join;
                        oldPos = new Point((int)join.p.X, (int)join.p.Y);
                        sendParent = true;
                        Refresh();
                    }
                }
            }
        }

        public void EventMouseEnter(object sender, EventArgs e)
        {
            isMouseEnter = true;
            BringToFront();
            if (!isDone)
                return;
            pen.Color = Color.MediumVioletRed;
            //OnPaint(null);
            Refresh();
        }

        public void EventMouseLeave(object sender, EventArgs e)
        {
            isMouseEnter = false;
            if (!isDone)
            {
                //((SchemePicture)Parent).EventMouseLeave(sender, e);
                return;
            }
            Paintjoin = null;
            sendParent = false;
            pen.Color = Color.Black;
            SendToBack();
            //OnPaint(null);
            Refresh();
        }

        public void EventResize(object sender, EventArgs e)
        {
            //Update();
            Refresh();
            //OnPaint(null);
        }

        public void addWire(Wire w)
        {
            wire.Add(w);
        }

        /**
         * 1 - по часовой
         * 2 - против
         * 3 - отражение по горизонтали
         * 4 - отражение по вертикали
         **/
        public void Rotate(int param)
        {
            image.Rotate(param);
            //FIXME//DefaultSize = new Size(elem.image.Width, elem.image.Height + 2);
            Size = new Size(image.Width, image.Height);
            Refresh();
        }

        private void InitializeComponent()
        {
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // ElemPictureBox
            // 
            this.WaitOnLoad = true;
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }
    }
}
