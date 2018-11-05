using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Interop {
    public class WindowBase : IDisposable {
        internal WindowBase() {
            _wndProc = this.WndProc;
        }

        public Win32.User32.HWND Handle { get; private set; }

        protected virtual void Dispose(bool disposing) {
            if (disposing) {
                var strongHwnd = this.Handle as Win32.ComCtl32.StrongHWND;

                if (strongHwnd != null) {
                    // Replace the StrongHWND reference with a regular "weak"
                    // HWND reference so that messages processed during
                    // disposing do not have to deal with a partially disposed
                    // SafeHandle.
                    this.Handle = new Win32.User32.HWND(strongHwnd.DangerousGetHandle());

                    strongHwnd.Dispose();

                    // All done, replace the "weak" HWND reference with null.
                    this.Handle = null;
                }
            }
        }

        protected virtual void Initialize() { }

        protected virtual IntPtr OnMessage(Win32.User32.WM message, IntPtr wParam, IntPtr lParam) {
            return Win32.NativeMethods.DefWindowProc(this.Handle, message, wParam, lParam);
        }

        public void SetLayeredWindowAttributes(byte? alpha, System.Windows.Media.Color? colorKey = null) {
            Win32.User32.LWA flags = 0;
            byte bAlpha = 0;
            uint crKey = 0;

            if (alpha != null) {
                bAlpha = alpha.Value;
                flags |= Win32.User32.LWA.ALPHA;
            }

            if (colorKey != null) {
                var r = (uint) colorKey.Value.R;
                var g = (uint) colorKey.Value.G;
                var b = (uint) colorKey.Value.B;

                crKey = r + (g << 8) + (b << 16);
                flags |= Win32.User32.LWA.COLORKEY;
            }

            Win32.NativeMethods.SetLayeredWindowAttributes(this.Handle, crKey, bAlpha, flags);
        }

        public bool Hide() {
            return Win32.NativeMethods.ShowWindow(this.Handle, Win32.User32.SW.HIDE);
        }

        public bool Restore() {
            return Win32.NativeMethods.ShowWindow(this.Handle, Win32.User32.SW.RESTORE);
        }

        public bool Minimize(bool force) {
            return Win32.NativeMethods.ShowWindow(
                this.Handle, force
                    ? Win32.User32.SW.FORCEMINIMIZE
                    : Win32.User32.SW.MINIMIZE);
        }

        public bool Maximize() {
            return Win32.NativeMethods.ShowWindow(this.Handle, Win32.User32.SW.MAXIMIZE);
        }

        public bool Show(WindowShowState showState = WindowShowState.Current, bool activate = true) {
            switch (showState) {
                case WindowShowState.Default:
                    return Win32.NativeMethods.ShowWindow(this.Handle, Win32.User32.SW.SHOWDEFAULT);

                case WindowShowState.Current:
                    return Win32.NativeMethods.ShowWindow(
                        this.Handle, activate
                            ? Win32.User32.SW.SHOW
                            : Win32.User32.SW.SHOWNA);

                case WindowShowState.Normal:
                    return Win32.NativeMethods.ShowWindow(
                        this.Handle, activate
                            ? Win32.User32.SW.SHOWNORMAL
                            : Win32.User32.SW.SHOWNOACTIVATE);

                case WindowShowState.Minimized:
                    return Win32.NativeMethods.ShowWindow(
                        this.Handle, activate
                            ? Win32.User32.SW.SHOWMINIMIZED
                            : Win32.User32.SW.SHOWMINNOACTIVE);

                case WindowShowState.Maximized:
                    return Win32.NativeMethods.ShowWindow(this.Handle, Win32.User32.SW.SHOWMAXIMIZED);

                default:
                    return false;
            }
        }

        /// <summary>
        ///     Called from WindowClass.CreateWindow to intialize this instance
        ///     when the HWND has been created.
        /// </summary>
        /// <param name="hwnd">
        ///     The HWND that was created.
        /// </param>
        /// <param name="param">
        ///     The creation parameter that was passsed to
        ///     WindowClass.CreateWindow.
        /// </param>
        internal IntPtr InitializeFromFirstMessage(IntPtr hwnd, int message, IntPtr wParam, IntPtr lParam) {
            this.Handle = new Win32.User32.HWND(hwnd);

            // Replace the window proceedure for this window instance.
            var wndProc = System.Runtime.InteropServices.Marshal.GetFunctionPointerForDelegate(_wndProc);
            Win32.NativeMethods.SetWindowLongPtr(this.Handle, Win32.User32.GWL.WNDPROC, wndProc);

            // Give the window a chance to initialize.
            this.Initialize();

            // Manually invoke the window proceedure for this message.
            return this.OnMessage((Win32.User32.WM) message, wParam, lParam);
        }

        internal void TransferHandleOwnership(Win32.ComCtl32.StrongHWND hwnd) {
            System.Diagnostics.Debug.Assert(hwnd == this.Handle); // equivalency, not reference equals
            this.Handle = hwnd;
        }

        internal IntPtr WndProc(IntPtr hwnd, int message, IntPtr wParam, IntPtr lParam) {
            System.Diagnostics.Debug.Assert(hwnd == this.Handle.DangerousGetHandle());

            return this.OnMessage((Win32.User32.WM) message, wParam, lParam);
        }

        public void Dispose() {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private readonly Win32.User32.WNDPROC _wndProc;
    }
}