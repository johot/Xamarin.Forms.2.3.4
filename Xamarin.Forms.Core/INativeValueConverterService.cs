using System;

namespace Xamarin.Forms.Xaml
{
	public interface INativeValueConverterService
	{
		bool ConvertTo(object value, Type toType, out object nativeValue);
	}
}