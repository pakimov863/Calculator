using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Calculator
{
    public partial class WolframPod : UserControl
    {
        public WolframPod()
        {
            InitializeComponent();
        }

        public WolframPod(string title)
        {
            InitializeComponent();

            this.Height = 22;
            label_title.Text = title;
            label_title.ForeColor = Color.SteelBlue;
            this.BackColor  = Color.Gainsboro;
        }

        public WolframPod(string title, string content)
        {
            InitializeComponent();

            if (title !="") label_title.Text = title;
            else
            {
                label_title.Visible = false;
                pictureBox1.Visible = false;
                label_content.Location = new Point(label_content.Location.X, 5);
                this.Height = label_content.Height + 10;
            }
            label_content.Text = content;
        }

        public WolframPod(string title, string content, string imageurl)
        {
            InitializeComponent();

            if (title != "") label_title.Text = title;
            else
            {
                label_title.Visible = false;
                label_content.Location = new Point(label_content.Location.X, 5);
                pictureBox1.Location = new Point(pictureBox1.Location.X, label_content.Location.Y + label_content.Height + 9);
                this.Height = label_content.Height + 10;
            }

            label_content.Text = content;

            if (imageurl != "")
            {
                pictureBox1.Load(imageurl);
                pictureBox1.Invalidate();
                pictureBox1.Location = new Point(pictureBox1.Location.X, label_content.Location.Y + label_content.Height +9);
                this.Height = pictureBox1.Location.X + pictureBox1.Height + 10;
            }
            else
            {
                pictureBox1.Visible = false;
            }
        }

        public WolframPod(string title, string content, bool err)
        {
            InitializeComponent();
            if(err)
                this.BackColor = Color.LightCoral;
            else
                this.BackColor = Color.LightBlue;
            this.Height = 48;
            label_title.Text = title;
            label_content.Text = content;
        }

        private void label_content_DoubleClick(object sender, EventArgs e)
        {
            Clipboard.SetText(label_content.Text); 
        }
    }
}
