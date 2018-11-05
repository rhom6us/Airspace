using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Extensions {
    public static class HwndSourceExtensions {
        public static System.Windows.Size GetClientSize(this System.Windows.Interop.HwndSource hwndSource) {
            var hwnd = new Win32.User32.HWND(hwndSource.Handle);

            var rcClient = new Win32.User32.RECT();
            Win32.NativeMethods.GetClientRect(hwnd, ref rcClient);

            // Client rect should always have (0,0) as the upper-left corner.
            System.Diagnostics.Debug.Assert(rcClient.left == 0);
            System.Diagnostics.Debug.Assert(rcClient.left == 0);

            // Convert from pixels into DIPs.
            var size = new System.Windows.Vector(rcClient.right, rcClient.bottom);
            size = hwndSource.CompositionTarget.TransformFromDevice.Transform(size);

            return new System.Windows.Size(size.X, size.Y);
        }

        /// <summary>
        /// </summary>
        /// <param name="hwndSource"></param>
        /// <returns></returns>
        public static System.Windows.Rect GetWindowRect(this System.Windows.Interop.HwndSource hwndSource) {
            var hwnd = new Win32.User32.HWND(hwndSource.Handle);

            var rcWindow = new Win32.User32.RECT();
            Win32.NativeMethods.GetWindowRect(hwnd, ref rcWindow);

            // Transform from pixels into DIPs.
            var position = new System.Windows.Point(rcWindow.left, rcWindow.top);
            var size = new System.Windows.Vector(rcWindow.right - rcWindow.left, rcWindow.bottom - rcWindow.top);
            position = hwndSource.CompositionTarget.TransformFromDevice.Transform(position);
            size = hwndSource.CompositionTarget.TransformFromDevice.Transform(size);

            return new System.Windows.Rect(position, size);
        }

        /// <summary>
        ///     Transform a point from "screen" coordinate space into the
        ///     "client" coordinate space of the window.
        /// </summary>
        public static System.Windows.Point TransformScreenToClient(this System.Windows.Interop.HwndSource hwndSource, System.Windows.Point point) {
            var hwnd = new Win32.User32.HWND(hwndSource.Handle);

            var pt = new Win32.User32.POINT();
            pt.x = (int) point.X;
            pt.y = (int) point.Y;

            Win32.NativeMethods.ScreenToClient(hwnd, ref pt);

            return new System.Windows.Point(pt.x, pt.y);
        }

        /// <summary>
        ///     Transform a rectangle from "screen" coordinate space into the
        ///     "client" coordinate space of the window.
        /// </summary>
        public static System.Windows.Rect TransformScreenToClient(this System.Windows.Interop.HwndSource hwndSource, System.Windows.Rect rect) {
            var hwnd = new Win32.User32.HWND(hwndSource.Handle);

            var ptUpperLeft = new Win32.User32.POINT();
            ptUpperLeft.x = (int) rect.Left;
            ptUpperLeft.y = (int) rect.Top;

            Win32.NativeMethods.ScreenToClient(hwnd, ref ptUpperLeft);

            var ptLowerRight = new Win32.User32.POINT();
            ptLowerRight.x = (int) rect.Right;
            ptLowerRight.y = (int) rect.Bottom;

            Win32.NativeMethods.ScreenToClient(hwnd, ref ptLowerRight);

            return new System.Windows.Rect(ptUpperLeft.x, ptUpperLeft.y, ptLowerRight.x - ptUpperLeft.x, ptLowerRight.y - ptUpperLeft.y);
        }

        /// <summary>
        ///     Transform a point from "client" coordinate space of a window
        ///     into the "screen" coordinate space.
        /// </summary>
        public static System.Windows.Point TransformClientToScreen(this System.Windows.Interop.HwndSource hwndSource, System.Windows.Point point) {
            var hwnd = new Win32.User32.HWND(hwndSource.Handle);

            var pt = new Win32.User32.POINT();
            pt.x = (int) point.X;
            pt.y = (int) point.Y;

            Win32.NativeMethods.ClientToScreen(hwnd, ref pt);

            return new System.Windows.Point(pt.x, pt.y);
        }

        /// <summary>
        ///     Transform a rectangle from the "client" coordinate space of the
        ///     window into the "screen" coordinate space.
        /// </summary>
        public static System.Windows.Rect TransformClientToScreen(this System.Windows.Interop.HwndSource hwndSource, System.Windows.Rect rect) {
            var hwnd = new Win32.User32.HWND(hwndSource.Handle);

            var ptUpperLeft = new Win32.User32.POINT();
            ptUpperLeft.x = (int) rect.Left;
            ptUpperLeft.y = (int) rect.Top;

            Win32.NativeMethods.ClientToScreen(hwnd, ref ptUpperLeft);

            var ptLowerRight = new Win32.User32.POINT();
            ptLowerRight.x = (int) rect.Right;
            ptLowerRight.y = (int) rect.Bottom;

            Win32.NativeMethods.ClientToScreen(hwnd, ref ptLowerRight);

            return new System.Windows.Rect(ptUpperLeft.x, ptUpperLeft.y, ptLowerRight.x - ptUpperLeft.x, ptLowerRight.y - ptUpperLeft.y);
        }
    }
}