using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Rhombus.Wpf.Airspace.Interop;

namespace Rhombus.Wpf.Airspace.Mdi {
    internal class MdiWindowCollection : ObservableCollection<MdiWindow> {
        // Bring to the front of the windows in the specified state.
        public void BringToFront(MdiWindow window, System.Windows.WindowState windowState) {
            var oldIndex = this.IndexOf(window);
            var newIndex = this.Count - 1;

            if (windowState == System.Windows.WindowState.Minimized) {
                var foundSelf = false;

                for (newIndex = 0; newIndex < this.Count - 1; newIndex++) {
                    if (this.Items[newIndex] == window)
                        foundSelf = true;
                    if (MdiPanel.GetWindowState(this.Items[newIndex]) != System.Windows.WindowState.Minimized)
                        break;
                }

                if (foundSelf)
                    newIndex--;
            }

            this.Move(oldIndex, newIndex);

            // HACK: how to coordinate Win32 ZOrder with WPF ZOrder?  This works, but assumes to many implementation details.
            if (System.Windows.Media.VisualTreeHelper.GetChildrenCount(window) > 0) {
                var hwndClipper = System.Windows.Media.VisualTreeHelper.GetChild(window, 0) as AirspaceDecorator;
                if (hwndClipper != null)
                    if (System.Windows.Media.VisualTreeHelper.GetChildrenCount(hwndClipper) > 0) {
                        var hwndSourceHost = System.Windows.Media.VisualTreeHelper.GetChild(hwndClipper, 0) as HwndSourceHost;
                        if (hwndSourceHost != null) {
                            var hwnd = new Win32.User32.HWND(hwndSourceHost.Handle);
                            Win32.NativeMethods.SetWindowPos(hwnd, Win32.User32.HWND.TOP, 0, 0, 0, 0, Win32.User32.SWP.NOMOVE | Win32.User32.SWP.NOSIZE);
                        }
                    }
            }
        }

        public void CoerceValues(System.Windows.DependencyProperty dp) {
            foreach (var window in this.Items) {
                window.CoerceValue(dp);
            }
        }
    }
}