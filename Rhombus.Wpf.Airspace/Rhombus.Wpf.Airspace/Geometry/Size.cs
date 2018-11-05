using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Geometry {
    /// <summary>
    ///     A simple implementation of an n-dimensional size.
    /// </summary>
    public struct Size<T> : ISize<T> {
        public Size(params T[] extents) : this((IEnumerable<T>) extents) { }

        public Size(IEnumerable<T> extents) {
            var nZero = default(T);

            foreach (var extent in extents) {
                dynamic nExtent = extent;
                if (nExtent < nZero)
                    throw new InvalidOperationException("Size extents may not be negative.");
            }

            _extents = extents.ToArray();
        }

        public int Dimensions => _extents == null
            ? 0
            : _extents.Length;

        public T GetExtent(int dimension) {
            if (dimension < 0 || _extents == null || dimension >= _extents.Length)
                throw new IndexOutOfRangeException();

            return _extents[dimension];
        }

        private readonly T[] _extents;
    }
}