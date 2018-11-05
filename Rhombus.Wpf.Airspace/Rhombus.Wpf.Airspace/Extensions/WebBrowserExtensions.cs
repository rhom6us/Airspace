using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Extensions {
    public static class WebBrowserExtensions {
        /// <summary>
        ///     Attached property to suppress script errors.
        /// </summary>
        public static readonly System.Windows.DependencyProperty SuppressScriptErrorsProperty = System.Windows.DependencyProperty.RegisterAttached("SuppressScriptErrors", typeof(bool), typeof(WebBrowserExtensions), new System.Windows.FrameworkPropertyMetadata(false, System.Windows.FrameworkPropertyMetadataOptions.Inherits, WebBrowserExtensions.OnSuppressScriptErrorsChanged));

        /// <summary>
        ///     Attached property that can be specified to subclass the
        ///     WebBrowser hosted IE window and suppress the WM_ERASEBKGND
        ///     message.
        /// </summary>
        public static readonly System.Windows.DependencyProperty SuppressEraseBackgroundProperty = System.Windows.DependencyProperty.RegisterAttached("SuppressEraseBackground", typeof(bool), typeof(WebBrowserExtensions), new System.Windows.FrameworkPropertyMetadata(false, System.Windows.FrameworkPropertyMetadataOptions.Inherits, WebBrowserExtensions.OnSuppressEraseBackgroundChanged));

        /// <summary>
        ///     Attached property that is used to store the HwndHook used to
        ///     intercept the messages to the IE window.
        /// </summary>
        private static readonly System.Windows.DependencyProperty SuppressEraseBackgroundWindowHookProperty = System.Windows.DependencyProperty.RegisterAttached("SuppressEraseBackgroundWindowHook", typeof(IEWindowHook), typeof(WebBrowserExtensions), new System.Windows.FrameworkPropertyMetadata(null));

        /// <summary>
        ///     Attached property getter for the SuppressScriptErrors property.
        /// </summary>
        public static bool GetSuppressScriptErrors(System.Windows.Controls.WebBrowser webBrowser) {
            return (bool) webBrowser.GetValue(SuppressScriptErrorsProperty);
        }

        /// <summary>
        ///     Attached property setter for the SuppressScriptErrors property.
        /// </summary>
        public static void SetSuppressScriptErrors(System.Windows.Controls.WebBrowser webBrowser, bool value) {
            webBrowser.SetValue(SuppressScriptErrorsProperty, value);
        }

        private static void OnSuppressScriptErrorsChanged(System.Windows.DependencyObject d, System.Windows.DependencyPropertyChangedEventArgs e) {
            var webBrowser = d as System.Windows.Controls.WebBrowser;
            if (webBrowser != null) {
                var value = (bool) e.NewValue;

                if (!WebBrowserExtensions.TrySetSuppressScriptErrors(webBrowser, value))
                    webBrowser.Navigated += (s, e2) => { WebBrowserExtensions.TrySetSuppressScriptErrors(webBrowser, value); };
            }
        }

        private static bool TrySetSuppressScriptErrors(System.Windows.Controls.WebBrowser webBrowser, bool value) {
            var field = typeof(System.Windows.Controls.WebBrowser).GetField("_axIWebBrowser2", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            if (field != null) {
                var axIWebBrowser2 = field.GetValue(webBrowser);
                if (axIWebBrowser2 != null) {
                    axIWebBrowser2.GetType().InvokeMember("Silent", System.Reflection.BindingFlags.SetProperty, null, axIWebBrowser2, new object[] {value});
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        ///     Attached property getter for the SuppressEraseBackground property.
        /// </summary>
        /// <param name="element">
        ///     The element the property should be read from.
        /// </param>
        /// <returns>
        ///     The value of the SuppressEraseBackground property on the
        ///     specified element.
        /// </returns>
        public static bool GetSuppressEraseBackground(System.Windows.DependencyObject element) {
            return (bool) element.GetValue(SuppressEraseBackgroundProperty);
        }

        /// <summary>
        ///     Attached property setter for the SuppressEraseBackground property.
        /// </summary>
        /// <param name="webBrowser">
        ///     The element to property should be read from.
        /// </param>
        /// <param name="value">
        ///     The value of the SuppressEraseBackground property on the
        ///     specified element.
        /// </param>
        public static void SetSuppressEraseBackground(System.Windows.DependencyObject element, bool value) {
            element.SetValue(SuppressEraseBackgroundProperty, value);
        }

        private static void OnSuppressEraseBackgroundChanged(System.Windows.DependencyObject d, System.Windows.DependencyPropertyChangedEventArgs e) {
            if (d is System.Windows.Controls.WebBrowser) {
                var webBrowser = (System.Windows.Controls.WebBrowser) d;

                var newValue = (bool) e.NewValue;
                if (newValue) {
                    if (!WebBrowserExtensions.TryHookWebBrowser(webBrowser))
                        webBrowser.LoadCompleted += WebBrowserExtensions.WebBrowserLoadCompleted;
                }
                else {
                    // When no longer suppressing the WM_ERASEBKGND message,
                    // dispose our window hook.
                    var hook = (IEWindowHook) webBrowser.GetValue(SuppressEraseBackgroundWindowHookProperty);
                    if (hook != null)
                        hook.Dispose();
                    webBrowser.ClearValue(SuppressEraseBackgroundWindowHookProperty);
                }
            }
        }

        private static bool TryHookWebBrowser(System.Windows.Controls.WebBrowser webBrowser) {
            if (WebBrowserExtensions.GetSuppressEraseBackground(webBrowser)) {
                // Try to find the IE window several layers within the WebBrowser.
                var hwndIEWindow = WebBrowserExtensions.GetIEWindow(webBrowser);
                if (hwndIEWindow != IntPtr.Zero) {
                    // Hook the window messages so we can intercept the
                    // WM_ERASEBKGND message.
                    var hook = new IEWindowHook(new Win32.User32.HWND(hwndIEWindow));

                    // Keep our hook alive.
                    webBrowser.SetValue(SuppressEraseBackgroundWindowHookProperty, hook);

                    return true;
                }
            }

            return false;
        }

        private static void WebBrowserLoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e) {
            // We only need to do this the first time. 
            var webBrowser = (System.Windows.Controls.WebBrowser) sender;
            webBrowser.LoadCompleted -= WebBrowserExtensions.WebBrowserLoadCompleted;

            if (WebBrowserExtensions.GetSuppressEraseBackground(webBrowser) && !WebBrowserExtensions.TryHookWebBrowser(webBrowser))
                throw new InvalidOperationException("Unable to hook the WebBrowser.");
        }

        private static IntPtr GetIEWindow(System.Windows.Controls.WebBrowser webBrowser) {
            var hwndIeWindow = Win32.User32.HWND.NULL;

            var hwndShellEmbeddingWindow = new Win32.User32.HWND(webBrowser.Handle);
            if (hwndShellEmbeddingWindow != Win32.User32.HWND.NULL) {
                var hwndShellDocObjectView = Win32.NativeMethods.GetWindow(hwndShellEmbeddingWindow, Win32.User32.GW.CHILD);
                if (hwndShellDocObjectView != Win32.User32.HWND.NULL)
                    hwndIeWindow = Win32.NativeMethods.GetWindow(hwndShellDocObjectView, Win32.User32.GW.CHILD);
            }

            return hwndIeWindow.DangerousGetHandle();
        }

        private class IEWindowHook : Win32.ComCtl32.WindowSubclass {
            public IEWindowHook(Win32.User32.HWND hwnd) : base(hwnd) { }

            protected override IntPtr WndProcOverride(Win32.User32.HWND hwnd, Win32.User32.WM msg, IntPtr wParam, IntPtr lParam, IntPtr id, IntPtr data) {
                // IE doesn't seem to really need to erase its background, since
                // it will paint the entire window with the web page in WM_PAINT.
                // However, it causes flickering on some systems, so we just
                // ignore the message.
                if (msg == Win32.User32.WM.ERASEBKGND)
                    return new IntPtr(1);

                return base.WndProcOverride(hwnd, msg, wParam, lParam, id, data);
            }
        }
    }
}