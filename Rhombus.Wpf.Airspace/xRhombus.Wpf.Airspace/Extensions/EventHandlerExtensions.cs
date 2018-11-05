using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Extensions {
    public static class EventHandlerExtensions {
        public static void RaiseEvent<T>(this EventHandler<T> handler, object sender, T args) where T : EventArgs {
            if (handler != null)
                handler(sender, args);
        }
    }
}