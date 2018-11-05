using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Rhombus.Wpf.Airspace.Interop;

namespace Rhombus.Wpf.Airspace.Extensions {
    public static class HwndHostExtensions {
        /// <summary>
        ///     Attached property for HwndHost instances that specifies the
        ///     behavior for handling the SWP_NOCOPYBITS flag during moving
        ///     or sizing operations.
        /// </summary>
        public static readonly System.Windows.DependencyProperty CopyBitsBehaviorProperty = System.Windows.DependencyProperty.RegisterAttached("CopyBitsBehavior", typeof(CopyBitsBehavior), typeof(HwndHostExtensions), new System.Windows.FrameworkPropertyMetadata(CopyBitsBehavior.Default, HwndHostExtensions.OnCopyBitsBehaviorChanged));

        /// <summary>
        ///     Attached property for HwndHost instances that specifies
        ///     whether or not a HwndHostCommands.MouseActivate command is
        ///     raised in response to WM_MOUSEACTIVATE.
        /// </summary>
        public static readonly System.Windows.DependencyProperty RaiseMouseActivateCommandProperty = System.Windows.DependencyProperty.RegisterAttached("RaiseMouseActivateCommand", typeof(bool), typeof(HwndHostExtensions), new System.Windows.FrameworkPropertyMetadata(false, HwndHostExtensions.OnRaiseMouseActivateCommandChanged));

        /// <summary>
        ///     Attached property that is used to store the HwndHook used to
        ///     intercept the messages to the HwndHost window.  We have to
        ///     store this to prevent premature garbage collection.
        /// </summary>
        private static readonly System.Windows.DependencyProperty WindowHookProperty = System.Windows.DependencyProperty.RegisterAttached("WindowHook", typeof(HwndHostExtensionsWindowHook), typeof(HwndHostExtensions), new System.Windows.FrameworkPropertyMetadata(null));

        /// <summary>
        ///     Attached property that is used to record the users of the
        ///     attached window hook.  It is basically a simple reference
        ///     counting scheme.
        /// </summary>
        private static readonly System.Windows.DependencyProperty WindowHookRefCountProperty = System.Windows.DependencyProperty.RegisterAttached("WindowHookRefCount", typeof(int), typeof(HwndHostExtensions), new System.Windows.FrameworkPropertyMetadata(0));

        /// <summary>
        ///     Attached property getter for the CopyBitsBehavior property.
        /// </summary>
        public static CopyBitsBehavior GetCopyBitsBehavior(this System.Windows.Interop.HwndHost @this) {
            return (CopyBitsBehavior) @this.GetValue(CopyBitsBehaviorProperty);
        }

        /// <summary>
        ///     Attached property setter for the CopyBitsBehavior property.
        /// </summary>
        public static void SetCopyBitsBehavior(this System.Windows.Interop.HwndHost @this, CopyBitsBehavior value) {
            @this.SetValue(CopyBitsBehaviorProperty, value);
        }

        private static void OnCopyBitsBehaviorChanged(System.Windows.DependencyObject d, System.Windows.DependencyPropertyChangedEventArgs e) {
            var hwndHost = d as System.Windows.Interop.HwndHost;

            if (hwndHost != null) {
                var newValue = (CopyBitsBehavior) e.NewValue;
                if (newValue != CopyBitsBehavior.Default)
                    HwndHostExtensions.AddWndProcUsage(hwndHost);
                else
                    HwndHostExtensions.RemoveWndProcUsage(hwndHost);
            }
        }

        /// <summary>
        ///     Attached property getter for the RaiseMouseActivateCommand property.
        /// </summary>
        public static bool GetRaiseMouseActivateCommand(this System.Windows.Interop.HwndHost @this) {
            return (bool) @this.GetValue(RaiseMouseActivateCommandProperty);
        }

        /// <summary>
        ///     Attached property setter for the RaiseMouseActivateCommand property.
        /// </summary>
        public static void SetRaiseMouseActivateCommand(this System.Windows.Interop.HwndHost @this, bool value) {
            @this.SetValue(RaiseMouseActivateCommandProperty, value);
        }

        private static void OnRaiseMouseActivateCommandChanged(System.Windows.DependencyObject d, System.Windows.DependencyPropertyChangedEventArgs e) {
            var hwndHost = d as System.Windows.Interop.HwndHost;

            if (hwndHost != null) {
                var newValue = (bool) e.NewValue;
                if (newValue)
                    HwndHostExtensions.AddWndProcUsage(hwndHost);
                else
                    HwndHostExtensions.RemoveWndProcUsage(hwndHost);
            }
        }

        private static void AddWndProcUsage(System.Windows.Interop.HwndHost hwndHost) {
            var refCount = (int) hwndHost.GetValue(WindowHookRefCountProperty);
            refCount++;
            hwndHost.SetValue(WindowHookRefCountProperty, refCount);

            if (refCount == 1)
                if (!HwndHostExtensions.TryHookWndProc(hwndHost))
                    hwndHost.Loaded += (s, e) => HwndHostExtensions.TryHookWndProc((System.Windows.Interop.HwndHost) s);
        }

