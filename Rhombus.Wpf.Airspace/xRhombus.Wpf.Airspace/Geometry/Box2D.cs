using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Geometry {
    /// <summary>
    ///     A simple implementation of a 2-dimensional box.
    /// </summary>
    public struct Box2D<T> : IBox2D<T> {
        public Box2D(IPoint2D<T> point, IVector2D<T> vector) : this() {
            dynamic startWidth = point.X;
            var endWidth = startWidth + vector.DeltaX;
            if (startWidth <= endWidth)
                this.Width = new Numerics.Interval<T>(startWidth, true, endWidth, true);
            else
                this.Width = new Numerics.Interval<T>(endWidth, true, startWidth, true);

            dynamic startHeight = point.Y;
            var endHeight = startHeight + vector.DeltaY;
            if (startHeight <= endHeight)
                this.Height = new Numerics.Interval<T>(startHeight, true, endHeight, true);
            else
                this.Height = new Numerics.Interval<T>(endHeight, true, startHeight, true);
        }

        public Box2D(Numerics.Interval<T> width, Numerics.Interval<T> height) : this() {
            this.Width = width;
            this.Height = height;
        }

        public Numerics.Interval<T> Width { get; }
        public Numerics.Interval<T> Height { get; }
    }
}