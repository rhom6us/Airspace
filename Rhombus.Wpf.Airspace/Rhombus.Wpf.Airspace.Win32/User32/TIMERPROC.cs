using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Win32.User32 {
    public delegate void TIMERPROC(IntPtr hwnd, int msg, IntPtr idEvent, int dwTime);
}