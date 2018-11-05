using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Geometry {
    /// <summary>
    ///     A simple implementation of an n-dimensional vector.
    /// </summary>
    public struct Vector<T> : IVector<T> {
        public Vector(params T[] deltas) : this((IEnumerable<T>) deltas) { }

        public Vector(IEnumerable<T> deltas) {
            _deltas = deltas.ToArray();
        }

        public int Dimensions => _deltas == null
            ? 0
            : _deltas.Length;

        public T GetDelta(int dimension) {
            if (dimension < 0 || _deltas == null || dimension >= _deltas.Length)
                throw new IndexOutOfRangeException();

            return _deltas[dimension];
        }

        private readonly T[] _deltas;
    }
}