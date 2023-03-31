using OpenGptChat.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Wpf.Ui.Mvvm.Contracts;

namespace OpenGptChat.FluentDesign.Services
{
    internal class FluentPageService : PageService, IPageService
    {
        public FluentPageService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}
