using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Interop {
    // This is always the root element of a HwndSourceHwndHost's inner frame.
    // It allows the logical parent to be surgically disabled in order to
    // work around the problem with IKIS routed events walking the logical
    // tree and escaping.
    public class HwndSourceHostRoot : System.Windows.Controls.Decorator {
        public static readonly System.Windows.DependencyProperty IsLogicalParentEnabledProperty = System.Windows.DependencyProperty.Register("IsLogicalParentEnabled", typeof(bool), typeof(HwndSourceHostRoot), new System.Windows.UIPropertyMetadata(true));

        public bool IsLogicalParentEnabled {
            get => (bool) this.GetValue(IsLogicalParentEnabledProperty);
            set => this.SetValue(IsLogicalParentEnabledProperty, value);
        }

        protected override System.Windows.DependencyObject GetUIParentCore() {
            if (this.IsLogicalParentEnabled)
                return base.GetUIParentCore();
            return null;
        }

        //TODO: must solve HwndHost brittle base class problem first.
        //protected override void OnRender(System.Windows.Media.DrawingContext drawingContext)
        //{
        //    // Draw something so that rendering does something.  This
        //    // is part of the fix for rendering artifacts, see WndProc
        //    // hooking WM_WINDOWPOSCHANGED too.
        //    drawingContext.DrawRectangle(Brushes.Red, null, new Rect(RenderSize));

        //    base.OnRender(drawingContext);
        //}

        public event EventHandler OnMeasure;

        /// <summary>
        ///     This virtual is called when the system measured the child
        ///     and the result was different.  The default behavior is
        ///     to invalidate measure on the element.  However, since
        ///     we are not visually connected to the containing
        ///     HwndSourceHost, this doesn't do anything useful.  We
        ///     manually tell the HwndSourceHost so it can invalidate
        ///     measure on itself.
        /// </summary>
        protected override void OnChildDesiredSizeChanged(System.Windows.UIElement child) {
            var handler = OnMeasure;
            if (handler != null)
                handler(this, EventArgs.Empty);

            base.OnChildDesiredSizeChanged(child);
        }
    }
}