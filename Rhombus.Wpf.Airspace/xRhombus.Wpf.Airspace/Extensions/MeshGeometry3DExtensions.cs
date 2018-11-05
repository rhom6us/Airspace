using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Extensions {
    public static class MeshGeometry3DExtensions {
        public static IEnumerable<Tuple<System.Windows.Media.Media3D.Point3D, System.Windows.Media.Media3D.Point3D, System.Windows.Media.Media3D.Point3D>> GetTrianglePositions(this System.Windows.Media.Media3D.MeshGeometry3D _this) {
            var numTriangles = _this.TriangleIndices.Count / 3;

            for (var i = 0; i < numTriangles; i++) {
                var j = i * 3;
                yield return new Tuple<System.Windows.Media.Media3D.Point3D, System.Windows.Media.Media3D.Point3D, System.Windows.Media.Media3D.Point3D>(_this.Positions[_this.TriangleIndices[j + 0]], _this.Positions[_this.TriangleIndices[j + 1]], _this.Positions[_this.TriangleIndices[j + 2]]);
            }
        }

        public static IEnumerable<Tuple<int, int, int>> GetTriangleIndices(this System.Windows.Media.Media3D.MeshGeometry3D _this) {
            var numTriangles = _this.TriangleIndices.Count / 3;

            for (var i = 0; i < numTriangles; i++) {
                var j = i * 3;
                yield return new Tuple<int, int, int>(_this.TriangleIndices[j + 0], _this.TriangleIndices[j + 1], _this.TriangleIndices[j + 2]);
            }
        }

        public static IEnumerable<Tuple<int, int>> GetTriangleEdgeIndices(this System.Windows.Media.Media3D.MeshGeometry3D _this) {
            var numTriangles = _this.TriangleIndices.Count / 3;

            for (var i = 0; i < numTriangles; i++) {
                var j = i * 3;
                yield return new Tuple<int, int>(_this.TriangleIndices[j + 0], _this.TriangleIndices[j + 1]);
                yield return new Tuple<int, int>(_this.TriangleIndices[j + 1], _this.TriangleIndices[j + 2]);
                yield return new Tuple<int, int>(_this.TriangleIndices[j + 2], _this.TriangleIndices[j + 0]);
            }
        }

        public static System.Windows.Size CalculateIdealTextureSize(this System.Windows.Media.Media3D.MeshGeometry3D _this) {
            var size = new System.Windows.Size();

            foreach (var edgeIndices in _this.GetTriangleEdgeIndices()) {
                var edgeTexture = _this.TextureCoordinates[edgeIndices.Item1] - _this.TextureCoordinates[edgeIndices.Item2];
                var edgeModel = _this.Positions[edgeIndices.Item1] - _this.Positions[edgeIndices.Item2];
                var scale = edgeModel.Length / edgeTexture.Length;

                var widthModel = edgeTexture.X * scale / edgeTexture.Length;
                var heightModel = edgeTexture.Y * scale / edgeTexture.Length;

                size.Width = Math.Max(size.Width, Math.Abs(widthModel));
                size.Height = Math.Max(size.Height, Math.Abs(heightModel));
            }

            return size;
        }
    }
}