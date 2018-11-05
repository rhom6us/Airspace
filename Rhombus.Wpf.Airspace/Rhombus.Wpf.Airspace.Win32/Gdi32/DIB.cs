using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Win32.Gdi32 {
    /// <summary>
    ///     DIB color table identifiers
    /// </summary>
    public enum DIB {
        RGB_COLORS = 0, /* color table in RGBs */
        PAL_COLORS = 1  /* color table in palette indices */
    }
}