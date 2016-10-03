using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using AppKit;
using CoreAnimation;
using Foundation;
using Xamarin.Forms.Internals;

namespace Xamarin.Forms.Platform.MacOS
{
	public class NavigationPageRenderer : NSViewController, IVisualElementRenderer, IEffectControlProvider
	{
		bool _disposed;
		bool _appeared;
		string _previousTitle;
		string _currentTitle;
		EventTracker _events;
		VisualElementTracker _tracker;
		Stack<PageWrapper> _currentStack = new Stack<PageWrapper>();
		NSView _currentView;
		CATransition _transition;

		internal readonly ToolbarTracker _toolbarTracker = new ToolbarTracker();

		NSToolbar _toolbar => NSApplication.SharedApplication.MainWindow.Toolbar;

		IPageController PageController => Element as IPageController;

		IElementController ElementController => Element as IElementController;

		INavigationPageController NavigationController => Element as INavigationPageController;

		void IEffectControlProvider.RegisterEffect(Effect effect)
		{
			var platformEffect = effect as PlatformEffect;
			if (platformEffect != null)
				platformEffect.Container = View;
		}

		public NavigationPageRenderer() : this(IntPtr.Zero) { }
		public NavigationPageRenderer(IntPtr handle)
		{
			View = new FormsNSView { WantsLayer = true };
			_toolbarTracker.CollectionChanged += ToolbarTrackerOnCollectionChanged;
		}

		public VisualElement Element { get; private set; }

		public event EventHandler<VisualElementChangedEventArgs> ElementChanged;

		public SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint)
		{
			return NativeView.GetSizeRequest(widthConstraint, heightConstraint);
		}

		public NSView NativeView => View;

		public void SetElement(VisualElement element)
		{
			var oldElement = Element;
			Element = element;

			Init();

			OnElementChanged(new VisualElementChangedEventArgs(oldElement, element));

			EffectUtilities.RegisterEffectControlProvider(this, oldElement, element);
		}

		public void SetElementSize(Size size)
		{
			Element.Layout(new Rectangle(Element.X, Element.Y, size.Width, size.Height));
		}

		public NSViewController ViewController
		{
			get { return this; }
		}

		public Task<bool> PopToRootAsync(Page page, bool animated = true)
		{
			return OnPopToRoot(page, animated);
		}

		public Task<bool> PopViewAsync(Page page, bool animated = true)
		{
			return Task.FromResult(OnPop(page, animated));
		}

		public Task<bool> PushPageAsync(Page page, bool animated = true)
		{
			return Task.FromResult(OnPush(page, animated));
		}

		protected override void Dispose(bool disposing)
		{
			if (!_disposed && disposing)
			{
				if (Element != null)
				{
					PageController?.SendDisappearing();
					((Element as IPageContainer<Page>)?.CurrentPage as IPageController)?.SendDisappearing();
					Element.PropertyChanged -= HandlePropertyChanged;
					Element = null;
				}

				_tracker?.Dispose();
				_tracker = null;

				_events?.Dispose();
				_events = null;

				if (_toolbarTracker != null)
				{
					_toolbarTracker.CollectionChanged -= ToolbarTrackerOnCollectionChanged;
				}
				_disposed = true;
			}
			base.Dispose(disposing);
		}

		public override void ViewWillDisappear()
		{
			if (_toolbar != null && _toolbar.Visible)
				_toolbar.Visible = false;
			base.ViewWillDisappear();
		}

		public override void ViewDidDisappear()
		{
			base.ViewDidDisappear();

			if (!_appeared)
				return;

			_appeared = false;
			PageController?.SendDisappearing();
		}

		public override void ViewDidAppear()
		{
			base.ViewDidAppear();

			if (_appeared)
				return;

			_appeared = true;
			PageController?.SendAppearing();
		}

		public override void ViewWillAppear()
		{
			if (_toolbar != null && !_toolbar.Visible)
				_toolbar.Visible = true;
			base.ViewWillAppear();
		}

		protected virtual void OnElementChanged(VisualElementChangedEventArgs e)
		{
			if (e.OldElement != null)
				e.OldElement.PropertyChanged -= HandlePropertyChanged;

			if (e.NewElement != null)
				e.NewElement.PropertyChanged += HandlePropertyChanged;

			ElementChanged?.Invoke(this, e);
		}

