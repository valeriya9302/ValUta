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
    class ElemPictureBox : PictureBox
    {
        public Elems elem { private set; get; }
        private Point oldPos;
        private Pen pen;
        private List<Text> text;
        private bool sendParent = false;
        public bool isDone;
        public bool isMouseEnter;
        private new Size DefaultSize;
        private Point DefaultLocation;
        public float angle { get; set; }
        private Image.Join Paintjoin;
        private List<Wire> wire;
        private bool current;
        public bool Current 
        {
            get
            {
                return current;
            }
            set
            {
                if (sendParent || !isDone)
                    return;
                current = value;
                if (current)
                    pen.Color = Color.MediumVioletRed;
                else
                    pen.Color = Color.Black;
            }
        }

        public ElemPictureBox(Elems el)
        {
            elem = el;
            //elem = new Elems(el);
            BackColor = Color.DeepPink;
            pen = new Pen(Color.Black);
            DefaultSize = new Size(elem.image.Width, elem.image.Height + 2);
            angle = 0.0F;
            Size = DefaultSize;
            wire = new List<Wire>();
        }

        public ElemPictureBox(Elems el, object parent)
        {
            elem = el;
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
            DefaultSize = new Size(elem.image.Width, elem.image.Height + 2);
            Size = DefaultSize;
            isDone = false;
            sendParent = false;
            angle = 0.0F;
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
            MouseClick -= EventMouseClick;
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
            OnPaint(null);
            Refresh();
        }

        public void setDone()
        {
            isDone = true;
            elem = new Elems(elem);
            MouseClick += new MouseEventHandler(EventMouseClick);
            MouseDoubleClick += new MouseEventHandler(EventMouseDoubleClick);
            MouseEnter += new EventHandler(EventMouseEnter);
            MouseLeave += new EventHandler(EventMouseLeave);

            text.Add(new Text(elem.getTextParam()));
            text[0].Parent = Parent;
            text[0].setLocation(Location);
            text.Add(new Text(Name));
            text[1].Parent = Parent;
            text[1].setLocation(text[0].Location);
            
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
            EditForm ef = new EditForm(text[1].Str, text[1].Hidden);
            if (ef.ShowDialog(this) == DialogResult.OK)
            {
                text[1].Str = ef.name;
                text[1].Hidden = ef.HiddenName;
            }
        }

        public string getPrefix()
        {
            return elem.prefix;
        }

        public void setScale(int scale)
        {
            Bitmap flag = new Bitmap(Width, Height);
            this.Image = flag;
            Graphics gfx = Graphics.FromImage(this.Image);
            gfx.ScaleTransform(scale, scale);
            //repaint();
            Update();
        }

        /*public new void Scale(float scale)
        {
            Width = (int)(DefaultSize.Width * scale);
            Height = (int)(DefaultSize.Height * scale);
            Location = new Point((int)(DefaultLocation.X / scale), (int)(DefaultLocation.Y / scale));
        }*/

        public void setName(string name)
        {
            this.Name = name;
            //OnPaint(
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            DefaultSize = new Size(elem.image.Width, elem.image.Height + 2);
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
                elem.Paint(pen, pe.Graphics);
                if (Paintjoin != null)
                    Paintjoin.tPaint(pen, pe.Graphics);
                //pe.Graphics.PageScale = Width / DefaultSize.Width;
                return;
            }
            Bitmap flag = new Bitmap(Width, Height);
            this.Image = flag;
            Graphics gfx = Graphics.FromImage(this.Image);
            gfx.Clear(Color.Azure);
            elem.Paint(pen, gfx);
            //gfx.ScaleTransform(Width / DefaultSize.Width, Height / DefaultSize.Height);
            //this.BackgroundImage = flag;
            //BringToFront();
        }

        public void EventMouseDown(object sender, MouseEventArgs e)
        {
            //MessageBox.Show("this");
            if (!isDone)
            {
                ((SchemePicture)Parent).EventMouseDown(sender, new MouseEventArgs(e.Button, e.Clicks, Location.X + e.X, Location.Y + e.Y, e.Delta));
                return;
            }
            if (sendParent && e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                //MouseEventArgs ne = new MouseEventArgs(e.Button, e.Clicks, oldPos.X + Location.X, oldPos.Y + Location.Y, e.Delta);
                if (Paintjoin == null)
                    return;
                MouseEventArgs ne = new MouseEventArgs(e.Button, e.Clicks, Location.X + (int)Paintjoin.p.X + 1, Location.Y + (int)Paintjoin.p.Y + 1, e.Delta);
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
                setLocation(new Point(
                    (int)((Location.X - oldPos.X + e.X) / ((SchemePicture)(Parent)).maskSize) * ((SchemePicture)(Parent)).maskSize - elem.image.TopLeft.X,
                    (int)((Location.Y - oldPos.Y + e.Y) / ((SchemePicture)(Parent)).maskSize) * ((SchemePicture)(Parent)).maskSize - elem.image.TopLeft.Y));
                //text[0].EventMouseMove(sender, e);
                text[0].offsetLocation(new Point(Location.X - oldLocation.X, Location.Y - oldLocation.Y));
                text[1].offsetLocation(new Point(Location.X - oldLocation.X, Location.Y - oldLocation.Y));
                /*text[0].offsetLocation(new Point(
                    (int)((Location.X - oldPos.X + e.X) / ((SchemePicture)(Parent)).maskSize) * ((SchemePicture)(Parent)).maskSize,
                    (int)((Location.Y - oldPos.Y + e.Y) / ((SchemePicture)(Parent)).maskSize) * ((SchemePicture)(Parent)).maskSize - elem.image.Height / 2));*/
                Parent.Refresh();
                return;
            }
            else
            {
                /*foreach (Image.Join join in elem.image.joins)
                    if (Math.Abs(join.x - e.X) < 3 && Math.Abs(join.y - e.Y) < 3)
                        MessageBox.Show("321");*/
                foreach (Image.Join join in elem.image.joins)
                {
                    if (Math.Abs(join.p.X + 1.0F - e.X) < 4.0F && Math.Abs(join.p.Y + 1.0F - e.Y) < 4.0F)
                    {
                        //MessageBox.Show(join.p.ToString() + this.Size.ToString());
                        Graphics gfx = Graphics.FromImage(this.Image);
                        join.tPaint(new Pen(Color.Red), gfx);
                        Paintjoin = join;
                        oldPos = new Point((int)join.p.X, (int)join.p.Y);
                        sendParent = true;
                    }
                }
            }
        }

        public void EventMouseClick(object sender, MouseEventArgs e)
        {
            Current = !Current;
            //MessageBox.Show("trololo");
            //Console.WriteLine(e.X);
            //set current
        }

        public void EventMouseEnter(object sender, EventArgs e)
        {
            isMouseEnter = true;
            if (!isDone)
                return;
            pen.Color = Color.MediumVioletRed;
            OnPaint(null);
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
            if (!Current)
                pen.Color = Color.Black;
            OnPaint(null);
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
            elem.Rotate(param);
            DefaultSize = new Size(elem.image.Width, elem.image.Height + 2);
            Size = DefaultSize;
            Refresh();
        }
    }
}
