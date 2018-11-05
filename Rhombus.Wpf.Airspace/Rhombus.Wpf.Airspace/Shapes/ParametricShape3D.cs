using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Shapes {
    /// <summary>
    ///     ParametricShape3D assumes responsibility for generating the
    ///     mesh for all parametric surfaces.  This is done by sampling the
    ///     UV plane at regular intervals, projecting these UV points into
    ///     3D space by calling a virtual method that derived types can
    ///     implement, and then finally constructing triangles between the
    ///     points.
    ///     Texture coordinates are simply the (u,v) coordinates scaled
    ///     between [0,1].
    ///     Normals are calculated in one of two ways:
    ///     1) Faceted
    ///     The number of triangles is doubled, and each vertex of a
    ///     given triangle has the same normal, which is the "face"
    ///     normal.
    ///     2) Smooth
    ///     There is one triangle for every 3 points, and each vertex
    ///     has a different vertex, calculates as the average of the
    ///     adjacent "face" normals.
    /// </summary>
    public abstract class ParametricShape3D : Shape3D {
        public static System.Windows.DependencyProperty MinUProperty = System.Windows.DependencyProperty.Register("MinU", typeof(double), typeof(ParametricShape3D), new System.Windows.PropertyMetadata(0.0, Shape3D.OnPropertyChangedAffectsModel));

        public static System.Windows.DependencyProperty MaxUProperty = System.Windows.DependencyProperty.Register("MaxU", typeof(double), typeof(ParametricShape3D), new System.Windows.PropertyMetadata(Math.PI * 2, Shape3D.OnPropertyChangedAffectsModel));

        public static System.Windows.DependencyProperty DivUProperty = System.Windows.DependencyProperty.Register("DivU", typeof(int), typeof(ParametricShape3D), new System.Windows.PropertyMetadata(32, Shape3D.OnPropertyChangedAffectsModel));

        public static System.Windows.DependencyProperty MinVProperty = System.Windows.DependencyProperty.Register("MinV", typeof(double), typeof(ParametricShape3D), new System.Windows.PropertyMetadata(0.0, Shape3D.OnPropertyChangedAffectsModel));

        public static System.Windows.DependencyProperty MaxVProperty = System.Windows.DependencyProperty.Register("MaxV", typeof(double), typeof(ParametricShape3D), new System.Windows.PropertyMetadata(Math.PI, Shape3D.OnPropertyChangedAffectsModel));

        public static System.Windows.DependencyProperty DivVProperty = System.Windows.DependencyProperty.Register("DivV", typeof(int), typeof(ParametricShape3D), new System.Windows.PropertyMetadata(32, Shape3D.OnPropertyChangedAffectsModel));

        static ParametricShape3D() {
            FrontMaterialProperty.OverrideMetadata(typeof(ParametricShape3D), new System.Windows.PropertyMetadata(new Media.Shape3DMaterial(new Func<Shape3D, System.Windows.UIElement>(ParametricShape3D.GetDefaultFrontMaterial))));

            BackMaterialProperty.OverrideMetadata(typeof(ParametricShape3D), new System.Windows.PropertyMetadata(new Media.Shape3DMaterial(new Func<Shape3D, System.Windows.UIElement>(ParametricShape3D.GetDefaultBackMaterial))));
        }

        public double MinU {
            get => (double) this.GetValue(MinUProperty);
            set => this.SetValue(MinUProperty, value);
        }

        public double MaxU {
            get => (double) this.GetValue(MaxUProperty);
            set => this.SetValue(MaxUProperty, value);
        }

        public int DivU {
            get => (int) this.GetValue(DivUProperty);
            set => this.SetValue(DivUProperty, value);
        }

        public double MinV {
            get => (double) this.GetValue(MinVProperty);
            set => this.SetValue(MinVProperty, value);
        }

        public double MaxV {
            get => (double) this.GetValue(MaxVProperty);
            set => this.SetValue(MaxVProperty, value);
        }

        public int DivV {
            get => (int) this.GetValue(DivVProperty);
            set => this.SetValue(DivVProperty, value);
        }

        protected abstract System.Windows.Media.Media3D.Point3D Project(Numerics.MemoizeMath u, Numerics.MemoizeMath v);

        protected sealed override System.Windows.Media.Media3D.MeshGeometry3D GetGeometry() {
            var minU = this.MinU;
            var maxU = this.MaxU;
            var divU = this.DivU;
            var minV = this.MinV;
            var maxV = this.MaxV;
            var divV = this.DivV;

            var spanU = maxU - minU;
            var spanV = maxV - minV;
            var stride = divU + 1;

            var mesh = new System.Windows.Media.Media3D.MeshGeometry3D();

            // Create memoized wrappers for each of the u and v divisions.
            // This is a massive performance improvement in time complexity.
            var uDivisions = new List<Numerics.MemoizeMath>();
            for (var iU = 0; iU <= divU; iU++) {
                var u = spanU * iU / divU + minU;
                uDivisions.Add(new Numerics.MemoizeMath(u));
            }

            var vDivisions = new List<Numerics.MemoizeMath>();
            for (var iV = 0; iV <= divV; iV++) {
                var v = spanV * iV / divV + minV;
                vDivisions.Add(new Numerics.MemoizeMath(v));
            }

            // Iterate through the (u,v) space and sample the parametric surface.
            foreach (var v in vDivisions) {
                foreach (var u in uDivisions) {
                    // Project the (u,v) points into 3D space.
                    var position = this.Project(u, v);
                    mesh.Positions.Add(position);
                }
            }

            // Assign texture coordinates based on (u,v) position such
            // that they are in the range of [0,1].
            for (var iV = 0; iV <= divV; iV++) {
                for (var iU = 0; iU <= divU; iU++) {
                    var tu = iU / (double) divU;
                    var tv = iV / (double) divV;
                    mesh.TextureCoordinates.Add(new System.Windows.Point(tu, tv));
                }
            }

            // Iterate through the positions in the mesh updating related properties.
            double maxQuadWidth = 0;
            double maxQuadHeight = 0;
            var iPosition = 0;
            var isNWSE = false;

            for (var iV = 0; iV < divV; iV++) {
                var oldIsNWSE = isNWSE;

                for (var iU = 0; iU < divU; iU++) {
                    // Get the width & height of this quad, keep track of the
                    // max width and height of each quad.
                    var uStep = mesh.Positions[iPosition + 1] - mesh.Positions[iPosition];
                    var vStep = mesh.Positions[iPosition + stride] - mesh.Positions[iPosition];
                    maxQuadWidth = Math.Max(uStep.Length, maxQuadWidth);
                    maxQuadHeight = Math.Max(vStep.Length, maxQuadHeight);

                    // Process each quad in the (u,v) grid into two triangles.
                    var quad = new Tuple<int, int, int, int>(iPosition, iPosition + stride, iPosition + stride + 1, iPosition + 1);
                    Tuple<int, int, int> tri1, tri2;
                    ParametricShape3D.GetTrianglesFromQuad(quad, isNWSE, out tri1, out tri2);

                    Extensions.Int32CollectionExtensions.Add(mesh.TriangleIndices, tri1);
                    Extensions.Int32CollectionExtensions.Add(mesh.TriangleIndices, tri2);

                    // Calculate normals...TODO

                    iPosition++;
                    isNWSE = !isNWSE;
                }

                iPosition++;
                isNWSE = !oldIsNWSE;
            }

            // Calculate the effective size of the surface based on the maximum
            // quad size.  This represents a surface that will have full
            // fidelity at the most distorted portions of the mesh.  The
            // portions of the mesh with the least distortion will have more
            // texture to sample from than needed.  Unfortunately, BitmapCache
            // does not support mip-mapping so this can lead to some speckles.
            //
            // TODO: register a default value callback...
            this.SurfaceWidth = maxQuadWidth * divU;
            this.SurfaceHeight = maxQuadHeight * divU;

            //mesh.Freeze();

            return mesh;
        }

        private static void GetTrianglesFromQuad(Tuple<int, int, int, int> quad, bool isNWSE, out Tuple<int, int, int> tri1, out Tuple<int, int, int> tri2) {
            if (isNWSE) {
                tri1 = new Tuple<int, int, int>(quad.Item1, quad.Item2, quad.Item3);
                tri2 = new Tuple<int, int, int>(quad.Item3, quad.Item4, quad.Item1);
            }
            else {
                tri1 = new Tuple<int, int, int>(quad.Item4, quad.Item1, quad.Item2);
                tri2 = new Tuple<int, int, int>(quad.Item2, quad.Item3, quad.Item4);
            }
        }

        private static System.Windows.UIElement GetDefaultFrontMaterial(Shape3D shape) {
            var _this = (ParametricShape3D) shape;
            return _this.GetDefaultMaterial(true);
        }

        private static System.Windows.UIElement GetDefaultBackMaterial(Shape3D shape) {
            var _this = (ParametricShape3D) shape;
            return _this.GetDefaultMaterial(false);
        }

        private System.Windows.UIElement GetDefaultMaterial(bool front) {
            var grid = new System.Windows.Controls.Grid();

            var divV = this.DivV;
            for (var iV = 0; iV < divV; iV++) {
                var row = new System.Windows.Controls.RowDefinition();
                row.Height = new System.Windows.GridLength(1, System.Windows.GridUnitType.Star);
                grid.RowDefinitions.Add(row);
            }

            var divU = this.DivU;
            for (var iU = 0; iU < divU; iU++) {
                var col = new System.Windows.Controls.ColumnDefinition();
                col.Width = new System.Windows.GridLength(1, System.Windows.GridUnitType.Star);
                grid.ColumnDefinitions.Add(col);
            }

            var useColorAtRowStart = true;
            bool useColor;
            for (var iV = 0; iV < divV; iV++) {
                useColor = useColorAtRowStart;
                useColorAtRowStart = !useColorAtRowStart;

                for (var iU = 0; iU < divU; iU++) {
                    System.Windows.UIElement thing;

                    var b = new System.Windows.Controls.Border();
                    b.Background = useColor
                        ? (front
                            ? System.Windows.Media.Brushes.Red
                            : System.Windows.Media.Brushes.AliceBlue)
                        : System.Windows.Media.Brushes.Black;
                    //b.BorderThickness = new Thickness(1.0);
                    //b.BorderBrush = Brushes.Black;

                    thing = b;

                    System.Windows.Controls.Grid.SetRow(thing, iV);
                    System.Windows.Controls.Grid.SetColumn(thing, iU);
                    grid.Children.Add(thing);

                    useColor = !useColor;
                }
            }

            return grid;
        }
    }
}