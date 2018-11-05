using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Geometry {
    /// <summary>
    ///     A simple implementation of an n-dimensional point.
    /// </summary>
    public struct Point<T> : IPoint<T> {
        public Point(params T[] coordinates) {
            _coordinates = coordinates.ToArray();
        }

        public Point(IEnumerable<T> coordinates) {
            _coordinates = coordinates.ToArray();
        }

        public int Dimensions => _coordinates == null
            ? 0
            : _coordinates.Length;

        public T GetCoordinate(int dimension) {
            if (dimension < 0 || _coordinates == null || dimension >= _coordinates.Length)
                throw new IndexOutOfRangeException();

            return _coordinates[dimension];
        }

        private readonly T[] _coordinates;
    }
}