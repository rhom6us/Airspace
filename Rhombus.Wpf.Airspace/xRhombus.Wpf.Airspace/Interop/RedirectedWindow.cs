using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Interop {
    internal class RedirectedWindow : WindowBase {
        private static readonly System.Windows.Media.PixelFormat _format = System.Windows.Media.PixelFormats.Bgr32;

        /// <summary>
        ///     The alpha value for the window.
        /// </summary>
        public byte Alpha {
            get => _alpha;

            set {
                if (value != _alpha) {
                    _alpha = value;

                    this.SetLayeredWindowAttributes(_alpha);
                }
            }
        }

        /// <summary>
        ///     Whether or not the window is hittestable.
        /// </summary>
        public bool IsHitTestable {
            get => _isHitTestable;

            set {
                if (value != _isHitTestable) {
                    _isHitTestable = value;

                    var exStyle = (Win32.User32.WS_EX) Win32.NativeMethods.GetWindowLong(this.Handle, Win32.User32.GWL.EXSTYLE);
                    if (_isHitTestable)
                        exStyle &= ~Win32.User32.WS_EX.TRANSPARENT;
                    else
                        exStyle |= Win32.User32.WS_EX.TRANSPARENT;
                    Win32.NativeMethods.SetWindowLong(this.Handle, Win32.User32.GWL.EXSTYLE, (int) exStyle);
                }
            }
        }

        /// <summary>
        ///     Aligns the RedirectedWindow such that the specified client
        ///     coordinate is aligned with the specified screen coordinate.
        /// </summary>
        public void AlignClientAndScreen(int xClient, int yClient, int xScreen, int yScreen) {
            var pt = new Win32.User32.POINT(xClient, yClient);
            Win32.NativeMethods.ClientToScreen(this.Handle, ref pt);

            var dx = xScreen - pt.x;
            var dy = yScreen - pt.y;

            var rcWindow = new Win32.User32.RECT();
            Win32.NativeMethods.GetWindowRect(this.Handle, ref rcWindow);

            Win32.NativeMethods.SetWindowPos(this.Handle, Win32.User32.HWND.NULL, rcWindow.left + dx, rcWindow.top + dy, 0, 0, Win32.User32.SWP.NOSIZE | Win32.User32.SWP.NOZORDER | Win32.User32.SWP.NOACTIVATE);
        }

        /// <summary>
        ///     Sizes the window such that the client area has the specified
        ///     size.
        /// </summary>
        public bool SetClientAreaSize(int width, int height) {
            var ptClient = new Win32.User32.POINT();
            Win32.NativeMethods.ClientToScreen(this.Handle, ref ptClient);

            var rect = new Win32.User32.RECT {
                left = ptClient.x,
                top = ptClient.y
            };
            rect.right = rect.left + width;
            rect.bottom = rect.top + height;

            var style = (Win32.User32.WS) Win32.NativeMethods.GetWindowLong(this.Handle, Win32.User32.GWL.STYLE);
            var exStyle = (Win32.User32.WS_EX) Win32.NativeMethods.GetWindowLong(this.Handle, Win32.User32.GWL.EXSTYLE);
            Win32.NativeMethods.AdjustWindowRectEx(ref rect, style, false, exStyle);

            Win32.NativeMethods.SetWindowPos(this.Handle, Win32.User32.HWND.NULL, rect.left, rect.top, rect.width, rect.height, Win32.User32.SWP.NOZORDER | Win32.User32.SWP.NOCOPYBITS);

            Win32.NativeMethods.GetClientRect(this.Handle, ref rect);
            return rect.width == width && rect.height == height;
        }

        /// <summary>
        ///     Returns a bitmap of the contents of the window.
        /// </summary>
        /// <remarks>
        ///     RedirectedWindow maintains a bitmap internally and only
        ///     allocates a new bitmap if the dimensions of the window
        ///     have changed.  Even if UpdateRedirectedBitmap returns the same
        ///     bitmap, the contents are guaranteed to have been updated with
        ///     the current contents of the window.
        /// </remarks>
        public System.Windows.Media.Imaging.BitmapSource UpdateRedirectedBitmap() {
            var rcClient = new Win32.User32.RECT();
            Win32.NativeMethods.GetClientRect(this.Handle, ref rcClient);
            if (_interopBitmap == null || rcClient.width != _bitmapWidth || rcClient.height != _bitmapHeight) {
                if (_interopBitmap != null) {
                    if (rcClient.width <= 0 || rcClient.height <= 0)
                        return _interopBitmap;
                    this.DestroyBitmap();
                }

                this.CreateBitmap(rcClient.width, rcClient.height);
            }

            // PrintWindow doesn't seem to work any better than BitBlt.
            // TODO: make it an option
            // User32.NativeMethods.PrintWindow(Handle, _hDC, PW.DEFAULT);

            var hdcSrc = Win32.NativeMethods.GetDC(this.Handle);
            Win32.NativeMethods.BitBlt(_hDC, 0, 0, _bitmapWidth, _bitmapHeight, hdcSrc, 0, 0, Win32.Gdi32.ROP.SRCCOPY);
            Win32.NativeMethods.ReleaseDC(this.Handle, hdcSrc);

            _interopBitmap.Invalidate();

            return _interopBitmap;
        }

        private void CreateBitmap(int width, int height) {
            System.Diagnostics.Debug.Assert(_hSection == IntPtr.Zero);
            System.Diagnostics.Debug.Assert(_hBitmap == IntPtr.Zero);
            System.Diagnostics.Debug.Assert(_interopBitmap == null);

            if (width == 0 || height == 0)
                return;

            _stride = (width * _format.BitsPerPixel + 7) / 8;
            var size = height * _stride;

            _hSection = Win32.NativeMethods.CreateFileMapping(Win32.Kernel32.PAGE.READWRITE, Win32.Kernel32.SEC.NONE, 0, (uint) size, null);


            var bmi = new Win32.Gdi32.BITMAPINFO {
                biWidth = width,
                biHeight = -height,
                biPlanes = 1,
                biBitCount = 32,
                biCompression = Win32.Gdi32.BI.RGB
            };
            // top-down
            bmi.biSize = System.Runtime.InteropServices.Marshal.SizeOf(bmi);

            var hdcScreen = Win32.NativeMethods.GetDC(Win32.User32.HWND.NULL);

            _hBitmap = Win32.NativeMethods.CreateDIBSection(hdcScreen, ref bmi, Win32.Gdi32.DIB.RGB_COLORS, out _pBits, _hSection, 0);

            // TODO: probably don't need a new DC every time...
            _hDC = Win32.NativeMethods.CreateCompatibleDC(hdcScreen);
            Win32.NativeMethods.SelectObject(_hDC, _hBitmap);

            _interopBitmap = System.Windows.Interop.Imaging.CreateBitmapSourceFromMemorySection(_hSection, width, height, _format, _stride, 0) as System.Windows.Interop.InteropBitmap;

            _bitmapWidth = width;
            _bitmapHeight = height;
        }

        private void DestroyBitmap() {
            _interopBitmap = null;

            if (_hDC != IntPtr.Zero) {
                Win32.NativeMethods.DeleteObject(_hDC);
                _hDC = IntPtr.Zero;
            }

            if (_hBitmap != IntPtr.Zero) {
                Win32.NativeMethods.DeleteObject(_hBitmap);
                _hBitmap = IntPtr.Zero;
            }

            _pBits = IntPtr.Zero;

            if (_hSection != IntPtr.Zero) {
                Win32.NativeMethods.CloseHandle(_hSection);
                _hSection = IntPtr.Zero;
            }

            _stride = 0;
            _bitmapWidth = 0;
            _bitmapHeight = 0;
        }

        private byte _alpha;
        private int _bitmapHeight;
        private int _bitmapWidth;
        private IntPtr _hBitmap;
        private IntPtr _hDC;
        private IntPtr _hSection;
        private System.Windows.Interop.InteropBitmap _interopBitmap;
        private bool _isHitTestable;
        private IntPtr _pBits;
        private int _stride;
    }
}