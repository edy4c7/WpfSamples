using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Edy4c7.WpfSamples.StopWatch.Models
{
	public interface IStopwatchService : INotifyPropertyChanged
	{
		TimeSpan Ellapsed { get; }
		ObservableCollection<TimeSpan> LapTimes { get; }
		bool IsRunning { get; }

		void Dispose();
		void Reset();
		void Start();
		void Stop();
		void Lap();
	}
}