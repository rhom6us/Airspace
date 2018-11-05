using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Geometry {
    /// <summary>
    ///     A generic interface for a 2-dimensional size.
    /// </summary>
    public interface ISize2D<T> {
        T Width { get; }
        T Height { get; }
    }
}