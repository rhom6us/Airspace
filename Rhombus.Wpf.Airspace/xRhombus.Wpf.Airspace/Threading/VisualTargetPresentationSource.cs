using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Threading {
    /// <summary>
    ///     The VisualTargetPresentationSource represents the root
    ///     of a visual subtree owned by a different thread that the
    ///     visual tree in which is is displayed.
    /// </summary>
    /// <remarks>
    ///     A HostVisual belongs to the same UI thread that owns the
    ///     visual tree in which it resides.
    ///     A HostVisual can reference a VisualTarget owned by another
    ///     thread.
    ///     A VisualTarget has a root visual.
    ///     VisualTargetPresentationSource wraps the VisualTarget and
    ///     enables basic functionality like Loaded, which depends on
    ///     a PresentationSource being available.
    /// </remarks>
    public class VisualTargetPresentationSource : System.Windows.PresentationSource {
        public VisualTargetPresentationSource(System.Windows.Media.HostVisual hostVisual) {
            _visualTarget = new System.Windows.Media.VisualTarget(hostVisual);
        }

        public override System.Windows.Media.Visual RootVisual {
            get => _visualTarget.RootVisual;

            set {
                var oldRoot = _visualTarget.RootVisual;


                // Set the root visual of the VisualTarget.  This visual will
                // now be used to visually compose the scene.
                _visualTarget.RootVisual = value;

                // Hook the SizeChanged event on framework elements for all
                // future changed to the layout size of our root, and manually
                // trigger a size change.
                var rootFE = value as System.Windows.FrameworkElement;
                if (rootFE != null) {
                    rootFE.SizeChanged += this.root_SizeChanged;
                    rootFE.DataContext = _dataContext;

                    // HACK!
                    if (_propertyName != null) {
                        var myBinding = new System.Windows.Data.Binding(_propertyName);
                        myBinding.Source = _dataContext;
                        rootFE.SetBinding(System.Windows.Controls.TextBlock.TextProperty, myBinding);
                    }
                }

                // Tell the PresentationSource that the root visual has
                // changed.  This kicks off a bunch of stuff like the
                // Loaded event.
                this.RootChanged(oldRoot, value);

                // Kickoff layout...
                var rootElement = value as System.Windows.UIElement;
                if (rootElement != null) {
                    rootElement.Measure(new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity));
                    rootElement.Arrange(new System.Windows.Rect(rootElement.DesiredSize));
                }
            }
        }

        public object DataContext {
            get => _dataContext;
            set {
                _dataContext = value;
                var rootElement = _visualTarget.RootVisual as System.Windows.FrameworkElement;
                if (rootElement != null)
                    rootElement.DataContext = _dataContext;
            }
        }

        // HACK!
        public string PropertyName {
            get => _propertyName;
            set {
                _propertyName = value;

                var rootElement = _visualTarget.RootVisual as System.Windows.Controls.TextBlock;
                if (rootElement != null) {
                    if (!rootElement.CheckAccess())
                        throw new InvalidOperationException("What?");

                    var myBinding = new System.Windows.Data.Binding(_propertyName);
                    myBinding.Source = _dataContext;
                    rootElement.SetBinding(System.Windows.Controls.TextBlock.TextProperty, myBinding);
                }
            }
        }

        public override bool IsDisposed => false;

        public event System.Windows.SizeChangedEventHandler SizeChanged;

        protected override System.Windows.Media.CompositionTarget GetCompositionTargetCore() {
            return _visualTarget;
        }

        private void root_SizeChanged(object sender, System.Windows.SizeChangedEventArgs e) {
            var handler = SizeChanged;
            if (handler != null)
                handler(this, e);
        }

        private object _dataContext;
        private string _propertyName;

        private readonly System.Windows.Media.VisualTarget _visualTarget;
    }
}