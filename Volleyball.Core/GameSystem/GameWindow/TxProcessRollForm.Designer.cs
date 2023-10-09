namespace Volleyball.Core.GameSystem.GameWindow
{
    partial class TxProcessRollForm
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
            this.uchScrollbar1 = new HZH_Controls.Controls.UCHScrollbar();
            this.SuspendLayout();
            // 
            // uchScrollbar1
            // 
            this.uchScrollbar1.BtnWidth = 18;
            this.uchScrollbar1.ConerRadius = 2;
            this.uchScrollbar1.FillColor = System.Drawing.Color.White;
            this.uchScrollbar1.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.uchScrollbar1.IsRadius = true;
            this.uchScrollbar1.IsShowRect = true;
            this.uchScrollbar1.LargeChange = 10;
            this.uchScrollbar1.Location = new System.Drawing.Point(3, 19);
            this.uchScrollbar1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uchScrollbar1.Maximum = 500;
            this.uchScrollbar1.Minimum = 0;
            this.uchScrollbar1.MinimumSize = new System.Drawing.Size(0, 10);
            this.uchScrollbar1.Name = "uchScrollbar1";
            this.uchScrollbar1.RectColor = System.Drawing.Color.White;
            this.uchScrollbar1.RectWidth = 1;
            this.uchScrollbar1.Size = new System.Drawing.Size(784, 89);
            this.uchScrollbar1.SmallChange = 50;
            this.uchScrollbar1.TabIndex = 3;
            this.uchScrollbar1.ThumbColor = System.Drawing.Color.ForestGreen;
            this.uchScrollbar1.Value = 0;
            // 
            // TxProcessRollForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 116);
            this.Controls.Add(this.uchScrollbar1);
            this.MaximizeBox = false;
            this.Name = "TxProcessRollForm";
            this.Text = "TxProcessRollForm";
            this.ResumeLayout(false);

        }

        #endregion

        private HZH_Controls.Controls.UCHScrollbar uchScrollbar1;
    }
}