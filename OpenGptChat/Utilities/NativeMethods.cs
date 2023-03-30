using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OpenGptChat.Utilities
{
    internal class NativeMethods
    {
        [DllImport("User32.dll", SetLastError = true)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);


        [DllImport("User32.dll", SetLastError = true)]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);


        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetWindow(IntPtr hWnd, uint uCmd);


        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool IsWindowVisible(IntPtr hWnd);


        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);


        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

        public delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        /// <summary>
        /// ShowWindow(hWnd, SW_NORMAL). SW_NORMAL: 1
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        public static bool ShowWindowNormal(IntPtr hWnd) => ShowWindow(hWnd, 1);



        /// <summary>
        /// 替代掉 Process 自带的主窗口查找函数, 因为他不会将隐藏的窗口作为主窗口 /
        /// Replace the built-in function for finding the main window in <see cref="Process"/>, because it does not include hidden windows as main windows.
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public static IntPtr GetProcessMainWindowHandle(int pid)
        {
            IntPtr bestHandle = IntPtr.Zero;
            EnumWindows(EnumWindowCallback, IntPtr.Zero);

            return bestHandle;


            bool EnumWindowCallback(IntPtr hWnd, IntPtr lParam)
            {
                GetWindowThreadProcessId(hWnd, out uint processId);
                if (pid == processId && IsMainWindow(hWnd))
                {
                    bestHandle = hWnd;
                    return false;
                }

                return true;
            }

            bool IsMainWindow(IntPtr hWnd)
            {
                // 如果它有 Owner, 则返回 false / Return false if it has an Owner.
                if (GetWindow(hWnd, 4) != IntPtr.Zero)
                    return false;

                return true;
            }
        }
    }
}
