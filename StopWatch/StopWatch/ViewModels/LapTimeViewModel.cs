using System;
using System.Collections.Generic;
using System.Text;
using Livet;
using Reactive.Bindings;

namespace Edy4c7.WpfSamples.StopWatch.ViewModels
{
	public class LapTimeViewModel : ViewModel
	{
		public ReadOnlyReactiveProperty<int> CountOfLap { get; }
		public ReadOnlyReactiveProperty<TimeSpan> LapTime { get; }

		public LapTimeViewModel(int countOfLap, TimeSpan lapTime)
		{
			CountOfLap = new ReactiveProperty<int>(countOfLap)
				.ToReadOnlyReactiveProperty();
			LapTime = new ReactiveProperty<TimeSpan>(lapTime)
				.ToReadOnlyReactiveProperty();
		}
	}
}
