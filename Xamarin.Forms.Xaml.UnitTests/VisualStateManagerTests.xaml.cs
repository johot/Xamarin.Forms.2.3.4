using System.Diagnostics;
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
			[TestCase (true)]
			public void VisualStatesFromStyleXaml(bool useCompiledXaml)
			{
				var layout = new VisualStateManagerTests(useCompiledXaml);
				
				var entry0 = layout.Entry0;

				// Verify that Entry0 has no VisualStateGroups
				Assert.That(VisualStateManager.GetVisualStateGroups(entry0).Count == 0);
				Assert.AreEqual(Color.Default, entry0.TextColor);
				Assert.AreEqual(Color.Default, entry0.PlaceholderColor);

				var entry1 = layout.Entry1;

				// Verify that the correct groups are set up for Entry1
				var groups = VisualStateManager.GetVisualStateGroups(entry1);
				Assert.AreEqual(1, groups.Count);
				Assert.That(groups[0].Name == "CommonStates");
				Assert.Contains("Normal", groups[0].States.Select(state => state.Name).ToList());
				Assert.Contains("Disabled", groups[0].States.Select(state => state.Name).ToList());
				
				Assert.AreEqual(Color.Default, entry1.TextColor);
				Assert.AreEqual(Color.Default, entry1.PlaceholderColor);

				// Change the state of Entry1
				Assert.True(VisualStateManager.GoToState(entry1, "Disabled"));

				// And verify that the changes took
				Assert.AreEqual(Color.Gray, entry1.TextColor);
				Assert.AreEqual(Color.LightGray, entry1.PlaceholderColor);

				// Verify that Entry0 was unaffected
				Assert.AreEqual(Color.Default, entry0.TextColor);
				Assert.AreEqual(Color.Default, entry0.PlaceholderColor);
			}

			[TestCase(false)]
			[TestCase(true)]
			public void UnapplyVisualState(bool useCompiledXaml)
			{
				var layout = new VisualStateManagerTests(useCompiledXaml);
				var entry1 = layout.Entry1;

				Assert.AreEqual(Color.Default, entry1.TextColor);
				Assert.AreEqual(Color.Default, entry1.PlaceholderColor);

				// Change the state of Entry1
				Assert.True(VisualStateManager.GoToState(entry1, "Disabled"));

				// And verify that the changes took
				Assert.AreEqual(Color.Gray, entry1.TextColor);
				Assert.AreEqual(Color.LightGray, entry1.PlaceholderColor);

				// Now change it to Normal
				Assert.True(VisualStateManager.GoToState(entry1, "Normal"));

				// And verify that the changes reverted
				Assert.AreEqual(Color.Default, entry1.TextColor);
				Assert.AreEqual(Color.Default, entry1.PlaceholderColor);
			}

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