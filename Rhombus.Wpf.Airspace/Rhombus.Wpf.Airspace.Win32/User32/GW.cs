using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Win32.User32 {
    /// <summary>
    ///     GetWindow() Constants
    /// </summary>
    public enum GW {
        HWNDFIRST = 0,
        HWNDLAST = 1,
        HWNDNEXT = 2,
        HWNDPREV = 3,
        OWNER = 4,
        CHILD = 5,
        ENABLEDPOPUP = 6,
        MAX = 6
    }
}