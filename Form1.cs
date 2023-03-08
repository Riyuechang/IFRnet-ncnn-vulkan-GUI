using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace IFRnet
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Start_Click(object sender, EventArgs e)
        {
            string path = System.Environment.CurrentDirectory;//獲得當前路徑
            string inputVideoText = Input_video.Text;//儲存各項參數
            string outputVideoText = Output_video.Text;
            string scaleRatioText = ScaleRatio.Text;
            string speedText = Speed.Text;
            string outputVideoModeText = Output_video_mode.Text;
            string aiModeText = Ai_mode.Text;

            async Task IFRnet_Start()
            {
                Process process = new Process();

                process.StartInfo.FileName = "cmd.exe";//設定CMD
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardInput = true;//啟用輸入
                process.StartInfo.RedirectStandardOutput = true;//啟用標準輸出
                process.StartInfo.CreateNoWindow = true;//不顯示視窗
                process.Start();//啟動
                log.Invoke((Action)(() => log.Text += "Start\r\n"));
                process.StandardInput.WriteLine(path + "\\IFRnet\\ffmpeg\\bin\\ffprobe.exe -v error -count_frames -select_streams v:0 -show_entries stream=nb_read_frames -of default=nokey=1:noprint_wrappers=1 " + inputVideoText);//獲得影片張數
                process.StandardInput.WriteLine("exit");//結束CMD
                string[] lines = process.StandardOutput.ReadToEnd().Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);//將回傳值逐行寫入lines陣列
                process.WaitForExit();//等待CMD結束
                process.Close();//關閉

                process.StartInfo.FileName = path + "\\IFRnet\\ffmpeg\\bin\\ffmpeg.exe";//設定ffmpeg路徑
                process.StartInfo.Arguments = "-i " + inputVideoText + " " + path + "\\IFRnet\\_cache/%08d.png";//設定輸入輸出參數
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardError = true;//啟用錯誤輸出
                process.StartInfo.CreateNoWindow = true;//不顯示視窗

                process.ErrorDataReceived += (s, a) =>//讀錯誤輸出
                {
                    log.Invoke((Action)(() => log.Text += "\r\n" + a.Data));//同步讀取ffmpeg.exe回傳值,並輸出到log.Text
                    if (a.Data != null)//判斷回傳值是否回空
                    {
                        if (a.Data.Substring(0, 6) == "frame=")//前6位回傳值是否等於"frame="
                        {
                            float i = ((float.Parse(a.Data.Substring(8, a.Data.IndexOf("fps=") - 9)) / (float.Parse(lines[4]) + 2)) * 100);//分離出張數,並轉換成百分比
                            Planned_speed.Invoke((Action)(() => Planned_speed.Text = "Out Png:" + i.ToString("0.00") + "%"));//同步輸出進度到Planned_speed.Text
                        }
                    }
                };

                process.Start();//啟動
                process.BeginErrorReadLine();//將ffmpeg.exe重新導向到錯誤輸出流

                var tcs = new TaskCompletionSource<bool>();//等待外部程式結束
                process.Exited += (s, args) => tcs.SetResult(true);
                await tcs.Task;
            }

            Task.Run(async () =>//開始補幀
            {
                await IFRnet_Start();
            });
        }

        private void log_TextChanged(object sender, EventArgs e)
        {//log.Text自動捲到最下面
            log.SelectionStart = log.Text.Length;
            log.ScrollToCaret();
        }
    }
}
