using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Mdi {
    public class MdiPanel : System.Windows.Controls.Panel, System.Windows.Controls.Primitives.IScrollInfo {
        private const double LineSize = 16;
        private const double WheelSize = 3 * LineSize;

        public static System.Windows.DependencyProperty WindowStateProperty = System.Windows.DependencyProperty.RegisterAttached("WindowState", typeof(System.Windows.WindowState), typeof(MdiPanel), new System.Windows.FrameworkPropertyMetadata(System.Windows.WindowState.Normal, System.Windows.FrameworkPropertyMetadataOptions.AffectsParentMeasure));

        public static System.Windows.DependencyProperty WindowRectProperty = System.Windows.DependencyProperty.RegisterAttached("WindowRect", typeof(System.Windows.Rect), typeof(MdiPanel), new System.Windows.FrameworkPropertyMetadata(new System.Windows.Rect(), System.Windows.FrameworkPropertyMetadataOptions.AffectsParentMeasure, MdiPanel.OnWindowRectChanged, MdiPanel.CoerceWindowRect));

        public static System.Windows.RoutedEvent WindowRectChangedEvent = System.Windows.EventManager.RegisterRoutedEvent("WindowRectChanged", System.Windows.RoutingStrategy.Direct, typeof(System.Windows.RoutedPropertyChangedEventHandler<System.Windows.Rect>), typeof(MdiPanel));

        private static readonly System.Windows.DependencyPropertyKey ExtentsPropertyKey = System.Windows.DependencyProperty.RegisterReadOnly("Extents", typeof(System.Windows.Rect), typeof(MdiPanel), new System.Windows.FrameworkPropertyMetadata(new System.Windows.Rect(0, 0, 0, 0), (s, e) => ((MdiPanel) s).OnExtentsChanged(e)));

        public static System.Windows.DependencyProperty ExtentsProperty = ExtentsPropertyKey.DependencyProperty;

        public static System.Windows.DependencyProperty ViewportOriginProperty = System.Windows.DependencyProperty.Register("ViewportOrigin", typeof(System.Windows.Point), typeof(MdiPanel), new System.Windows.FrameworkPropertyMetadata(new System.Windows.Point(0, 0), System.Windows.FrameworkPropertyMetadataOptions.AffectsArrange, (s, e) => ((MdiPanel) s).OnViewportOriginChanged(e), (d, bv) => ((MdiPanel) d).CoerceViewportOrigin(bv)));

        private static readonly System.Windows.DependencyPropertyKey BaseWindowRectPropertyKey = System.Windows.DependencyProperty.RegisterAttachedReadOnly("BaseWindowRect", typeof(System.Windows.Rect), typeof(MdiPanel), new System.Windows.FrameworkPropertyMetadata(new System.Windows.Rect(), System.Windows.FrameworkPropertyMetadataOptions.AffectsParentMeasure));

        private static readonly System.Windows.Point Origin = new System.Windows.Point(0, 0);

        public System.Windows.Rect Extents {
            get => (System.Windows.Rect) this.GetValue(ExtentsProperty);
            private set => this.SetValue(ExtentsPropertyKey, value);
        }

        public System.Windows.Point ViewportOrigin {
            get => (System.Windows.Point) this.GetValue(ViewportOriginProperty);
            set => this.SetValue(ViewportOriginProperty, value);
        }

        private IEnumerable<System.Windows.FrameworkElement> ChildrenInZOrder => this.Children.OfType<System.Windows.FrameworkElement>().Where(e => e != null).OrderBy(e => System.Windows.Controls.Panel.GetZIndex(e));

        public static System.Windows.WindowState GetWindowState(System.Windows.FrameworkElement e) {
            return (System.Windows.WindowState) e.GetValue(WindowStateProperty);
        }

        public static void SetWindowState(System.Windows.FrameworkElement e, System.Windows.WindowState value) {
            e.SetValue(WindowStateProperty, value);
        }

        public static System.Windows.Rect GetWindowRect(System.Windows.FrameworkElement e) {
            return (System.Windows.Rect) e.GetValue(WindowRectProperty);
        }

        public static void SetWindowRect(System.Windows.FrameworkElement e, System.Windows.Rect value) {
            e.SetValue(WindowRectProperty, value);
        }

        public static void AddWindowRectChangedHandler(MdiWindow window, System.Windows.RoutedPropertyChangedEventHandler<System.Windows.Rect> handler) {
            window.AddHandler(WindowRectChangedEvent, handler);
        }

        public static void RemoveWindowRectChangedHandler(MdiWindow window, System.Windows.RoutedPropertyChangedEventHandler<System.Windows.Rect> handler) {
            window.RemoveHandler(WindowRectChangedEvent, handler);
        }

        protected override System.Windows.Size MeasureOverride(System.Windows.Size availableSize) {
            var bounds = this.CalculateExtents(
                availableSize, delegate(System.Windows.FrameworkElement child, System.Windows.WindowState windowState, System.Windows.Rect windowRect) {
                    if (windowState == System.Windows.WindowState.Normal) {
                        if (!this.CanHorizontallyScroll)
                            windowRect.Width = Math.Min(windowRect.Width, availableSize.Width);

                        if (!this.CanVerticallyScroll)
                            windowRect.Height = Math.Min(windowRect.Height, availableSize.Height);
                    }

                    child.Measure(windowRect.Size);
                });

            var desiredSize = new System.Windows.Size {
                Width = Math.Min(bounds.Width, availableSize.Width),
                Height = Math.Min(bounds.Height, availableSize.Height)
            };
            return desiredSize;
        }

        protected override System.Windows.Size ArrangeOverride(System.Windows.Size arrangeSize) {
            this.ViewportWidth = arrangeSize.Width;
            this.ViewportHeight = arrangeSize.Height;

            var extents = new System.Windows.Rect(Origin, arrangeSize);
            if (this.CanHorizontallyScroll || this.CanVerticallyScroll) {
                var left = extents.Left;
                var top = extents.Top;
                var right = extents.Right;
                var bottom = extents.Bottom;

                var naturalExtents = this.CalculateExtents(arrangeSize, (c, ws, wr) => { });

                if (this.CanHorizontallyScroll) {
                    left = Math.Min(extents.Left, naturalExtents.Left);
                    right = Math.Max(extents.Right, naturalExtents.Right);
                }

                if (this.CanVerticallyScroll) {
                    top = Math.Min(extents.Top, naturalExtents.Top);
                    bottom = Math.Max(extents.Bottom, naturalExtents.Bottom);
                }

                extents.X = left;
                extents.Y = top;
                extents.Width = right - left;
                extents.Height = bottom - top;
            }

            this.Extents = extents;

            this.CalculateExtents(
                arrangeSize, delegate(System.Windows.FrameworkElement child, System.Windows.WindowState windowState, System.Windows.Rect windowRect) {
                    if (windowState == System.Windows.WindowState.Normal) {
                        child.CoerceValue(WindowRectProperty);
                        windowRect = MdiPanel.GetWindowRect(child);

                        windowRect.X -= this.ViewportOrigin.X;
                        windowRect.Y -= this.ViewportOrigin.Y;
                    }

                    child.Arrange(windowRect);
                });

            if (this.ScrollOwner != null)
                this.ScrollOwner.InvalidateScrollInfo();

            return arrangeSize;
        }

        private System.Windows.Rect CalculateExtents(System.Windows.Size viewportSize, Action<System.Windows.FrameworkElement, System.Windows.WindowState, System.Windows.Rect> callback) {
            double minimizedRowMaxHeight = 0;
            var minimizedWindowPosition = new System.Windows.Point();
            System.Windows.Rect? minimizedWindowExtents = null;
            System.Windows.Rect? normalWindowExtents = null;

            foreach (var child in this.ChildrenInZOrder) {
                var windowState = MdiPanel.GetWindowState(child);
                switch (windowState) {
                    case System.Windows.WindowState.Maximized:
                        callback(child, windowState, new System.Windows.Rect(viewportSize));
                        break;

                    case System.Windows.WindowState.Minimized:
                        var minimumSize = new System.Windows.Size(child.MinWidth, child.MinHeight);

                        if (minimizedWindowPosition.X + minimumSize.Width > viewportSize.Width) {
                            minimizedWindowPosition.X = 0;
                            minimizedWindowPosition.Y += minimizedRowMaxHeight;
                            minimizedRowMaxHeight = 0;
                        }

                        var minimizedWindowRect = new System.Windows.Rect(minimizedWindowPosition, minimumSize);
                        callback(child, windowState, minimizedWindowRect);

                        if (minimizedWindowExtents.HasValue) {
                            var temp = minimizedWindowExtents.Value;
                            temp.Union(minimizedWindowRect);
                            minimizedWindowExtents = temp;
                        }
                        else {
                            minimizedWindowExtents = minimizedWindowRect;
                        }

                        minimizedWindowPosition.X += minimumSize.Width;
                        minimizedRowMaxHeight = Math.Max(minimizedRowMaxHeight, minimumSize.Height);
                        break;

                    case System.Windows.WindowState.Normal:
                    default:
                        var windowRect = (System.Windows.Rect) child.GetValue(BaseWindowRectPropertyKey.DependencyProperty);

                        windowRect.Width = Math.Max(windowRect.Width, child.MinWidth);
                        windowRect.Height = Math.Max(windowRect.Height, child.MinHeight);

                        callback(child, windowState, windowRect);

                        if (normalWindowExtents.HasValue) {
                            var temp = normalWindowExtents.Value;
                            temp.Union(windowRect);
                            normalWindowExtents = temp;
                        }
                        else {
                            normalWindowExtents = windowRect;
                        }

                        break;
                }
            }

            var extents = normalWindowExtents ?? new System.Windows.Rect(0, 0, 0, 0);

            if (minimizedWindowExtents.HasValue)
                extents.Union(new System.Windows.Rect(extents.TopLeft, minimizedWindowExtents.Value.Size));

            return extents;
        }

        private static object CoerceWindowRect(System.Windows.DependencyObject d, object baseValue) {
            var panel = System.Windows.Media.VisualTreeHelper.GetParent(d) as MdiPanel;
            var child = d as System.Windows.FrameworkElement;
            var windowRect = (System.Windows.Rect) baseValue;

            if (panel != null && d != null) {
                d.SetValue(BaseWindowRectPropertyKey, windowRect);

                windowRect.Width = Math.Max(windowRect.Width, child.MinWidth);
                windowRect.Height = Math.Max(windowRect.Height, child.MinHeight);

                windowRect = Extensions.RectExtensions.ConstrainWithin(windowRect, panel.Extents);
            }

            return windowRect;
        }

        private void OnExtentsChanged(System.Windows.DependencyPropertyChangedEventArgs e) {
            this.ViewportOrigin = (System.Windows.Point) this.CoerceViewportOrigin(this.ViewportOrigin);

            if (this.ScrollOwner != null)
                this.ScrollOwner.InvalidateScrollInfo();
        }

        private object CoerceViewportOrigin(object baseValue) {
            var viewportOrigin = (System.Windows.Point) baseValue;

            viewportOrigin.X = Math.Min(Math.Max(viewportOrigin.X, this.Extents.Left), this.Extents.Right - this.ViewportWidth);
            viewportOrigin.Y = Math.Min(Math.Max(viewportOrigin.Y, this.Extents.Top), this.Extents.Bottom - this.ViewportHeight);

            return viewportOrigin;
        }

        private void OnViewportOriginChanged(System.Windows.DependencyPropertyChangedEventArgs e) {
            if (this.ScrollOwner != null)
                this.ScrollOwner.InvalidateScrollInfo();
        }

        private static void OnWindowRectChanged(System.Windows.DependencyObject d, System.Windows.DependencyPropertyChangedEventArgs e) {
            var window = d as MdiWindow;
            if (window != null) {
                var args = new System.Windows.RoutedPropertyChangedEventArgs<System.Windows.Rect>((System.Windows.Rect) e.OldValue, (System.Windows.Rect) e.NewValue, WindowRectChangedEvent);
                window.RaiseEvent(args);
            }
        }

        public System.Windows.Controls.ScrollViewer ScrollOwner { get; set; }
        public bool CanHorizontallyScroll { get; set; }
        public bool CanVerticallyScroll { get; set; }

        public double ViewportWidth { get; private set; }
        public double ViewportHeight { get; private set; }

        public double ExtentWidth => this.Extents.Width;

        public double ExtentHeight => this.Extents.Height;

        public double HorizontalOffset => this.ViewportOrigin.X - this.Extents.Left;

        public double VerticalOffset => this.ViewportOrigin.Y - this.Extents.Top;

        public void LineLeft() {
            this.SetHorizontalOffset(this.HorizontalOffset - LineSize);
        }

        public void LineRight() {
            this.SetHorizontalOffset(this.HorizontalOffset + LineSize);
        }

        public void LineUp() {
            this.SetVerticalOffset(this.VerticalOffset - LineSize);
        }

        public void LineDown() {
            this.SetVerticalOffset(this.VerticalOffset + LineSize);
        }

        public void MouseWheelLeft() {
            this.SetHorizontalOffset(this.HorizontalOffset - WheelSize);
        }

        public void MouseWheelRight() {
            this.SetHorizontalOffset(this.HorizontalOffset + WheelSize);
        }

        public void MouseWheelUp() {
            this.SetVerticalOffset(this.VerticalOffset - WheelSize);
        }

        public void MouseWheelDown() {
            this.SetVerticalOffset(this.VerticalOffset + WheelSize);
        }

        public void PageLeft() {
            this.SetHorizontalOffset(this.HorizontalOffset - this.ViewportWidth);
        }

        public void PageRight() {
            this.SetHorizontalOffset(this.HorizontalOffset + this.ViewportWidth);
        }

        public void PageUp() {
            this.SetVerticalOffset(this.VerticalOffset - this.ViewportHeight);
        }

        public void PageDown() {
            this.SetVerticalOffset(this.VerticalOffset + this.ViewportHeight);
        }

        public System.Windows.Rect MakeVisible(System.Windows.Media.Visual visual, System.Windows.Rect rectangle) {
            rectangle = Extensions.ElementExtensions.TransformElementToElement(((System.Windows.UIElement) visual), rectangle, this);

            rectangle.Offset(new System.Windows.Vector(this.ViewportOrigin.X, this.ViewportOrigin.Y));

            var viewportOrigin = this.ViewportOrigin;
            if (rectangle.Right > viewportOrigin.X + this.ViewportWidth)
                viewportOrigin.X = rectangle.Right - this.ViewportWidth;
            if (rectangle.Left < viewportOrigin.X)
                viewportOrigin.X = rectangle.Left;

            if (rectangle.Bottom > viewportOrigin.Y + this.ViewportHeight)
                viewportOrigin.Y = rectangle.Bottom - this.ViewportHeight;
            if (rectangle.Top < viewportOrigin.Y)
                viewportOrigin.Y = rectangle.Top;

            this.ViewportOrigin = viewportOrigin;

            return rectangle;
        }

        public void SetHorizontalOffset(double offset) {
            var viewportOrigin = new System.Windows.Point(this.Extents.Left + offset, this.ViewportOrigin.Y);

            this.ViewportOrigin = viewportOrigin;
        }

        public void SetVerticalOffset(double offset) {
            var viewportOrigin = new System.Windows.Point(this.ViewportOrigin.X, this.Extents.Top + offset);

            this.ViewportOrigin = viewportOrigin;
        }
    }
}