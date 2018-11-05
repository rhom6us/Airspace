using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Rhombus.Wpf.Airspace.Interop;

namespace Rhombus.Wpf.Airspace.Mdi {
    public class MdiWindow : System.Windows.Controls.HeaderedContentControl {
        static MdiWindow() {
            // Look up the style for this control by using its type as its key.
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MdiWindow), new System.Windows.FrameworkPropertyMetadata(typeof(MdiWindow)));

            System.Windows.Controls.Panel.ZIndexProperty.OverrideMetadata(
                /* Type:                 */ typeof(MdiWindow),
                /* Metadata:             */ new System.Windows.FrameworkPropertyMetadata(
                    /*     Changed Callback: */ delegate { },
                    /*     Coerce Callback:  */ (d, v) => ((MdiWindow) d).OnCoerceZIndex(v)));

            MdiPanel.WindowStateProperty.OverrideMetadata(
                /* For Type:             */ typeof(MdiWindow),
                /* Metadata:             */ new System.Windows.FrameworkPropertyMetadata(
                    /*     Changed Callback: */ (d, e) => ((MdiWindow) d).OnWindowStateChanged(e)));

            System.Windows.EventManager.RegisterClassHandler(typeof(MdiWindow), System.Windows.Input.Mouse.PreviewMouseDownEvent, (System.Windows.Input.MouseButtonEventHandler) ((s, e) => ((MdiWindow) s).OnMouseActivate(e)));

            System.Windows.EventManager.RegisterClassHandler(typeof(MdiWindow), System.Windows.Input.FocusManager.GotFocusEvent, (System.Windows.RoutedEventHandler) ((s, e) => ((MdiWindow) s).OnGotFocus(e)));

