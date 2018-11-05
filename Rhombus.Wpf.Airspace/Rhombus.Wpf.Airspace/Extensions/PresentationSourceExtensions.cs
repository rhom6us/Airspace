using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Extensions {
    public static class PresentationSourceExtensions {
        /// <summary>
        ///     Convert a point from "client" coordinate space of a window into
        ///     the coordinate space of the specified element of the same window.
        /// </summary>
        public static System.Windows.Point TransformClientToDescendant(this System.Windows.PresentationSource presentationSource, System.Windows.Point point, System.Windows.Media.Visual descendant) {
            var pt = presentationSource.TransformClientToRoot(point);
            return presentationSource.RootVisual.TransformToDescendant(descendant).Transform(pt);
        }

        /// <summary>
        ///     Convert a rectangle from "client" coordinate space of a window
        ///     into the coordinate space of the specified element of the same
        ///     window.
        /// </summary>
        public static System.Windows.Rect TransformClientToDescendant(this System.Windows.PresentationSource presentationSource, System.Windows.Rect rect, System.Windows.Media.Visual descendant) {
            // Transform all 4 corners.  Since a rectangle is convex, it will
            // remain convex under affine transforms.
            var pt1 = presentationSource.TransformClientToDescendant(new System.Windows.Point(rect.Left, rect.Top), descendant);
            var pt2 = presentationSource.TransformClientToDescendant(new System.Windows.Point(rect.Right, rect.Top), descendant);
            var pt3 = presentationSource.TransformClientToDescendant(new System.Windows.Point(rect.Right, rect.Bottom), descendant);
            var pt4 = presentationSource.TransformClientToDescendant(new System.Windows.Point(rect.Left, rect.Bottom), descendant);

            var minX = Math.Min(pt1.X, Math.Min(pt2.X, Math.Min(pt3.X, pt4.X)));
            var minY = Math.Min(pt1.Y, Math.Min(pt2.Y, Math.Min(pt3.Y, pt4.Y)));
            var maxX = Math.Max(pt1.X, Math.Max(pt2.X, Math.Max(pt3.X, pt4.X)));
            var maxY = Math.Max(pt1.Y, Math.Max(pt2.Y, Math.Max(pt3.Y, pt4.Y)));

            return new System.Windows.Rect(minX, minY, maxX - minX, maxY - minY);
        }

        /// <summary>
        ///     Convert a point from the coordinate space of the specified
        ///     element into the "client" coordinate space of the window.
        /// </summary>
        public static System.Windows.Point TransformDescendantToClient(this System.Windows.PresentationSource presentationSource, System.Windows.Point point, System.Windows.Media.Visual descendant) {
            var pt = descendant.TransformToAncestor(presentationSource.RootVisual).Transform(point);
            return presentationSource.TransformRootToClient(pt);
        }

        /// <summary>
        ///     Convert a rectangle from the coordinate space of the specified
        ///     element into the "client" coordinate space of the window.
        /// </summary>
        public static System.Windows.Rect TransformDescendantToClient(this System.Windows.PresentationSource presentationSource, System.Windows.Rect rect, System.Windows.Media.Visual descendant) {
            // Transform all 4 corners.  Since a rectangle is convex, it will
            // remain convex under affine transforms.
            var pt1 = presentationSource.TransformDescendantToClient(new System.Windows.Point(rect.Left, rect.Top), descendant);
            var pt2 = presentationSource.TransformDescendantToClient(new System.Windows.Point(rect.Right, rect.Top), descendant);
            var pt3 = presentationSource.TransformDescendantToClient(new System.Windows.Point(rect.Right, rect.Bottom), descendant);
            var pt4 = presentationSource.TransformDescendantToClient(new System.Windows.Point(rect.Left, rect.Bottom), descendant);

            var minX = Math.Min(pt1.X, Math.Min(pt2.X, Math.Min(pt3.X, pt4.X)));
            var minY = Math.Min(pt1.Y, Math.Min(pt2.Y, Math.Min(pt3.Y, pt4.Y)));
            var maxX = Math.Max(pt1.X, Math.Max(pt2.X, Math.Max(pt3.X, pt4.X)));
            var maxY = Math.Max(pt1.Y, Math.Max(pt2.Y, Math.Max(pt3.Y, pt4.Y)));

            return new System.Windows.Rect(minX, minY, maxX - minX, maxY - minY);
        }

        /// <summary>
        ///     Convert a point from "client" coordinate space of a window into
        ///     the coordinate space of the root element of the same window.
        /// </summary>
        public static System.Windows.Point TransformClientToRoot(this System.Windows.PresentationSource presentationSource, System.Windows.Point pt) {
            // Convert from pixels into DIPs.
            pt = presentationSource.CompositionTarget.TransformFromDevice.Transform(pt);

            // We need to include the root element's transform.
            pt = PresentationSourceExtensions.ApplyVisualTransform(presentationSource.RootVisual, pt, true);

            return pt;
        }

        /// <summary>
        ///     Convert a point from the coordinate space of the root element
        ///     into the "client" coordinate space of the same window.
        /// </summary>
        public static System.Windows.Point TransformRootToClient(this System.Windows.PresentationSource presentationSource, System.Windows.Point pt) {
            // We need to include the root element's transform.
            pt = PresentationSourceExtensions.ApplyVisualTransform(presentationSource.RootVisual, pt, false);

            // Convert from DIPs into pixels.
            pt = presentationSource.CompositionTarget.TransformToDevice.Transform(pt);

            return pt;
        }

        /// <summary>
        ///     Convert a point from "above" the coordinate space of a
        ///     visual into the the coordinate space "below" the visual.
        /// </summary>
        private static System.Windows.Point ApplyVisualTransform(System.Windows.Media.Visual v, System.Windows.Point pt, bool inverse) {
            var m = PresentationSourceExtensions.GetVisualTransform(v);

            if (inverse)
                m.Invert();

            return m.Transform(pt);
        }

        /// <summary>
        ///     Gets the matrix that will convert a point
        ///     from "above" the coordinate space of a visual
        ///     into the the coordinate space "below" the visual.
        /// </summary>
        private static System.Windows.Media.Matrix GetVisualTransform(System.Windows.Media.Visual v) {
            var m = System.Windows.Media.Matrix.Identity;

            // A visual can currently have two properties that affect
            // its coordinate space:
            //    1) Transform - any matrix
            //    2) Offset - a simpification for just a 2D offset.
            var transform = System.Windows.Media.VisualTreeHelper.GetTransform(v);
            if (transform != null) {
                var cm = transform.Value;
                m = System.Windows.Media.Matrix.Multiply(m, cm);
            }

            var offset = System.Windows.Media.VisualTreeHelper.GetOffset(v);
            m.Translate(offset.X, offset.Y);

            return m;
        }
    }
}