using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Shapes {
    /// <summary>
    ///     The abstract base class for 3D shapes.
    /// </summary>
    /// <remarks>
    ///     Shape3D inherits from UIElement3D, which provides the rich set of
    ///     input properties, methods, and events.
    ///     The geometry for a Shape3D is provided by derived types.
    ///     The material for the surface of a Shape3D is normally defined by
    ///     2D Visuals, though derived types may specify the materials
    ///     underneath the visuals.
    /// </remarks>
    [System.Windows.Markup.ContentProperty("Material")]
    public abstract class Shape3D : System.Windows.UIElement3D {
        public static System.Windows.DependencyProperty FrontMaterialProperty = System.Windows.DependencyProperty.Register("FrontMaterial", typeof(Media.Shape3DMaterial), typeof(Shape3D), new System.Windows.PropertyMetadata(new Media.Shape3DMaterial(new System.Windows.Media.Media3D.DiffuseMaterial(System.Windows.Media.Brushes.Red)), Shape3D.OnPropertyChangedAffectsModel));

        public static System.Windows.DependencyProperty BackMaterialProperty = System.Windows.DependencyProperty.Register("BackMaterial", typeof(Media.Shape3DMaterial), typeof(Shape3D), new System.Windows.PropertyMetadata(new Media.Shape3DMaterial(new System.Windows.Media.Media3D.DiffuseMaterial(System.Windows.Media.Brushes.Red)), Shape3D.OnPropertyChangedAffectsModel));

        public static System.Windows.DependencyProperty CacheScaleProperty = System.Windows.DependencyProperty.Register("CacheScale", typeof(Media.CacheScale), typeof(Shape3D), new System.Windows.PropertyMetadata(Media.CacheScale.Auto, Shape3D.OnPropertyChangedAffectsModel));

        public static System.Windows.DependencyProperty SurfaceWidthProperty = System.Windows.DependencyProperty.Register("SurfaceWidth", typeof(double), typeof(ParametricShape3D), new System.Windows.PropertyMetadata(0.0, Shape3D.OnPropertyChangedAffectsModel));

        public static System.Windows.DependencyProperty SurfaceHeightProperty = System.Windows.DependencyProperty.Register("SurfaceHeight", typeof(double), typeof(Shape3D), new System.Windows.PropertyMetadata(0.0, Shape3D.OnPropertyChangedAffectsModel));

        public static System.Windows.DependencyProperty MeshTransformProperty = System.Windows.DependencyProperty.Register("MeshTransform", typeof(System.Windows.Media.Media3D.Transform3D), typeof(Shape3D), new System.Windows.PropertyMetadata(new System.Windows.Media.Media3D.ScaleTransform3D(100.0, 100.0, 100.0), Shape3D.OnPropertyChangedAffectsModel));

        public Media.Shape3DMaterial FrontMaterial {
            get => (Media.Shape3DMaterial) this.GetValue(FrontMaterialProperty);
            set => this.SetValue(FrontMaterialProperty, value);
        }

        public Media.Shape3DMaterial BackMaterial {
            get => (Media.Shape3DMaterial) this.GetValue(BackMaterialProperty);
            set => this.SetValue(BackMaterialProperty, value);
        }

        public Media.CacheScale CacheScale {
            get => (Media.CacheScale) this.GetValue(CacheScaleProperty);
            set => this.SetValue(CacheScaleProperty, value);
        }

        public double SurfaceWidth {
            get => (double) this.GetValue(SurfaceWidthProperty);
            set => this.SetValue(SurfaceWidthProperty, value);
        }

        public double SurfaceHeight {
            get => (double) this.GetValue(SurfaceHeightProperty);
            set => this.SetValue(SurfaceHeightProperty, value);
        }

        public System.Windows.Media.Media3D.Transform3D MeshTransform {
            get => (System.Windows.Media.Media3D.Transform3D) this.GetValue(MeshTransformProperty);
            set => this.SetValue(MeshTransformProperty, value);
        }

        /// <summary>
        ///     Returns the number of children of this Visual3D.
        /// </summary>
        protected sealed override int Visual3DChildrenCount {
            get {
                var numChildren = 0;

                if (_frontChild != null)
                    numChildren++;

                if (_backChild != null)
                    numChildren++;

                return numChildren;
            }
        }

        private System.Windows.Media.Media3D.DiffuseMaterial HostingMaterial {
            get {
                if (_hostingMaterial == null) {
                    _hostingMaterial = new System.Windows.Media.Media3D.DiffuseMaterial {
                        Color = System.Windows.Media.Colors.White
                    };
                    System.Windows.Media.Media3D.Viewport2DVisual3D.SetIsVisualHostMaterial(_hostingMaterial, true);

                    _hostingMaterial.Freeze();
                }

                return _hostingMaterial;
            }
        }

        protected abstract System.Windows.Media.Media3D.MeshGeometry3D GetGeometry();

        /// <summary>
        ///     Updates the model for this shape.
        /// </summary>
        /// <remarks>
        ///     The model is determined by what needs to be displayed.  If no
        ///     interactive visual needs to be displayed, then a simple
        ///     ModelVisual3D is used to display a GeometryModel3D.  If an
        ///     interactive visual needs to be displayed, then a
        ///     Viewport2DVisual3D is used.
        /// </remarks>
        protected override void OnUpdateModel() {
            var geometry = this.GetGeometry();
            for (var i = 0; i < geometry.Positions.Count; i++) {
                geometry.Positions[i] = this.MeshTransform.Transform(geometry.Positions[i]);
            }

            var surfaceSize = Extensions.MeshGeometry3DExtensions.CalculateIdealTextureSize(geometry);
            this.SurfaceWidth = surfaceSize.Width;
            this.SurfaceHeight = surfaceSize.Height;

            System.Windows.Media.Media3D.MeshGeometry3D frontGeometry = null;
            var frontMaterial = this.FrontMaterial;
            if (frontMaterial != null) {
                frontGeometry = geometry;
                if (frontGeometry == null)
                    frontMaterial = null;
            }

            System.Windows.Media.Media3D.MeshGeometry3D backGeometry = null;
            var backMaterial = this.BackMaterial;
            if (backMaterial != null) {
                backGeometry = Shape3D.CreateBackGeometry(geometry);
                if (backGeometry == null)
                    backMaterial = null;
            }


            this.EnsureChildren(frontMaterial, backMaterial);
            this.UpdateChild(_frontChild, frontGeometry, frontMaterial);
            this.UpdateChild(_backChild, backGeometry, backMaterial);
        }

        /// <summary>
        ///     Returns the requested child identified by an index.
        /// </summary>
        /// <param name="index">
        ///     The index that identifies the child to return.
        /// </param>
        /// <returns>
        ///     The child Visual3D identified by the index.
        /// </returns>
        protected sealed override System.Windows.Media.Media3D.Visual3D GetVisual3DChild(int index) {
            if (index == 0) {
                if (_frontChild != null)
                    return _frontChild;
                if (_backChild != null)
                    return _backChild;
            }
            else if (index == 1) {
                if (_frontChild != null && _backChild != null)
                    return _backChild;
            }

            throw new IndexOutOfRangeException("index");
        }

        public System.Windows.UIElement GrabFrontChildElement() {
            var e = (System.Windows.UIElement) ((System.Windows.Media.Media3D.Viewport2DVisual3D) _frontChild).Visual;
            ((System.Windows.Media.Media3D.Viewport2DVisual3D) _frontChild).Visual = null;
            return e;
        }

        // All derived classes can use this as the property change handler for
        // properties that affect the model.
        protected static void OnPropertyChangedAffectsModel(object sender, System.Windows.DependencyPropertyChangedEventArgs e) {
            var _this = (Shape3D) sender;
            _this.InvalidateModel();
        }

        private void EnsureChildren(Media.Shape3DMaterial frontMaterial, Media.Shape3DMaterial backMaterial) {
            var frontChild = this.EnsureChild(_frontChild, frontMaterial);
            if (frontChild != _frontChild) {
                if (_frontChild != null)
                    this.RemoveVisual3DChild(_frontChild);

                _frontChild = frontChild;

                if (_frontChild != null)
                    this.AddVisual3DChild(_frontChild);
            }

            var backChild = this.EnsureChild(_backChild, backMaterial);
            if (backChild != _backChild) {
                if (_backChild != null)
                    this.RemoveVisual3DChild(_backChild);

                _backChild = backChild;

                if (_backChild != null)
                    this.AddVisual3DChild(_backChild);
            }
        }

        private System.Windows.Media.Media3D.Visual3D EnsureChild(System.Windows.Media.Media3D.Visual3D currentChild, Media.Shape3DMaterial material) {
            System.Windows.Media.Media3D.Visual3D newChild = null;

            if (material != null) {
                if (material.HasElement) {
                    // We need a Viewport2DVisual3D to display an element.
                    if (currentChild is System.Windows.Media.Media3D.Viewport2DVisual3D) {
                        newChild = currentChild;
                    }
                    else {
                        //Viewbox viewbox = new Viewbox();
                        //viewbox.StretchDirection = StretchDirection.Both;
                        //viewbox.Stretch = Stretch.Fill;

                        var border = new System.Windows.Controls.Border {
                            UseLayoutRounding = true,
                            Background = System.Windows.Media.Brushes.Green
                        };
                        //border.Child = viewbox;

                        var viewport = new System.Windows.Media.Media3D.Viewport2DVisual3D {
                            Visual = border
                        };

                        newChild = viewport;
                    }

                    // Set the appropriate caching strategy.
                    var cacheScale = this.CacheScale;
                    if (cacheScale == null) {
                        // Remove any VisualBrush caching.
                        System.Windows.Media.RenderOptions.SetCachingHint(newChild, System.Windows.Media.CachingHint.Unspecified);

                        // Remove any BitmapCache.
                        ((System.Windows.Media.Media3D.Viewport2DVisual3D) newChild).CacheMode = null;
                    }
                    else if (cacheScale.IsAuto) {
                        // Remove any BitmapCache.
                        ((System.Windows.Media.Media3D.Viewport2DVisual3D) newChild).CacheMode = null;

                        // Specify VisualBrush caching with 2x min and max
                        // thresholds.
                        System.Windows.Media.RenderOptions.SetCachingHint(newChild, System.Windows.Media.CachingHint.Cache);
                        System.Windows.Media.RenderOptions.SetCacheInvalidationThresholdMinimum(newChild, 0.5);
                        System.Windows.Media.RenderOptions.SetCacheInvalidationThresholdMaximum(newChild, 2.0);
                    }
                    else {
                        // Remove any VisualBrush caching.
                        System.Windows.Media.RenderOptions.SetCachingHint(newChild, System.Windows.Media.CachingHint.Unspecified);

                        // Set a BitmapCache with the appropriate scale.
                        var bitmapCache = ((System.Windows.Media.Media3D.Viewport2DVisual3D) newChild).CacheMode as System.Windows.Media.BitmapCache;
                        if (bitmapCache == null)
                            ((System.Windows.Media.Media3D.Viewport2DVisual3D) newChild).CacheMode = new System.Windows.Media.BitmapCache(cacheScale.Scale);
                        else
                            bitmapCache.RenderAtScale = cacheScale.Scale;
                    }
                }
                else {
                    System.Diagnostics.Debug.Assert(material.HasMaterial);

                    // We need a ModelVisual3D to display the material.
                    if (currentChild is System.Windows.Media.Media3D.ModelVisual3D) {
                        System.Diagnostics.Debug.Assert(((System.Windows.Media.Media3D.ModelVisual3D) currentChild).Content is System.Windows.Media.Media3D.GeometryModel3D);
                        newChild = currentChild;
                    }
                    else {
                        newChild = new System.Windows.Media.Media3D.ModelVisual3D();
                        ((System.Windows.Media.Media3D.ModelVisual3D) newChild).Content = new System.Windows.Media.Media3D.GeometryModel3D();
                    }
                }
            }

            return newChild;
        }

        private void UpdateChild(System.Windows.Media.Media3D.Visual3D child, System.Windows.Media.Media3D.MeshGeometry3D geometry, Media.Shape3DMaterial material) {
            if (material != null) {
                if (material.HasElement) {
                    var viewport = (System.Windows.Media.Media3D.Viewport2DVisual3D) child;
                    viewport.Geometry = geometry;
                    viewport.Material = this.HostingMaterial;

                    // Set the size of the root element to the surface size.
                    var border = (System.Windows.Controls.Border) viewport.Visual;
                    border.Width = this.SurfaceWidth;
                    border.Height = this.SurfaceHeight;
                    border.Child = material.GetElement(this);

                    // Add the material.
                    //Viewbox viewbox = (Viewbox)border.Child;
                    //viewbox.Width = SurfaceWidth;
                    //viewbox.Height = SurfaceHeight;
                    //viewbox.Child = material.GetElement(this);

                    // Set the mesh into the visualizer.
                    //viewbox.Child = new MeshTextureCoordinateVisualizer();
                    //((MeshTextureCoordinateVisualizer)viewbox.Child).Mesh = geometry;
                }
                else {
                    System.Diagnostics.Debug.Assert(material.HasMaterial);

                    var model = (System.Windows.Media.Media3D.GeometryModel3D) ((System.Windows.Media.Media3D.ModelVisual3D) child).Content;
                    model.Geometry = geometry;
                    model.Material = material.GetMaterial(this);
                }
            }
        }

        private static System.Windows.Media.Media3D.MeshGeometry3D CreateBackGeometry(System.Windows.Media.Media3D.MeshGeometry3D frontMesh) {
            if (frontMesh == null)
                return null;

            var backMesh = new System.Windows.Media.Media3D.MeshGeometry3D {
                // Simply share the same (frozen) collections for positions and
                // texture coordinates.
                Positions = frontMesh.Positions,
                TextureCoordinates = frontMesh.TextureCoordinates,

                // Make a copy of the triangle indices and wind them backwards.
                TriangleIndices = new System.Windows.Media.Int32Collection(frontMesh.TriangleIndices)
            };
            var numTriangles = backMesh.TriangleIndices.Count / 3;
            for (var iTriangle = 0; iTriangle < numTriangles; iTriangle++) {
                var temp = backMesh.TriangleIndices[iTriangle * 3];
                backMesh.TriangleIndices[iTriangle * 3] = backMesh.TriangleIndices[iTriangle * 3 + 2];
                backMesh.TriangleIndices[iTriangle * 3 + 2] = temp;
            }

            // Make a copy of the normals and reverse their direction.
            backMesh.Normals = new System.Windows.Media.Media3D.Vector3DCollection(frontMesh.Normals);
            var numNormals = backMesh.Normals.Count;
            for (var iNormal = 0; iNormal < numNormals; iNormal++) {
                backMesh.Normals[iNormal] *= -1.0;
            }

            backMesh.Freeze();
            return backMesh;
        }

        private System.Windows.Media.Media3D.Visual3D _backChild;

        private System.Windows.Media.Media3D.Visual3D _frontChild;
        private System.Windows.Media.Media3D.DiffuseMaterial _hostingMaterial;
    }
}