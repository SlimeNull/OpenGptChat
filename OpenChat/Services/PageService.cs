using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace OpenChat.Services
{
    public class PageService
    {
        private readonly MainWindow mainWindow;

        public PageService(MainWindow mainWindow)
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
