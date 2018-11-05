using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Interop {
    public static class HwndHostCommands {
        public static Input.RoutedCommand<MouseActivateParameter> MouseActivate = new Input.RoutedCommand<MouseActivateParameter>("MouseActivate", typeof(HwndHostCommands));
    }
}