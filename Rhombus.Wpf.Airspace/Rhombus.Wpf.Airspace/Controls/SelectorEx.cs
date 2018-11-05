using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Controls {
    /// <summary>
    ///     A simple generic version of ItemsControl that overrides the
    ///     virtual methods to support items of type T.
    /// </summary>
    public class SelectorEx<TContainer> : System.Windows.Controls.Primitives.Selector where TContainer : System.Windows.DependencyObject, new() {
        public SelectorEx() {
            ((System.Collections.Specialized.INotifyCollectionChanged) this.Items).CollectionChanged += this.OnItemsCollectionChanged;
        }

        /// <summary>
        ///     Determines if the item needs to be wrapped in a container.
        /// </summary>
        /// <remarks>
        ///     This is determined by whether or not the item is an instance
        ///     of the container type.  This virtual method is sealed.
        /// </remarks>
        protected sealed override bool IsItemItsOwnContainerOverride(object item) {
            return item is TContainer;
        }

        /// <summary>
        ///     Provides an empty container to be used to wrap an item.
        /// </summary>
        /// <remarks>
        ///     The container is a new instance of the container type.  This
        ///     virtual method is sealed.
        /// </remarks>
        protected sealed override System.Windows.DependencyObject GetContainerForItemOverride() {
            var container = new TContainer();
            this.OnContainerAdded(container);

            return container;
        }

        /// <summary>
        ///     Returns the immediate container from an arbitrary element.
        /// </summary>
        /// <remarks>
        ///     This is a new strongly typed version of the base
        ///     ContainerFromElement method.
        /// </remarks>
        public new TContainer ContainerFromElement(System.Windows.DependencyObject element) {
            return (TContainer) base.ContainerFromElement(element);
        }

        /// <summary>
        ///     Clears a container for an item.
        /// </summary>
        /// <remarks>
        ///     In order to implement a strongly typed version of this method,
        ///     this virtual is sealed and it delegates to a new strongly
        ///     typed virtual.
        /// </remarks>
        protected sealed override void ClearContainerForItemOverride(System.Windows.DependencyObject element, object item) {
            var container = (TContainer) element;
            this.ClearContainerForItemOverride(container, item);
            this.OnContainerRemoved(container);
        }

        /// <summary>
        ///     Clears a container for an item.
        /// </summary>
        /// <remarks>
        ///     This is the new strongly typed virtual.  It calls the original
        ///     weakly typed base implementation to preserve functionality.
        /// </remarks>
        protected virtual void ClearContainerForItemOverride(TContainer container, object item) {
            base.ClearContainerForItemOverride(container, item);
        }

        /// <summary>
        ///     Prepares a container for an item.
        /// </summary>
        /// <remarks>
        ///     In order to implement a strongly typed version of this method,
        ///     this virtual is sealed and it delegates to a new strongly
        ///     typed virtual.
        /// </remarks>
        protected sealed override void PrepareContainerForItemOverride(System.Windows.DependencyObject element, object item) {
            this.PrepareContainerForItemOverride((TContainer) element, item);
        }

        /// <summary>
        ///     Prepares a container for an item.
        /// </summary>
        /// <remarks>
        ///     This is the new strongly typed virtual.  It calls the original
        ///     weakly typed base implementation to preserve functionality.
        /// </remarks>
        protected virtual void PrepareContainerForItemOverride(TContainer container, object item) {
            base.PrepareContainerForItemOverride(container, item);
        }

        /// <summary>
        ///     Determines if the item container style should be applied.
        /// </summary>
        /// <remarks>
        ///     This is the new strongly typed virtual.  It calls the original
        ///     weakly typed base implementation to preserve functionality.
        /// </remarks>
        protected sealed override bool ShouldApplyItemContainerStyle(System.Windows.DependencyObject container, object item) {
            return this.ShouldApplyItemContainerStyle((TContainer) container, item);
        }

        /// <summary>
        ///     Determines if the item container style should be applied.
        /// </summary>
        /// <remarks>
        ///     This is the new strongly typed virtual.  It calls the original
        ///     weakly typed base implementation to preserve functionality.
        /// </remarks>
        protected virtual bool ShouldApplyItemContainerStyle(TContainer container, object item) {
            return base.ShouldApplyItemContainerStyle(container, item);
        }

        /// <summary>
        ///     Called when the containers are reset.
        /// </summary>
        protected virtual void OnContainersReset() { }

        /// <summary>
        ///     Called either when a container is generated or when an item
        ///     that is its own container is added.
        /// </summary>
        protected virtual void OnContainerAdded(TContainer container) { }

        /// <summary>
        ///     Called either when a generated container is removed or when
        ///     an item that is its own container is removed.
        /// </summary>
        protected virtual void OnContainerRemoved(TContainer container) { }

        private void OnItemsCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) {
            switch (e.Action) {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Reset: {
                    this.OnContainersReset();

                    foreach (var item in this.Items) {
                        if (this.IsItemItsOwnContainerOverride(item))
                            this.OnContainerAdded((TContainer) item);
                    }
                }
                    break;

                case System.Collections.Specialized.NotifyCollectionChangedAction.Add: {
                    foreach (var item in e.NewItems) {
                        if (this.IsItemItsOwnContainerOverride(item))
                            this.OnContainerAdded((TContainer) item);
                    }
                }
                    break;

                case System.Collections.Specialized.NotifyCollectionChangedAction.Replace: {
                    foreach (var item in e.OldItems) {
                        if (this.IsItemItsOwnContainerOverride(item))
                            this.OnContainerRemoved((TContainer) item);
                    }

                    foreach (var item in e.NewItems) {
                        if (this.IsItemItsOwnContainerOverride(item))
                            this.OnContainerAdded((TContainer) item);
                    }
                }
                    break;

                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove: {
                    foreach (var item in e.OldItems) {
                        if (this.IsItemItsOwnContainerOverride(item))
                            this.OnContainerRemoved((TContainer) item);
                    }
                }
                    break;
            }
        }
    }
}