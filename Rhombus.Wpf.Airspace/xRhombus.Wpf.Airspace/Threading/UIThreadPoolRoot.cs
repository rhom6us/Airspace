using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Threading {
    /// <summary>
    ///     The root element of a tree created on the UIThreadPool.
    /// </summary>
    [System.Windows.Markup.ContentProperty("Content")]
    public class UIThreadPoolRoot : Controls.VisualWrapper<System.Windows.Media.HostVisual>, IDisposable {
        public static readonly System.Windows.DependencyProperty ContentProperty = System.Windows.DependencyProperty.Register("Content", typeof(System.Windows.DataTemplate), typeof(UIThreadPoolRoot), new System.Windows.UIPropertyMetadata(null, UIThreadPoolRoot.OnContentChangedThunk));

        public static readonly System.Windows.DependencyProperty PropertyNameProperty = System.Windows.DependencyProperty.Register("PropertyName", typeof(string), typeof(UIThreadPoolRoot), new System.Windows.UIPropertyMetadata(null, UIThreadPoolRoot.OnPropertyNameChangedThunk));

        static UIThreadPoolRoot() {
            DataContextProperty.OverrideMetadata(typeof(UIThreadPoolRoot), new System.Windows.FrameworkPropertyMetadata(UIThreadPoolRoot.OnDataContextChangedThunk));
        }

        public UIThreadPoolRoot() {
            // The UIThreadPoolRoot instance itself is owned by the calling
            // thread, which is typically the UI thread.  It uses a HostVisual
            // as its only child to host the results of inflating the template
            // on a thread pool thread.
            var child = new System.Windows.Media.HostVisual();
            this.Child = child;

            _threadPoolThread = UIThreadPool.AcquireThread();

            _threadPoolThread.Dispatcher.Invoke(
                delegate {
                    _root = new VisualTargetPresentationSource(child);
                    _root.SizeChanged += this.VisualTargetSizeChanged;
                });
        }

        public System.Windows.DataTemplate Content {
            get => (System.Windows.DataTemplate) this.GetValue(ContentProperty);
            set => this.SetValue(ContentProperty, value);
        }

        // HACK!  Called on UI thread
        public string PropertyName {
            get => (string) this.GetValue(PropertyNameProperty);
            set => this.SetValue(PropertyNameProperty, value);
        }

        // Called by UI thread
        private void OnContentChanged(System.Windows.DependencyPropertyChangedEventArgs e) {
            // Seal the data template, so that we can expand it on the worker thread.
            var dataTemplate = e.NewValue as System.Windows.DataTemplate;

            // Asynchronously pass to worker thread.
            _root.Dispatcher.BeginInvoke(
                (Action) delegate {
                    if (dataTemplate != null) {
                        var rootElement = dataTemplate.LoadContent() as System.Windows.FrameworkElement;
                        _root.RootVisual = rootElement;
                        this.VisualTargetSizeChanged(rootElement.RenderSize);
                    }
                    else {
                        _root.RootVisual = null;
                        this.VisualTargetSizeChanged(System.Windows.Size.Empty);
                    }
                });
        }

        // Called by UI thread
        private void OnDataContextChanged(System.Windows.DependencyPropertyChangedEventArgs e) {
            var dataContext = e.NewValue;

            // Asynchronously pass to worker thread.
            _root.Dispatcher.BeginInvoke((Action) delegate { _root.DataContext = dataContext; });
        }

        // Called by UI thread
        private void OnPropertyNameChanged(System.Windows.DependencyPropertyChangedEventArgs e) {
            var propertyName = e.NewValue as string;

            // Asynchronously pass to worker thread.
            _root.Dispatcher.BeginInvoke(
                (Action) delegate {
                    // HACK
                    _root.PropertyName = propertyName;
                });
        }

        private static void OnContentChangedThunk(object sender, System.Windows.DependencyPropertyChangedEventArgs e) {
            var _this = sender as UIThreadPoolRoot;
            _this.OnContentChanged(e);
        }

        private static void OnDataContextChangedThunk(object sender, System.Windows.DependencyPropertyChangedEventArgs e) {
            var _this = sender as UIThreadPoolRoot;
            _this.OnDataContextChanged(e);
        }

        private static void OnPropertyNameChangedThunk(object sender, System.Windows.DependencyPropertyChangedEventArgs e) {
            var _this = sender as UIThreadPoolRoot;
            _this.OnPropertyNameChanged(e);
        }

        // Called by worker thread.
        private void VisualTargetSizeChanged(object sender, System.Windows.SizeChangedEventArgs e) {
            this.VisualTargetSizeChanged(e.NewSize);
        }

        // Called by worker thread.
        private void VisualTargetSizeChanged(System.Windows.Size newSize) {
            // Asynchronously pass new size over to UI thread.
            this.Dispatcher.BeginInvoke(
                (System.Windows.Threading.DispatcherOperationCallback) delegate(object parameter) {
                    this.UpdateSize((System.Windows.Size) parameter);
                    return null;
                }, newSize);
        }

        // Called by UI thread.
        private void UpdateSize(System.Windows.Size newSize) {
            this.Width = newSize.Width;
            this.Height = newSize.Height;
        }

        public void Dispose() { }

        private VisualTargetPresentationSource _root; // only touch from thread pool thread!

        private readonly UIThreadPoolThread _threadPoolThread;
    }
}