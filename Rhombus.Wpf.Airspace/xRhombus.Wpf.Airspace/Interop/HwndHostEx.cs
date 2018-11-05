using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Interop {
    /// <summary>
    ///     This class works around issues with the HwndHost in current WPF versions.
    /// </summary>
    public class HwndHostEx : System.Windows.Interop.HwndHost, System.Windows.Interop.IKeyboardInputSink {
        /// <summary>
        ///     The behavior for handling of the SWP_NOCOPYBITS flag during
        ///     moving or sizing operations.
        /// </summary>
        /// <remarks>
        ///     Real implementation is HwndHostExtensions.CopyBitsBehaviorProperty
        /// </remarks>
        public static readonly System.Windows.DependencyProperty CopyBitsBehaviorProperty = Extensions.HwndHostExtensions.CopyBitsBehaviorProperty.AddOwner(typeof(HwndHostEx));

        /// <summary>
        ///     Specifies whether or not a HwndHostCommands.MouseActivate
        ///     command is raised in response to WM_MOUSEACTIVATE.
        /// </summary>
        /// <remarks>
        ///     Real implementation is HwndHostExtensions.RaiseMouseActivateCommandProperty
        /// </remarks>
        public static readonly System.Windows.DependencyProperty RaiseMouseActivateCommandProperty = Extensions.HwndHostExtensions.RaiseMouseActivateCommandProperty;

        // Set the window position asynchronously since for nested WPF
        // layouts, the layout manager will refuse to raise the
        // LayoutUpdated event which we depend on.
        //
        // This is very important since HwndSourceHost will cause this
        // nesting scenario very frequently.
        /// <summary>
        ///     Specifies whether or not to set the window position of the
        ///     hosted window asynchronously.
        /// </summary>
        /// <remarks>
        ///     HwndHost depends on the LayoutUpdated event to synchronize the
        ///     position of the HWND with the layout slot.  However, when WPF
        ///     gets into a recursive/nested layout loop, the LayoutUpdated
        ///     event is not raised.  Since we resize the hosted HWND in
        ///     response to this event, if the hosted HWND contains an
        ///     HwndSource, that HwndSource will run an immediate layout pass,
        ///     but the LayoutUpdated event will not be raised.
        ///     TODO: fix in 4.5?
        /// </remarks>
        public static readonly System.Windows.DependencyProperty AsyncUpdateWindowPosProperty = System.Windows.DependencyProperty.Register("AsyncUpdateWindowPos", typeof(bool), typeof(HwndHostEx), new System.Windows.FrameworkPropertyMetadata(false));

        /// <summary>
        ///     The behavior for handling of the SWP_NOCOPYBITS flag during
        ///     moving or sizing operations.
        /// </summary>
        public CopyBitsBehavior CopyBitsBehavior {
            get => (CopyBitsBehavior) this.GetValue(CopyBitsBehaviorProperty);
            set => this.SetValue(CopyBitsBehaviorProperty, value);
        }

        /// <summary>
        ///     Specifies whether or not a HwndHostCommands.MouseActivate
        ///     command is raised in response to WM_MOUSEACTIVATE.
        /// </summary>
        public bool RaiseMouseActivateCommand {
            get => (bool) this.GetValue(RaiseMouseActivateCommandProperty);
            set => this.SetValue(RaiseMouseActivateCommandProperty, value);
        }

        public bool AsyncUpdateWindowPos {
            get => (bool) this.GetValue(AsyncUpdateWindowPosProperty);
            set => this.SetValue(AsyncUpdateWindowPosProperty, value);
        }

        protected sealed override System.Runtime.InteropServices.HandleRef BuildWindowCore(System.Runtime.InteropServices.HandleRef hwndParent) {
            var hwndParent2 = new Win32.User32.HWND(hwndParent.Handle);
            _hwndChild = this.BuildWindowOverride(hwndParent2);

            // Ideally, the window would have been created with the expected
            // parent.  But just in case, we set it explicitly.
            Win32.NativeMethods.SetParent(_hwndChild, hwndParent2);

            return new System.Runtime.InteropServices.HandleRef(this, _hwndChild.DangerousGetHandle());
        }

        protected sealed override void DestroyWindowCore(System.Runtime.InteropServices.HandleRef hwnd) {
            var hwndChild = new Win32.User32.HWND(hwnd.Handle);
            System.Diagnostics.Debug.Assert(hwndChild == _hwndChild); // Equivalency, not reference equality.

            this.DestroyWindowOverride(_hwndChild);

            // Release our reference to the child HWND.  Note that we do not
            // explicitly dispose this handle, because we let
            // DestroyWindowOverride decide what to do with it.
            _hwndChild = null;
        }

        /// <summary>
        ///     Default implementation of BuildWindowOverride, which just
        ///     creates a simple "STATIC" HWND.  This is almost certainly not
        ///     the desired window, but at least something shows up on the
        ///     screen. Override this method in your derived class and build
        ///     the window you want.
        /// </summary>
        protected virtual Win32.User32.HWND BuildWindowOverride(Win32.User32.HWND hwndParent) {
            return Win32.NativeMethods.CreateWindowEx(0, "STATIC", "Default HwndHostEx Window", Win32.User32.WS.CHILD | Win32.User32.WS.CLIPSIBLINGS | Win32.User32.WS.CLIPCHILDREN, 0, 0, 0, 0, hwndParent, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
        }

        /// <summary>
        ///     Default implementation of DestroyWindowCore, which just
        ///     destroys the hosted window.  If this is undesirable,
        ///     override this method and provide alternative logic.
        /// </summary>
        protected virtual void DestroyWindowOverride(Win32.User32.HWND hwnd) {
            Win32.NativeMethods.DestroyWindow(hwnd);
        }

        protected sealed override IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled) {
            System.Diagnostics.Debug.Assert(hwnd == _hwndChild.DangerousGetHandle());

            var messageHandledOldValue = _messageHandled;
            try {
                _messageHandled = true;
                var result = this.WndProcOverride(_hwndChild, (Win32.User32.WM) msg, wParam, lParam);

                handled = _messageHandled;
                return result;
            }
            finally {
                _messageHandled = messageHandledOldValue;
            }
        }

        protected virtual IntPtr WndProcOverride(Win32.User32.HWND hwnd, Win32.User32.WM msg, IntPtr wParam, IntPtr lParam) {
            // The default implementation is to mark the message as not
            // handled.
            _messageHandled = false;
            return IntPtr.Zero;
        }

        /// <summary>
        /// </summary>
        protected override void OnWindowPositionChanged(System.Windows.Rect rcBoundingBox) {
            var callBase = false;
            var callAsync = false;

            if (_lastWindowPosChanged != null) {
                // Only call the base if something changed since last time.
                if (rcBoundingBox != _lastWindowPosChanged.Value) {
                    callBase = true;

                    if (rcBoundingBox.Width != _lastWindowPosChanged.Value.Width || rcBoundingBox.Height != _lastWindowPosChanged.Value.Height)
                        callAsync = true;
                }
            }
            else {
                // First time.
                callBase = true;
                callAsync = true;
            }

            if (callBase) {
                if (callAsync)
                    this.Dispatcher.BeginInvoke((Action) delegate { base.OnWindowPositionChanged(rcBoundingBox); });
                else
                    base.OnWindowPositionChanged(rcBoundingBox);
            }

            _lastWindowPosChanged = rcBoundingBox;
        }

        private Win32.User32.HWND _hwndChild;

        private System.Windows.Rect? _lastWindowPosChanged;
        private bool _messageHandled;
    }
}