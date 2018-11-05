using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Threading {
    public class UIThreadPerfCounters : INotifyPropertyChanged {
        public UIThreadPerfCounters() {
            this.NumberOfSamples = 10;

            var sysinfo = new Win32.Kernel32.SYSTEM_INFO();
            Win32.NativeMethods.GetSystemInfo(ref sysinfo);
            this.NumberOfProcessors = (int) sysinfo.dwNumberOfProcessors;

            var prop = DesignerProperties.IsInDesignModeProperty;
            var isInDesignMode = (bool) DependencyPropertyDescriptor.FromProperty(prop, typeof(System.Windows.FrameworkElement)).Metadata.DefaultValue;
            if (!isInDesignMode)
                System.Windows.Media.CompositionTarget.Rendering += this.CompositionTarget_Rendering;
        }

        public int NumberOfProcessors { get; }

        public int NumberOfSamples {
            get => _samples.Count();

            set {
                var numberOfSamples = value >= 1
                    ? value
                    : 1;

                if (numberOfSamples != _samples.Count) {
                    _samples.Clear();
                    for (var i = 0; i < numberOfSamples; i++) {
                        _samples.Add(null);
                    }

                    var handler = PropertyChanged;
                    if (handler != null)
                        handler(this, new PropertyChangedEventArgs("NumberOfSamples"));
                }
            }
        }

        public double FPS {
            get => _fps;
            private set {
                if (value != _fps) {
                    _fps = value;

                    var handler = PropertyChanged;
                    if (handler != null)
                        handler(this, new PropertyChangedEventArgs("FPS"));
                }
            }
        }

        public long ProcessCycleTime {
            get => _processCycleTime;
            private set {
                if (value != _processCycleTime) {
                    _processCycleTime = value;

                    var handler = PropertyChanged;
                    if (handler != null) {
                        handler(this, new PropertyChangedEventArgs("ProcessCycleTime"));
                        handler(this, new PropertyChangedEventArgs("ProcessCyclePercentage"));
                        handler(this, new PropertyChangedEventArgs("IdleCyclePercentage"));
                    }
                }
            }
        }

        public long IdleCycleTime {
            get => _idleCycleTime;
            private set {
                if (value != _idleCycleTime) {
                    _idleCycleTime = value;

                    var handler = PropertyChanged;
                    if (handler != null) {
                        handler(this, new PropertyChangedEventArgs("IdleCycleTime"));
                        handler(this, new PropertyChangedEventArgs("ProcessCyclePercentage"));
                        handler(this, new PropertyChangedEventArgs("IdleCyclePercentage"));
                    }
                }
            }
        }

        public double ProcessCyclePercentage {
            get {
                // BIG assumption is that the only contributors to this
                // calculation are the activity of this process and the
                // activity of the idle threads.  If other processes are
                // consuming CPU, then this calculation is off.  Then we
                // would have to enumerate all running processes and
                // account for their usage too.
                var total = this.ProcessCycleTime + this.IdleCycleTime;
                return total > 0
                    ? this.ProcessCycleTime / (double) total
                    : 0;
            }
        }

        public double IdleCyclePercentage {
            get {
                // BIG assumption is that the only contributors to this
                // calculation are the activity of this process and the
                // activity of the idle threads.  If other processes are
                // consuming CPU, then this calculation is off.  Then we
                // would have to enumerate all running processes and
                // account for their usage too.
                var total = this.ProcessCycleTime + this.IdleCycleTime;
                return total > 0
                    ? this.IdleCycleTime / (double) total
                    : 0;
            }
        }

        private void CompositionTarget_Rendering(object sender, EventArgs e) {
            var renderingEventArgs = e as System.Windows.Media.RenderingEventArgs;

            if (renderingEventArgs.RenderingTime > _lastRenderingTime) {
                _lastRenderingTime = renderingEventArgs.RenderingTime;

                var frameCount = 1;

                long processCycleTime = 0;
                Win32.NativeMethods.QueryProcessCycleTime(new IntPtr(-1), ref processCycleTime);

                var idleCycleTimes = new long[this.NumberOfProcessors];
                var sizeIdleCycleTimes = this.NumberOfProcessors * System.Runtime.InteropServices.Marshal.SizeOf(typeof(long));
                Win32.NativeMethods.QueryIdleProcessorCycleTime(ref sizeIdleCycleTimes, idleCycleTimes);
                long idleCycleTime = 0;
                foreach (var time in idleCycleTimes) {
                    idleCycleTime += time;
                }

                // Remove the oldest sample.
                _samples.RemoveAt(0);

                // Add a new sample at the end.
                _samples.Add(new UIThreadPerfSample(_lastRenderingTime, frameCount, processCycleTime, idleCycleTime));

                // Update the perf counters by examining all of the samples.
                this.UpdatePerfCounters();
            }
        }

        private void UpdatePerfCounters() {
            TimeSpan? start = null;
            TimeSpan? end = null;
            var frameCount = 0;
            long startProcessCycleTime = 0;
            long startIdleCycleTime = 0;
            long endProcessCycleTime = 0;
            long endIdleCycleTime = 0;

            foreach (var sample in _samples) {
                if (sample != null) {
                    if (start == null) {
                        start = sample.SampleTime;
                        startProcessCycleTime = sample.ProcessCycleTime;
                        startIdleCycleTime = sample.IdleCycleTime;
                    }
                    else if (sample.SampleTime < start) {
                        start = sample.SampleTime;
                        startProcessCycleTime = sample.ProcessCycleTime;
                        startIdleCycleTime = sample.IdleCycleTime;
                    }

                    if (end == null) {
                        end = sample.SampleTime;
                        endProcessCycleTime = sample.ProcessCycleTime;
                        endIdleCycleTime = sample.IdleCycleTime;
                    }
                    else if (sample.SampleTime > end) {
                        end = sample.SampleTime;
                        endProcessCycleTime = sample.ProcessCycleTime;
                        endIdleCycleTime = sample.IdleCycleTime;
                    }

                    frameCount += sample.FrameCount;
                }
            }

            if (start != null && end != null) {
                var sampleSpan = end.Value - start.Value;
                this.FPS = frameCount / sampleSpan.TotalSeconds;
                this.ProcessCycleTime = endProcessCycleTime - startProcessCycleTime;
                this.IdleCycleTime = endIdleCycleTime - startIdleCycleTime;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private double _fps;

        private long _idleCycleTime;
        private TimeSpan _lastRenderingTime = TimeSpan.MinValue;

        private long _processCycleTime;


        private readonly List<UIThreadPerfSample> _samples = new List<UIThreadPerfSample>();
    }
}