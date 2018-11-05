using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Rhombus.Wpf.Airspace.Interop;

namespace Rhombus.Wpf.Airspace.Mdi {
    public class MdiView : Controls.SelectorEx<MdiWindow> {
        static MdiView() {
            // Look up the style for this control by using its type as its key.
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MdiView), new System.Windows.FrameworkPropertyMetadata(typeof(MdiView)));

            SelectedItemProperty.OverrideMetadata(
                /* Type:                 */ typeof(MdiView),
                /* Metadata:             */ new System.Windows.FrameworkPropertyMetadata(
                    /*     Changed Callback: */ (s, e) => ((MdiView) s).OnSelectedItemChanged(e),
                    /*     Coerce Callback:  */ (d, bv) => ((MdiView) d).OnCoerceSelectedItem(bv)));

            System.Windows.Input.CommandManager.RegisterClassCommandBinding(
                /* Type:            */ typeof(MdiView),
                /* Command Binding: */ new System.Windows.Input.CommandBinding(
                    /*     Command:     */ MdiCommands.MaximizeWindow,
                    /*     Execute:     */ (s, e) => ((MdiView) s).ExecuteMaximizeWindow(e),
                    /*     CanExecute:  */ (s, e) => ((MdiView) s).CanExecuteMaximizeWindow(e)));

            System.Windows.Input.CommandManager.RegisterClassCommandBinding(
                /* Type:            */ typeof(MdiView),
                /* Command Binding: */ new System.Windows.Input.CommandBinding(
                    /*     Command:     */ MdiCommands.MinimizeWindow,
                    /*     Execute:     */ (s, e) => ((MdiView) s).ExecuteMinimizeWindow(e),
                    /*     CanExecute:  */ (s, e) => ((MdiView) s).CanExecuteMinimizeWindow(e)));

            System.Windows.Input.CommandManager.RegisterClassCommandBinding(
                /* Type:            */ typeof(MdiView),
                /* Command Binding: */ new System.Windows.Input.CommandBinding(
                    /*     Command:     */ MdiCommands.RestoreWindow,
                    /*     Execute:     */ (s, e) => ((MdiView) s).ExecuteRestoreWindow(e),
                    /*     CanExecute:  */ (s, e) => ((MdiView) s).CanExecuteRestoreWindow(e)));

            System.Windows.Input.CommandManager.RegisterClassCommandBinding(
                /* Type:            */ typeof(MdiView),
                /* Command Binding: */ new System.Windows.Input.CommandBinding(
                    /*     Command:     */ MdiCommands.CloseWindow,
                    /*     Execute:     */ (s, e) => ((MdiView) s).ExecuteCloseWindow(e),
                    /*     CanExecute:  */ (s, e) => ((MdiView) s).CanExecuteCloseWindow(e)));

            System.Windows.Input.CommandManager.RegisterClassCommandBinding(
                /* Type:            */ typeof(MdiView),
                /* Command Binding: */ new System.Windows.Input.CommandBinding(
                    /*     Command:     */ MdiCommands.AdjustWindowRect,
                    /*     Execute:     */ (s, e) => ((MdiView) s).ExecuteAdjustWindowRect(e),
                    /*     CanExecute:  */ (s, e) => ((MdiView) s).CanExecuteAdjustWindowRect(e)));

            System.Windows.Input.CommandManager.RegisterClassCommandBinding(
                /* Type:            */ typeof(MdiView),
                /* Command Binding: */ new System.Windows.Input.CommandBinding(
                    /*     Command:     */ MdiCommands.ActivateWindow,
                    /*     Execute:     */ (s, e) => ((MdiView) s).ExecuteActivateWindow(e),
                    /*     CanExecute:  */ (s, e) => ((MdiView) s).CanExecuteActivateWindow(e)));

            System.Windows.Input.CommandManager.RegisterClassCommandBinding(
                /* Type:            */ typeof(MdiView),
                /* Command Binding: */ new System.Windows.Input.CommandBinding(
                    /*     Command:     */ MdiCommands.ActivateNextWindow,
                    /*     Execute:     */ (s, e) => ((MdiView) s).ExecuteActivateNextWindow(e),
                    /*     CanExecute:  */ (s, e) => ((MdiView) s).CanExecuteCommandThatRequiresMoreThanOneWindow(e)));

            System.Windows.Input.CommandManager.RegisterClassInputBinding(
                /* Type:            */ typeof(MdiView),
                /* Input Binding:   */ new System.Windows.Input.InputBinding(
                    /*     Command:     */ MdiCommands.ActivateNextWindow,
                    /*     Gesture:     */ new System.Windows.Input.KeyGesture(System.Windows.Input.Key.Tab, System.Windows.Input.ModifierKeys.Control)));

            System.Windows.Input.CommandManager.RegisterClassInputBinding(
                /* Type:            */ typeof(MdiView),
                /* Input Binding:   */ new System.Windows.Input.InputBinding(
                    /*     Command:     */ MdiCommands.ActivatePreviousWindow,
                    /*     Gesture:     */ new System.Windows.Input.KeyGesture(System.Windows.Input.Key.Tab, System.Windows.Input.ModifierKeys.Control | System.Windows.Input.ModifierKeys.Shift)));

            System.Windows.Input.CommandManager.RegisterClassCommandBinding(
                /* Type:            */ typeof(MdiView),
                /* Command Binding: */ new System.Windows.Input.CommandBinding(
                    /*     Command:     */ MdiCommands.ActivatePreviousWindow,
                    /*     Execute:     */ (s, e) => ((MdiView) s).ExecuteActivatePreviousWindow(e),
                    /*     CanExecute:  */ (s, e) => ((MdiView) s).CanExecuteCommandThatRequiresMoreThanOneWindow(e)));

            System.Windows.Input.CommandManager.RegisterClassCommandBinding(
                /* Type:            */ typeof(MdiView),
                /* Command Binding: */ new System.Windows.Input.CommandBinding(
                    /*     Command:     */ MdiCommands.TileWindows,
                    /*     Execute:     */ (s, e) => ((MdiView) s).ExecuteTileWindows(e),
                    /*     CanExecute:  */ (s, e) => ((MdiView) s).CanExecuteCommandThatRequiresMoreThanOneWindow(e)));

