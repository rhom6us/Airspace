using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Mdi {
    public class AdjustWindowRectParameter {
        public AdjustWindowRectParameter() { }

        public AdjustWindowRectParameter(System.Windows.Vector delta, MdiWindowEdge interactiveEdges) {
            this.Delta = delta;
            this.InteractiveEdges = interactiveEdges;
        }

        /// <summary>
        ///     The amount to adjust the window rect by.
        /// </summary>
        public System.Windows.Vector Delta { get; set; }

        /// <summary>
        ///     The edges of the window rect to adust.
        /// </summary>
        /// <remarks>
        ///     Specifying None means to move the window rect by the Delta
        ///     amount.
        /// </remarks>
        public MdiWindowEdge InteractiveEdges { get; set; }
    }
}