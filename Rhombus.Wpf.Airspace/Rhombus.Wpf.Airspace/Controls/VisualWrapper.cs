using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Controls {
    /// <summary>
    ///     The VisualWrapper simply integrates a raw Visual child into a tree
    ///     of FrameworkElements.
    /// </summary>
    [System.Windows.Markup.ContentProperty("Child")]
    public class VisualWrapper<T> : System.Windows.FrameworkElement where T : System.Windows.Media.Visual {
        public T Child {
            get => _child;

            set {
                if (_child != null)
                    this.RemoveVisualChild(_child);

                _child = value;

                if (_child != null)
                    this.AddVisualChild(_child);
            }
        }

        protected override int VisualChildrenCount => _child != null
            ? 1
            : 0;

        protected override System.Windows.Media.Visual GetVisualChild(int index) {
            if (_child != null && index == 0)
                return _child;
            throw new ArgumentOutOfRangeException("index");
        }

        private T _child;
    }

    /// <summary>
    ///     The VisualWrapper simply integrates a raw Visual child into a tree
    ///     of FrameworkElements.
    /// </summary>
    public class VisualWrapper : VisualWrapper<System.Windows.Media.Visual> { }
}