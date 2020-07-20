using System;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using Moq;
using NUnit.Framework;
using Reactive.Bindings.Extensions;
using Edy4c7.WpfSamples.StopWatch.Models;
using Infrastructures = Edy4c7.WpfSamples.StopWatch.Models.Infrastructures;

namespace Edy4c7.WpfSamples.StopWatch.Test.Models
{
	public class TestStopwatchService
	{
		[SetUp]
		public void Setup()
		{
		}

		[Test]
		public void TestStart()
		{
			var mock = new Mock<Infrastructures.IStopwatch>();
			var called = false;
			mock.Setup(m => m.Start()).Callback(() => called = true);

			using (var sw = new StopwatchService(mock.Object))
			using (var are = new AutoResetEvent(false))
			{
				sw.ObserveProperty(x => x.Ellapsed, false)
					.Subscribe(_ => are.Set());

				sw.Start();

				Assert.IsTrue(called);
				Assert.IsTrue(are.WaitOne(10));
			}
		}

		[Test]
		public void TestStop()
		{
			var mock = new Mock<Infrastructures.IStopwatch>();
			var called = false;
			mock.Setup(m => m.Stop()).Callback(() => called = true);

			using (var sw = new StopwatchService(mock.Object))
			using (var are = new AutoResetEvent(false))
			{
				sw.ObserveProperty(x => x.Ellapsed, false)
					.Subscribe(_ => are.Set());

				sw.Start();
				are.WaitOne(10);
				sw.Stop();

				Assert.IsTrue(called);
				are.WaitOne(10);
				Assert.IsFalse(are.WaitOne(10));
			}
		}

		[Test]
		public void TestLap()
		{
			var mock = new Mock<Infrastructures.IStopwatch>();

			using (var sw = new StopwatchService(mock.Object))
			{
				sw.Start();

				mock.Setup(m => m.Elapsed).Returns(TimeSpan.FromMilliseconds(10));
				sw.Lap();
				mock.Setup(m => m.Elapsed).Returns(TimeSpan.FromMilliseconds(30));
				sw.Lap();
				mock.Setup(m => m.Elapsed).Returns(TimeSpan.FromMilliseconds(60));
				sw.Lap();
				mock.Setup(m => m.Elapsed).Returns(TimeSpan.FromMilliseconds(100));
				sw.Stop();

				Assert.AreEqual(10, sw.LapTimes[0].TotalMilliseconds);
				Assert.AreEqual(20, sw.LapTimes[1].TotalMilliseconds);
				Assert.AreEqual(30, sw.LapTimes[2].TotalMilliseconds);
				Assert.AreEqual(40, sw.LapTimes[3].TotalMilliseconds);
			}
		}

		[Test]
		public void TestReset()
		{
			var mock = new Mock<Infrastructures.IStopwatch>();
			var called = false;
			mock.Setup(m => m.Reset()).Callback(() => called = true);

			using (var sw = new StopwatchService(mock.Object))
			using (var are = new AutoResetEvent(false))
			{
				sw.ObserveProperty(x => x.Ellapsed, false)
					.Subscribe(_ => are.Set());

				sw.Start();
				sw.Lap();
				sw.Lap();
				sw.Lap();
				are.WaitOne(10);
				sw.Reset();

				Assert.IsTrue(called);
				Assert.IsTrue(are.WaitOne(10));
				Assert.AreEqual(TimeSpan.Zero, sw.Ellapsed);
				Assert.AreEqual(0, sw.LapTimes.Count);
			}
		}

		[Test]
		public void TestIsRunning()
		{
			var mock = new Mock<Infrastructures.IStopwatch>();
			var called = false;
			mock.Setup(m => m.IsRunning).Callback(() => called = true);

			using (var sw = new StopwatchService(mock.Object))
			using (var are = new AutoResetEvent(false))
			{
				sw.ObserveProperty(x => x.IsRunning, false)
					.Subscribe(_ => are.Set());

				var isRunning = sw.IsRunning;
				Assert.IsTrue(called);

				sw.Start();
				Assert.IsTrue(are.WaitOne(10));
				
				sw.Stop();
				Assert.IsTrue(are.WaitOne(10));
			}
		}

		[Test]
		public void TestEllapsed()
		{
			var mock = new Mock<Infrastructures.IStopwatch>();
			var called = false;
			mock.Setup(m => m.Elapsed).Callback(() => called = true);

			using (var sw = new StopwatchService(mock.Object))
			{
				var ellapsed = sw.Ellapsed;
				Assert.IsTrue(called);
			}
		}
	}
}