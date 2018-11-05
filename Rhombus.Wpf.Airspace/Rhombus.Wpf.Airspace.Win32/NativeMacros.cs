using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Win32 {
    public static class NativeMacros {
        public static ushort HIWORD(uint dword) {
            return (ushort) ((dword >> 16) & 0xFFFF);
        }

        public static ushort LOWORD(uint dword) {
            return (ushort) dword;
        }

        public static int GET_X_LPARAM(uint dword) {
            return unchecked((short) NativeMacros.LOWORD(dword));
        }

        public static int GET_Y_LPARAM(uint dword) {
            return unchecked((short) NativeMacros.HIWORD(dword));
        }

        public static Gdi32.COLORREF RGB(byte r, byte g, byte b) {
            return new Gdi32.COLORREF(r, g, b);
        }
    }
}