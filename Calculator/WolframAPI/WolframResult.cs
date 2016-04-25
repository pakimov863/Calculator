using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WolframAlphaAPI;

namespace Calculator
{
    public partial class WolframResult : Form
    {
        delegate WolframAlphaQueryResult Delegate(WolframAlphaQuery WAQ);
        Delegate del;
        WolframAlphaEngine Engine;
        int loadingslide;

        public WolframResult()
        {
            InitializeComponent();
        }

        private void WolframResult_Load(object sender, EventArgs e)
        {
            panelLoading.Visible = true;
            panelContent.Visible = false;
            WolframResult_Resize(sender, e);
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

        public void ShowAndCalculate(string query, string appkey)
        {
            this.Show();
            Properties.Settings.Default.WA_RespSend++;
            Properties.Settings.Default.Save();
            Engine = new WolframAlphaEngine(appkey);//KLQJ9W-5UU8EGQUG3
            WolframAlphaQuery WAQ = new WolframAlphaQuery();
            WAQ.APIKey = appkey;
            WAQ.MoreOutput = false;
            WAQ.Asynchronous = false;
            WAQ.AllowCaching = false;
            WAQ.Query = query;
            WAQ.TimeLimit = 30000;
            WAQ.Format = WolframAlphaQueryFormat.Image + "," + WolframAlphaQueryFormat.PlainText;

            pictureLoadAnim.Image = Properties.Resources.anim_0;
            loadingslide = 0;
            timerAnim.Start();
            del = new Delegate(Engine.LoadResponse);
            del.BeginInvoke(WAQ, new AsyncCallback(CallBackFunc), null);
        }

        private void CallBackFunc(IAsyncResult aRes)
        {
            del.EndInvoke(aRes);
            this.BeginInvoke((MethodInvoker)(() =>
            {
                ShowResult(this.Engine.QueryResult);
            }));
        }

        private void ShowResult(WolframAlphaQueryResult result)
        {
            //textBoxOutput.AppendText("Детали ответа\r\n");
            //textBoxOutput.AppendText("Ветвей найдено: " + result.NumberOfPods + "\r\n");
            //textBoxOutput.AppendText("Время разбора: " + result.ParseTiming + " сек.\r\n");
            //textBoxOutput.AppendText("Время выполнения: " + result.Timing + " сек.\r\n");
            //textBoxOutput.AppendText("=============================\r\n");
            textBoxOutput.Clear();

            labelPodsFound.Text = result.NumberOfPods.ToString();
            labelExecTime.Text = result.ParseTiming + "/" + result.Timing;

            Int32 PodNumber = 1;

            foreach (WolframAlphaPod Item in result.Pods)
            {
                textBoxOutput.AppendText("\r\n");

                textBoxOutput.AppendText("Ветвь " + PodNumber + "\r\n");

                textBoxOutput.AppendText("Саб-ветвей: " + Item.NumberOfSubPods + "\r\n");
                textBoxOutput.AppendText("Заголовок: \"" + Item.Title + "\"\r\n");
                textBoxOutput.AppendText("Позиция: " + Item.Position + "\r\n");

                Int32 SubPodNumber = 1;

                foreach (WolframAlphaSubPod SubItem in Item.SubPods)
                {
                    textBoxOutput.AppendText("\r\n");

                    textBoxOutput.AppendText(">Саб-ветвь " + SubPodNumber + "\r\n");
                    textBoxOutput.AppendText(">Заголовок: \"" + SubItem.Title + "\"\r\n");
                    textBoxOutput.AppendText(">Содержимое: \"" + SubItem.PodText + "\"\r\n");
                    if (SubItem.PodImage != null)
                    {
                        textBoxOutput.AppendText(">>Заголовок изображения: \"" + SubItem.PodImage.Title + "\"\r\n");
                        textBoxOutput.AppendText(">>Ширина изображения: " + SubItem.PodImage.Width + "\r\n");
                        textBoxOutput.AppendText(">>Высота изображения: " + SubItem.PodImage.Height + "\r\n");
                        textBoxOutput.AppendText(">>Путь к изображению: \"" + SubItem.PodImage.Location.ToString() + "\"\r\n");
                        textBoxOutput.AppendText(">>Описание к изображению: \"" + SubItem.PodImage.HoverText + "\"\r\n");
                    }

                    SubPodNumber += 1;
                }
                PodNumber += 1;
            }
            panelContent.Visible = true;
            panelLoading.Visible = false;
        }
    }
}
