using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using Livet;
using MicroResolver;
using Edy4c7.WpfSamples.StopWatch.Models.Infrastructures;

namespace Edy4c7.WpfSamples.StopWatch.Models
{
	public class StopwatchService : NotificationObject, IDisposable, IStopwatchService
	{
		private IStopwatch stopwatch;

		private IDisposable subscription;

		public bool IsRunning => stopwatch.IsRunning;

		public TimeSpan Ellapsed => stopwatch.Elapsed;

		private ObservableCollection<TimeSpan> _LapTimes = new ObservableCollection<TimeSpan>();

		public ObservableCollection<TimeSpan> LapTimes => _LapTimes;

		[Inject]
		public StopwatchService(IStopwatch stopwatch)
		{
			this.stopwatch = stopwatch;
		}

		public void Start()
		{
			stopwatch.Start();
			subscription = Observable.Timer(TimeSpan.Zero, TimeSpan.FromMilliseconds(10))
				.Subscribe(_ => RaisePropertyChanged(nameof(Ellapsed)));
			RaisePropertyChanged(nameof(IsRunning));
		}

		public void Stop()
		{
			stopwatch.Stop();
			subscription?.Dispose();
			RaisePropertyChanged(nameof(IsRunning));
			RaisePropertyChanged(nameof(Ellapsed));
			Lap();
		}

		public void Reset()
		{
			stopwatch.Reset();
			subscription?.Dispose();
			RaisePropertyChanged(nameof(IsRunning));
			RaisePropertyChanged(nameof(Ellapsed));
			LapTimes.Clear();
		}

		public void Lap()
		{
			var elapsed = Ellapsed;
			var total = LapTimes.Count != 0
				? LapTimes.Aggregate((total, t) => total + t)
				: TimeSpan.Zero;
			LapTimes.Add(elapsed - total);
		}

		#region IDisposable Support
		private bool isDisposed = false; // 重複する呼び出しを検出するには

		protected virtual void Dispose(bool disposing)
		{
			if (!isDisposed)
			{
				if (disposing)
				{
					// TODO: マネージ状態を破棄します (マネージ オブジェクト)。
					subscription?.Dispose();
				}

				// TODO: アンマネージ リソース (アンマネージ オブジェクト) を解放し、下のファイナライザーをオーバーライドします。
				// TODO: 大きなフィールドを null に設定します。

				isDisposed = true;
			}
		}

		// TODO: 上の Dispose(bool disposing) にアンマネージ リソースを解放するコードが含まれる場合にのみ、ファイナライザーをオーバーライドします。
		// ~Stopwatch()
		// {
		//   // このコードを変更しないでください。クリーンアップ コードを上の Dispose(bool disposing) に記述します。
		//   Dispose(false);
		// }

		// このコードは、破棄可能なパターンを正しく実装できるように追加されました。
		public void Dispose()
		{
			// このコードを変更しないでください。クリーンアップ コードを上の Dispose(bool disposing) に記述します。
			Dispose(true);
			// TODO: 上のファイナライザーがオーバーライドされる場合は、次の行のコメントを解除してください。
			// GC.SuppressFinalize(this);
		}
		#endregion
	}
}
