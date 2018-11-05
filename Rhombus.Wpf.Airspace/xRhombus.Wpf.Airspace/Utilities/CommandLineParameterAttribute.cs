using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Utilities {
    /// <summary>
    ///     The CommandLineParameter attribute can be used to indicate how
    ///     properties of a type are mapped to command line parameters.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class CommandLineParameterAttribute : Attribute {
        public CommandLineParameterAttribute(string name) {
            Name = name;
        }

        public bool IsRequired;

        public string Name;
        public string ShortDescription;
    }
}