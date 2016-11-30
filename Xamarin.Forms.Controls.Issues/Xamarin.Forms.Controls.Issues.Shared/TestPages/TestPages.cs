using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using Xamarin.Forms.CustomAttributes;

#if UITEST
using NUnit.Framework;
using Xamarin.UITest;

#endif

namespace Xamarin.Forms.Controls
{
	internal static class AppPaths
    {
        public static string ApkPath = "../../../Xamarin.Forms.ControlGallery.Android/bin/Debug/AndroidControlGallery.AndroidControlGallery-Signed.apk";

		// Have to continue using the old BundleId for now; Test Cloud doesn't like
		// when you change the BundleId
        public static string BundleId = "com.xamarin.quickui.controlgallery";
    }

#if UITEST
	internal static class AppSetup
	{
		static IApp InitializeApp ()
		{
			IApp app = null;
#if __ANDROID__

			app = InitializeAndroidApp();

#elif __IOS__

			app = InitializeiOSApp();
#endif
			if (app == null)
				throw new NullReferenceException ("App was not initialized.");

			// Wrap the app in ScreenshotConditional so it only takes screenshots if the SCREENSHOTS symbol is specified
			return new ScreenshotConditionalApp(app);
		}

#if __ANDROID__
		static IApp InitializeAndroidApp()
		{
			return ConfigureApp.Android.ApkFile(AppPaths.ApkPath).Debug().StartApp();
		}
#endif

#if __IOS__
		static IApp InitializeiOSApp() 
		{ 
			// Running on a device
			var app = ConfigureApp.iOS.InstalledApp(AppPaths.BundleId).Debug()
				//Uncomment to run from a specific iOS SIM, get the ID from XCode -> Devices
				.StartApp();

			// Running on the simulator
			//var app = ConfigureApp.iOS
			//				  .PreferIdeSettings()
			//		  		  .AppBundle("../../../Xamarin.Forms.ControlGallery.iOS/bin/iPhoneSimulator/Debug/XamarinFormsControlGalleryiOS.app")
			//				  .Debug()
			//				  .StartApp();

			return app;
		}
#endif

		public static void NavigateToIssue (Type type, IApp app)
		{
			var typeIssueAttribute = type.GetTypeInfo ().GetCustomAttribute <IssueAttribute> ();

			string cellName = "";
			if (typeIssueAttribute.IssueTracker.ToString () != "None" &&
				typeIssueAttribute.IssueNumber != 1461 &&
				typeIssueAttribute.IssueNumber != 342) {
				cellName = typeIssueAttribute.IssueTracker.ToString ().Substring(0, 1) + typeIssueAttribute.IssueNumber.ToString ();
			} else {
				cellName = typeIssueAttribute.Description;
			}

			int maxAttempts = 2;
			int attempts = 0;
			
			while (attempts < maxAttempts)
			{
				attempts += 1;

				try
				{
					// Attempt the direct way of navigating to the test page
#if __ANDROID__

					if (bool.Parse((string)app.Invoke("NavigateToTest", cellName)))
					{
						return;
					}
#endif
#if __IOS__
				if (bool.Parse(app.Invoke("navigateToTest:", cellName).ToString()))
				{
					return;
				}
#endif
				}
				catch (Exception ex)
				{
					var debugMessage = $"Could not directly invoke test; using UI navigation instead. {ex}";

					System.Diagnostics.Debug.WriteLine(debugMessage);
					Console.WriteLine(debugMessage);
				}

				try
				{
					// Fall back to the "manual" navigation method
					app.Tap(q => q.Button("Go to Test Cases"));
					app.WaitForElement(q => q.Raw("* marked:'TestCasesIssueList'"));

					app.EnterText(q => q.Raw("* marked:'SearchBarGo'"), cellName);

					app.WaitForElement(q => q.Raw("* marked:'SearchButton'"));
					app.Tap(q => q.Raw("* marked:'SearchButton'"));

					return;
				}
				catch (Exception ex)
				{
					var debugMessage = $"Both navigation methods failed. {ex}";

					System.Diagnostics.Debug.WriteLine(debugMessage);
					Console.WriteLine(debugMessage);

					if (attempts < maxAttempts)
					{
						// Something has failed and we're stuck in a place where we can't navigate
						// to the test. Usually this is because we're getting network/HTTP errors 
						// communicating with the server on the device. So we'll try restarting the app.
						RunningApp = InitializeApp();
					}
					else
					{
						// But if it's still not working after [maxAttempts], we've got assume this is a legit
						// problem that restarting won't fix
						throw;
					}
				}
			}
		}

