using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Interop {
    /// <summary>
    ///     A simple HwndHost that accepts callbacks for creating and
    ///     destroying the hosted window.
    /// </summary>
    public class CallbackHwndHost : HwndHostEx {
        public CallbackHwndHost(Func<Win32.User32.HWND, Win32.User32.HWND> buildWindow, Action<Win32.User32.HWND> destroyWindow) {
            _buildWindow = buildWindow;
            _destroyWindow = destroyWindow;
        }

        protected override Win32.User32.HWND BuildWindowOverride(Win32.User32.HWND hwndParent) {
            return _buildWindow(hwndParent);
        }

        protected override void DestroyWindowOverride(Win32.User32.HWND hwnd) {
            _destroyWindow(hwnd);
        }

        private readonly Func<Win32.User32.HWND, Win32.User32.HWND> _buildWindow;
        private readonly Action<Win32.User32.HWND> _destroyWindow;
    }
}