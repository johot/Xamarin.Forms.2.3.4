using System;
using Windows.Foundation;
namespace Windows.UI.Xaml
{

	public class FrameworkElement : UIElement
	{
		public event TypedEventHandler<FrameworkElement, DataContextChangedEventArgs> DataContextChanged;
	}
}
