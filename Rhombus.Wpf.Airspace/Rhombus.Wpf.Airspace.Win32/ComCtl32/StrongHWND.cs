using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Win32.ComCtl32 {
    /// <summary>
    ///     A SafeHandle representing an HWND with strong ownership semantics.
    /// </summary>
    /// <remarks>
    ///     This class is located in the ComCtl32 library because of its
    ///     dependency on WindowSubclass.
    /// </remarks>
    public class StrongHWND : User32.HWND {
        public StrongHWND() : base(true) {
            throw new InvalidOperationException("I need the HWND!");
        }

        public StrongHWND(IntPtr hwnd) : base(hwnd, true) {
            _subclass = new StrongHWNDSubclass(this);
        }

        public static StrongHWND CreateWindowEx(User32.WS_EX dwExStyle, string lpClassName, string lpWindowName, User32.WS dwStyle, int x, int y, int nWidth, int nHeight, User32.HWND hWndParent, IntPtr hMenu, IntPtr hInstance, IntPtr lpParam) {
            var hwnd = NativeMethods.CreateWindowEx(dwExStyle, lpClassName, lpWindowName, dwStyle, x, y, nWidth, nHeight, hWndParent, hMenu, hInstance, lpParam);

            return new StrongHWND(hwnd.DangerousGetHandle());
        }

        protected override bool ReleaseHandle() {
            _subclass.Dispose();
            return true;
        }

        // Called from StrongHWNDSubclass
        protected internal virtual void OnHandleReleased() {
            handle = IntPtr.Zero;
        }

        private readonly StrongHWNDSubclass _subclass;
    }
}