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
        const string _cache2 = "\\IFRnet\\_cache\\_cache1\\";//暫存2路徑
        const string _cache3 = "\\IFRnet\\_cache\\_cache2\\";//暫存3路徑
        const string _cache4 = "\\IFRnet\\_cache\\_cache3\\";//暫存4路徑
        string[] cache = {_cache,_cache2,_cache3,_cache4 };

        float frame = 0;//幀數
        float nowFrame = 0;//及時幀數
        float pngNum = 0;//目標幀數
        bool state = false;//開始狀態
        string stageState = "";//階段狀態
        int ifrnetStage = 1;//補幀階段
        int frequency = 1;//補幀次數
        int stateTime = 0;//開始時間
        int stageTime = 0;//補幀開始時間
        float Fps = 0;//FPS
        float frameRate = 0;//幀率
        float videoTime = 0;//影片時間
        float finalFps = 0;//最終幀率
        int fpsRatio = 1;//FPS倍率
        int mixTo = 0;//混幀
        string aiModeState = "GoPro";//紀錄Ai_mode狀態

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer.Start();
            Scale_ratio.SelectedIndex = 0;
            Speed.SelectedIndex = 0;
            Output_video_mode.SelectedIndex = 0;
            Ai_mode.SelectedIndex = 0;
            Mix_to.SelectedIndex = 0;
        }
        
        private void timer_Tick(object sender, EventArgs e)
        {
            string path = System.Environment.CurrentDirectory;//獲得當前路徑

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
                    numTotal = pngNum / (float)Math.Pow(2, frequency - ifrnetStage);
                    num = (float)Directory.GetFiles(path + cache[ifrnetStage]).Length;//暫存2 png數
                    Planned_speed.Text = "IFRnet_" + ifrnetStage + ":" + ((Directory.GetFiles(path + cache[ifrnetStage]).Length / (pngNum / Math.Pow(2, frequency - ifrnetStage))) * 100).ToString("0.00") + "%";//輸出百分比到Planned_speed.Text
                    break;
                case "audio:":
                    numTotal = videoTime * fpsRatio;
                    num = nowFrame * frameRate;//將及時幀數寫進num
                    float c = (nowFrame / (videoTime * fpsRatio)) * 100;//分離出時間,並轉換成百分比
                    Planned_speed.Text = stageState + c.ToString("0.00") + "%";//同步輸出進度到Planned_speed.Text
                    break;
                case "Out video:":
                    numTotal = pngNum;
                    num = nowFrame;//將及時幀數寫進num
                    float b = (nowFrame / pngNum) * 100;//分離出幀數,並轉換成百分比
                    Planned_speed.Text = stageState + b.ToString("0.00") + "%";//同步輸出進度到Planned_speed.Text
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
                Time_left.Text = TimeSpan.FromSeconds(timeLeft).ToString(@"hh\:mm\:ss");//輸出剩餘時間
            }
        }

        private void Start_Click(object sender, EventArgs e)
        {
            string path = System.Environment.CurrentDirectory;//獲得當前路徑
            string inputVideoText = Input_video.Text;//儲存各項參數
            string outputVideoText = Output_video.Text;
            string scaleRatioText = Scale_ratio.Text;
            string speedText = Speed.Text;
            string outputVideoModeText = Output_video_mode.Text;
            string aiModeText = Ai_mode.Text;
            string mixToText = Mix_to.Text;
            float finalFpsSetValue = finalFps;
            int fpsRatioSetValue = fpsRatio;
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
            Directory.CreateDirectory(path + _cache2);//創建_cache和裡面的子資料夾_cache1
            Directory.CreateDirectory(path + _cache3);//創建_cache和裡面的子資料夾_cache2
            Directory.CreateDirectory(path + _cache4);//創建_cache和裡面的子資料夾_cache3

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
                Scale_ratio.Invoke((Action)(() =>
                {
                    if (Scale_ratio.Text == "2.5" || Scale_ratio.Text == "7.5")//補幀倍率是否為2.5或7.5
                    {
                        if (fileNum % 2 == 1)//如果png的數量為單數
                        {
                            File.Copy(path + _cache + fileNum.ToString().PadLeft(8, '0') + ".png", path + _cache + (fileNum + 1).ToString().PadLeft(8, '0') + ".png");//複製一個與最後一張png相同的png,並編號+1
                            fileNum++;//fileNum + 1
                        }
                    }
                }));

                Scale_ratio.Invoke((Action)(() => pngNum =fileNum * float.Parse(Scale_ratio.Text)));//目前幀數*補幀倍率=目標幀數

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

                //檢測模型類型
                frequency = 1;//補幀次數
                string numFrame = "";//目標幀數參數
                if (aiModeText.Contains("Vimeo90K"))//如果是使用Vimeo90K模型就不使用-n參數
                {
                    numFrame = "";
                    frequency = (int)Math.Log(int.Parse(scaleRatioText),2);
                }
                else
                    numFrame = " -n " + pngNum;

                //開始補幀
                for (ifrnetStage = 1; ifrnetStage <= frequency; ifrnetStage++)
                {
                    Process IFRnet = new Process();

                    IFRnet.StartInfo.UseShellExecute = false;
                    IFRnet.StartInfo.CreateNoWindow = true;//不顯示視窗

                    IFRnet.StartInfo.FileName = path + ifrnet;//設定ifrnet-ncnn-vulkan.exe路徑
                    IFRnet.StartInfo.Arguments = "-m " + aiModeText + numFrame + gpuLevel + tta + uhd + " -i " + path + cache[ifrnetStage - 1] + " -o " + path + cache[ifrnetStage];//設定輸入輸出參數
                    IFRnet.Start();//啟動

                    while (Directory.GetFiles(path + cache[ifrnetStage]).Length != pngNum / Math.Pow(2, frequency - ifrnetStage)) //檢測暫存ifrnetStage png數是否等於目標幀數
                    {
                        log.Invoke((Action)(() => log.Text += "\r\nframe=" + Directory.GetFiles(path + cache[ifrnetStage]).Length + "/" + pngNum / Math.Pow(2, frequency - ifrnetStage)));//輸出當前幀數到log.Text
                        Thread.Sleep(1000);
                    }

                    IFRnet.WaitForExit();//等待ifrnet-ncnn-vulkan.exe結束
                    IFRnet.Close();//關閉
                }

                //紀錄補幀時間和FPS
                int ifrnetResultTime = (Environment.TickCount - stageTime) / 1000;//拿掉毫秒
                string ifrnetResultFps = FPS.Text;

                //給影片增加混合幀率
                string mixToName = "";
                if (mixToText != "None")
                    mixToName = "MixTo_" + mixTo + "fps_";

                //給影片增加編號
                string videoNum = "";//輸出檔案編號
                if (File.Exists(outputVideoText + videoName + "_x" + scaleRatioText + "_" + finalFpsSetValue + "fps_" + mixToName + aiModeText + "." + outputVideoModeText) == true)//檢測輸出資料夾是否存在同名檔案
                {
                    for (int i = 1; ; i++)//循環檢測其他編號
                    {
                        if (File.Exists(outputVideoText + videoName + "_x" + scaleRatioText + "_" + finalFpsSetValue + "fps_" + mixToName + aiModeText + "_" + i + "." + outputVideoModeText) != true)//檢測是否有其他編號
                        {
                            videoNum = "_" + i;//設定檔案編號
                            break;//跳出迴圈
                        }
                    }
                }

                string videoNameOut = videoName + "_x" + scaleRatioText + "_" + finalFpsSetValue + "fps_" + mixToName + aiModeText + videoNum + "." + outputVideoModeText;//設定影片輸出名

                //檢測並設定音訊格式
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

                //開始處理聲音放慢倍率
                stageState = "audio:";//設定階段狀態
                stageTime = Environment.TickCount;//紀錄轉換時間
                nowFrame = 0;

                //處理音訊
                string audioParameter = "";//音訊參數
                bool checkAudio = false;
                if (Audio_switch.Checked)//檢測是否啟用音訊
                {
                    //從影片分離音訊檔
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
                    if (checkAudio)//檢測是否分離出音訊檔
                    {
                        string audioName = "";
                        if (speedText == "Normal Speed")
                            audioName = "audio";//設定音訊檔名
                        else
                        {
                            //設定放慢倍率參數
                            string audioRatio = " -filter:a \"atempo = 0.5";

                            //處理聲音放慢倍率
                            float numA = (float)Math.Log(fpsRatioSetValue, 2);//將FPS倍率取log2
                            int numB = (int)numA;//取整數
                            float numC = numA % 1;//取小數

                            for (; numB > 1; numB--)
                                audioRatio += ",atempo=0.5";

                            audioRatio += ",atempo=" + Math.Pow(0.5, numC) + "\" ";

                            //用ffmpeg處理聲音倍率
                            Process FFmpeg_process_audio = new Process();

                            FFmpeg_process_audio.StartInfo.UseShellExecute = false;
                            FFmpeg_process_audio.StartInfo.RedirectStandardError = true;//啟用錯誤輸出
                            FFmpeg_process_audio.StartInfo.CreateNoWindow = true;//不顯示視窗

                            FFmpeg_process_audio.StartInfo.FileName = path + ffmpeg;//設定ffmpeg路徑
                            FFmpeg_process_audio.StartInfo.Arguments = " -i \"" + path + _cache + "audio" + audio + "\"" + audioRatio + "\"" + path + _cache + "audio_2" + audio + "\"";//設定輸入輸出參數

                            FFmpeg_process_audio.ErrorDataReceived += (s, a) =>//讀取錯誤輸出
                            {
                                log.Invoke((Action)(() => log.Text += "\r\n" + a.Data));//同步讀取ffmpeg.exe回傳值,並輸出到log.Text
                                if (a.Data != null)//判斷回傳值是否回空
                                {
                                    if (a.Data.Contains("time="))//回傳值是否有"time="
                                    {
                                        nowFrame = (float)TimeSpan.Parse(a.Data.Substring(a.Data.IndexOf("time=") + 5, a.Data.IndexOf(".") - a.Data.IndexOf("time=") - 5)).TotalSeconds;
                                    }
                                }
                            };

                            FFmpeg_process_audio.Start();//啟動
                            FFmpeg_process_audio.BeginErrorReadLine();//將ffmpeg.exe重新導向到錯誤輸出流
                            FFmpeg_process_audio.WaitForExit();//等待ffmpeg.exe結束
                            FFmpeg_process_audio.Close();//關閉

                            audioName = "audio_2";//設定音訊檔名
                        }

                        audioParameter = " -i \"" + path + _cache + audioName + audio + "\" -c:a copy";//設定音訊參數
                    }
                }

                //處理混幀
                string mixFrame = "";
                if(mixToText != "None")
                    mixFrame = " -r " + mixTo;

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
                FFmpeg_video.StartInfo.Arguments = "-framerate " + finalFpsSetValue + " -i \"" + path + cache[frequency] + "/%08d.png\"" + audioParameter + mixFrame + " -crf 20 -c:v libx264 -pix_fmt yuv420p " + "\"" + outputVideoText + videoNameOut + "\"";//設定輸入輸出參數

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

                //停止計時
                state = false;
                stageState = "";

                //輸出補幀資訊
                log.Invoke((Action)(() => log.Text += "\r\n\r\n\r\nIFRnet result:" + TimeSpan.FromSeconds(ifrnetResultTime).ToString(@"hh\:mm\:ss") + "    " + ifrnetResultFps));//輸出結果

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

        private void Ai_mode_SelectionChangeCommitted(object sender, EventArgs e)
        {
            string aiModeText = Ai_mode.Text;

            string[] scaleRatio = new string[] { "4", "8" };//Vimeo90K的選項
            string[] scaleRatio2 = new string[] { "2.5", "3", "4", "5", "6", "7", "7.5", "8", "9", "10" };//GoPro的選項

            if (aiModeText.Contains("Vimeo90K"))//選擇的模型是Vimeo90K
            {
                if (aiModeState != "Vimeo90K")//上次不是選擇Vimeo90K模型
                {
                    Scale_ratio.SelectedIndex = 0;//設定Scale_ratio成第一個選項

                    for (int i = 1; i <= 10; i++)//清除除了2以外的選項
                        Scale_ratio.Items.RemoveAt(1);

                    Scale_ratio.Items.AddRange(scaleRatio);

                    aiModeState = "Vimeo90K";//更改狀態為Vimeo90K
                }
            }
            else//是GoPro
            {
                if (aiModeState != "GoPro")//上次不是選擇GoPro模型
                {
                    Scale_ratio.SelectedIndex = 0;//設定Scale_ratio成第一個選項

                    for (int i = 1; i <= 2; i++)//清除除了2以外的選項
                        Scale_ratio.Items.RemoveAt(1);

                    Scale_ratio.Items.AddRange(scaleRatio2);

                    aiModeState = "GoPro";//更改狀態為GoPro
                }
            }
        }

        private void setOutputText()
        {
            Output_fps.Text = (float.Parse(Input_fps.Text) * float.Parse(Scale_ratio.Text)).ToString();//更新輸出幀率
        }

        private void setFinalFps()
        {
            finalFps = float.Parse(Output_fps.Text) / fpsRatio;//設定最終幀率
        }

        private void setMixTo()
        {
            //檢測最終幀率是否小於等於混幀
            if (Mix_to.Text != "None" && finalFps == mixTo && finalFps != 0)
            {
                Mix_to.SelectedIndex = 0;
                MessageBox.Show("The output frame rate cannot be equal to \"mix to\"");
            }
        }

        private void Input_video_TextChanged(object sender, EventArgs e)//影片輸入發生變化時
        {
            string path = System.Environment.CurrentDirectory;//獲得當前路徑

            Process process = new Process();

            //讀取影片幀率
            Process FFprobe_frame_rate = new Process();

            FFprobe_frame_rate.StartInfo.UseShellExecute = false;
            FFprobe_frame_rate.StartInfo.RedirectStandardOutput = true;//啟用錯誤輸出
            FFprobe_frame_rate.StartInfo.CreateNoWindow = true;//不顯示視窗

            FFprobe_frame_rate.StartInfo.FileName = path + ffprobe;//設定ffprobe路徑
            FFprobe_frame_rate.StartInfo.Arguments = "-v quiet -select_streams v:0 -show_entries stream=r_frame_rate -of csv=p=0 " + "\"" + Input_video.Text + "\"";//設定讀取參數

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

            //讀取影片時間
            Process FFprobe_frame_time = new Process();

            FFprobe_frame_time.StartInfo.UseShellExecute = false;
            FFprobe_frame_time.StartInfo.RedirectStandardOutput = true;//啟用錯誤輸出
            FFprobe_frame_time.StartInfo.CreateNoWindow = true;//不顯示視窗

            FFprobe_frame_time.StartInfo.FileName = path + ffprobe;//設定ffprobe路徑
            FFprobe_frame_time.StartInfo.Arguments = "-v quiet -select_streams v:0 -show_entries format=duration -of csv=p=0 " + "\"" + Input_video.Text + "\"";//設定讀取參數

            FFprobe_frame_time.OutputDataReceived += (s, a) =>//讀取錯誤輸出
            {
                if (a.Data != null)//判斷回傳值是否回空
                {
                    videoTime = float.Parse(a.Data);//將回傳值儲存在videoTime
                }
            };

            FFprobe_frame_time.Start();//啟動
            FFprobe_frame_time.BeginOutputReadLine();//將ffprobe.exe重新導向到錯誤輸出流
            FFprobe_frame_time.WaitForExit();//等待ffprobe.exe結束
            FFprobe_frame_time.Close();//關閉

            Input_fps.Text = frameRate.ToString();//輸出幀率到Input_fps.Text
            setOutputText();//更新輸出幀率
            setFinalFps();//設定最終幀率
            setMixTo();//檢測最終幀率是否小於等於混幀
        }

        private void Scale_ratio_SelectedIndexChanged(object sender, EventArgs e)//幀率發生變化時
        {
            setOutputText();//更新輸出幀率
            setFinalFps();//設定最終幀率
            setMixTo();//檢測最終幀率是否小於等於混幀
        }

        private void Speed_SelectedIndexChanged(object sender, EventArgs e)//放慢倍率發生變化時
        {
            //設定放慢倍率
            switch (Speed.Text)//判斷Speed.Text內的倍率
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

            setFinalFps();//設定最終幀率
            setMixTo();//檢測最終幀率是否小於等於混幀
        }

        private void Mix_to_SelectedIndexChanged(object sender, EventArgs e)//混幀發生變化時
        {
            //轉換Mix_to
            switch (Mix_to.Text)
            {
                case "None":
                    mixTo = 0;
                    break;
                case "24FPS":
                    mixTo = 24;
                    break;
                case "30FPS":
                    mixTo = 30;
                    break;
                case "60FPS":
                    mixTo = 60;
                    break;
            }

            setMixTo();//檢測最終幀率是否小於等於混幀
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Process[] processes = Process.GetProcessesByName("ifrnet-ncnn-vulkan");
            foreach (Process process in processes)
            {
                process.Kill();
            }
        }
    }
}
