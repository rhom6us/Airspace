using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Win32 {
    /// <summary>
    ///     This class contains the PInvoke signatures for functions in
    ///     ComCtl32.dll.
    /// </summary>
    public static class NativeMethods {
        [System.Runtime.InteropServices.DllImport("comctl32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        public static extern bool GetWindowSubclass(User32.HWND hWnd, ComCtl32.SUBCLASSPROC pfnSubclass, IntPtr uIdSubclass, ref IntPtr pdwRefData);

        [System.Runtime.InteropServices.DllImport("comctl32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        public static extern bool SetWindowSubclass(User32.HWND hwnd, ComCtl32.SUBCLASSPROC callback, IntPtr id, IntPtr data);

        [System.Runtime.InteropServices.DllImport("comctl32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        public static extern bool RemoveWindowSubclass(User32.HWND hwnd, ComCtl32.SUBCLASSPROC callback, IntPtr id);

        [System.Runtime.InteropServices.DllImport("comctl32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        public static extern IntPtr DefSubclassProc(User32.HWND hwnd, User32.WM msg, IntPtr wParam, IntPtr lParam);

        [System.Runtime.InteropServices.DllImport("dwmapi.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, PreserveSig = true)]
        [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Error)]
        public static extern int DwmIsCompositionEnabled(out bool pfEnabled);

        [System.Runtime.InteropServices.DllImport("dwmapi.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, PreserveSig = true)]
        [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Error)]
        public static extern int DwmEnableComposition(DwmApi.DWM_EC uCompositionAction);

        [System.Runtime.InteropServices.DllImport("gdi32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        public static extern bool GdiFlush();

        [System.Runtime.InteropServices.DllImport("gdi32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        public static extern bool DeleteObject(IntPtr hObject);

        // TODO: HDC
        [System.Runtime.InteropServices.DllImport("gdi32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        public static extern IntPtr CreateCompatibleDC(IntPtr hdc);

        [System.Runtime.InteropServices.DllImport("gdi32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        public static extern IntPtr CreateCompatibleBitmap(IntPtr hdc, int width, int height);

        [System.Runtime.InteropServices.DllImport("gdi32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

        [System.Runtime.InteropServices.DllImport("gdi32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        public static extern IntPtr CreateDIBSection(IntPtr hdc, [System.Runtime.InteropServices.In] ref Gdi32.BITMAPINFO pbmi, Gdi32.DIB iUsage, out IntPtr ppvBits, IntPtr hSection, uint dwOffset);

        [System.Runtime.InteropServices.DllImport("gdi32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetStockObject(int stockObject);

        // TODO: HDC
        [System.Runtime.InteropServices.DllImport("gdi32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        public static extern bool BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, Gdi32.ROP dwRop);

        [System.Runtime.InteropServices.DllImport("gdi32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        public static extern bool SetBkColor(IntPtr hdcDest, Gdi32.COLORREF crColor);

        [System.Runtime.InteropServices.DllImport("gdi32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        public static extern bool SetTextColor(IntPtr hdcDest, Gdi32.COLORREF crColor);

        [System.Runtime.InteropServices.DllImport("gdi32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        public static extern int SetBkMode(IntPtr hdc, Gdi32.BKMODE iBkMode);

        /// <summary>
        ///     The GetDeviceCaps function retrieves device-specific information
        ///     for the specified device.
        /// </summary>
        /// <param name="hdc">
        ///     A handle to the DC.
        /// </param>
        /// <param name="nIndex">
        ///     The item to be returned.
        /// </param>
        /// <returns></returns>
        [System.Runtime.InteropServices.DllImport("gdi32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        public static extern int GetDeviceCaps(IntPtr hdc, Gdi32.GDC nIndex);

        /// <summary>
        ///     The CreateFont function creates a logical font with the specified
        ///     characteristics. The logical font can subsequently be selected as
        ///     the font for any device.
        /// </summary>
        [System.Runtime.InteropServices.DllImport("gdi32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        public static extern IntPtr CreateFont(int nHeight, int nWidth, int nEscapement, int nOrientation, Gdi32.FW fnWeight, [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
            bool fdwItalic, [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
            bool fdwUnderline, [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
            bool fdwStrikeOut, Gdi32.CHARSET fdwCharSet, Gdi32.OUTPRECIS fdwOutputPrecision, Gdi32.CLIP fdwClipPrecision, Gdi32.QUALITY fdwQuality, Gdi32.PITCH_FF fdwPitchAndFamily, string lpszFace);

        [System.Runtime.InteropServices.DllImport("gdi32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
        public static extern bool DeleteDC(Gdi32.HDC hdc);

        [System.Runtime.InteropServices.DllImport("kernel32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        public static extern IntPtr GetModuleHandle(string modName);

        [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool QueryProcessCycleTime(IntPtr processHandle, ref long processCycleTime);

        [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool QueryIdleProcessorCycleTime(ref int bufferLength, long[] processorIdleCycleTime);

        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        public static extern void GetSystemInfo([System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Struct)]
            ref Kernel32.SYSTEM_INFO lpSystemInfo);

        [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
        public static extern int GetCurrentProcessId();

        [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
        public static extern int GetCurrentThreadId();

        // TODO: HANDLE
        // TODO: SECURITY_ATTRIBUTES
        [System.Runtime.InteropServices.DllImport("kernel32.dll", EntryPoint = "CreateFileMapping", SetLastError = true)]
        private static extern IntPtr _CreateFileMapping(IntPtr hFile, IntPtr lpFileMappingAttributes, int flProtect, uint dwMaximumSizeHigh, uint dwMaximumSizeLow, string lpName);

        public static IntPtr CreateFileMapping(Kernel32.PAGE pageProtect, Kernel32.SEC secProtect, uint dwMaximumSizeHigh, uint dwMaximumSizeLow, string lpName) {
            // This is a messy function to wrap.
            // GetLastError returns ERROR_ALREADY_EXISTS if the mapping already exists, and the handle is returned.
            // not all PAGE and SEC combos are legit

            var INVALID_HANDLE_VALUE = new IntPtr(-1); // SafeFileHandle.Invalid?
            return NativeMethods._CreateFileMapping(INVALID_HANDLE_VALUE, IntPtr.Zero, (int) pageProtect | (int) secProtect, dwMaximumSizeHigh, dwMaximumSizeLow, lpName);
        }

        [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr MapViewOfFile(IntPtr hFileMappingObject, uint dwDesiredAccess, uint dwFileOffsetHigh, uint dwFileOffsetLow, uint dwNumberOfBytesToMap);

        [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool UnmapViewOfFile(IntPtr lpBaseAddress);

        // TODO: HANDLE
        [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool CloseHandle(IntPtr hObject);

        /// <summary>
        ///     Multiplies two 32-bit values and then divides the 64-bit result by
        ///     a third 32-bit value. The final result is rounded to the nearest
        ///     integer.
        /// </summary>
        /// <param name="nNumber">
        ///     The multiplicand.
        /// </param>
        /// <param name="nNumerator">
        ///     The multiplier.
        /// </param>
        /// <param name="nDenominator">
        ///     The number by which the result of the multiplication operation is
        ///     to be divided.
        /// </param>
        /// <returns>
        ///     If the function succeeds, the return value is the result of the
        ///     multiplication and division, rounded to the nearest integer. If
        ///     the result is a positive half integer (ends in .5), it is rounded
        ///     up. If the result is a negative half integer, it is rounded down.
        ///     If either an overflow occurred or nDenominator was 0, the return
        ///     value is -1.
        /// </returns>
        [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
        public static extern int MulDiv(int nNumber, int nNumerator, int nDenominator);

        [System.Runtime.InteropServices.DllImport("urlmon.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, PreserveSig = true)]
        [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Error)]
        public static extern int CoInternetSetFeatureEnabled(UrlMon.INTERNETFEATURELIST featureEntry, UrlMon.SET_FEATURE flags, bool enable);

        #region Mouse Input Functions

        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        public static extern User32.HWND GetCapture();

        #endregion

        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        public static extern bool PrintWindow(User32.HWND hwnd, IntPtr hdcBlt, User32.PW nFlags);

        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        public static extern int SendInput(int nInput, [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPArray)]
            User32.INPUT[] pInputs, int cbSize);

        #region Window Class Functions

        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        public static extern bool GetClassInfoEx(IntPtr hinst, string lpszClass, ref User32.WNDCLASSEX lpwcx);

        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        public static extern int GetClassLong(User32.HWND hwnd, User32.GCL nIndex);

        public static IntPtr GetClassLongPtr(User32.HWND hwnd, User32.GCL nIndex) {
            if (IntPtr.Size == 4)
                return new IntPtr(NativeMethods.GetClassLong(hwnd, nIndex));
            return NativeMethods._GetClassLongPtr(hwnd, nIndex);
        }

        [System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint = "GetClassLongPtr", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        private static extern IntPtr _GetClassLongPtr(User32.HWND hwnd, User32.GCL nIndex);

        public static int GetClassName(User32.HWND hwnd, System.Text.StringBuilder lpClassName) {
            return NativeMethods._GetClassName(hwnd, lpClassName, lpClassName.Capacity);
        }

        [System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint = "GetClassName", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        private static extern int _GetClassName(User32.HWND hWnd, System.Text.StringBuilder lpClassName, int nMaxCount);

        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        public static extern short GetClassWord(User32.HWND hwnd, User32.GCW nIndex);

        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        public static extern int GetWindowLong(User32.HWND hwnd, User32.GWL nIndex);

        public static IntPtr GetWindowLongPtr(User32.HWND hwnd, User32.GWL nIndex) {
            if (IntPtr.Size == 4)
                return new IntPtr(NativeMethods.GetWindowLong(hwnd, nIndex));
            return NativeMethods._GetWindowLongPtr(hwnd, nIndex);
        }

        [System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint = "GetWindowLongPtr", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        private static extern IntPtr _GetWindowLongPtr(User32.HWND hwnd, User32.GWL nIndex);

        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        public static extern short RegisterClassEx([System.Runtime.InteropServices.In] ref User32.WNDCLASSEX wcex);

        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        public static extern int SetClassLong(User32.HWND hwnd, User32.GCL nIndex, int dwNewLong);

        public static IntPtr SetClassLongPtr(User32.HWND hwnd, User32.GCL nIndex, IntPtr dwNewLong) {
            if (IntPtr.Size == 4)
                return new IntPtr(NativeMethods.SetClassLong(hwnd, nIndex, dwNewLong.ToInt32()));
            return NativeMethods._SetClassLongPtr(hwnd, nIndex, dwNewLong);
        }

        [System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint = "SetClassLongPtr", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        private static extern IntPtr _SetClassLongPtr(User32.HWND hwnd, User32.GCL nIndex, IntPtr dwNewLong);

        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        public static extern short SetClassWord(User32.HWND hwnd, User32.GCW nIndex, short dwNewLong);

        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        public static extern int SetWindowLong(User32.HWND hwnd, User32.GWL nIndex, int dwNewLong);

        public static IntPtr SetWindowLongPtr(User32.HWND hwnd, User32.GWL nIndex, IntPtr dwNewLong) {
            if (IntPtr.Size == 4)
                return new IntPtr(NativeMethods.SetWindowLong(hwnd, nIndex, dwNewLong.ToInt32()));
            return NativeMethods._SetWindowLongPtr(hwnd, nIndex, dwNewLong);
        }

        [System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint = "SetWindowLongPtr", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        private static extern IntPtr _SetWindowLongPtr(User32.HWND hwnd, User32.GWL nIndex, IntPtr dwNewLong);

        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        private static extern bool UnregisterClass(string lpClassName, IntPtr hInstance);

        #endregion

        #region Message Functions

        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        public static extern User32.WM RegisterWindowMessage(string messageName);

        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SendMessage(User32.HWND hwnd, User32.WM msg, IntPtr wParam, IntPtr lParam);

        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        public static extern IntPtr PostMessage(User32.HWND hwnd, User32.WM msg, IntPtr wParam, IntPtr lParam);

        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        public static extern uint GetMessagePos();

        #endregion

        #region Timer Functions

        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        public static extern bool KillTimer(User32.HWND hwnd, IntPtr uIDEvent);

        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SetCoalescableTimer(User32.HWND hwnd, IntPtr nIDEvent, int uElapse, User32.TIMERPROC lpTimerFunc, int uToleranceDelay);

        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SetTimer(User32.HWND hWnd, IntPtr nIDEvent, int uElapse, User32.TIMERPROC lpTimerFunc);

        #endregion

        #region Window Functions

        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        public static extern bool DestroyWindow(User32.HWND hwnd);

        // TODO: CreateChildWindow, CreateTopLevelWindow... note hwndParent and hmenu mean different things.
        // TODO: HMODULE
        // TODO: HMENU
        // TODO: object lparam
        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        public static extern User32.HWND CreateWindowEx(User32.WS_EX dwExStyle, string lpClassName, string lpWindowName, User32.WS dwStyle, int x, int y, int nWidth, int nHeight, User32.HWND hWndParent, IntPtr hMenu, IntPtr hInstance, IntPtr lpParam);

        // TODO: COLORREF
        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        public static extern bool SetLayeredWindowAttributes(User32.HWND hwnd, uint crKey, byte bAlpha, User32.LWA dwFlags);

        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        public static extern bool ShowWindow(User32.HWND hWnd, User32.SW nCmdShow);

        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        public static extern int GetWindowThreadProcessId(User32.HWND hWnd, out int processId);

        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        public static extern User32.HWND GetParent(User32.HWND hwnd);

        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        public static extern User32.HWND GetAncestor(User32.HWND hwnd, User32.GA gaFlags);

        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        public static extern User32.HWND GetWindow(User32.HWND hwnd, User32.GW uCmd);

        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        public static extern bool GetWindowRect(User32.HWND hwnd, ref User32.RECT lpRect);

        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        public static extern bool GetClientRect(User32.HWND hwnd, ref User32.RECT lpRect);

        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        public static extern User32.HWND GetDesktopWindow();

        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        public static extern bool AdjustWindowRectEx(ref User32.RECT lpRect, User32.WS style, bool bMenu, User32.WS_EX exStyle);

        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        public static extern User32.HWND SetParent(User32.HWND hwndChild, User32.HWND hwndNewParent);

        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        public static extern Gdi32.COLORREF GetSysColor(Gdi32.COLOR nIndex);

        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetSysColorBrush(Gdi32.COLOR nIndex);

        #endregion

        #region Window Procedure Functions

        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        public static extern IntPtr CallWindowProc(User32.WNDPROC lpPrevWndFunc, User32.HWND hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        public static extern IntPtr DefWindowProc(User32.HWND hWnd, User32.WM Msg, IntPtr wParam, IntPtr lParam);

        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        public static extern bool IsChild(User32.HWND hWndParent, User32.HWND hwnd);

        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        public static extern bool SetWindowPos(User32.HWND hwnd, User32.HWND hWndInsertAfter, int X, int Y, int cx, int cy, User32.SWP uFlags);

        #endregion

        #region Painting and Drawing Functions

        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        public static extern int DrawText(IntPtr hDC, string lpString, int nCount, ref User32.RECT lpRect, User32.DT uFormat);


        // TODO: optional RECT
        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        public static extern bool RedrawWindow(User32.HWND hwnd, IntPtr lprcUpdate, IntPtr hrgnUpdate, User32.RDW flags);

        #endregion

        #region Coordinate Space and Transformation Functions

        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        public static extern bool ScreenToClient(User32.HWND hwnd, ref User32.POINT lpPoint);

        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        public static extern bool ClientToScreen(User32.HWND hwnd, ref User32.POINT lpPoint);

        #endregion

        #region Keyboard Input Functions

        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        public static extern User32.HWND GetFocus();

        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        public static extern short GetKeyState(User32.VK nVirtKey);

        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        public static extern short GetAsyncKeyState(User32.VK vKey);

        #endregion

        #region Device Context Functions

        //TODO: HDC
        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetDC(User32.HWND hWnd);

        //TODO: HDC
        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        public static extern bool ReleaseDC(User32.HWND hWnd, IntPtr hDC);

        #endregion
    }
}