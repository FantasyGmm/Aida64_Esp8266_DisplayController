namespace Aida64_Esp8266_DisplayControler
{
    partial class PackForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnMain = new System.Windows.Forms.Panel();
            this.tbxPath = new System.Windows.Forms.TextBox();
            this.btnBrowser = new System.Windows.Forms.Button();
            this.nbxWidth = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.nbxHeight = new System.Windows.Forms.NumericUpDown();
            this.pbar = new System.Windows.Forms.ProgressBar();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.pnMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nbxWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nbxHeight)).BeginInit();
            this.SuspendLayout();
            // 
            // pnMain
            // 
            this.pnMain.Controls.Add(this.btnStop);
            this.pnMain.Controls.Add(this.btnStart);
            this.pnMain.Controls.Add(this.pbar);
            this.pnMain.Controls.Add(this.label2);
            this.pnMain.Controls.Add(this.nbxHeight);
            this.pnMain.Controls.Add(this.label1);
            this.pnMain.Controls.Add(this.nbxWidth);
            this.pnMain.Controls.Add(this.btnBrowser);
            this.pnMain.Controls.Add(this.tbxPath);
            this.pnMain.Location = new System.Drawing.Point(12, 12);
            this.pnMain.Name = "pnMain";
            this.pnMain.Size = new System.Drawing.Size(530, 254);
            this.pnMain.TabIndex = 0;
            // 
            // tbxPath
            // 
            this.tbxPath.Location = new System.Drawing.Point(12, 26);
            this.tbxPath.Name = "tbxPath";
            this.tbxPath.Size = new System.Drawing.Size(404, 21);
            this.tbxPath.TabIndex = 0;
            // 
            // btnBrowser
            // 
            this.btnBrowser.Location = new System.Drawing.Point(447, 26);
            this.btnBrowser.Name = "btnBrowser";
            this.btnBrowser.Size = new System.Drawing.Size(67, 21);
            this.btnBrowser.TabIndex = 1;
            this.btnBrowser.Text = "浏览";
            this.btnBrowser.UseVisualStyleBackColor = true;
            this.btnBrowser.Click += new System.EventHandler(this.btnBrowser_Click);
            // 
            // nbxWidth
            // 
            this.nbxWidth.Location = new System.Drawing.Point(97, 73);
            this.nbxWidth.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nbxWidth.Name = "nbxWidth";
            this.nbxWidth.Size = new System.Drawing.Size(87, 21);
            this.nbxWidth.TabIndex = 2;
            this.nbxWidth.Value = new decimal(new int[] {
            128,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(95, 58);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "宽度";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(237, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "高度";
            // 
            // nbxHeight
            // 
            this.nbxHeight.Location = new System.Drawing.Point(239, 73);
            this.nbxHeight.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nbxHeight.Name = "nbxHeight";
            this.nbxHeight.Size = new System.Drawing.Size(87, 21);
            this.nbxHeight.TabIndex = 4;
            this.nbxHeight.Value = new decimal(new int[] {
            64,
            0,
            0,
            0});
            // 
            // pbar
            // 
            this.pbar.Location = new System.Drawing.Point(12, 127);
            this.pbar.Name = "pbar";
            this.pbar.Size = new System.Drawing.Size(502, 15);
            this.pbar.TabIndex = 6;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(115, 197);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(102, 31);
            this.btnStart.TabIndex = 7;
            this.btnStart.Text = "开始";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(290, 197);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(102, 31);
            this.btnStop.TabIndex = 8;
            this.btnStop.Text = "停止";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // PackForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(553, 280);
            this.Controls.Add(this.pnMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PackForm";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "打包图片";
            this.Load += new System.EventHandler(this.PackForm_Load);
            this.pnMain.ResumeLayout(false);
            this.pnMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nbxWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nbxHeight)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnMain;
        private System.Windows.Forms.NumericUpDown nbxWidth;
        private System.Windows.Forms.Button btnBrowser;
        private System.Windows.Forms.TextBox tbxPath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nbxHeight;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ProgressBar pbar;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnStart;
    }
}