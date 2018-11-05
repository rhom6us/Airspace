using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Extensions {
    public static class Int32RectExtensions {
        public static System.Windows.Int32Rect Union(this System.Windows.Int32Rect rect1, System.Windows.Int32Rect rect2) {
            if (rect1.IsEmpty)
                return rect2;

            if (rect2.IsEmpty)
                return rect1;

            var left = rect1.X < rect2.X
                ? rect1.X
                : rect2.X;
            var top = rect1.Y < rect2.Y
                ? rect1.Y
                : rect2.Y;
            var right = rect1.X + rect1.Width > rect2.X + rect2.Width
                ? rect1.X + rect1.Width
                : rect2.X + rect2.Width;
            var bottom = rect1.Y + rect1.Height > rect2.Y + rect2.Height
                ? rect1.Y + rect1.Height
                : rect2.Y + rect2.Height;

            return new System.Windows.Int32Rect(left, top, right - left, bottom - top);
        }

        public static int GetArea(this System.Windows.Int32Rect rect) {
            if (rect.IsEmpty)
                return 0;
            return rect.Width * rect.Height;
        }
    }
}