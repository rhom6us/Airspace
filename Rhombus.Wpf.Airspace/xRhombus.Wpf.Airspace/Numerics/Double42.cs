﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Numerics {
    /// <summary>
    ///     Double50 represents a floating point value with a 50-bit mantissa.
    /// </summary>
    /// <remarks>
    /// </remarks>
    public struct Double42 : IEquatable<Double42>, IComparable<Double42> {
        public Double42(double value) {
            _value = value;
        }

        public static implicit operator double(Double42 value) {
            return value._value;
        }

        public static implicit operator Double42(double value) {
            return new Double42(value);
        }

        public static bool operator ==(Double42 a, Double42 b) {
            return a.Equals(b);
        }

        public static bool operator !=(Double42 a, Double42 b) {
            return a.CompareTo(b) != 0;
        }

        public static bool operator <(Double42 a, Double42 b) {
            return a.CompareTo(b) < 0;
        }

        public static bool operator <=(Double42 a, Double42 b) {
            return a.CompareTo(b) <= 0;
        }

        public static bool operator >(Double42 a, Double42 b) {
            return a.CompareTo(b) > 0;
        }

        public static bool operator >=(Double42 a, Double42 b) {
            return a.CompareTo(b) >= 0;
        }

        // IEquatable
        public bool Equals(Double42 other) {
            var a = new Binary64(_value);
            var b = new Binary64(other._value);

            if (a.IsNan || b.IsNan)
                return false;

            // Round away the insignificant bits of both.
            a = a.Round(INSIGNIFICANT_BITS);
            b = b.Round(INSIGNIFICANT_BITS);

            return a.Bits == b.Bits;
        }

        // IComparable
        public int CompareTo(Double42 other) {
            var a = new Binary64(_value);
            var b = new Binary64(other._value);

            if (a.IsNan || b.IsNan)
                throw new ArithmeticException("CompareTo cannot accept Nans.");

            // Round away the insignificant bits of both.
            a = a.Round(INSIGNIFICANT_BITS);
            b = b.Round(INSIGNIFICANT_BITS);

            if (a.Bits < b.Bits)
                return -1;
            if (a.Bits > b.Bits)
                return 1;
            return 0;
        }

        public override bool Equals(object other) {
            if (other is double)
                return this.Equals((double) other);
            if (other is Double42)
                return this.Equals((Double42) other);
            return base.Equals(other);
        }

        public override int GetHashCode() {
            var a = new Binary64(_value);

            // Round away the insignificant bits.
            a = a.Round(INSIGNIFICANT_BITS);
            return a.GetHashCode();
        }

        public Double42 NextRepresentableValue() {
            var a = new Binary64(_value);
            return a.NextRepresentableValue(INSIGNIFICANT_BITS).Value;
        }

        public Double42 PreviousRepresentableValue() {
            var a = new Binary64(_value);
            return a.PreviousRepresentableValue(INSIGNIFICANT_BITS).Value;
        }

        private readonly double _value;
        private static readonly Binary64InsignificantBits INSIGNIFICANT_BITS = 10;
    }
}