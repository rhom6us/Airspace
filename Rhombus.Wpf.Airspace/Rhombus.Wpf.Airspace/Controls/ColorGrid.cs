using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Controls {
    /// <summary>
    ///     A very simply element that displays a grid of colored
    ///     cells.  The only input is the number of cells.  The
    ///     colors are randomly generated.
    /// </summary>
    public class ColorGrid : System.Windows.FrameworkElement {
        public static System.Windows.DependencyProperty NumberOfCellsProperty = System.Windows.DependencyProperty.Register(
            /* Name:                 */ "NumberOfCells",
            /* Value Type:           */ typeof(int),
            /* Owner Type:           */ typeof(ColorGrid),
            /* Metadata:             */ new System.Windows.FrameworkPropertyMetadata(
                /*     Default Value:    */ 1,
                /*     Flags:            */ System.Windows.FrameworkPropertyMetadataOptions.AffectsMeasure,
                /*     Property Changed: */ (d, e) => ((ColorGrid) d).NumberOfCells_PropertyChanged(e)));

        public int NumberOfCells {
            get => (int) this.GetValue(NumberOfCellsProperty);
            set => this.SetValue(NumberOfCellsProperty, value);
        }

        protected override int VisualChildrenCount => _uniformGrid != null
            ? 1
            : 0;

        private void NumberOfCells_PropertyChanged(System.Windows.DependencyPropertyChangedEventArgs e) {
            if (_uniformGrid != null)
                this.RemoveVisualChild(_uniformGrid);

            var newValue = (int) e.NewValue;
            _uniformGrid = ColorGrid.BuildColorGrid(newValue);

            if (_uniformGrid != null)
                this.AddVisualChild(_uniformGrid);
        }

        private static System.Windows.Controls.Primitives.UniformGrid BuildColorGrid(int numberOfCells) {
            var r = new Random();

            var grid = new System.Windows.Controls.Primitives.UniformGrid();
            for (var i = 0; i < numberOfCells; i++) {
                var color = System.Windows.Media.Color.FromScRgb(1.0f, (float) r.NextDouble(), (float) r.NextDouble(), (float) r.NextDouble());
                var fill = new System.Windows.Media.SolidColorBrush(color);
                fill.Freeze();

                var child = new System.Windows.Shapes.Rectangle();
                child.Fill = fill;

                grid.Children.Add(child);
            }

            return grid;
        }

        protected override System.Windows.Media.Visual GetVisualChild(int index) {
            if (_uniformGrid == null || index != 0)
                throw new ArgumentOutOfRangeException("index");
            return _uniformGrid;
        }

        protected override System.Windows.Size MeasureOverride(System.Windows.Size constraint) {
            if (_uniformGrid != null) {
                _uniformGrid.Measure(constraint);
                return _uniformGrid.DesiredSize;
            }

            return new System.Windows.Size();
        }

        protected override System.Windows.Size ArrangeOverride(System.Windows.Size arrangeSize) {
            if (_uniformGrid != null)
                _uniformGrid.Arrange(new System.Windows.Rect(arrangeSize));
            return arrangeSize;
        }

        private System.Windows.Controls.Primitives.UniformGrid _uniformGrid;
    }
}