		protected virtual NSToolbar ConfigureToolbar()
		{
			var toolbar = new NSToolbar("MainToolbar")
			{
				Delegate = new MainToolBarDelegate(GetCurrentPageTitle, GetPreviousPageTitle, GetToolbarItems, NavigateBackFrombackButton)
			};
			toolbar.DisplayMode = NSToolbarDisplayMode.Icon;
			return toolbar;
		}

		protected virtual void ConfigurePageRenderer()
		{
			_transition = new CATransition();
			_transition.Type = CAAnimation.TransitionPush;
			_transition.Subtype = CAAnimation.TransitionFromLeft;
			View.WantsLayer = true;
		}

		//TODO: Implement PopToRoot
		protected virtual async Task<bool> OnPopToRoot(Page page, bool animated)
		{
			var renderer = Platform.GetRenderer(page);
			if (renderer == null || renderer.ViewController == null)
				return false;

			var success = false;

			UpdateToolBarVisible();
			return success;
		}

		protected virtual bool OnPop(Page page, bool animated)
		{
			var removed = true;
			RemovePage(page);
			UpdateToolBarVisible();
			return removed;
		}

		protected virtual bool OnPush(Page page, bool animated)
		{
			var shown = true;
			AddPage(page);
			UpdateToolBarVisible();
			return shown;
		}

		void Init()
		{
			ConfigurePageRenderer();

			var navPage = (NavigationPage)Element;

			if (navPage.CurrentPage == null)
				throw new InvalidOperationException("NavigationPage must have a root Page before being used. Either call PushAsync with a valid Page, or pass a Page to the constructor before usage.");

			var navController = ((INavigationPageController)navPage);

			navController.PushRequested += OnPushRequested;
			navController.PopRequested += OnPopRequested;
			navController.PopToRootRequested += OnPopToRootRequested;
			navController.RemovePageRequested += OnRemovedPageRequested;
			navController.InsertPageBeforeRequested += OnInsertPageBeforeRequested;

			UpdateBarBackgroundColor();
			UpdateBarTextColor();

			_events = new EventTracker(this);
			_events.LoadEvents(NativeView);
			_tracker = new VisualElementTracker(this);

			((INavigationPageController)navPage).StackCopy.Reverse().ForEach(async p => await PushPageAsync(p, false));

			UpdateBackgroundColor();
		}

		IVisualElementRenderer CreateViewControllerForPage(Page page)
		{
			if (Platform.GetRenderer(page) == null)
				Platform.SetRenderer(page, Platform.CreateRenderer(page));

			var pageRenderer = Platform.GetRenderer(page);
			return pageRenderer;
		}

		void InsertPageBefore(Page page, Page before)
		{
			if (before == null)
				throw new ArgumentNullException(nameof(before));
			if (page == null)
				throw new ArgumentNullException(nameof(page));

		}

		void OnInsertPageBeforeRequested(object sender, NavigationRequestedEventArgs e)
		{
			InsertPageBefore(e.Page, e.BeforePage);
		}

		void OnPopRequested(object sender, NavigationRequestedEventArgs e)
		{
			e.Task = PopViewAsync(e.Page, e.Animated);
		}

		void OnPopToRootRequested(object sender, NavigationRequestedEventArgs e)
		{
			e.Task = PopToRootAsync(e.Page, e.Animated);
		}

		void OnPushRequested(object sender, NavigationRequestedEventArgs e)
		{
			e.Task = PushPageAsync(e.Page, e.Animated);
		}

		void OnRemovedPageRequested(object sender, NavigationRequestedEventArgs e)
		{
			RemovePage(e.Page);
		}

		void RemovePage(Page page)
		{
			if (page == null)
				throw new ArgumentNullException(nameof(page));

			var wrapper = _currentStack.Peek();
			if (page != wrapper.Page)
				throw new NotSupportedException("Popped page does not appear on top of current navigation stack, please file a bug.");

			_currentStack.Pop();

			var target = Platform.GetRenderer(page);
			target.NativeView.RemoveFromSuperview();
			target.Dispose();

			ShowHidePreviousPage(false);

			UpdateTitles(_currentStack.Peek().Page);
			(page as IPageController)?.SendDisappearing();

		}

