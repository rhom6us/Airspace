using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Geometry {
    /// <summary>
    ///     A simple implementation of a 3-dimensional size.
    /// </summary>
    public struct Size3D<T> : ISize3D<T> {
        public Size3D(T width, T height, T depth) : this() {
            var nZero = default(T);
            dynamic nWidth = width;
            dynamic nHeight = height;
            dynamic nDepth = depth;
            if (nWidth < nZero || nHeight < nZero || nDepth < nZero)
                throw new InvalidOperationException("Size extents may not be negative.");

            this.Width = width;
            this.Height = height;
            this.Depth = depth;
        }

        public T Width { get; }
        public T Height { get; }
        public T Depth { get; }
    }
}