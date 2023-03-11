namespace IFRnet
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.Input_video = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.Output_video = new System.Windows.Forms.TextBox();
            this.Browse_input = new System.Windows.Forms.Button();
            this.Browse_output = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.ScaleRatio = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.Speed = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.Output_video_mode = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.Ai_mode = new System.Windows.Forms.ComboBox();
            this.TTA = new System.Windows.Forms.CheckBox();
            this.UHD = new System.Windows.Forms.CheckBox();
            this.Start = new System.Windows.Forms.Button();
            this.Input_fps = new System.Windows.Forms.TextBox();
            this.Output_fps = new System.Windows.Forms.TextBox();
            this.log = new System.Windows.Forms.TextBox();
            this.Time_left = new System.Windows.Forms.TextBox();
            this.Planned_speed = new System.Windows.Forms.TextBox();
            this.Time = new System.Windows.Forms.TextBox();
            this.FPS = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.Audio_switch = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.GPU_level = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.GPU_level)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微軟正黑體", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.ForeColor = System.Drawing.SystemColors.Control;
            this.label1.Location = new System.Drawing.Point(21, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 26);
            this.label1.TabIndex = 0;
            this.label1.Text = "Input";
            // 
            // Input_video
            // 
            this.Input_video.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Input_video.Location = new System.Drawing.Point(103, 9);
            this.Input_video.Name = "Input_video";
            this.Input_video.Size = new System.Drawing.Size(634, 29);
            this.Input_video.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微軟正黑體", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label2.ForeColor = System.Drawing.SystemColors.Control;
            this.label2.Location = new System.Drawing.Point(12, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 26);
            this.label2.TabIndex = 2;
            this.label2.Text = "Output";
            // 
            // Output_video
            // 
            this.Output_video.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Output_video.Location = new System.Drawing.Point(103, 44);
            this.Output_video.Name = "Output_video";
            this.Output_video.Size = new System.Drawing.Size(634, 29);
            this.Output_video.TabIndex = 3;
            // 
            // Browse_input
            // 
            this.Browse_input.Font = new System.Drawing.Font("微軟正黑體", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Browse_input.Location = new System.Drawing.Point(743, 9);
            this.Browse_input.Name = "Browse_input";
            this.Browse_input.Size = new System.Drawing.Size(29, 29);
            this.Browse_input.TabIndex = 4;
            this.Browse_input.Text = "...";
            this.Browse_input.UseVisualStyleBackColor = true;
            this.Browse_input.Click += new System.EventHandler(this.Browse_input_Click);
            // 
            // Browse_output
            // 
            this.Browse_output.Font = new System.Drawing.Font("微軟正黑體", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Browse_output.Location = new System.Drawing.Point(743, 44);
            this.Browse_output.Name = "Browse_output";
            this.Browse_output.Size = new System.Drawing.Size(29, 29);
            this.Browse_output.TabIndex = 5;
            this.Browse_output.Text = "...";
            this.Browse_output.UseVisualStyleBackColor = true;
            this.Browse_output.Click += new System.EventHandler(this.Browse_output_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微軟正黑體", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label3.ForeColor = System.Drawing.SystemColors.Control;
            this.label3.Location = new System.Drawing.Point(12, 79);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(126, 26);
            this.label3.TabIndex = 6;
            this.label3.Text = "Output FPS";
            // 
            // ScaleRatio
            // 
            this.ScaleRatio.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ScaleRatio.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.ScaleRatio.FormattingEnabled = true;
            this.ScaleRatio.Items.AddRange(new object[] {
            "2",
            "2.5",
            "3",
            "4",
            "5",
            "6",
            "7",
            "7.5",
            "8",
            "9",
            "10"});
            this.ScaleRatio.Location = new System.Drawing.Point(285, 79);
            this.ScaleRatio.Name = "ScaleRatio";
            this.ScaleRatio.Size = new System.Drawing.Size(50, 29);
            this.ScaleRatio.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微軟正黑體", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label4.ForeColor = System.Drawing.SystemColors.Control;
            this.label4.Location = new System.Drawing.Point(341, 82);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(28, 26);
            this.label4.TabIndex = 9;
            this.label4.Text = "=";
            // 
            // Speed
            // 
            this.Speed.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Speed.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Speed.FormattingEnabled = true;
            this.Speed.Items.AddRange(new object[] {
            "Normal Speed",
            "x2 Slowmo",
            "x3 Slowmo",
            "x4 Slowmo",
            "x5 Slowmo",
            "x6 Slowmo",
            "x7 Slowmo",
            "x8 Slowmo",
            "x9 Slowmo",
            "x10 Slowmo"});
            this.Speed.Location = new System.Drawing.Point(461, 79);
            this.Speed.Name = "Speed";
            this.Speed.Size = new System.Drawing.Size(135, 29);
            this.Speed.TabIndex = 11;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("微軟正黑體", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label5.ForeColor = System.Drawing.SystemColors.Control;
            this.label5.Location = new System.Drawing.Point(12, 114);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(151, 26);
            this.label5.TabIndex = 12;
            this.label5.Text = "Output Mode";
            // 
            // Output_video_mode
            // 
            this.Output_video_mode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Output_video_mode.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Output_video_mode.FormattingEnabled = true;
            this.Output_video_mode.Items.AddRange(new object[] {
            "MP4",
            "MKV",
            "AVI"});
            this.Output_video_mode.Location = new System.Drawing.Point(170, 114);
            this.Output_video_mode.Name = "Output_video_mode";
            this.Output_video_mode.Size = new System.Drawing.Size(80, 29);
            this.Output_video_mode.TabIndex = 13;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("微軟正黑體", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label6.ForeColor = System.Drawing.SystemColors.Control;
            this.label6.Location = new System.Drawing.Point(324, 117);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(99, 26);
            this.label6.TabIndex = 14;
            this.label6.Text = "Ai Mode";
            // 
            // Ai_mode
            // 
            this.Ai_mode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Ai_mode.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Ai_mode.FormattingEnabled = true;
            this.Ai_mode.Items.AddRange(new object[] {
            "IFRNet_GoPro",
            "IFRNet_S_GoPro",
            "IFRNet_L_GoPro"});
            this.Ai_mode.Location = new System.Drawing.Point(429, 114);
            this.Ai_mode.Name = "Ai_mode";
            this.Ai_mode.Size = new System.Drawing.Size(180, 29);
            this.Ai_mode.TabIndex = 15;
            // 
            // TTA
            // 
            this.TTA.AutoSize = true;
            this.TTA.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.TTA.ForeColor = System.Drawing.SystemColors.Control;
            this.TTA.Location = new System.Drawing.Point(615, 120);
            this.TTA.Name = "TTA";
            this.TTA.Size = new System.Drawing.Size(49, 20);
            this.TTA.TabIndex = 17;
            this.TTA.Text = "TTA";
            this.TTA.UseVisualStyleBackColor = true;
            // 
            // UHD
            // 
            this.UHD.AutoSize = true;
            this.UHD.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.UHD.ForeColor = System.Drawing.SystemColors.Control;
            this.UHD.Location = new System.Drawing.Point(672, 120);
            this.UHD.Name = "UHD";
            this.UHD.Size = new System.Drawing.Size(53, 20);
            this.UHD.TabIndex = 18;
            this.UHD.Text = "UHD";
            this.UHD.UseVisualStyleBackColor = true;
            // 
            // Start
            // 
            this.Start.Font = new System.Drawing.Font("微軟正黑體", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Start.Location = new System.Drawing.Point(672, 184);
            this.Start.Name = "Start";
            this.Start.Size = new System.Drawing.Size(100, 100);
            this.Start.TabIndex = 19;
            this.Start.Text = "Start";
            this.Start.UseVisualStyleBackColor = true;
            this.Start.Click += new System.EventHandler(this.Start_Click);
            // 
            // Input_fps
            // 
            this.Input_fps.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Input_fps.Location = new System.Drawing.Point(170, 79);
            this.Input_fps.Name = "Input_fps";
            this.Input_fps.ReadOnly = true;
            this.Input_fps.Size = new System.Drawing.Size(80, 29);
            this.Input_fps.TabIndex = 20;
            this.Input_fps.Text = "0";
            this.Input_fps.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // Output_fps
            // 
            this.Output_fps.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Output_fps.Location = new System.Drawing.Point(375, 79);
            this.Output_fps.Name = "Output_fps";
            this.Output_fps.ReadOnly = true;
            this.Output_fps.Size = new System.Drawing.Size(80, 29);
            this.Output_fps.TabIndex = 21;
            this.Output_fps.Text = "0";
            this.Output_fps.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // log
            // 
            this.log.Font = new System.Drawing.Font("微軟正黑體", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.log.Location = new System.Drawing.Point(12, 184);
            this.log.Multiline = true;
            this.log.Name = "log";
            this.log.ReadOnly = true;
            this.log.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.log.Size = new System.Drawing.Size(654, 100);
            this.log.TabIndex = 22;
            this.log.TextChanged += new System.EventHandler(this.log_TextChanged);
            // 
            // Time_left
            // 
            this.Time_left.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Time_left.Location = new System.Drawing.Point(127, 149);
            this.Time_left.Name = "Time_left";
            this.Time_left.ReadOnly = true;
            this.Time_left.Size = new System.Drawing.Size(109, 29);
            this.Time_left.TabIndex = 24;
            this.Time_left.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // Planned_speed
            // 
            this.Planned_speed.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Planned_speed.Location = new System.Drawing.Point(357, 149);
            this.Planned_speed.Name = "Planned_speed";
            this.Planned_speed.ReadOnly = true;
            this.Planned_speed.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Planned_speed.Size = new System.Drawing.Size(155, 29);
            this.Planned_speed.TabIndex = 25;
            // 
            // Time
            // 
            this.Time.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Time.Location = new System.Drawing.Point(12, 149);
            this.Time.Name = "Time";
            this.Time.ReadOnly = true;
            this.Time.Size = new System.Drawing.Size(109, 29);
            this.Time.TabIndex = 26;
            this.Time.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // FPS
            // 
            this.FPS.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.FPS.Location = new System.Drawing.Point(242, 149);
            this.FPS.Name = "FPS";
            this.FPS.ReadOnly = true;
            this.FPS.Size = new System.Drawing.Size(109, 29);
            this.FPS.TabIndex = 27;
            this.FPS.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("微軟正黑體", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label7.ForeColor = System.Drawing.SystemColors.Control;
            this.label7.Location = new System.Drawing.Point(256, 82);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(23, 26);
            this.label7.TabIndex = 28;
            this.label7.Text = "x";
            // 
            // timer
            // 
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // Audio_switch
            // 
            this.Audio_switch.AutoSize = true;
            this.Audio_switch.Checked = true;
            this.Audio_switch.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Audio_switch.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Audio_switch.ForeColor = System.Drawing.SystemColors.Control;
            this.Audio_switch.Location = new System.Drawing.Point(256, 120);
            this.Audio_switch.Name = "Audio_switch";
            this.Audio_switch.Size = new System.Drawing.Size(62, 20);
            this.Audio_switch.TabIndex = 29;
            this.Audio_switch.Text = "Audio";
            this.Audio_switch.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("微軟正黑體", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label8.ForeColor = System.Drawing.SystemColors.Control;
            this.label8.Location = new System.Drawing.Point(602, 82);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(109, 26);
            this.label8.TabIndex = 30;
            this.label8.Text = "GPULevel";
            // 
            // GPU_level
            // 
            this.GPU_level.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.GPU_level.Location = new System.Drawing.Point(717, 79);
            this.GPU_level.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.GPU_level.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.GPU_level.Name = "GPU_level";
            this.GPU_level.ReadOnly = true;
            this.GPU_level.Size = new System.Drawing.Size(55, 29);
            this.GPU_level.TabIndex = 31;
            this.GPU_level.TabStop = false;
            this.GPU_level.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.ClientSize = new System.Drawing.Size(784, 296);
            this.Controls.Add(this.GPU_level);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.Audio_switch);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.FPS);
            this.Controls.Add(this.Time);
            this.Controls.Add(this.Planned_speed);
            this.Controls.Add(this.Time_left);
            this.Controls.Add(this.log);
            this.Controls.Add(this.Output_fps);
            this.Controls.Add(this.Input_fps);
            this.Controls.Add(this.Start);
            this.Controls.Add(this.UHD);
            this.Controls.Add(this.TTA);
            this.Controls.Add(this.Ai_mode);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.Output_video_mode);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.Speed);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.ScaleRatio);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.Browse_output);
            this.Controls.Add(this.Browse_input);
            this.Controls.Add(this.Output_video);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Input_video);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "IFRnet";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.GPU_level)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox Input_video;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox Output_video;
        private System.Windows.Forms.Button Browse_input;
        private System.Windows.Forms.Button Browse_output;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox ScaleRatio;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox Speed;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox Output_video_mode;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox Ai_mode;
        private System.Windows.Forms.CheckBox TTA;
        private System.Windows.Forms.CheckBox UHD;
        private System.Windows.Forms.Button Start;
        private System.Windows.Forms.TextBox Input_fps;
        private System.Windows.Forms.TextBox Output_fps;
        private System.Windows.Forms.TextBox log;
        private System.Windows.Forms.TextBox Time_left;
        private System.Windows.Forms.TextBox Planned_speed;
        private System.Windows.Forms.TextBox Time;
        private System.Windows.Forms.TextBox FPS;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.CheckBox Audio_switch;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown GPU_level;
    }
}

