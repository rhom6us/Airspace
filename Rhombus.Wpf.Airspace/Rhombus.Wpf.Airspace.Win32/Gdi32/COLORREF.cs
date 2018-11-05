using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Win32.Gdi32 {
    public struct COLORREF {
        public COLORREF(uint cr) {
            this.Value = cr;
        }

        public COLORREF(byte red, byte green, byte blue) {
            this.Value = (uint) (red | (green << 8) | (blue << 16));
        }

        public uint Value { get; }
    }
}