namespace Aida64_Esp8266_DisplayControler
{
    partial class Main
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.清空日志ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.关于ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tmpBox = new System.Windows.Forms.GroupBox();
            this.hddTmp = new System.Windows.Forms.CheckBox();
            this.ramTmp = new System.Windows.Forms.CheckBox();
            this.gpuTmp = new System.Windows.Forms.CheckBox();
            this.cpuTmp = new System.Windows.Forms.CheckBox();
            this.runServer = new System.Windows.Forms.Button();
            this.utiBox = new System.Windows.Forms.GroupBox();
            this.vramUTI = new System.Windows.Forms.CheckBox();
            this.ramUTI = new System.Windows.Forms.CheckBox();
            this.gpuUTI = new System.Windows.Forms.CheckBox();
            this.cpuUTI = new System.Windows.Forms.CheckBox();
            this.logBox = new System.Windows.Forms.TextBox();
            this.getAidaData = new System.Windows.Forms.Timer(this.components);
            this.selectAll = new System.Windows.Forms.Button();
            this.unSelectAll = new System.Windows.Forms.Button();
            this.clkBox = new System.Windows.Forms.GroupBox();
            this.gpuClk = new System.Windows.Forms.CheckBox();
            this.cpuClk = new System.Windows.Forms.CheckBox();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.selButton = new System.Windows.Forms.Button();
            this.customPath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.customButton = new System.Windows.Forms.RadioButton();
            this.biliButton = new System.Windows.Forms.RadioButton();
            this.asusButton = new System.Windows.Forms.RadioButton();
            this.baButton = new System.Windows.Forms.RadioButton();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.sendData = new System.Windows.Forms.Timer(this.components);
            this.sendGif = new System.Windows.Forms.Timer(this.components);
            this.menuStrip.SuspendLayout();
            this.tmpBox.SuspendLayout();
            this.utiBox.SuspendLayout();
            this.clkBox.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.设置ToolStripMenuItem,
            this.关于ToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(390, 25);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip1";
            // 
            // 设置ToolStripMenuItem
            // 
            this.设置ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.清空日志ToolStripMenuItem});
            this.设置ToolStripMenuItem.Name = "设置ToolStripMenuItem";
            this.设置ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.设置ToolStripMenuItem.Text = "设置";
            // 
            // 清空日志ToolStripMenuItem
            // 
            this.清空日志ToolStripMenuItem.Name = "清空日志ToolStripMenuItem";
            this.清空日志ToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.清空日志ToolStripMenuItem.Text = "清空LogBox";
            this.清空日志ToolStripMenuItem.Click += new System.EventHandler(this.清空日志ToolStripMenuItem_Click);
            // 
            // 关于ToolStripMenuItem
            // 
            this.关于ToolStripMenuItem.Name = "关于ToolStripMenuItem";
            this.关于ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.关于ToolStripMenuItem.Text = "关于";
            // 
            // tmpBox
            // 
            this.tmpBox.Controls.Add(this.hddTmp);
            this.tmpBox.Controls.Add(this.ramTmp);
            this.tmpBox.Controls.Add(this.gpuTmp);
            this.tmpBox.Controls.Add(this.cpuTmp);
            this.tmpBox.Location = new System.Drawing.Point(14, 62);
            this.tmpBox.Name = "tmpBox";
            this.tmpBox.Size = new System.Drawing.Size(157, 71);
            this.tmpBox.TabIndex = 1;
            this.tmpBox.TabStop = false;
            this.tmpBox.Text = "温度";
            // 
            // hddTmp
            // 
            this.hddTmp.AutoSize = true;
            this.hddTmp.Location = new System.Drawing.Point(82, 44);
            this.hddTmp.Name = "hddTmp";
            this.hddTmp.Size = new System.Drawing.Size(42, 16);
            this.hddTmp.TabIndex = 3;
            this.hddTmp.Text = "HDD";
            this.hddTmp.UseVisualStyleBackColor = true;
            // 
            // ramTmp
            // 
            this.ramTmp.AutoSize = true;
            this.ramTmp.Location = new System.Drawing.Point(82, 21);
            this.ramTmp.Name = "ramTmp";
            this.ramTmp.Size = new System.Drawing.Size(42, 16);
            this.ramTmp.TabIndex = 2;
            this.ramTmp.Text = "RAM";
            this.ramTmp.UseVisualStyleBackColor = true;
            // 
            // gpuTmp
            // 
            this.gpuTmp.AutoSize = true;
            this.gpuTmp.Location = new System.Drawing.Point(13, 44);
            this.gpuTmp.Name = "gpuTmp";
            this.gpuTmp.Size = new System.Drawing.Size(42, 16);
            this.gpuTmp.TabIndex = 1;
            this.gpuTmp.Text = "GPU";
            this.gpuTmp.UseVisualStyleBackColor = true;
            // 
            // cpuTmp
            // 
            this.cpuTmp.AutoSize = true;
            this.cpuTmp.Location = new System.Drawing.Point(13, 21);
            this.cpuTmp.Name = "cpuTmp";
            this.cpuTmp.Size = new System.Drawing.Size(42, 16);
            this.cpuTmp.TabIndex = 0;
            this.cpuTmp.Text = "CPU";
            this.cpuTmp.UseVisualStyleBackColor = true;
            // 
            // runServer
            // 
            this.runServer.Location = new System.Drawing.Point(15, 28);
            this.runServer.Name = "runServer";
            this.runServer.Size = new System.Drawing.Size(75, 23);
            this.runServer.TabIndex = 4;
            this.runServer.Text = "启动服务器";
            this.runServer.UseVisualStyleBackColor = true;
            this.runServer.Click += new System.EventHandler(this.Runserver_Click);
            // 
            // utiBox
            // 
            this.utiBox.Controls.Add(this.vramUTI);
            this.utiBox.Controls.Add(this.ramUTI);
            this.utiBox.Controls.Add(this.gpuUTI);
            this.utiBox.Controls.Add(this.cpuUTI);
            this.utiBox.Location = new System.Drawing.Point(14, 139);
            this.utiBox.Name = "utiBox";
            this.utiBox.Size = new System.Drawing.Size(157, 77);
            this.utiBox.TabIndex = 5;
            this.utiBox.TabStop = false;
            this.utiBox.Text = "使用率";
            // 
            // vramUTI
            // 
            this.vramUTI.AutoSize = true;
            this.vramUTI.Location = new System.Drawing.Point(82, 44);
            this.vramUTI.Name = "vramUTI";
            this.vramUTI.Size = new System.Drawing.Size(48, 16);
            this.vramUTI.TabIndex = 7;
            this.vramUTI.Text = "VRAM";
            this.vramUTI.UseVisualStyleBackColor = true;
            // 
            // ramUTI
            // 
            this.ramUTI.AutoSize = true;
            this.ramUTI.Location = new System.Drawing.Point(82, 20);
            this.ramUTI.Name = "ramUTI";
            this.ramUTI.Size = new System.Drawing.Size(42, 16);
            this.ramUTI.TabIndex = 6;
            this.ramUTI.Text = "RAM";
            this.ramUTI.UseVisualStyleBackColor = true;
            // 
            // gpuUTI
            // 
            this.gpuUTI.AutoSize = true;
            this.gpuUTI.Location = new System.Drawing.Point(13, 44);
            this.gpuUTI.Name = "gpuUTI";
            this.gpuUTI.Size = new System.Drawing.Size(42, 16);
            this.gpuUTI.TabIndex = 5;
            this.gpuUTI.Text = "GPU";
            this.gpuUTI.UseVisualStyleBackColor = true;
            // 
            // cpuUTI
            // 
            this.cpuUTI.AutoSize = true;
            this.cpuUTI.Location = new System.Drawing.Point(13, 21);
            this.cpuUTI.Name = "cpuUTI";
            this.cpuUTI.Size = new System.Drawing.Size(42, 16);
            this.cpuUTI.TabIndex = 4;
            this.cpuUTI.Text = "CPU";
            this.cpuUTI.UseVisualStyleBackColor = true;
            // 
            // logBox
            // 
            this.logBox.Location = new System.Drawing.Point(15, 314);
            this.logBox.Multiline = true;
            this.logBox.Name = "logBox";
            this.logBox.ReadOnly = true;
            this.logBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.logBox.Size = new System.Drawing.Size(359, 111);
            this.logBox.TabIndex = 6;
            // 
            // getAidaData
            // 
            this.getAidaData.Interval = 3000;
            this.getAidaData.Tick += new System.EventHandler(this.GetAidaData_Tick);
            // 
            // selectAll
            // 
            this.selectAll.Location = new System.Drawing.Point(178, 28);
            this.selectAll.Name = "selectAll";
            this.selectAll.Size = new System.Drawing.Size(75, 23);
            this.selectAll.TabIndex = 8;
            this.selectAll.Text = "全选";
            this.selectAll.UseVisualStyleBackColor = true;
            // 
            // unSelectAll
            // 
            this.unSelectAll.Location = new System.Drawing.Point(260, 28);
            this.unSelectAll.Name = "unSelectAll";
            this.unSelectAll.Size = new System.Drawing.Size(75, 23);
            this.unSelectAll.TabIndex = 9;
            this.unSelectAll.Text = "全不选";
            this.unSelectAll.UseVisualStyleBackColor = true;
            // 
            // clkBox
            // 
            this.clkBox.Controls.Add(this.gpuClk);
            this.clkBox.Controls.Add(this.cpuClk);
            this.clkBox.Location = new System.Drawing.Point(14, 222);
            this.clkBox.Name = "clkBox";
            this.clkBox.Size = new System.Drawing.Size(157, 48);
            this.clkBox.TabIndex = 10;
            this.clkBox.TabStop = false;
            this.clkBox.Text = "频率";
            // 
            // gpuClk
            // 
            this.gpuClk.AutoSize = true;
            this.gpuClk.Location = new System.Drawing.Point(82, 20);
            this.gpuClk.Name = "gpuClk";
            this.gpuClk.Size = new System.Drawing.Size(42, 16);
            this.gpuClk.TabIndex = 3;
            this.gpuClk.Text = "GPU";
            this.gpuClk.UseVisualStyleBackColor = true;
            // 
            // cpuClk
            // 
            this.cpuClk.AutoSize = true;
            this.cpuClk.Location = new System.Drawing.Point(13, 20);
            this.cpuClk.Name = "cpuClk";
            this.cpuClk.Size = new System.Drawing.Size(42, 16);
            this.cpuClk.TabIndex = 2;
            this.cpuClk.Text = "CPU";
            this.cpuClk.UseVisualStyleBackColor = true;
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Text = "notifyIcon";
            this.notifyIcon1.Visible = true;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(86, 282);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(55, 21);
            this.textBox1.TabIndex = 11;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 287);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 12;
            this.label1.Text = "刷新间隔";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBox1);
            this.groupBox1.Controls.Add(this.selButton);
            this.groupBox1.Controls.Add(this.customPath);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.customButton);
            this.groupBox1.Controls.Add(this.biliButton);
            this.groupBox1.Controls.Add(this.asusButton);
            this.groupBox1.Controls.Add(this.baButton);
            this.groupBox1.Controls.Add(this.pictureBox);
            this.groupBox1.Location = new System.Drawing.Point(178, 62);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(196, 208);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "动画";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(13, 19);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(72, 16);
            this.checkBox1.TabIndex = 8;
            this.checkBox1.Text = "启用动画";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.CheckBox1_CheckedChanged);
            // 
            // selButton
            // 
            this.selButton.Location = new System.Drawing.Point(147, 104);
            this.selButton.Name = "selButton";
            this.selButton.Size = new System.Drawing.Size(27, 21);
            this.selButton.TabIndex = 7;
            this.selButton.Text = "...";
            this.selButton.UseVisualStyleBackColor = true;
            this.selButton.Click += new System.EventHandler(this.SelButton_Click);
            // 
            // customPath
            // 
            this.customPath.Location = new System.Drawing.Point(13, 104);
            this.customPath.Name = "customPath";
            this.customPath.Size = new System.Drawing.Size(128, 21);
            this.customPath.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 83);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "自定义路径";
            // 
            // customButton
            // 
            this.customButton.AutoSize = true;
            this.customButton.Location = new System.Drawing.Point(104, 63);
            this.customButton.Name = "customButton";
            this.customButton.Size = new System.Drawing.Size(59, 16);
            this.customButton.TabIndex = 4;
            this.customButton.TabStop = true;
            this.customButton.Text = "自定义";
            this.customButton.UseVisualStyleBackColor = true;
            this.customButton.CheckedChanged += new System.EventHandler(this.CustomButton_CheckedChanged);
            // 
            // biliButton
            // 
            this.biliButton.AutoSize = true;
            this.biliButton.Location = new System.Drawing.Point(104, 41);
            this.biliButton.Name = "biliButton";
            this.biliButton.Size = new System.Drawing.Size(71, 16);
            this.biliButton.TabIndex = 3;
            this.biliButton.TabStop = true;
            this.biliButton.Text = "BiliBili";
            this.biliButton.UseVisualStyleBackColor = true;
            this.biliButton.CheckedChanged += new System.EventHandler(this.BiliButton_CheckedChanged);
            // 
            // asusButton
            // 
            this.asusButton.AutoSize = true;
            this.asusButton.Location = new System.Drawing.Point(13, 63);
            this.asusButton.Name = "asusButton";
            this.asusButton.Size = new System.Drawing.Size(47, 16);
            this.asusButton.TabIndex = 2;
            this.asusButton.TabStop = true;
            this.asusButton.Text = "Asus";
            this.asusButton.UseVisualStyleBackColor = true;
            this.asusButton.CheckedChanged += new System.EventHandler(this.AsusButton_CheckedChanged);
            // 
            // baButton
            // 
            this.baButton.AutoSize = true;
            this.baButton.Location = new System.Drawing.Point(13, 41);
            this.baButton.Name = "baButton";
            this.baButton.Size = new System.Drawing.Size(71, 16);
            this.baButton.TabIndex = 1;
            this.baButton.TabStop = true;
            this.baButton.Text = "BadApple";
            this.baButton.UseVisualStyleBackColor = true;
            this.baButton.CheckedChanged += new System.EventHandler(this.BaButton_CheckedChanged);
            // 
            // pictureBox
            // 
            this.pictureBox.Location = new System.Drawing.Point(29, 138);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(128, 64);
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            // 
            // sendData
            // 
            this.sendData.Tick += new System.EventHandler(this.SendData_Tick);
            // 
            // sendGif
            // 
            this.sendGif.Tick += new System.EventHandler(this.SendGif_Tick);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(390, 433);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.unSelectAll);
            this.Controls.Add(this.selectAll);
            this.Controls.Add(this.runServer);
            this.Controls.Add(this.clkBox);
            this.Controls.Add(this.utiBox);
            this.Controls.Add(this.tmpBox);
            this.Controls.Add(this.menuStrip);
            this.Controls.Add(this.logBox);
            this.MainMenuStrip = this.menuStrip;
            this.Name = "Main";
            this.ShowIcon = false;
            this.Text = "Aida64_Esp8266_DisplayControler";
            this.Load += new System.EventHandler(this.Main_Load);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.tmpBox.ResumeLayout(false);
            this.tmpBox.PerformLayout();
            this.utiBox.ResumeLayout(false);
            this.utiBox.PerformLayout();
            this.clkBox.ResumeLayout(false);
            this.clkBox.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem 设置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 关于ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 清空日志ToolStripMenuItem;
        private System.Windows.Forms.GroupBox tmpBox;
        private System.Windows.Forms.Button runServer;
        private System.Windows.Forms.CheckBox ramTmp;
        private System.Windows.Forms.CheckBox gpuTmp;
        private System.Windows.Forms.CheckBox cpuTmp;
        private System.Windows.Forms.GroupBox utiBox;
        private System.Windows.Forms.CheckBox hddTmp;
        public System.Windows.Forms.TextBox logBox;
        private System.Windows.Forms.Timer getAidaData;
        private System.Windows.Forms.Button selectAll;
        private System.Windows.Forms.Button unSelectAll;
        private System.Windows.Forms.CheckBox ramUTI;
        private System.Windows.Forms.CheckBox gpuUTI;
        private System.Windows.Forms.CheckBox cpuUTI;
        private System.Windows.Forms.GroupBox clkBox;
        private System.Windows.Forms.CheckBox vramUTI;
        private System.Windows.Forms.CheckBox gpuClk;
        private System.Windows.Forms.CheckBox cpuClk;
        public System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.RadioButton customButton;
        private System.Windows.Forms.RadioButton biliButton;
        private System.Windows.Forms.RadioButton asusButton;
        private System.Windows.Forms.RadioButton baButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox customPath;
        private System.Windows.Forms.Button selButton;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Timer sendData;
        private System.Windows.Forms.Timer sendGif;
    }
}

