using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Interop {
    /// <summary>
    ///     A control that can be configured to use an various airspace
    ///     techniques for the content being presented.
    /// </summary>
    /// <remarks>
    ///     All of the interesting details are in the template for this class.
    ///     The template uses triggers to switch the structure of the visual
    ///     tree according to the AirspaceMode property.
    /// </remarks>
    public class AirspaceDecorator : System.Windows.Controls.ContentControl, System.Windows.Controls.Primitives.IScrollInfo {
        static AirspaceDecorator() {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AirspaceDecorator), new System.Windows.FrameworkPropertyMetadata(typeof(AirspaceDecorator)));
        }

        #region AirspaceMode

        public static readonly System.Windows.DependencyProperty AirspaceModeProperty = System.Windows.DependencyProperty.Register(
            /* Name:                 */ "AirspaceMode",
            /* Value Type:           */ typeof(AirspaceMode),
            /* Owner Type:           */ typeof(AirspaceDecorator),
            /* Metadata:             */ new System.Windows.FrameworkPropertyMetadata(
                /*     Default Value:    */ AirspaceMode.None));

        /// <summary>
        ///     The airspace mode for this instance.
        /// </summary>
        public AirspaceMode AirspaceMode {
            get => (AirspaceMode) this.GetValue(AirspaceModeProperty);
            set => this.SetValue(AirspaceModeProperty, value);
        }

        #endregion

        #region ClippingBackground

        public static readonly System.Windows.DependencyProperty ClippingBackgroundProperty = System.Windows.DependencyProperty.Register(
            /* Name:                 */ "ClippingBackground",
            /* Value Type:           */ typeof(System.Windows.Media.Brush),
            /* Owner Type:           */ typeof(AirspaceDecorator),
            /* Metadata:             */ new System.Windows.FrameworkPropertyMetadata(
                /*     Default Value:    */ null));

        /// <summary>
        ///     The brush to paint the background when the airspace mode is
        ///     set to clipping.
        /// </summary>
        public System.Windows.Media.Brush ClippingBackground {
            get => (System.Windows.Media.Brush) this.GetValue(ClippingBackgroundProperty);
            set => this.SetValue(ClippingBackgroundProperty, value);
        }

        #endregion

        #region ClippingCopyBitsBehavior

        public static readonly System.Windows.DependencyProperty ClippingCopyBitsBehaviorProperty = System.Windows.DependencyProperty.Register(
            /* Name:                 */ "ClippingCopyBitsBehavior",
            /* Value Type:           */ typeof(CopyBitsBehavior),
            /* Owner Type:           */ typeof(AirspaceDecorator),
            /* Metadata:             */ new System.Windows.FrameworkPropertyMetadata(
                /*     Default Value:    */ CopyBitsBehavior.Default));

        /// <summary>
        ///     The behavior of copying bits when the airspace mode is set to
        ///     clipping.
        /// </summary>
        public CopyBitsBehavior ClippingCopyBitsBehavior {
            get => (CopyBitsBehavior) this.GetValue(ClippingCopyBitsBehaviorProperty);
            set => this.SetValue(ClippingCopyBitsBehaviorProperty, value);
        }

        #endregion

        #region RedirectionVisibility

        public static readonly System.Windows.DependencyProperty RedirectionVisibilityProperty = System.Windows.DependencyProperty.Register(
            /* Name:                 */ "RedirectionVisibility",
            /* Value Type:           */ typeof(RedirectionVisibility),
            /* Owner Type:           */ typeof(AirspaceDecorator),
            /* Metadata:             */ new System.Windows.FrameworkPropertyMetadata(
                /*     Default Value:    */ RedirectionVisibility.Hidden));

        /// <summary>
        ///     The visibility of the redirection surface.
        /// </summary>
        public RedirectionVisibility RedirectionVisibility {
            get => (RedirectionVisibility) this.GetValue(RedirectionVisibilityProperty);
            set => this.SetValue(RedirectionVisibilityProperty, value);
        }

        #endregion

        #region IsOutputRedirectionEnabled

        public static readonly System.Windows.DependencyProperty IsOutputRedirectionEnabledProperty = System.Windows.DependencyProperty.Register(
            /* Name:                 */ "IsOutputRedirectionEnabled",
            /* Value Type:           */ typeof(bool),
            /* Owner Type:           */ typeof(AirspaceDecorator),
            /* Metadata:             */ new System.Windows.FrameworkPropertyMetadata(
                /*     Default Value:    */ false));

        /// <summary>
        ///     Whether or not output redirection is enabled.
        /// </summary>
        public bool IsOutputRedirectionEnabled {
            get => (bool) this.GetValue(IsOutputRedirectionEnabledProperty);
            set => this.SetValue(IsOutputRedirectionEnabledProperty, value);
        }

        #endregion

        #region OutputRedirectionPeriod

        public static readonly System.Windows.DependencyProperty OutputRedirectionPeriodProperty = System.Windows.DependencyProperty.Register(
            /* Name:                 */ "OutputRedirectionPeriod",
            /* Value Type:           */ typeof(TimeSpan),
            /* Owner Type:           */ typeof(AirspaceDecorator),
            /* Metadata:             */ new System.Windows.FrameworkPropertyMetadata(
                /*     Default Value:    */ TimeSpan.FromMilliseconds(30)));

        /// <summary>
        ///     The period of time to update the output redirection.
        /// </summary>
        public TimeSpan OutputRedirectionPeriod {
            get => (TimeSpan) this.GetValue(OutputRedirectionPeriodProperty);
            set => this.SetValue(OutputRedirectionPeriodProperty, value);
        }

        #endregion

        #region IsInputRedirectionEnabled

        public static readonly System.Windows.DependencyProperty IsInputRedirectionEnabledProperty = System.Windows.DependencyProperty.Register(
            /* Name:                 */ "IsInputRedirectionEnabled",
            /* Value Type:           */ typeof(bool),
            /* Owner Type:           */ typeof(AirspaceDecorator),
            /* Metadata:             */ new System.Windows.FrameworkPropertyMetadata(
                /*     Default Value:    */ false));

        /// <summary>
        ///     Whether or not input redirection is enabled.
        /// </summary>
        public bool IsInputRedirectionEnabled {
            get => (bool) this.GetValue(IsInputRedirectionEnabledProperty);
            set => this.SetValue(IsInputRedirectionEnabledProperty, value);
        }

        #endregion

        #region InputRedirectionPeriod

        public static readonly System.Windows.DependencyProperty InputRedirectionPeriodProperty = System.Windows.DependencyProperty.Register(
            /* Name:                 */ "InputRedirectionPeriod",
            /* Value Type:           */ typeof(TimeSpan),
            /* Owner Type:           */ typeof(AirspaceDecorator),
            /* Metadata:             */ new System.Windows.FrameworkPropertyMetadata(
                /*     Default Value:    */ TimeSpan.FromMilliseconds(30)));

        /// <summary>
        ///     The period of time to update the input redirection.
        /// </summary>
        public TimeSpan InputRedirectionPeriod {
            get => (TimeSpan) this.GetValue(InputRedirectionPeriodProperty);
            set => this.SetValue(InputRedirectionPeriodProperty, value);
        }

        #endregion

        #region IScrollInfo

        bool System.Windows.Controls.Primitives.IScrollInfo.CanHorizontallyScroll {
            get => this.Content is System.Windows.Controls.Primitives.IScrollInfo realImplementation
                ? realImplementation.CanHorizontallyScroll
                : throw new NotImplementedException();
            set {
                if (!(this.Content is System.Windows.Controls.Primitives.IScrollInfo realImplementation))
                    throw new NotImplementedException();

                realImplementation.CanHorizontallyScroll = value;
            }
        }

        bool System.Windows.Controls.Primitives.IScrollInfo.CanVerticallyScroll {
            get => this.Content is System.Windows.Controls.Primitives.IScrollInfo realImplementation
                ? realImplementation.CanVerticallyScroll
                : throw new NotImplementedException();
            set {
                if (!(this.Content is System.Windows.Controls.Primitives.IScrollInfo realImplementation))
                    throw new NotImplementedException();

                realImplementation.CanVerticallyScroll = value;
            }
        }

        double System.Windows.Controls.Primitives.IScrollInfo.ExtentHeight => this.Content is System.Windows.Controls.Primitives.IScrollInfo realImplementation
            ? realImplementation.ExtentHeight
            : throw new NotImplementedException();

        double System.Windows.Controls.Primitives.IScrollInfo.ExtentWidth => this.Content is System.Windows.Controls.Primitives.IScrollInfo realImplementation
            ? realImplementation.ExtentWidth
            : throw new NotImplementedException();

        double System.Windows.Controls.Primitives.IScrollInfo.HorizontalOffset => this.Content is System.Windows.Controls.Primitives.IScrollInfo realImplementation
            ? realImplementation.HorizontalOffset
            : throw new NotImplementedException();

        void System.Windows.Controls.Primitives.IScrollInfo.LineDown() {
            if (!(this.Content is System.Windows.Controls.Primitives.IScrollInfo realImplementation))
                throw new NotImplementedException();

            realImplementation.LineDown();
        }

        void System.Windows.Controls.Primitives.IScrollInfo.LineLeft() {
            if (!(this.Content is System.Windows.Controls.Primitives.IScrollInfo realImplementation))
                throw new NotImplementedException();

            realImplementation.LineLeft();
        }

        void System.Windows.Controls.Primitives.IScrollInfo.LineRight() {
            if (!(this.Content is System.Windows.Controls.Primitives.IScrollInfo realImplementation))
                throw new NotImplementedException();

            realImplementation.LineRight();
        }

        void System.Windows.Controls.Primitives.IScrollInfo.LineUp() {
            if (!(this.Content is System.Windows.Controls.Primitives.IScrollInfo realImplementation))
                throw new NotImplementedException();

            realImplementation.LineUp();
        }

        System.Windows.Rect System.Windows.Controls.Primitives.IScrollInfo.MakeVisible(System.Windows.Media.Visual visual, System.Windows.Rect rectangle) {
            return this.Content is System.Windows.Controls.Primitives.IScrollInfo realImplementation
                ? realImplementation.MakeVisible(visual, rectangle)
                : throw new NotImplementedException();
        }

        void System.Windows.Controls.Primitives.IScrollInfo.MouseWheelDown() {
            if (!(this.Content is System.Windows.Controls.Primitives.IScrollInfo realImplementation))
                throw new NotImplementedException();

            realImplementation.MouseWheelDown();
        }

        void System.Windows.Controls.Primitives.IScrollInfo.MouseWheelLeft() {
            if (!(this.Content is System.Windows.Controls.Primitives.IScrollInfo realImplementation))
                throw new NotImplementedException();

            realImplementation.MouseWheelLeft();
        }

        void System.Windows.Controls.Primitives.IScrollInfo.MouseWheelRight() {
            if (!(this.Content is System.Windows.Controls.Primitives.IScrollInfo realImplementation))
                throw new NotImplementedException();

            realImplementation.MouseWheelRight();
        }

        void System.Windows.Controls.Primitives.IScrollInfo.MouseWheelUp() {
            if (!(this.Content is System.Windows.Controls.Primitives.IScrollInfo realImplementation))
                throw new NotImplementedException();

            realImplementation.MouseWheelUp();
        }

        void System.Windows.Controls.Primitives.IScrollInfo.PageDown() {
            if (!(this.Content is System.Windows.Controls.Primitives.IScrollInfo realImplementation))
                throw new NotImplementedException();

            realImplementation.PageDown();
        }

        void System.Windows.Controls.Primitives.IScrollInfo.PageLeft() {
            if (!(this.Content is System.Windows.Controls.Primitives.IScrollInfo realImplementation))
                throw new NotImplementedException();

            realImplementation.PageLeft();
        }

        void System.Windows.Controls.Primitives.IScrollInfo.PageRight() {
            if (!(this.Content is System.Windows.Controls.Primitives.IScrollInfo realImplementation))
                throw new NotImplementedException();

            realImplementation.LineRight();
        }

        void System.Windows.Controls.Primitives.IScrollInfo.PageUp() {
            if (!(this.Content is System.Windows.Controls.Primitives.IScrollInfo realImplementation))
                throw new NotImplementedException();

            realImplementation.PageUp();
        }

        System.Windows.Controls.ScrollViewer System.Windows.Controls.Primitives.IScrollInfo.ScrollOwner {
            get => this.Content is System.Windows.Controls.Primitives.IScrollInfo realImplementation
                ? realImplementation.ScrollOwner
                : throw new NotImplementedException();
            set {
                if (!(this.Content is System.Windows.Controls.Primitives.IScrollInfo realImplementation))
                    throw new NotImplementedException();

                realImplementation.ScrollOwner = value;
            }
        }

        void System.Windows.Controls.Primitives.IScrollInfo.SetHorizontalOffset(double offset) {
            if (!(this.Content is System.Windows.Controls.Primitives.IScrollInfo realImplementation))
                throw new NotImplementedException();

            realImplementation.SetHorizontalOffset(offset);
        }

        void System.Windows.Controls.Primitives.IScrollInfo.SetVerticalOffset(double offset) {
            if (!(this.Content is System.Windows.Controls.Primitives.IScrollInfo realImplementation))
                throw new NotImplementedException();

            realImplementation.SetVerticalOffset(offset);
        }

        double System.Windows.Controls.Primitives.IScrollInfo.VerticalOffset => this.Content is System.Windows.Controls.Primitives.IScrollInfo realImplementation
            ? realImplementation.VerticalOffset
            : throw new NotImplementedException();

        double System.Windows.Controls.Primitives.IScrollInfo.ViewportHeight => this.Content is System.Windows.Controls.Primitives.IScrollInfo realImplementation
            ? realImplementation.ViewportHeight
            : throw new NotImplementedException();

        double System.Windows.Controls.Primitives.IScrollInfo.ViewportWidth => this.Content is System.Windows.Controls.Primitives.IScrollInfo realImplementation
            ? realImplementation.ViewportWidth
            : throw new NotImplementedException();

        #endregion
    }
}