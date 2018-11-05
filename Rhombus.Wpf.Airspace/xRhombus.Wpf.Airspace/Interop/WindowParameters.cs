using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Interop {
    public class WindowParameters {
        public object Tag { get; set; }
        public IntPtr HINSTANCE { get; set; }
        public System.Windows.Int32Rect WindowRect { get; set; }
        public string Name { get; set; }
        public Win32.User32.WS Style { get; set; }
        public Win32.User32.WS_EX ExtendedStyle { get; set; }

        public Win32.User32.HWND Parent {
            get => _parent ?? Win32.User32.HWND.NULL;

            set => _parent = value;
        }

        private Win32.User32.HWND _parent;
    }
}