using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Win32.Gdi32 {
    public class HBRUSH : HGDIOBJ {
        private HBRUSH() { }

        public HBRUSH(IntPtr handle) {
            this.SetHandle(handle);
        }

        public HBRUSH(COLORREF color) {
            var hbrush = HBRUSH.CreateSolidBrush(color.Value);
            this.SetHandle(hbrush.DangerousGetHandle());
            DangerousOwnsHandle = true;
        }

        #region PInvoke

        [System.Runtime.InteropServices.DllImport("gdi32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        private static extern HBRUSH CreateSolidBrush(uint color);

        #endregion
    }
}