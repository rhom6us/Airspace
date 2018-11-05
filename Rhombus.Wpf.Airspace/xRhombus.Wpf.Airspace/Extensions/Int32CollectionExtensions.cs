using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Extensions {
    public static class Int32CollectionExtensions {
        public static void Add(this System.Windows.Media.Int32Collection _this, Tuple<int, int, int> tuple) {
            _this.Add(tuple.Item1);
            _this.Add(tuple.Item2);
            _this.Add(tuple.Item3);
        }
    }
}