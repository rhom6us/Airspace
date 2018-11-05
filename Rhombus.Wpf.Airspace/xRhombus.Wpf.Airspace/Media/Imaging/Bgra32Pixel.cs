using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Media.Imaging {
    /// <summary>
    ///     The Bgra32Pixel struct defines the memory layout of a Bgra32 pixel.
    /// </summary>
    public struct Bgra32Pixel {
        public byte Blue;
        public byte Green;
        public byte Red;
        public byte Alpha;
    }
}