		void AddPage(Page page)
		{
			if (page == null)
				throw new ArgumentNullException(nameof(page));

			var wrapper = new PageWrapper(page);

			_currentStack.Push(wrapper);
			ShowHidePreviousPage(false);

			var vc = CreateViewControllerForPage(page);
			page.Layout(new Rectangle(0, 0, View.Bounds.Width, View.Frame.Height));

			SetCurrentView(vc.NativeView);

			UpdateTitles(page);
			(page as IPageController)?.SendAppearing();
		}

		void UpdateTitles(Page page)
		{
			_currentTitle = page.Title;
			if (_currentStack.Count <= 1)
				_previousTitle = "";
			else
				_previousTitle = NavigationPage.GetHasBackButton(page) ? NavigationPage.GetBackButtonTitle(_currentStack.ElementAt(_currentStack.Count - 2).Page) ?? _currentStack.ElementAt(_currentStack.Count - 1).Page.Title : "";
		}


		void UpdateBackgroundColor()
		{
			if (!(View is FormsNSView))
				return;
			var color = Element.BackgroundColor == Color.Default ? Color.White : Element.BackgroundColor;
			(View as FormsNSView).BackgroundColor = color.ToNSColor();
		}

		void UpdateToolBarVisible()
		{
			if (!_currentStack.Any())
				return;

			if (NavigationPage.GetHasNavigationBar(_currentStack.Peek().Page))
			{
				if (_toolbar == null)
				{
					NSApplication.SharedApplication.MainWindow.Toolbar = ConfigureToolbar();
					_toolbar.Visible = false;
				}
				UpdateToolbarItems();

			}
			else
			{
				if (_toolbar != null && _toolbar.Visible)
				{
					NSApplication.SharedApplication.MainWindow.ToggleToolbarShown(this);
				}
			}
		}
		void UpdateToolbarItems()
		{
			_toolbarTracker.Target = _currentStack.Peek().Page;
			var nItems = _toolbar.Items.Length;
			for (int i = nItems - 1; i >= 0; i--)
			{
				_toolbar.RemoveItem(i);
			}
			_toolbar.InsertItem(MainToolBarDelegate.BackButtonIdentifier, 0);
			_toolbar.InsertItem(NSToolbar.NSToolbarFlexibleSpaceItemIdentifier, 1);
			_toolbar.InsertItem(MainToolBarDelegate.TitleIdentifier, 2);
			_toolbar.InsertItem(NSToolbar.NSToolbarFlexibleSpaceItemIdentifier, 3);
			_toolbar.InsertItem(MainToolBarDelegate.ToolbarItemsIdentifier, 4);
		}

		async void NavigateBackFrombackButton()
		{
			await NavigationController?.PopAsyncInner(true, true);
		}

		string GetCurrentPageTitle()
		{
			return _currentTitle ?? "";
		}

		string GetPreviousPageTitle()
		{
			return _previousTitle ?? "";
		}

		List<ToolbarItem> GetToolbarItems()
		{
			return _toolbarTracker.ToolbarItems.ToList();
		}

		//TODO: Implement
		void UpdateBarBackgroundColor()
		{

		}

		//TODO: Implement
		void UpdateBarTextColor()
		{

		}

		void ShowHidePreviousPage(bool hidden)
		{
			if (View.Subviews.Count() > 0)
			{
				var previous = View.Subviews.Last();
				previous.Layer.Hidden = hidden;
				previous.Hidden = hidden;
			}
		}

		void ToolbarTrackerOnCollectionChanged(object sender, EventArgs eventArgs)
		{
			UpdateToolbarItems();
		}

		void SetCurrentView(NSView view)
		{
			view.WantsLayer = true;
			if (_currentView == null)
			{
				_currentView = view;
				View.AddSubview(_currentView, NSWindowOrderingMode.Above, null);
				View.Animations = NSDictionary.FromObjectAndKey(_transition, new NSString("subviews"));
				return;
			}

			View.AddSubview(view, NSWindowOrderingMode.Above, null);

			_currentView = view;
		}

		void HandlePropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (_tracker == null)
				return;

			if (e.PropertyName == NavigationPage.BarBackgroundColorProperty.PropertyName)
				UpdateBarBackgroundColor();
			else if (e.PropertyName == NavigationPage.BarTextColorProperty.PropertyName)
				UpdateBarTextColor();
			else if (e.PropertyName == VisualElement.BackgroundColorProperty.PropertyName)
				UpdateBackgroundColor();
		}
	}
}
