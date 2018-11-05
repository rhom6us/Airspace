using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Numerics {
    /// <summary>
    ///     An interface representing a connected portion of the real line
    ///     where each of the endpoints can be open or closed.
    /// </summary>
    public interface IInterval<T> {
        T Min { get; }
        bool IsMinClosed { get; }

        T Max { get; }
        bool IsMaxClosed { get; }
    }
}