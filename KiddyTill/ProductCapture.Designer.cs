namespace KiddyTill
{
    partial class ProductCapture
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
            this.pctDisplay = new System.Windows.Forms.PictureBox();
            this.btnStartCamera = new System.Windows.Forms.Button();
            this.btnSaveProduct = new System.Windows.Forms.Button();
            this.cmbCameras = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtProductDescription = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtPrice = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.txtBarCode = new System.Windows.Forms.TextBox();
            this.chkMirrorImage = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.pctDisplay)).BeginInit();
            this.SuspendLayout();
            // 
            // pctDisplay
            // 
            this.pctDisplay.Location = new System.Drawing.Point(9, 8);
            this.pctDisplay.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pctDisplay.Name = "pctDisplay";
            this.pctDisplay.Size = new System.Drawing.Size(640, 480);
            this.pctDisplay.TabIndex = 0;
            this.pctDisplay.TabStop = false;
            // 
            // btnStartCamera
            // 
            this.btnStartCamera.Location = new System.Drawing.Point(9, 578);
            this.btnStartCamera.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnStartCamera.Name = "btnStartCamera";
            this.btnStartCamera.Size = new System.Drawing.Size(107, 33);
            this.btnStartCamera.TabIndex = 4;
            this.btnStartCamera.Text = "Start Camera";
            this.btnStartCamera.UseVisualStyleBackColor = true;
            this.btnStartCamera.Click += new System.EventHandler(this.btnStartCamera_Click);
            // 
            // btnSaveProduct
            // 
            this.btnSaveProduct.Location = new System.Drawing.Point(122, 578);
            this.btnSaveProduct.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnSaveProduct.Name = "btnSaveProduct";
            this.btnSaveProduct.Size = new System.Drawing.Size(107, 33);
            this.btnSaveProduct.TabIndex = 5;
            this.btnSaveProduct.Text = "Save Product";
            this.btnSaveProduct.UseVisualStyleBackColor = true;
            this.btnSaveProduct.Click += new System.EventHandler(this.btnSaveProduct_Click);
            // 
            // cmbCameras
            // 
            this.cmbCameras.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCameras.FormattingEnabled = true;
            this.cmbCameras.Location = new System.Drawing.Point(327, 584);
            this.cmbCameras.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cmbCameras.Name = "cmbCameras";
            this.cmbCameras.Size = new System.Drawing.Size(174, 21);
            this.cmbCameras.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(237, 588);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Camera Device:";
            // 
            // txtProductDescription
            // 
            this.txtProductDescription.Location = new System.Drawing.Point(122, 498);
            this.txtProductDescription.Name = "txtProductDescription";
            this.txtProductDescription.Size = new System.Drawing.Size(527, 20);
            this.txtProductDescription.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 501);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(103, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Product Description:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 527);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Price:";
            // 
            // txtPrice
            // 
            this.txtPrice.Location = new System.Drawing.Point(122, 524);
            this.txtPrice.Name = "txtPrice";
            this.txtPrice.Size = new System.Drawing.Size(104, 20);
            this.txtPrice.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 552);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(438, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "(Press PgUp or PgDown to start/stop webcam. Scan barcode to capture and save prod" +
    "uct)";
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnClose.Location = new System.Drawing.Point(542, 577);
            this.btnClose.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(107, 33);
            this.btnClose.TabIndex = 7;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(236, 527);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(119, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Barcode (manual entry):";
            // 
            // txtBarCode
            // 
            this.txtBarCode.Location = new System.Drawing.Point(361, 524);
            this.txtBarCode.Name = "txtBarCode";
            this.txtBarCode.Size = new System.Drawing.Size(288, 20);
            this.txtBarCode.TabIndex = 2;
            // 
            // chkMirrorImage
            // 
            this.chkMirrorImage.AutoSize = true;
            this.chkMirrorImage.Location = new System.Drawing.Point(544, 551);
            this.chkMirrorImage.Name = "chkMirrorImage";
            this.chkMirrorImage.Size = new System.Drawing.Size(93, 17);
            this.chkMirrorImage.TabIndex = 3;
            this.chkMirrorImage.Text = "Mirror Preview";
            this.chkMirrorImage.UseVisualStyleBackColor = true;
            // 
            // ProductCapture
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(659, 622);
            this.Controls.Add(this.chkMirrorImage);
            this.Controls.Add(this.txtBarCode);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtPrice);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtProductDescription);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbCameras);
            this.Controls.Add(this.btnSaveProduct);
            this.Controls.Add(this.btnStartCamera);
            this.Controls.Add(this.pctDisplay);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProductCapture";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ProductCapture";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ProductCapture_FormClosed);
            this.Load += new System.EventHandler(this.ProductCapture_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pctDisplay)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pctDisplay;
        private System.Windows.Forms.Button btnStartCamera;
        private System.Windows.Forms.Button btnSaveProduct;
        private System.Windows.Forms.ComboBox cmbCameras;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtProductDescription;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtPrice;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtBarCode;
        private System.Windows.Forms.CheckBox chkMirrorImage;
    }
}