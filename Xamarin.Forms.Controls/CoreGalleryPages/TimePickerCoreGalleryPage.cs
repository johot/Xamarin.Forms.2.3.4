using System;
using Xamarin.Forms.CustomAttributes;

namespace Xamarin.Forms.Controls
{
	internal class TimePickerCoreGalleryPage : CoreGalleryPage<TimePicker>
	{
		protected override bool SupportsTapGestureRecognizer => false;

		protected override void Build(StackLayout stackLayout)
		{
			base.Build(stackLayout);
			var formatContainer = new ViewContainer<TimePicker>(Test.TimePicker.Format, new TimePicker { Format = "HH-mm-ss" });
			var timeContainer = new ViewContainer<TimePicker>(Test.TimePicker.Time,
				new TimePicker { Time = new TimeSpan(14, 45, 50) });
			var textColorContainer = new ViewContainer<TimePicker>(Test.TimePicker.TextColor,
				new TimePicker { Time = new TimeSpan(14, 45, 50), TextColor = Color.Lime });
			var fontAttributesContainer = new ViewContainer<TimePicker>(Test.TimePicker.FontAttributes,
				new TimePicker { FontAttributes = FontAttributes.Bold });

			var fontFamilyContainer = new ViewContainer<TimePicker>(Test.TimePicker.FontFamily,
				new TimePicker());
			// Set font family based on available fonts per platform
			Device.OnPlatform(
				Android: () => fontFamilyContainer.View.FontFamily = "sans-serif-thin",
				iOS: () => fontFamilyContainer.View.FontFamily = "Courier",
				Default: () => fontFamilyContainer.View.FontFamily = "Garamond");

			var fontSizeContainer = new ViewContainer<TimePicker>(Test.TimePicker.FontSize,
				new TimePicker { FontSize = 24 });

			Add(formatContainer);
			Add(timeContainer);
			Add(textColorContainer);
			Add(fontAttributesContainer);
			Add(fontFamilyContainer);
			Add(fontSizeContainer);
		}
	}
}