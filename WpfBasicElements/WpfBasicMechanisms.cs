using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using System.Threading;

namespace WpfBasicElements
{
    public static class WpfBasicMechanisms
    {
        public static ScrollViewer FindScrollViewer(FlowDocumentScrollViewer flowDocumentScrollViewer)
        {
            if (VisualTreeHelper.GetChildrenCount(flowDocumentScrollViewer) == 0)
            {
                return null;
            }

            // Border is the first child of first child of a ScrolldocumentViewer
            DependencyObject firstChild = VisualTreeHelper.GetChild(flowDocumentScrollViewer, 0);
            if (firstChild == null)
            {
                return null;
            }

            Decorator border = VisualTreeHelper.GetChild(firstChild, 0) as Decorator;

            if (border == null)
            {
                return null;
            }

            return border.Child as ScrollViewer;
        }

        public static void DispatcherWrapper(Action action, Dispatcher dispacher)
        {
            if (dispacher.Thread != Thread.CurrentThread)
            {
                dispacher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, (ThreadStart)delegate
                {
                    action();
                });
            }
            else
            {
                action();
            }
        }
    }
}
