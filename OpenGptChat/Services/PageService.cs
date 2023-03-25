using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using OpenGptChat;

namespace OpenGptChat.Services
{
    public class PageService
    {
        private readonly AppWindow mainWindow;

        public PageService(AppWindow mainWindow)
        {
            this.mainWindow = mainWindow;
        }

        public void Navigate<T>()
            where T : FrameworkElement
        {
            mainWindow.Navigate(App.GetService<T>());
        }
    }
}
