using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.WindowsSpecific;

namespace Xamarin.Forms.Controls
{
	public class ZoomableScrollView : ScrollView
	{
		public static readonly BindableProperty CurrentZoomProperty = BindableProperty.Create(nameof(CurrentZoom), typeof(double), typeof(ZoomableScrollView), 1d);

		public double CurrentZoom
		{
			get { return (double)GetValue(CurrentZoomProperty); }
			set { SetValue(CurrentZoomProperty, value); }
		}

		public static readonly BindableProperty MaximumZoomProperty = BindableProperty.Create(nameof(MaxZoom), typeof(double), typeof(ZoomableScrollView), 1d);

		public double MaxZoom
		{
			get { return (double)GetValue(MaximumZoomProperty); }
			set { SetValue(MaximumZoomProperty, value); }
		}

		public static readonly BindableProperty IgnoreMinimumZoomProperty = BindableProperty.Create(nameof(IgnoreMinimumZoom), typeof(bool), typeof(ZoomableScrollView), false);

		public bool IgnoreMinimumZoom
		{
			get { return (bool)GetValue(IgnoreMinimumZoomProperty); }
			set { SetValue(IgnoreMinimumZoomProperty, value); }
		}
	}

	public class App : Application
	{
		public const string AppName = "XamarinFormsControls";
		static string s_insightsKey;

		// ReSharper disable once InconsistentNaming
		public static int IOSVersion = -1;

		public static List<string> AppearingMessages = new List<string>();

		static Dictionary<string, string> s_config;
		readonly ITestCloudService _testCloudService;

		public App()
		{
			_testCloudService = DependencyService.Get<ITestCloudService>();
			InitInsights();

			//MainPage = new MasterDetailPage
			//{
			//	Master = new ContentPage { Title = "Master", BackgroundColor = Color.Red },
			//	Detail = CoreGallery.GetMainPage()
			//};

			MainPage = new ContentPage
			{
				Content = new ZoomableScrollView
				{
					MaxZoom = 3,
					Orientation = ScrollOrientation.Both,
					Content = new Grid
					{
						WidthRequest = 1000,
						HeightRequest = 1000,
						ColumnSpacing = 10,
						RowSpacing = 10,
						Children =
						{
							{ new BoxView {Color = Color.Purple}, 0, 0 },
							{ new BoxView {Color = Color.Orange}, 0, 1 },
							{ new BoxView {Color = Color.Blue}, 0, 2 },
							{ new BoxView {Color = Color.Purple}, 1, 0 },
							{ new BoxView {Color = Color.Orange}, 1, 1 },
							{ new BoxView {Color = Color.Blue}, 1, 2 },
							{ new BoxView {Color = Color.Purple}, 2, 0 },
							{ new BoxView {Color = Color.Orange}, 2, 1 },
							{ new BoxView {Color = Color.Blue}, 2, 2 },
						}
					}
				}
			};
		}

		protected override void OnAppLinkRequestReceived(Uri uri)
		{
			var appDomain = "http://" + AppName.ToLowerInvariant() + "/";

			if (!uri.ToString().ToLowerInvariant().StartsWith(appDomain))
				return;

			var url = uri.ToString().Replace(appDomain, "");

			var parts = url.Split('/');
			if (parts.Length == 2)
			{
				var isPage = parts[0].Trim().ToLower() == "gallery";
				if (isPage)
				{
					string page = parts[1].Trim();
					var pageForms = Activator.CreateInstance(Type.GetType(page));

					var appLinkPageGallery = pageForms as AppLinkPageGallery;
					if (appLinkPageGallery != null)
					{
						appLinkPageGallery.ShowLabel = true;
						(MainPage as MasterDetailPage)?.Detail.Navigation.PushAsync((pageForms as Page));
					}
				}
			}

			base.OnAppLinkRequestReceived(uri);
		}

		public static Dictionary<string, string> Config
		{
			get
			{
				if (s_config == null)
					LoadConfig();

				return s_config;
			}
		}

		public static string InsightsApiKey
		{
			get
			{
				if (s_insightsKey == null)
				{
					string key = Config["InsightsApiKey"];
					s_insightsKey = string.IsNullOrEmpty(key) ? Insights.DebugModeKey : key;
				}

				return s_insightsKey;
			}
		}

		public static ContentPage MenuPage { get; set; }

		public void SetMainPage(Page rootPage)
		{
			MainPage = rootPage;
		}

		static Assembly GetAssembly(out string assemblystring)
		{
			assemblystring = typeof(App).AssemblyQualifiedName.Split(',')[1].Trim();
			var assemblyname = new AssemblyName(assemblystring);
			return Assembly.Load(assemblyname);
		}

		void InitInsights()
		{
			if (Insights.IsInitialized)
			{
				Insights.ForceDataTransmission = true;
				if (_testCloudService != null && _testCloudService.IsOnTestCloud())
					Insights.Identify(_testCloudService.GetTestCloudDevice(), "Name", _testCloudService.GetTestCloudDeviceName());
				else
					Insights.Identify("DemoUser", "Name", "Demo User");
			}
		}

		static void LoadConfig()
		{
			s_config = new Dictionary<string, string>();

			string keyData = LoadResource("controlgallery.config").Result;
			string[] entries = keyData.Split("\n\r".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
			foreach (string entry in entries)
			{
				string[] parts = entry.Split(':');
				if (parts.Length < 2)
					continue;

				s_config.Add(parts[0].Trim(), parts[1].Trim());
			}
		}

		static async Task<string> LoadResource(string filename)
		{
			string assemblystring;
			Assembly assembly = GetAssembly(out assemblystring);

			Stream stream = assembly.GetManifestResourceStream($"{assemblystring}.{filename}");
			string text;
			using (var reader = new StreamReader(stream))
				text = await reader.ReadToEndAsync();
			return text;
		}
	}
}