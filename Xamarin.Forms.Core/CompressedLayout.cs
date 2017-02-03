using System;
namespace Xamarin.Forms
{
	public static class CompressedLayout
	{
		public static readonly BindableProperty IsHeadlessProperty =
			BindableProperty.Create("IsHeadless", typeof(bool), typeof(CompressedLayout), default(bool));

		public static bool GetIsHeadless(BindableObject bindable)
		{
			//return false;
			return (bool)bindable.GetValue(IsHeadlessProperty);
		}

		public static void SetIsHeadless(BindableObject bindable, bool value)
		{
			bindable.SetValue(IsHeadlessProperty, value);
		}
	}
}