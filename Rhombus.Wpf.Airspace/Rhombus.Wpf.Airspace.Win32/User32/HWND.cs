using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Win32.User32 {
    /// <summary>
    ///     A SafeHandle representing an HWND.
    /// </summary>
    /// <remarks>
    ///     HWNDs have very loose ownership semantics.  Unlike normal handles,
    ///     there is no "CloseHandle" API for HWNDs.  There are APIs like
    ///     CloseWindow or DestroyWindow, but these actually affect the window,
    ///     not just your handle to the window.  This SafeHandle type does not
    ///     actually do anything to release the handle in the finalizer, it
    ///     simply provides type safety to the PInvoke signatures.
    ///     The StrongHWND SafeHandle will actually destroy the HWND when it
    ///     is disposed or finalized.
    ///     Because of this loose ownership semantic, the same HWND value can
    ///     be returned from multiple APIs and can be directly compared.  Since
    ///     SafeHandles are actually reference types, we have to override all
    ///     of the comparison methods and operators.  We also support equality
    ///     between null and HWND(IntPtr.Zero).
    /// </remarks>
    public class HWND : System.Runtime.InteropServices.SafeHandle {
        static HWND() {
            HWND.NULL = new HWND(IntPtr.Zero);
            HWND.BROADCAST = new HWND(new IntPtr(0xffff));
            HWND.MESSAGE = new HWND(new IntPtr(-3));
            HWND.DESKTOP = new HWND(new IntPtr(0));
            HWND.TOP = new HWND(new IntPtr(0));
            HWND.BOTTOM = new HWND(new IntPtr(1));
            HWND.TOPMOST = new HWND(new IntPtr(-1));
            HWND.NOTOPMOST = new HWND(new IntPtr(-2));
        }

        /// <summary>
        ///     Public constructor to create an empty HWND SafeHandle instance.
        /// </summary>
        /// <remarks>
        ///     This constructor is used by the marshaller.  The handle value
        ///     is then set directly.
        /// </remarks>
        public HWND() : base(IntPtr.Zero, false) { }

        /// <summary>
        ///     Public constructor to create an HWND SafeHandle instance for
        ///     an existing handle.
        /// </summary>
        public HWND(IntPtr hwnd) : this() {
            this.SetHandle(hwnd);
        }

        /// <summary>
        ///     Constructor for derived classes to control whether or not the
        ///     handle is owned.
        /// </summary>
        protected HWND(bool ownsHandle) : base(IntPtr.Zero, ownsHandle) { }

        /// <summary>
        ///     Constructor for derived classes to specify a handle and to
        ///     control whether or not the handle is owned.
        /// </summary>
        protected HWND(IntPtr hwnd, bool ownsHandle) : base(IntPtr.Zero, ownsHandle) {
            this.SetHandle(hwnd);
        }

        public static HWND NULL { get; }
        public static HWND BROADCAST { get; }
        public static HWND MESSAGE { get; }
        public static HWND DESKTOP { get; }
        public static HWND TOP { get; }
        public static HWND BOTTOM { get; }
        public static HWND TOPMOST { get; }
        public static HWND NOTOPMOST { get; }

        public override bool IsInvalid => !HWND.IsWindow(handle);

        protected override bool ReleaseHandle() {
            // This should never get called, since we specify ownsHandle:false
            // when constructed.
            throw new NotImplementedException();
        }

        public override bool Equals(object obj) {
            if (object.ReferenceEquals(obj, null))
                return handle == IntPtr.Zero;

            var other = obj as HWND;
            return other != null && this.Equals(other);
        }

        public bool Equals(HWND other) {
            if (object.ReferenceEquals(other, null))
                return handle == IntPtr.Zero;
            return other.handle == handle;
        }

        public override int GetHashCode() {
            return handle.GetHashCode();
        }

        public static bool operator ==(HWND lvalue, HWND rvalue) {
            if (object.ReferenceEquals(lvalue, null))
                return object.ReferenceEquals(rvalue, null) || rvalue.handle == IntPtr.Zero;
            if (object.ReferenceEquals(rvalue, null))
                return lvalue.handle == IntPtr.Zero;
            return lvalue.handle == rvalue.handle;
        }

        public static bool operator !=(HWND lvalue, HWND rvalue) {
            return !(lvalue == rvalue);
        }

        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        private static extern bool IsWindow(IntPtr hwnd);
    }
}