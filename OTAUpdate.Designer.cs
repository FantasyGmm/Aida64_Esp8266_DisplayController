namespace Aida64_Esp8266_DisplayControler
{
    partial class OTAUpdate
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
            this.selBin = new System.Windows.Forms.Button();
            this.binPath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.startUpdate = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // selBin
            // 
            this.selBin.Location = new System.Drawing.Point(294, 33);
            this.selBin.Name = "selBin";
            this.selBin.Size = new System.Drawing.Size(25, 23);
            this.selBin.TabIndex = 0;
            this.selBin.Text = "...";
            this.selBin.UseVisualStyleBackColor = true;
            this.selBin.Click += new System.EventHandler(this.SelBin_Click);
            // 
            // binPath
            // 
            this.binPath.Location = new System.Drawing.Point(12, 33);
            this.binPath.Name = "binPath";
            this.binPath.Size = new System.Drawing.Size(276, 21);
            this.binPath.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "请选择固件路径：";
            // 
            // startUpdate
            // 
            this.startUpdate.Location = new System.Drawing.Point(129, 67);
            this.startUpdate.Name = "startUpdate";
            this.startUpdate.Size = new System.Drawing.Size(75, 23);
            this.startUpdate.TabIndex = 3;
            this.startUpdate.Text = "开始更新";
            this.startUpdate.UseVisualStyleBackColor = true;
            this.startUpdate.Click += new System.EventHandler(this.StartUpdate_Click);
            // 
            // OTAUpdate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(332, 102);
            this.Controls.Add(this.startUpdate);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.binPath);
            this.Controls.Add(this.selBin);
            this.Name = "OTAUpdate";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "OTAUpdate";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button selBin;
        private System.Windows.Forms.TextBox binPath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button startUpdate;
    }
}