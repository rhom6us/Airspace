﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Utilities {
    /// <summary>
    ///     The Buffer2D class allows a simple memory pointer to be treated
    ///     like a 2D buffer of a managed value type.  The value type must be
    ///     simple (blittable).  The pointer is presumed to be a contiguous
    ///     block of memory organized as rows of the value type.  The block of
    ///     memory can have rows of any size, and the buffer simply references
    ///     a section specified by bounds.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct Buffer2D<T> where T : struct {
        public Buffer2D(IntPtr pBits, int rowWidth, System.Windows.Int32Rect bounds) {
            if (pBits.ToInt64() == 0)
                throw new ArgumentNullException("pBits");
            if (rowWidth <= 0)
                throw new ArgumentOutOfRangeException("rowWidth", "The specified row width must be > 0.");

            if (bounds.X + bounds.Width > rowWidth)
                throw new ArgumentOutOfRangeException("bounds", "The specified bounds requires a larger rowWidth");

            _pBits = pBits;
            _rowWidth = rowWidth;
            _bounds = bounds;
        }

        public Buffer2D(Buffer2D<T> buffer, System.Windows.Int32Rect bounds) {
            if (buffer._pBits.ToInt64() == 0 || buffer._rowWidth <= 0)
                throw new ArgumentException("buffer", "The specified buffer is not initialized.");

            if (bounds.X < buffer._bounds.X || bounds.X >= buffer._bounds.X + buffer._bounds.Width || bounds.X + bounds.Width > buffer._bounds.X + buffer._bounds.Width || bounds.Y < buffer._bounds.Y || bounds.Y >= buffer._bounds.Y + buffer._bounds.Height || bounds.Y + bounds.Height > buffer._bounds.Y + buffer._bounds.Height)
                throw new ArgumentOutOfRangeException("bounds", "The specified bounds must be within the bounds of the buffer.");

            _pBits = buffer._pBits;
            _rowWidth = buffer._rowWidth;
            _bounds = bounds;
        }

        public static implicit operator Buffer2DView<T>(Buffer2D<T> buffer) {
            return new Buffer2DView<T>(buffer);
        }

        public void SetBits(Buffer2DView<T> srcView, System.Windows.Int32Rect srcRect, Numerics.Int32Point? dstPoint = null) {
            if (srcRect.X < 0 || srcRect.Width < 0 || srcRect.X + srcRect.Width > srcView._buffer._bounds.Width || srcRect.Y < 0 || srcRect.Height < 0 || srcRect.Y + srcRect.Height > srcView._buffer._bounds.Height)
                throw new ArgumentOutOfRangeException("srcRect", "The source rect must specify coordinates within the bounds of the source buffer.");

            var theDstPoint = dstPoint ?? new Numerics.Int32Point(srcRect.X, srcRect.Y);
            if (theDstPoint.X < 0 || theDstPoint.X + srcRect.Width > _bounds.Width || theDstPoint.Y < 0 || theDstPoint.Y + srcRect.Height > _bounds.Height)
                throw new ArgumentOutOfRangeException("dstPoint", "The destination point must specify coordinates within the bounds of the buffer.");

            unsafe {
                var sizeOfT = System.Runtime.InteropServices.Marshal.SizeOf(typeof(T));
                var pRead = (byte*) srcView._buffer._pBits.ToPointer() + (srcRect.Y + srcView._buffer._bounds.Y) * srcView._buffer._rowWidth * sizeOfT + (srcRect.X + srcView._buffer._bounds.X) * sizeOfT;
                var pWrite = (byte*) _pBits.ToPointer() + (theDstPoint.Y + _bounds.Y) * _rowWidth * sizeOfT + (theDstPoint.X + _bounds.X) * sizeOfT;

                for (var row = 0; row < srcRect.Height; row++) {
                    for (var col = 0; col < srcRect.Width * sizeOfT; col++) {
                        *(pWrite + col) = *(pRead + col);
                    }

                    pRead += srcView._buffer._rowWidth * sizeOfT;
                    pWrite += _rowWidth * sizeOfT;
                }
            }
        }

        public bool CompareBits(Buffer2DView<T> srcView, System.Windows.Int32Rect srcRect, Numerics.Int32Point? dstPoint = null) {
            if (srcRect.X < 0 || srcRect.Width < 0 || srcRect.X + srcRect.Width > srcView._buffer._bounds.Width || srcRect.Y < 0 || srcRect.Height < 0 || srcRect.Y + srcRect.Height > srcView._buffer._bounds.Height)
                throw new ArgumentOutOfRangeException("srcRect", "The source rect must specify coordinates within the bounds of the source buffer.");

            var theDstPoint = dstPoint ?? new Numerics.Int32Point(srcRect.X, srcRect.Y);
            if (theDstPoint.X < 0 || theDstPoint.X + srcRect.Width > _bounds.Width || theDstPoint.Y < 0 || theDstPoint.Y + srcRect.Height > _bounds.Height)
                throw new ArgumentOutOfRangeException("dstPoint", "The destination point must specify coordinates within the bounds of the buffer.");

            unsafe {
                var sizeOfT = System.Runtime.InteropServices.Marshal.SizeOf(typeof(T));
                var pRead = (byte*) srcView._buffer._pBits.ToPointer() + (srcRect.Y + srcView._buffer._bounds.Y) * srcView._buffer._rowWidth * sizeOfT + (srcRect.X + srcView._buffer._bounds.X) * sizeOfT;
                var pWrite = (byte*) _pBits.ToPointer() + (theDstPoint.Y + _bounds.Y) * _rowWidth * sizeOfT + (theDstPoint.X + _bounds.X) * sizeOfT;

                for (var row = 0; row < srcRect.Height; row++) {
                    for (var col = 0; col < srcRect.Width * sizeOfT; col++) {
                        if (*(pWrite + col) != *(pRead + col))
                            return false;
                    }

                    pRead += srcView._buffer._rowWidth * sizeOfT;
                    pWrite += _rowWidth * sizeOfT;
                }
            }

            return true;
        }

        public void XorBits(Buffer2DView<T> srcView, System.Windows.Int32Rect srcRect, Numerics.Int32Point? dstPoint = null) {
            if (srcRect.X < 0 || srcRect.Width < 0 || srcRect.X + srcRect.Width > srcView._buffer._bounds.Width || srcRect.Y < 0 || srcRect.Height < 0 || srcRect.Y + srcRect.Height > srcView._buffer._bounds.Height)
                throw new ArgumentOutOfRangeException("srcRect", "The source rect must specify coordinates within the bounds of the source buffer.");

            var theDstPoint = dstPoint ?? new Numerics.Int32Point(srcRect.X, srcRect.Y);
            if (theDstPoint.X < 0 || theDstPoint.X + srcRect.Width > _bounds.Width || theDstPoint.Y < 0 || theDstPoint.Y + srcRect.Height > _bounds.Height)
                throw new ArgumentOutOfRangeException("dstPoint", "The destination point must specify coordinates within the bounds of the buffer.");

            unsafe {
                var sizeOfT = System.Runtime.InteropServices.Marshal.SizeOf(typeof(T));
                var pRead = (byte*) srcView._buffer._pBits.ToPointer() + (srcRect.Y + srcView._buffer._bounds.Y) * srcView._buffer._rowWidth * sizeOfT + (srcRect.X + srcView._buffer._bounds.X) * sizeOfT;
                var pWrite = (byte*) _pBits.ToPointer() + (theDstPoint.Y + _bounds.Y) * _rowWidth * sizeOfT + (theDstPoint.X + _bounds.X) * sizeOfT;

                for (var row = 0; row < srcRect.Height; row++) {
                    for (var col = 0; col < srcRect.Width * sizeOfT; col++) {
                        *(pWrite + col) ^= *(pRead + col);
                    }

                    pRead += srcView._buffer._rowWidth * sizeOfT;
                    pWrite += _rowWidth * sizeOfT;
                }
            }
        }

        public T this[int x, int y] {
            get {
                if (x < 0 || x > _bounds.Width)
                    throw new ArgumentOutOfRangeException("x", "The indexer must specify coordinates within the bounds of the buffer.");

                if (y < 0 || y > _bounds.Height)
                    throw new ArgumentOutOfRangeException("y", "The indexer must specify coordinates within the bounds of the buffer.");

                unsafe {
                    var sizeOfT = System.Runtime.InteropServices.Marshal.SizeOf(typeof(T));
                    var pRead = (byte*) _pBits.ToPointer() + (y + _bounds.Y) * _rowWidth * sizeOfT + (x + _bounds.X) * sizeOfT;
                    return (T) System.Runtime.InteropServices.Marshal.PtrToStructure(new IntPtr(pRead), typeof(T));
                }
            }

            set {
                if (x < 0 || x > _bounds.Width)
                    throw new ArgumentOutOfRangeException("x", "The indexer must specify coordinates within the bounds of the buffer.");

                if (y < 0 || y > _bounds.Height)
                    throw new ArgumentOutOfRangeException("y", "The indexer must specify coordinates within the bounds of the buffer.");

                unsafe {
                    var sizeOfT = System.Runtime.InteropServices.Marshal.SizeOf(typeof(T));
                    var pWrite = (byte*) _pBits.ToPointer() + (y + _bounds.Y) * _rowWidth * sizeOfT + (x + _bounds.X) * sizeOfT;
                    System.Runtime.InteropServices.Marshal.StructureToPtr(value, new IntPtr(pWrite), false);
                }
            }
        }

        public int Width => _bounds.Width;

        public int Height => _bounds.Height;

        public System.Windows.Media.Imaging.BitmapSource CreateBitmapSource(double dpiX, double dpiY, System.Windows.Media.PixelFormat pixelFormat, System.Windows.Media.Imaging.BitmapPalette bitmapPalette) {
            var sizeOfT = System.Runtime.InteropServices.Marshal.SizeOf(typeof(T));
            var stride = _rowWidth * sizeOfT;
            var buffer = _pBits + _bounds.Y * _rowWidth * sizeOfT + _bounds.X * sizeOfT;
            var bufferSize = _rowWidth * sizeOfT * _bounds.Height - _bounds.X;

            return System.Windows.Media.Imaging.BitmapSource.Create(_bounds.Width, _bounds.Height, dpiX, dpiY, pixelFormat, bitmapPalette, buffer, bufferSize, stride);
        }

        private readonly IntPtr _pBits;
        private readonly int _rowWidth;
        private readonly System.Windows.Int32Rect _bounds;
    }
}