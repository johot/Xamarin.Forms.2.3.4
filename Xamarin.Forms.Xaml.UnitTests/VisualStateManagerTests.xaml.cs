using System.Linq;

using NUnit.Framework;

using Xamarin.Forms;
using Xamarin.Forms.Xaml.UnitTests;
using Xamarin.Forms.Core.UnitTests;

namespace Xamarin.Forms.Xaml.UnitTests
{
	public partial class VisualStateManagerTests : ContentPage
	{
		public VisualStateManagerTests()
		{
			InitializeComponent ();
		}

		public VisualStateManagerTests(bool useCompiledXaml)
		{
			//this stub will be replaced at compile time
		}

		[TestFixture]
		public class Tests
		{
			[SetUp]
			public void SetUp ()
			{
				Device.PlatformServices = new MockPlatformServices ();
				Application.Current = new MockApplication ();
			}

			[TestCase (false)]
			//[TestCase (true)]
			public void TestStyle (bool useCompiledXaml)
			{
				string xaml = @"<?xml version=""1.0"" encoding=""UTF - 8""?>
<ContentPage
xmlns=""http://xamarin.com/schemas/2014/forms""
xmlns:x=""http://schemas.microsoft.com/winfx/2009/xaml""
x:Class=""Xamarin.Forms.Xaml.UnitTests.VisualStateManagerTests"">
<ContentPage.Resources>
	<ResourceDictionary>
		<Style TargetType = ""Label"" >
			<Setter Property = ""VisualStateManager.VisualStateGroups"">
				<VisualStateGroup x:Name = ""Common"">
					<VisualState x:Name = ""Disabled"">
						<VisualState.Setters>
								
						</VisualState.Setters>
					</VisualState>
				</VisualStateGroup>
			</Setter>
		</Style>
	</ResourceDictionary>
</ContentPage.Resources>
	<StackLayout>
		<Label x:Name = ""label0"" />
	</StackLayout>
</ContentPage>";

				var page = new ContentPage();
				Assert.DoesNotThrow(() => page.LoadFromXaml(xaml));

				var layout = new VisualStateManagerTests(useCompiledXaml);
				Assert.AreEqual(layout.label0.TextColor, Color.Default);

				var movedToState = VisualStateManager.GoToState(layout.label0, "Disabled");

				Assert.True(movedToState);

				Assert.AreEqual(layout.label0.TextColor, Color.Red);
			}

			//[TestCase (false)]
			//[TestCase (true)]
			//public void TestConversionOnSetters (bool useCompiledXaml)
			//{
			//	var layout = new VisualStateManagerTests(useCompiledXaml);
			//	Style style = layout.style1;
			//	Setter setter;

			//	//Test built-in conversions
			//	setter = style.Setters.Single (s => s.Property == HeightProperty);
			//	Assert.That (setter.Value, Is.TypeOf<double> ());
			//	Assert.AreEqual (42d, (double)setter.Value);

			//	//Test TypeConverters
			//	setter = style.Setters.Single (s => s.Property == BackgroundColorProperty);
			//	Assert.That (setter.Value, Is.TypeOf<Color> ());
			//	Assert.AreEqual (Color.Pink, (Color)setter.Value);

			//	//Test implicit cast operator
			//	setter = style.Setters.Single (s => s.Property == Image.SourceProperty);
			//	Assert.That (setter.Value, Is.TypeOf<FileImageSource> ());
			//	Assert.AreEqual ("foo.png", ((FileImageSource)setter.Value).File);
			//}

			//[TestCase (false)]
			//[TestCase (true)]
			//public void ImplicitStyleAreApplied (bool useCompiledXaml)
			//{
			//	var layout = new VisualStateManagerTests(useCompiledXaml);
			//	Assert.AreEqual (Color.Red, layout.label1.TextColor);
			//}

			//[TestCase (false)]
			//[TestCase (true)]
			//public void PropertyDoesNotNeedTypes (bool useCompiledXaml)
			//{
			//	var layout = new VisualStateManagerTests(useCompiledXaml);
			//	Style style2 = layout.style2;
			//	var s0 = style2.Setters [0];
			//	var s1 = style2.Setters [1];
			//	Assert.AreEqual (Label.TextProperty, s0.Property);
			//	Assert.AreEqual (BackgroundColorProperty, s1.Property);
			//	Assert.AreEqual (Color.Red, s1.Value);
			//}

			//[TestCase (false)]
			//[TestCase (true)]
			////issue #2406
			//public void StylesDerivedFromDynamicStylesThroughStaticResource (bool useCompiledXaml)
			//{
			//	var layout = new VisualStateManagerTests(useCompiledXaml);
			//	Application.Current.MainPage = layout;

			//	var label = layout.labelWithStyleDerivedFromDynamic_StaticResource;

			//	Assert.AreEqual (50, label.FontSize);
			//	Assert.AreEqual (Color.Red, label.TextColor);
			//}

			//[TestCase (false)]
			//[TestCase (true)]
			////issue #2406
			//public void StylesDerivedFromDynamicStylesThroughDynamicResource (bool useCompiledXaml)
			//{
			//	var layout = new VisualStateManagerTests(useCompiledXaml);
			//	Application.Current.MainPage = layout;

			//	var label = layout.labelWithStyleDerivedFromDynamic_DynamicResource;

			//	Assert.AreEqual (50, label.FontSize);
			//	Assert.AreEqual (Color.Red, label.TextColor);
			//}
		}
	}
}