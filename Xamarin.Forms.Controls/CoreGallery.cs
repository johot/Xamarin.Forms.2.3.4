using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms.Controls.GalleryPages;
using Xamarin.Forms.CustomAttributes;
using Xamarin.Forms.Internals;

namespace Xamarin.Forms.Controls
{
	public static class Messages
	{
		public const string ChangeRoot = "com.xamarin.ChangeRoot";
	}

	internal class CoreCarouselPage : CarouselPage
	{
		public CoreCarouselPage ()
		{
			AutomationId = "CarouselPageRoot";
			Children.Add (new CoreRootPage (this, NavigationBehavior.PushModalAsync) { Title = "Page 1" });
			Children.Add (new CoreRootPage (this, NavigationBehavior.PushModalAsync) { Title = "Page 2" });
		}
	}

	internal class CoreContentPage : ContentPage
	{
		public CoreContentPage ()
		{
			AutomationId = "ContentPageRoot";
			Content = new StackLayout { Children = { new CoreRootView (), new CorePageView (this, NavigationBehavior.PushModalAsync) } };
		}
	}

	internal class CoreMasterDetailPage : MasterDetailPage
	{
		public CoreMasterDetailPage ()
		{
			AutomationId = "MasterDetailPageRoot";

			var toCrashButton = new Button {Text = "Crash Me"};

			var masterPage = new ContentPage {Title = "Menu", Icon = "bank.png", Content = toCrashButton};
			var detailPage = new CoreRootPage (this, NavigationBehavior.PushModalAsync) { Title = "DetailPage" };

			bool toggle = false;
			toCrashButton.Clicked += (sender, args) => {
				if (toggle)
					Detail = new ContentPage { BackgroundColor = Color.Green, };
				else
					Detail = detailPage;

				toggle = !toggle;
			};

			Master = masterPage;
			Detail = detailPage;
		}
	}

	internal class CoreNavigationPage : NavigationPage
	{
		public CoreNavigationPage ()
		{
			AutomationId = "NavigationPageRoot";

			BarBackgroundColor = Color.Maroon;
			BarTextColor = Color.Yellow;

			Device.StartTimer(TimeSpan.FromSeconds(2), () => {
				BarBackgroundColor = Color.Default;
				BarTextColor = Color.Default;

				return false;
			});

			Navigation.PushAsync (new CoreRootPage (this));
		}
	}

	[Preserve (AllMembers = true)]
	[Issue (IssueTracker.Github, 2456, "StackOverflow after reordering tabs in a TabbedPageView", PlatformAffected.All)]
	public class CoreTabbedPage : TestTabbedPage
	{
		protected override void Init ()
		{
		}
#if APP
		public CoreTabbedPage ()
		{
			AutomationId = "TabbedPageRoot";


			Device.StartTimer(TimeSpan.FromSeconds(6), () => {
				BarBackgroundColor = Color.Maroon;
				BarTextColor = Color.Yellow;

				Device.StartTimer(TimeSpan.FromSeconds(6), () => {
					BarBackgroundColor = Color.Default;
					BarTextColor = Color.Default;

					return false;
				});

				return false;
			});

			Children.Add(new CoreRootPage(this, NavigationBehavior.PushModalAsync) { Title = "Tab 1" });
			Children.Add(new CoreRootPage(this, NavigationBehavior.PushModalAsync) { Title = "Tab 2" });
			Children.Add(new NavigationPage(new Page())
				{
					Title = "Rubriques",
					Icon = "coffee.png",
					BarBackgroundColor = Color.Blue,
					BarTextColor = Color.Aqua
				});

			Children.Add(new NavigationPage(new Page())
				{
					Title = "Le Club"
				});

			Children.Add(new NavigationPage(new Page { Title = "Bookmarks" })
				{
					Title = "Bookmarks",
				});

			Children.Add(new NavigationPage(new Page { Title = "Alertes" })
				{
					Title = "Notifications",  
				});

			Children.Add(new NavigationPage(new Page { Title = "My account" })
				{
					Title = "My account",
				});

			Children.Add(new NavigationPage(new Page { Title = "About" })
				{
					Title = "About",
				});
		}
#endif

#if UITest
		[Test]
		[Issue (IssueTracker.Github, 2456, "StackOverflow after reordering tabs in a TabbedPageView", PlatformAffected.iOS)]
		public void TestReorderTabs ()
		{
			App.Tap (c => c.Marked("More"));
			App.Tap (c => c.Marked("Edit"));
			var bookmarks = App.Query (c => c.Marked ("Bookmarks"))[0];
			var notifications = App.Query (c => c.Marked ("Notifications"))[0];
			var tab2 = App.Query (c => c.Marked ("Tab 2"))[2];
			var rubriques = App.Query (c => c.Marked ("Rubriques"))[2];
			App.DragCoordinates (bookmarks.Rect.CenterX, bookmarks.Rect.CenterY, rubriques.Rect.CenterX, rubriques.Rect.CenterY);
			App.DragCoordinates (notifications.Rect.CenterX, notifications.Rect.CenterY, tab2.Rect.CenterX, tab2.Rect.CenterY);
			App.Tap (c => c.Marked("Done"));
			App.Tap (c => c.Marked("Tab 1"));
			App.Tap (c => c.Marked("Le Club"));
			App.Tap (c => c.Marked("Bookmarks"));
			App.Tap (c => c.Marked("Notifications"));
		}
#endif
	}

