using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Numerics {
    public static class IntervalExtensions {
        // Support converting any ICloseInterval to an IInterval
        public static IInterval<T> AsInterval<T>(this IClosedInterval<T> closedInterval) {
            return new Interval<T>(closedInterval.Min, true, closedInterval.Max, true);
        }

        // Support converting any IOpenInterval to an IInterval
        public static IInterval<T> AsInterval<T>(this IOpenInterval<T> openInterval) {
            return new Interval<T>(openInterval.Min, false, openInterval.Max, false);
        }
    }
}