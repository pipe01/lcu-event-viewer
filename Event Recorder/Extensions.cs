using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace Event_Recorder
{
    internal static class Extensions
    {
        public static void ScrollToBottom(this ListBox list)
        {
            list.GetScrollViewer()?.ScrollToBottom();
        }

        public static bool ShouldScrollToBottom(this ListBox list)
        {
            var scrollViewer = list.GetScrollViewer();

            if (scrollViewer != null)
            {
                return scrollViewer.ScrollableHeight > 0
                    && scrollViewer.VerticalOffset + scrollViewer.ViewportHeight >= scrollViewer.ExtentHeight - 1;
            }

            return false;
        }

        public static ScrollViewer GetScrollViewer(this ListBox list)
        {
            if (VisualTreeHelper.GetChildrenCount(list) > 0)
            {
                var border = (Border)VisualTreeHelper.GetChild(list, 0);
                return (ScrollViewer)VisualTreeHelper.GetChild(border, 0);
            }

            return null;
        }
    }
}
