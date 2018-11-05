using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Numerics {
    /// <summary>
    ///     The Binary64Exponent struct represents the exponent of a
    ///     double-precision IEEE 754 floating point number.
    /// </summary>
    /// <remarks>
    ///     The IEEE 754 standard calls for biasing the exponent by 1023.
    ///     Biasing provides a simple scheme for encoding both positive and
    ///     negative numbers in an unsigned field.
    /// </remarks>
    public struct Binary64Exponent {
        public Binary64Exponent(uint exponent) {
            if (exponent > 2047)
                throw new ArgumentOutOfRangeException("exponent");

            this.UnbiasedValue = exponent;
        }

        public uint UnbiasedValue { get; }

        public int BiasedValue => (int) this.UnbiasedValue - (int) Bias;
        public Sign Sign => new Sign(this.UnbiasedValue < Bias);

        public const uint Bias = 1023;
    }
}