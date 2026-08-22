namespace YiRongMachine
{
    partial class ParamSettingForm
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageDuanMianA = new System.Windows.Forms.TabPage();
            this.tabPageWaiYuanA = new System.Windows.Forms.TabPage();
            this.tabPageDuanMianB = new System.Windows.Forms.TabPage();
            this.btnSave = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tabControl1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageDuanMianA);
            this.tabControl1.Controls.Add(this.tabPageWaiYuanA);
            this.tabControl1.Controls.Add(this.tabPageDuanMianB);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(3, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1488, 793);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPageDuanMianA
            // 
            this.tabPageDuanMianA.Location = new System.Drawing.Point(4, 22);
            this.tabPageDuanMianA.Name = "tabPageDuanMianA";
            this.tabPageDuanMianA.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDuanMianA.Size = new System.Drawing.Size(1480, 767);
            this.tabPageDuanMianA.TabIndex = 0;
            this.tabPageDuanMianA.Text = "端面A设置";
            this.tabPageDuanMianA.UseVisualStyleBackColor = true;
            // 
            // tabPageWaiYuanA
            // 
            this.tabPageWaiYuanA.Location = new System.Drawing.Point(4, 22);
            this.tabPageWaiYuanA.Name = "tabPageWaiYuanA";
            this.tabPageWaiYuanA.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageWaiYuanA.Size = new System.Drawing.Size(1269, 625);
            this.tabPageWaiYuanA.TabIndex = 5;
            this.tabPageWaiYuanA.Text = "外圆A设置";
            this.tabPageWaiYuanA.UseVisualStyleBackColor = true;
            // 
            // tabPageDuanMianB
            // 
            this.tabPageDuanMianB.Location = new System.Drawing.Point(4, 22);
            this.tabPageDuanMianB.Margin = new System.Windows.Forms.Padding(2);
            this.tabPageDuanMianB.Name = "tabPageDuanMianB";
            this.tabPageDuanMianB.Size = new System.Drawing.Size(1269, 625);
            this.tabPageDuanMianB.TabIndex = 8;
            this.tabPageDuanMianB.Text = "端面B设置";
            this.tabPageDuanMianB.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnSave.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSave.Location = new System.Drawing.Point(635, 802);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(224, 44);
            this.btnSave.TabIndex = 8;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tabControl1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnSave, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1494, 849);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // ParamSettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1494, 849);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ParamSettingForm";
            this.Text = "参数设置界面";
            this.tabControl1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageDuanMianA;
        private System.Windows.Forms.TabPage tabPageWaiYuanA;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TabPage tabPageDuanMianB;
    }
}