            System.Windows.Input.CommandManager.RegisterClassCommandBinding(
                /* Type:            */ typeof(MdiView),
                /* Command Binding: */ new System.Windows.Input.CommandBinding(
                    /*     Command:     */ MdiCommands.CascadeWindows,
                    /*     Execute:     */ (s, e) => ((MdiView) s).ExecuteCascadeWindows(e),
                    /*     CanExecute:  */ (s, e) => ((MdiView) s).CanExecuteCommandThatRequiresMoreThanOneWindow(e)));

            System.Windows.Input.CommandManager.RegisterClassCommandBinding(
                /* Type:            */ typeof(MdiView),
                /* Command Binding: */ new System.Windows.Input.CommandBinding(
                    /*     Command:     */ MdiCommands.MinimizeAllWindows,
                    /*     Execute:     */ (s, e) => ((MdiView) s).ExecuteMinimizeAllWindows(e),
                    /*     CanExecute:  */ (s, e) => ((MdiView) s).CanExecuteMinimizeAllWindows(e)));

            System.Windows.Input.CommandManager.RegisterClassCommandBinding(
                /* Type:            */ typeof(MdiView),
                /* Command Binding: */ new System.Windows.Input.CommandBinding(
                    /*     Command:     */ MdiCommands.MaximizeAllWindows,
                    /*     Execute:     */ (s, e) => ((MdiView) s).ExecuteMaximizeAllWindows(e),
                    /*     CanExecute:  */ (s, e) => ((MdiView) s).CanExecuteMaximizeAllWindows(e)));

            System.Windows.Input.CommandManager.RegisterClassCommandBinding(
                /* Type:            */ typeof(MdiView),
                /* Command Binding: */ new System.Windows.Input.CommandBinding(
                    /*     Command:     */ MdiCommands.RestoreAllWindows,
                    /*     Execute:     */ (s, e) => ((MdiView) s).ExecuteRestoreAllWindows(e),
                    /*     CanExecute:  */ (s, e) => ((MdiView) s).CanExecuteRestoreAllWindows(e)));

