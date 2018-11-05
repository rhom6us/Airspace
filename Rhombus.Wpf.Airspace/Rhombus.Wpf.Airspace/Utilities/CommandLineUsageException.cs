using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Utilities {
    [Serializable]
    public class CommandLineUsageException : Exception {
        public CommandLineUsageException() { }

        public CommandLineUsageException(string message) : base(message) { }

        public CommandLineUsageException(string message, Exception innerException) : base(message, innerException) { }

        protected CommandLineUsageException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) {
            if (info != null) {
                //this.fUserName = info.GetString("fUserName");
            }
        }

        public override void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) {
            base.GetObjectData(info, context);

            if (info != null) {
                // info.AddValue("fUserName", this.fUserName);
            }
        }
    }
}