using System;
using System.Runtime.CompilerServices;

namespace Windows.UI.Xaml
{
	static class DependencyPropertyExtensions
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static object GetDefaultValue(this DependencyProperty dp, Type type)
		{
			var metadata = dp.GetMetadata(type);
			return metadata?.CreateDefaultValueCallback?.Invoke() ?? metadata?.DefaultValue ?? null;
		}

		public static void OnPropertyChanged(this DependencyProperty dp, DependencyObject dependencyObject, object oldValue, object newValue)
		{
			var metadata = dp.GetMetadata(dependencyObject.GetType());
			metadata?.OnPropertyChanged(dependencyObject, oldValue, newValue, dp);
		}
	}
}