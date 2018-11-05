using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Win32.ComCtl32 {
    /// <summary>
    ///     WindowSubclass hooks into the stream of messages that are dispatched to
    ///     a window.
    /// </summary>
    public abstract class WindowSubclass : System.Runtime.ConstrainedExecution.CriticalFinalizerObject, IDisposable {
        private static readonly User32.WM _disposeMessage;

        static WindowSubclass() {
            _disposeMessage = NativeMethods.RegisterWindowMessage("WindowSubclass.DisposeMessage");
        }

        /// <summary>
        ///     Hooks into the stream of messages that are dispatched to the
        ///     specified window.
        /// </summary>
        /// <remarks>
        ///     The window must be owned by the calling thread.
        /// </remarks>
        public WindowSubclass(User32.HWND hwnd) {
            if (!this.IsCorrectThread(hwnd))
                throw new InvalidOperationException("May not hook a window created by a different thread.");

            this.Hwnd = hwnd;
            _wndproc = this.WndProcStub;
            _wndprocPtr = System.Runtime.InteropServices.Marshal.GetFunctionPointerForDelegate(_wndproc);

            // Because our window proc is an instance method, it is connected
            // to this instance of WindowSubclass, where we can store state.
            // We do not need to store any extra state in native memory.
            NativeMethods.SetWindowSubclass(hwnd, _wndproc, IntPtr.Zero, IntPtr.Zero);
        }

        protected User32.HWND Hwnd { get; private set; }

        ~WindowSubclass() {
            // The finalizer is our last chance!  The finalizer is always on
            // the wrong thread, but we need to ensure that native code will
            // not try to call into us since we are being removed from memory.
            // Even though it is dangerous, and we risk a deadlock, we
            // send the dispose message to the WndProc to remove itself on
            // the correct thread.
            this.DisposeHelper(false);
        }

        protected virtual void Dispose(bool disposing) {
            if (this.Hwnd == null || !this.IsCorrectThread(this.Hwnd))
                throw new InvalidOperationException("Dispose virtual should only be called by WindowSubclass once on the correct thread.");

            NativeMethods.RemoveWindowSubclass(this.Hwnd, _wndproc, IntPtr.Zero);
            this.Hwnd = null;
        }

        protected virtual IntPtr WndProcOverride(User32.HWND hwnd, User32.WM msg, IntPtr wParam, IntPtr lParam, IntPtr id, IntPtr data) {
            // Call the next window proc in the subclass chain.
            return NativeMethods.DefSubclassProc(hwnd, msg, wParam, lParam);
        }

        private bool IsCorrectThread(User32.HWND hwnd) {
            int processId;
            var threadId = NativeMethods.GetWindowThreadProcessId(hwnd, out processId);

            return processId == NativeMethods.GetCurrentProcessId() && threadId == NativeMethods.GetCurrentThreadId();
        }

        private void DisposeHelper(bool disposing) {
            var hwnd = this.Hwnd;

            if (hwnd != null) {
                if (this.IsCorrectThread(hwnd))
                    this.Dispose(disposing);
                else
                    NativeMethods.SendMessage(
                        hwnd, _disposeMessage, _wndprocPtr, disposing
                            ? new IntPtr(1)
                            : IntPtr.Zero);
            }
        }

        private IntPtr WndProcStub(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, IntPtr id, IntPtr data) {
            var hwnd2 = new User32.HWND(hwnd);
            return this.WndProc(hwnd2, (User32.WM) msg, wParam, lParam, id, data);
        }

        private IntPtr WndProc(User32.HWND hwnd, User32.WM msg, IntPtr wParam, IntPtr lParam, IntPtr id, IntPtr data) {
            var retval = IntPtr.Zero;

            try {
                retval = this.WndProcOverride(hwnd, msg, wParam, lParam, id, data);
            }
            finally {
                if (this.Hwnd != User32.HWND.NULL) {
                    System.Diagnostics.Debug.Assert(this.Hwnd == hwnd);

                    if (msg == User32.WM.NCDESTROY)
                        this.Dispose();
                    else if (msg == _disposeMessage && wParam == _wndprocPtr)
                        this.DisposeHelper(
                            lParam == IntPtr.Zero
                                ? false
                                : true);
                }
            }

            return retval;
        }

        public void Dispose() {
            GC.SuppressFinalize(this);
            this.DisposeHelper(true);
        }

        private readonly SUBCLASSPROC _wndproc;
        private readonly IntPtr _wndprocPtr;
    }
}