	[Preserve (AllMembers = true)]
	internal class CoreViewContainer
	{
		public string Name { get; private set; }
		public Type PageType { get; private set; }

		public CoreViewContainer (string name, Type pageType)
		{
			Name = name;
			PageType = pageType;
		}
	}

	public class CoreRootView : ListView
	{
		public CoreRootView ()
		{
			var roots = new [] {
				new CoreViewContainer ("SwapRoot - CarouselPage", typeof(CoreCarouselPage)), 
				new CoreViewContainer ("SwapRoot - ContentPage", typeof(CoreContentPage)),
				new CoreViewContainer ("SwapRoot - MasterDetailPage", typeof(CoreMasterDetailPage)),
				new CoreViewContainer ("SwapRoot - NavigationPage", typeof(CoreNavigationPage)),
				new CoreViewContainer ("SwapRoot - TabbedPage", typeof(CoreTabbedPage)),
			};

			var template = new DataTemplate (typeof(TextCell));
			template.SetBinding (TextCell.TextProperty, "Name");

			ItemTemplate = template;
			ItemsSource = roots;

#if PRE_APPLICATION_CLASS
			ItemSelected += (sender, args) => MessagingCenter.Send (this, Messages.ChangeRoot, ((CoreViewContainer)args.SelectedItem).PageType);
#else			
			ItemSelected += (sender, args) => {
				var app = Application.Current as App;
				if (app != null) {
					var page = (Page)Activator.CreateInstance (((CoreViewContainer)args.SelectedItem).PageType);
					app.SetMainPage (page);
				}		
			};
#endif
			SetValue(Accessibility.NameProperty, "SwapRoot");
		}
	}

	internal class CorePageView : ListView
	{
		public CorePageView (Page rootPage, NavigationBehavior navigationBehavior = NavigationBehavior.PushAsync)
		{
			public GalleryPageFactory(Func<Page> create, string title)
			{
				Realize = () =>
				{
					var p = create();
					p.Title = title;
					return p;
				};
			
				Title = title;
			}

			public Func<Page> Realize { get; set; }
			public string Title { get; set; }

			public override string ToString()
			{	
				// a11y: let Narrator read a friendly string instead of the default ToString()
				return Title;
			}
		}