        private static bool TryHookWndProc(System.Windows.Interop.HwndHost hwndHost) {
            if (hwndHost.Handle != IntPtr.Zero) {
                // Hook the window messages so we can intercept the
                // various messages.
                var hook = new HwndHostExtensionsWindowHook(hwndHost);

                // Keep our hook alive.
                hwndHost.SetValue(WindowHookProperty, hook);

                return true;
            }

            return false;
        }

        private static void RemoveWndProcUsage(System.Windows.Interop.HwndHost hwndHost) {
            var refCount = (int) hwndHost.GetValue(WindowHookRefCountProperty);
            refCount--;
            hwndHost.SetValue(WindowHookRefCountProperty, refCount);

            if (refCount == 0) {
                var hook = (HwndHostExtensionsWindowHook) hwndHost.GetValue(WindowHookProperty);
                hook.Dispose();
                hwndHost.ClearValue(WindowHookProperty);
            }
        }

        private class HwndHostExtensionsWindowHook : Win32.ComCtl32.WindowSubclass {
            private static readonly Win32.User32.WM _redrawMessage;

            static HwndHostExtensionsWindowHook() {
                _redrawMessage = Win32.NativeMethods.RegisterWindowMessage("HwndHostExtensionsWindowHook.RedrawMessage");
            }

            public HwndHostExtensionsWindowHook(System.Windows.Interop.HwndHost hwndHost) : base(new Win32.User32.HWND(hwndHost.Handle)) {
                _hwndHost = hwndHost;
            }

            protected override IntPtr WndProcOverride(Win32.User32.HWND hwnd, Win32.User32.WM msg, IntPtr wParam, IntPtr lParam, IntPtr id, IntPtr data) {
                IntPtr? result = null;

                if (msg == Win32.User32.WM.WINDOWPOSCHANGING) {
                    unsafe {
                        var pWindowPos = (Win32.User32.WINDOWPOS*) lParam;

                        var copyBitsBehavior = _hwndHost.GetCopyBitsBehavior();

                        switch (copyBitsBehavior) {
                            case CopyBitsBehavior.AlwaysCopyBits:
                                pWindowPos->flags &= ~Win32.User32.SWP.NOCOPYBITS;
                                break;

                            case CopyBitsBehavior.CopyBitsAndRepaint:
                                pWindowPos->flags &= ~Win32.User32.SWP.NOCOPYBITS;
                                if (!_redrawMessagePosted) {
                                    Win32.NativeMethods.PostMessage(hwnd, _redrawMessage, IntPtr.Zero, IntPtr.Zero);
                                    _redrawMessagePosted = true;
                                }

                                break;

                            case CopyBitsBehavior.NeverCopyBits:
                                pWindowPos->flags |= Win32.User32.SWP.NOCOPYBITS;
                                break;

                            case CopyBitsBehavior.Default:
                            default:
                                // do nothing.
                                break;
                        }
                    }
                }
                else if (msg == _redrawMessage) {
                    _redrawMessagePosted = false;

                    // Invalidate the window that moved, because it might have copied garbage
                    // due to WPF rendering through DX on a different thread.
                    Win32.NativeMethods.RedrawWindow(hwnd, IntPtr.Zero, IntPtr.Zero, Win32.User32.RDW.INVALIDATE | Win32.User32.RDW.ALLCHILDREN);

                    // Then immediately redraw all invalid regions within the top-level window.
                    var hwndRoot = Win32.NativeMethods.GetAncestor(hwnd, Win32.User32.GA.ROOT);
                    Win32.NativeMethods.RedrawWindow(hwndRoot, IntPtr.Zero, IntPtr.Zero, Win32.User32.RDW.UPDATENOW | Win32.User32.RDW.ALLCHILDREN);
                }
                else if (msg == Win32.User32.WM.MOUSEACTIVATE) {
                    var raiseMouseActivateCommand = _hwndHost.GetRaiseMouseActivateCommand();
                    if (raiseMouseActivateCommand) {
                        // Raise the HwndHostCommands.MouseActivate command.
                        var parameter = new MouseActivateParameter();
                        HwndHostCommands.MouseActivate.Execute(parameter, _hwndHost);

                        if (parameter.HandleMessage) {
                            if (parameter.Activate == false && parameter.EatMessage == false)
                                result = new IntPtr((int) Win32.User32.MA.NOACTIVATE);
                            else if (parameter.Activate == false && parameter.EatMessage)
                                result = new IntPtr((int) Win32.User32.MA.NOACTIVATEANDEAT);
                            else if (parameter.Activate && parameter.EatMessage == false)
                                result = new IntPtr((int) Win32.User32.MA.ACTIVATE);
                            else // if(parameter.Activate == true && parameter.EatMessage == true)
                                result = new IntPtr((int) Win32.User32.MA.ACTIVATEANDEAT);
                        }
                    }
                }

                return result ?? base.WndProcOverride(hwnd, msg, wParam, lParam, id, data);
            }

            private readonly System.Windows.Interop.HwndHost _hwndHost;
            private bool _redrawMessagePosted;
        }
    }
}