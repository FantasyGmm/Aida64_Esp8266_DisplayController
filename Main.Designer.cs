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
            this.mbTmp = new System.Windows.Forms.CheckBox();
            this.gpuTmp = new System.Windows.Forms.CheckBox();
            this.cpuTmp = new System.Windows.Forms.CheckBox();
            this.btnSendData = new System.Windows.Forms.Button();
            this.utiBox = new System.Windows.Forms.GroupBox();
            this.vramUTI = new System.Windows.Forms.CheckBox();
            this.ramUTI = new System.Windows.Forms.CheckBox();
            this.gpuUTI = new System.Windows.Forms.CheckBox();
            this.cpuUTI = new System.Windows.Forms.CheckBox();
            this.logBox = new System.Windows.Forms.TextBox();
            this.selectAll = new System.Windows.Forms.Button();
            this.unSelectAll = new System.Windows.Forms.Button();
            this.clkBox = new System.Windows.Forms.GroupBox();
            this.gpuClk = new System.Windows.Forms.CheckBox();
            this.cpuClk = new System.Windows.Forms.CheckBox();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.bmpPanel = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.nbxHeight = new System.Windows.Forms.NumericUpDown();
            this.nbxWidth = new System.Windows.Forms.NumericUpDown();
            this.selButton = new System.Windows.Forms.Button();
            this.customPath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lbxClient = new System.Windows.Forms.ListBox();
            this.cbSendAll = new System.Windows.Forms.CheckBox();
            this.btnReboot = new System.Windows.Forms.Button();
            this.btnLed = new System.Windows.Forms.Button();
            this.btnSendGif = new System.Windows.Forms.Button();
            this.rpmBox = new System.Windows.Forms.GroupBox();
            this.gpuRpm = new System.Windows.Forms.CheckBox();
            this.cpuRpm = new System.Windows.Forms.CheckBox();
            this.volBox = new System.Windows.Forms.GroupBox();
            this.gpuVol = new System.Windows.Forms.CheckBox();
            this.cpuVol = new System.Windows.Forms.CheckBox();
            this.timerInterval = new System.Windows.Forms.NumericUpDown();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.制作动画包ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.displayTime = new System.Windows.Forms.CheckBox();
            this.menuStrip.SuspendLayout();
            this.tmpBox.SuspendLayout();
            this.utiBox.SuspendLayout();
            this.clkBox.SuspendLayout();
            this.bmpPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nbxHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nbxWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.panel1.SuspendLayout();
            this.rpmBox.SuspendLayout();
            this.volBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.timerInterval)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.设置ToolStripMenuItem,
            this.关于ToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(731, 25);
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
            this.清空日志ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
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
            this.tmpBox.Controls.Add(this.mbTmp);
            this.tmpBox.Controls.Add(this.gpuTmp);
            this.tmpBox.Controls.Add(this.cpuTmp);
            this.tmpBox.Location = new System.Drawing.Point(360, 68);
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
            this.hddTmp.CheckedChanged += new System.EventHandler(this.HddTmp_CheckedChanged);
            // 
            // mbTmp
            // 
            this.mbTmp.AutoSize = true;
            this.mbTmp.Location = new System.Drawing.Point(82, 21);
            this.mbTmp.Name = "mbTmp";
            this.mbTmp.Size = new System.Drawing.Size(48, 16);
            this.mbTmp.TabIndex = 2;
            this.mbTmp.Text = "主板";
            this.mbTmp.UseVisualStyleBackColor = true;
            this.mbTmp.CheckedChanged += new System.EventHandler(this.MbTmp_CheckedChanged);
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
            this.gpuTmp.CheckedChanged += new System.EventHandler(this.GpuTmp_CheckedChanged);
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
            this.cpuTmp.CheckedChanged += new System.EventHandler(this.CpuTmp_CheckedChanged);
            // 
            // btnSendData
            // 
            this.btnSendData.Location = new System.Drawing.Point(537, 28);
            this.btnSendData.Name = "btnSendData";
            this.btnSendData.Size = new System.Drawing.Size(85, 23);
            this.btnSendData.TabIndex = 4;
            this.btnSendData.Text = "发送监测数据";
            this.btnSendData.UseVisualStyleBackColor = true;
            this.btnSendData.Click += new System.EventHandler(this.BtnSendData_Click);
            // 
            // utiBox
            // 
            this.utiBox.Controls.Add(this.vramUTI);
            this.utiBox.Controls.Add(this.ramUTI);
            this.utiBox.Controls.Add(this.gpuUTI);
            this.utiBox.Controls.Add(this.cpuUTI);
            this.utiBox.Location = new System.Drawing.Point(360, 145);
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
            this.vramUTI.CheckedChanged += new System.EventHandler(this.VramUTI_CheckedChanged);
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
            this.ramUTI.CheckedChanged += new System.EventHandler(this.RamUTI_CheckedChanged);
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
            this.gpuUTI.CheckedChanged += new System.EventHandler(this.GpuUTI_CheckedChanged);
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
            this.cpuUTI.CheckedChanged += new System.EventHandler(this.CpuUTI_CheckedChanged);
            // 
            // logBox
            // 
            this.logBox.Location = new System.Drawing.Point(361, 400);
            this.logBox.Multiline = true;
            this.logBox.Name = "logBox";
            this.logBox.ReadOnly = true;
            this.logBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.logBox.Size = new System.Drawing.Size(359, 111);
            this.logBox.TabIndex = 6;
            // 
            // selectAll
            // 
            this.selectAll.Location = new System.Drawing.Point(361, 28);
            this.selectAll.Name = "selectAll";
            this.selectAll.Size = new System.Drawing.Size(75, 23);
            this.selectAll.TabIndex = 8;
            this.selectAll.Text = "全选";
            this.selectAll.UseVisualStyleBackColor = true;
            this.selectAll.Click += new System.EventHandler(this.selectAll_Click);
            // 
            // unSelectAll
            // 
            this.unSelectAll.Location = new System.Drawing.Point(443, 28);
            this.unSelectAll.Name = "unSelectAll";
            this.unSelectAll.Size = new System.Drawing.Size(75, 23);
            this.unSelectAll.TabIndex = 9;
            this.unSelectAll.Text = "全不选";
            this.unSelectAll.UseVisualStyleBackColor = true;
            this.unSelectAll.Click += new System.EventHandler(this.unSelectAll_Click);
            // 
            // clkBox
            // 
            this.clkBox.Controls.Add(this.gpuClk);
            this.clkBox.Controls.Add(this.cpuClk);
            this.clkBox.Location = new System.Drawing.Point(360, 228);
            this.clkBox.Name = "clkBox";
            this.clkBox.Size = new System.Drawing.Size(157, 57);
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
            this.gpuClk.CheckedChanged += new System.EventHandler(this.GpuClk_CheckedChanged);
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
            this.cpuClk.CheckedChanged += new System.EventHandler(this.CpuClk_CheckedChanged);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Text = "notifyIcon";
            this.notifyIcon1.Visible = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(535, 369);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 12;
            this.label1.Text = "刷新间隔";
            // 
            // bmpPanel
            // 
            this.bmpPanel.Controls.Add(this.label5);
            this.bmpPanel.Controls.Add(this.label4);
            this.bmpPanel.Controls.Add(this.nbxHeight);
            this.bmpPanel.Controls.Add(this.nbxWidth);
            this.bmpPanel.Controls.Add(this.selButton);
            this.bmpPanel.Controls.Add(this.customPath);
            this.bmpPanel.Controls.Add(this.label2);
            this.bmpPanel.Controls.Add(this.pictureBox);
            this.bmpPanel.Location = new System.Drawing.Point(524, 68);
            this.bmpPanel.Name = "bmpPanel";
            this.bmpPanel.Size = new System.Drawing.Size(196, 271);
            this.bmpPanel.TabIndex = 13;
            this.bmpPanel.TabStop = false;
            this.bmpPanel.Text = "动画";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(109, 223);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 12);
            this.label5.TabIndex = 25;
            this.label5.Text = "高度";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 223);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 24;
            this.label4.Text = "宽度";
            // 
            // nbxHeight
            // 
            this.nbxHeight.Location = new System.Drawing.Point(111, 238);
            this.nbxHeight.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nbxHeight.Name = "nbxHeight";
            this.nbxHeight.Size = new System.Drawing.Size(63, 21);
            this.nbxHeight.TabIndex = 23;
            this.nbxHeight.Value = new decimal(new int[] {
            64,
            0,
            0,
            0});
            // 
            // nbxWidth
            // 
            this.nbxWidth.Location = new System.Drawing.Point(13, 238);
            this.nbxWidth.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nbxWidth.Name = "nbxWidth";
            this.nbxWidth.Size = new System.Drawing.Size(63, 21);
            this.nbxWidth.TabIndex = 22;
            this.nbxWidth.Value = new decimal(new int[] {
            128,
            0,
            0,
            0});
            // 
            // selButton
            // 
            this.selButton.Enabled = false;
            this.selButton.Location = new System.Drawing.Point(147, 98);
            this.selButton.Name = "selButton";
            this.selButton.Size = new System.Drawing.Size(27, 21);
            this.selButton.TabIndex = 7;
            this.selButton.Text = "...";
            this.selButton.UseVisualStyleBackColor = true;
            this.selButton.Click += new System.EventHandler(this.SelButton_Click);
            // 
            // customPath
            // 
            this.customPath.Enabled = false;
            this.customPath.Location = new System.Drawing.Point(13, 98);
            this.customPath.Name = "customPath";
            this.customPath.Size = new System.Drawing.Size(128, 21);
            this.customPath.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "自定义文件";
            // 
            // pictureBox
            // 
            this.pictureBox.Location = new System.Drawing.Point(35, 132);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(128, 64);
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 41);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 15;
            this.label3.Text = "终端列表";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.lbxClient);
            this.panel1.Controls.Add(this.cbSendAll);
            this.panel1.Controls.Add(this.btnReboot);
            this.panel1.Controls.Add(this.btnLed);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Location = new System.Drawing.Point(14, 68);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(312, 271);
            this.panel1.TabIndex = 16;
            // 
            // lbxClient
            // 
            this.lbxClient.FormattingEnabled = true;
            this.lbxClient.ItemHeight = 12;
            this.lbxClient.Location = new System.Drawing.Point(12, 56);
            this.lbxClient.Name = "lbxClient";
            this.lbxClient.Size = new System.Drawing.Size(265, 148);
            this.lbxClient.TabIndex = 4;
            // 
            // cbSendAll
            // 
            this.cbSendAll.AutoSize = true;
            this.cbSendAll.Location = new System.Drawing.Point(12, 20);
            this.cbSendAll.Name = "cbSendAll";
            this.cbSendAll.Size = new System.Drawing.Size(120, 16);
            this.cbSendAll.TabIndex = 3;
            this.cbSendAll.Text = "发送至所有客户端";
            this.cbSendAll.UseVisualStyleBackColor = true;
            // 
            // btnReboot
            // 
            this.btnReboot.Location = new System.Drawing.Point(213, 222);
            this.btnReboot.Name = "btnReboot";
            this.btnReboot.Size = new System.Drawing.Size(64, 32);
            this.btnReboot.TabIndex = 2;
            this.btnReboot.Text = "重启";
            this.btnReboot.UseVisualStyleBackColor = true;
            this.btnReboot.Click += new System.EventHandler(this.BtnReboot_Click);
            // 
            // btnLed
            // 
            this.btnLed.Location = new System.Drawing.Point(12, 222);
            this.btnLed.Name = "btnLed";
            this.btnLed.Size = new System.Drawing.Size(64, 32);
            this.btnLed.TabIndex = 0;
            this.btnLed.Text = "开灯";
            this.btnLed.UseVisualStyleBackColor = true;
            this.btnLed.Click += new System.EventHandler(this.BtnLed_Click);
            // 
            // btnSendGif
            // 
            this.btnSendGif.Location = new System.Drawing.Point(628, 28);
            this.btnSendGif.Name = "btnSendGif";
            this.btnSendGif.Size = new System.Drawing.Size(92, 24);
            this.btnSendGif.TabIndex = 20;
            this.btnSendGif.Text = "发送动画";
            this.btnSendGif.UseVisualStyleBackColor = true;
            this.btnSendGif.Click += new System.EventHandler(this.BtnSendGif_Click);
            // 
            // rpmBox
            // 
            this.rpmBox.Controls.Add(this.gpuRpm);
            this.rpmBox.Controls.Add(this.cpuRpm);
            this.rpmBox.Location = new System.Drawing.Point(361, 291);
            this.rpmBox.Name = "rpmBox";
            this.rpmBox.Size = new System.Drawing.Size(156, 48);
            this.rpmBox.TabIndex = 17;
            this.rpmBox.TabStop = false;
            this.rpmBox.Text = "风扇转速";
            // 
            // gpuRpm
            // 
            this.gpuRpm.AutoSize = true;
            this.gpuRpm.Location = new System.Drawing.Point(81, 20);
            this.gpuRpm.Name = "gpuRpm";
            this.gpuRpm.Size = new System.Drawing.Size(42, 16);
            this.gpuRpm.TabIndex = 1;
            this.gpuRpm.Text = "GPU";
            this.gpuRpm.UseVisualStyleBackColor = true;
            this.gpuRpm.CheckedChanged += new System.EventHandler(this.GpuRpm_CheckedChanged);
            // 
            // cpuRpm
            // 
            this.cpuRpm.AutoSize = true;
            this.cpuRpm.Location = new System.Drawing.Point(12, 21);
            this.cpuRpm.Name = "cpuRpm";
            this.cpuRpm.Size = new System.Drawing.Size(42, 16);
            this.cpuRpm.TabIndex = 0;
            this.cpuRpm.Text = "CPU";
            this.cpuRpm.UseVisualStyleBackColor = true;
            this.cpuRpm.CheckedChanged += new System.EventHandler(this.CpuRpm_CheckedChanged);
            // 
            // volBox
            // 
            this.volBox.Controls.Add(this.gpuVol);
            this.volBox.Controls.Add(this.cpuVol);
            this.volBox.Location = new System.Drawing.Point(361, 347);
            this.volBox.Name = "volBox";
            this.volBox.Size = new System.Drawing.Size(156, 47);
            this.volBox.TabIndex = 18;
            this.volBox.TabStop = false;
            this.volBox.Text = "电压功耗";
            // 
            // gpuVol
            // 
            this.gpuVol.AutoSize = true;
            this.gpuVol.Location = new System.Drawing.Point(81, 20);
            this.gpuVol.Name = "gpuVol";
            this.gpuVol.Size = new System.Drawing.Size(42, 16);
            this.gpuVol.TabIndex = 1;
            this.gpuVol.Text = "GPU";
            this.gpuVol.UseVisualStyleBackColor = true;
            this.gpuVol.CheckedChanged += new System.EventHandler(this.GpuVol_CheckedChanged);
            // 
            // cpuVol
            // 
            this.cpuVol.AutoSize = true;
            this.cpuVol.Location = new System.Drawing.Point(12, 21);
            this.cpuVol.Name = "cpuVol";
            this.cpuVol.Size = new System.Drawing.Size(42, 16);
            this.cpuVol.TabIndex = 0;
            this.cpuVol.Text = "CPU";
            this.cpuVol.UseVisualStyleBackColor = true;
            this.cpuVol.CheckedChanged += new System.EventHandler(this.CpuVol_CheckedChanged);
            // 
            // timerInterval
            // 
            this.timerInterval.Location = new System.Drawing.Point(594, 365);
            this.timerInterval.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.timerInterval.Name = "timerInterval";
            this.timerInterval.Size = new System.Drawing.Size(93, 21);
            this.timerInterval.TabIndex = 19;
            this.timerInterval.Value = new decimal(new int[] {
            33,
            0,
            0,
            0});
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.制作动画包ToolStripMenuItem});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(44, 21);
            this.toolStripMenuItem1.Text = "文件";
            // 
            // 制作动画包ToolStripMenuItem
            // 
            this.制作动画包ToolStripMenuItem.Name = "制作动画包ToolStripMenuItem";
            this.制作动画包ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.制作动画包ToolStripMenuItem.Text = "制作动画包";
            this.制作动画包ToolStripMenuItem.Click += new System.EventHandler(this.制作动画包ToolStripMenuItem_Click);
            // 
            // displayTime
            // 
            this.displayTime.AutoSize = true;
            this.displayTime.Location = new System.Drawing.Point(104, 15);
            this.displayTime.Name = "displayTime";
            this.displayTime.Size = new System.Drawing.Size(72, 16);
            this.displayTime.TabIndex = 26;
            this.displayTime.Text = "显示时间";
            this.displayTime.UseVisualStyleBackColor = true;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(731, 518);
            this.Controls.Add(this.btnSendGif);
            this.Controls.Add(this.timerInterval);
            this.Controls.Add(this.volBox);
            this.Controls.Add(this.rpmBox);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.bmpPanel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.unSelectAll);
            this.Controls.Add(this.selectAll);
            this.Controls.Add(this.btnSendData);
            this.Controls.Add(this.clkBox);
            this.Controls.Add(this.utiBox);
            this.Controls.Add(this.tmpBox);
            this.Controls.Add(this.menuStrip);
            this.Controls.Add(this.logBox);
            this.MainMenuStrip = this.menuStrip;
            this.MaximizeBox = false;
            this.Name = "Main";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
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
            this.bmpPanel.ResumeLayout(false);
            this.bmpPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nbxHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nbxWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.rpmBox.ResumeLayout(false);
            this.rpmBox.PerformLayout();
            this.volBox.ResumeLayout(false);
            this.volBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.timerInterval)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem 设置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 关于ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 清空日志ToolStripMenuItem;
        private System.Windows.Forms.GroupBox tmpBox;
        private System.Windows.Forms.Button btnSendData;
        private System.Windows.Forms.CheckBox gpuTmp;
        private System.Windows.Forms.CheckBox cpuTmp;
        private System.Windows.Forms.GroupBox utiBox;
        private System.Windows.Forms.CheckBox hddTmp;
        public System.Windows.Forms.TextBox logBox;
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
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox bmpPanel;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox customPath;
        private System.Windows.Forms.Button selButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnLed;
        private System.Windows.Forms.Button btnReboot;
        private System.Windows.Forms.CheckBox cbSendAll;
        private System.Windows.Forms.CheckBox mbTmp;
        private System.Windows.Forms.GroupBox rpmBox;
        private System.Windows.Forms.CheckBox cpuRpm;
        private System.Windows.Forms.CheckBox gpuRpm;
        private System.Windows.Forms.GroupBox volBox;
        private System.Windows.Forms.CheckBox gpuVol;
        private System.Windows.Forms.CheckBox cpuVol;
        private System.Windows.Forms.NumericUpDown timerInterval;
        private System.Windows.Forms.Button btnSendGif;
        private System.Windows.Forms.ListBox lbxClient;
        private System.Windows.Forms.NumericUpDown nbxHeight;
        private System.Windows.Forms.NumericUpDown nbxWidth;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 制作动画包ToolStripMenuItem;
        private System.Windows.Forms.CheckBox displayTime;
    }
}

