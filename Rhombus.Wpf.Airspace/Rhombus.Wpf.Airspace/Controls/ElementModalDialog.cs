using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Controls {
    public class ElementModalDialog<V, T> : System.Windows.Documents.Adorner where T : IModalContent<V>, new() {
        // Private - access through ShowDialog static method.
        private ElementModalDialog(System.Windows.UIElement adornedElement) : base(adornedElement) { }

        public static V ShowDialog(System.Windows.UIElement adornedElement) {
            var isEnabled = adornedElement.IsEnabled;
            try {
                // Disable the adorned element while the dialog is displayed.
                adornedElement.IsEnabled = false;

                var dialog = new ElementModalDialog<V, T>(adornedElement);

                var adornerLayer = System.Windows.Documents.AdornerLayer.GetAdornerLayer(adornedElement);
                adornerLayer.Add(dialog);

                var modalContent = new T();
                return modalContent.Accept();
            }
            finally {
                // Restore the enabled state of the adorned element once the dialog is dismissed.
                adornedElement.IsEnabled = isEnabled;
            }
        }

        protected override void OnRender(System.Windows.Media.DrawingContext drawingContext) {
            drawingContext.DrawRectangle(System.Windows.Media.Brushes.Blue, new System.Windows.Media.Pen(System.Windows.Media.Brushes.Red, 1), new System.Windows.Rect(new System.Windows.Point(0, 0), this.DesiredSize));

            base.OnRender(drawingContext);
        }
    }
}