            System.Windows.Input.CommandManager.RegisterClassCommandBinding(
                /* Type:            */ typeof(MdiView),
                /* Command Binding: */ new System.Windows.Input.CommandBinding(
                    /*     Command:     */ MdiCommands.FloatWindow,
                    /*     Execute:     */ (s, e) => ((MdiView) s).ExecuteFloatWindow(e),
                    /*     CanExecute:  */ (s, e) => ((MdiView) s).CanExecuteFloatWindow(e)));
        }

        /// <summary>
        ///     Constructor for the MdiView class.
        /// </summary>
        public MdiView() {
            this.Windows = new ReadOnlyObservableCollection<MdiWindow>(_windows);
        }

        /// <summary>
        ///     A read-only observable collection of all windows in this view.
        /// </summary>
        public ReadOnlyObservableCollection<MdiWindow> Windows { get; }

        /// <summary>
        ///     Returns the effective ZIndex for a window in the view.
        /// </summary>
        /// <remarks>
        ///     The MdiWindow control will coerce its ZOrder property
        ///     to this value.
        /// </remarks>
        public int GetZIndex(MdiWindow window) {
            if (window == null)
                throw new ArgumentNullException("window");

            if (window.View != this)
                throw new ArgumentException("Window does not belong to this view.", "window");

            System.Diagnostics.Debug.Assert(_windows.Contains(window));
            return _windows.IndexOf(window);
        }

        /// <summary>
        ///     Adjust the window rect of a window.
        /// </summary>
        public void AdjustWindowRect(MdiWindow window, System.Windows.Vector delta, MdiWindowEdge interactiveEdges) {
            var panel = (MdiPanel) System.Windows.Media.VisualTreeHelper.GetParent(window);
            var contentScrollViewer = this.GetTemplateChild("PART_ContentScrollViewer") as System.Windows.Controls.ScrollViewer;

            var panelBounds = panel.Extents;
            var proposedWindowRect = MdiPanel.GetWindowRect(window);
            var originalProposedWindowRect = proposedWindowRect;

            if (interactiveEdges == MdiWindowEdge.None) {
                proposedWindowRect.X += delta.X;
                proposedWindowRect.Y += delta.Y;

                if (contentScrollViewer == null || System.Windows.Controls.ScrollViewer.GetHorizontalScrollBarVisibility(contentScrollViewer) == System.Windows.Controls.ScrollBarVisibility.Disabled) {
                    // Can't extend off the right.
                    proposedWindowRect.X = Math.Min(proposedWindowRect.X, panelBounds.Right - proposedWindowRect.Width);

                    // Can't extend off the left.
                    proposedWindowRect.X = Math.Max(proposedWindowRect.X, panelBounds.Left);
                }

                if (contentScrollViewer == null || System.Windows.Controls.ScrollViewer.GetVerticalScrollBarVisibility(contentScrollViewer) == System.Windows.Controls.ScrollBarVisibility.Disabled) {
                    // Can't extend off the bottom.
                    proposedWindowRect.Y = Math.Min(proposedWindowRect.Y, panelBounds.Bottom - proposedWindowRect.Height);

                    // Can't extend off the top.
                    proposedWindowRect.Y = Math.Max(proposedWindowRect.Y, panelBounds.Top);
                }
            }
            else {
                if ((interactiveEdges & MdiWindowEdge.Left) != 0) {
                    // Can't size smaller than the minimum size.
                    var constrainedDelta = Math.Min(delta.X, proposedWindowRect.Width - window.MinWidth);
                    if (System.Windows.Controls.ScrollViewer.GetHorizontalScrollBarVisibility(this) == System.Windows.Controls.ScrollBarVisibility.Disabled)
                        constrainedDelta = Math.Max(constrainedDelta, panelBounds.Left - proposedWindowRect.X);

                    proposedWindowRect.X += constrainedDelta;
                    proposedWindowRect.Width -= constrainedDelta;
                }

                if ((interactiveEdges & MdiWindowEdge.Right) != 0) {
                    // Can't size smaller than the minimum size.
                    var constrainedDelta = Math.Max(delta.X, -(proposedWindowRect.Width - window.MinWidth));
                    if (System.Windows.Controls.ScrollViewer.GetHorizontalScrollBarVisibility(this) == System.Windows.Controls.ScrollBarVisibility.Disabled)
                        constrainedDelta = Math.Min(constrainedDelta, panelBounds.Right - proposedWindowRect.Right);

                    proposedWindowRect.Width += constrainedDelta;
                }

                if ((interactiveEdges & MdiWindowEdge.Top) != 0) {
                    // Can't size smaller than the minimum size.
                    var constrainedDelta = Math.Min(delta.Y, proposedWindowRect.Height - window.MinHeight);
                    if (System.Windows.Controls.ScrollViewer.GetVerticalScrollBarVisibility(this) == System.Windows.Controls.ScrollBarVisibility.Disabled)
                        constrainedDelta = Math.Max(constrainedDelta, panelBounds.Top - proposedWindowRect.Y);

                    proposedWindowRect.Y += constrainedDelta;
                    proposedWindowRect.Height -= constrainedDelta;
                }

                if ((interactiveEdges & MdiWindowEdge.Bottom) != 0) {
                    // Can't size smaller than the minimum size.
                    var constrainedDelta = Math.Max(delta.Y, -(proposedWindowRect.Height - window.MinHeight));
                    if (System.Windows.Controls.ScrollViewer.GetVerticalScrollBarVisibility(this) == System.Windows.Controls.ScrollBarVisibility.Disabled)
                        constrainedDelta = Math.Min(constrainedDelta, panelBounds.Bottom - proposedWindowRect.Bottom);

                    proposedWindowRect.Height += constrainedDelta;
                }
            }

            if (this.EnableSnapping) {
                if (interactiveEdges == MdiWindowEdge.None) {
                    var left = proposedWindowRect.Left;
                    if (this.SnapXEdge(window, ref left)) {
                        proposedWindowRect.X = left;
                    }
                    else {
                        var right = proposedWindowRect.Right;
                        if (this.SnapXEdge(window, ref right))
                            proposedWindowRect.X = right - proposedWindowRect.Width;
                    }

                    var top = proposedWindowRect.Top;
                    if (this.SnapYEdge(window, ref top)) {
                        proposedWindowRect.Y = top;
                    }
                    else {
                        var bottom = proposedWindowRect.Bottom;
                        if (this.SnapYEdge(window, ref bottom))
                            proposedWindowRect.Y = bottom - proposedWindowRect.Height;
                    }
                }
                else {
                    // Snap the left or right edge if it is changing, but not both.
                    if ((interactiveEdges & MdiWindowEdge.Left) != 0) {
                        System.Diagnostics.Debug.Assert((interactiveEdges & MdiWindowEdge.Right) == 0);
                        var left = proposedWindowRect.Left;
                        if (this.SnapXEdge(window, ref left)) {
                            proposedWindowRect.Width = proposedWindowRect.Right - left;
                            proposedWindowRect.X = left;
                        }
                    }
                    else if ((interactiveEdges & MdiWindowEdge.Right) != 0) {
                        System.Diagnostics.Debug.Assert((interactiveEdges & MdiWindowEdge.Left) == 0);
                        var right = proposedWindowRect.Right;
                        if (this.SnapXEdge(window, ref right))
                            proposedWindowRect.Width = right - proposedWindowRect.Left;
                    }

                    // Snap the top or bottom edge if it is changing, but not both.
                    if ((interactiveEdges & MdiWindowEdge.Top) != 0) {
                        System.Diagnostics.Debug.Assert((interactiveEdges & MdiWindowEdge.Bottom) == 0);
                        var top = proposedWindowRect.Top;
                        if (this.SnapYEdge(window, ref top)) {
                            proposedWindowRect.Height = proposedWindowRect.Bottom - top;
                            proposedWindowRect.Y = top;
                        }
                    }
                    else if ((interactiveEdges & MdiWindowEdge.Bottom) != 0) {
                        System.Diagnostics.Debug.Assert((interactiveEdges & MdiWindowEdge.Top) == 0);
                        var bottom = proposedWindowRect.Bottom;
                        if (this.SnapYEdge(window, ref bottom))
                            proposedWindowRect.Height = bottom - proposedWindowRect.Top;
                    }
                }
            }

            if (window.MinWidth > proposedWindowRect.Width) {
                if ((interactiveEdges & MdiWindowEdge.Left) != 0)
                    proposedWindowRect.X = proposedWindowRect.Right - window.MinWidth;
                proposedWindowRect.Width = window.MinWidth;
            }

            if (window.MinHeight > proposedWindowRect.Height) {
                if ((interactiveEdges & MdiWindowEdge.Top) != 0)
                    proposedWindowRect.Y = proposedWindowRect.Bottom - window.MinHeight;
                proposedWindowRect.Height = window.MinHeight;
            }

            MdiPanel.SetWindowRect(window, proposedWindowRect);
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

            if (window.View != this)
                throw new ArgumentException("Window does not belong to this view.", "window");

            window.SetValue(MdiPanel.WindowStateProperty, windowState);

            // The ZOrder property of the MdiWindow is coerced to be determined
            // by their orders in the window state lists.
            _windows.CoerceValues(System.Windows.Controls.Panel.ZIndexProperty);

            // Selection is coerced to normal/maximized windows.
            this.CoerceValue(SelectedItemProperty);
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
            this.ActivateWindow(window);
        }

        /// <summary>
        ///     Removes the window from the view.
        /// </summary>
        public void CloseWindow(MdiWindow window) {
            if (window == null)
                throw new ArgumentNullException("window");
            if (window.View != this)
                throw new ArgumentException("Window does not belong to this view.", "window");

            // Raise the window.Closing event, which can be canceled.
            if (window.Close()) {
                // Removing an item may throw if the Items are bound to the ItemsSource.
                var item = this.ItemContainerGenerator.ItemFromContainer(window);

                // This will call back into OnContainerRemoved.
                this.Items.Remove(item);

                // Walk the visual tree looking for disposable elements.
                Extensions.ElementExtensions.DisposeSubTree(window);
            }
        }

        /// <summary>
        ///     Activate the window.
        /// </summary>
        public void ActivateWindow(MdiWindow window) {
            if (window == null)
                throw new ArgumentNullException("window");

            if (window.View != this)
                throw new ArgumentException("Window does not belong to this view.", "window");

            var item = this.ItemContainerGenerator.ItemFromContainer(window);

            if (item != this.SelectedItem)
                this.SelectedItem = this.ItemContainerGenerator.ItemFromContainer(window);
        }

        /// <summary>
        ///     Activate the previous window.
        /// </summary>
        public void ActivatePreviousWindow() {
            var count = this.Items.Count;
            var currentIndex = this.SelectedIndex;

            if (currentIndex >= 0)
                for (var i = 1; i < count; i++) {
                    var candidateIndex = currentIndex - i;
                    if (candidateIndex < 0)
                        candidateIndex += count;
                    var candidateItem = this.Items[candidateIndex];
                    var candidateWindow = (MdiWindow) this.ItemContainerGenerator.ContainerFromItem(candidateItem);

                    if (MdiPanel.GetWindowState(candidateWindow) != System.Windows.WindowState.Minimized) {
                        this.ActivateWindow(candidateWindow);
                        break;
                    }
                }
        }

        /// <summary>
        ///     Activate the next window.
        /// </summary>
        public void ActivateNextWindow() {
            var count = this.Items.Count;
            var currentIndex = this.SelectedIndex;

            if (currentIndex >= 0)
                for (var i = 1; i < count; i++) {
                    var candidateItem = this.Items[(currentIndex + i) % count];
                    var candidateWindow = (MdiWindow) this.ItemContainerGenerator.ContainerFromItem(candidateItem);

                    if (MdiPanel.GetWindowState(candidateWindow) != System.Windows.WindowState.Minimized) {
                        this.ActivateWindow(candidateWindow);
                        break;
                    }
                }
        }

        /// <summary>
        ///     Minimize all windows.
        /// </summary>
        public void MinimizeAllWindows() {
            foreach (var window in _windows.ToArray()) {
                this.SetWindowState(window, System.Windows.WindowState.Minimized);
            }
        }

        /// <summary>
        ///     Maximize all windows.
        /// </summary>
        public void MaximizeAllWindows() {
            foreach (var window in _windows.ToArray()) {
                this.SetWindowState(window, System.Windows.WindowState.Maximized);
            }
        }

        /// <summary>
        ///     Restore all windows.
        /// </summary>
        /// <remarks>
        ///     Minimized windows are restored behind the other windows, and
        ///     the order of maximized windows is not changed.
        /// </remarks>
        public void RestoreAllWindows() {
            foreach (var window in _windows.ToArray()) {
                this.SetWindowState(window, System.Windows.WindowState.Normal);
            }
        }

        public void FloatWindow(MdiWindow window) {
            if (window == null)
                throw new ArgumentNullException("window");
            if (window.View != this)
                throw new ArgumentException("Window does not belong to this view.", "window");

            // Get the current window rect, relative to the MdiPanel, and
            // transform it into screen coordinates.  
            var hwndSource = System.Windows.PresentationSource.FromVisual(window) as System.Windows.Interop.HwndSource;
            var panel = (MdiPanel) System.Windows.Media.VisualTreeHelper.GetParent(window);
            var panelRect = MdiPanel.GetWindowRect(window);
            var clientRect = Extensions.PresentationSourceExtensions.TransformDescendantToClient(hwndSource, panelRect, panel);
            var screenRect = Extensions.HwndSourceExtensions.TransformClientToScreen(hwndSource, clientRect);

            // Removing an item may throw if the Items are bound to the ItemsSource.
            var item = this.ItemContainerGenerator.ItemFromContainer(window);

            // This will call back into OnContainerRemoved.
            // TODO: see bug 8582
            this.Items.Remove(item);

            // Set these screen coordinates back into the window rect property.
            // The MdiFloater works in screen coordinates.
            MdiPanel.SetWindowRect(window, screenRect);

            var floater = new MdiFloater();
            floater.Left = screenRect.Left;
            floater.Top = screenRect.Top;
            floater.Width = screenRect.Width;
            floater.Height = screenRect.Height;
            floater.Owner = hwndSource.RootVisual as System.Windows.Window;
            floater.Content = window;
            floater.Show();
        }

        /// <summary>
        ///     Tile the windows.
        /// </summary>
        public void TileWindows() {
            if (_windows.Count > 1) {
                this.RestoreAllWindows();

                var panel = (MdiPanel) System.Windows.Media.VisualTreeHelper.GetParent(_windows[0]);

                var cols = (int) Math.Ceiling(Math.Sqrt(_windows.Count));
                var rows = (int) Math.Ceiling((double) _windows.Count / cols);
                var position = new System.Windows.Point();
                var size = new System.Windows.Size(panel.RenderSize.Width / cols, panel.RenderSize.Height / rows);

                var row = 0;
                var col = 0;
                for (var i = 0; i < _windows.Count; i++) {
                    position.X = col * size.Width;
                    position.Y = row * size.Height;
                    MdiPanel.SetWindowRect(_windows[row * cols + col], new System.Windows.Rect(position, size));

                    col++;
                    if (col >= cols) {
                        col = 0;
                        row++;
                    }
                }
            }
        }

        /// <summary>
        ///     Cascade the windows.
        /// </summary>
        public void CascadeWindows() {
            if (_windows.Count > 1) {
                var panel = (MdiPanel) System.Windows.Media.VisualTreeHelper.GetParent(_windows[0]);

                var position = new System.Windows.Point();
                var size = new System.Windows.Size(panel.RenderSize.Width / 2, panel.RenderSize.Height / 2);

                this.RestoreAllWindows();
                foreach (var window in _windows) {
                    MdiPanel.SetWindowRect(window, new System.Windows.Rect(position, size));

                    position.X += 29;
                    position.Y += 29;
                }
            }
        }

        /// <summary>
        ///     Initialize the container to be in the list that its WindowState
        ///     property indicates.  Note that this is before our coercion
        ///     logic is applied.
        /// </summary>
        protected override void OnContainerAdded(MdiWindow window) {
            System.Diagnostics.Debug.Assert(window.View == null);

            _windows.Add(window);
            _windows.BringToFront(window, MdiPanel.GetWindowState(window));

            window.View = this;

            // This virtual is called before the container is fully connected,
            // so we defer the coercion until later.
            this.Dispatcher.BeginInvoke((Action) (() => this.CoerceValue(SelectedItemProperty)));
        }

        /// <summary>
        ///     Make sure the container is removed from any list that may
        ///     contain it.
        /// </summary>
        protected override void OnContainerRemoved(MdiWindow window) {
            // BUG: sometimes this gets called twice for the same window.
            // try adding a plain window, then floating it
            // first time: SelectorEx.ClearContainerForItemOverride
            // second time: SelectorEx.OnItemsCollectionChanged (removed item)
            System.Diagnostics.Debug.Assert(window.View == this);

            _windows.Remove(window);
            window.View = null;

            this.CoerceValue(SelectedItemProperty);
        }

        /// <summary>
        ///     Execute handler for the MdiCommands.MaximizeWindow command.
        /// </summary>
        private void ExecuteMaximizeWindow(System.Windows.Input.ExecutedRoutedEventArgs e) {
            var window = this.ContainerFromElement((System.Windows.DependencyObject) e.OriginalSource);
            System.Diagnostics.Debug.Assert(window != null && MdiPanel.GetWindowState(window) != System.Windows.WindowState.Maximized);

            this.MaximizeWindow(window);
        }

        /// <summary>
        ///     CanExecute handler for the MdiCommands.MaximizeWindow command.
        /// </summary>
        private void CanExecuteMaximizeWindow(System.Windows.Input.CanExecuteRoutedEventArgs e) {
            var window = this.ContainerFromElement((System.Windows.DependencyObject) e.OriginalSource);
            e.CanExecute = window != null && MdiPanel.GetWindowState(window) != System.Windows.WindowState.Maximized;
        }

        /// <summary>
        ///     Execute handler for the MdiCommands.MinimizeWindow command.
        /// </summary>
        private void ExecuteMinimizeWindow(System.Windows.Input.ExecutedRoutedEventArgs e) {
            var window = this.ContainerFromElement((System.Windows.DependencyObject) e.OriginalSource);
            System.Diagnostics.Debug.Assert(window != null && MdiPanel.GetWindowState(window) != System.Windows.WindowState.Minimized);

            this.MinimizeWindow(window);
        }

        /// <summary>
        ///     CanExecute handler for the MdiCommands.MinimizeWindow command.
        /// </summary>
        private void CanExecuteMinimizeWindow(System.Windows.Input.CanExecuteRoutedEventArgs e) {
            var window = this.ContainerFromElement((System.Windows.DependencyObject) e.OriginalSource);
            e.CanExecute = window != null && MdiPanel.GetWindowState(window) != System.Windows.WindowState.Minimized;
        }

        /// <summary>
        ///     Execute handler for the MdiCommands.RestoreWindow command.
        /// </summary>
        private void ExecuteRestoreWindow(System.Windows.Input.ExecutedRoutedEventArgs e) {
            var window = this.ContainerFromElement((System.Windows.DependencyObject) e.OriginalSource);
            System.Diagnostics.Debug.Assert(window != null && MdiPanel.GetWindowState(window) != System.Windows.WindowState.Normal);

            this.RestoreWindow(window);
        }

        /// <summary>
        ///     CanExecute handler for the MdiCommands.RestoreWindow command.
        /// </summary>
        private void CanExecuteRestoreWindow(System.Windows.Input.CanExecuteRoutedEventArgs e) {
            var window = this.ContainerFromElement((System.Windows.DependencyObject) e.OriginalSource);
            e.CanExecute = window != null && MdiPanel.GetWindowState(window) != System.Windows.WindowState.Normal;
        }

        /// <summary>
        ///     Execute handler for the MdiCommands.CloseWindow command.
        /// </summary>
        private void ExecuteCloseWindow(System.Windows.Input.ExecutedRoutedEventArgs e) {
            var window = this.ContainerFromElement((System.Windows.DependencyObject) e.OriginalSource);
            System.Diagnostics.Debug.Assert(window != null && this.ItemsSource == null && this.Items != null);

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
            var window = this.ContainerFromElement((System.Windows.DependencyObject) e.OriginalSource);
            e.CanExecute = window != null && this.ItemsSource == null && this.Items != null;
        }

        /// <summary>
        ///     Execute handler for the MdiCommands.ActivateWindow command.
        /// </summary>
        private void ExecuteActivateWindow(System.Windows.Input.ExecutedRoutedEventArgs e) {
            var window = this.ContainerFromElement((System.Windows.DependencyObject) e.OriginalSource);
            this.ActivateWindow(window);
        }

        /// <summary>
        ///     CanExecute handler for the MdiCommands.ActivateWindow command.
        /// </summary>
        private void CanExecuteActivateWindow(System.Windows.Input.CanExecuteRoutedEventArgs e) {
            e.CanExecute = true;
        }

        /// <summary>
        ///     Execute handler for the MdiCommands.ActivateNextWindow command.
        /// </summary>
        private void ExecuteActivateNextWindow(System.Windows.Input.ExecutedRoutedEventArgs e) {
            this.ActivateNextWindow();
        }

        /// <summary>
        ///     Execute handler for the MdiCommands.ActivatePreviousWindow command.
        /// </summary>
        private void ExecuteActivatePreviousWindow(System.Windows.Input.ExecutedRoutedEventArgs e) {
            this.ActivatePreviousWindow();
        }

        /// <summary>
        ///     Execute handler for the MdiCommands.AdjustWindowRect command.
        /// </summary>
        private void ExecuteAdjustWindowRect(System.Windows.Input.ExecutedRoutedEventArgs e) {
            var originalSource = (System.Windows.UIElement) e.OriginalSource;
            var swp = (AdjustWindowRectParameter) e.Parameter;

            var window = this.ContainerFromElement(originalSource);
            System.Diagnostics.Debug.Assert(window != null && MdiPanel.GetWindowState(window) == System.Windows.WindowState.Normal);

            var panel = (MdiPanel) System.Windows.Media.VisualTreeHelper.GetParent(window);
            var delta = Extensions.ElementExtensions.TransformElementToElement(originalSource, swp.Delta, panel);

            this.AdjustWindowRect(window, delta, swp.InteractiveEdges);
        }

        /// <summary>
        ///     CanExecute handler for the MdiCommands.AdjustWindowRect command.
        /// </summary>
        private void CanExecuteAdjustWindowRect(System.Windows.Input.CanExecuteRoutedEventArgs e) {
            var window = this.ContainerFromElement((System.Windows.DependencyObject) e.OriginalSource);
            e.CanExecute = window != null && MdiPanel.GetWindowState(window) == System.Windows.WindowState.Normal;
        }

        /// <summary>
        ///     Execute handler for the MdiCommands.CascadeWindows command.
        /// </summary>
        private void ExecuteCascadeWindows(System.Windows.Input.ExecutedRoutedEventArgs e) {
            System.Diagnostics.Debug.Assert(_windows.Count > 1);
            this.CascadeWindows();
        }

        /// <summary>
        ///     Execute handler for the MdiCommands.TileWindows command.
        /// </summary>
        private void ExecuteTileWindows(System.Windows.Input.ExecutedRoutedEventArgs e) {
            System.Diagnostics.Debug.Assert(_windows.Count > 1);
            this.TileWindows();
        }

        /// <summary>
        ///     CanExecute handler for any command that requires more than one window.
        /// </summary>
        private void CanExecuteCommandThatRequiresMoreThanOneWindow(System.Windows.Input.CanExecuteRoutedEventArgs e) {
            e.CanExecute = _windows.Count > 1;
        }

        /// <summary>
        ///     Execute handler for the MdiCommands.MinimizeAllWindows command.
        /// </summary>
        private void ExecuteMinimizeAllWindows(System.Windows.Input.ExecutedRoutedEventArgs e) {
            System.Diagnostics.Debug.Assert(_windows.Where(w => MdiPanel.GetWindowState(w) != System.Windows.WindowState.Minimized).Count() > 0);
            this.MinimizeAllWindows();
        }

        /// <summary>
        ///     CanExecute handler for the MdiCommands.MinimizeAllWindows command.
        /// </summary>
        private void CanExecuteMinimizeAllWindows(System.Windows.Input.CanExecuteRoutedEventArgs e) {
            e.CanExecute = _windows.Where(w => MdiPanel.GetWindowState(w) != System.Windows.WindowState.Minimized).Count() > 0;
        }

        /// <summary>
        ///     Execute handler for the MdiCommands.MaximizeAllWindows command.
        /// </summary>
        private void ExecuteMaximizeAllWindows(System.Windows.Input.ExecutedRoutedEventArgs e) {
            System.Diagnostics.Debug.Assert(_windows.Where(w => MdiPanel.GetWindowState(w) != System.Windows.WindowState.Maximized).Count() > 0);
            this.MaximizeAllWindows();
        }

        /// <summary>
        ///     CanExecute handler for the MdiCommands.MaximizeAllWindows command.
        /// </summary>
        private void CanExecuteMaximizeAllWindows(System.Windows.Input.CanExecuteRoutedEventArgs e) {
            e.CanExecute = _windows.Where(w => MdiPanel.GetWindowState(w) != System.Windows.WindowState.Maximized).Count() > 0;
        }

        /// <summary>
        ///     Execute handler for the MdiCommands.RestoreAllWindows command.
        /// </summary>
        private void ExecuteRestoreAllWindows(System.Windows.Input.ExecutedRoutedEventArgs e) {
            System.Diagnostics.Debug.Assert(_windows.Where(w => MdiPanel.GetWindowState(w) != System.Windows.WindowState.Normal).Count() > 0);
            this.RestoreAllWindows();
        }

        /// <summary>
        ///     CanExecute handler for the MdiCommands.RestoreAllWindows command.
        /// </summary>
        private void CanExecuteRestoreAllWindows(System.Windows.Input.CanExecuteRoutedEventArgs e) {
            e.CanExecute = _windows.Where(w => MdiPanel.GetWindowState(w) != System.Windows.WindowState.Normal).Count() > 0;
        }

        /// <summary>
        ///     Execute handler for the MdiCommands.FloatWindow command.
        /// </summary>
        private void ExecuteFloatWindow(System.Windows.Input.ExecutedRoutedEventArgs e) {
            var window = this.ContainerFromElement((System.Windows.DependencyObject) e.OriginalSource);
            System.Diagnostics.Debug.Assert(window != null && MdiPanel.GetWindowState(window) == System.Windows.WindowState.Normal);

            this.FloatWindow(window);
        }

        /// <summary>
        ///     CanExecute handler for the MdiCommands.FloatWindow command.
        /// </summary>
        private void CanExecuteFloatWindow(System.Windows.Input.CanExecuteRoutedEventArgs e) {
            var window = this.ContainerFromElement((System.Windows.DependencyObject) e.OriginalSource);
            e.CanExecute = window != null && MdiPanel.GetWindowState(window) == System.Windows.WindowState.Normal;
        }

        private bool SnapYEdge(MdiWindow window, ref double y) {
            foreach (var otherWindow in _windows) {
                // Skip the window itself.
                if (otherWindow == window)
                    continue;

                // Skip any child that is not in the normal state.
                var windowState = MdiPanel.GetWindowState(otherWindow);
                if (windowState != System.Windows.WindowState.Normal)
                    continue;

                var windowRect = MdiPanel.GetWindowRect(otherWindow);
                if (Math.Abs(windowRect.Top - y) < this.SnapThreshold) {
                    // Snap to the top edge of the child.
                    y = windowRect.Top;
                    return true;
                }

                if (Math.Abs(windowRect.Top + otherWindow.RenderSize.Height - y) < this.SnapThreshold) {
                    // Snap to the bottom edge of the child.
                    y = windowRect.Top + otherWindow.RenderSize.Height;
                    return true;
                }
            }

            return false;
        }

        private bool SnapXEdge(MdiWindow window, ref double x) {
            foreach (var otherWindow in _windows) {
                // Skip the window itself.
                if (otherWindow == window)
                    continue;

                // Skip any child that is not in the normal state.
                var windowState = MdiPanel.GetWindowState(otherWindow);
                if (windowState != System.Windows.WindowState.Normal)
                    continue;

                var windowRect = MdiPanel.GetWindowRect(otherWindow);
                if (Math.Abs(windowRect.Left - x) < this.SnapThreshold) {
                    // Snap to the left edge of the child.
                    x = windowRect.Left;
                    return true;
                }

                if (Math.Abs(windowRect.Left + otherWindow.RenderSize.Width - x) < this.SnapThreshold) {
                    // Snap to the right edge of the child.
                    x = windowRect.Left + otherWindow.RenderSize.Width;
                    return true;
                }
            }

            return false;
        }

        private object OnCoerceSelectedItem(object baseValue) {
            var selectedItem = baseValue;
            MdiWindow selectedWindow = null;
            if (selectedItem != null) {
                selectedWindow = (MdiWindow) this.ItemContainerGenerator.ContainerFromItem(selectedItem);
                if (selectedWindow != null && MdiPanel.GetWindowState(selectedWindow) == System.Windows.WindowState.Minimized)
                    selectedWindow = null;
            }

            // If selection is not valid, or null, try to select a non-minimized window.
            if (selectedWindow == null)
                selectedWindow = _windows.Where(w => MdiPanel.GetWindowState(w) != System.Windows.WindowState.Minimized).LastOrDefault();

            if (selectedWindow != null)
                selectedItem = this.ItemContainerGenerator.ItemFromContainer(selectedWindow);
            else
                selectedItem = null;

            return selectedItem;
        }

        private void OnSelectedItemChanged(System.Windows.DependencyPropertyChangedEventArgs e) {
            var selection = e.NewValue;
            if (selection != null) {
                var window = (MdiWindow) this.ItemContainerGenerator.ContainerFromItem(e.NewValue);

                // Bring the newly activated window to the front.
                _windows.BringToFront(window, MdiPanel.GetWindowState(window));
                _windows.CoerceValues(System.Windows.Controls.Panel.ZIndexProperty);

                // Give the newly activated window focus.
                window.Focus();
            }
        }

        private readonly MdiWindowCollection _windows = new MdiWindowCollection();

        #region ContentAirspaceMode

        public static readonly System.Windows.DependencyProperty ContentAirspaceModeProperty = System.Windows.DependencyProperty.Register(
            /* Name:                */ "ContentAirspaceMode",
            /* Value Type:          */ typeof(AirspaceMode),
            /* Owner Type:          */ typeof(MdiView),
            /* Metadata:            */ new System.Windows.FrameworkPropertyMetadata(
                /*     Default Value:   */ AirspaceMode.None));

        /// <summary>
        ///     Whether or not the content should be clipped.
        /// </summary>
        /// <remarks>
        ///     In the default style, we use an HwndClipper to enforce this property.
        /// </remarks>
        public AirspaceMode ContentAirspaceMode {
            get => (AirspaceMode) this.GetValue(ContentAirspaceModeProperty);
            set => this.SetValue(ContentAirspaceModeProperty, value);
        }

        #endregion

        #region ContentClippingBackground

        public static readonly System.Windows.DependencyProperty ContentClippingBackgroundProperty = System.Windows.DependencyProperty.Register(
            /* Name:                 */ "ContentClippingBackground",
            /* Value Type:           */ typeof(System.Windows.Media.Brush),
            /* Owner Type:           */ typeof(MdiView),
            /* Metadata:             */ new System.Windows.FrameworkPropertyMetadata(
                /*     Default Value:    */ null));

        /// <summary>
        ///     The brush to paint the background when the airspace mode is
        ///     set to clipping.
        /// </summary>
        public System.Windows.Media.Brush ContentClippingBackground {
            get => (System.Windows.Media.Brush) this.GetValue(ContentClippingBackgroundProperty);
            set => this.SetValue(ContentClippingBackgroundProperty, value);
        }

        #endregion

        #region ContentClippingCopyBitsBehavior

        public static readonly System.Windows.DependencyProperty ContentClippingCopyBitsBehaviorProperty = System.Windows.DependencyProperty.Register(
            /* Name:                 */ "ContentClippingCopyBitsBehavior",
            /* Value Type:           */ typeof(CopyBitsBehavior),
            /* Owner Type:           */ typeof(MdiView),
            /* Metadata:             */ new System.Windows.FrameworkPropertyMetadata(
                /*     Default Value:    */ CopyBitsBehavior.Default));

        /// <summary>
        ///     The behavior of copying bits when the airspace mode is set to
        ///     clipping.
        /// </summary>
        public CopyBitsBehavior ContentClippingCopyBitsBehavior {
            get => (CopyBitsBehavior) this.GetValue(ContentClippingCopyBitsBehaviorProperty);
            set => this.SetValue(ContentClippingCopyBitsBehaviorProperty, value);
        }

        #endregion

        #region ContentRedirectionVisibility

        public static readonly System.Windows.DependencyProperty ContentRedirectionVisibilityProperty = System.Windows.DependencyProperty.Register(
            /* Name:                 */ "ContentRedirectionVisibility",
            /* Value Type:           */ typeof(RedirectionVisibility),
            /* Owner Type:           */ typeof(MdiView),
            /* Metadata:             */ new System.Windows.FrameworkPropertyMetadata(
                /*     Default Value:    */ RedirectionVisibility.Hidden));

        /// <summary>
        ///     The visibility of the redirection surface.
        /// </summary>
        public RedirectionVisibility ContentRedirectionVisibility {
            get => (RedirectionVisibility) this.GetValue(ContentRedirectionVisibilityProperty);
            set => this.SetValue(ContentRedirectionVisibilityProperty, value);
        }

        #endregion

        #region ContentIsOutputRedirectionEnabled

        public static readonly System.Windows.DependencyProperty ContentIsOutputRedirectionEnabledProperty = System.Windows.DependencyProperty.Register(
            /* Name:                 */ "ContentIsOutputRedirectionEnabled",
            /* Value Type:           */ typeof(bool),
            /* Owner Type:           */ typeof(MdiView),
            /* Metadata:             */ new System.Windows.FrameworkPropertyMetadata(
                /*     Default Value:    */ false));

        /// <summary>
        ///     Whether or not output redirection is enabled.
        /// </summary>
        public bool ContentIsOutputRedirectionEnabled {
            get => (bool) this.GetValue(ContentIsOutputRedirectionEnabledProperty);
            set => this.SetValue(ContentIsOutputRedirectionEnabledProperty, value);
        }

        #endregion

        #region ContentOutputRedirectionPeriod

        public static readonly System.Windows.DependencyProperty ContentOutputRedirectionPeriodProperty = System.Windows.DependencyProperty.Register(
            /* Name:                 */ "ContentOutputRedirectionPeriod",
            /* Value Type:           */ typeof(TimeSpan),
            /* Owner Type:           */ typeof(MdiView),
            /* Metadata:             */ new System.Windows.FrameworkPropertyMetadata(
                /*     Default Value:    */ TimeSpan.FromMilliseconds(30)));

        /// <summary>
        ///     The period of time to update the output redirection.
        /// </summary>
        public TimeSpan ContentOutputRedirectionPeriod {
            get => (TimeSpan) this.GetValue(ContentOutputRedirectionPeriodProperty);
            set => this.SetValue(ContentOutputRedirectionPeriodProperty, value);
        }

        #endregion

        #region ContentIsInputRedirectionEnabled

        public static readonly System.Windows.DependencyProperty ContentIsInputRedirectionEnabledProperty = System.Windows.DependencyProperty.Register(
            /* Name:                 */ "ContentIsInputRedirectionEnabled",
            /* Value Type:           */ typeof(bool),
            /* Owner Type:           */ typeof(MdiView),
            /* Metadata:             */ new System.Windows.FrameworkPropertyMetadata(
                /*     Default Value:    */ false));

        /// <summary>
        ///     Whether or not input redirection is enabled.
        /// </summary>
        public bool ContentIsInputRedirectionEnabled {
            get => (bool) this.GetValue(ContentIsInputRedirectionEnabledProperty);
            set => this.SetValue(ContentIsInputRedirectionEnabledProperty, value);
        }

        #endregion

        #region ContentInputRedirectionPeriod

        public static readonly System.Windows.DependencyProperty ContentInputRedirectionPeriodProperty = System.Windows.DependencyProperty.Register(
            /* Name:                 */ "ContentInputRedirectionPeriod",
            /* Value Type:           */ typeof(TimeSpan),
            /* Owner Type:           */ typeof(MdiView),
            /* Metadata:             */ new System.Windows.FrameworkPropertyMetadata(
                /*     Default Value:    */ TimeSpan.FromMilliseconds(30)));

        /// <summary>
        ///     The period of time to update the input redirection.
        /// </summary>
        public TimeSpan ContentInputRedirectionPeriod {
            get => (TimeSpan) this.GetValue(ContentInputRedirectionPeriodProperty);
            set => this.SetValue(ContentInputRedirectionPeriodProperty, value);
        }

        #endregion

        #region EnableSnapping

        /// <summary>
        ///     A dependency property indicating whether or not the window
        ///     rect of a window in the view should be snapped to the
        ///     edges of the other children during interactive operations.
        /// </summary>
        public static System.Windows.DependencyProperty EnableSnappingProperty = System.Windows.DependencyProperty.Register(
            /* Name:              */ "EnableSnapping",
            /* Value Type:        */ typeof(bool),
            /* Owner Type:        */ typeof(MdiView),
            /* Metadata:          */ new System.Windows.FrameworkPropertyMetadata(
                /*     Default Value: */ false));

        /// <summary>
        ///     Whether or not the window rect of a window in the view
        ///     should be snapped to the edges of the other children during
        ///     interactive operations.
        /// </summary>
        public bool EnableSnapping {
            get => (bool) this.GetValue(EnableSnappingProperty);
            set => this.SetValue(EnableSnappingProperty, value);
        }

        #endregion

        #region SnapThreshold

        /// <summary>
        ///     A dependency property indicating the threshold to use when
        ///     snapping the window rect of a window in the view.
        /// </summary>
        public static System.Windows.DependencyProperty SnapThresholdProperty = System.Windows.DependencyProperty.Register(
            /* Name:                */ "SnapThreshold",
            /* Value Type:          */ typeof(double),
            /* Owner Type:          */ typeof(MdiView),
            /* Metadata:            */ new System.Windows.FrameworkPropertyMetadata(
                /*     Default Value:   */ 10.0));

        /// <summary>
        ///     The threshold to use when snapping the window rect of a
        ///     window in the view.
        /// </summary>
        public double SnapThreshold {
            get => (double) this.GetValue(SnapThresholdProperty);
            set => this.SetValue(SnapThresholdProperty, value);
        }

        #endregion
    }
}