		List<GalleryPageFactory> _pages = new List<GalleryPageFactory> {
				new GalleryPageFactory(() => new AccessibilityGallery(), "Accessibility"),
				new GalleryPageFactory(() => new PlatformSpecificsGallery(), "Platform Specifics"),
				new GalleryPageFactory(() => new NativeBindingGalleryPage(), "Native Binding Controls Gallery"),
				new GalleryPageFactory(() => new XamlNativeViews(), "Xaml Native Views Gallery"),
				new GalleryPageFactory(() => new AppLinkPageGallery(), "App Link Page Gallery"),
				new GalleryPageFactory(() => new NestedNativeControlGalleryPage(), "Nested Native Controls Gallery"),
				new GalleryPageFactory(() => new CellForceUpdateSizeGalleryPage(), "Cell Force Update Size Gallery"),
				new GalleryPageFactory(() => new AppearingGalleryPage(), "Appearing Gallery"),
				new GalleryPageFactory(() => new EntryCoreGalleryPage(), "Entry Gallery"),
				new GalleryPageFactory(() => new NavBarTitleTestPage(), "Titles And Navbar Windows"),
				new GalleryPageFactory(() => new PanGestureGalleryPage(), "Pan gesture Gallery"),
				new GalleryPageFactory(() => new PinchGestureTestPage(), "Pinch gesture Gallery"),
				new GalleryPageFactory(() => new AutomationIdGallery(), "AutomationID Gallery"),
				new GalleryPageFactory(() => new LayoutPerformanceGallery(), "Layout Perf Gallery"),
				new GalleryPageFactory(() => new ListViewSelectionColor(), "ListView SelectionColor Gallery"),
				new GalleryPageFactory(() => new AlertGallery(), "DisplayAlert Gallery"),
				new GalleryPageFactory(() => new ToolbarItems(), "ToolbarItems Gallery"),
				new GalleryPageFactory(() => new ActionSheetGallery(), "ActionSheet Gallery"),
				new GalleryPageFactory(() => new ActivityIndicatorCoreGalleryPage(), "ActivityIndicator Gallery"),
				new GalleryPageFactory(() => new BehaviorsAndTriggers(), "BehaviorsTriggers Gallery"),
				new GalleryPageFactory(() => new ContextActionsGallery(), "ContextActions List Gallery"),
				new GalleryPageFactory(() => new ContextActionsGallery (tableView: true), "ContextActions Table Gallery"),
				new GalleryPageFactory(() => new CoreBoxViewGalleryPage(), "BoxView Gallery"),
				new GalleryPageFactory(() => new ButtonCoreGalleryPage(), "Button Gallery"),
				new GalleryPageFactory(() => new DatePickerCoreGalleryPage(), "DatePicker Gallery"),
				new GalleryPageFactory(() => new EditorCoreGalleryPage(), "Editor Gallery"),
				new GalleryPageFactory(() => new FrameCoreGalleryPage(), "Frame Gallery"),
				new GalleryPageFactory(() => new ImageCoreGalleryPage(), "Image Gallery"),
				new GalleryPageFactory(() => new KeyboardCoreGallery(), "Keyboard Gallery"),
				new GalleryPageFactory(() => new LabelCoreGalleryPage(), "Label Gallery"),
				new GalleryPageFactory(() => new ListViewCoreGalleryPage(), "ListView Gallery"),
				new GalleryPageFactory(() => new OpenGLViewCoreGalleryPage(), "OpenGLView Gallery"),
				new GalleryPageFactory(() => new PickerCoreGalleryPage(), "Picker Gallery"),
				new GalleryPageFactory(() => new ProgressBarCoreGalleryPage(), "ProgressBar Gallery"),
				new GalleryPageFactory(() => new ScrollGallery(), "ScrollView Gallery"),
				new GalleryPageFactory(() => new ScrollGallery(ScrollOrientation.Horizontal), "ScrollView Gallery Horizontal"),
				new GalleryPageFactory(() => new ScrollGallery(ScrollOrientation.Both), "ScrollView Gallery 2D"),
				new GalleryPageFactory(() => new SearchBarCoreGalleryPage(), "SearchBar Gallery"),
				new GalleryPageFactory(() => new SliderCoreGalleryPage(), "Slider Gallery"),
				new GalleryPageFactory(() => new StepperCoreGalleryPage(), "Stepper Gallery"),
				new GalleryPageFactory(() => new SwitchCoreGalleryPage(), "Switch Gallery"),
				new GalleryPageFactory(() => new TableViewCoreGalleryPage(), "TableView Gallery"),
				new GalleryPageFactory(() => new TimePickerCoreGalleryPage(), "TimePicker Gallery"),
				new GalleryPageFactory(() => new WebViewCoreGalleryPage(), "WebView Gallery"),
				//pages
 				new RootContentPage ("Content") { Title = "RootPages Gallery" },
				new MasterDetailPageTabletPage { Title = "MasterDetailPage Tablet Page" },
				// legacy galleries
				new AbsoluteLayoutGallery { Title = "AbsoluteLayout Gallery - Legacy" }, 
				new BoundContentPage { Title = "BoundPage Gallery - Legacy" }, 
				new BackgroundImageGallery { Title = "BackgroundImage gallery" },
				new ButtonGallery { Title = "Button Gallery - Legacy" }, 
				new CarouselPageGallery { Title = "CarouselPage Gallery - Legacy" },
				new CellTypesListPage { Title = "Cells Gallery - Legacy" },
				new ClipToBoundsGallery { Title = "ClipToBounds Gallery - Legacy" }, 
				new ControlTemplatePage { Title = "ControlTemplated Gallery - Legacy" },
				new ControlTemplateXamlPage { Title = "ControlTemplated XAML Gallery - Legacy" },
				new DisposeGallery { Title = "Dispose Gallery - Legacy" }, 
				new EditorGallery { Title = "Editor Gallery - Legacy" },
				new EntryGallery { Title = "Entry Gallery - Legacy" }, 
				new FrameGallery  { Title = "Frame Gallery - Legacy" }, 
				new GridGallery { Title = "Grid Gallery - Legacy" }, 
				new GroupedListActionsGallery { Title = "GroupedListActions Gallery - Legacy" }, 
				new GroupedListContactsGallery { Title = "GroupedList Gallery - Legacy" },
				new ImageGallery  { Title = "Image Gallery - Legacy" },
				new ImageLoadingGallery  { Title = "ImageLoading Gallery - Legacy" },
				new InputIntentGallery { Title = "InputIntent Gallery - Legacy" },
				new LabelGallery { Title = "Label Gallery - Legacy" },
				new LayoutAddPerformance { Title = "Layout Add Performance - Legacy" },
				new LayoutOptionsGallery { Title = "LayoutOptions Gallery - Legacy" },
				new LineBreakModeGallery { Title = "LineBreakMode Gallery - Legacy" },
				new ListPage { Title = "ListView Gallery - Legacy" },
				new ListScrollTo { Title = "ListView.ScrollTo" },
				new ListRefresh { Title = "ListView.PullToRefresh" },
				new ListViewDemoPage { Title = "ListView Demo Gallery - Legacy" },
				new MapGallery { Title = "Map Gallery - Legacy" }, 
				new MinimumSizeGallery { Title = "MinimumSize Gallery - Legacy" },
				new MultiGallery { Title = "Multi Gallery - Legacy" },
				new NavigationMenuGallery { Title = "NavigationMenu Gallery - Legacy" },
				new NavigationPropertiesGallery { Title = "Navigation Properties" },
#if HAVE_OPENTK
				new OpenGLGallery { Title = "OpenGLGallery - Legacy" },
#endif
				new PickerGallery {Title = "Picker Gallery - Legacy"}, 
				new ProgressBarGallery { Title = "ProgressBar Gallery - Legacy" }, 
				new RelativeLayoutGallery { Title = "RelativeLayout Gallery - Legacy" },
				new ScaleRotate { Title = "Scale Rotate Gallery - Legacy" }, 
				new SearchBarGallery { Title = "SearchBar Gallery - Legacy" },
				new SettingsPage { Title = "Settings Page - Legacy" }, 
				new SliderGallery { Title = "Slider Gallery - Legacy" },
				new StackLayoutGallery { Title = "StackLayout Gallery - Legacy" }, 
				new StepperGallery { Title = "Stepper Gallery - Legacy" },
				new StyleGallery {Title = "Style Gallery"},
				new StyleXamlGallery {Title = "Style Gallery in Xaml"},
				new SwitchGallery { Title = "Switch Gallery - Legacy" }, 
				new TableViewGallery { Title = "TableView Gallery - Legacy" }, 
				new TemplatedCarouselGallery { Title = "TemplatedCarouselPage Gallery - Legacy" }, 
				new TemplatedTabbedGallery { Title = "TemplatedTabbedPage Gallery - Legacy" }, 
 				new UnevenViewCellGallery { Title = "UnevenViewCell Gallery - Legacy" }, 
				new UnevenListGallery { Title = "UnevenList Gallery - Legacy" }, 
				new ViewCellGallery { Title = "ViewCell Gallery - Legacy" }, 
				new WebViewGallery {Title = "WebView Gallery - Legacy"},
			};

