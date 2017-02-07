using System;
namespace Windows.UI.Xaml.Data
{
	public interface IValueConverter
	{
		object Convert(object value, Type targetType, object parameter, string language);
		object ConvertBack(object value, Type targetType, object parameter, string language);
	}
	
}