		public static IApp Setup (Type pageType = null)
		{
			IApp runningApp = null;
			try {
				runningApp = InitializeApp ();
			} catch (Exception e) {
				Assert.Inconclusive ($"App did not start for some reason: {e}");
			}
			
			if (pageType != null)
				NavigateToIssue (pageType, runningApp);

			return runningApp;
		}

		// Make sure the server on the device is still up and running;
		// if not, restart the app
		public static void EnsureConnection()
		{
			if (RunningApp != null)
			{
				try
				{
					RunningApp.TestServer.Get("version");
					return;
				}
				catch (Exception ex)
				{
				}

				RunningApp = InitializeApp();
			}
		}

		static int s_testsrun;
		const int ConsecutiveTestLimit = 30;

		// Until we get more of our memory leak issues worked out, restart the app 
		// after a specified number of tests so we don't get bogged down in GC
		// (or booted by jetsam)
		public static void EnsureMemory()
		{
			if (RunningApp != null)
			{
				ListAllocations();

				s_testsrun += 1;

				if (s_testsrun >= ConsecutiveTestLimit)
				{
					s_testsrun = 0;
					RunningApp = InitializeApp();
				}
			}
		}

		// For tests which just don't play well with others, we can ensure
		// that they run in their own instance of the application
		public static void BeginIsolate()
		{
			if (RunningApp != null && s_testsrun > 0)
			{
				s_testsrun = 0;
				RunningApp = InitializeApp();
			}
		}

		public static void EndIsolate()
		{
			s_testsrun = ConsecutiveTestLimit;
		}

		public static IApp RunningApp { get; set; }

		static readonly List<Type> s_typesAllocated = new List<Type>();

		public static void RegisterAllocation(Type type)
		{
			s_typesAllocated.Add(type);
			//Log.Warning("UI Tests", $">>>>>>>> Adding: {type}");
		}

		public static void UnregisterAllocation(Type type)
		{
			s_typesAllocated.Remove(type);
			//Log.Warning("UI Tests", $">>>>>>>> Removing: {type}");
		}

		static long s_lastMemoryCheck = 0;

		public static void ListAllocations()
		{
			GC.Collect();
			GC.WaitForPendingFinalizers();

			long currentMemory = GC.GetTotalMemory(false);

			Log.Warning("UI Tests", $">>>>>>>> Total memory: {currentMemory} (was: {s_lastMemoryCheck}, delta {currentMemory - s_lastMemoryCheck}");

			s_lastMemoryCheck = currentMemory;

			if (s_typesAllocated.Count > 0)
			{
				Log.Warning("UI Tests", $">>>>>>>> Test Pages Allocated:");
			}

			foreach (Type type in s_typesAllocated)
			{
				Log.Warning("UI Tests", $">>>>>>>> \t{type}");
			}

			
		}
	}
#endif

	public abstract class TestPage : Page
	{
#if UITEST
		public IApp RunningApp => AppSetup.RunningApp;

		protected virtual bool Isolate => false;
#endif

		protected TestPage ()
		{
#if UITEST
			AppSetup.RegisterAllocation(GetType());
#endif
#if APP
			Init ();
#endif
		}

#if UITEST
		~TestPage()
		{
			AppSetup.UnregisterAllocation(GetType());
		}
#endif

#if UITEST
		[SetUp]
		public void Setup()
		{
			if (Isolate)
			{
				AppSetup.BeginIsolate();
			}
			else
			{
				AppSetup.EnsureMemory();
				AppSetup.EnsureConnection();
			}

			AppSetup.NavigateToIssue(GetType(), RunningApp);
		}

		[TearDown]
		public void TearDown()
		{
			if (Isolate)
			{
				AppSetup.EndIsolate();
			}
		}
#endif

		protected abstract void Init ();
	}


	public abstract class TestContentPage : ContentPage
	{
#if UITEST
		public IApp RunningApp => AppSetup.RunningApp;

		protected virtual bool Isolate => false;
#endif

		protected TestContentPage ()
		{
#if UITEST
			AppSetup.RegisterAllocation(GetType());
#endif
#if APP
			Init ();
#endif
		}

#if UITEST
		~TestContentPage()
		{
			AppSetup.UnregisterAllocation(GetType());
		}
#endif

#if UITEST
		[SetUp]
		public void Setup ()
		{
			if (Isolate)
			{
				AppSetup.BeginIsolate();
			}
			else
			{
				AppSetup.EnsureMemory();
				AppSetup.EnsureConnection();
			}

			AppSetup.NavigateToIssue(GetType(), RunningApp);
		}

		[TearDown]
		public void TearDown()
		{
			if (Isolate)
			{
				AppSetup.EndIsolate();
			}
		}
#endif

		protected abstract void Init ();
	}

	public abstract class TestCarouselPage : CarouselPage
	{
#if UITEST
		public IApp RunningApp => AppSetup.RunningApp;

