using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Calculator
{
    public partial class WolframResult : Form
    {
        int loadingslide;

        public WolframResult()
        {
            InitializeComponent();
        }

        private void WolframResult_Load(object sender, EventArgs e)
        {
            WolframResult_Resize(sender, e);
            pictureLoadAnim.Image = Properties.Resources.anim_0;
            loadingslide = 0;
            timerAnim.Start();
        }

        private void WolframResult_Resize(object sender, EventArgs e)
        {
            pictureLoadAnim.Location = new Point(panelLoading.Width/2 - pictureLoadAnim.Width / 2, panelLoading.Height/2 - pictureLoadAnim.Height / 2);
        }

        private void timerAnim_Tick(object sender, EventArgs e)
        {
            loadingslide++;
            if (loadingslide > 74) loadingslide = 0;
            pictureLoadAnim.Image = Properties.Resources.ResourceManager.GetObject("anim_" + loadingslide.ToString()) as Image;
        }

        public void ShowAndCalculate(string query)
        {
            this.Show();
           
        }
    }
}
