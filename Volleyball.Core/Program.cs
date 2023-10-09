using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Volleyball.Core.GameSystem;
using Volleyball.Core.GameSystem.GameWindow;

namespace Volleyball.Core
{
    internal static class Program
    {
        public const int WM_SYSCOMMAND = 0x112;
        public const int SC_RESTORE = 0xF120;

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        private static void Main()
        {
            GameRoot GameRoot = new GameRoot();
            Process cur = Process.GetCurrentProcess();
            foreach (Process p in Process.GetProcesses())
            {
                if (p.Id == cur.Id) continue;
                if (p.ProcessName == cur.ProcessName)
                {
                    GameRoot.SetForegroundWindow(p.MainWindowHandle);
                    GameRoot.SendMessage(p.MainWindowHandle, WM_SYSCOMMAND, SC_RESTORE, 0);
                    //p.Close();
                    return;
                }
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainWindow());
        }
    }
}