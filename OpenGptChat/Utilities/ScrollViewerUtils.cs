using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace OpenGptChat.Utilities
{
    static class ScrollViewerUtils
    {
        public static bool IsAtEnd(this ScrollViewer scrollViewer, int threshold = 5)
        {
            if (scrollViewer.VerticalOffset == scrollViewer.ScrollableHeight)
                return true;

            if (scrollViewer.VerticalOffset + threshold >= scrollViewer.ScrollableHeight)
                return true;

            return false;
        }
    }
}
