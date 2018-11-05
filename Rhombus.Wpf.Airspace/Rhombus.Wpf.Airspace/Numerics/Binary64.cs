using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Numerics {
    /// <summary>
    ///     Struct exposing the binary64 floating point format as specified by
    ///     the IEEE Standard for Floating-Point Arithmetic (IEEE 754).
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Explicit)]
    public struct Binary64 {
        public Binary64(double value) : this() {
            _value = value;
        }

        public Binary64(ulong bits) : this() {
            _bits = bits;
        }

        public static implicit operator double(Binary64 value) {
            return value._value;
        }

        public static implicit operator Binary64(double value) {
            return new Binary64(value);
        }

        public static implicit operator ulong(Binary64 value) {
            return value._bits;
        }

        public static implicit operator Binary64(ulong value) {
            return new Binary64(value);
        }

        public ulong Bits => _bits;

        public double Value => _value;

        public Sign Sign => new Sign((_bits & BITS_SIGN) != 0);

        public Binary64Exponent Exponent => new Binary64Exponent((uint) ((_bits & BITS_EXPONENT) >> 52));

        public Binary64Significand Significand => new Binary64Significand(_bits & BITS_SIGNIFICAND, (_bits & BITS_EXPONENT) == 0);

        public bool IsInfinite => (_bits & BITS_EXPONENT) == BITS_EXPONENT && (_bits & BITS_SIGNIFICAND) == 0;

        public bool IsNan => (_bits & BITS_EXPONENT) == BITS_EXPONENT && (_bits & BITS_SIGNIFICAND) != 0;

        public Binary64 NextRepresentableValue(Binary64InsignificantBits insignificantBits) {
            var lsb = (ulong) 1 << insignificantBits;
            var mask = lsb - 1;
            var maxMask = BITS_MAX_POSITIVE ^ mask;

            // Only keep the significant bits, drop the sign.
            var nextBits = _bits & maxMask;

            if (this.Sign.IsNegative) {
                if (nextBits != 0) {
                    // Some negative number getting smaller.
                    nextBits -= lsb;
                    nextBits |= BITS_SIGN;
                }
            }
            else {
                if (nextBits == maxMask)
                    throw new InvalidOperationException();

                // Some positive number getting larger.
                nextBits += lsb;
            }

            return new Binary64(nextBits);
        }

        public Binary64 PreviousRepresentableValue(Binary64InsignificantBits insignificantBits) {
            var lsb = (ulong) 1 << insignificantBits;
            var mask = lsb - 1;
            var maxMask = BITS_MAX_POSITIVE ^ mask;

            // Only keep the significant bits, drop the sign.
            var prevBits = _bits & maxMask;

            if (this.Sign.IsPositive) {
                if (prevBits != 0)
                    prevBits -= lsb;
                else
                    prevBits |= BITS_SIGN;
            }
            else {
                if (prevBits == maxMask)
                    throw new InvalidOperationException();

                // Some negative number getting larger.
                prevBits -= lsb;
                prevBits |= BITS_SIGN;
            }

            return new Binary64(prevBits);
        }

        public Binary64 Round(Binary64InsignificantBits insignificantBits) {
            // Don't round special values...
            if (this.IsNan || this.IsInfinite)
                return new Binary64(_bits);

            var lsb = (ulong) 1 << insignificantBits;
            var mask = lsb - 1;
            var maxMask = BITS_MAX_POSITIVE ^ mask;

            // Only keep the significant bits, drop the sign.
            var bits = _bits & maxMask;

            // Imlpement rounding by adding half of the maximum value that can
            // be represented by the insignificant bits, and then truncating.
            bits += mask / 2;

            // It is possible that the significand just overflowed.  If so, we
            // just rounded up to one of the special values (Infinity or Nan).
            // Simply return +/- infinite.
            if (bits > BITS_MAX_NORMAL) {
                if (this.Sign.IsNegative)
                    bits = BITS_SIGN | BITS_EXPONENT;
                else
                    bits = BITS_EXPONENT;
            }
            else {
                // Restore the sign.  We never round from negative to positive.
                if (this.Sign.IsNegative)
                    bits |= BITS_SIGN;
            }

            return new Binary64(bits);
        }

        private const ulong BITS_SIGNIFICAND = 0x000FFFFFFFFFFFFF;
        private const ulong BITS_EXPONENT = 0x7FF0000000000000;
        private const ulong BITS_SIGN = 0x8000000000000000;
        private const ulong BITS_MAX_POSITIVE = 0x7FFFFFFFFFFFFFFF;
        private const ulong BITS_MAX_NORMAL = 0x7FEFFFFFFFFFFFFF;

        [System.Runtime.InteropServices.FieldOffset(0)]
        private readonly ulong _bits;

        [System.Runtime.InteropServices.FieldOffset(0)]
        private readonly double _value;
    }
}