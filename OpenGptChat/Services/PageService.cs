using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using OpenGptChat;

namespace OpenGptChat.Services
{
    /// <summary>
    /// 页面服务, 用于导航 / Page service, used for navigation.
    /// </summary>
    public class PageService
    {
        private readonly AppWindow mainWindow;

        public PageService(AppWindow mainWindow)
        {
            this.mainWindow = mainWindow;
        }

        /// <summary>
        /// 将主窗口的页面切换到指定页面 / Switch the main window's page to the specified page.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void Navigate<T>()
            where T : FrameworkElement
        {
            mainWindow.Navigate(App.GetService<T>());
        }
    }
}
