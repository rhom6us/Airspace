using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Geometry {
    /// <summary>
    ///     A generic interface for a 1-dimensional size.
    /// </summary>
    public interface ISize1D<T> {
        T Length { get; }
    }
}