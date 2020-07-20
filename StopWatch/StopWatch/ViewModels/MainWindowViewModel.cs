using System;
using System.Linq;
using Livet;
using MicroResolver;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Edy4c7.WpfSamples.StopWatch.Models;

namespace Edy4c7.WpfSamples.StopWatch.ViewModels
{
	public class MainWindowViewModel : ViewModel
	{
		// Some useful code snippets for ViewModel are defined as l*(llcom, llcomn, lvcomm, lsprop, etc...).
		private IStopwatchService service;
		
		public ReadOnlyReactiveProperty<TimeSpan> Ellapsed { get; }

		public ReadOnlyReactiveCollection<LapTimeViewModel> LapTimes { get; }
		
		public ReactiveCommand StartCommand { get; }

		public ReactiveCommand StopCommand { get; }

		public ReactiveCommand ResetCommand { get; }

		public ReactiveCommand LapCommand { get; }

		[Inject]
		public MainWindowViewModel(IStopwatchService service)
		{
			this.service = service;

			Ellapsed = this.service.ObserveProperty(x => x.Ellapsed)
				.ToReadOnlyReactiveProperty()
				.AddTo(CompositeDisposable);

			LapTimes = this.service.LapTimes
				?.ToReadOnlyReactiveCollection(t => new LapTimeViewModel(this.service.LapTimes.Count, t));

			StartCommand = this.service.ObserveProperty(x => x.IsRunning)
				.Inverse()
				.ToReactiveCommand()
				.AddTo(CompositeDisposable);
			StartCommand.Subscribe(() => this.service.Start()).AddTo(CompositeDisposable);

			StopCommand = this.service.ObserveProperty(x => x.IsRunning)
				.ToReactiveCommand()
				.AddTo(CompositeDisposable);
			StopCommand.Subscribe(() => this.service.Stop()).AddTo(CompositeDisposable);

			ResetCommand = new ReactiveCommand().AddTo(CompositeDisposable);
			ResetCommand.Subscribe(() => this.service.Reset()).AddTo(CompositeDisposable);

			LapCommand = this.service.ObserveProperty(x => x.IsRunning)
				.ToReactiveCommand()
				.AddTo(CompositeDisposable);
			LapCommand.Subscribe(() => this.service.Lap()).AddTo(CompositeDisposable);
		}

		public void Initialize()
		{

		}
	}
}
