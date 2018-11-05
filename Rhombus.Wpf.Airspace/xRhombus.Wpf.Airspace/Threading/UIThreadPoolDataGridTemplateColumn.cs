using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Threading {
    public class UIThreadPoolDataGridTemplateColumn : System.Windows.Controls.DataGridTemplateColumn {
        public string PropertyName { get; set; }

        protected override System.Windows.FrameworkElement GenerateElement(System.Windows.Controls.DataGridCell cell, object dataItem) {
            // Get the cell template and make sure it is sealed because we need
            // to inflate it on the worker thread.
            var cellTemplate = this.CellTemplate;
            if (!cellTemplate.IsSealed)
                cellTemplate.Seal();

            var root = new UIThreadPoolRoot();
            root.DataContext = dataItem;
            root.Content = cellTemplate;
            root.PropertyName = this.PropertyName;

            return root;
        }
    }
}