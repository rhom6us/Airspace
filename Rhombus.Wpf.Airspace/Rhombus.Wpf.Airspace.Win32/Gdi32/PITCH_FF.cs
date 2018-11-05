using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Win32.Gdi32 {
    /// <summary>
    ///     A union of a PITCH and a FF value.
    /// </summary>
    public struct PITCH_FF {
        public PITCH_FF(PITCH pitch = PITCH.DEFAULT, FF family = FF.DONTCARE) {
            this.Value = (int) pitch | (int) family;
        }

        public static implicit operator PITCH_FF(PITCH pitch) {
            return new PITCH_FF(pitch);
        }

        public static implicit operator PITCH_FF(FF family) {
            return new PITCH_FF(PITCH.DEFAULT, family);
        }

        public int Value { get; }
    }
}