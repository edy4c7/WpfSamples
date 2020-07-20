using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Edy4c7.WpfSamples.StopWatch.Models;
using Edy4c7.WpfSamples.StopWatch.Models.Infrastructures;
using Edy4c7.WpfSamples.StopWatch.ViewModels;
using MR = MicroResolver;

namespace Edy4c7.WpfSamples.StopWatch.ObjectResolvers
{
	/// <summary>
	/// オブジェクトの依存関係管理クラス
	/// </summary>
	public static class ObjectResolver
	{
		/// <summary>
		/// MicroResolver.ObjectResolverのインスタンス
		/// </summary>
		public static MR.ObjectResolver Instance { get; }
			= MR.ObjectResolver.Create();

		/// <summary>
		/// 静的コンストラクタ
		/// MicroResolverの設定及びViewModelResolverの設定を行う
		/// </summary>
		static ObjectResolver()
		{
			Instance.Register<IStopwatch, Stopwatch>();
			Instance.Register<IStopwatchService, StopwatchService>();
			Instance.RegisterCollection<INotifyPropertyChanged>(typeof(MainWindowViewModel));

			ViewModelResolver.InjectViewModelChanged += ViewModelResolver_InjectViewModelChanged;
		}

		private static void ViewModelResolver_InjectViewModelChanged(object sender, InjectViewModelChangedEventArgs e)
		{
			var viewName = sender.GetType().Name;
			var viewModelName = $"{viewName}{(viewName.EndsWith("View") ? "Model" : "ViewModel")}";
			var viewModel = Instance.Resolve<IEnumerable<INotifyPropertyChanged>>()
				.Where(vm => vm.GetType().Name == viewModelName)
				.First();
			e.Callback(viewModel);
		}
	}
}
