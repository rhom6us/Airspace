using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Numerics {
    public struct Int32Point {
        public Int32Point(int x, int y) {
            X = x;
            Y = y;
        }

        public int X;
        public int Y;
    }
}