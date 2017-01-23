using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using WStyle = Windows.UI.Xaml.Style;

namespace Xamarin.Forms.Platform.WinRT
{
	internal sealed class WindowsPhoneResourcesProvider
		: ISystemResourcesProvider
	{
		readonly TextBlock _prototype = new TextBlock();

		public IResourceDictionary GetSystemResources()
		{
			return new ResourceDictionary
			{
				[Device.Styles.TitleStyleKey] = GetStyle("HeaderTextBlockStyle"),
				[Device.Styles.SubtitleStyleKey] = GetStyle("SubheaderTextBlockStyle"),
				[Device.Styles.BodyStyleKey] = GetStyle("BodyTextBlockStyle"),
				[Device.Styles.CaptionStyleKey] = GetStyle("BodyTextBlockStyle"),
				[Device.Styles.ListItemTextStyleKey] = GetStyle("ListViewItemTextBlockStyle"),
				[Device.Styles.ListItemDetailTextStyleKey] = GetStyle("ListViewItemContentTextBlockStyle")
			};
		}

		Style GetStyle(object nativeKey)
		{
			var style = (WStyle)Windows.UI.Xaml.Application.Current.Resources[nativeKey];

			_prototype.Style = style;

			var formsStyle = new Style(typeof(Label));

			formsStyle.Setters.Add(Label.FontSizeProperty, _prototype.FontSize);
			formsStyle.Setters.Add(Label.FontFamilyProperty, _prototype.FontFamily.Source);
			formsStyle.Setters.Add(Label.FontAttributesProperty, ToAttributes(_prototype.FontWeight));
			formsStyle.Setters.Add(Label.LineBreakModeProperty, ToLineBreakMode(_prototype.TextWrapping));

			return formsStyle;
		}

		static FontAttributes ToAttributes(FontWeight fontWeight)
		{
			if (fontWeight.Weight == FontWeights.Bold.Weight || fontWeight.Weight == FontWeights.SemiBold.Weight 
				|| fontWeight.Weight == FontWeights.ExtraBold.Weight)
			{
				return FontAttributes.Bold;
			}

			return FontAttributes.None;
		}

		static LineBreakMode ToLineBreakMode(TextWrapping value)
		{
			switch (value)
			{
				case TextWrapping.Wrap:
					return LineBreakMode.CharacterWrap;
				case TextWrapping.WrapWholeWords:
					return LineBreakMode.WordWrap;
				default:
				case TextWrapping.NoWrap:
					return LineBreakMode.NoWrap;
			}
		}
	}
}