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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.清空日志ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.关于ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.hddBox = new System.Windows.Forms.CheckBox();
            this.ramBox = new System.Windows.Forms.CheckBox();
            this.gpuBox = new System.Windows.Forms.CheckBox();
            this.cpuBox = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.groupbox2 = new System.Windows.Forms.GroupBox();
            this.logBox = new System.Windows.Forms.TextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.设置ToolStripMenuItem,
            this.关于ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(434, 25);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
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
            // 
            // 关于ToolStripMenuItem
            // 
            this.关于ToolStripMenuItem.Name = "关于ToolStripMenuItem";
            this.关于ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.关于ToolStripMenuItem.Text = "关于";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.hddBox);
            this.groupBox1.Controls.Add(this.ramBox);
            this.groupBox1.Controls.Add(this.gpuBox);
            this.groupBox1.Controls.Add(this.cpuBox);
            this.groupBox1.Location = new System.Drawing.Point(14, 62);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 71);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "温度";
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
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(14, 28);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "启动服务器";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupbox2
            // 
            this.groupbox2.Location = new System.Drawing.Point(14, 139);
            this.groupbox2.Name = "groupbox2";
            this.groupbox2.Size = new System.Drawing.Size(200, 100);
            this.groupbox2.TabIndex = 5;
            this.groupbox2.TabStop = false;
            this.groupbox2.Text = "使用率";
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
            // timer1
            // 
            this.timer1.Interval = 3000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 461);
            this.Controls.Add(this.logBox);
            this.Controls.Add(this.groupbox2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Main";
            this.ShowIcon = false;
            this.Text = "Aida64_Esp8266_DisplayControler";
            this.Load += new System.EventHandler(this.Main_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 设置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 关于ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 清空日志ToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox ramBox;
        private System.Windows.Forms.CheckBox gpuBox;
        private System.Windows.Forms.CheckBox cpuBox;
        private System.Windows.Forms.GroupBox groupbox2;
        private System.Windows.Forms.CheckBox hddBox;
        public System.Windows.Forms.TextBox logBox;
        private System.Windows.Forms.Timer timer1;
    }
}

