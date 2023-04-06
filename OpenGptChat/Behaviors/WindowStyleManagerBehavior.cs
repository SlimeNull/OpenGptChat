using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Xaml.Behaviors;
using OpenGptChat.Services;

namespace OpenGptChat.Behaviors
{
    public class WindowStyleManagerBehavior : Behavior<Window>
    {
        public ColorModeService ColorModeService { get; } = 
            App.GetService<ColorModeService>();

        protected override void OnAttached()
        {
            base.OnAttached();

            UpdateWindowStyle();
        }

        private void UpdateWindowStyle()
        {
            ColorModeService.ApplyThemeForWindow(AssociatedObject);
        }
    }
}
