using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Interop {
    public sealed class WindowClass<TWindow> : ISupportInitialize where TWindow : WindowBase, new() {
        public WindowClass() {
            _wcex = new Win32.User32.WNDCLASSEX();
            _wcex.cbSize = System.Runtime.InteropServices.Marshal.SizeOf(_wcex);
            _wcex.hInstance = System.Runtime.InteropServices.Marshal.GetHINSTANCE(typeof(WindowClass<TWindow>).Module);
            _wcex.lpszClassName = typeof(WindowClass<TWindow>).FullName;
            _wcex.lpfnWndProc = this.WndProc;
        }

        public string Name => _wcex.lpszClassName;

        public bool EnableDoubleClickMessages {
            get => (_wcex.style & Win32.User32.CS.DBLCLKS) == Win32.User32.CS.DBLCLKS;

            set => _wcex.style |= Win32.User32.CS.DBLCLKS;
        }

        public bool RedrawWindowOnVerticalChange {
            get => (_wcex.style & Win32.User32.CS.VREDRAW) == Win32.User32.CS.VREDRAW;

            set => _wcex.style |= Win32.User32.CS.VREDRAW;
        }

        public bool RedrawWindowOnHorizontalChange {
            get => (_wcex.style & Win32.User32.CS.HREDRAW) == Win32.User32.CS.HREDRAW;

            set => _wcex.style |= Win32.User32.CS.HREDRAW;
        }

        public bool CacheBackgroundBitmap {
            get => (_wcex.style & Win32.User32.CS.SAVEBITS) == Win32.User32.CS.SAVEBITS;

            set => _wcex.style |= Win32.User32.CS.SAVEBITS;
        }

        public bool UseParentClippingRect {
            get => (_wcex.style & Win32.User32.CS.PARENTDC) == Win32.User32.CS.PARENTDC;

            set => _wcex.style |= Win32.User32.CS.PARENTDC;
        }

        public bool EnableDropShadow {
            get => (_wcex.style & Win32.User32.CS.DROPSHADOW) == Win32.User32.CS.DROPSHADOW;

            set => _wcex.style |= Win32.User32.CS.DROPSHADOW;
        }

        public WindowClassType Type {
            get => (_wcex.style & Win32.User32.CS.GLOBALCLASS) == Win32.User32.CS.GLOBALCLASS
                ? WindowClassType.ApplicationGlobal
                : WindowClassType.ApplicationLocal;

            set {
                switch (value) {
                    case WindowClassType.ApplicationGlobal:
                        _wcex.style |= Win32.User32.CS.DROPSHADOW;
                        break;

                    case WindowClassType.ApplicationLocal:
                        _wcex.style &= ~Win32.User32.CS.DROPSHADOW;
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public DeviceContextCachePolicy DeviceContextCachePolicy {
            get {
                var mask = Win32.User32.CS.CLASSDC | Win32.User32.CS.OWNDC;
                switch (_wcex.style & mask) {
                    case Win32.User32.CS.CLASSDC:
                        return DeviceContextCachePolicy.WindowClass;

                    case Win32.User32.CS.OWNDC:
                        return DeviceContextCachePolicy.Window;

                    default:
                        return DeviceContextCachePolicy.Global;
                }
            }

            set {
                var mask = Win32.User32.CS.CLASSDC | Win32.User32.CS.OWNDC;

                switch (value) {
                    case DeviceContextCachePolicy.WindowClass:
                        _wcex.style &= ~mask;
                        _wcex.style |= Win32.User32.CS.CLASSDC;
                        break;

                    case DeviceContextCachePolicy.Window:
                        _wcex.style &= ~mask;
                        _wcex.style |= Win32.User32.CS.OWNDC;
                        break;

                    case DeviceContextCachePolicy.Global:
                        _wcex.style &= ~mask;
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public int ExtraClassBytes {
            get => _wcex.cbClsExtra;

            set {
                if (value < 0)
                    throw new ArgumentOutOfRangeException();
                _wcex.cbClsExtra = value;
            }
        }

        public int ExtraWindowBytes {
            get => _wcex.cbWndExtra;

            set {
                if (value < 0)
                    throw new ArgumentOutOfRangeException();
                _wcex.cbWndExtra = value;
            }
        }

        public IntPtr Background {
            get => _wcex.hbrBackground;

            set => _wcex.hbrBackground = value;
        }

        public TWindow CreateWindow(WindowParameters windowParams) {
            var gcHandle = new System.Runtime.InteropServices.GCHandle();
            var lpCreateParam = IntPtr.Zero;
            if (windowParams.Tag != null) {
                gcHandle = System.Runtime.InteropServices.GCHandle.Alloc(windowParams.Tag);
                lpCreateParam = System.Runtime.InteropServices.GCHandle.ToIntPtr(gcHandle);
            }

            var hwnd = Win32.ComCtl32.StrongHWND.CreateWindowEx(windowParams.ExtendedStyle, this.Name, windowParams.Name, windowParams.Style, windowParams.WindowRect.X, windowParams.WindowRect.Y, windowParams.WindowRect.Width, windowParams.WindowRect.Height, windowParams.Parent, IntPtr.Zero, System.Runtime.InteropServices.Marshal.GetHINSTANCE(typeof(TWindow).Module), lpCreateParam);

            TWindow createdWindow = null;
            if (!hwnd.IsInvalid) {
                System.Diagnostics.Debug.Assert(_createdWindow != null);
                _createdWindow.TransferHandleOwnership(hwnd);

                createdWindow = _createdWindow;
            }

            _createdWindow = null;
            return createdWindow;
        }

        private IntPtr WndProc(IntPtr hwnd, int message, IntPtr wParam, IntPtr lParam) {
            // This window proc is only ever used to receive the first message
            // intended for a window.  Here we create an instance of the real
            // TWindow type. 
            _createdWindow = new TWindow();

            // Pass the parameter for this first message to the new window.
            // It will replace the window proceedure and pass this first
            // message to it.
            return _createdWindow.InitializeFromFirstMessage(hwnd, message, wParam, lParam);
        }

        public void BeginInit() {
            switch (_initializationState) {
                case false:
                    throw new InvalidOperationException("The window class has already begun initialization.");

                case true:
                    throw new InvalidOperationException("The window class has already completed initialization.");

                case null:
                    _initializationState = false;
                    break;
            }
        }

        public void EndInit() {
            switch (_initializationState) {
                case null:
                    throw new InvalidOperationException("The window class has not begun initialization.");

                case true:
                    throw new InvalidOperationException("The window class has already completed initialization.");

                case false:
                    _atom = Win32.NativeMethods.RegisterClassEx(ref _wcex);
                    break;
            }
        }

        private short _atom;

        [ThreadStatic] private TWindow _createdWindow;

        private bool? _initializationState;
        private Win32.User32.WNDCLASSEX _wcex;
    }
}