using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Utilities {
    /// <summary>
    ///     The CommandLineParser class supports parsing the command line of an
    ///     application in a standard way that maps to the static properties of
    ///     a specified class.
    /// </summary>
    public static class CommandLineParser {
        /// <summary>
        ///     Parses the specified command line in a standard way that maps
        ///     the command line parameters to the public static properties of
        ///     the specified class.
        /// </summary>
        /// <typeparam name="T">
        ///     The class that has properties that will be specified from the
        ///     command line parameters.
        /// </typeparam>
        /// <param name="args">
        ///     The command line to parse.
        /// </param>
        public static void Parse<T>(string[] args) {
            // First scan for any "-help" or "-?" strings.
            foreach (var arg in args) {
                if (arg == "/?" || arg == "/help" || arg == "-?" || arg == "-help")
                    CommandLineParser.ThrowUsageException<T>();
            }

            try {
                var bindings = CommandLineParser.GetParamBindings<T>();

                // process the command line
                foreach (var arg in args) {
                    var split = arg.Split(new[] {'='}, 2);

                    var argName = split[0];
                    var argValue = split.Length == 2
                        ? split[1]
                        : null;

                    if (bindings.ContainsKey(argName)) {
                        var paramBinding = bindings[argName];

                        if (paramBinding.HasBeenSet)
                            throw new CommandLineParameterException($"Argument '{argName}' has already been specified.");

                        if (argValue != null) {
                            var value = paramBinding.ParserMethod.Invoke(null, new object[] {argValue});
                            paramBinding.PropertyInfo.GetSetMethod().Invoke(null, new[] {value});
                            paramBinding.HasBeenSet = true;
                        }
                        else {
                            if (paramBinding.PropertyInfo.PropertyType == typeof(bool)) {
                                // Set the boolean property to true.
                                paramBinding.PropertyInfo.GetSetMethod().Invoke(null, new object[] {true});
                                paramBinding.HasBeenSet = true;
                            }
                            else {
                                // Only boolean properties can be specified without a value.
                                throw new CommandLineParameterException($"Argument '{argName}' must provide a value.");
                            }
                        }
                    }
                    else {
                        throw new CommandLineParameterException($"Argument '{argName}' was not expected.");
                    }
                }

                // Make sure all required parameters were provided.
                foreach (var paramBinding in bindings.Values) {
                    if (!paramBinding.HasBeenSet && paramBinding.Attribute.IsRequired)
                        throw new CommandLineParameterException($"Argument '{paramBinding.Attribute.Name}' is required.");
                }
            }
            catch (Exception e) {
                CommandLineParser.ThrowUsageException<T>(e);
            }
        }

        public static void PrintUsageException(string programName, CommandLineUsageException e) {
            if (e.InnerException != null)
                Console.WriteLine("Error: {0}", e.InnerException.Message);

            Console.WriteLine("Usage: {0} {1}", programName, e.Message);
        }

        private static void ThrowUsageException<T>(Exception e = null) {
            var usage = "";
            var bindings = CommandLineParser.GetParamBindings<T>();

            foreach (var paramBinding in bindings.Values) {
                var shortDescription = string.IsNullOrWhiteSpace(paramBinding.Attribute.ShortDescription)
                    ? "value"
                    : paramBinding.Attribute.ShortDescription;
                var valueAssignment = $"=<{shortDescription}>";

                var possiblyOptionalValueAssignement = valueAssignment;
                if (paramBinding.PropertyInfo.PropertyType == typeof(bool))
                    possiblyOptionalValueAssignement = $"[{valueAssignment}]";

                var argumentAssignment = $"{paramBinding.Attribute.Name}{possiblyOptionalValueAssignement}";
                var possiblyOptionalArgumentAssignment = argumentAssignment;
                if (!paramBinding.Attribute.IsRequired)
                    possiblyOptionalArgumentAssignment = $"[{argumentAssignment}]";

                if (string.IsNullOrWhiteSpace(usage))
                    usage = possiblyOptionalArgumentAssignment;
                else
                    usage = $"{usage} {possiblyOptionalArgumentAssignment}";
            }

            throw new CommandLineUsageException(usage, e);
        }

        private static Dictionary<string, ParamBinding> GetParamBindings<T>() {
            var bindings = new Dictionary<string, ParamBinding>();

            // Iterate over all of the public writable static properties.
            foreach (var property in typeof(T).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.SetProperty)) {
                // Look up the CommandLineAttribute for the property.
                var attribute = Attribute.GetCustomAttribute(property, typeof(CommandLineParameterAttribute)) as CommandLineParameterAttribute;
                if (attribute != null) {
                    // Find the parse method for the property's type.
                    var parser = property.PropertyType.GetMethod("Parse", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public, null, new[] {typeof(string)}, null);
                    if (parser == null)
                        throw new InvalidOperationException($"Unable to locate Parse method for type '{property.PropertyType.Name}' for argument '{attribute.Name}'.");

                    bindings.Add(attribute.Name, new ParamBinding {HasBeenSet = false, Attribute = attribute, PropertyInfo = property, ParserMethod = parser});
                }
            }

            return bindings;
        }

        /// <summary>
        ///     The CommandLineParser.ParamBinding class is a helper class for
        ///     storing information related to the binding and processing of
        ///     command line parameters.
        /// </summary>
        private class ParamBinding {
            public CommandLineParameterAttribute Attribute;
            public bool HasBeenSet;
            public System.Reflection.MethodInfo ParserMethod;
            public System.Reflection.PropertyInfo PropertyInfo;
        }
    }
}