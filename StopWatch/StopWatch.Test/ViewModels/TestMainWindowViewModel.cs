using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reactive.Linq;
using System.Threading;
using Moq;
using NUnit.Framework;
using Reactive.Bindings.Extensions;
using Edy4c7.WpfSamples.StopWatch.Models;
using Edy4c7.WpfSamples.StopWatch.ViewModels;

namespace Edy4c7.WpfSamples.StopWatch.Test.ViewModels
{
	public class TestMainWindowViewModel
	{
		[SetUp]
		public void Setup()
		{
		}

		[Test]
		public void TestStartCommand()
		{
			var mock = new Mock<IStopwatchService>();
			mock.Setup(m => m.Start())
				.Callback(() =>
				{
					mock.Setup(m => m.Ellapsed).Returns(TimeSpan.FromMilliseconds(10));
					mock.Raise(m => m.PropertyChanged += null, new PropertyChangedEventArgs("Ellapsed"));
					mock.Setup(m => m.IsRunning).Returns(true);
					mock.Raise(m => m.PropertyChanged += null, new PropertyChangedEventArgs("IsRunning"));
				});

			using (var vm = new MainWindowViewModel(mock.Object))
			using (var are = new AutoResetEvent(false))
			{
				vm.Ellapsed.ObserveProperty(x => x.Value, false)
					.Subscribe(_ => are.Set());
				vm.StartCommand.Execute();

				Assert.IsTrue(are.WaitOne(10));
				Assert.IsFalse(vm.StartCommand.CanExecute());
			}
		}

		[Test]
		public void TestStopCommand()
		{
			var mock = new Mock<IStopwatchService>();
			mock.Setup(m => m.Start())
				.Callback(() =>
				{
					mock.Setup(m => m.Ellapsed).Returns(TimeSpan.FromMilliseconds(10));
					mock.Raise(m => m.PropertyChanged += null, new PropertyChangedEventArgs("Ellapsed"));
					mock.Setup(m => m.IsRunning).Returns(true);
					mock.Raise(m => m.PropertyChanged += null, new PropertyChangedEventArgs("IsRunning"));
				});

			using (var vm = new MainWindowViewModel(mock.Object))
			using (var are = new AutoResetEvent(false))
			{
				Assert.IsFalse(vm.StopCommand.CanExecute());

				vm.Ellapsed.ObserveProperty(x => x.Value, false)
					.Subscribe(_ => are.Set());
				vm.StartCommand.Execute();

				Assert.IsTrue(vm.StopCommand.CanExecute());

				are.WaitOne(10);
				vm.StopCommand.Execute();

				Assert.IsFalse(are.WaitOne(10));
			}
		}

		[Test]
		public void TestLapCommand()
		{
			var mock = new Mock<IStopwatchService>();
			mock.Setup(m => m.Start())
				.Callback(() =>
				{
					mock.Setup(m => m.IsRunning).Returns(true);
					mock.Raise(m => m.PropertyChanged += null, new PropertyChangedEventArgs("IsRunning"));
				});
			var lapTimes = new ObservableCollection<TimeSpan>();
			mock.Setup(m => m.LapTimes).Returns(lapTimes);
			mock.Setup(m => m.Lap())
				.Callback(() =>
				{
					lapTimes.Add(TimeSpan.FromMilliseconds(10));
				});
			mock.Setup(m => m.Stop())
				.Callback(() =>
				{
					mock.Setup(m => m.IsRunning).Returns(false);
					mock.Raise(m => m.PropertyChanged += null, new PropertyChangedEventArgs("IsRunning"));
				});

			using (var vm = new MainWindowViewModel(mock.Object))
			using (var are = new AutoResetEvent(false))
			{
				Assert.IsFalse(vm.LapCommand.CanExecute());

				vm.StartCommand.Execute();
				Assert.IsTrue(vm.LapCommand.CanExecute());

				vm.LapTimes.CollectionChangedAsObservable()
					.Subscribe((_) => are.Set());
				vm.LapCommand.Execute();
				Assert.IsTrue(are.WaitOne(10));
				vm.LapCommand.Execute();
				Assert.IsTrue(are.WaitOne(10));
				vm.LapCommand.Execute();
				Assert.IsTrue(are.WaitOne(10));

				vm.StopCommand.Execute();
				Assert.IsFalse(vm.LapCommand.CanExecute());

				Assert.AreEqual(3, vm.LapTimes.Count);
			}
		}

		[Test]
		public void TestResetCommand()
		{
			var mock = new Mock<IStopwatchService>();
			var lapTime = new ObservableCollection<TimeSpan>();
			lapTime.Add(TimeSpan.FromMilliseconds(10));
			lapTime.Add(TimeSpan.FromMilliseconds(10));
			lapTime.Add(TimeSpan.FromMilliseconds(10));
			mock.Setup(m => m.LapTimes).Returns(lapTime);
			mock.Setup(m => m.Reset()).Callback(() => mock.Object.LapTimes.Clear());

			using (var vm = new MainWindowViewModel(mock.Object))
			using (var are = new AutoResetEvent(false))
			{
				Assert.IsTrue(vm.ResetCommand.CanExecute());

				vm.Ellapsed.ObserveProperty(x => x.Value, false)
					.Subscribe(_ => are.Set());
				vm.ResetCommand.Execute();

				are.WaitOne(10);

				Assert.IsFalse(are.WaitOne(10));
				Assert.AreEqual(0, vm.LapTimes.Count);
			}
		}
	}
}