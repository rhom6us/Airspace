using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Numerics {
    /// <summary>
    ///     An interface representing a connected portion of the real line
    ///     where each of the endpoints is closed.
    /// </summary>
    public interface IClosedInterval<T> {
        T Min { get; }
        T Max { get; }
    }
}