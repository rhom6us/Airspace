using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Threading {
    public sealed class UIThreadPoolThread : IDisposable {
        internal UIThreadPoolThread(System.Windows.Threading.Dispatcher dispatcher) {
            UIThreadPool.IncrementThread(dispatcher);
            this.Dispatcher = dispatcher;
        }

        public System.Windows.Threading.Dispatcher Dispatcher { get; private set; }

        ~UIThreadPoolThread() {
            this.Dispose(false);
        }

        private void Dispose(bool disposing) {
            if (this.Dispatcher == null)
                throw new ObjectDisposedException("UIThreadPoolThread");

            var dispatcher = this.Dispatcher;
            this.Dispatcher = null;

            UIThreadPool.DecrementThread(dispatcher);
        }

        public void Dispose() {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}