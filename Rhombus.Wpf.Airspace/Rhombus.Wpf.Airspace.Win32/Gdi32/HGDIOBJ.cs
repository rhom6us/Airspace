using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Win32.Gdi32 {
    /// <summary>
    ///     A handle to a GDI object.
    /// </summary>
    public abstract class HGDIOBJ : Common.ThreadAffinitizedHandle {
        protected HGDIOBJ() : base(true) { }

        protected HGDIOBJ(IntPtr hObject) : base(true) {
            this.SetHandle(hObject);
        }

        /// <summary>
        ///     Retrieves the GDI object type.
        /// </summary>
        public OBJ ObjectType => HGDIOBJ.GetObjectType(this);

        protected override bool ReleaseHandleSameThread() {
            if (DangerousOwnsHandle)
                HGDIOBJ.DeleteObject(handle);

            return true;
        }

        public bool DangerousOwnsHandle;

        #region PInvoke

        [System.Runtime.InteropServices.DllImport("gdi32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        private static extern OBJ GetObjectType(HGDIOBJ hObject);

        // The handle type is IntPtr because this function is called during
        // handle cleanup, and the SafeHandle itself cannot be marshalled.
        [System.Runtime.InteropServices.DllImport("gdi32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
        private static extern bool DeleteObject(IntPtr hObject);

        #endregion
    }
}