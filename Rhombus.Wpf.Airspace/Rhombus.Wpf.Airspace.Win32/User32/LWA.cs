using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Win32.User32 {
    /// <summary>
    ///     Flags for SetLayeredWindowAttributes
    /// </summary>
    [Flags]
    public enum LWA {
        COLORKEY = 0x1,
        ALPHA = 0x2
    }
}