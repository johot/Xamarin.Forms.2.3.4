using System;
using AppKit;

[assembly: Xamarin.Forms.Dependency(typeof(Xamarin.Forms.Platform.MacOS.NativeValueConverterService))]

namespace Xamarin.Forms.Platform.MacOS
{
	class NativeValueConverterService : Xaml.INativeValueConverterService
	{
		public bool ConvertTo(object value, Type toType, out object nativeValue)
		{
			nativeValue = null;
			if (typeof(NSView).IsInstanceOfType(value) && toType.IsAssignableFrom(typeof(View)))
			{
				nativeValue = ((NSView)value).ToView();
				return true;
			}
			return false;
		}
	}
}
