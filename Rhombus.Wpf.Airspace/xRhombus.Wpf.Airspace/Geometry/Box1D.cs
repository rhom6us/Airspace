using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Geometry {
    /// <summary>
    ///     A simple implementation of a 1-dimensional box.
    /// </summary>
    public struct Box1D<T> : IBox1D<T> {
        public Box1D(IPoint1D<T> point, IVector1D<T> vector) : this() {
            dynamic startWidth = point.X;
            var endWidth = startWidth + vector.DeltaX;
            if (startWidth <= endWidth)
                this.Width = new Numerics.Interval<T>(startWidth, true, endWidth, true);
            else
                this.Width = new Numerics.Interval<T>(endWidth, true, startWidth, true);
        }

        public Box1D(Numerics.Interval<T> width) : this() {
            this.Width = width;
        }

        public Numerics.Interval<T> Width { get; }
    }
}