		protected virtual bool Isolate => false;
#endif

		protected TestCarouselPage ()
		{
#if UITEST
			AppSetup.RegisterAllocation(GetType());
#endif
#if APP
			Init ();
#endif
		}

#if UITEST
		~TestCarouselPage()
		{
			AppSetup.UnregisterAllocation(GetType());
		}
#endif

#if UITEST
		[SetUp]
		public void Setup()
		{
			if (Isolate)
			{
				AppSetup.BeginIsolate();
			}
			else
			{
				AppSetup.EnsureMemory();
				AppSetup.EnsureConnection();
			}

			AppSetup.NavigateToIssue(GetType(), RunningApp);
		}

		[TearDown]
		public void TearDown()
		{
			if (Isolate)
			{
				AppSetup.EndIsolate();
			}
		}
#endif

		protected abstract void Init ();
	}

	public abstract class TestMasterDetailPage : MasterDetailPage
	{
#if UITEST
		public IApp RunningApp => AppSetup.RunningApp;

		protected virtual bool Isolate => false;
#endif

		protected TestMasterDetailPage ()
		{
#if UITEST
			AppSetup.RegisterAllocation(GetType());
#endif
#if APP
			Init ();
#endif
		}

#if UITEST
		~TestMasterDetailPage()
		{
			AppSetup.UnregisterAllocation(GetType());
		}
#endif

#if UITEST
		[SetUp]
		public void Setup()
		{
			if (Isolate)
			{
				AppSetup.BeginIsolate();
			}
			else
			{
				AppSetup.EnsureMemory();
				AppSetup.EnsureConnection();
			}

			AppSetup.NavigateToIssue(GetType(), RunningApp);
		}

		[TearDown]
		public void TearDown()
		{
			if (Isolate)
			{
				AppSetup.EndIsolate();
			}
		}
#endif

		protected abstract void Init ();
	}

	public abstract class TestNavigationPage : NavigationPage
	{
#if UITEST
		public IApp RunningApp => AppSetup.RunningApp;

		protected virtual bool Isolate => false;
#endif

		protected TestNavigationPage ()
		{
#if UITEST
			AppSetup.RegisterAllocation(GetType());
#endif
#if APP
			Init ();
#endif
		}

#if UITEST
		~TestNavigationPage()
		{
			AppSetup.UnregisterAllocation(GetType());
		}
#endif

#if UITEST
		[SetUp]
		public void Setup()
		{
			if (Isolate)
			{
				AppSetup.BeginIsolate();
			}
			else
			{
				AppSetup.EnsureMemory();
				AppSetup.EnsureConnection();
			}

			AppSetup.NavigateToIssue(GetType(), RunningApp);
		}

		[TearDown]
		public void TearDown()
		{
			if (Isolate)
			{
				AppSetup.EndIsolate();
			}
		}
#endif

		protected abstract void Init ();
	}

	public abstract class TestTabbedPage : TabbedPage
	{
#if UITEST
		public IApp RunningApp => AppSetup.RunningApp;

		protected virtual bool Isolate => false;
#endif

		protected TestTabbedPage ()
		{
#if UITEST
			AppSetup.RegisterAllocation(GetType());
#endif
#if APP
			Init ();
#endif
		}

#if UITEST
		~TestTabbedPage()
		{
			AppSetup.UnregisterAllocation(GetType());
		}
#endif

#if UITEST
		[SetUp]
		public void Setup()
		{
			if (Isolate)
			{
				AppSetup.BeginIsolate();
			}
			else
			{
				AppSetup.EnsureMemory();
				AppSetup.EnsureConnection();
			}

			AppSetup.NavigateToIssue(GetType(), RunningApp);
		}

		[TearDown]
		public void TearDown()
		{
			if (Isolate)
			{
				AppSetup.EndIsolate();
			}
		}
#endif

		protected abstract void Init ();
	}
}

#if UITEST
namespace Xamarin.Forms.Controls.Issues
{
	using System;
	using NUnit.Framework;

	// Run setup once for all tests in the Xamarin.Forms.Controls.Issues namespace
	// (instead of once for each test)
	[SetUpFixture]
	public class IssuesSetup
	{
		[SetUp]
		public void RunBeforeAnyTests()
		{
			Log.Listeners.Add(new DelegateLogListener((c, m) => Console.WriteLine($"[{c}] {m}")));
			Log.Listeners.Add(new DelegateLogListener((c, m) => System.Diagnostics.Debug.WriteLine($"[{c}] {m}")));
			AppSetup.RunningApp = AppSetup.Setup(null);
		}

		[TearDown]
		public void RunAfterAllTests()
		{
			AppSetup.ListAllocations();
		}
	}
}
#endif
