using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Interop {
    /// <summary>
    ///     A scrollviewer that uses an AirspaceDecorator to contain the
    ///     ScrollContentPresenter.
    /// </summary>
    /// <remarks>
    ///     All of the interesting details are in the template for this class.
    ///     The template uses an AirspaceDecorator to contain the
    ///     ScrollContentPresenter for the ScrollViewer.
    /// </remarks>
    public class AirspaceScrollViewer : System.Windows.Controls.ScrollViewer {
        static AirspaceScrollViewer() {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AirspaceScrollViewer), new System.Windows.FrameworkPropertyMetadata(typeof(AirspaceScrollViewer)));
        }

        #region AirspaceMode

        public static readonly System.Windows.DependencyProperty AirspaceModeProperty = System.Windows.DependencyProperty.Register(
            /* Name:                 */ "AirspaceMode",
            /* Value Type:           */ typeof(AirspaceMode),
            /* Owner Type:           */ typeof(AirspaceScrollViewer),
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
            /* Owner Type:           */ typeof(AirspaceScrollViewer),
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
            /* Owner Type:           */ typeof(AirspaceScrollViewer),
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
            /* Owner Type:           */ typeof(AirspaceScrollViewer),
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
            /* Owner Type:           */ typeof(AirspaceScrollViewer),
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
            /* Owner Type:           */ typeof(AirspaceScrollViewer),
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
            /* Owner Type:           */ typeof(AirspaceScrollViewer),
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
            /* Owner Type:           */ typeof(AirspaceScrollViewer),
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
    }
}