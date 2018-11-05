﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Win32.Common {
    /// <summary>
    ///     A safe handle for Win32 objects that have thread affinity.
    /// </summary>
    /// <remarks>
    ///     This is an abstract base class, it cannot be created directly.  The
    ///     invalid handle is IntPtr.Zero.  Due to thread affinity, handles that
    ///     derive from this class cannot be finalized, they must be disposed
    ///     explicitly and they must be disposed from the thread that created them.
    /// </remarks>
    public abstract class ThreadAffinitizedHandle : System.Runtime.InteropServices.SafeHandle {
        /// <summary>
        ///     Constructor for derived classes to control whether or not the
        ///     handle is owned.
        /// </summary>
        protected ThreadAffinitizedHandle(bool ownsHandle) : base(IntPtr.Zero, ownsHandle) {
            _thread = Thread.CurrentThread;
        }

        public override bool IsInvalid => handle == IntPtr.Zero;

        /// <summary>
        ///     Check whether or not the calling thread has access to this handle.
        /// </summary>
        public bool CheckAccess() {
            return _thread == Thread.CurrentThread;
        }

        /// <summary>
        ///     Verify whether or not the calling thread has access to this handle.
        /// </summary>
        public void VerifyAccess() {
            if (_thread != Thread.CurrentThread)
                throw new InvalidOperationException("Calling thread does not have access to this handle.");
        }

        protected sealed override bool ReleaseHandle() {
            if (this.CheckAccess())
                return this.ReleaseHandleSameThread();
            return false;
        }

        protected abstract bool ReleaseHandleSameThread();

        private readonly Thread _thread;
    }
}