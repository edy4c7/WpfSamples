using System;
using System.ComponentModel;
using System.Windows;

namespace Edy4c7.WpfSamples.StopWatch.ObjectResolvers
{
	public class InjectViewModelChangedEventArgs : EventArgs
	{
		public Action<INotifyPropertyChanged> Callback { get; }

		public InjectViewModelChangedEventArgs(Action<INotifyPropertyChanged> callback)
		{
			this.Callback = callback;
		}
	}

	/// <summary>
	/// Referenced https://github.com/PrismLibrary/Prism/blob/master/src/Wpf/Prism.Wpf/Mvvm/ViewModelLocator.cs
	/// </summary>
	public static class ViewModelResolver
	{
		// Using a DependencyProperty as the backing store for InjectViewModel.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty InjectViewModelProperty =
			DependencyProperty.RegisterAttached("InjectViewModel", typeof(bool), typeof(ViewModelResolver), new PropertyMetadata(false, InjectViewModelPropertyChanged));

		public static event EventHandler<InjectViewModelChangedEventArgs> InjectViewModelChanged;

		public static bool GetInjectViewModel(DependencyObject obj)
		{
			return (bool)obj.GetValue(InjectViewModelProperty);
		}

		public static void SetInjectViewModel(DependencyObject obj, bool value)
		{
			obj.SetValue(InjectViewModelProperty, value);
		}
		
		private static void InjectViewModelPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if ((bool)e.NewValue)
			{
				InjectViewModelChanged?.Invoke(
					d, new InjectViewModelChangedEventArgs((vm) => ((FrameworkElement)d).DataContext = vm));
			}
		}
	}
}
