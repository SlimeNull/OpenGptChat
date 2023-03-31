using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGptChat
{
    public static class GlobalServices
    {
        private static IServiceProvider? serviceProvider;

        public static void InitServices(IServiceProvider serviceProvider)
        {
            GlobalServices.serviceProvider = serviceProvider;
        }

        public static T GetService<T>()
        {
            if (serviceProvider == null)
                throw new InvalidOperationException("Not initialized");

            return serviceProvider.GetService<T>() ?? throw new ArgumentException("Cannot service with specified type");
        }
    }
}
