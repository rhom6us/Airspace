using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Media {
    public class CacheScale {
        private static CacheScale _auto;

        public CacheScale(double scale) : this((double?) scale) { }

        private CacheScale(double? scale) {
            _scale = scale;
        }

        public static CacheScale Auto {
            get {
                if (_auto == null)
                    _auto = new CacheScale(null);

                return _auto;
            }
        }

        public bool IsAuto => !_scale.HasValue;

        public double Scale => _scale.Value;
        private readonly double? _scale;
    }
}