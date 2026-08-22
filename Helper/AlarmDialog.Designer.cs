namespace YiRongMachine
{
    partial class AlarmDialog
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
            this.btnAlarmMsg = new System.Windows.Forms.Button();
            this.btnRestart = new System.Windows.Forms.Button();
            this.btnNG = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnAlarmMsg
            // 
            this.btnAlarmMsg.Font = new System.Drawing.Font("宋体", 14.1F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnAlarmMsg.Location = new System.Drawing.Point(4, 5);
            this.btnAlarmMsg.Margin = new System.Windows.Forms.Padding(2);
            this.btnAlarmMsg.Name = "btnAlarmMsg";
            this.btnAlarmMsg.Size = new System.Drawing.Size(662, 162);
            this.btnAlarmMsg.TabIndex = 0;
            this.btnAlarmMsg.Text = "1123132";
            this.btnAlarmMsg.UseVisualStyleBackColor = true;
            this.btnAlarmMsg.Click += new System.EventHandler(this.btnAlarmMsg_Click);
            // 
            // btnRestart
            // 
            this.btnRestart.Font = new System.Drawing.Font("宋体", 14.1F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnRestart.Location = new System.Drawing.Point(57, 185);
            this.btnRestart.Margin = new System.Windows.Forms.Padding(2);
            this.btnRestart.Name = "btnRestart";
            this.btnRestart.Size = new System.Drawing.Size(167, 46);
            this.btnRestart.TabIndex = 1;
            this.btnRestart.Text = "重试";
            this.btnRestart.UseVisualStyleBackColor = true;
            this.btnRestart.Click += new System.EventHandler(this.btnRestart_Click);
            // 
            // btnNG
            // 
            this.btnNG.Font = new System.Drawing.Font("宋体", 14.1F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnNG.Location = new System.Drawing.Point(424, 185);
            this.btnNG.Margin = new System.Windows.Forms.Padding(2);
            this.btnNG.Name = "btnNG";
            this.btnNG.Size = new System.Drawing.Size(204, 46);
            this.btnNG.TabIndex = 2;
            this.btnNG.Text = "NG";
            this.btnNG.UseVisualStyleBackColor = true;
            this.btnNG.Click += new System.EventHandler(this.btnNG_Click);
            // 
            // btnOK
            // 
            this.btnOK.Font = new System.Drawing.Font("宋体", 14.1F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOK.Location = new System.Drawing.Point(228, 185);
            this.btnOK.Margin = new System.Windows.Forms.Padding(2);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(192, 46);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "确认";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // AlarmDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(668, 239);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnNG);
            this.Controls.Add(this.btnRestart);
            this.Controls.Add(this.btnAlarmMsg);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "AlarmDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "报警界面";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnAlarmMsg;
        private System.Windows.Forms.Button btnRestart;
        private System.Windows.Forms.Button btnNG;
        private System.Windows.Forms.Button btnOK;
    }
}