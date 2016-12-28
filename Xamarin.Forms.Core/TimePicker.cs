using System;
using Xamarin.Forms.Platform;

namespace Xamarin.Forms
{
	[RenderWith(typeof(_TimePickerRenderer))]
	public class TimePicker : View, IFontElement, IElementConfiguration<TimePicker>
	{
		public static readonly BindableProperty FormatProperty = BindableProperty.Create(nameof(Format), typeof(string), typeof(TimePicker), "t");

		public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(TimePicker), Color.Default);

		public static readonly BindableProperty TimeProperty = BindableProperty.Create(nameof(Time), typeof(TimeSpan), typeof(TimePicker), new TimeSpan(0), BindingMode.TwoWay, (bindable, value) =>
		{
			var time = (TimeSpan)value;
			return time.TotalHours < 24 && time.TotalMilliseconds >= 0;
		});

		public static readonly BindableProperty FontAttributesProperty = BindableProperty.Create(nameof(FontAttributes), typeof(FontAttributes), typeof(TimePicker), FontAttributes.None);

		public static readonly BindableProperty FontFamilyProperty = BindableProperty.Create(nameof(FontFamily), typeof(string), typeof(TimePicker), default(string));

		public static readonly BindableProperty FontSizeProperty =
			BindableProperty.Create(nameof(FontSize), typeof(double), typeof(TimePicker), -1.0, defaultValueCreator: bindable => Device.GetNamedSize(NamedSize.Default, (TimePicker)bindable));

		readonly Lazy<PlatformConfigurationRegistry<TimePicker>> _platformConfigurationRegistry;

		public TimePicker()
		{
			_platformConfigurationRegistry = new Lazy<PlatformConfigurationRegistry<TimePicker>>(() => new PlatformConfigurationRegistry<TimePicker>(this));
		}

		public string Format
		{
			get { return (string)GetValue(FormatProperty); }
			set { SetValue(FormatProperty, value); }
		}

		public Color TextColor
		{
			get { return (Color)GetValue(TextColorProperty); }
			set { SetValue(TextColorProperty, value); }
		}

		public TimeSpan Time
		{
			get { return (TimeSpan)GetValue(TimeProperty); }
			set { SetValue(TimeProperty, value); }
		}

		public FontAttributes FontAttributes
		{
			get { return (FontAttributes)GetValue(FontAttributesProperty); }
			set { SetValue(FontAttributesProperty, value); }
		}

		public string FontFamily
		{
			get { return (string)GetValue(FontFamilyProperty); }
			set { SetValue(FontFamilyProperty, value); }
		}

		[TypeConverter(typeof(FontSizeConverter))]
		public double FontSize
		{
			get { return (double)GetValue(FontSizeProperty); }
			set { SetValue(FontSizeProperty, value); }
		}
		
		public IPlatformElementConfiguration<T, TimePicker> On<T>() where T : IConfigPlatform
		{
			return _platformConfigurationRegistry.Value.On<T>();
		}
	}
}