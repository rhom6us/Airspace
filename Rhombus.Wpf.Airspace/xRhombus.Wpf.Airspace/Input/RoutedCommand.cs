using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Input {
    /// <summary>
    ///     A simple generic version of RoutedCommand that provides new
    ///     strongly typed methods to support command parameters of type T.
    /// </summary>
    public class RoutedCommand<T> : System.Windows.Input.RoutedCommand {
        public RoutedCommand() { }
        public RoutedCommand(string name, Type ownerType) : base(name, ownerType) { }
        public RoutedCommand(string name, Type ownerType, System.Windows.Input.InputGestureCollection inputGestures) : base(name, ownerType, inputGestures) { }

        /// <summary>
        ///     Determines whether this command can execute on the specified
        ///     target.
        /// </summary>
        public bool CanExecute(T parameter, System.Windows.IInputElement target) {
            return base.CanExecute(parameter, target);
        }

        /// <summary>
        ///     Executes the command on the specified target.
        /// </summary>
        public void Execute(T parameter, System.Windows.IInputElement target) {
            base.Execute(parameter, target);
        }
    }
}