using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Geometry {
    /// <summary>
    ///     A simple implementation of a 2-dimensional size.
    /// </summary>
    public struct Size2D<T> : ISize2D<T> {
        public Size2D(T width, T height) : this() {
            var nZero = default(T);
            dynamic nWidth = width;
            dynamic nHeight = height;
            if (nWidth < nZero || nHeight < nZero)
                throw new InvalidOperationException("Size extents may not be negative.");

            this.Width = width;
            this.Height = height;
        }

        public T Width { get; }
        public T Height { get; }
    }
}