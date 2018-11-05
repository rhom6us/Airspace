using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Win32.Kernel32 {
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Explicit)]
    public struct _PROCESSOR_INFO_UNION {
        [System.Runtime.InteropServices.FieldOffset(0)]
        internal uint dwOemId;

        [System.Runtime.InteropServices.FieldOffset(0)]
        internal ushort wProcessorArchitecture;

        [System.Runtime.InteropServices.FieldOffset(2)]
        internal ushort wReserved;
    }
}