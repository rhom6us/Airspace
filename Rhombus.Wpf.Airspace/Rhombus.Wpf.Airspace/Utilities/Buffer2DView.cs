using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Utilities {
    public struct Buffer2DView<T> where T : struct {
        public Buffer2DView(Buffer2D<T> buffer) {
            _buffer = buffer;
        }

        public Buffer2DView(Buffer2D<T> buffer, System.Windows.Int32Rect bounds) {
            _buffer = new Buffer2D<T>(buffer, bounds);
        }

        public Buffer2DView(Buffer2DView<T> buffer, System.Windows.Int32Rect bounds) {
            _buffer = new Buffer2D<T>(buffer._buffer, bounds);
        }

        public bool CompareBits(Buffer2D<T> srcBuffer, System.Windows.Int32Rect srcRect, Numerics.Int32Point? dstPoint = null) {
            return _buffer.CompareBits(srcBuffer, srcRect, dstPoint);
        }

        public T this[int x, int y] => _buffer[x, y];
        public int Width => _buffer.Width;
        public int Height => _buffer.Height;

        public System.Windows.Media.Imaging.BitmapSource CreateBitmapSource(double dpiX, double dpiY, System.Windows.Media.PixelFormat pixelFormat, System.Windows.Media.Imaging.BitmapPalette bitmapPalette) {
            return _buffer.CreateBitmapSource(dpiX, dpiY, pixelFormat, bitmapPalette);
        }

        internal Buffer2D<T> _buffer;
    }
}