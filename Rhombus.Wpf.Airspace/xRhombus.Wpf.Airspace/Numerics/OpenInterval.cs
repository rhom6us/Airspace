using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Numerics {
    /// <summary>
    ///     A simple implementation of an open interval.
    /// </summary>
    public struct OpenInterval<T> : IOpenInterval<T> {
        public T Min { get; private set; }
        public T Max { get; private set; }

        // Use the standard notation for intervals.
        public override string ToString() {
            return $"[{this.Min},{this.Max}]";
        }
    }
}