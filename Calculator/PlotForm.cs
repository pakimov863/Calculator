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
    public partial class PlotForm : Form
    {
        public PlotForm()
        {
            InitializeComponent();
            cx = -pictureBox1.Width / 2-10;
            cy = pictureBox1.Height / 2-10;
        }

        private Dictionary<string, PlotParams> Plots;
        private int lx = 0, ly = 0;
        private int cx = 0, cy = 0;
        private double plotmultiplier = 1;
        private byte plotside = 0;

        public void AddPlot(string expression)
        {
            if (Plots == null) Plots = new Dictionary<string, PlotParams>();

            if (Plots.Count > 16) MessageBox.Show("Нельзя добавлять больше 16 графиков", "Невозможно добавить график", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                if (expression != "") Plots[expression] = new PlotParams(GraphArithmetic.GetPoints(expression, cx, cx + pictureBox1.Width), Color.AliceBlue);
            }

            RefreshList();
            pictureBox1.Invalidate();
        }

        private void RefreshList()
        {
            int plotseparate = 15;
            double plotmult = plotseparate * plotmultiplier;
            if (plotmult == 0) plotmult = plotseparate;

            checkedListBox1.Items.Clear();
            foreach (var item in Plots)
            {
                Plots[item.Key].Points = GraphArithmetic.GetPoints(item.Key, cx / plotmult, (cx + pictureBox1.Width) / plotmult, 0.5 / plotmultiplier);
                checkedListBox1.Items.Add(item.Key);
            }
        }
        
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            lx = e.X;
            ly = e.Y;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (pictureBox1.Capture)
            {
                //int dx = e.X - lx;
                //int dy = e.Y - ly;
                int dx = 0, dy = 0;
                if (e.X > 20 && e.Y < 20)
                    dx = e.X - lx;
                else if (e.X < 20 && e.Y > 20)
                    dy = e.Y - ly;
                else if (e.X > 20 && e.Y > 20)
                {
                    dx = e.X - lx;
                    dy = e.Y - ly;
                }
                cx -= dx;
                cy += dy;
                lx = e.X;
                ly = e.Y;
                pictureBox1.Invalidate();
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Bitmap IMAGE = GraphArithmetic.GetPlot("", cx, cx + pictureBox1.Width, cy, cy - pictureBox1.Height, plotmultiplier,plotside );
            RefreshList();
            foreach (var item in Plots)
            {
                GraphArithmetic.DrawPlot(IMAGE, item.Value.Points, cx, cx + pictureBox1.Width, cy, cy - pictureBox1.Height, plotmultiplier, plotside);
            }
            e.Graphics.DrawImageUnscaled(IMAGE, 0, 0);
        }

        private void PlotForm_Resize(object sender, EventArgs e)
        {
            cx = -pictureBox1.Width / 2-10;
            cy = pictureBox1.Height / 2-10;
            pictureBox1.Invalidate();
        }

        private void buttonZoomNorm_Click(object sender, EventArgs e)
        {
            cx = -pictureBox1.Width / 2;
            cy = pictureBox1.Height / 2;
            plotmultiplier = 1;
            pictureBox1.Invalidate();
        }

        private void buttonZoomMinus_Click(object sender, EventArgs e)
        {
            plotmultiplier /= 2;
            if (plotmultiplier == 0) plotmultiplier = -1;
            pictureBox1.Invalidate();
        }

        private void buttonZoomPlus_Click(object sender, EventArgs e)
        {
            plotmultiplier *= 2;
            if (plotmultiplier == 0) plotmultiplier = 1;
            pictureBox1.Invalidate();
        }

        private void PlotForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Dispose();
        }

        private void buttonDeleteRefresh_Click(object sender, EventArgs e)
        {
            foreach (string item in checkedListBox1.CheckedItems) Plots.Remove(item);
            RefreshList();
        }
        
    }

}
