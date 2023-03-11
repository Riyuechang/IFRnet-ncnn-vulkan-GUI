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
using Microsoft.WindowsAPICodePack.Dialogs;

namespace IFRnet
{
    public partial class Form1 : Form
    {
        //設定常數
        const string ffprobe = "\\IFRnet\\ffmpeg\\bin\\ffprobe.exe";//ffprobe路徑
        const string ffmpeg = "\\IFRnet\\ffmpeg\\bin\\ffmpeg.exe";//ffmpeg路徑
        const string ifrnet = "\\IFRnet\\ifrnet-ncnn-vulkan.exe";
        const string _cache = "\\IFRnet\\_cache\\";//暫存1路徑
        const string _cache2 = "\\IFRnet\\_cache\\_cache\\";//暫存2路徑

        float frame = 0;//幀數
        float nowFrame = 0;//及時幀數
        float pngNum = 0;//目標幀數

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer.Start();
            ScaleRatio.SelectedIndex = 0;
            Speed.SelectedIndex = 0;
            Output_video_mode.SelectedIndex = 0;
            Ai_mode.SelectedIndex = 0;
        }

        bool state = false;//開始狀態
        string stageState = "";//補幀狀態
        int stateTime = 0;//開始時間
        int stageTime = 0;//補幀開始時間
        float Fps = 0;//FPS
        float frameRate = 0;//幀率
        string change = "";//變化
        string change2 = "";//變化2
        string change3 = "";//變化3
        private void timer_Tick(object sender, EventArgs e)
        {
            string path = System.Environment.CurrentDirectory;//獲得當前路徑
            string inputVideoText = Input_video.Text;//儲存各項參數
            string inputFpsText = Input_fps.Text;
            string scaleRatioText = ScaleRatio.Text;

            if (change != inputVideoText)//判斷影片輸入是否有變化,有變化就更新影片幀率
            {
                Process process = new Process();

                //讀取影片幀率
                Process FFprobe_frame_rate = new Process();

                FFprobe_frame_rate.StartInfo.UseShellExecute = false;
                FFprobe_frame_rate.StartInfo.RedirectStandardOutput = true;//啟用錯誤輸出
                FFprobe_frame_rate.StartInfo.CreateNoWindow = true;//不顯示視窗

                FFprobe_frame_rate.StartInfo.FileName = path + ffprobe;//設定ffprobe路徑
                FFprobe_frame_rate.StartInfo.Arguments = "-v quiet -select_streams v:0 -show_entries stream=r_frame_rate -of csv=p=0 " + "\"" + inputVideoText + "\"";//設定讀取參數

                FFprobe_frame_rate.OutputDataReceived += (s, a) =>//讀取錯誤輸出
                {
                    if (a.Data != null)//判斷回傳值是否回空
                    {
                        frameRate = float.Parse(a.Data.Substring(0, a.Data.IndexOf("/"))) / float.Parse(a.Data.Substring(a.Data.IndexOf("/") + 1, a.Data.Length - a.Data.IndexOf("/") - 1));//將回傳值轉換成幀率,並儲存在frameRate
                    }
                };

                FFprobe_frame_rate.Start();//啟動
                FFprobe_frame_rate.BeginOutputReadLine();//將ffprobe.exe重新導向到錯誤輸出流
                FFprobe_frame_rate.WaitForExit();//等待ffprobe.exe結束
                FFprobe_frame_rate.Close();//關閉

                Input_fps.Text = frameRate.ToString();//輸出幀率到Input_fps.Text
                change = inputVideoText;//更新change
            }

            if (change2 != inputFpsText || change3 != scaleRatioText)//檢測輸入幀率或補幀倍率是否發生變化
            {
                Output_fps.Text = (float.Parse(inputFpsText) * float.Parse(scaleRatioText)).ToString();//更新輸出幀率
                change2 = inputFpsText;//更新變化2為新的值
                change3 = scaleRatioText;//更新變化3為新的值
            }

            if (state == true)//檢測是否開始
            {
                int elapsedTime = (Environment.TickCount - stateTime) / 1000;//拿掉毫秒
                int timeS = elapsedTime % 60;//分離出秒
                int timeM = elapsedTime % 3600 / 60;//分離出分
                int timeH = elapsedTime / 3600;//分離出小時
                Time.Text = timeH + ":" + timeM.ToString().PadLeft(2,'0') + ":" + timeS.ToString().PadLeft(2, '0');//輸出經過時間
            }

            float num = 0;//幀數
            float numTotal = 0;//總幀數
            int stageElapsedTime = (Environment.TickCount - stageTime) / 1000;//階段經過時間,拿掉毫秒
            switch (stageState)//檢測階段,計算百分比
            {
                case "Out png:":
                    numTotal = frame;
                    num = nowFrame;//將及時幀數寫進num
                    float a = (nowFrame / frame) * 100;//分離出幀數,並轉換成百分比
                    Planned_speed.Text = stageState + a.ToString("0.00") + "%";//同步輸出進度到Planned_speed.Text
                    break;
                case "IFRnet:":
                    numTotal = pngNum;
                    num = (float)Directory.GetFiles(path + _cache2).Length;//暫存2 png數
                    Planned_speed.Text = stageState + ((Directory.GetFiles(path + _cache2).Length / pngNum) * 100).ToString("0.00") + "%";//輸出百分比到Planned_speed.Text
                    break;
                case "Out video:":
                    numTotal = pngNum;
                    num = nowFrame;//將及時幀數寫進num
                    float b = (nowFrame / pngNum) * 100;//分離出幀數,並轉換成百分比
                    Planned_speed.Text = stageState + b.ToString("0.00") + "%";//同步輸出進度到Planned_speed.Text
                    break;
                case "audio:":
                    numTotal = pngNum;
                    num = nowFrame;//將及時幀數寫進num
                    float c = (nowFrame / pngNum) * 100;//分離出幀數,並轉換成百分比
                    Planned_speed.Text = stageState + c.ToString("0.00") + "%";//同步輸出進度到Planned_speed.Text
                    break;
                default:
                    Planned_speed.Text = stageState;
                    break;
            }

            if (num != 0)//FPS
            {
                Fps = num / (float)stageElapsedTime;//計算FPS
                FPS.Text = Fps.ToString("0.00") + "FPS";//輸出FPS
            }

            if (numTotal != 0 && num != 0)//剩餘時間
            {
                int timeLeft = (int)((numTotal - num) / Fps);//計算剩餘時間
                int timeLeftS = timeLeft % 60;//分離出秒
                int timeLeftM = timeLeft % 3600 / 60;//分離出分
                int timeLeftH = timeLeft / 3600;//分離出小時
                Time_left.Text = timeLeftH + ":" + timeLeftM.ToString().PadLeft(2, '0') + ":" + timeLeftS.ToString().PadLeft(2, '0');//輸出剩餘時間
            }
        }

