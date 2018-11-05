using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Win32.User32 {
    /// <summary>
    ///     The "real" ancestor window
    /// </summary>
    public enum GA {
        PARENT = 1,
        ROOT = 2,
        ROOTOWNER = 3
    }
}