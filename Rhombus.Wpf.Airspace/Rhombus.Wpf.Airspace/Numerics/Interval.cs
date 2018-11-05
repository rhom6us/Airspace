using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Numerics {
    /// <summary>
    ///     A simple interval implementation that can have endpoints that are
    ///     either open or closed.
    /// </summary>
    public struct Interval<T> : IInterval<T> {
        public Interval(T min, bool isMinClosed, T max, bool isMaxClosed) : this() {
            this.Min = min;
            this.IsMinClosed = isMinClosed;
            this.Max = max;
            this.IsMaxClosed = isMaxClosed;
        }

        public T Min { get; }
        public bool IsMinClosed { get; }

        public T Max { get; }
        public bool IsMaxClosed { get; }

        public override string ToString() {
            return $"{(this.IsMinClosed ? "(" : "[")}{this.Min},{this.Max}{(this.IsMaxClosed ? ")" : "]")}";
        }
    }
}