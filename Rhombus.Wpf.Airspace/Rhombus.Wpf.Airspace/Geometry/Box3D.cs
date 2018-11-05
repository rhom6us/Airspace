using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Geometry {
    /// <summary>
    ///     A simple implementation of a 3-dimensional box.
    /// </summary>
    public struct Box3D<T> : IBox3D<T> {
        public Box3D(IPoint3D<T> point, IVector3D<T> vector) : this() {
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

            dynamic startDepth = point.Z;
            var endDepth = startDepth + vector.DeltaZ;
            if (startDepth <= endDepth)
                this.Depth = new Numerics.Interval<T>(startDepth, true, endDepth, true);
            else
                this.Depth = new Numerics.Interval<T>(endDepth, true, startDepth, true);
        }

        public Box3D(Numerics.Interval<T> width, Numerics.Interval<T> height, Numerics.Interval<T> depth) : this() {
            this.Width = width;
            this.Height = height;
            this.Depth = depth;
        }

        public Numerics.Interval<T> Width { get; }
        public Numerics.Interval<T> Height { get; }
        public Numerics.Interval<T> Depth { get; }
    }
}