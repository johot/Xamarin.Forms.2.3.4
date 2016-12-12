using System.Linq;
using NUnit.Framework;
using Xamarin.Forms.Controls;
using Xamarin.UITest.Desktop;

namespace Xamarin.Forms.Core.macOS.UITests
{

	public class Tests
	{
		[TestFixture]
		public class Test
		{

			//[OneTimeSetUp]
			//public void RunBeforeAnyTests()
			//{
			//	TestAgent.Start();
			//}

			//[OneTimeTearDown]
			//public void RunAfterAnyTests()
			//{
			//	TestAgent.Stop();
			//}
			CocoaApp app;

			[SetUp]
			public void SetUp()
			{
				var configurator = new CocoaAppConfigurator();
				app = configurator
							//.AppBundle("/Users/rmarinho/Xamarin/Xamarin.Forms/Xamarin.Forms.ControlGallery.MacOS/bin/Debug/Xamarin.Forms.ControlGallery.MacOS.app")
							.AppBundle(AppPaths.MacOSPath)
							.BundleId(AppPaths.MacOSBundleId)
							.StartApp();
			}

			[Test]
			public void TestLaunchApp()
			{
				var title = app.QueryByText("Go to Test Cases");
				Assert.True(title.First().Text == "Go to Test Cases",
						"Query by text doesn't work.");

			}
		}
	}
}
