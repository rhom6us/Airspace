using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Threading {
    /// <summary>
    ///     This class represents a perf sample taken from a UI thread.  The
    ///     time stamp of the same is usually taken from the RenderingEventArgs
    /// </summary>
    internal class UIThreadPerfSample {
        public UIThreadPerfSample(TimeSpan sampleTime, int frameCount, long processCycleTime, long idleCycleTime) {
            this.SampleTime = sampleTime;
            this.FrameCount = frameCount;
            this.ProcessCycleTime = processCycleTime;
            this.IdleCycleTime = idleCycleTime;
        }

        public TimeSpan SampleTime { get; }
        public int FrameCount { get; }
        public long ProcessCycleTime { get; }
        public long IdleCycleTime { get; }
    }
}