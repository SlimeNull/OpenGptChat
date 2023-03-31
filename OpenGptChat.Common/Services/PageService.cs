using System.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace OpenGptChat.Services
{
    /// <summary>
    /// 页面服务, 用于导航 / Page service, used for navigation.
    /// </summary>
    public class PageService
    {
        public PageService(
            IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public IServiceProvider ServiceProvider { get; }

        /// <summary>
        /// 将主窗口的页面切换到指定页面 / Switch the main window's page to the specified page.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public T? GetPage<T>()
            where T : class
        {
            return ServiceProvider.GetService<T>() ?? throw new InvalidOperationException("Cannot find specified Page service");
        }

        public FrameworkElement? GetPage(Type type)
        {
            return ServiceProvider.GetService(type) as FrameworkElement;
        }
    }
}
