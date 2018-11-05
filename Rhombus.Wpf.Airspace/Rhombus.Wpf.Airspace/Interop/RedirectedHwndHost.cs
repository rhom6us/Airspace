using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Interop {
    public class RedirectedHwndHost : System.Windows.FrameworkElement, IDisposable, System.Windows.Interop.IKeyboardInputSink {
        private static readonly WindowClass<RedirectedWindow> _redirectionWindowFactory;

        static RedirectedHwndHost() {
            _redirectionWindowFactory = new WindowClass<RedirectedWindow>();
            _redirectionWindowFactory.BeginInit();
            _redirectionWindowFactory.Type = WindowClassType.ApplicationLocal;
            //_redirectionWindowFactory.Background = NativeMethods.GetStockObject(5);
            _redirectionWindowFactory.EndInit();
        }

        public RedirectedHwndHost() {
            System.Windows.PresentationSource.AddSourceChangedHandler(this, (s, e) => this.CurrentHwndSource = (System.Windows.Interop.HwndSource) e.NewSource);
            Loaded += this.OnLoaded;

            _inputRedirectionTimer = new System.Windows.Threading.DispatcherTimer(System.Windows.Threading.DispatcherPriority.Input);
            _inputRedirectionTimer.Tick += (e, a) => this.UpdateInputRedirection();
            _inputRedirectionTimer.Interval = this.InputRedirectionPeriod;
            _inputRedirectionTimer.IsEnabled = this.IsInputRedirectionEnabled;

            _outputRedirectionTimer = new System.Windows.Threading.DispatcherTimer(System.Windows.Threading.DispatcherPriority.Render);
            _outputRedirectionTimer.Tick += (e, a) => this.UpdateOutputRedirection();
            _outputRedirectionTimer.Interval = this.OutputRedirectionPeriod;
            _outputRedirectionTimer.IsEnabled = this.IsOutputRedirectionEnabled;
        }

        /// <summary>
        ///     The window handle of the hosted child window.
        /// </summary>
        /// <remarks>
        ///     Derived types override the BuildWindowCore virtual method to
        ///     create the child window, the handle is exposed through this
        ///     property.
        /// </remarks>
        public Win32.User32.HWND Handle { get; private set; }

        protected virtual void OnCurrentHwndSourceChanged(System.Windows.DependencyPropertyChangedEventArgs e) {
            this.Initialize();

            // Unregister the old keyboard input site.
            var keyboardInputSite = ((System.Windows.Interop.IKeyboardInputSink) this).KeyboardInputSite;
            if (keyboardInputSite != null) {
                ((System.Windows.Interop.IKeyboardInputSink) this).KeyboardInputSite = null;
                keyboardInputSite.Unregister();
            }

            // Register the new keyboard input site with the containing
            // HwndSource.
            System.Windows.Interop.IKeyboardInputSink sink = this.CurrentHwndSource;
            if (sink != null)
                ((System.Windows.Interop.IKeyboardInputSink) this).KeyboardInputSite = sink.RegisterKeyboardInputSink(this);

            // Set the owner of the RedirectedWindow to our CurrentHwndSource.
            // This keeps the RedirectedWindow on top of the HwndSource.
            if (this.CurrentHwndSource != null) {
                var hwndSource = new Win32.User32.HWND(this.CurrentHwndSource.Handle);
                var hwndRoot = hwndSource; // User32NativeMethods.GetAncestor(hwndSource, GA.ROOT); // need to get the top-level window?
                Win32.NativeMethods.SetWindowLongPtr(_redirectedWindow.Handle, Win32.User32.GWL.HWNDPARENT, hwndRoot.DangerousGetHandle());
            }
        }

        /// <summary>
        ///     Creates the hosted child window.
        /// </summary>
        /// <remarks>
        ///     Derived types override the BuildWindowCore virtual method to
        ///     create the child window.  The child window is parented to
        ///     a seperate top-level window for the purposes of redirection.
        ///     The SafeWindowHandle type controls the lifetime of the
        ///     child window.  It will be disposed when the RedirectedHwndHost
        ///     is disposed, or when the SafeWindowHandle is finalized.  Set
        ///     the SafeWindowHandle.DestroyWindowOnRelease property to true
        ///     if you want the window destroyed automatically.
        /// </remarks>
        protected virtual Win32.User32.HWND BuildWindowCore(Win32.User32.HWND hwndParent) {
            var hwndChild = Win32.ComCtl32.StrongHWND.CreateWindowEx(0, "STATIC", "Default RedirectedHwndHost Window", Win32.User32.WS.CHILD | Win32.User32.WS.CLIPSIBLINGS | Win32.User32.WS.CLIPCHILDREN, 0, 0, 0, 0, hwndParent, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);

            return hwndChild;
        }

        protected override void OnRender(System.Windows.Media.DrawingContext drawingContext) {
            if (_bitmap != null) {
                drawingContext.PushClip(new System.Windows.Media.RectangleGeometry(new System.Windows.Rect(this.RenderSize)));
                drawingContext.DrawImage(_bitmap, new System.Windows.Rect(new System.Windows.Size(_bitmap.Width, _bitmap.Height)));
                drawingContext.Pop();
            }
        }

        protected override void OnRenderSizeChanged(System.Windows.SizeChangedInfo sizeInfo) {
            var dpiScale = this.CurrentHwndSource.CompositionTarget.TransformToDevice;
            var vSize = new System.Windows.Vector(sizeInfo.NewSize.Width, sizeInfo.NewSize.Height);
            vSize = dpiScale.Transform(vSize);

            var width = (int) Math.Ceiling(vSize.X);
            var height = (int) Math.Ceiling(vSize.Y);

            // Size the child window to be the natural size of the element.
            Win32.NativeMethods.SetWindowPos(this.Handle, Win32.User32.HWND.NULL, 0, 0, width, height, Win32.User32.SWP.NOZORDER | Win32.User32.SWP.NOCOPYBITS);

            // Size the redirected window to contain the child window.
            _redirectedWindow.SetClientAreaSize(width, height);

            if (this.IsOutputRedirectionEnabled)
                this.UpdateOutputRedirection();

            base.OnRenderSizeChanged(sizeInfo);
        }

        private void OnLoaded(object sender, System.Windows.RoutedEventArgs e) {
            // Only need this once.
            Loaded -= this.OnLoaded;

            this.Initialize();
        }

        private void Initialize() {
            if (this.Handle != null)
                return;

            // When loaded for the first time, build the top-level redirected
            // window to host the child window.
            var windowParams = new WindowParameters();
            windowParams.Name = "RedirectedHwndHost";
            windowParams.Style = Win32.User32.WS.OVERLAPPED | Win32.User32.WS.CLIPCHILDREN | Win32.User32.WS.CAPTION;
            windowParams.ExtendedStyle = Win32.User32.WS_EX.LAYERED | Win32.User32.WS_EX.NOACTIVATE | Win32.User32.WS_EX.TOOLWINDOW | Win32.User32.WS_EX.TRANSPARENT;
            windowParams.WindowRect = new System.Windows.Int32Rect(0, 0, 500, 500);

            _redirectedWindow = _redirectionWindowFactory.CreateWindow(windowParams);
            this.UpdateRedirectedWindowSettings(this.RedirectionVisibility, false);

            // Then create the child window to host.
            this.Handle = this.BuildWindowCore(_redirectedWindow.Handle);
            if (this.Handle == null || this.Handle.IsInvalid)
                throw new InvalidOperationException("BuildWindowCore did not return a valid handle.");

            _redirectedWindow.Show(WindowShowState.Normal, false);
        }

        private void UpdateInputRedirection() {
            var messagePos = Win32.NativeMethods.GetMessagePos();
            var xScreen = Win32.NativeMacros.GET_X_LPARAM(messagePos);
            var yScreen = Win32.NativeMacros.GET_Y_LPARAM(messagePos);

            var ptClient = this.GetInputSyncPoint(xScreen, yScreen);
            if (ptClient.HasValue) {
                _redirectedWindow.AlignClientAndScreen(ptClient.Value.x, ptClient.Value.y, xScreen, yScreen);
                this.UpdateRedirectedWindowSettings(this.RedirectionVisibility, true);
            }
            else {
                this.UpdateRedirectedWindowSettings(this.RedirectionVisibility, false);
                // TODO: shove the window away? (on an animation?)
            }
        }

        private void UpdateRedirectedWindowSettings(RedirectionVisibility visibility, bool isMouseOver) {
            if (_redirectedWindow != null)
                switch (visibility) {
                    case RedirectionVisibility.Visible:
                        _redirectedWindow.Alpha = 100;
                        _redirectedWindow.IsHitTestable = isMouseOver;
                        break;

                    case RedirectionVisibility.Interactive:
                        _redirectedWindow.Alpha = 100;
                        _redirectedWindow.IsHitTestable = true;
                        break;

                    default:
                    case RedirectionVisibility.Hidden:
                        _redirectedWindow.Alpha = 1; // Not *quite* invisible, which is important so we can still capture content.
                        _redirectedWindow.IsHitTestable = isMouseOver;
                        break;
                }
        }

        /// <summary>
        ///     Return the point to sync the window to.  The point is in the coordinate
        ///     space of the image, which is the same as the client coordinate space
        ///     of the hosted window.  This function returns null if the input should
        ///     not be synchronized for this redirected window.
        /// </summary>
        /// <returns></returns>
        private Win32.User32.POINT? GetInputSyncPoint(int xScreen, int yScreen) {
            Win32.User32.POINT? ptClient = null;

            var currentHwndSource = this.CurrentHwndSource;
            if (currentHwndSource != null) {
                var hwndCapture = Win32.NativeMethods.GetCapture();
                if (hwndCapture != Win32.User32.HWND.NULL) {
                    // The mouse is captured, so only sync input if the mouse is
                    // captured to a hosted window within us.
                    var root = Win32.NativeMethods.GetAncestor(hwndCapture, Win32.User32.GA.ROOT);
                    if (_redirectedWindow.Handle == root) {
                        // The HWND with capture is within us.
                        // Transform the screen coordinates into the local coordinates.
                        var pt = new System.Windows.Point(xScreen, yScreen);
                        pt = Extensions.HwndSourceExtensions.TransformScreenToClient(currentHwndSource, pt);
                        pt = Extensions.PresentationSourceExtensions.TransformClientToRoot(currentHwndSource, pt);
                        pt = currentHwndSource.RootVisual.TransformToDescendant(this).Transform(pt);

                        ptClient = new Win32.User32.POINT {x = (int) Math.Round(pt.X), y = (int) Math.Round(pt.Y)};
                    }
                }
                else {
                    // The mouse is not captured, so only sync input if the mouse
                    // is over our element.
                    // Convert the mouse coordinates to the client coordinates of the
                    // HwndSource.
                    var pt = new System.Windows.Point(xScreen, yScreen);
                    pt = Extensions.HwndSourceExtensions.TransformScreenToClient(currentHwndSource, pt);
                    pt = Extensions.PresentationSourceExtensions.TransformClientToRoot(currentHwndSource, pt);
                    var hit = ((System.Windows.UIElement) currentHwndSource.RootVisual).InputHitTest(pt) as RedirectedHwndHost;

                    if (hit == this) {
                        // Transform into the coordinate space of the
                        // RedirectedHwndHost element.
                        var xfrm = currentHwndSource.RootVisual.TransformToDescendant(hit);
                        pt = xfrm.Transform(pt);
                        ptClient = new Win32.User32.POINT {x = (int) Math.Round(pt.X), y = (int) Math.Round(pt.Y)};
                    }
                }
            }

            return ptClient;
        }

        private void UpdateOutputRedirection() {
            var bitmap = _redirectedWindow.UpdateRedirectedBitmap();
            if (bitmap == _bitmap)
                return;
            _bitmap = bitmap;
            this.InvalidateVisual();
        }

        private System.Windows.Media.Imaging.BitmapSource _bitmap;
        private System.Windows.Threading.DispatcherTimer _inputRedirectionTimer;
        private System.Windows.Threading.DispatcherTimer _outputRedirectionTimer;
        internal RedirectedWindow _redirectedWindow;

        #region RedirectionVisibility

        public static readonly System.Windows.DependencyProperty RedirectionVisibilityProperty = System.Windows.DependencyProperty.Register(
            /* Name:                 */ "RedirectionVisibility",
            /* Value Type:           */ typeof(RedirectionVisibility),
            /* Owner Type:           */ typeof(RedirectedHwndHost),
            /* Metadata:             */ new System.Windows.FrameworkPropertyMetadata(
                /*     Default Value:    */ RedirectionVisibility.Hidden,
                /*     Property Changed: */ (d, e) => ((RedirectedHwndHost) d).OnRedirectionVisibilityChanged(e)));

        /// <summary>
        ///     The visibility of the redirection surface.
        /// </summary>
        public RedirectionVisibility RedirectionVisibility {
            get => (RedirectionVisibility) this.GetValue(RedirectionVisibilityProperty);
            set => this.SetValue(RedirectionVisibilityProperty, value);
        }

        private void OnRedirectionVisibilityChanged(System.Windows.DependencyPropertyChangedEventArgs e) {
            this.UpdateRedirectedWindowSettings(this.RedirectionVisibility, false);
        }

        #endregion

        #region IsOutputRedirectionEnabled

        public static readonly System.Windows.DependencyProperty IsOutputRedirectionEnabledProperty = System.Windows.DependencyProperty.Register(
            /* Name:                 */ "IsOutputRedirectionEnabled",
            /* Value Type:           */ typeof(bool),
            /* Owner Type:           */ typeof(RedirectedHwndHost),
            /* Metadata:             */ new System.Windows.FrameworkPropertyMetadata(
                /*     Default Value:    */ false,
                /*     Property Changed: */ (d, e) => ((RedirectedHwndHost) d).OnIsOutputRedirectionEnabledChanged(e)));

        /// <summary>
        ///     Whether or not output redirection is enabled.
        /// </summary>
        public bool IsOutputRedirectionEnabled {
            get => (bool) this.GetValue(IsOutputRedirectionEnabledProperty);
            set => this.SetValue(IsOutputRedirectionEnabledProperty, value);
        }

        private void OnIsOutputRedirectionEnabledChanged(System.Windows.DependencyPropertyChangedEventArgs e) {
            _outputRedirectionTimer.IsEnabled = (bool) e.NewValue;
        }

        #endregion

        #region OutputRedirectionPeriod

        public static readonly System.Windows.DependencyProperty OutputRedirectionPeriodProperty = System.Windows.DependencyProperty.Register(
            /* Name:                 */ "OutputRedirectionPeriod",
            /* Value Type:           */ typeof(TimeSpan),
            /* Owner Type:           */ typeof(RedirectedHwndHost),
            /* Metadata:             */ new System.Windows.FrameworkPropertyMetadata(
                /*     Default Value:    */ TimeSpan.FromMilliseconds(30),
                /*     Property Changed: */ (d, e) => ((RedirectedHwndHost) d).OnOutputRedirectionPeriodChanged(e)));

        /// <summary>
        ///     The period of time to update the output redirection.
        /// </summary>
        public TimeSpan OutputRedirectionPeriod {
            get => (TimeSpan) this.GetValue(OutputRedirectionPeriodProperty);
            set => this.SetValue(OutputRedirectionPeriodProperty, value);
        }

        private void OnOutputRedirectionPeriodChanged(System.Windows.DependencyPropertyChangedEventArgs e) {
            _outputRedirectionTimer.Interval = (TimeSpan) e.NewValue;
        }

        #endregion

        #region IsInputRedirectionEnabled

        public static readonly System.Windows.DependencyProperty IsInputRedirectionEnabledProperty = System.Windows.DependencyProperty.Register(
            /* Name:                 */ "IsInputRedirectionEnabled",
            /* Value Type:           */ typeof(bool),
            /* Owner Type:           */ typeof(RedirectedHwndHost),
            /* Metadata:             */ new System.Windows.FrameworkPropertyMetadata(
                /*     Default Value:    */ false,
                /*     Property Changed: */ (d, e) => ((RedirectedHwndHost) d).OnIsInputRedirectionEnabledChanged(e)));

        /// <summary>
        ///     Whether or not input redirection is enabled.
        /// </summary>
        public bool IsInputRedirectionEnabled {
            get => (bool) this.GetValue(IsInputRedirectionEnabledProperty);
            set => this.SetValue(IsInputRedirectionEnabledProperty, value);
        }

        private void OnIsInputRedirectionEnabledChanged(System.Windows.DependencyPropertyChangedEventArgs e) {
            _inputRedirectionTimer.IsEnabled = (bool) e.NewValue;
        }

        #endregion

        #region InputRedirectionPeriod

        public static readonly System.Windows.DependencyProperty InputRedirectionPeriodProperty = System.Windows.DependencyProperty.Register(
            /* Name:                 */ "InputRedirectionPeriod",
            /* Value Type:           */ typeof(TimeSpan),
            /* Owner Type:           */ typeof(RedirectedHwndHost),
            /* Metadata:             */ new System.Windows.FrameworkPropertyMetadata(
                /*     Default Value:    */ TimeSpan.FromMilliseconds(30),
                /*     Property Changed: */ (d, e) => ((RedirectedHwndHost) d).OnInputRedirectionPeriodChanged(e)));

        /// <summary>
        ///     The period of time to update the input redirection.
        /// </summary>
        public TimeSpan InputRedirectionPeriod {
            get => (TimeSpan) this.GetValue(InputRedirectionPeriodProperty);
            set => this.SetValue(InputRedirectionPeriodProperty, value);
        }

        private void OnInputRedirectionPeriodChanged(System.Windows.DependencyPropertyChangedEventArgs e) {
            _inputRedirectionTimer.Interval = (TimeSpan) e.NewValue;
        }

        #endregion

        #region CurrentHwndSource

        public static readonly System.Windows.DependencyPropertyKey CurrentHwndSourcePropertyKey = System.Windows.DependencyProperty.RegisterReadOnly("CurrentHwndSource", typeof(System.Windows.Interop.HwndSource), typeof(RedirectedHwndHost), new System.Windows.PropertyMetadata(null, (d, e) => ((RedirectedHwndHost) d).OnCurrentHwndSourceChanged(e)));

        public static readonly System.Windows.DependencyProperty CurrentHwndSourceProperty = CurrentHwndSourcePropertyKey.DependencyProperty;

        public System.Windows.Interop.HwndSource CurrentHwndSource {
            get => (System.Windows.Interop.HwndSource) this.GetValue(CurrentHwndSourceProperty);
            private set => this.SetValue(CurrentHwndSourcePropertyKey, value);
        }

        #endregion

        #region IDisposable

        public void Dispose() {
            this.Dispose(true);

            // Call SuppressFinalize, even though we don't have a finalizer.
            // This is because a derived type may add a finalizer, and they
            // should not have to reimplement the Dispose pattern.
            GC.SuppressFinalize(this);
        }

        public bool IsDisposed { get; private set; }

        protected virtual void Dispose(bool disposing) {
            this.VerifyNotDisposed();

            if (disposing) {
                _inputRedirectionTimer.IsEnabled = false;
                _inputRedirectionTimer = null;

                _outputRedirectionTimer.IsEnabled = false;
                _outputRedirectionTimer = null;

                if (this.Handle != null) {
                    this.Handle.Dispose();
                    this.Handle = null;
                }

                if (_redirectedWindow != null)
                    _redirectedWindow.Dispose();
            }

            this.IsDisposed = true;
        }

        private void VerifyNotDisposed() {
            if (this.IsDisposed)
                throw new ObjectDisposedException(this.GetType().ToString());
        }

        #endregion

        #region IKeyboardInputSink

        bool System.Windows.Interop.IKeyboardInputSink.HasFocusWithin() {
            return this.HasFocusWithinCore();
        }

        System.Windows.Interop.IKeyboardInputSite System.Windows.Interop.IKeyboardInputSink.KeyboardInputSite { get; set; }

        bool System.Windows.Interop.IKeyboardInputSink.OnMnemonic(ref System.Windows.Interop.MSG msg, System.Windows.Input.ModifierKeys modifiers) {
            return this.OnMnemonicCore(ref msg, modifiers);
        }

        System.Windows.Interop.IKeyboardInputSite System.Windows.Interop.IKeyboardInputSink.RegisterKeyboardInputSink(System.Windows.Interop.IKeyboardInputSink sink) {
            return this.RegisterKeyboardInputSinkCore(sink);
        }

        bool System.Windows.Interop.IKeyboardInputSink.TabInto(System.Windows.Input.TraversalRequest request) {
            return this.TabIntoCore(request);
        }

        bool System.Windows.Interop.IKeyboardInputSink.TranslateAccelerator(ref System.Windows.Interop.MSG msg, System.Windows.Input.ModifierKeys modifiers) {
            return this.TranslateAcceleratorCore(ref msg, modifiers);
        }

        bool System.Windows.Interop.IKeyboardInputSink.TranslateChar(ref System.Windows.Interop.MSG msg, System.Windows.Input.ModifierKeys modifiers) {
            return this.TranslateCharCore(ref msg, modifiers);
        }

        protected virtual bool HasFocusWithinCore() {
            var hwndFocus = Win32.NativeMethods.GetFocus();
            return hwndFocus != null && !hwndFocus.IsInvalid && Win32.NativeMethods.IsChild(this.Handle, hwndFocus);
        }

        protected virtual bool OnMnemonicCore(ref System.Windows.Interop.MSG msg, System.Windows.Input.ModifierKeys modifiers) {
            return false;
        }

        protected virtual System.Windows.Interop.IKeyboardInputSite RegisterKeyboardInputSinkCore(System.Windows.Interop.IKeyboardInputSink sink) {
            throw new InvalidOperationException("RedirectedHwndHost does not support child keyboard sinks be default.");
        }

        protected virtual bool TabIntoCore(System.Windows.Input.TraversalRequest request) {
            return false;
        }

        protected virtual bool TranslateAcceleratorCore(ref System.Windows.Interop.MSG msg, System.Windows.Input.ModifierKeys modifiers) {
            return false;
        }

        protected virtual bool TranslateCharCore(ref System.Windows.Interop.MSG msg, System.Windows.Input.ModifierKeys modifiers) {
            return false;
        }

        #endregion
    }
}