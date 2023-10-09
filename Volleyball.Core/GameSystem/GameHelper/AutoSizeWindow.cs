using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Volleyball.Core.GameSystem.GameHelper
{
    public class AutoSizeWindow
    {
        private readonly float x;
        private readonly float y;

        public AutoSizeWindow(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="control"></param>
        public void SetTag(Control control)
        {
            foreach (Control con in control.Controls)
            {
                con.Tag = con.Width + ";" + con.Height + ";" + con.Left + ";" + con.Top + ";" + con.Font.Size;
                if (con.Controls.Count > 0) SetTag(con);
            }
        }

        /// <summary>
        ///
        /// </summary>
        public void ReWinformLayout(Control control)
        {
            var newx = control.Width / x;
            var newy = control.Height / y;
            SetControls(newx, newy, control);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="newx"></param>
        /// <param name="newy"></param>
        /// <param name="cons"></param>
        private void SetControls(float newx, float newy, Control cons)
        {
            foreach (Control con in cons.Controls)
            {
                //获取控件的Tag属性值，并分割后存储字符串数组
                if (con.Tag != null)
                {
                    var mytag = con.Tag.ToString().Split(';');
                    //根据窗体缩放的比例确定控件的值
                    con.Width = Convert.ToInt32(Convert.ToSingle(mytag[0]) * newx); //宽度
                    con.Height = Convert.ToInt32(Convert.ToSingle(mytag[1]) * newy); //高度
                    con.Left = Convert.ToInt32(Convert.ToSingle(mytag[2]) * newx); //左边距
                    con.Top = Convert.ToInt32(Convert.ToSingle(mytag[3]) * newy); //顶边距
                    var currentSize = Convert.ToSingle(mytag[4]) * newy; //字体大小
                    if (currentSize > 0) con.Font = new Font(con.Font.Name, currentSize, con.Font.Style, con.Font.Unit);
                    con.Focus();
                    if (con.Controls.Count > 0)
                    {
                        SetControls(newx, newy, con);
                    }
                }
            }
        }
    }
}