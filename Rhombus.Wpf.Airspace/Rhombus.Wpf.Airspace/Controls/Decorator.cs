using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Controls {
    [System.Windows.Markup.ContentProperty("Child")]
    public class Decorator<T> : System.Windows.FrameworkElement where T : System.Windows.FrameworkElement {
        public static System.Windows.DependencyProperty ChildProperty = System.Windows.DependencyProperty.Register(
            /* Name:                 */ "Child",
            /* Value Type:           */ typeof(System.Windows.FrameworkElement),
            /* Owner Type:           */ typeof(Decorator<T>),
            /* Metadata:             */ new System.Windows.PropertyMetadata(
                /*     Default Value:    */ null,
                /*     Property Changed: */ (d, e) => ((Decorator<T>) d).OnChildChanged(e)));

        public T Child {
            get => (T) this.GetValue(ChildProperty);
            set => this.SetValue(ChildProperty, value);
        }

        protected override System.Collections.IEnumerator LogicalChildren {
            get {
                var child = this.Child;
                if (child != null)
                    yield return child;
            }
        }

        protected override int VisualChildrenCount => this.Child != null
            ? 1
            : 0;

        protected virtual void OnChildChanged(T oldChild, T newChild) { }

        private void OnChildChanged(System.Windows.DependencyPropertyChangedEventArgs e) {
            var oldChild = (T) e.OldValue;
            if (oldChild != null) {
                this.RemoveVisualChild(oldChild);
                this.RemoveLogicalChild(oldChild);
            }

            var newChild = (T) e.NewValue;
            if (newChild != null) {
                this.AddLogicalChild(newChild);
                this.AddVisualChild(newChild);
            }

            this.InvalidateMeasure();

            this.OnChildChanged(oldChild, newChild);
        }

        protected override System.Windows.Media.Visual GetVisualChild(int index) {
            var child = this.Child;
            if (child == null || index != 0)
                throw new ArgumentOutOfRangeException("index");
            return child;
        }

        protected override System.Windows.Size MeasureOverride(System.Windows.Size constraint) {
            var child = this.Child;
            if (child != null) {
                child.Measure(constraint);
                return child.DesiredSize;
            }

            return new System.Windows.Size();
        }

        protected override System.Windows.Size ArrangeOverride(System.Windows.Size arrangeSize) {
            var child = this.Child;
            if (child != null)
                child.Arrange(new System.Windows.Rect(arrangeSize));
            return arrangeSize;
        }
    }
}