            System.Windows.Input.CommandManager.RegisterClassCommandBinding(
                /* Type:            */ typeof(MdiWindow),
                /* Command Binding: */ new System.Windows.Input.CommandBinding(
                    /*     Command:     */ HwndHostCommands.MouseActivate,
                    /*     Execute:     */ (s, e) => ((MdiWindow) s).ExecuteMouseActivate(e),
                    /*     CanExecute:  */ (s, e) => ((MdiWindow) s).CanExecuteMouseActivate(e)));
        }

        public MdiView View {
            get => _mdiView;

            set {
                _mdiView = value;

                // We delegate the coercion of our properties to the MdiView.
                this.CoerceValue(MdiPanel.WindowStateProperty);
                this.CoerceValue(System.Windows.Controls.Panel.ZIndexProperty);
            }
        }

        public event CancelEventHandler Closing;

        // Called from MdiView
        internal bool Close() {
            var handler = Closing;
            if (handler != null) {
                var e = new CancelEventArgs();

                handler(this, e);

                if (e.Cancel)
                    return false;
            }

            return true;
        }

        private void OnMinimizedContentChanged(System.Windows.DependencyPropertyChangedEventArgs e) {
            this.HasMinimizedContent = e.NewValue != null
                ? true
                : false;
            this.RemoveLogicalChild(e.OldValue);
            this.AddLogicalChild(e.NewValue);
        }

        private object OnCoerceZIndex(object baseValue) {
            if (_mdiView != null)
                return _mdiView.GetZIndex(this);

            return baseValue;
        }

        private void OnWindowStateChanged(System.Windows.DependencyPropertyChangedEventArgs e) {
            if (_mdiView != null)
                _mdiView.SetWindowState(this, (System.Windows.WindowState) e.NewValue);
        }

        private void OnMouseActivate(System.Windows.Input.MouseButtonEventArgs e) {
            MdiCommands.ActivateWindow.Execute(null, this);
        }

        private new void OnGotFocus(System.Windows.RoutedEventArgs e) {
            if (e.OriginalSource == this) {
                if (this.LastFocusedElement != null) {
                    this.LastFocusedElement.Focus();

                    if (this.IsKeyboardFocusWithin && System.Windows.Input.Keyboard.FocusedElement != this)
                        e.Handled = true;
                }
            }
            else {
                this.LastFocusedElement = (System.Windows.UIElement) e.OriginalSource;
            }
        }

        /// <summary>
        ///     Execute handler for the HwndHostCommands.ActivateWindow command.
        /// </summary>
        private void ExecuteMouseActivate(System.Windows.Input.ExecutedRoutedEventArgs e) {
            // Convert any HwndHostCommands.ActivateWindow command coming up
            // our element tree into an MdiCommands.ActivateWindow command.
            MdiCommands.ActivateWindow.Execute(null, this);
        }

        /// <summary>
        ///     CanExecute handler for the HwndHostCommands.ActivateWindow  command.
        /// </summary>
        private void CanExecuteMouseActivate(System.Windows.Input.CanExecuteRoutedEventArgs e) {
            e.CanExecute = true;
        }

        private MdiView _mdiView;

        #region WindowAirspaceMode

        public static readonly System.Windows.DependencyProperty WindowAirspaceModeProperty = System.Windows.DependencyProperty.Register(
            /* Name:                */ "WindowAirspaceMode",
            /* Value Type:          */ typeof(AirspaceMode),
            /* Owner Type:          */ typeof(MdiWindow),
            /* Metadata:            */ new System.Windows.FrameworkPropertyMetadata(
                /*     Default Value:   */ AirspaceMode.None));

        /// <summary>
        ///     The airspace mode for the MdiWindow itself.
        /// </summary>
        /// <remarks>
        ///     In the default style, we use an AirspaceDecorator to implement this property.
        /// </remarks>
        public bool WindowAirspaceMode {
            get => (bool) this.GetValue(WindowAirspaceModeProperty);
            set => this.SetValue(WindowAirspaceModeProperty, value);
        }

        #endregion

        #region WindowClippingBackground

        public static readonly System.Windows.DependencyProperty WindowClippingBackgroundProperty = System.Windows.DependencyProperty.Register(
            /* Name:                 */ "WindowClippingBackground",
            /* Value Type:           */ typeof(System.Windows.Media.Brush),
            /* Owner Type:           */ typeof(MdiWindow),
            /* Metadata:             */ new System.Windows.FrameworkPropertyMetadata(
                /*     Default Value:    */ null));

        /// <summary>
        ///     The brush to paint the background when the airspace mode is
        ///     set to clipping.
        /// </summary>
        public System.Windows.Media.Brush WindowClippingBackground {
            get => (System.Windows.Media.Brush) this.GetValue(WindowClippingBackgroundProperty);
            set => this.SetValue(WindowClippingBackgroundProperty, value);
        }

        #endregion

        #region WindowClippingCopyBitsBehavior

        public static readonly System.Windows.DependencyProperty WindowClippingCopyBitsBehaviorProperty = System.Windows.DependencyProperty.Register(
            /* Name:                 */ "WindowClippingCopyBitsBehavior",
            /* Value Type:           */ typeof(CopyBitsBehavior),
            /* Owner Type:           */ typeof(MdiWindow),
            /* Metadata:             */ new System.Windows.FrameworkPropertyMetadata(
                /*     Default Value:    */ CopyBitsBehavior.Default));

        /// <summary>
        ///     The behavior of copying bits when the airspace mode is set to
        ///     clipping.
        /// </summary>
        public CopyBitsBehavior WindowClippingCopyBitsBehavior {
            get => (CopyBitsBehavior) this.GetValue(WindowClippingCopyBitsBehaviorProperty);
            set => this.SetValue(WindowClippingCopyBitsBehaviorProperty, value);
        }

        #endregion

        #region WindowRedirectionVisibility

        public static readonly System.Windows.DependencyProperty WindowRedirectionVisibilityProperty = System.Windows.DependencyProperty.Register(
            /* Name:                 */ "WindowRedirectionVisibility",
            /* Value Type:           */ typeof(RedirectionVisibility),
            /* Owner Type:           */ typeof(MdiWindow),
            /* Metadata:             */ new System.Windows.FrameworkPropertyMetadata(
                /*     Default Value:    */ RedirectionVisibility.Hidden));

        /// <summary>
        ///     The visibility of the redirection surface.
        /// </summary>
        public RedirectionVisibility WindowRedirectionVisibility {
            get => (RedirectionVisibility) this.GetValue(WindowRedirectionVisibilityProperty);
            set => this.SetValue(WindowRedirectionVisibilityProperty, value);
        }

        #endregion

        #region WindowIsOutputRedirectionEnabled

        public static readonly System.Windows.DependencyProperty WindowIsOutputRedirectionEnabledProperty = System.Windows.DependencyProperty.Register(
            /* Name:                 */ "WindowIsOutputRedirectionEnabled",
            /* Value Type:           */ typeof(bool),
            /* Owner Type:           */ typeof(MdiWindow),
            /* Metadata:             */ new System.Windows.FrameworkPropertyMetadata(
                /*     Default Value:    */ false));

        /// <summary>
        ///     Whether or not output redirection is enabled.
        /// </summary>
        public bool WindowIsOutputRedirectionEnabled {
            get => (bool) this.GetValue(WindowIsOutputRedirectionEnabledProperty);
            set => this.SetValue(WindowIsOutputRedirectionEnabledProperty, value);
        }

        #endregion

        #region WindowOutputRedirectionPeriod

        public static readonly System.Windows.DependencyProperty WindowOutputRedirectionPeriodProperty = System.Windows.DependencyProperty.Register(
            /* Name:                 */ "WindowOutputRedirectionPeriod",
            /* Value Type:           */ typeof(TimeSpan),
            /* Owner Type:           */ typeof(MdiWindow),
            /* Metadata:             */ new System.Windows.FrameworkPropertyMetadata(
                /*     Default Value:    */ TimeSpan.FromMilliseconds(30)));

        /// <summary>
        ///     The period of time to update the output redirection.
        /// </summary>
        public TimeSpan WindowOutputRedirectionPeriod {
            get => (TimeSpan) this.GetValue(WindowOutputRedirectionPeriodProperty);
            set => this.SetValue(WindowOutputRedirectionPeriodProperty, value);
        }

        #endregion

        #region WindowIsInputRedirectionEnabled

        public static readonly System.Windows.DependencyProperty WindowIsInputRedirectionEnabledProperty = System.Windows.DependencyProperty.Register(
            /* Name:                 */ "WindowIsInputRedirectionEnabled",
            /* Value Type:           */ typeof(bool),
            /* Owner Type:           */ typeof(MdiWindow),
            /* Metadata:             */ new System.Windows.FrameworkPropertyMetadata(
                /*     Default Value:    */ false));

        /// <summary>
        ///     Whether or not input redirection is enabled.
        /// </summary>
        public bool WindowIsInputRedirectionEnabled {
            get => (bool) this.GetValue(WindowIsInputRedirectionEnabledProperty);
            set => this.SetValue(WindowIsInputRedirectionEnabledProperty, value);
        }

        #endregion

        #region WindowInputRedirectionPeriod

        public static readonly System.Windows.DependencyProperty WindowInputRedirectionPeriodProperty = System.Windows.DependencyProperty.Register(
            /* Name:                 */ "WindowInputRedirectionPeriod",
            /* Value Type:           */ typeof(TimeSpan),
            /* Owner Type:           */ typeof(MdiWindow),
            /* Metadata:             */ new System.Windows.FrameworkPropertyMetadata(
                /*     Default Value:    */ TimeSpan.FromMilliseconds(30)));

        /// <summary>
        ///     The period of time to update the input redirection.
        /// </summary>
        public TimeSpan WindowInputRedirectionPeriod {
            get => (TimeSpan) this.GetValue(WindowInputRedirectionPeriodProperty);
            set => this.SetValue(WindowInputRedirectionPeriodProperty, value);
        }

        #endregion

        #region ContentAirspaceMode

        public static readonly System.Windows.DependencyProperty ContentAirspaceModeProperty = System.Windows.DependencyProperty.Register(
            /* Name:                */ "ContentAirspaceMode",
            /* Value Type:          */ typeof(AirspaceMode),
            /* Owner Type:          */ typeof(MdiWindow),
            /* Metadata:            */ new System.Windows.FrameworkPropertyMetadata(
                /*     Default Value:   */ AirspaceMode.None));

        /// <summary>
        ///     The airspace mode for the MdiWindow content.
        /// </summary>
        /// <remarks>
        ///     In the default style, we use an AirspaceDecorator to implement this property.
        /// </remarks>
        public bool ContentAirspaceMode {
            get => (bool) this.GetValue(ContentAirspaceModeProperty);
            set => this.SetValue(ContentAirspaceModeProperty, value);
        }

        #endregion

        #region ContentClippingBackground

        public static readonly System.Windows.DependencyProperty ContentClippingBackgroundProperty = System.Windows.DependencyProperty.Register(
            /* Name:                 */ "ContentClippingBackground",
            /* Value Type:           */ typeof(System.Windows.Media.Brush),
            /* Owner Type:           */ typeof(MdiWindow),
            /* Metadata:             */ new System.Windows.FrameworkPropertyMetadata(
                /*     Default Value:    */ null));

        /// <summary>
        ///     The brush to paint the background when the airspace mode is
        ///     set to clipping.
        /// </summary>
        public System.Windows.Media.Brush ContentClippingBackground {
            get => (System.Windows.Media.Brush) this.GetValue(ContentClippingBackgroundProperty);
            set => this.SetValue(ContentClippingBackgroundProperty, value);
        }

        #endregion

        #region ContentClippingCopyBitsBehavior

        public static readonly System.Windows.DependencyProperty ContentClippingCopyBitsBehaviorProperty = System.Windows.DependencyProperty.Register(
            /* Name:                 */ "ContentClippingCopyBitsBehavior",
            /* Value Type:           */ typeof(CopyBitsBehavior),
            /* Owner Type:           */ typeof(MdiWindow),
            /* Metadata:             */ new System.Windows.FrameworkPropertyMetadata(
                /*     Default Value:    */ CopyBitsBehavior.Default));

        /// <summary>
        ///     The behavior of copying bits when the airspace mode is set to
        ///     clipping.
        /// </summary>
        public CopyBitsBehavior ContentClippingCopyBitsBehavior {
            get => (CopyBitsBehavior) this.GetValue(ContentClippingCopyBitsBehaviorProperty);
            set => this.SetValue(ContentClippingCopyBitsBehaviorProperty, value);
        }

        #endregion

        #region ContentRedirectionVisibility

        public static readonly System.Windows.DependencyProperty ContentRedirectionVisibilityProperty = System.Windows.DependencyProperty.Register(
            /* Name:                 */ "ContentRedirectionVisibility",
            /* Value Type:           */ typeof(RedirectionVisibility),
            /* Owner Type:           */ typeof(MdiWindow),
            /* Metadata:             */ new System.Windows.FrameworkPropertyMetadata(
                /*     Default Value:    */ RedirectionVisibility.Hidden));

        /// <summary>
        ///     The visibility of the redirection surface.
        /// </summary>
        public RedirectionVisibility ContentRedirectionVisibility {
            get => (RedirectionVisibility) this.GetValue(ContentRedirectionVisibilityProperty);
            set => this.SetValue(ContentRedirectionVisibilityProperty, value);
        }

        #endregion

        #region ContentIsOutputRedirectionEnabled

        public static readonly System.Windows.DependencyProperty ContentIsOutputRedirectionEnabledProperty = System.Windows.DependencyProperty.Register(
            /* Name:                 */ "ContentIsOutputRedirectionEnabled",
            /* Value Type:           */ typeof(bool),
            /* Owner Type:           */ typeof(MdiWindow),
            /* Metadata:             */ new System.Windows.FrameworkPropertyMetadata(
                /*     Default Value:    */ false));

        /// <summary>
        ///     Whether or not output redirection is enabled.
        /// </summary>
        public bool ContentIsOutputRedirectionEnabled {
            get => (bool) this.GetValue(ContentIsOutputRedirectionEnabledProperty);
            set => this.SetValue(ContentIsOutputRedirectionEnabledProperty, value);
        }

        #endregion

        #region ContentOutputRedirectionPeriod

        public static readonly System.Windows.DependencyProperty ContentOutputRedirectionPeriodProperty = System.Windows.DependencyProperty.Register(
            /* Name:                 */ "ContentOutputRedirectionPeriod",
            /* Value Type:           */ typeof(TimeSpan),
            /* Owner Type:           */ typeof(MdiWindow),
            /* Metadata:             */ new System.Windows.FrameworkPropertyMetadata(
                /*     Default Value:    */ TimeSpan.FromMilliseconds(30)));

        /// <summary>
        ///     The period of time to update the output redirection.
        /// </summary>
        public TimeSpan ContentOutputRedirectionPeriod {
            get => (TimeSpan) this.GetValue(ContentOutputRedirectionPeriodProperty);
            set => this.SetValue(ContentOutputRedirectionPeriodProperty, value);
        }

        #endregion

        #region ContentIsInputRedirectionEnabled

        public static readonly System.Windows.DependencyProperty ContentIsInputRedirectionEnabledProperty = System.Windows.DependencyProperty.Register(
            /* Name:                 */ "ContentIsInputRedirectionEnabled",
            /* Value Type:           */ typeof(bool),
            /* Owner Type:           */ typeof(MdiWindow),
            /* Metadata:             */ new System.Windows.FrameworkPropertyMetadata(
                /*     Default Value:    */ false));

        /// <summary>
        ///     Whether or not input redirection is enabled.
        /// </summary>
        public bool ContentIsInputRedirectionEnabled {
            get => (bool) this.GetValue(ContentIsInputRedirectionEnabledProperty);
            set => this.SetValue(ContentIsInputRedirectionEnabledProperty, value);
        }

        #endregion

        #region ContentInputRedirectionPeriod

        public static readonly System.Windows.DependencyProperty ContentInputRedirectionPeriodProperty = System.Windows.DependencyProperty.Register(
            /* Name:                 */ "ContentInputRedirectionPeriod",
            /* Value Type:           */ typeof(TimeSpan),
            /* Owner Type:           */ typeof(MdiWindow),
            /* Metadata:             */ new System.Windows.FrameworkPropertyMetadata(
                /*     Default Value:    */ TimeSpan.FromMilliseconds(30)));

        /// <summary>
        ///     The period of time to update the input redirection.
        /// </summary>
        public TimeSpan ContentInputRedirectionPeriod {
            get => (TimeSpan) this.GetValue(ContentInputRedirectionPeriodProperty);
            set => this.SetValue(ContentInputRedirectionPeriodProperty, value);
        }

        #endregion

        #region MinimizedContent

        /// <summary>
        ///     The content to display when the MdiWindow is minimized.
        /// </summary>
        public static System.Windows.DependencyProperty MinimizedContentProperty = System.Windows.DependencyProperty.Register(
            /* Name:                 */ "MinimizedContent",
            /* Value Type:           */ typeof(object),
            /* Owner Type:           */ typeof(MdiWindow),
            /* Metadata:             */ new System.Windows.FrameworkPropertyMetadata(
                /*     Default Value:    */ null,
                /*     Changed Callback: */ (d, e) => ((MdiWindow) d).OnMinimizedContentChanged(e)));

        /// <summary>
        ///     The content to display when the MdiWindow is minimized.
        /// </summary>
        public object MinimizedContent {
            get => this.GetValue(MinimizedContentProperty);
            set => this.SetValue(MinimizedContentProperty, value);
        }

        #endregion

        #region MinimizedContentTemplate

        /// <summary>
        ///     The template used to display the minimized content.
        /// </summary>
        public static readonly System.Windows.DependencyProperty MinimizedContentTemplateProperty = System.Windows.DependencyProperty.Register(
            /* Name:                 */ "MinimizedContentTemplate",
            /* Value Type:           */ typeof(System.Windows.DataTemplate),
            /* Owner Type:           */ typeof(MdiWindow),
            /* Metadata:             */ new System.Windows.FrameworkPropertyMetadata(
                /*     Default Value:    */ null));

        /// <summary>
        ///     The template used to display the minimized content.
        /// </summary>
        public System.Windows.DataTemplate MinimizedContentTemplate {
            get => (System.Windows.DataTemplate) this.GetValue(MinimizedContentTemplateProperty);
            set => this.SetValue(MinimizedContentTemplateProperty, value);
        }

        #endregion

        #region MinimizedContentTemplateSelector

        /// <summary>
        ///     The DataTemplateSelector used to select the template to display the minimized content.
        /// </summary>
        /// <remarks>
        ///     A DataTemplateSelector allows the application writer to provide custom logic
        ///     for choosing the template used to display content.
        /// </remarks>
        public static readonly System.Windows.DependencyProperty MinimizedContentTemplateSelectorProperty = System.Windows.DependencyProperty.Register(
            /* Name:                 */ "MinimizedContentTemplateSelector",
            /* Value Type:           */ typeof(System.Windows.Controls.DataTemplateSelector),
            /* Owner Type:           */ typeof(MdiWindow),
            /* Metadata:             */ new System.Windows.FrameworkPropertyMetadata(
                /*     Default Value:    */ null));

        /// <summary>
        ///     The DataTemplateSelector used to select the template to display the minimized content.
        /// </summary>
        /// <remarks>
        ///     A DataTemplateSelector allows the application writer to provide custom logic
        ///     for choosing the template used to display content.
        /// </remarks>
        public System.Windows.Controls.DataTemplateSelector MinimizedContentTemplateSelector {
            get => (System.Windows.Controls.DataTemplateSelector) this.GetValue(MinimizedContentTemplateSelectorProperty);
            set => this.SetValue(MinimizedContentTemplateSelectorProperty, value);
        }

        #endregion

        #region HasMinimizedContent

        /// <summary>
        ///     A private key for setting whether or not the MdiWindow has
        ///     minimized content.
        /// </summary>
        private static readonly System.Windows.DependencyPropertyKey HasMinimizedContentPropertyKey = System.Windows.DependencyProperty.RegisterReadOnly(
            /* Name:              */ "HasMinimizedContent",
            /* Value Type:        */ typeof(bool),
            /* Owner Type:        */ typeof(MdiWindow),
            /* Metadata:          */ new System.Windows.FrameworkPropertyMetadata(
                /*     Default Value: */ false));

        /// <summary>
        ///     Whether or not the MdiWindow has minimized content.
        /// </summary>
        public static readonly System.Windows.DependencyProperty HasMinimizedContentProperty = HasMinimizedContentPropertyKey.DependencyProperty;

        /// <summary>
        ///     Whether or not the MdiWindow has minimized content.
        /// </summary>
        public bool HasMinimizedContent {
            get => (bool) this.GetValue(HasMinimizedContentProperty);
            private set => this.SetValue(HasMinimizedContentPropertyKey, value);
        }

        #endregion

        #region LastFocusElement

        /// <summary>
        ///     A private dependency property key used to set the value
        ///     indicating which element within the MdiWindow last had focus.
        /// </summary>
        private static readonly System.Windows.DependencyPropertyKey LastFocusedElementPropertyKey = System.Windows.DependencyProperty.RegisterReadOnly(
            /* Name:                 */ "LastFocusedElement",
            /* Value Type:           */ typeof(System.Windows.UIElement),
            /* Owner Type:           */ typeof(MdiWindow),
            /* Metadata:             */ new System.Windows.FrameworkPropertyMetadata(
                /*     Default Value:    */ null));

        /// <summary>
        ///     A read-only dependency property indicating which element
        ///     within the MdiWindow last had focus.
        /// </summary>
        public static readonly System.Windows.DependencyProperty LastFocusedElementProperty = LastFocusedElementPropertyKey.DependencyProperty;

        /// <summary>
        ///     Which element within the MdiWindow last had focus.
        /// </summary>
        public System.Windows.UIElement LastFocusedElement {
            get => (System.Windows.UIElement) this.GetValue(LastFocusedElementProperty);
            private set => this.SetValue(LastFocusedElementPropertyKey, value);
        }

        #endregion

        #region IsDragging

        /// <summary>
        ///     A private key for setting whether or not the MdiWindow has
        ///     minimized content.
        /// </summary>
        private static readonly System.Windows.DependencyProperty IsDraggingProperty = System.Windows.DependencyProperty.Register(
            /* Name:              */ "IsDragging",
            /* Value Type:        */ typeof(bool),
            /* Owner Type:        */ typeof(MdiWindow),
            /* Metadata:          */ new System.Windows.FrameworkPropertyMetadata(
                /*     Default Value: */ false));

        /// <summary>
        ///     Whether or not the MdiWindow is being dragged.
        /// </summary>
        public bool IsDragging {
            get => (bool) this.GetValue(IsDraggingProperty);
            set => this.SetValue(IsDraggingProperty, value);
        }

        #endregion
    }
}