using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Win32.User32 {
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public struct RECT {
        public int left;
        public int top;
        public int right;
        public int bottom;

        public int width => right - left;
        public int height => bottom - top;

        public override string ToString() {
            return "(" + left + ", " + top + "), (" + this.width + " x " + this.height + ")";
        }
    }
}