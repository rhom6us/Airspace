using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Win32.User32 {
    // TODO: User32 names this enum type INPUT, which conflicts with the struct INPUT...

    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Explicit)]
    public struct INPUT {
        [System.Runtime.InteropServices.FieldOffset(0)]
        public INPUTTYPE type;

        [System.Runtime.InteropServices.FieldOffset(4)]
        public MOUSEINPUT mouseInput;

        [System.Runtime.InteropServices.FieldOffset(4)]
        public KEYBDINPUT keyboardInput;

        [System.Runtime.InteropServices.FieldOffset(4)]
        public HARDWAREINPUT hardwareInput;
    }
}