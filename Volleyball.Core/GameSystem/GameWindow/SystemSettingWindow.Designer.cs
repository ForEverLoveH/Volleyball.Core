namespace Volleyball.Core.GameSystem.GameWindow
{
    partial class SystemSettingWindow
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
            this.确定玫瑰花 = new Sunny.UI.UIButton();
            this.txt_FloatType = new Sunny.UI.UIComboBox();
            this.uiLabel5 = new Sunny.UI.UILabel();
            this.txt_TestMethod = new Sunny.UI.UIComboBox();
            this.uiLabel4 = new Sunny.UI.UILabel();
            this.txt_BestScoreMode = new Sunny.UI.UIComboBox();
            this.uiLabel3 = new Sunny.UI.UILabel();
            this.txt_RoundCount = new Sunny.UI.UIComboBox();
            this.uiLabel2 = new Sunny.UI.UILabel();
            this.txt_projectName = new Sunny.UI.UITextBox();
            this.uiLabel1 = new Sunny.UI.UILabel();
            this.uiTitlePanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // uiTitlePanel1
            // 
            this.uiTitlePanel1.Controls.Add(this.确定玫瑰花);
            this.uiTitlePanel1.Controls.Add(this.txt_FloatType);
            this.uiTitlePanel1.Controls.Add(this.uiLabel5);
            this.uiTitlePanel1.Controls.Add(this.txt_TestMethod);
            this.uiTitlePanel1.Controls.Add(this.uiLabel4);
            this.uiTitlePanel1.Controls.Add(this.txt_BestScoreMode);
            this.uiTitlePanel1.Controls.Add(this.uiLabel3);
            this.uiTitlePanel1.Controls.Add(this.txt_RoundCount);
            this.uiTitlePanel1.Controls.Add(this.uiLabel2);
            this.uiTitlePanel1.Controls.Add(this.txt_projectName);
            this.uiTitlePanel1.Controls.Add(this.uiLabel1);
            this.uiTitlePanel1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiTitlePanel1.Location = new System.Drawing.Point(1, 3);
            this.uiTitlePanel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiTitlePanel1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiTitlePanel1.Name = "uiTitlePanel1";
            this.uiTitlePanel1.ShowText = false;
            this.uiTitlePanel1.Size = new System.Drawing.Size(800, 446);
            this.uiTitlePanel1.TabIndex = 0;
            this.uiTitlePanel1.Text = "系统设置";
            this.uiTitlePanel1.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // 确定玫瑰花
            // 
            this.确定玫瑰花.Cursor = System.Windows.Forms.Cursors.Hand;
            this.确定玫瑰花.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.确定玫瑰花.Location = new System.Drawing.Point(322, 380);
            this.确定玫瑰花.MinimumSize = new System.Drawing.Size(1, 1);
            this.确定玫瑰花.Name = "确定玫瑰花";
            this.确定玫瑰花.Size = new System.Drawing.Size(100, 35);
            this.确定玫瑰花.TabIndex = 10;
            this.确定玫瑰花.Text = "确定";
            this.确定玫瑰花.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.确定玫瑰花.Click += new System.EventHandler(this.确定玫瑰花_Click);
            // 
            // txt_FloatType
            // 
            this.txt_FloatType.DataSource = null;
            this.txt_FloatType.FillColor = System.Drawing.Color.White;
            this.txt_FloatType.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txt_FloatType.Items.AddRange(new object[] {
            "小数点后0位",
            "小数点后1位",
            "小数点后2位",
            "小数点后3位",
            "小数点后4位"});
            this.txt_FloatType.Location = new System.Drawing.Point(287, 307);
            this.txt_FloatType.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txt_FloatType.MinimumSize = new System.Drawing.Size(63, 0);
            this.txt_FloatType.Name = "txt_FloatType";
            this.txt_FloatType.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.txt_FloatType.Size = new System.Drawing.Size(196, 29);
            this.txt_FloatType.TabIndex = 9;
            this.txt_FloatType.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txt_FloatType.Watermark = "";
            this.txt_FloatType.SelectedIndexChanged += new System.EventHandler(this.txt_FloatType_SelectedIndexChanged);
            // 
            // uiLabel5
            // 
            this.uiLabel5.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel5.Location = new System.Drawing.Point(149, 307);
            this.uiLabel5.Name = "uiLabel5";
            this.uiLabel5.Size = new System.Drawing.Size(100, 23);
            this.uiLabel5.TabIndex = 8;
            this.uiLabel5.Text = "保留位数：";
            this.uiLabel5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txt_TestMethod
            // 
            this.txt_TestMethod.DataSource = null;
            this.txt_TestMethod.FillColor = System.Drawing.Color.White;
            this.txt_TestMethod.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txt_TestMethod.Items.AddRange(new object[] {
            "自动下一位",
            "自动下一轮"});
            this.txt_TestMethod.Location = new System.Drawing.Point(287, 243);
            this.txt_TestMethod.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txt_TestMethod.MinimumSize = new System.Drawing.Size(63, 0);
            this.txt_TestMethod.Name = "txt_TestMethod";
            this.txt_TestMethod.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.txt_TestMethod.Size = new System.Drawing.Size(196, 29);
            this.txt_TestMethod.TabIndex = 7;
            this.txt_TestMethod.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txt_TestMethod.Watermark = "";
            this.txt_TestMethod.SelectedIndexChanged += new System.EventHandler(this.txt_TestMethod_SelectedIndexChanged);
            // 
            // uiLabel4
            // 
            this.uiLabel4.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel4.Location = new System.Drawing.Point(149, 243);
            this.uiLabel4.Name = "uiLabel4";
            this.uiLabel4.Size = new System.Drawing.Size(100, 23);
            this.uiLabel4.TabIndex = 6;
            this.uiLabel4.Text = "比赛方式：";
            this.uiLabel4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txt_BestScoreMode
            // 
            this.txt_BestScoreMode.DataSource = null;
            this.txt_BestScoreMode.FillColor = System.Drawing.Color.White;
            this.txt_BestScoreMode.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txt_BestScoreMode.Items.AddRange(new object[] {
            "数值最大最优",
            "数值最小最优"});
            this.txt_BestScoreMode.Location = new System.Drawing.Point(287, 174);
            this.txt_BestScoreMode.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txt_BestScoreMode.MinimumSize = new System.Drawing.Size(63, 0);
            this.txt_BestScoreMode.Name = "txt_BestScoreMode";
            this.txt_BestScoreMode.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.txt_BestScoreMode.Size = new System.Drawing.Size(196, 29);
            this.txt_BestScoreMode.TabIndex = 5;
            this.txt_BestScoreMode.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txt_BestScoreMode.Watermark = "";
            this.txt_BestScoreMode.SelectedIndexChanged += new System.EventHandler(this.txt_BestScoreMode_SelectedIndexChanged);
            // 
            // uiLabel3
            // 
            this.uiLabel3.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel3.Location = new System.Drawing.Point(149, 181);
            this.uiLabel3.Name = "uiLabel3";
            this.uiLabel3.Size = new System.Drawing.Size(100, 23);
            this.uiLabel3.TabIndex = 4;
            this.uiLabel3.Text = "最优成绩：";
            this.uiLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txt_RoundCount
            // 
            this.txt_RoundCount.DataSource = null;
            this.txt_RoundCount.FillColor = System.Drawing.Color.White;
            this.txt_RoundCount.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txt_RoundCount.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10"});
            this.txt_RoundCount.Location = new System.Drawing.Point(287, 128);
            this.txt_RoundCount.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txt_RoundCount.MinimumSize = new System.Drawing.Size(63, 0);
            this.txt_RoundCount.Name = "txt_RoundCount";
            this.txt_RoundCount.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.txt_RoundCount.Size = new System.Drawing.Size(196, 29);
            this.txt_RoundCount.TabIndex = 3;
            this.txt_RoundCount.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txt_RoundCount.Watermark = "";
            this.txt_RoundCount.SelectedIndexChanged += new System.EventHandler(this.txt_RoundCount_SelectedIndexChanged);
            // 
            // uiLabel2
            // 
            this.uiLabel2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel2.Location = new System.Drawing.Point(158, 128);
            this.uiLabel2.Name = "uiLabel2";
            this.uiLabel2.Size = new System.Drawing.Size(100, 23);
            this.uiLabel2.TabIndex = 2;
            this.uiLabel2.Text = "比赛轮次：";
            this.uiLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txt_projectName
            // 
            this.txt_projectName.ButtonSymbolOffset = new System.Drawing.Point(0, 0);
            this.txt_projectName.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txt_projectName.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txt_projectName.Location = new System.Drawing.Point(287, 62);
            this.txt_projectName.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txt_projectName.MinimumSize = new System.Drawing.Size(1, 16);
            this.txt_projectName.Name = "txt_projectName";
            this.txt_projectName.Padding = new System.Windows.Forms.Padding(5);
            this.txt_projectName.ShowText = false;
            this.txt_projectName.Size = new System.Drawing.Size(196, 29);
            this.txt_projectName.TabIndex = 1;
            this.txt_projectName.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txt_projectName.Watermark = "";
            // 
            // uiLabel1
            // 
            this.uiLabel1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel1.Location = new System.Drawing.Point(158, 62);
            this.uiLabel1.Name = "uiLabel1";
            this.uiLabel1.Size = new System.Drawing.Size(100, 23);
            this.uiLabel1.TabIndex = 0;
            this.uiLabel1.Text = "项目名：";
            this.uiLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // SystemSettingWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.uiTitlePanel1);
            this.MaximizeBox = false;
            this.Name = "SystemSettingWindow";
            this.Text = "SystemSettingWindow";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SystemSettingWindow_FormClosed);
            this.Load += new System.EventHandler(this.SystemSettingWindow_Load);
            this.uiTitlePanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Sunny.UI.UITitlePanel uiTitlePanel1;
        private Sunny.UI.UIComboBox txt_RoundCount;
        private Sunny.UI.UILabel uiLabel2;
        private Sunny.UI.UITextBox txt_projectName;
        private Sunny.UI.UILabel uiLabel1;
        private Sunny.UI.UIComboBox txt_FloatType;
        private Sunny.UI.UILabel uiLabel5;
        private Sunny.UI.UIComboBox txt_TestMethod;
        private Sunny.UI.UILabel uiLabel4;
        private Sunny.UI.UIComboBox txt_BestScoreMode;
        private Sunny.UI.UILabel uiLabel3;
        private Sunny.UI.UIButton 确定玫瑰花;
    }
}