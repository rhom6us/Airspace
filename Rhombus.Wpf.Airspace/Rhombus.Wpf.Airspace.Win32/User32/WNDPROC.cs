using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Win32.User32 {
    // It would be great to use the HWND type for hwnd, but this is not
    // possible because you will get a MarshalDirectiveException complaining
    // that the unmanaged code cannot pass in a SafeHandle.  Instead, most
    // classes that use a WNDPROC will expose its own virtual that creates
    // new HWND instances for the incomming handles.
    public delegate IntPtr WNDPROC(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam);
}