namespace KiddyTill
{
    partial class Options
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
            this.label2 = new System.Windows.Forms.Label();
            this.btnChoseLocation = new System.Windows.Forms.Button();
            this.txtProductsLocation = new System.Windows.Forms.TextBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Products location:";
            // 
            // btnChoseLocation
            // 
            this.btnChoseLocation.Location = new System.Drawing.Point(11, 56);
            this.btnChoseLocation.Name = "btnChoseLocation";
            this.btnChoseLocation.Size = new System.Drawing.Size(127, 33);
            this.btnChoseLocation.TabIndex = 1;
            this.btnChoseLocation.Text = "Chose...";
            this.btnChoseLocation.UseVisualStyleBackColor = true;
            this.btnChoseLocation.Click += new System.EventHandler(this.btnChoseLocation_Click);
            // 
            // txtProductsLocation
            // 
            this.txtProductsLocation.Location = new System.Drawing.Point(12, 30);
            this.txtProductsLocation.Name = "txtProductsLocation";
            this.txtProductsLocation.ReadOnly = true;
            this.txtProductsLocation.Size = new System.Drawing.Size(429, 20);
            this.txtProductsLocation.TabIndex = 0;
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnClose.Location = new System.Drawing.Point(315, 237);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(127, 32);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "OK";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(182, 237);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(127, 32);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // Options
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(454, 281);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.txtProductsLocation);
            this.Controls.Add(this.btnChoseLocation);
            this.Controls.Add(this.label2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Options";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Options";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Options_FormClosed);
            this.Load += new System.EventHandler(this.Options_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnChoseLocation;
        private System.Windows.Forms.TextBox txtProductsLocation;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnCancel;
    }
}