using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Windows.Controls;
using Microsoft.Xaml.Behaviors;

namespace Edy4c7.WpfSamples.StopWatch.Views.Behaviors
{
	/// <summary>
	/// ListBoxで追加したItemの位置まで自動スクロールするBehavior<br/>
	/// Referenced https://qiita.com/naminodarie/items/ee7270ae4dae94424d68
	/// </summary>
	class ListBoxAutoScrollBehavior : Behavior<ListBox>
	{
		protected override void OnAttached()
		{
			base.OnAttached();
			((INotifyCollectionChanged)AssociatedObject.Items).CollectionChanged += OnCollectionChanged;

		}

		protected override void OnDetaching()
		{
			base.OnDetaching();
			((INotifyCollectionChanged)AssociatedObject.Items).CollectionChanged -= OnCollectionChanged;
		}

		private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.Action == NotifyCollectionChangedAction.Add)
			{
				AssociatedObject.ScrollIntoView(AssociatedObject.Items[e.NewStartingIndex]);
			}
		}
	}
}
