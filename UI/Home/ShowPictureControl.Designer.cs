namespace YiRongMachine
{
    partial class ShowPictureControl
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.ckbSaveNG = new System.Windows.Forms.CheckBox();
            this.ckbForbidden = new System.Windows.Forms.CheckBox();
            this.lblResult = new System.Windows.Forms.Label();
            this.lblSignCamera = new System.Windows.Forms.Label();
            this.lblSignPicture = new System.Windows.Forms.Label();
            this.lblSignStart = new System.Windows.Forms.Label();
            this.lblGrayNum = new System.Windows.Forms.Label();
            this.txtShowError = new System.Windows.Forms.TextBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.picb = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picb)).BeginInit();
            this.SuspendLayout();
            // 
            // ckbSaveNG
            // 
            this.ckbSaveNG.AutoSize = true;
            this.ckbSaveNG.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ckbSaveNG.Location = new System.Drawing.Point(736, 490);
            this.ckbSaveNG.Margin = new System.Windows.Forms.Padding(4);
            this.ckbSaveNG.Name = "ckbSaveNG";
            this.ckbSaveNG.Size = new System.Drawing.Size(108, 19);
            this.ckbSaveNG.TabIndex = 9;
            this.ckbSaveNG.Text = "存NG图";
            this.ckbSaveNG.UseVisualStyleBackColor = true;
            this.ckbSaveNG.CheckedChanged += new System.EventHandler(this.ckbSaveNG_CheckedChanged);
            // 
            // ckbForbidden
            // 
            this.ckbForbidden.AutoSize = true;
            this.ckbForbidden.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ckbForbidden.Location = new System.Drawing.Point(980, 490);
            this.ckbForbidden.Margin = new System.Windows.Forms.Padding(4);
            this.ckbForbidden.Name = "ckbForbidden";
            this.ckbForbidden.Size = new System.Drawing.Size(84, 19);
            this.ckbForbidden.TabIndex = 0;
            this.ckbForbidden.Text = "禁用";
            this.ckbForbidden.UseVisualStyleBackColor = true;
            this.ckbForbidden.CheckedChanged += new System.EventHandler(this.ckbForbidden_CheckedChanged);
            // 
            // lblResult
            // 
            this.lblResult.AutoSize = true;
            this.lblResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblResult.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblResult.Location = new System.Drawing.Point(736, 459);
            this.lblResult.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(236, 27);
            this.lblResult.TabIndex = 8;
            this.lblResult.Text = "Result";
            this.lblResult.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblSignCamera
            // 
            this.lblSignCamera.AutoSize = true;
            this.lblSignCamera.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSignCamera.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSignCamera.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblSignCamera.Location = new System.Drawing.Point(248, 459);
            this.lblSignCamera.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSignCamera.Name = "lblSignCamera";
            this.lblSignCamera.Size = new System.Drawing.Size(236, 27);
            this.lblSignCamera.TabIndex = 5;
            this.lblSignCamera.Text = "相机";
            this.lblSignCamera.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblSignPicture
            // 
            this.lblSignPicture.AutoSize = true;
            this.lblSignPicture.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSignPicture.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSignPicture.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblSignPicture.Location = new System.Drawing.Point(492, 459);
            this.lblSignPicture.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSignPicture.Name = "lblSignPicture";
            this.lblSignPicture.Size = new System.Drawing.Size(236, 27);
            this.lblSignPicture.TabIndex = 4;
            this.lblSignPicture.Text = "图像";
            this.lblSignPicture.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblSignStart
            // 
            this.lblSignStart.AutoSize = true;
            this.lblSignStart.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSignStart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSignStart.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblSignStart.Location = new System.Drawing.Point(4, 459);
            this.lblSignStart.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSignStart.Name = "lblSignStart";
            this.lblSignStart.Size = new System.Drawing.Size(236, 27);
            this.lblSignStart.TabIndex = 3;
            this.lblSignStart.Text = "触发";
            this.lblSignStart.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblGrayNum
            // 
            this.lblGrayNum.AutoSize = true;
            this.lblGrayNum.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblGrayNum.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblGrayNum.Location = new System.Drawing.Point(980, 459);
            this.lblGrayNum.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblGrayNum.Name = "lblGrayNum";
            this.lblGrayNum.Size = new System.Drawing.Size(239, 27);
            this.lblGrayNum.TabIndex = 2;
            this.lblGrayNum.Text = "灰度：";
            this.lblGrayNum.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtShowError
            // 
            this.tableLayoutPanel3.SetColumnSpan(this.txtShowError, 3);
            this.txtShowError.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtShowError.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtShowError.Location = new System.Drawing.Point(4, 490);
            this.txtShowError.Margin = new System.Windows.Forms.Padding(4);
            this.txtShowError.Multiline = true;
            this.txtShowError.Name = "txtShowError";
            this.txtShowError.ReadOnly = true;
            this.tableLayoutPanel3.SetRowSpan(this.txtShowError, 2);
            this.txtShowError.Size = new System.Drawing.Size(724, 47);
            this.txtShowError.TabIndex = 6;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel3.SetColumnSpan(this.lblTitle, 2);
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitle.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblTitle.Location = new System.Drawing.Point(736, 513);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(483, 28);
            this.lblTitle.TabIndex = 10;
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 5;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel3.Controls.Add(this.lblTitle, 0, 3);
            this.tableLayoutPanel3.Controls.Add(this.txtShowError, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.lblGrayNum, 4, 1);
            this.tableLayoutPanel3.Controls.Add(this.lblSignStart, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.lblSignPicture, 2, 1);
            this.tableLayoutPanel3.Controls.Add(this.lblSignCamera, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.lblResult, 3, 1);
            this.tableLayoutPanel3.Controls.Add(this.ckbForbidden, 4, 2);
            this.tableLayoutPanel3.Controls.Add(this.ckbSaveNG, 3, 2);
            this.tableLayoutPanel3.Controls.Add(this.picb, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 4;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 85F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(1223, 541);
            this.tableLayoutPanel3.TabIndex = 3;
            // 
            // picb
            // 
            this.tableLayoutPanel3.SetColumnSpan(this.picb, 5);
            this.picb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picb.Location = new System.Drawing.Point(3, 3);
            this.picb.Name = "picb";
            this.picb.Size = new System.Drawing.Size(1217, 453);
            this.picb.TabIndex = 11;
            this.picb.TabStop = false;
            // 
            // ShowPictureControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.tableLayoutPanel3);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "ShowPictureControl";
            this.Size = new System.Drawing.Size(1223, 541);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picb)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox ckbSaveNG;
        private System.Windows.Forms.CheckBox ckbForbidden;
        private System.Windows.Forms.Label lblResult;
        private System.Windows.Forms.Label lblSignCamera;
        private System.Windows.Forms.Label lblSignPicture;
        private System.Windows.Forms.Label lblSignStart;
        private System.Windows.Forms.Label lblGrayNum;
        private System.Windows.Forms.TextBox txtShowError;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.PictureBox picb;
    }
}
