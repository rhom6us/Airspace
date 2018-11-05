using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Win32.User32 {
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public struct KEYBDINPUT {
        public short wVk;
        public short wScan;
        public KEYEVENTF dwFlags;
        public int time;
        public IntPtr dwExtraInfo;
    }
}