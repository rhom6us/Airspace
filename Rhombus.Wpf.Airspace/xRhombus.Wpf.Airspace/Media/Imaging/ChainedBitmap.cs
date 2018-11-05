using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Media.Imaging {
    /// <summary>
    ///     The ChainedBitmap class is the base class for custom bitmaps that
    ///     processes the content of another source.
    /// </summary>
    /// <remarks>
    ///     The default implementation of the BitmapSource virtuals is to
    ///     delegate to the source.  This makes sense for most properties like
    ///     DpiX, DpiY, PixelWidth, PixelHeight, etc, as in many scenarios
    ///     these properties are the same for the entire chain of bitmap
    ///     sources.  However, derived classes should pay special attention to
    ///     the Format property.  Many bitmap processors only support a limited
    ///     number of pixel formats, and they should return this for the
    ///     Format property.  ChainedBitmap will take care of converting the
    ///     pixel format as needed in the base implementation of CopyPixels.
    /// </remarks>
    public class ChainedBitmap : CustomBitmap {
        protected override void Dispose(bool disposing) {
            if (disposing) {
                var disposable = this.Source as IDisposable;
                if (disposable != null)
                    disposable.Dispose();
                this.Source = null;
            }

            base.Dispose(disposing);
        }

        #region BitmapSource Decode

        private void OnSourceDecodeFailed(object sender, System.Windows.Media.ExceptionEventArgs e) {
            this.RaiseDecodeFailed(e);
        }

        #endregion BitmapSource Decode

        #region Freezable

        /// <summary>
        ///     Creates an instance of the ChainedBitmap class.
        /// </summary>
        /// <returns>
        ///     The new instance.  If you derive from this class, you must
        ///     override this method to return your own type.
        /// </returns>
        protected override System.Windows.Freezable CreateInstanceCore() {
            return new ChainedBitmap();
        }

        /// <summary>
        ///     Transitions this instance into a thread-safe, read-only mode.
        /// </summary>
        /// <param name="isChecking">
        ///     Whether or not the transition should really happen, or just to
        ///     determine if the transition is possible.
        /// </param>
        /// <remarks>
        ///     Override this method if you have additional non-DP state that
        ///     should be frozen along with the instance.
        /// </remarks>
        protected override bool FreezeCore(bool isChecking) {
            if (_formatConverter != null) {
                if (isChecking)
                    return _formatConverter.CanFreeze;
                _formatConverter.Freeze();
            }

            return true;
        }

        /// <summary>
        ///     Copies data into a cloned instance.
        /// </summary>
        /// <param name="original">
        ///     The original instance to copy data from.
        /// </param>
        /// <param name="useCurrentValue">
        ///     Whether or not to copy the current value of expressions, or the
        ///     expressions themselves.
        /// </param>
        /// <param name="willBeFrozen">
        ///     Indicates whether or not the clone will be frozen.  If the
        ///     clone will be immediately frozen, there is no need to clone
        ///     data that is already frozen, you can just share the instance.
        /// </param>
        /// <remarks>
        ///     Override this method if you have additional non-DP state that
        ///     should be transfered to clones.
        /// </remarks>
        protected override void CopyCore(CustomBitmap original, bool useCurrentValue, bool willBeFrozen) {
            var originalChainedBitmap = (ChainedBitmap) original;
            if (originalChainedBitmap._formatConverter != null) {
                if (useCurrentValue) {
                    if (willBeFrozen)
                        _formatConverter = (System.Windows.Media.Imaging.FormatConvertedBitmap) originalChainedBitmap._formatConverter.GetCurrentValueAsFrozen();
                    else
                        _formatConverter = originalChainedBitmap._formatConverter.CloneCurrentValue();
                }
                else {
                    if (willBeFrozen)
                        _formatConverter = (System.Windows.Media.Imaging.FormatConvertedBitmap) originalChainedBitmap._formatConverter.GetAsFrozen();
                    else
                        _formatConverter = originalChainedBitmap._formatConverter.Clone();
                }
            }
        }

        #endregion Freezable

        #region Source

        /// <summary>
        ///     The DependencyProperty for the Source property.
        /// </summary>
        public static readonly System.Windows.DependencyProperty SourceProperty = System.Windows.DependencyProperty.Register("Source", typeof(System.Windows.Media.Imaging.BitmapSource), typeof(ChainedBitmap), new System.Windows.FrameworkPropertyMetadata(ChainedBitmap.OnSourcePropertyChanged));

        /// <summary>
        ///     The BitmapSource to chain.
        /// </summary>
        public System.Windows.Media.Imaging.BitmapSource Source {
            get => (System.Windows.Media.Imaging.BitmapSource) this.GetValue(SourceProperty);

            set => this.SetValue(SourceProperty, value);
        }

        protected virtual void OnSourcePropertyChanged(System.Windows.DependencyPropertyChangedEventArgs e) {
            // Stop listening for the download and decode events on the old source.
            var oldValue = e.OldValue as System.Windows.Media.Imaging.BitmapSource;
            if (oldValue != null && !oldValue.IsFrozen) {
                oldValue.DownloadCompleted -= this.OnSourceDownloadCompleted;
                oldValue.DownloadProgress -= this.OnSourceDownloadProgress;
                oldValue.DownloadFailed -= this.OnSourceDownloadFailed;
                oldValue.DecodeFailed -= this.OnSourceDecodeFailed;
            }

            // Start listening for the download and decode events on the new source.
            var newValue = e.NewValue as System.Windows.Media.Imaging.BitmapSource;
            if (newValue != null && !newValue.IsFrozen) {
                newValue.DownloadCompleted += this.OnSourceDownloadCompleted;
                newValue.DownloadProgress += this.OnSourceDownloadProgress;
                newValue.DownloadFailed += this.OnSourceDownloadFailed;
                newValue.DecodeFailed += this.OnSourceDecodeFailed;
            }
        }

        private static void OnSourcePropertyChanged(System.Windows.DependencyObject d, System.Windows.DependencyPropertyChangedEventArgs e) {
            var chainedBitmap = (ChainedBitmap) d;
            chainedBitmap.OnSourcePropertyChanged(e);
        }

        #endregion Source

        #region BitmapSource Properties

        /// <summary>
        ///     Horizontal DPI of the bitmap.
        /// </summary>
        /// <remarks>
        ///     Derived classes can override this to specify their own value.
        ///     This implementation simply delegates to the source, if present.
        /// </remarks>
        public override double DpiX {
            get {
                var source = this.Source;
                if (source != null)
                    return source.DpiX;
                return base.DpiX;
            }
        }

        /// <summary>
        ///     Vertical DPI of the bitmap.
        /// </summary>
        /// <remarks>
        ///     Derived classes can override this to specify their own value.
        ///     This implementation simply delegates to the source, if present.
        /// </remarks>
        public override double DpiY {
            get {
                var source = this.Source;
                if (source != null)
                    return source.DpiY;
                return base.DpiY;
            }
        }

        /// <summary>
        ///     Pixel format of the bitmap.
        /// </summary>
        /// <remarks>
        ///     Derived classes can override this to specify their own value.
        ///     This implementation simply delegates to the source, if present.
        /// </remarks>
        public override System.Windows.Media.PixelFormat Format {
            get {
                var source = this.Source;
                if (source != null)
                    return source.Format;
                return base.Format;
            }
        }

        /// <summary>
        ///     Width of the bitmap contents.
        /// </summary>
        /// <remarks>
        ///     Derived classes can override this to specify their own value.
        ///     This implementation simply delegates to the source, if present.
        /// </remarks>
        public override int PixelWidth {
            get {
                var source = this.Source;
                if (source != null)
                    return source.PixelWidth;
                return base.PixelWidth;
            }
        }

        /// <summary>
        ///     Height of the bitmap contents.
        /// </summary>
        /// <remarks>
        ///     Derived classes can override this to specify their own value.
        ///     This implementation simply delegates to the source, if present.
        /// </remarks>
        public override int PixelHeight {
            get {
                var source = this.Source;
                if (source != null)
                    return source.PixelHeight;
                return base.PixelHeight;
            }
        }

        /// <summary>
        ///     Palette of the bitmap.
        /// </summary>
        /// <remarks>
        ///     Derived classes can override this to specify their own value.
        ///     This implementation simply delegates to the source, if present.
        /// </remarks>
        public override System.Windows.Media.Imaging.BitmapPalette Palette {
            get {
                var source = this.Source;
                if (source != null)
                    return source.Palette;
                return base.Palette;
            }
        }

        #endregion BitmapSource Properties

        #region BitmapSource Download

        /// <summary>
        ///     Whether or not the BitmapSource is downloading content.
        /// </summary>
        /// <remarks>
        ///     Derived classes can override this to specify their own value.
        ///     This implementation simply delegates to the source, if present.
        /// </remarks>
        public override bool IsDownloading {
            get {
                var source = this.Source;
                if (source != null)
                    return source.IsDownloading;
                return false;
            }
        }

        private void OnSourceDownloadCompleted(object sender, EventArgs e) {
            this.RaiseDownloadCompleted();
        }

        private void OnSourceDownloadProgress(object sender, System.Windows.Media.Imaging.DownloadProgressEventArgs e) {
            this.RaiseDownloadProgress(e);
        }

        private void OnSourceDownloadFailed(object sender, System.Windows.Media.ExceptionEventArgs e) {
            this.RaiseDownloadFailed(e);
        }

        #endregion BitmapSource Download

        #region BitmapSource CopyPixels

        /// <summary>
        ///     Requests pixels from the ChainedCustomBitmapSource.
        /// </summary>
        /// <param name="sourceRect">
        ///     The rectangle of pixels being requested.
        /// </param>
        /// <param name="stride">
        ///     The stride of the destination buffer.
        /// </param>
        /// <param name="bufferSize">
        ///     The size of the destination buffer.
        /// </param>
        /// <param name="buffer">
        ///     The destination buffer.
        /// </param>
        /// <remarks>
        ///     This implementation simply delegates to the source, if present.
        /// </remarks>
        protected override void CopyPixelsCore(System.Windows.Int32Rect sourceRect, int stride, int bufferSize, IntPtr buffer) {
            var source = this.Source;
            var convertedSource = source;

            if (source != null) {
                var sourceFormat = source.Format;
                var destinationFormat = this.Format;

                if (sourceFormat != destinationFormat) {
                    // We need a format converter.  Reuse the cached one if
                    // it still matches.
                    if (_formatConverter == null || _formatConverter.Source != source || _formatConverter.Format != destinationFormat || _formatConverterSourceFormat != sourceFormat) {
                        this.WritePreamble();
                        _formatConverterSourceFormat = sourceFormat;
                        _formatConverter = new System.Windows.Media.Imaging.FormatConvertedBitmap(source, destinationFormat, this.Palette, 0);
                        this.WritePostscript();
                    }

                    convertedSource = _formatConverter;
                }

                convertedSource.CopyPixels(sourceRect, buffer, bufferSize, stride);
            }
        }

        private System.Windows.Media.PixelFormat _formatConverterSourceFormat;
        private System.Windows.Media.Imaging.FormatConvertedBitmap _formatConverter;

        #endregion BitmapSource CopyPixels
    }
}