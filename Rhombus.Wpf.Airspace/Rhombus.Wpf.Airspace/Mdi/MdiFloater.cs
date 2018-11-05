using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Mdi {
    public class MdiFloater : System.Windows.Window {
        static MdiFloater() {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MdiFloater), new System.Windows.FrameworkPropertyMetadata(typeof(MdiFloater)));

            System.Windows.Input.CommandManager.RegisterClassCommandBinding(
                /* Type:            */ typeof(MdiFloater),
                /* Command Binding: */ new System.Windows.Input.CommandBinding(
                    /*     Command:     */ MdiCommands.AdjustWindowRect,
                    /*     Execute:     */ (s, e) => ((MdiFloater) s).ExecuteAdjustWindowRect(e),
                    /*     CanExecute:  */ (s, e) => ((MdiFloater) s).CanExecuteAdjustWindowRect(e)));

            System.Windows.Input.CommandManager.RegisterClassCommandBinding(
                /* Type:            */ typeof(MdiFloater),
                /* Command Binding: */ new System.Windows.Input.CommandBinding(
                    /*     Command:     */ MdiCommands.MaximizeWindow,
                    /*     Execute:     */ (s, e) => ((MdiFloater) s).ExecuteMaximizeWindow(e),
                    /*     CanExecute:  */ (s, e) => ((MdiFloater) s).CanExecuteMaximizeWindow(e)));

            System.Windows.Input.CommandManager.RegisterClassCommandBinding(
                /* Type:            */ typeof(MdiFloater),
                /* Command Binding: */ new System.Windows.Input.CommandBinding(
                    /*     Command:     */ MdiCommands.MinimizeWindow,
                    /*     Execute:     */ (s, e) => ((MdiFloater) s).ExecuteMinimizeWindow(e),
                    /*     CanExecute:  */ (s, e) => ((MdiFloater) s).CanExecuteMinimizeWindow(e)));

            System.Windows.Input.CommandManager.RegisterClassCommandBinding(
                /* Type:            */ typeof(MdiFloater),
                /* Command Binding: */ new System.Windows.Input.CommandBinding(
                    /*     Command:     */ MdiCommands.RestoreWindow,
                    /*     Execute:     */ (s, e) => ((MdiFloater) s).ExecuteRestoreWindow(e),
                    /*     CanExecute:  */ (s, e) => ((MdiFloater) s).CanExecuteRestoreWindow(e)));

            System.Windows.Input.CommandManager.RegisterClassCommandBinding(
                /* Type:            */ typeof(MdiFloater),
                /* Command Binding: */ new System.Windows.Input.CommandBinding(
                    /*     Command:     */ MdiCommands.CloseWindow,
                    /*     Execute:     */ (s, e) => ((MdiFloater) s).ExecuteCloseWindow(e),
                    /*     CanExecute:  */ (s, e) => ((MdiFloater) s).CanExecuteCloseWindow(e)));
        }

        /// <summary>
        ///     Adjust the window rect of a window.
        /// </summary>
        public void AdjustWindowRect(MdiWindow window, System.Windows.Vector delta, MdiWindowEdge interactiveEdges) {
            if (window == null)
                throw new ArgumentNullException("window");

            if (this.Content != window)
                throw new ArgumentException("Window does not belong to this floater.", "window");

            var screenRect = MdiPanel.GetWindowRect(window);

            if (interactiveEdges == MdiWindowEdge.None) {
                screenRect.X += delta.X;
                screenRect.Y += delta.Y;
            }
            else {
                if ((interactiveEdges & MdiWindowEdge.Left) != 0) {
                    // Can't size smaller than the minimum size.
                    var constrainedDelta = Math.Min(delta.X, screenRect.Width - window.MinWidth);

                    screenRect.X += constrainedDelta;
                    screenRect.Width -= constrainedDelta;
                }

                if ((interactiveEdges & MdiWindowEdge.Right) != 0) {
                    // Can't size smaller than the minimum size.
                    var constrainedDelta = Math.Max(delta.X, -(screenRect.Width - window.MinWidth));

                    screenRect.Width += constrainedDelta;
                }

                if ((interactiveEdges & MdiWindowEdge.Top) != 0) {
                    // Can't size smaller than the minimum size.
                    var constrainedDelta = Math.Min(delta.Y, screenRect.Height - window.MinHeight);

                    screenRect.Y += constrainedDelta;
                    screenRect.Height -= constrainedDelta;
                }

                if ((interactiveEdges & MdiWindowEdge.Bottom) != 0) {
                    // Can't size smaller than the minimum size.
                    var constrainedDelta = Math.Max(delta.Y, -(screenRect.Height - window.MinHeight));

                    screenRect.Height += constrainedDelta;
                }
            }

            if (window.MinWidth > screenRect.Width) {
                if ((interactiveEdges & MdiWindowEdge.Left) != 0)
                    screenRect.X = screenRect.Right - window.MinWidth;
                screenRect.Width = window.MinWidth;
            }

            if (window.MinHeight > screenRect.Height) {
                if ((interactiveEdges & MdiWindowEdge.Top) != 0)
                    screenRect.Y = screenRect.Bottom - window.MinHeight;
                screenRect.Height = window.MinHeight;
            }

            MdiPanel.SetWindowRect(window, screenRect);

            this.Left = screenRect.Left;
            this.Top = screenRect.Top;
            this.Width = screenRect.Width;
            this.Height = screenRect.Height;
        }

        /// <summary>
        ///     Set the window state of a window in the view.
        /// </summary>
        /// <remarks>
        ///     MdiWindow itself will call this method when its
        ///     MdiPanel.WindowState property changes.
        /// </remarks>
        public void SetWindowState(MdiWindow window, System.Windows.WindowState windowState) {
            if (window == null)
                throw new ArgumentNullException("window");

            if (this.Content != window)
                throw new ArgumentException("Window does not belong to this floater.", "window");

            window.SetValue(MdiPanel.WindowStateProperty, windowState);
            this.WindowState = windowState;
        }

        /// <summary>
        ///     Sets the window state of the window to maximized.
        /// </summary>
        public void MaximizeWindow(MdiWindow window) {
            this.SetWindowState(window, System.Windows.WindowState.Maximized);
        }

        /// <summary>
        ///     Sets the window state of the window to maximized.
        /// </summary>
        public void MinimizeWindow(MdiWindow window) {
            this.SetWindowState(window, System.Windows.WindowState.Minimized);
        }

        /// <summary>
        ///     Sets the window state of the window to normal.
        /// </summary>
        public void RestoreWindow(MdiWindow window) {
            this.SetWindowState(window, System.Windows.WindowState.Normal);
        }

        /// <summary>
        ///     Removes the window from the view.
        /// </summary>
        public void CloseWindow(MdiWindow window) {
            if (window == null)
                throw new ArgumentNullException("window");

            if (this.Content != window)
                throw new ArgumentException("Window does not belong to this floater.", "window");

            // Raise the window.Closing event, which can be canceled.
            if (window.Close()) {
                // close this floater.
                this.Close();

                // Walk the visual tree looking for disposable elements.
                Extensions.ElementExtensions.DisposeSubTree(window);
            }
        }

        /// <summary>
        ///     Execute handler for the MdiCommands.AdjustWindowRect command.
        /// </summary>
        private void ExecuteAdjustWindowRect(System.Windows.Input.ExecutedRoutedEventArgs e) {
            var originalSource = (System.Windows.UIElement) e.OriginalSource;
            var swp = (AdjustWindowRectParameter) e.Parameter;

            var window = this.Content as MdiWindow;
            System.Diagnostics.Debug.Assert(window != null && MdiPanel.GetWindowState(window) == System.Windows.WindowState.Normal);

            var delta = Extensions.ElementExtensions.TransformElementToElement(originalSource, swp.Delta, window);

            this.AdjustWindowRect(window, delta, swp.InteractiveEdges);
        }

        /// <summary>
        ///     CanExecute handler for the MdiCommands.AdjustWindowRect command.
        /// </summary>
        private void CanExecuteAdjustWindowRect(System.Windows.Input.CanExecuteRoutedEventArgs e) {
            var window = this.Content as MdiWindow;
            e.CanExecute = window != null && MdiPanel.GetWindowState(window) == System.Windows.WindowState.Normal;
        }

        /// <summary>
        ///     Execute handler for the MdiCommands.MaximizeWindow command.
        /// </summary>
        private void ExecuteMaximizeWindow(System.Windows.Input.ExecutedRoutedEventArgs e) {
            var window = this.Content as MdiWindow;
            System.Diagnostics.Debug.Assert(window != null && MdiPanel.GetWindowState(window) != System.Windows.WindowState.Maximized);

            this.MaximizeWindow(window);
        }

        /// <summary>
        ///     CanExecute handler for the MdiCommands.MaximizeWindow command.
        /// </summary>
        private void CanExecuteMaximizeWindow(System.Windows.Input.CanExecuteRoutedEventArgs e) {
            var window = this.Content as MdiWindow;
            e.CanExecute = window != null && MdiPanel.GetWindowState(window) != System.Windows.WindowState.Maximized;
        }

        /// <summary>
        ///     Execute handler for the MdiCommands.MinimizeWindow command.
        /// </summary>
        private void ExecuteMinimizeWindow(System.Windows.Input.ExecutedRoutedEventArgs e) {
            var window = this.Content as MdiWindow;
            System.Diagnostics.Debug.Assert(window != null && MdiPanel.GetWindowState(window) != System.Windows.WindowState.Minimized);

            this.MinimizeWindow(window);
        }

        /// <summary>
        ///     CanExecute handler for the MdiCommands.MinimizeWindow command.
        /// </summary>
        private void CanExecuteMinimizeWindow(System.Windows.Input.CanExecuteRoutedEventArgs e) {
            var window = this.Content as MdiWindow;
            e.CanExecute = window != null && MdiPanel.GetWindowState(window) != System.Windows.WindowState.Minimized;
        }

        /// <summary>
        ///     Execute handler for the MdiCommands.RestoreWindow command.
        /// </summary>
        private void ExecuteRestoreWindow(System.Windows.Input.ExecutedRoutedEventArgs e) {
            var window = this.Content as MdiWindow;
            System.Diagnostics.Debug.Assert(window != null && MdiPanel.GetWindowState(window) != System.Windows.WindowState.Normal);

            this.RestoreWindow(window);
        }

        /// <summary>
        ///     CanExecute handler for the MdiCommands.RestoreWindow command.
        /// </summary>
        private void CanExecuteRestoreWindow(System.Windows.Input.CanExecuteRoutedEventArgs e) {
            var window = this.Content as MdiWindow;
            e.CanExecute = window != null && MdiPanel.GetWindowState(window) != System.Windows.WindowState.Normal;
        }

        /// <summary>
        ///     Execute handler for the MdiCommands.CloseWindow command.
        /// </summary>
        private void ExecuteCloseWindow(System.Windows.Input.ExecutedRoutedEventArgs e) {
            var window = this.Content as MdiWindow;
            System.Diagnostics.Debug.Assert(window != null);

            this.CloseWindow(window);
        }

        /// <summary>
        ///     CanExecute handler for the MdiCommands.CloseWindow command.
        /// </summary>
        /// <remarks>
        ///     The command is only enabled if the items are not bound via
        ///     the ItemsSource.
        /// </remarks>
        private void CanExecuteCloseWindow(System.Windows.Input.CanExecuteRoutedEventArgs e) {
            var window = this.Content as MdiWindow;
            e.CanExecute = window != null;
        }
    }
}