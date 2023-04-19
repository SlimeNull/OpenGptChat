using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using OpenGptChat.Utilities;

namespace OpenGptChat
{
    internal static class EntryPoint
    {
        public static IntPtr MainWindowHandle { get; set; }

        static EntryPoint()
        {
            AppDomain.CurrentDomain.UnhandledException += 
                CurrentDomain_UnhandledException;
        }

        [STAThread]
        static void Main()
        {
            App app = new App();

            app.InitializeComponent();
            app.Run();
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            NativeMethods.MessageBox(MainWindowHandle, $"{e.ExceptionObject}", "UnhandledException", MessageBoxFlags.Ok | MessageBoxFlags.IconError);
        }
    }
}
