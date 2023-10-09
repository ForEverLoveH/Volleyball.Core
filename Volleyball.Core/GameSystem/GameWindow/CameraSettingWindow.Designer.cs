﻿namespace Volleyball.Core.GameSystem.GameWindow
{
    partial class CameraSettingWindow
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
            this.uiTitlePanel1 = new Sunny.UI.UITitlePanel();
            this.uiButton1 = new Sunny.UI.UIButton();
            this.comboBox1 = new Sunny.UI.UIComboBox();
            this.uiLabel3 = new Sunny.UI.UILabel();
            this.comboBox2 = new Sunny.UI.UIComboBox();
            this.uiLabel2 = new Sunny.UI.UILabel();
            this.comboBox_camera = new Sunny.UI.UIComboBox();
            this.uiLabel1 = new Sunny.UI.UILabel();
            this.uiTitlePanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // uiTitlePanel1
            // 
            this.uiTitlePanel1.Controls.Add(this.uiButton1);
            this.uiTitlePanel1.Controls.Add(this.comboBox1);
            this.uiTitlePanel1.Controls.Add(this.uiLabel3);
            this.uiTitlePanel1.Controls.Add(this.comboBox2);
            this.uiTitlePanel1.Controls.Add(this.uiLabel2);
            this.uiTitlePanel1.Controls.Add(this.comboBox_camera);
            this.uiTitlePanel1.Controls.Add(this.uiLabel1);
            this.uiTitlePanel1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiTitlePanel1.Location = new System.Drawing.Point(1, 6);
            this.uiTitlePanel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiTitlePanel1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiTitlePanel1.Name = "uiTitlePanel1";
            this.uiTitlePanel1.ShowText = false;
            this.uiTitlePanel1.Size = new System.Drawing.Size(636, 444);
            this.uiTitlePanel1.TabIndex = 0;
            this.uiTitlePanel1.Text = "本地摄像头设置";
            this.uiTitlePanel1.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiButton1
            // 
            this.uiButton1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.uiButton1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiButton1.Location = new System.Drawing.Point(249, 277);
            this.uiButton1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiButton1.Name = "uiButton1";
            this.uiButton1.Size = new System.Drawing.Size(100, 35);
            this.uiButton1.TabIndex = 6;
            this.uiButton1.Text = "确定";
            this.uiButton1.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiButton1.Click += new System.EventHandler(this.uiButton1_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.DataSource = null;
            this.comboBox1.FillColor = System.Drawing.Color.White;
            this.comboBox1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBox1.Location = new System.Drawing.Point(234, 189);
            this.comboBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.comboBox1.MinimumSize = new System.Drawing.Size(63, 0);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.comboBox1.Size = new System.Drawing.Size(150, 29);
            this.comboBox1.TabIndex = 5;
            this.comboBox1.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.comboBox1.Watermark = "";
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // uiLabel3
            // 
            this.uiLabel3.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel3.Location = new System.Drawing.Point(128, 189);
            this.uiLabel3.Name = "uiLabel3";
            this.uiLabel3.Size = new System.Drawing.Size(87, 23);
            this.uiLabel3.TabIndex = 4;
            this.uiLabel3.Text = "帧数：";
            this.uiLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBox2
            // 
            this.comboBox2.DataSource = null;
            this.comboBox2.FillColor = System.Drawing.Color.White;
            this.comboBox2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBox2.Items.AddRange(new object[] {
            "1280*720"});
            this.comboBox2.Location = new System.Drawing.Point(234, 119);
            this.comboBox2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.comboBox2.MinimumSize = new System.Drawing.Size(63, 0);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.comboBox2.Size = new System.Drawing.Size(150, 29);
            this.comboBox2.TabIndex = 3;
            this.comboBox2.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.comboBox2.Watermark = "";
            // 
            // uiLabel2
            // 
            this.uiLabel2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel2.Location = new System.Drawing.Point(131, 119);
            this.uiLabel2.Name = "uiLabel2";
            this.uiLabel2.Size = new System.Drawing.Size(84, 23);
            this.uiLabel2.TabIndex = 2;
            this.uiLabel2.Text = "分辨率：";
            this.uiLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBox_camera
            // 
            this.comboBox_camera.DataSource = null;
            this.comboBox_camera.FillColor = System.Drawing.Color.White;
            this.comboBox_camera.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBox_camera.Location = new System.Drawing.Point(234, 57);
            this.comboBox_camera.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.comboBox_camera.MinimumSize = new System.Drawing.Size(63, 0);
            this.comboBox_camera.Name = "comboBox_camera";
            this.comboBox_camera.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.comboBox_camera.Size = new System.Drawing.Size(150, 29);
            this.comboBox_camera.TabIndex = 1;
            this.comboBox_camera.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.comboBox_camera.Watermark = "";
            this.comboBox_camera.SelectedIndexChanged += new System.EventHandler(this.comboBox_camera_SelectedIndexChanged);
            // 
            // uiLabel1
            // 
            this.uiLabel1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel1.Location = new System.Drawing.Point(128, 64);
            this.uiLabel1.Name = "uiLabel1";
            this.uiLabel1.Size = new System.Drawing.Size(87, 23);
            this.uiLabel1.TabIndex = 0;
            this.uiLabel1.Text = "相机：";
            this.uiLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // CameraSettingWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(642, 450);
            this.Controls.Add(this.uiTitlePanel1);
            this.MaximizeBox = false;
            this.Name = "CameraSettingWindow";
            this.Text = "CameraSettingWindow";
            this.Load += new System.EventHandler(this.CameraSettingWindow_Load);
            this.uiTitlePanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Sunny.UI.UITitlePanel uiTitlePanel1;
        private Sunny.UI.UILabel uiLabel1;
        private Sunny.UI.UIButton uiButton1;
        private Sunny.UI.UIComboBox comboBox1;
        private Sunny.UI.UILabel uiLabel3;
        private Sunny.UI.UIComboBox comboBox2;
        private Sunny.UI.UILabel uiLabel2;
        private Sunny.UI.UIComboBox comboBox_camera;
    }
}