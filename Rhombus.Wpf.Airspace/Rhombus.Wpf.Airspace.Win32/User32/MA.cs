using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Win32.User32 {
    /// <summary>
    ///     WM_MOUSEACTIVATE Return Codes
    /// </summary>
    public enum MA {
        ACTIVATE = 1,
        ACTIVATEANDEAT = 2,
        NOACTIVATE = 3,
        NOACTIVATEANDEAT = 4
    }
}