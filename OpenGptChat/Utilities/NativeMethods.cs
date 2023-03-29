using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OpenGptChat.Utilities
{
    internal class NativeMethods
    {
        [DllImport("User32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("User32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        /// <summary>
        /// ShowWindow(hWnd, SW_NORMAL). SW_NORMAL: 1
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        public static bool ShowWindowNormal(IntPtr hWnd) => ShowWindow(hWnd, 5);
    }
}
