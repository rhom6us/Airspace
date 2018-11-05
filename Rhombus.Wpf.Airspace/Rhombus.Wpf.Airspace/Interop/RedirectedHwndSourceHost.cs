using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Interop {
    /// <summary>
    ///     A special type of RedirectedHwndHost that hosts more WPF content
    ///     via an HwndSource.  Since WPF is on both sides of the HWND
    ///     boundary, we can use the logical tree to bridge across, rather
    ///     than the less functional IKeyboardInputSink.
    /// </summary>
    [System.Windows.Markup.ContentProperty("Child")]
    public class RedirectedHwndSourceHost : RedirectedHwndHost {
        /// <summary>
        ///     The child of this HwndSourceHost.
        /// </summary>
        public static System.Windows.DependencyProperty ChildProperty = System.Windows.DependencyProperty.Register(
            /* Name:                 */ "Child",
            /* Value Type:           */ typeof(System.Windows.FrameworkElement),
            /* Owner Type:           */ typeof(RedirectedHwndSourceHost),
            /* Metadata:             */ new System.Windows.PropertyMetadata(
                /*     Default Value:    */ null,
                /*     Property Changed: */ (d, e) => ((RedirectedHwndSourceHost) d).OnChildChanged(e)));

        public System.Windows.FrameworkElement Child {
            get => (System.Windows.FrameworkElement) this.GetValue(ChildProperty);
            set => this.SetValue(ChildProperty, value);
        }

        protected sealed override IEnumerator LogicalChildren {
            get {
                if (_hwndSource != null)
                    yield return _hwndSource.RootVisual;
            }
        }

        protected sealed override Win32.User32.HWND BuildWindowCore(Win32.User32.HWND hwndParent) {
            var hwndSourceParameters = new System.Windows.Interop.HwndSourceParameters();
            hwndSourceParameters.WindowStyle = (int) (Win32.User32.WS.VISIBLE | Win32.User32.WS.CHILD | Win32.User32.WS.CLIPSIBLINGS | Win32.User32.WS.CLIPCHILDREN);
            //hwndSourceParameters.ExtendedWindowStyle = (int)(WS_EX.NOACTIVATE);
            hwndSourceParameters.ParentWindow = hwndParent.DangerousGetHandle();

            _hwndSource = new System.Windows.Interop.HwndSource(hwndSourceParameters);
            _hwndSource.SizeToContent = System.Windows.SizeToContent.Manual;

            // TODO: make this an option
            // On Vista, or when Win7 uses vista-blit, DX content is not
            // available via BitBlit or PrintWindow?  If WPF is using hardware
            // acceleration, anything it renders won't be available either.
            // One workaround is to force WPF to use software rendering.  Of
            // course, this is only a partial workaround since other content
            // like XNA or D2D won't work either.
            //_hwndSource.CompositionTarget.RenderMode = RenderMode.SoftwareOnly;

            // Set the root visual of the HwndSource to an instance of
            // HwndSourceHostRoot.  Hook it up as a logical child if
            // we are on the same thread.
            var root = new HwndSourceHostRoot();
            _hwndSource.RootVisual = root;

            root.OnMeasure += this.OnRootMeasured;
            this.AddLogicalChild(_hwndSource.RootVisual);

            this.SetRootVisual(this.Child);

            return new Win32.User32.HWND(_hwndSource.Handle);
        }

        /// <summary>
        ///     Determine the desired size of this element within the
        ///     specified constraints.
        /// </summary>
        protected override System.Windows.Size MeasureOverride(System.Windows.Size constraint) {
            if (_hwndSource != null && _hwndSource.RootVisual != null) {
                var root = (HwndSourceHostRoot) _hwndSource.RootVisual;

                // We are a simple pass-through element.
                root.Measure(constraint);

                return root.DesiredSize;
            }

            // We don't have a child yet.
            return new System.Windows.Size();
        }

        protected override System.Windows.Size ArrangeOverride(System.Windows.Size finalSize) {
            if (_hwndSource != null && _hwndSource.RootVisual != null) {
                var root = (System.Windows.UIElement) _hwndSource.RootVisual;

                // We are a simple pass-through element.
                root.Arrange(new System.Windows.Rect(finalSize));
                return root.RenderSize;
            }

            // We don't have a child yet.
            return finalSize;
        }

        protected override void Dispose(bool disposing) {
            if (disposing)
                if (_hwndSource != null) {
                    var root = _hwndSource.RootVisual as System.Windows.UIElement;
                    if (root != null)
                        Extensions.ElementExtensions.DisposeSubTree(root);

                    _hwndSource.Dispose();
                    _hwndSource = null;
                }

            base.Dispose(disposing);
        }

        private void OnChildChanged(System.Windows.DependencyPropertyChangedEventArgs e) {
            var child = (System.Windows.FrameworkElement) e.NewValue;
            if (_hwndSource != null)
                this.SetRootVisual(child);
        }

        private object SetRootVisual(object arg) {
            System.Diagnostics.Debug.Assert(_hwndSource != null);

            var child = arg as System.Windows.FrameworkElement;
            if (child == null && arg is Uri)
                child = (System.Windows.FrameworkElement) System.Windows.Application.LoadComponent((Uri) arg);

            var root = (HwndSourceHostRoot) _hwndSource.RootVisual;
            root.Child = child;

            // Invalidate measure on this HwndHost so that we can remeasure
            // ourselves to our content.
            this.InvalidateMeasure();

            return null;
        }

        private void OnRootMeasured(object sender, EventArgs e) {
            // If the root visual gets measured, there is a good chance we may
            // need to be remeasured too.  But since we are not connected
            // visually, we need to propagate this manually.
            //
            // Note: sometimes we cause the measure ourselves, so there is no
            // need to propagate this back.
            this.InvalidateMeasure();
        }

        protected System.Windows.Interop.HwndSource _hwndSource;

        #region IKeyboardInputSink

        // Delegate IKeyboardInputSink calls to the hosted HwndSource.
        protected sealed override bool HasFocusWithinCore() {
            if (_hwndSource != null)
                return ((System.Windows.Interop.IKeyboardInputSink) _hwndSource).HasFocusWithin();
            return base.HasFocusWithinCore();
        }

        // Delegate IKeyboardInputSink calls to the hosted HwndSource.
        protected sealed override bool OnMnemonicCore(ref System.Windows.Interop.MSG msg, System.Windows.Input.ModifierKeys modifiers) {
            if (_hwndSource != null)
                return ((System.Windows.Interop.IKeyboardInputSink) _hwndSource).OnMnemonic(ref msg, modifiers);
            return base.OnMnemonicCore(ref msg, modifiers);
        }

        // Delegate IKeyboardInputSink calls to the hosted HwndSource.
        protected sealed override bool TabIntoCore(System.Windows.Input.TraversalRequest request) {
            if (_hwndSource != null)
                return ((System.Windows.Interop.IKeyboardInputSink) _hwndSource).TabInto(request);
            return base.TabIntoCore(request);
        }

        // Delegate IKeyboardInputSink calls to the hosted HwndSource.
        protected sealed override bool TranslateAcceleratorCore(ref System.Windows.Interop.MSG msg, System.Windows.Input.ModifierKeys modifiers) {
            if (_hwndSource != null) {
                var root = (HwndSourceHostRoot) _hwndSource.RootVisual;

                System.Diagnostics.Debug.Assert(root.IsLogicalParentEnabled);
                root.IsLogicalParentEnabled = false;
                try {
                    return ((System.Windows.Interop.IKeyboardInputSink) _hwndSource).TranslateAccelerator(ref msg, modifiers);
                }
                finally {
                    root.IsLogicalParentEnabled = true;
                }
            }

            return base.TranslateAcceleratorCore(ref msg, modifiers);
        }

        // Delegate IKeyboardInputSink calls to the hosted HwndSource.
        protected sealed override bool TranslateCharCore(ref System.Windows.Interop.MSG msg, System.Windows.Input.ModifierKeys modifiers) {
            if (_hwndSource != null)
                return ((System.Windows.Interop.IKeyboardInputSink) _hwndSource).TranslateChar(ref msg, modifiers);
            return base.TranslateCharCore(ref msg, modifiers);
        }

        #endregion
    }
}