using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Numerics {
    /// <summary>
    ///     A simple implementation of a closed interval.
    /// </summary>
    public struct ClosedInterval<T> : IClosedInterval<T> {
        public T Min { get; private set; }
        public T Max { get; private set; }


        // Use the standard notation for intervals.
        public override string ToString() {
            return $"({this.Min},{this.Max})";
        }
    }
}