using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Rhombus.Wpf.Airspace.Interop;

namespace Rhombus.Wpf.Airspace.Controls {
    /// <summary>
    ///     WebBrowser is sealed, so we can't derive from it.  Instead, we
    ///     provide a simple element that wraps a WebBrowser and directly
    ///     exposes the WebBrowserExtension properties.
    /// </summary>
    public class WebBrowserEx : System.Windows.FrameworkElement {
        /// <summary>
        ///     A property controlling the behavior for handling the
        ///     SWP_NOCOPYBITS flag during moving or sizing operations.
        /// </summary>
        public static readonly System.Windows.DependencyProperty CopyBitsBehaviorProperty = System.Windows.DependencyProperty.Register("CopyBitsBehavior", typeof(CopyBitsBehavior), typeof(WebBrowserEx), new System.Windows.FrameworkPropertyMetadata(CopyBitsBehavior.Default));

        /// <summary>
        ///     A property controlling whether or not script errors are
        ///     suppressed.
        /// </summary>
        public static readonly System.Windows.DependencyProperty SuppressScriptErrorsProperty = System.Windows.DependencyProperty.Register("SuppressScriptErrors", typeof(bool), typeof(WebBrowserEx), new System.Windows.FrameworkPropertyMetadata(false));

        /// <summary>
        ///     A property controlling whether or not WM_ERASEBKGND is
        ///     suppressed.
        /// </summary>
        public static readonly System.Windows.DependencyProperty SuppressEraseBackgroundProperty = System.Windows.DependencyProperty.Register("SuppressEraseBackground", typeof(bool), typeof(WebBrowserEx), new System.Windows.FrameworkPropertyMetadata(false));

        static WebBrowserEx() {
            // Look up the style for this control by using its type as its key.
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WebBrowserEx), new System.Windows.FrameworkPropertyMetadata(typeof(WebBrowserEx)));
        }

        public WebBrowserEx() {
            this.WebBrowser = new System.Windows.Controls.WebBrowser();
            this.AddVisualChild(this.WebBrowser);

            // Create a binding between the
            // HwndHostExtensions.CopyBitsBehaviorProperty on the child
            // WebBrowser and the CopyBitsBehaviorProperty on this object.
            var bindingCopyBitsBehavior = new System.Windows.Data.Binding("CopyBitsBehavior");
            bindingCopyBitsBehavior.Source = this;
            bindingCopyBitsBehavior.Mode = System.Windows.Data.BindingMode.TwoWay;
            this.WebBrowser.SetBinding(Extensions.HwndHostExtensions.CopyBitsBehaviorProperty, bindingCopyBitsBehavior);

            // Create a binding between the
            // WebBrowserExtensions.SuppressScriptErrorsProperty on the child
            // WebBrowser and the SuppressScriptErrorsProperty on this object.
            var bindingSuppressScriptErrors = new System.Windows.Data.Binding("SuppressScriptErrors");
            bindingSuppressScriptErrors.Source = this;
            bindingSuppressScriptErrors.Mode = System.Windows.Data.BindingMode.TwoWay;
            this.WebBrowser.SetBinding(Extensions.WebBrowserExtensions.SuppressScriptErrorsProperty, bindingSuppressScriptErrors);

            // Create a binding between the
            // WebBrowserExtensions.SuppressEraseBackgroundProperty on the child
            // WebBrowser and the SuppressEraseBackgroundProperty on this object.
            var bindingSuppressEraseBackground = new System.Windows.Data.Binding("SuppressEraseBackground");
            bindingSuppressEraseBackground.Source = this;
            bindingSuppressEraseBackground.Mode = System.Windows.Data.BindingMode.TwoWay;
            this.WebBrowser.SetBinding(Extensions.WebBrowserExtensions.SuppressEraseBackgroundProperty, bindingSuppressEraseBackground);
        }

        /// <summary>
        ///     The behavior for handling the SWP_NOCOPYBITS flag during
        ///     moving or sizing operations.
        /// </summary>
        public CopyBitsBehavior CopyBitsBehavior {
            get => (CopyBitsBehavior) this.GetValue(CopyBitsBehaviorProperty);

            set => this.SetValue(CopyBitsBehaviorProperty, value);
        }

        /// <summary>
        ///     Whether or not to suppress script errors.
        /// </summary>
        public bool SuppressScriptErrors {
            get => (bool) this.GetValue(SuppressScriptErrorsProperty);

            set => this.SetValue(SuppressScriptErrorsProperty, value);
        }

        /// <summary>
        ///     Whether or not to suppress WM_ERASEBKGND.
        /// </summary>
        public bool SuppressEraseBackground {
            get => (bool) this.GetValue(SuppressEraseBackgroundProperty);

            set => this.SetValue(SuppressEraseBackgroundProperty, value);
        }

        public System.Windows.Controls.WebBrowser WebBrowser { get; }

        protected override int VisualChildrenCount => 1;

        protected override System.Windows.Media.Visual GetVisualChild(int index) {
            if (index != 0)
                throw new IndexOutOfRangeException();

            return this.WebBrowser;
        }

        protected override System.Windows.Size MeasureOverride(System.Windows.Size availableSize) {
            this.WebBrowser.Measure(availableSize);
            return this.WebBrowser.DesiredSize;
        }

        protected override System.Windows.Size ArrangeOverride(System.Windows.Size finalSize) {
            this.WebBrowser.Arrange(new System.Windows.Rect(finalSize));
            return this.WebBrowser.RenderSize;
        }
    }
}