        private void Start_Click(object sender, EventArgs e)
        {
            string path = System.Environment.CurrentDirectory;//獲得當前路徑
            string inputVideoText = Input_video.Text;//儲存各項參數
            string outputVideoText = Output_video.Text;
            string scaleRatioText = ScaleRatio.Text;
            string speedText = Speed.Text;
            string outputFpsText = Output_fps.Text;
            string outputVideoModeText = Output_video_mode.Text;
            string aiModeText = Ai_mode.Text;
            string videoName = inputVideoText.Substring(inputVideoText.LastIndexOf("\\") + 1, inputVideoText.LastIndexOf(".") - inputVideoText.LastIndexOf("\\") - 1);//儲存影片名稱
            if (outputVideoText.LastIndexOf("\\") + 1 != outputVideoText.Length)//檢查outputVideoText最後一個字元是否為"\",不是就補上"\"
                outputVideoText += "\\";

            //開始計時
            stateTime = Environment.TickCount;//紀錄開始時間
            state = true;//設定狀態

            //檢查暫存是否為空
            if (Directory.Exists(path + _cache) == true)
                Directory.Delete(path + _cache, true);//清除_cache資料夾和裡面所有的東西

            //建立暫存資料夾
            Directory.CreateDirectory(path + _cache2);//創建_cache和裡面的子資料夾_cache

            async Task IFRnet_Start()
            {
                Process process = new Process();

                //讀取影片幀數
                Process FFprobe_format_duration = new Process();

                FFprobe_format_duration.StartInfo.UseShellExecute = false;
                FFprobe_format_duration.StartInfo.RedirectStandardOutput = true;//啟用錯誤輸出
                FFprobe_format_duration.StartInfo.CreateNoWindow = true;//不顯示視窗

                FFprobe_format_duration.StartInfo.FileName = path + ffprobe;//設定ffprobe路徑
                FFprobe_format_duration.StartInfo.Arguments = "-v quiet -select_streams v:0 -show_entries stream=nb_frames -of csv=p=0 " + "\"" + inputVideoText + "\"";//設定讀取參數

                FFprobe_format_duration.OutputDataReceived += (s, a) =>//讀取錯誤輸出
                {
                    if (a.Data != null)//判斷回傳值是否回空
                    {
                        frame = float.Parse(a.Data);//將幀數儲存在frame
                    }
                };

                FFprobe_format_duration.Start();//啟動
                FFprobe_format_duration.BeginOutputReadLine();//將ffprobe.exe重新導向到錯誤輸出流
                FFprobe_format_duration.WaitForExit();//等待ffprobe.exe結束
                FFprobe_format_duration.Close();//關閉

                //開始將影片轉png
                stageState = "Out png:";//設定階段狀態
                stageTime = Environment.TickCount;//紀錄轉換時間
                nowFrame = 0;

                //把影片轉成png
                Process FFmpeg_png = new Process();

                FFmpeg_png.StartInfo.UseShellExecute = false;
                FFmpeg_png.StartInfo.RedirectStandardError = true;//啟用錯誤輸出
                FFmpeg_png.StartInfo.CreateNoWindow = true;//不顯示視窗

                FFmpeg_png.StartInfo.FileName = path + ffmpeg;//設定ffmpeg路徑
                FFmpeg_png.StartInfo.Arguments = "-i " + "\"" + inputVideoText + "\"" + " \"" + path + _cache + "/%08d.png\"";//設定輸入輸出參數

                FFmpeg_png.ErrorDataReceived += (s, a) =>//讀取錯誤輸出
                {
                    log.Invoke((Action)(() => log.Text += "\r\n" + a.Data));//同步讀取ffmpeg.exe回傳值,並輸出到log.Text
                    if (a.Data != null)//判斷回傳值是否回空
                    {
                        if (a.Data.Substring(0, 6) == "frame=")//前6位回傳值是否等於"frame="
                        {
                            nowFrame = float.Parse(a.Data.Substring(6, a.Data.IndexOf("fps=") - 7));//解析幀數
                        }
                    }
                };

                FFmpeg_png.Start();//啟動
                FFmpeg_png.BeginErrorReadLine();//將ffmpeg.exe重新導向到錯誤輸出流
                FFmpeg_png.WaitForExit();//等待ffmpeg.exe結束
                FFmpeg_png.Close();//關閉

                //當補幀倍率為2.5或7.5,並且暫存1 png數量是單數,將暫存1 png數量+1
                int fileNum = Directory.GetFiles(path + _cache).Length;//讀取暫存1 png的數量到fileNum
                ScaleRatio.Invoke((Action)(() =>
                {
                    if (ScaleRatio.Text == "2.5" || ScaleRatio.Text == "7.5")//補幀倍率是否為2.5或7.5
                    {
                        if (fileNum % 2 == 1)//如果png的數量為單數
                        {
                            File.Copy(path + _cache + fileNum.ToString().PadLeft(8, '0') + ".png", path + _cache + (fileNum + 1).ToString().PadLeft(8, '0') + ".png");//複製一個與最後一張png相同的png,並編號+1
                            fileNum++;//fileNum + 1
                        }
                    }
                }));

                ScaleRatio.Invoke((Action)(() => pngNum =fileNum * float.Parse(ScaleRatio.Text)));//目前幀數*補幀倍率=目標幀數

                //開始補幀
                stageState = "IFRnet:";//設定階段狀態
                stageTime = Environment.TickCount;//紀錄補幀時間

                string gpuLevel = "";
                log.Invoke((Action)(() => gpuLevel = " -j " + GPU_level.Text + ":" + GPU_level.Text + ":" + GPU_level.Text));//設定GPU負載等級

                string tta = "";
                if (TTA.Checked)//設定TTA
                    tta = " -x";

                string uhd = "";
                if (UHD.Checked)//設定UHD
                    uhd = " -u";

                //開始補幀
                Process IFRnet = new Process();

                IFRnet.StartInfo.UseShellExecute = false;
                IFRnet.StartInfo.CreateNoWindow = true;//不顯示視窗

                IFRnet.StartInfo.FileName = path + ifrnet;//設定ifrnet-ncnn-vulkan.exe路徑
                IFRnet.StartInfo.Arguments = "-m " + aiModeText + " -n " + pngNum + gpuLevel + tta + uhd +" -i " + path + _cache + " -o " + path + _cache2;//設定輸入輸出參數
                log.Invoke((Action)(() => log.Text += "\r\n-m " + aiModeText + " -n " + pngNum + gpuLevel + tta + uhd + " -i " + path + _cache + " -o " + path + _cache2));
                IFRnet.Start();//啟動
                while (Directory.GetFiles(path + _cache2).Length != pngNum) //檢測暫存2 png數是否等於目標幀數
                {
                    log.Invoke((Action)(() => log.Text += "\r\nframe=" + Directory.GetFiles(path + _cache2).Length + "/" + pngNum));//輸出當前幀數到log.Text
                    Thread.Sleep(500);
                }

                //補幀結束直接輸出最終數值
                log.Invoke((Action)(() => log.Text += "\r\nframe=" + pngNum));
                Planned_speed.Invoke((Action)(() => Planned_speed.Text = "IFRnet:100%"));

                IFRnet.WaitForExit();//等待ifrnet-ncnn-vulkan.exe結束
                IFRnet.Close();//關閉

                //紀錄補幀時間和FPS
                int ifrnetResultTime = (Environment.TickCount - stageTime) / 1000;//拿掉毫秒
                string ifrnetResultFps = FPS.Text;

                //輸出幀率放慢倍率
                int fpsRatio = 0;//FPS倍率
                switch (speedText)//判斷Output_fps.Text內的倍率
                {
                    case "Normal Speed":
                        fpsRatio = 1;
                        break;
                    case "x2 Slowmo":
                        fpsRatio = 2;
                        break;
                    case "x3 Slowmo":
                        fpsRatio = 3;
                        break;
                    case "x4 Slowmo":
                        fpsRatio = 4;
                        break;
                    case "x5 Slowmo":
                        fpsRatio = 5;
                        break;
                    case "x6 Slowmo":
                        fpsRatio = 6;
                        break;
                    case "x7 Slowmo":
                        fpsRatio = 7;
                        break;
                    case "x8 Slowmo":
                        fpsRatio = 8;
                        break;
                    case "x9 Slowmo":
                        fpsRatio = 9;
                        break;
                    case "x10 Slowmo":
                        fpsRatio = 10;
                        break;
                }
                float finalFps = float.Parse(outputFpsText) / fpsRatio;//最終幀率

                //給影片增加編號
                string videoNum = "";//輸出檔案編號
                if (File.Exists(outputVideoText + videoName + "_x" + scaleRatioText + "_" + finalFps + "fps_" + aiModeText + "." + outputVideoModeText) == true)//檢測輸出資料夾是否存在同名檔案
                {
                    for (int i = 1; ; i++)//循環檢測其他編號
                    {
                        if (File.Exists(outputVideoText + videoName + "_x" + scaleRatioText + "_" + finalFps + "fps_" + aiModeText + "_" + i + "." + outputVideoModeText) != true)//檢測是否有其他編號
                        {
                            videoNum = "_" + i;//設定檔案編號
                            break;//跳出迴圈
                        }
                    }
                }
                string videoNameOut = videoName + "_x" + scaleRatioText + "_" + finalFps + "fps_" + aiModeText + videoNum + "." + outputVideoModeText;

                //檢測並設定影片格式
                string audio = "";
                switch (outputVideoModeText)
                {
                    case "MP4":
                        audio = ".m4a";
                        break;
                    case "MKV":
                        audio = ".mka";
                        break;
                    case "AVI":
                        audio = ".mp3";
                        break;
                }

                //從影片分離音訊檔
                string outputPath = "";
                string audioParameter = "";//音訊參數
                bool checkAudio = false;
                if (Audio_switch.Checked)//檢測是否啟用音訊
                {
                    Process FFmpeg_audio = new Process();

                    FFmpeg_audio.StartInfo.UseShellExecute = false;
                    FFmpeg_audio.StartInfo.RedirectStandardError = true;//啟用錯誤輸出
                    FFmpeg_audio.StartInfo.CreateNoWindow = true;//不顯示視窗

                    FFmpeg_audio.StartInfo.FileName = path + ffmpeg;//設定ffmpeg路徑
                    FFmpeg_audio.StartInfo.Arguments = "-i " + "\"" + inputVideoText + "\"" + " -vn -acodec copy \"" + path + _cache + "audio" + audio + "\"";//設定輸入輸出參數

                    FFmpeg_audio.ErrorDataReceived += (s, a) =>//讀取錯誤輸出
                    {
                        log.Invoke((Action)(() => log.Text += "\r\n" + a.Data));//同步讀取ffmpeg.exe回傳值,並輸出到log.Text
                    };

                    FFmpeg_audio.Start();//啟動
                    FFmpeg_audio.BeginErrorReadLine();//將ffmpeg.exe重新導向到錯誤輸出流
                    FFmpeg_audio.WaitForExit();//等待ffmpeg.exe結束
                    FFmpeg_audio.Close();//關閉

                    checkAudio = File.Exists(path + _cache + "audio" + audio);//檢查音訊檔
                    if (checkAudio == true)//檢測是否分離出音訊檔
                        audioParameter = " -i \"" + path + _cache + "audio" + audio + "\" -c:a copy";//設定音訊參數
                }

                if (Audio_switch.Checked && checkAudio && speedText != "Normal Speed")//如果音訊啟用且有音訊且放慢倍率不是Normal Speed,就把檔案輸出路徑改到暫存1
                    outputPath = path + _cache;
                else
                    outputPath = outputVideoText;

                //開始將png轉影片
                stageState = "Out video:";//設定階段狀態
                stageTime = Environment.TickCount;//紀錄轉換時間
                nowFrame = 0;

                //將補幀完的png用ffmpeg合成影片
                Process FFmpeg_video = new Process();

                FFmpeg_video.StartInfo.UseShellExecute = false;
                FFmpeg_video.StartInfo.RedirectStandardError = true;//啟用錯誤輸出
                FFmpeg_video.StartInfo.CreateNoWindow = true;//不顯示視窗

                FFmpeg_video.StartInfo.FileName = path + ffmpeg;//設定ffmpeg路徑
                FFmpeg_video.StartInfo.Arguments = " -framerate " + finalFps + " -i \"" + path + _cache2 + "/%08d.png\"" + audioParameter + " -crf 20 -c:v libx264 -pix_fmt yuv420p " + "\"" + outputPath + videoNameOut + "\"";//設定輸入輸出參數

                FFmpeg_video.ErrorDataReceived += (s, a) =>//讀取錯誤輸出
                {
                    log.Invoke((Action)(() => log.Text += "\r\n" + a.Data));//同步讀取ffmpeg.exe回傳值,並輸出到log.Text
                    if (a.Data != null)//判斷回傳值是否回空
                    {
                        if (a.Data.Substring(0, 6) == "frame=")//前6位回傳值是否等於"frame="
                        {
                            nowFrame = float.Parse(a.Data.Substring(6, a.Data.IndexOf("fps=") - 7));
                        }
                    }
                };

                FFmpeg_video.Start();//啟動
                FFmpeg_video.BeginErrorReadLine();//將ffmpeg.exe重新導向到錯誤輸出流
                FFmpeg_video.WaitForExit();//等待ffmpeg.exe結束
                FFmpeg_video.Close();//關閉

                //開始處理聲音放慢倍率
                stageState = "audio:";//設定階段狀態
                stageTime = Environment.TickCount;//紀錄轉換時間
                nowFrame = 0;

                //設定放慢倍率參數
                string audioRatio = " -filter:a \"atempo = 0.5";
                if (Audio_switch.Checked && checkAudio && speedText != "Normal Speed")//檢測是否有和啟用音訊
                {
                    //處理聲音放慢倍率
                    float numA = (float)Math.Log(fpsRatio,2);//將FPS倍率取log2
                    int numB = (int)numA;//取整數
                    float numC = numA % 1;//取小數

                    for (; numB > 1; numB--)
                        audioRatio += ",atempo=0.5";

                    audioRatio += ",atempo=" + Math.Pow(0.5,numC) + "\" ";

                    //用ffmpeg處理聲音倍率
                    Process FFmpeg_process_audio = new Process();

                    FFmpeg_process_audio.StartInfo.UseShellExecute = false;
                    FFmpeg_process_audio.StartInfo.RedirectStandardError = true;//啟用錯誤輸出
                    FFmpeg_process_audio.StartInfo.CreateNoWindow = true;//不顯示視窗

                    FFmpeg_process_audio.StartInfo.FileName = path + ffmpeg;//設定ffmpeg路徑
                    FFmpeg_process_audio.StartInfo.Arguments = " -i \"" + path + _cache + videoNameOut + "\"" + audioRatio + "\"" + outputVideoText + videoNameOut + "\"";//設定輸入輸出參數

                    FFmpeg_process_audio.ErrorDataReceived += (s, a) =>//讀取錯誤輸出
                    {
                        log.Invoke((Action)(() => log.Text += "\r\n" + a.Data));//同步讀取ffmpeg.exe回傳值,並輸出到log.Text
                        if (a.Data != null)//判斷回傳值是否回空
                        {
                            if (a.Data.Substring(0, 6) == "frame=")//前6位回傳值是否等於"frame="
                            {
                                nowFrame = float.Parse(a.Data.Substring(6, a.Data.IndexOf("fps=") - 7));
                            }
                        }
                    };

                    FFmpeg_process_audio.Start();//啟動
                    FFmpeg_process_audio.BeginErrorReadLine();//將ffmpeg.exe重新導向到錯誤輸出流
                    FFmpeg_process_audio.WaitForExit();//等待ffmpeg.exe結束
                    FFmpeg_process_audio.Close();//關閉
                }

                //停止計時
                state = false;
                stageState = "";

                //輸出補幀資訊
                int ifrnetTimeS = ifrnetResultTime % 60;//分離出秒
                int ifrnetTimeM = ifrnetResultTime % 3600 / 60;//分離出分
                int ifrnetTimeH = ifrnetResultTime / 3600;//分離出小時
                log.Invoke((Action)(() => log.Text += "\r\n\r\n\r\nIFRnet result:" + ifrnetTimeH + ":" + ifrnetTimeM.ToString().PadLeft(2, '0') + ":" + ifrnetTimeS.ToString().PadLeft(2, '0') + "    " + ifrnetResultFps));//輸出結果

                //清空暫存
                Directory.Delete(path + _cache, true);//清除_cache資料夾和裡面所有的東西
                
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

        private void Browse_input_Click(object sender, EventArgs e)
        {
            OpenFileDialog path = new OpenFileDialog();
            path.Filter = "MP4|*.mp4|MKV|*.mkv|AVI|*.avi";
            path.ShowDialog();
            Input_video.Text = path.FileName;
        }

        private void Browse_output_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog path = new CommonOpenFileDialog();
            path.IsFolderPicker = true;
            if (path.ShowDialog() == CommonFileDialogResult.Ok)
                Output_video.Text = path.FileName;
        }
    }
}
