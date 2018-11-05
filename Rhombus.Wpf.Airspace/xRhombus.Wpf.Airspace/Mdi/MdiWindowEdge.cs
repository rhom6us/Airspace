using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Mdi {
    [Flags]
    public enum MdiWindowEdge {
        None = 0,
        Left = 1,
        Top = 2,
        Right = 4,
        Bottom = 8
    }
}