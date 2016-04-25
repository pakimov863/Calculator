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
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
            RefreshFields();
        }

        private void RefreshFields()
        {
            checkBox1.Checked = Properties.Settings.Default.AutoALT;
            foreach(string str in Properties.Settings.Default.ShowPanels.Split(','))
            {
                if (Convert.ToInt32(str) < checkedListBox1.Items.Count)
                    checkedListBox1.SetItemChecked(Convert.ToInt32(str),true);
            }
            if(Properties.Settings.Default.StartPanelLast)
                checkedListBox2.SetItemChecked(checkedListBox2.Items.Count - 1, true);
            else
                checkedListBox2.SetItemChecked(Properties.Settings.Default.StartPanel, true);
            checkedListBox3.SetItemChecked(Properties.Settings.Default.HistoryMode, true);
            checkedListBox4.SetItemChecked(Properties.Settings.Default.PlotSide, true);

            textBox1.Text = Properties.Settings.Default.WA_AppKey;
            textBox2.Text = Properties.Settings.Default.WA_RespSend.ToString() + "/" + Properties.Settings.Default.WA_RespLimit.ToString();
            checkBox3.Checked = Properties.Settings.Default.WA_RespWipe;
            checkBox4.Checked = Properties.Settings.Default.WA_LoadImages ;
            checkBox5.Checked = Properties.Settings.Default.WA_UseCache;
            textBox3.Text = Properties.Settings.Default.WA_CacheSize.ToString();
        }

        private void checkedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            var list = sender as CheckedListBox;
            if (e.NewValue == CheckState.Checked)
                foreach (int index in list.CheckedIndices)
                {
                    list.SetSelected(index, false);
                    if (index != e.Index)
                        list.SetItemChecked(index, false);
                }
        }

        private void buttonSetDefault_Click(object sender, EventArgs e)
        {
            checkBox1.Checked = false;
            checkedListBox1.SetItemChecked(0, true);
            checkedListBox1.SetItemChecked(1, true);
            checkedListBox1.SetItemChecked(2, true);
            checkedListBox1.SetItemChecked(3, true);
            checkedListBox1.SetItemChecked(4, true);
            checkedListBox2.SetItemChecked(0, true);
            checkedListBox3.SetItemChecked(0, true);
            checkedListBox4.SetItemChecked(0, true); //TopLeft=0, BotLeft=1, TopRight=2, BotRight=3

            textBox1.Text = "";
            textBox2.Text = "0/0";
            checkBox3.Checked = true;
            checkBox4.Checked = false;
            checkBox5.Checked = false;
            textBox3.Text = "";
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.AutoALT = checkBox1.Checked;
            string str = "";
            foreach (int value in checkedListBox1.CheckedIndices )
            {
                str += value.ToString() + ",";
            }
            Properties.Settings.Default.ShowPanels = str.Substring(0, str.Length - 1);
            if (checkedListBox2.GetItemChecked(checkedListBox2.Items.Count - 1))
            {
                Properties.Settings.Default.StartPanelLast = true;
                Properties.Settings.Default.StartPanel = 0;
            }
            else
            {
                Properties.Settings.Default.StartPanel = checkedListBox2.CheckedIndices[0];
            }
            Properties.Settings.Default.HistoryMode = checkedListBox3.CheckedIndices[0];
            Properties.Settings.Default.PlotSide = checkedListBox4.CheckedIndices[0];

            Properties.Settings.Default.WA_AppKey = textBox1.Text;
            string[] tstr = textBox2.Text.Split(new char[] {'/'});
            Properties.Settings.Default.WA_RespSend = Convert.ToInt32(tstr[0]);
            Properties.Settings.Default.WA_RespLimit = Convert.ToInt32(tstr[1]);
            Properties.Settings.Default.WA_RespWipe = checkBox3.Checked;
            Properties.Settings.Default.WA_LoadImages = checkBox4.Checked;
            Properties.Settings.Default.WA_UseCache = checkBox5.Checked;
            Properties.Settings.Default.WA_CacheSize = Convert.ToDouble(textBox3.Text);

            Properties.Settings.Default.Save();
            MessageBox.Show("Для применения настроек перезапустите программу", "Настройки", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            textBox2.Enabled = checkBox2.Checked;
        }
    }
}
