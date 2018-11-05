using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Extensions {
    public static class ElementExtensions {
        /// <summary>
        ///     WPF will use software rendering if the machine is not capable of
        ///     hardware rendering, or if the render mode on the HwndTarget
        ///     specifies SoftwareOnly.  WPF may choose software rendering at other
        ///     times too, such as when a recoverable error happens on the render
        ///     thread.  Note that we can only detect some situations.
        /// </summary>
        public static bool IsSoftwareRendering(this System.Windows.UIElement @this) {
            var isSoftwareRendering = false;

            var hwndSource = System.Windows.PresentationSource.FromVisual(@this) as System.Windows.Interop.HwndSource;
            if (hwndSource != null) {
                var hwndTarget = hwndSource.CompositionTarget;

                isSoftwareRendering = hwndTarget.RenderMode == System.Windows.Interop.RenderMode.SoftwareOnly || System.Windows.Media.RenderCapability.Tier >> 16 == 0;
            }

            return isSoftwareRendering;
        }

        public static T FindAncestor<T>(this System.Windows.UIElement @this) where T : System.Windows.UIElement {
            System.Windows.DependencyObject e = @this;

            do {
                var p = System.Windows.Media.VisualTreeHelper.GetParent(e);
                if (p == null && e is System.Windows.FrameworkElement)
                    p = ((System.Windows.FrameworkElement) e).Parent;
                e = p;
            } while (!(e is T) && e != null);

            return (T) e;
        }

        public static System.Windows.Rect TransformElementToElement(this System.Windows.UIElement @this, System.Windows.Rect rect, System.Windows.UIElement target) {
            // Find the HwndSource for this element and use it to transform
            // the rectangle up into screen coordinates.
            var hwndSource = (System.Windows.Interop.HwndSource) System.Windows.PresentationSource.FromVisual(@this);
            rect = hwndSource.TransformDescendantToClient(rect, @this);
            rect = hwndSource.TransformClientToScreen(rect);

            // Find the HwndSource for the target element and use it to
            // transform the rectangle from screen coordinates down to the
            // target elemnent.
            var targetHwndSource = (System.Windows.Interop.HwndSource) System.Windows.PresentationSource.FromVisual(target);
            rect = targetHwndSource.TransformScreenToClient(rect);
            rect = targetHwndSource.TransformClientToDescendant(rect, target);

            return rect;
        }

        public static System.Windows.Point TransformElementToElement(this System.Windows.UIElement @this, System.Windows.Point pt, System.Windows.UIElement target) {
            // Find the HwndSource for this element and use it to transform
            // the point up into screen coordinates.
            var hwndSource = (System.Windows.Interop.HwndSource) System.Windows.PresentationSource.FromVisual(@this);
            pt = hwndSource.TransformDescendantToClient(pt, @this);
            pt = hwndSource.TransformClientToScreen(pt);

            // Find the HwndSource for the target element and use it to
            // transform the rectangle from screen coordinates down to the
            // target elemnent.
            var targetHwndSource = (System.Windows.Interop.HwndSource) System.Windows.PresentationSource.FromVisual(target);
            pt = targetHwndSource.TransformScreenToClient(pt);
            pt = targetHwndSource.TransformClientToDescendant(pt, target);

            return pt;
        }

        public static System.Windows.Vector TransformElementToElement(this System.Windows.UIElement @this, System.Windows.Vector v, System.Windows.UIElement target) {
            var ptOrigin = @this.TransformElementToElement(new System.Windows.Point(0, 0), target);
            var ptDelta = @this.TransformElementToElement(new System.Windows.Point(v.X, v.Y), target);
            return new System.Windows.Vector(ptDelta.X - ptOrigin.X, ptDelta.Y - ptOrigin.Y);
        }

        public static void DisposeSubTree(this System.Windows.UIElement @this) {
            var childrenCount = System.Windows.Media.VisualTreeHelper.GetChildrenCount(@this);
            for (var iChild = 0; iChild < childrenCount; iChild++) {
                var child = System.Windows.Media.VisualTreeHelper.GetChild(@this, iChild) as System.Windows.UIElement;
                if (child != null) {
                    if (child is IDisposable)
                        ((IDisposable) child).Dispose();
                    else
                        child.DisposeSubTree();
                }
            }
        }
    }
}