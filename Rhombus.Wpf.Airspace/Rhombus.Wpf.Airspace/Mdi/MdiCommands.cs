using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Mdi {
    public static class MdiCommands {
        public static Input.RoutedCommand<AdjustWindowRectParameter> AdjustWindowRect = new Input.RoutedCommand<AdjustWindowRectParameter>("SizeWindow", typeof(MdiCommands));
        public static System.Windows.Input.RoutedCommand RestoreWindow = new System.Windows.Input.RoutedCommand("RestoreWindow", typeof(MdiCommands));
        public static System.Windows.Input.RoutedCommand MaximizeWindow = new System.Windows.Input.RoutedCommand("MaximizeWindow", typeof(MdiCommands));
        public static System.Windows.Input.RoutedCommand MinimizeWindow = new System.Windows.Input.RoutedCommand("MinimizeWindow", typeof(MdiCommands));
        public static System.Windows.Input.RoutedCommand CloseWindow = new System.Windows.Input.RoutedCommand("CloseWindow", typeof(MdiCommands));
        public static System.Windows.Input.RoutedCommand ActivateWindow = new System.Windows.Input.RoutedCommand("ActivateWindow", typeof(MdiCommands));
        public static System.Windows.Input.RoutedCommand ActivateNextWindow = new System.Windows.Input.RoutedCommand("ActivateNextWindow", typeof(MdiCommands));
        public static System.Windows.Input.RoutedCommand ActivatePreviousWindow = new System.Windows.Input.RoutedCommand("ActivatePreviousWindow", typeof(MdiCommands));
        public static System.Windows.Input.RoutedCommand CascadeWindows = new System.Windows.Input.RoutedCommand("CascadeWindows", typeof(MdiCommands));
        public static System.Windows.Input.RoutedCommand TileWindows = new System.Windows.Input.RoutedCommand("TileWindows", typeof(MdiCommands));
        public static System.Windows.Input.RoutedCommand MinimizeAllWindows = new System.Windows.Input.RoutedCommand("MinimizeAllWindows", typeof(MdiCommands));
        public static System.Windows.Input.RoutedCommand MaximizeAllWindows = new System.Windows.Input.RoutedCommand("MaximizeAllWindows", typeof(MdiCommands));
        public static System.Windows.Input.RoutedCommand RestoreAllWindows = new System.Windows.Input.RoutedCommand("RestoreAllWindows", typeof(MdiCommands));
        public static System.Windows.Input.RoutedCommand FloatWindow = new System.Windows.Input.RoutedCommand("FloatWindow", typeof(MdiCommands));
    }
}