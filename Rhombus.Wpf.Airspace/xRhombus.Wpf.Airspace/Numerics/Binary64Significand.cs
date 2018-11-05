using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Numerics {
    /// <summary>
    ///     A numeric type for representing a fraction.
    /// </summary>
    public struct Binary64Significand {
        public Binary64Significand(ulong value, bool isSubNormal) : this() {
            // Only 52 bits can be specified.
            var mask = ((ulong) 1 << 52) - 1;
            if ((value & ~mask) != 0)
                throw new ArgumentOutOfRangeException("significand");

            this.Value = value;
            this.IsSubnormal = isSubNormal;
        }

        public ulong Value { get; }
        public bool IsSubnormal { get; }

        public double Fraction {
            get {
                var denominator = (ulong) 1 << 52;
                var numerator = this.Value;
                if (!this.IsSubnormal)
                    numerator += denominator;

                return numerator / (double) denominator;
            }
        }
    }
}