			titleToPage = pages.ToDictionary (o => o.Title);

			// avoid NRE for root pages without NavigationBar
			if (navigationBehavior == NavigationBehavior.PushAsync && rootPage.GetType () == typeof (CoreNavigationPage)) {
				pages.Add (new NavigationBarGallery ((NavigationPage)rootPage) { Title = "NavigationBar Gallery - Legacy" });
			}

			var template = new DataTemplate (typeof(TextCell));
			template.SetBinding (TextCell.TextProperty, "Title");

			BindingContext = pages;
			ItemTemplate = template;
			ItemsSource = pages;

			ItemSelected += async (sender, args) => {
				if (SelectedItem == null)
					return;

				var item = args.SelectedItem;
				var page = item as Page;
				if (page != null)
					await PushPage (page);

				SelectedItem = null;
			};

			SetValue(Accessibility.NameProperty, "Core Pages");
		}

		NavigationBehavior navigationBehavior;

		async Task PushPage (Page contentPage)
		{
			if (Insights.IsInitialized) {
				Insights.Track ("Navigation", new Dictionary<string, string> {
					{ "Pushing", contentPage.GetType().Name }
				});
			}

			if (navigationBehavior == NavigationBehavior.PushModalAsync) {
				await Navigation.PushModalAsync (contentPage);
			} else {
				await Navigation.PushAsync (contentPage);
			}
		}

