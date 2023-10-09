using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Volleyball.Core.GameSystem.GameHelper
{
    public class ListViewUtils
    {
        /// <summary>
        /// 自动调整ListView的列宽的方法
        /// </summary>
        /// <param name="lv"></param>
        public static void AutoResizeColumnWidth(ListView lv)
        {
            int count = lv.Columns.Count;
            int MaxWidth = 0;
            Graphics graphics = lv.CreateGraphics();
            int width;
            lv.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            for (int i = 0; i < count; i++)
            {
                string str = lv.Columns[i].Text;
                MaxWidth = lv.Columns[i].Width;

                foreach (ListViewItem item in lv.Items)
                {
                    str = item.SubItems[i].Text;
                    width = (int)graphics.MeasureString(str, lv.Font).Width;
                    if (width > MaxWidth)
                    {
                        MaxWidth = width;
                    }
                }
                if (MaxWidth <= 150)
                {
                    lv.Columns[i].Width = MaxWidth;
                }
                else
                {
                    lv.Columns[i].Width = 100;
                }
            }
        }
    }
}