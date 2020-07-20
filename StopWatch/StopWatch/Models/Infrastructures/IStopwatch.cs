using System;

namespace Edy4c7.WpfSamples.StopWatch.Models.Infrastructures
{
	public interface IStopwatch
	{
		TimeSpan Elapsed { get; }
		long ElapsedMilliseconds { get; }
		long ElapsedTicks { get; }
		bool IsRunning { get; }
		void Reset();
		void Restart();
		void Start();
		void Stop();
	}
}
