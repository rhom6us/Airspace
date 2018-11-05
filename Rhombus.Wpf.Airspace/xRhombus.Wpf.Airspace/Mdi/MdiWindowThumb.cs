using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Mdi {
    public class MdiWindowThumb : System.Windows.Controls.Primitives.Thumb {
        /// <summary>
        ///     The edge that the thumb should interact with.
        /// </summary>
        public static System.Windows.DependencyProperty InteractiveEdgesProperty = System.Windows.DependencyProperty.Register(
            /* Name:                 */ "InteractiveEdges",
            /* Value Type:           */ typeof(MdiWindowEdge),
            /* Owner Type:           */ typeof(MdiWindowThumb),
            /* Metadata:             */ new System.Windows.FrameworkPropertyMetadata(
                /*     Default Value:    */ MdiWindowEdge.None,
                /*     Change Callback:  */ (s, e) => ((MdiWindowThumb) s).OnInteractiveEdgesChanged(e)));

        /// <summary>
        ///     The command to raise when the thumb is double clicked.
        /// </summary>
        public static System.Windows.DependencyProperty DoubleClickCommandProperty = System.Windows.DependencyProperty.Register(
            /* Name:                 */ "DoubleClickCommand",
            /* Value Type:           */ typeof(System.Windows.Input.RoutedCommand),
            /* Owner Type:           */ typeof(MdiWindowThumb),
            /* Metadata:             */ new System.Windows.FrameworkPropertyMetadata(
                /*     Default Value:    */ null));

        static MdiWindowThumb() {
            // Look up the style for this control by using its type as its key.
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MdiWindowThumb), new System.Windows.FrameworkPropertyMetadata(typeof(MdiWindowThumb)));

            System.Windows.EventManager.RegisterClassHandler(typeof(MdiWindowThumb), DragDeltaEvent, (System.Windows.Controls.Primitives.DragDeltaEventHandler) ((s, e) => ((MdiWindowThumb) s).OnDragDelta(e)));

            CursorProperty.OverrideMetadata(
                /* Type:                 */ typeof(MdiWindowThumb),
                /* Metadata:             */ new System.Windows.FrameworkPropertyMetadata(
                    /*     Default Value:    */ System.Windows.Input.Cursors.Arrow,
                    /*     Changed Callback: */ delegate { },
                    /*     Coerce Callback:  */ (d, v) => ((MdiWindowThumb) d).OnCoerceCursor(v)));
        }

        /// <summary>
        ///     The edge that the thumb should interact with.
        /// </summary>
        public MdiWindowEdge InteractiveEdges {
            get => (MdiWindowEdge) this.GetValue(InteractiveEdgesProperty);
            set => this.SetValue(InteractiveEdgesProperty, value);
        }

        /// <summary>
        ///     The command to raise when the thumb is double clicked.
        /// </summary>
        public System.Windows.Input.RoutedCommand DoubleClickCommand {
            get => (System.Windows.Input.RoutedCommand) this.GetValue(DoubleClickCommandProperty);
            set => this.SetValue(DoubleClickCommandProperty, value);
        }

        protected override void OnMouseDoubleClick(System.Windows.Input.MouseButtonEventArgs e) {
            if (this.DoubleClickCommand != null)
                this.DoubleClickCommand.Execute(null, this);

            base.OnMouseDoubleClick(e);
        }

        private void OnDragDelta(System.Windows.Controls.Primitives.DragDeltaEventArgs e) {
            var swp = new AdjustWindowRectParameter();
            swp.InteractiveEdges = this.InteractiveEdges;
            swp.Delta = new System.Windows.Vector(e.HorizontalChange, e.VerticalChange);

            MdiCommands.AdjustWindowRect.Execute(swp, this);
        }

        private void OnInteractiveEdgesChanged(System.Windows.DependencyPropertyChangedEventArgs e) {
            this.CoerceValue(CursorProperty);
        }

        private object OnCoerceCursor(object baseValue) {
            var cursor = (System.Windows.Input.Cursor) baseValue;

            // Only coerce the default value.
            var vs = System.Windows.DependencyPropertyHelper.GetValueSource(this, CursorProperty);
            if (vs.BaseValueSource == System.Windows.BaseValueSource.Default)
                switch (this.InteractiveEdges) {
                    case MdiWindowEdge.None:
                        cursor = System.Windows.Input.Cursors.Arrow;
                        break;

                    case MdiWindowEdge.Left:
                    case MdiWindowEdge.Right:
                        cursor = System.Windows.Input.Cursors.SizeWE;
                        break;

                    case MdiWindowEdge.Top:
                    case MdiWindowEdge.Bottom:
                        cursor = System.Windows.Input.Cursors.SizeNS;
                        break;

                    case MdiWindowEdge.Left | MdiWindowEdge.Top:
                    case MdiWindowEdge.Right | MdiWindowEdge.Bottom:
                        cursor = System.Windows.Input.Cursors.SizeNWSE;
                        break;

                    case MdiWindowEdge.Left | MdiWindowEdge.Bottom:
                    case MdiWindowEdge.Right | MdiWindowEdge.Top:
                        cursor = System.Windows.Input.Cursors.SizeNESW;
                        break;

                    default:
                        cursor = System.Windows.Input.Cursors.Help;
                        break;
                }

            return cursor;
        }
    }
}