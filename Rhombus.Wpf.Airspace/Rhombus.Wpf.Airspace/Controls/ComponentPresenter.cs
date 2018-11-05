using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Controls {
    public class ComponentPresenter : System.Windows.FrameworkElement {
        /// <summary>
        ///     The source Uri of the component to present.
        /// </summary>
        public static readonly System.Windows.DependencyProperty SourceProperty = System.Windows.DependencyProperty.Register(
            /* Name:                 */ "Source",
            /* Value Type:           */ typeof(Uri),
            /* Owner Type:           */ typeof(ComponentPresenter),
            /* Metadata:             */ new System.Windows.FrameworkPropertyMetadata(
                /*     Default Value:    */ null,
                /*     Changed Callback: */ (s, e) => ((ComponentPresenter) s).OnSourceChanged(e)));

        /// <summary>
        ///     The source Uri of the component to present.
        /// </summary>
        public Uri Source {
            get => (Uri) this.GetValue(SourceProperty);
            set => this.SetValue(SourceProperty, value);
        }

        /// <summary>
        ///     Return the number of visual children.
        /// </summary>
        protected override int VisualChildrenCount {
            get {
                if (_component != null)
                    return 1;
                return 0;
            }
        }

        /// <summary>
        ///     Return the logical children.
        /// </summary>
        protected override IEnumerator LogicalChildren {
            get {
                if (_component != null)
                    yield return _component;
            }
        }

        /// <summary>
        ///     Return the specified visual child.
        /// </summary>
        protected override System.Windows.Media.Visual GetVisualChild(int index) {
            if (_component == null || index != 0)
                throw new ArgumentOutOfRangeException("index");
            return _component;
        }

        /// <summary>
        ///     Measure the loaded component.
        /// </summary>
        protected override System.Windows.Size MeasureOverride(System.Windows.Size constraint) {
            // Load the component if we haven't already.
            if (!_loaded)
                try {
                    if (this.Source != null) {
                        _component = System.Windows.Application.LoadComponent(this.Source) as System.Windows.FrameworkElement;
                        if (_component != null) {
                            this.AddVisualChild(_component);
                            this.AddLogicalChild(_component);
                        }
                    }
                }
                finally {
                    _loaded = true;
                }

            // Pass on measure to the child.
            if (_component != null) {
                _component.Measure(constraint);
                return _component.DesiredSize;
            }

            return new System.Windows.Size();
        }

        /// <summary>
        ///     Arrange the loaded component.
        /// </summary>
        protected override System.Windows.Size ArrangeOverride(System.Windows.Size arrangeSize) {
            // Pass on arrange to the child.
            if (_component != null)
                _component.Arrange(new System.Windows.Rect(arrangeSize));

            return arrangeSize;
        }

        /// <summary>
        ///     Prepare to load the component from a new source.
        /// </summary>
        private void OnSourceChanged(System.Windows.DependencyPropertyChangedEventArgs e) {
            if (_component != null) {
                this.RemoveVisualChild(_component);
                this.RemoveLogicalChild(_component);
                _component = null;
            }

            _loaded = false;
            this.InvalidateMeasure();
        }

        private System.Windows.FrameworkElement _component;

        private bool _loaded;
    }
}