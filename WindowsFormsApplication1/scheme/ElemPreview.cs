using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace MainWindow.scheme
{
    public partial class ElemPreview : Control, ISupportInitialize
    {
        private PictureBox pictureBox;
        private ToolStrip toolStrip;
        private ToolStripButton toolStripButton1;
        private ToolStripButton toolStripButton2;
        private ToolStripButton toolStripButton3;
        private ToolStripButton toolStripButton4;
        private ElemPictureBox epb;

        public ElemPreview()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ElemPreview));
            toolStripButton1 = new ToolStripButton();
            toolStripButton2 = new ToolStripButton();
            toolStripButton3 = new ToolStripButton();
            toolStripButton4 = new ToolStripButton();
            toolStrip = new ToolStrip();
            pictureBox = new PictureBox();
            /*((System.ComponentModel.ISupportInitialize)toolStripButton1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)toolStripButton2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)toolStripButton3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)toolStripButton4).BeginInit();
            ((System.ComponentModel.ISupportInitialize)toolStrip).BeginInit();*/
            ((System.ComponentModel.ISupportInitialize)pictureBox).BeginInit();
            pictureBox.SuspendLayout();
            toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripButton1
            // 
            toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("object-rotate-right.Image")));
            toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButton1.Name = "toolStripButton1";
            toolStripButton1.Size = new System.Drawing.Size(21, 20);
            toolStripButton1.Text = "Повернуть по часовой";
            toolStripButton1.Click += new EventHandler(toolStripButton1_Click);
            //this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton3_Click);
            // 
            // toolStripButton2
            // 
            toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("object-rotate-left.Image")));
            //toolStripButton2.Image = new Size(50, 50);
            toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButton2.Name = "toolStripButton2";
            toolStripButton2.Size = new System.Drawing.Size(21, 20);
            toolStripButton2.Text = "Повернуть против часовой";
            toolStripButton2.Click += new EventHandler(toolStripButton2_Click);
            //this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton4_Click);
            // 
            // toolStripButton3
            // 
            toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButton3.Image = ((System.Drawing.Image)(resources.GetObject("object-flip-horizontal.Image")));
            toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButton3.Name = "toolStripButton3";
            toolStripButton3.Size = new System.Drawing.Size(21, 20);
            toolStripButton3.Text = "Отразить по горизонтали";
            // 
            // toolStripButton4
            // 
            toolStripButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButton4.Image = ((System.Drawing.Image)(resources.GetObject("object-flip-vertical.Image")));
            toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButton4.Name = "toolStripButton4";
            toolStripButton4.Size = new System.Drawing.Size(21, 20);
            toolStripButton4.Text = "Отразить по вертикали";
            // 
            // toolStrip
            // 
            this.toolStrip.Dock = System.Windows.Forms.DockStyle.Left;
            this.toolStrip.ImageScalingSize = new Size(24, 24);
            this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripButton2,
            this.toolStripButton3,
            this.toolStripButton4});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.MaximumSize = new System.Drawing.Size(32, 130);
            this.toolStrip.MinimumSize = new System.Drawing.Size(32, 130);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(32, 130);
            this.toolStrip.TabIndex = 2;
            this.toolStrip.Text = "toolStrip";
            
            // 
            // pictureBox
            // 
            this.pictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox.BackColor = System.Drawing.SystemColors.ControlText;
            this.pictureBox.Location = new System.Drawing.Point(32, 0);
            this.pictureBox.MinimumSize = new System.Drawing.Size(100, 100);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(100, 100);
            this.pictureBox.TabIndex = 1;
            this.pictureBox.TabStop = false;
            //this.pictureBox.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);

            /*Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));*/
            /*((System.ComponentModel.ISupportInitialize)toolStripButton1).EndInit();
            ((System.ComponentModel.ISupportInitialize)toolStripButton2).EndInit();
            ((System.ComponentModel.ISupportInitialize)toolStripButton3).EndInit();
            ((System.ComponentModel.ISupportInitialize)toolStripButton4).EndInit();
            ((System.ComponentModel.ISupportInitialize)toolStrip).EndInit();*/
            Controls.Add(toolStrip);
            Controls.Add(pictureBox); 
            ((System.ComponentModel.ISupportInitialize)pictureBox).EndInit();
            pictureBox.ResumeLayout(false);
            pictureBox.PerformLayout();
            toolStrip.ResumeLayout(false);
            toolStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (epb == null || epb.Disposing)
                return;
            epb.Rotate(1);
            epb.Location = new Point((this.Width - 32 - epb.Width) / 2, (this.Height - epb.Height) / 2);
            Refresh();
        }

        void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (epb == null || epb.Disposing)
                return;
            epb.Rotate(2);
            epb.Location = new Point((this.Width - 32 - epb.Width) / 2, (this.Height - epb.Height) / 2);
            Refresh();
        }

        void ISupportInitialize.BeginInit()
        {
            //throw new NotImplementedException();
        }

        void ISupportInitialize.EndInit()
        {
            //throw new NotImplementedException();
        }

        public void setElement(Elems el)
        {
            if (epb != null && !epb.Disposing)
                epb.Dispose();
            epb = new ElemPictureBox(el);
            pictureBox.Controls.Add(epb);
            //epb.setLocation(new Point((this.Width / 2 - epb.Width / 2) / 1, (this.Height - epb.Height) / 2));
            epb.Location = new Point((this.Width - 32 - epb.Width) / 2, (this.Height - epb.Height) / 2);
        }

        public void reset()
        {
            if (epb != null && !epb.Disposing)
                epb.Dispose();
            Refresh();
        }
    }
}