		Dictionary<string, Page> titleToPage = new Dictionary<string, Page>();
		public async Task PushPage (string pageTitle)
		{

			Page page = null;
			if (!titleToPage.TryGetValue (pageTitle, out page))
				return;

			if (Insights.IsInitialized) {
				Insights.Track ("Navigation", new Dictionary<string, string> {
					{ "Pushing", page.GetType().Name }
				});
			}

			await PushPage (page);
		}
	}

	internal class CoreRootPage : ContentPage
	{
		public CoreRootPage (Page rootPage, NavigationBehavior navigationBehavior = NavigationBehavior.PushAsync)
		{
			IStringProvider stringProvider = DependencyService.Get<IStringProvider> ();

			Title = stringProvider.CoreGalleryTitle;

			var corePageView = new CorePageView (rootPage, navigationBehavior);

			var searchBar = new SearchBar () {
				AutomationId = "SearchBar"
			};

			var testCasesButton = new Button {
				Text = "Go to Test Cases",
				AutomationId = "GoToTestButton",
				Command = new Command (async () => {
					if (!string.IsNullOrEmpty (searchBar.Text))
						await corePageView.PushPage (searchBar.Text);
					else
						await Navigation.PushModalAsync (TestCases.GetTestCases ());
				})
			};

			var stackLayout = new StackLayout () { 
				Children = {
					testCasesButton,
					searchBar,
					new Button {
						Text = "Click to Force GC", 
						Command = new Command(() => {
							GC.Collect ();
							GC.WaitForPendingFinalizers ();
							GC.Collect ();
						})
					}

				}
			};

			Content = new AbsoluteLayout {
				Children = {
					{ new CoreRootView (), new Rectangle(0, 0.0, 1, 0.35), AbsoluteLayoutFlags.All },
					{ stackLayout, new Rectangle(0, 0.5, 1, 0.30), AbsoluteLayoutFlags.All },
					{ corePageView, new Rectangle(0, 1.0, 1.0, 0.35), AbsoluteLayoutFlags.All },
				}
			};
		}
	}

	public interface IStringProvider
	{
		string CoreGalleryTitle { get; }
	}

	public static class CoreGallery
	{
		public static Page GetMainPage ()
		{
			return new CoreNavigationPage ();
		}
	}
}
