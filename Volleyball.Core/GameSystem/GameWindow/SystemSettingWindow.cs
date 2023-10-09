using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Volleyball.Core.GameSystem.GameHelper;
using Volleyball.Core.GameSystem.GameModel;

namespace Volleyball.Core.GameSystem.GameWindow
{
    public partial class SystemSettingWindow : Form
    {
        public SystemSettingWindow()
        {
            InitializeComponent();
        }
        public IFreeSql fsql = DbFreesql.Sqlite;
        SportProjectInfos sportProjectInfoTemp = null;
        private void SystemSettingWindow_Load(object sender, EventArgs e)
        {
            sportProjectInfoTemp = fsql.Select<SportProjectInfos>().ToOne();
            if (sportProjectInfoTemp != null)
            {
                txt_projectName.Text = sportProjectInfoTemp.Name;
                txt_RoundCount.SelectedIndex = sportProjectInfoTemp.RoundCount;
                txt_BestScoreMode.SelectedIndex = sportProjectInfoTemp.BestScoreMode;
                txt_TestMethod.SelectedIndex = sportProjectInfoTemp.TestMethod;
                txt_FloatType.SelectedIndex = sportProjectInfoTemp.FloatType;
            }
        }

        private void txt_RoundCount_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = txt_RoundCount.SelectedIndex;
            if (index < 0) return;
            fsql.Update<SportProjectInfos>().Set(a => a.RoundCount == index).Where("1=1").ExecuteAffrows();
        }

        private void txt_BestScoreMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = txt_BestScoreMode.SelectedIndex;
            if (index < 0) return;
            fsql.Update<SportProjectInfos>().Set(a => a.BestScoreMode == index).Where("1=1").ExecuteAffrows();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txt_TestMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = txt_TestMethod.SelectedIndex;
            if (index < 0) return;
            fsql.Update<SportProjectInfos>().Set(a => a.TestMethod == index).Where("1=1").ExecuteAffrows();
        }

        private void txt_FloatType_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = txt_FloatType.SelectedIndex;
            if (index < 0) return;
            fsql.Update<SportProjectInfos>().Set(a => a.FloatType == index).Where("1=1").ExecuteAffrows();
        }

        private void SystemSettingWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            string name = txt_projectName.Text;
            fsql.Update<SportProjectInfos>().Set(a => a.Name == name).Where("1=1").ExecuteAffrows();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 确定玫瑰花_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
