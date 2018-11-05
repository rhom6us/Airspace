using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Geometry {
    /// <summary>
    ///     A simple implementation of a 1-dimensional size.
    /// </summary>
    public struct Size1D<T> : ISize1D<T> {
        public Size1D(T length) : this() {
            var nZero = default(T);
            dynamic nLength = length;
            if (nLength < nZero)
                throw new InvalidOperationException("Size extents may not be negative.");

            this.Length = length;
        }

        public T Length { get; }
    }
}