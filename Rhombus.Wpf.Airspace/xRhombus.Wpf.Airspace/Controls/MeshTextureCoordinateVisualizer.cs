using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Controls {
    /// <summary>
    ///     Renders lines connecting the texture coordinates for each triangle
    ///     in a mesh.  This can be rendered on top of the texture to show the
    ///     triangular mesh.
    /// </summary>
    internal class MeshTextureCoordinateVisualizer : System.Windows.FrameworkElement {
        public static readonly System.Windows.DependencyProperty MyPropertyProperty = System.Windows.DependencyProperty.Register("Mesh", typeof(System.Windows.Media.Media3D.MeshGeometry3D), typeof(MeshTextureCoordinateVisualizer), new System.Windows.FrameworkPropertyMetadata(null, System.Windows.FrameworkPropertyMetadataOptions.AffectsRender));

        public System.Windows.Media.Media3D.MeshGeometry3D Mesh {
            get => (System.Windows.Media.Media3D.MeshGeometry3D) this.GetValue(MyPropertyProperty);
            set => this.SetValue(MyPropertyProperty, value);
        }

        protected override System.Windows.Size MeasureOverride(System.Windows.Size availableSize) {
            return base.MeasureOverride(availableSize);
        }

        protected override System.Windows.Size ArrangeOverride(System.Windows.Size finalSize) {
            return base.ArrangeOverride(finalSize);
        }

        protected override void OnRender(System.Windows.Media.DrawingContext drawingContext) {
            var width = this.RenderSize.Width;
            var height = this.RenderSize.Height;
            var mesh = this.Mesh;

            if (mesh != null) {
                var pen = new System.Windows.Media.Pen(System.Windows.Media.Brushes.Black, 1.0);

                var numTriangles = mesh.TriangleIndices.Count / 3;

                for (var i = 0; i < numTriangles; i++) {
                    MeshTextureCoordinateVisualizer.DrawTriangle(drawingContext, pen, mesh.TextureCoordinates[mesh.TriangleIndices[i * 3]], mesh.TextureCoordinates[mesh.TriangleIndices[i * 3 + 1]], mesh.TextureCoordinates[mesh.TriangleIndices[i * 3 + 2]], width, height);
                }
            }

            base.OnRender(drawingContext);
        }

        private static void DrawTriangle(System.Windows.Media.DrawingContext drawingContext, System.Windows.Media.Pen pen, System.Windows.Point a, System.Windows.Point b, System.Windows.Point c, double width, double height) {
            var ta = new System.Windows.Point(a.X * width, a.Y * height);
            var tb = new System.Windows.Point(b.X * width, b.Y * height);
            var tc = new System.Windows.Point(c.X * width, c.Y * height);

            drawingContext.DrawLine(pen, ta, tb);
            drawingContext.DrawLine(pen, tb, tc);
            drawingContext.DrawLine(pen, tc, ta);
        }
    }
}