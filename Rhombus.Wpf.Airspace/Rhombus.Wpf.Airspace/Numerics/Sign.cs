using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Numerics {
    /// <summary>
    ///     A simple type representing the sign of a numeric.
    /// </summary>
    public struct Sign {
        public Sign(bool isNegative) {
            this.IsNegative = isNegative;
        }

        public bool IsPositive => !this.IsNegative;
        public bool IsNegative { get; }

        public static readonly Sign Positive = new Sign(false);
        public static readonly Sign Negative = new Sign(true);
    }
}