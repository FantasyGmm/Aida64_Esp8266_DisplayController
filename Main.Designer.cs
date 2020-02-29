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
            this.hddBox = new System.Windows.Forms.CheckBox();
            this.ramBox = new System.Windows.Forms.CheckBox();
            this.gpuBox = new System.Windows.Forms.CheckBox();
            this.cpuBox = new System.Windows.Forms.CheckBox();
            this.runServer = new System.Windows.Forms.Button();
            this.uitBox = new System.Windows.Forms.GroupBox();
            this.logBox = new System.Windows.Forms.TextBox();
            this.getData = new System.Windows.Forms.Timer(this.components);
            this.pauseSend = new System.Windows.Forms.Button();
            this.selectAll = new System.Windows.Forms.Button();
            this.unSelectAll = new System.Windows.Forms.Button();
            this.menuStrip.SuspendLayout();
            this.tmpBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.设置ToolStripMenuItem,
            this.关于ToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(434, 25);
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
            this.tmpBox.Controls.Add(this.hddBox);
            this.tmpBox.Controls.Add(this.ramBox);
            this.tmpBox.Controls.Add(this.gpuBox);
            this.tmpBox.Controls.Add(this.cpuBox);
            this.tmpBox.Location = new System.Drawing.Point(14, 62);
            this.tmpBox.Name = "tmpBox";
            this.tmpBox.Size = new System.Drawing.Size(200, 71);
            this.tmpBox.TabIndex = 1;
            this.tmpBox.TabStop = false;
            this.tmpBox.Text = "温度";
            // 
            // hddBox
            // 
            this.hddBox.AutoSize = true;
            this.hddBox.Location = new System.Drawing.Point(56, 44);
            this.hddBox.Name = "hddBox";
            this.hddBox.Size = new System.Drawing.Size(42, 16);
            this.hddBox.TabIndex = 3;
            this.hddBox.Text = "HDD";
            this.hddBox.UseVisualStyleBackColor = true;
            // 
            // ramBox
            // 
            this.ramBox.AutoSize = true;
            this.ramBox.Location = new System.Drawing.Point(55, 20);
            this.ramBox.Name = "ramBox";
            this.ramBox.Size = new System.Drawing.Size(42, 16);
            this.ramBox.TabIndex = 2;
            this.ramBox.Text = "RAM";
            this.ramBox.UseVisualStyleBackColor = true;
            // 
            // gpuBox
            // 
            this.gpuBox.AutoSize = true;
            this.gpuBox.Location = new System.Drawing.Point(7, 44);
            this.gpuBox.Name = "gpuBox";
            this.gpuBox.Size = new System.Drawing.Size(42, 16);
            this.gpuBox.TabIndex = 1;
            this.gpuBox.Text = "GPU";
            this.gpuBox.UseVisualStyleBackColor = true;
            // 
            // cpuBox
            // 
            this.cpuBox.AutoSize = true;
            this.cpuBox.Location = new System.Drawing.Point(7, 21);
            this.cpuBox.Name = "cpuBox";
            this.cpuBox.Size = new System.Drawing.Size(42, 16);
            this.cpuBox.TabIndex = 0;
            this.cpuBox.Text = "CPU";
            this.cpuBox.UseVisualStyleBackColor = true;
            // 
            // runServer
            // 
            this.runServer.Location = new System.Drawing.Point(14, 28);
            this.runServer.Name = "runServer";
            this.runServer.Size = new System.Drawing.Size(75, 23);
            this.runServer.TabIndex = 4;
            this.runServer.Text = "启动服务器";
            this.runServer.UseVisualStyleBackColor = true;
            this.runServer.Click += new System.EventHandler(this.button1_Click);
            // 
            // uitBox
            // 
            this.uitBox.Location = new System.Drawing.Point(14, 139);
            this.uitBox.Name = "uitBox";
            this.uitBox.Size = new System.Drawing.Size(200, 100);
            this.uitBox.TabIndex = 5;
            this.uitBox.TabStop = false;
            this.uitBox.Text = "使用率";
            // 
            // logBox
            // 
            this.logBox.Location = new System.Drawing.Point(14, 338);
            this.logBox.Multiline = true;
            this.logBox.Name = "logBox";
            this.logBox.ReadOnly = true;
            this.logBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.logBox.Size = new System.Drawing.Size(411, 111);
            this.logBox.TabIndex = 6;
            // 
            // getData
            // 
            this.getData.Interval = 3000;
            this.getData.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // pauseSend
            // 
            this.pauseSend.Location = new System.Drawing.Point(96, 27);
            this.pauseSend.Name = "pauseSend";
            this.pauseSend.Size = new System.Drawing.Size(75, 23);
            this.pauseSend.TabIndex = 7;
            this.pauseSend.Text = "暂停发送";
            this.pauseSend.UseVisualStyleBackColor = true;
            this.pauseSend.Click += new System.EventHandler(this.button2_Click);
            // 
            // selectAll
            // 
            this.selectAll.Location = new System.Drawing.Point(178, 26);
            this.selectAll.Name = "selectAll";
            this.selectAll.Size = new System.Drawing.Size(75, 23);
            this.selectAll.TabIndex = 8;
            this.selectAll.Text = "全选";
            this.selectAll.UseVisualStyleBackColor = true;
            // 
            // unSelectAll
            // 
            this.unSelectAll.Location = new System.Drawing.Point(260, 25);
            this.unSelectAll.Name = "unSelectAll";
            this.unSelectAll.Size = new System.Drawing.Size(75, 23);
            this.unSelectAll.TabIndex = 9;
            this.unSelectAll.Text = "全不选";
            this.unSelectAll.UseVisualStyleBackColor = true;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 461);
            this.Controls.Add(this.unSelectAll);
            this.Controls.Add(this.selectAll);
            this.Controls.Add(this.pauseSend);
            this.Controls.Add(this.logBox);
            this.Controls.Add(this.uitBox);
            this.Controls.Add(this.runServer);
            this.Controls.Add(this.tmpBox);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.Name = "Main";
            this.ShowIcon = false;
            this.Text = "Aida64_Esp8266_DisplayControler";
            this.Load += new System.EventHandler(this.Main_Load);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.tmpBox.ResumeLayout(false);
            this.tmpBox.PerformLayout();
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
        private System.Windows.Forms.CheckBox ramBox;
        private System.Windows.Forms.CheckBox gpuBox;
        private System.Windows.Forms.CheckBox cpuBox;
        private System.Windows.Forms.GroupBox uitBox;
        private System.Windows.Forms.CheckBox hddBox;
        public System.Windows.Forms.TextBox logBox;
        private System.Windows.Forms.Timer getData;
        private System.Windows.Forms.Button pauseSend;
        private System.Windows.Forms.Button selectAll;
        private System.Windows.Forms.Button unSelectAll;
    }
}

