using System;

namespace Edy4c7.WpfSamples.StopWatch.Models.Infrastructures
{
	public class Stopwatch : IStopwatch
	{
		private System.Diagnostics.Stopwatch stopwatch
			= new System.Diagnostics.Stopwatch();

		public TimeSpan Elapsed => stopwatch.Elapsed;

		public long ElapsedMilliseconds => stopwatch.ElapsedMilliseconds;

		public long ElapsedTicks => stopwatch.ElapsedTicks;

		public bool IsRunning => stopwatch.IsRunning;

		public void Reset() => stopwatch.Reset();

		public void Restart() => stopwatch.Restart();

		public void Start() => stopwatch.Start();

		public void Stop() => stopwatch.Stop();
	}
}
