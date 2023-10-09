namespace Volleyball.Core.GameSystem.GameWindow
{
    partial class MainWindow
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
            this.components = new System.ComponentModel.Container();
            this.miniToolStrip = new System.Windows.Forms.MenuStrip();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.menuStrip2 = new System.Windows.Forms.MenuStrip();
            this.系统设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.导入名单ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.平台设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.数据管理ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.初始化数据库ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.数据库备份ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uiTitlePanel1 = new Sunny.UI.UITitlePanel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.uiTitlePanel3 = new Sunny.UI.UITitlePanel();
            this.uiProcessBar1 = new Sunny.UI.UIProcessBar();
            this.uiButton3 = new Sunny.UI.UIButton();
            this.uiButton2 = new Sunny.UI.UIButton();
            this.uiButton1 = new Sunny.UI.UIButton();
            this.listView1 = new System.Windows.Forms.ListView();
            this.uiTitlePanel2 = new Sunny.UI.UITitlePanel();
            this.uiTreeView1 = new Sunny.UI.UITreeView();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.系统参数设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip2.SuspendLayout();
            this.uiTitlePanel1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.uiTitlePanel3.SuspendLayout();
            this.uiTitlePanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // miniToolStrip
            // 
            this.miniToolStrip.AccessibleName = "新项选择";
            this.miniToolStrip.AccessibleRole = System.Windows.Forms.AccessibleRole.ComboBox;
            this.miniToolStrip.AutoSize = false;
            this.miniToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.miniToolStrip.Location = new System.Drawing.Point(6, 2);
            this.miniToolStrip.Name = "miniToolStrip";
            this.miniToolStrip.Size = new System.Drawing.Size(1455, 24);
            this.miniToolStrip.TabIndex = 0;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1455, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // menuStrip2
            // 
            this.menuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.系统设置ToolStripMenuItem,
            this.数据管理ToolStripMenuItem});
            this.menuStrip2.Location = new System.Drawing.Point(0, 0);
            this.menuStrip2.Name = "menuStrip2";
            this.menuStrip2.Size = new System.Drawing.Size(1455, 25);
            this.menuStrip2.TabIndex = 0;
            this.menuStrip2.Text = "menuStrip2";
            // 
            // 系统设置ToolStripMenuItem
            // 
            this.系统设置ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.导入名单ToolStripMenuItem,
            this.平台设置ToolStripMenuItem,
            this.系统参数设置ToolStripMenuItem});
            this.系统设置ToolStripMenuItem.Name = "系统设置ToolStripMenuItem";
            this.系统设置ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.系统设置ToolStripMenuItem.Text = "系统设置";
            // 
            // 导入名单ToolStripMenuItem
            // 
            this.导入名单ToolStripMenuItem.Name = "导入名单ToolStripMenuItem";
            this.导入名单ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.导入名单ToolStripMenuItem.Text = "导入名单";
            this.导入名单ToolStripMenuItem.Click += new System.EventHandler(this.导入名单ToolStripMenuItem_Click);
            // 
            // 平台设置ToolStripMenuItem
            // 
            this.平台设置ToolStripMenuItem.Name = "平台设置ToolStripMenuItem";
            this.平台设置ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.平台设置ToolStripMenuItem.Text = "平台设置";
            this.平台设置ToolStripMenuItem.Click += new System.EventHandler(this.平台设置ToolStripMenuItem_Click);
            // 
            // 数据管理ToolStripMenuItem
            // 
            this.数据管理ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.初始化数据库ToolStripMenuItem,
            this.数据库备份ToolStripMenuItem});
            this.数据管理ToolStripMenuItem.Name = "数据管理ToolStripMenuItem";
            this.数据管理ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.数据管理ToolStripMenuItem.Text = "数据管理";
            // 
            // 初始化数据库ToolStripMenuItem
            // 
            this.初始化数据库ToolStripMenuItem.Name = "初始化数据库ToolStripMenuItem";
            this.初始化数据库ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.初始化数据库ToolStripMenuItem.Text = "初始化数据库";
            // 
            // 数据库备份ToolStripMenuItem
            // 
            this.数据库备份ToolStripMenuItem.Name = "数据库备份ToolStripMenuItem";
            this.数据库备份ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.数据库备份ToolStripMenuItem.Text = "数据库备份";
            // 
            // uiTitlePanel1
            // 
            this.uiTitlePanel1.Controls.Add(this.statusStrip1);
            this.uiTitlePanel1.Controls.Add(this.uiTitlePanel3);
            this.uiTitlePanel1.Controls.Add(this.uiTitlePanel2);
            this.uiTitlePanel1.Controls.Add(this.menuStrip2);
            this.uiTitlePanel1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiTitlePanel1.Location = new System.Drawing.Point(3, 6);
            this.uiTitlePanel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiTitlePanel1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiTitlePanel1.Name = "uiTitlePanel1";
            this.uiTitlePanel1.RectColor = System.Drawing.Color.White;
            this.uiTitlePanel1.ShowText = false;
            this.uiTitlePanel1.Size = new System.Drawing.Size(1455, 841);
            this.uiTitlePanel1.Style = Sunny.UI.UIStyle.Custom;
            this.uiTitlePanel1.TabIndex = 0;
            this.uiTitlePanel1.Text = "德育龙测试系统";
            this.uiTitlePanel1.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.uiTitlePanel1.TitleColor = System.Drawing.Color.White;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 819);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1455, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(131, 17);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // uiTitlePanel3
            // 
            this.uiTitlePanel3.Controls.Add(this.uiProcessBar1);
            this.uiTitlePanel3.Controls.Add(this.uiButton3);
            this.uiTitlePanel3.Controls.Add(this.uiButton2);
            this.uiTitlePanel3.Controls.Add(this.uiButton1);
            this.uiTitlePanel3.Controls.Add(this.listView1);
            this.uiTitlePanel3.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiTitlePanel3.Location = new System.Drawing.Point(288, 36);
            this.uiTitlePanel3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiTitlePanel3.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiTitlePanel3.Name = "uiTitlePanel3";
            this.uiTitlePanel3.ShowText = false;
            this.uiTitlePanel3.Size = new System.Drawing.Size(1163, 784);
            this.uiTitlePanel3.Style = Sunny.UI.UIStyle.Custom;
            this.uiTitlePanel3.TabIndex = 2;
            this.uiTitlePanel3.Text = "学生信息";
            this.uiTitlePanel3.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiProcessBar1
            // 
            this.uiProcessBar1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiProcessBar1.Location = new System.Drawing.Point(4, 752);
            this.uiProcessBar1.MinimumSize = new System.Drawing.Size(70, 3);
            this.uiProcessBar1.Name = "uiProcessBar1";
            this.uiProcessBar1.Size = new System.Drawing.Size(1152, 29);
            this.uiProcessBar1.Style = Sunny.UI.UIStyle.Custom;
            this.uiProcessBar1.TabIndex = 4;
            this.uiProcessBar1.Text = "uiProcessBar1";
            // 
            // uiButton3
            // 
            this.uiButton3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.uiButton3.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiButton3.Location = new System.Drawing.Point(372, 52);
            this.uiButton3.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiButton3.Name = "uiButton3";
            this.uiButton3.Size = new System.Drawing.Size(100, 35);
            this.uiButton3.Style = Sunny.UI.UIStyle.Custom;
            this.uiButton3.TabIndex = 3;
            this.uiButton3.Text = "导出成绩";
            this.uiButton3.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiButton3.Click += new System.EventHandler(this.uiButton3_Click);
            // 
            // uiButton2
            // 
            this.uiButton2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.uiButton2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiButton2.Location = new System.Drawing.Point(247, 50);
            this.uiButton2.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiButton2.Name = "uiButton2";
            this.uiButton2.Size = new System.Drawing.Size(100, 35);
            this.uiButton2.Style = Sunny.UI.UIStyle.Custom;
            this.uiButton2.TabIndex = 2;
            this.uiButton2.Text = "上传成绩";
            this.uiButton2.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiButton2.Click += new System.EventHandler(this.uiButton2_Click);
            // 
            // uiButton1
            // 
            this.uiButton1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.uiButton1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiButton1.Location = new System.Drawing.Point(118, 52);
            this.uiButton1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiButton1.Name = "uiButton1";
            this.uiButton1.Size = new System.Drawing.Size(100, 35);
            this.uiButton1.Style = Sunny.UI.UIStyle.Custom;
            this.uiButton1.TabIndex = 1;
            this.uiButton1.Text = "启动测试";
            this.uiButton1.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiButton1.Click += new System.EventHandler(this.uiButton1_Click);
            // 
            // listView1
            // 
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(4, 91);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(1152, 655);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            // 
            // uiTitlePanel2
            // 
            this.uiTitlePanel2.Controls.Add(this.uiTreeView1);
            this.uiTitlePanel2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiTitlePanel2.Location = new System.Drawing.Point(0, 36);
            this.uiTitlePanel2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiTitlePanel2.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiTitlePanel2.Name = "uiTitlePanel2";
            this.uiTitlePanel2.ShowText = false;
            this.uiTitlePanel2.Size = new System.Drawing.Size(280, 784);
            this.uiTitlePanel2.Style = Sunny.UI.UIStyle.Custom;
            this.uiTitlePanel2.TabIndex = 1;
            this.uiTitlePanel2.Text = " ";
            this.uiTitlePanel2.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiTreeView1
            // 
            this.uiTreeView1.FillColor = System.Drawing.Color.White;
            this.uiTreeView1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiTreeView1.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiTreeView1.Location = new System.Drawing.Point(4, 40);
            this.uiTreeView1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiTreeView1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiTreeView1.Name = "uiTreeView1";
            this.uiTreeView1.ShowText = false;
            this.uiTreeView1.Size = new System.Drawing.Size(270, 729);
            this.uiTreeView1.Style = Sunny.UI.UIStyle.Custom;
            this.uiTreeView1.TabIndex = 0;
            this.uiTreeView1.Text = "uiTreeView1";
            this.uiTreeView1.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.uiTreeView1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.uiTreeView1_MouseDown);
            this.uiTreeView1.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.uiTreeView1_NodeMouseClick);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // 系统参数设置ToolStripMenuItem
            // 
            this.系统参数设置ToolStripMenuItem.Name = "系统参数设置ToolStripMenuItem";
            this.系统参数设置ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.系统参数设置ToolStripMenuItem.Text = "系统参数设置";
            this.系统参数设置ToolStripMenuItem.Click += new System.EventHandler(this.系统参数设置ToolStripMenuItem_Click);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1459, 849);
            this.Controls.Add(this.uiTitlePanel1);
            this.Name = "MainWindow";
            this.Text = "MainWindow";
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.Resize += new System.EventHandler(this.MainWindow_Resize);
            this.menuStrip2.ResumeLayout(false);
            this.menuStrip2.PerformLayout();
            this.uiTitlePanel1.ResumeLayout(false);
            this.uiTitlePanel1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.uiTitlePanel3.ResumeLayout(false);
            this.uiTitlePanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuStrip miniToolStrip;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.MenuStrip menuStrip2;
        private System.Windows.Forms.ToolStripMenuItem 系统设置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 导入名单ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 平台设置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 数据管理ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 初始化数据库ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 数据库备份ToolStripMenuItem;
        private Sunny.UI.UITitlePanel uiTitlePanel1;
        private Sunny.UI.UITitlePanel uiTitlePanel2;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private Sunny.UI.UITitlePanel uiTitlePanel3;
        private Sunny.UI.UIButton uiButton3;
        private Sunny.UI.UIButton uiButton2;
        private Sunny.UI.UIButton uiButton1;
        private System.Windows.Forms.ListView listView1;
        private Sunny.UI.UITreeView uiTreeView1;
        private System.Windows.Forms.Timer timer1;
        private Sunny.UI.UIProcessBar uiProcessBar1;
        private System.Windows.Forms.ToolStripMenuItem 系统参数设置ToolStripMenuItem;
    }
}