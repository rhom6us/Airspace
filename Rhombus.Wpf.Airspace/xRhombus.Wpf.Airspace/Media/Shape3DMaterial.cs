using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Media {
    /// <summary>
    ///     Shape3DMaterial describes the material to use to display on the
    ///     surface of a shape.
    /// </summary>
    /// <remarks>
    ///     This can be either:
    ///     1) A non-interactive Material, possibly deferred
    ///     2) A an interactive UIElement, possibly deferred
    /// </remarks>
    public class Shape3DMaterial {
        public Shape3DMaterial(System.Windows.Media.Media3D.Material material) {
            _getMaterial = shape => material;
        }

        public Shape3DMaterial(Func<Shapes.Shape3D, System.Windows.Media.Media3D.Material> getMaterial) {
            _getMaterial = getMaterial;
        }

        public Shape3DMaterial(System.Windows.UIElement element) {
            _getElement = shape => element;
        }

        public Shape3DMaterial(Func<Shapes.Shape3D, System.Windows.UIElement> getElement) {
            _getElement = getElement;
        }

        public bool HasMaterial => _getMaterial != null;

        public bool HasElement => _getElement != null;

        public static implicit operator Shape3DMaterial(System.Windows.Media.Media3D.Material material) {
            return new Shape3DMaterial(material);
        }

        public static implicit operator Shape3DMaterial(System.Windows.UIElement element) {
            return new Shape3DMaterial(element);
        }

        public System.Windows.Media.Media3D.Material GetMaterial(Shapes.Shape3D shape) {
            if (_getMaterial != null)
                return _getMaterial(shape);
            return null;
        }

        public System.Windows.UIElement GetElement(Shapes.Shape3D shape) {
            if (_getElement != null)
                return _getElement(shape);
            return null;
        }

        private readonly Func<Shapes.Shape3D, System.Windows.UIElement> _getElement;

        private readonly Func<Shapes.Shape3D, System.Windows.Media.Media3D.Material> _getMaterial;
    }
}