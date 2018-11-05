using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Geometry {
    /// <summary>
    ///     A simple implementation of an n-dimensional box.
    /// </summary>
    public struct Box<T> : IBox<T> {
        /// <summary>
        /// </summary>
        /// <param name="point">
        ///     An n-dimensional point for one "corner".
        /// </param>
        /// <param name="vector">
        ///     An n-dimensional vector to the opposite "corner".
        /// </param>
        public Box(IPoint<T> point, IVector<T> vector) {
            var dimensions = point.Dimensions;
            if (dimensions != vector.Dimensions)
                throw new ArgumentException("The point and the vector must agree on dimensionality.");

            _intervals = new Numerics.Interval<T>[dimensions];
            for (var dimension = 0; dimension < dimensions; dimension++) {
                dynamic start = point.GetCoordinate(dimension);
                var end = start + vector.GetDelta(dimension);
                if (start <= end)
                    _intervals[dimension] = new Numerics.Interval<T>(start, true, end, true);
                else
                    _intervals[dimension] = new Numerics.Interval<T>(end, true, start, true);
            }
        }

        public Box(params Numerics.Interval<T>[] intervals) : this((IEnumerable<Numerics.Interval<T>>) intervals) { }

        public Box(IEnumerable<Numerics.Interval<T>> intervals) {
            _intervals = intervals.ToArray();
        }

        public int Dimensions => _intervals == null
            ? 0
            : _intervals.Length;

        public Numerics.Interval<T> GetInterval(int dimension) {
            if (dimension < 0 || _intervals == null || dimension >= _intervals.Length)
                throw new IndexOutOfRangeException();

            return _intervals[dimension];
        }

        private readonly Numerics.Interval<T>[] _intervals;
    }
}