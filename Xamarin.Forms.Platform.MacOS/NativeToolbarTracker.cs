using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppKit;
using CoreGraphics;
using Foundation;
using Xamarin.Forms.Internals;

namespace Xamarin.Forms.Platform.MacOS
{
	class NativeToolbarGroup
	{
		public class Item
		{
			public NSToolbarItem ToolbarItem;
			public NSButton Button;
		}

		public NativeToolbarGroup(NSToolbarItemGroup itemGroup)
		{
			Group = itemGroup;
			Items = new List<Item>();
		}

		public NSToolbarItemGroup Group
		{
			get;
			private set;
		}

		public List<Item> Items
		{
			get;
			private set;
		}
	}

	internal class NativeToolbarTracker : NSToolbarDelegate
	{
		const string ToolBarId = "AwesomeBarToolbar";

		INavigationPageController NavigationController => _navigation as INavigationPageController;

		ToolbarTracker _toolbarTracker;
		NSToolbar _toolbar;
		NavigationPage _navigation;
		string _defaultBackButtonTitle = "Back";

		const double BackButtonItemWidth = 30;
		const double ToolbarItemWidth = 44;
		const double ToolbarItemHeight = 25;
		const double ToolbarItemSpacing = 6;
		const double ToolbarHeight = 30;

		const string NavigationGroupIdentifier = "NavigationGroup";
		const string TabbedGroupIdentifier = "TabbedGroup";
		const string ToolbarItemsGroupIdentifier = "ToolbarGroup";
		const string TitleGroupIdentifier = "TitleGroup";

		NativeToolbarGroup _navigationGroup;
		NativeToolbarGroup _tabbedGroup;
		NativeToolbarGroup _toolbarGroup;
		NativeToolbarGroup _titleGroup;

		public NativeToolbarTracker()
		{
			_toolbarTracker = new ToolbarTracker();
			_toolbarTracker.CollectionChanged += ToolbarTrackerOnCollectionChanged;
		}

		public NavigationPage Navigation
		{
			get { return _navigation; }
			set
			{
				if (_navigation == value)
					return;

				if (_navigation != null)
					_navigation.PropertyChanged -= NavigationPagePropertyChanged;

				_navigation = value;
				//_toolbarTracker.AdditionalTargets = _navigation.GetParentPages();

				if (_navigation != null)
				{
					_toolbarTracker.Target = _navigation.CurrentPage;
					_navigation.PropertyChanged += NavigationPagePropertyChanged;
				}

				UpdateToolBar();
			}
		}


		public void TryHide(NavigationPage navPage = null)
		{
			if (navPage == null || navPage == _navigation)
			{
				Navigation = null;
			}
		}

		public override string[] AllowedItemIdentifiers(NSToolbar toolbar)
		{
			return new string[] { };
		}

		public override string[] DefaultItemIdentifiers(NSToolbar toolbar)
		{
			return new string[] { };
		}

		public override NSToolbarItem WillInsertItem(NSToolbar toolbar, string itemIdentifier, bool willBeInserted)
		{
			var group = new NSToolbarItemGroup(itemIdentifier);
			var view = new NSView();
			group.View = view;

			if (itemIdentifier == NavigationGroupIdentifier)
				_navigationGroup = new NativeToolbarGroup(group);
			else if (itemIdentifier == TitleGroupIdentifier)
				_titleGroup = new NativeToolbarGroup(group);
			else if (itemIdentifier == TabbedGroupIdentifier)
				_tabbedGroup = new NativeToolbarGroup(group);
			else if (itemIdentifier == ToolbarItemsGroupIdentifier)
				_toolbarGroup = new NativeToolbarGroup(group);

			return group;
		}

		protected virtual bool HasTabs => false;

		protected virtual NSToolbar ConfigureToolbar()
		{
			var toolbar = new NSToolbar(ToolBarId)
			{
				DisplayMode = NSToolbarDisplayMode.Icon,
				AllowsUserCustomization = false,
				ShowsBaselineSeparator = true,
				SizeMode = NSToolbarSizeMode.Regular
			};

			toolbar.Delegate = this;
			return toolbar;
		}

		internal void UpdateToolBar()
		{
			if (NSApplication.SharedApplication.MainWindow == null)
				return;

			if (NavigationController == null)
			{
				if (_toolbar != null)
					_toolbar.Visible = false;
				_toolbar = null;
				return;
			}

			var currentPage = NavigationController.StackCopy.Peek();

			if (NavigationPage.GetHasNavigationBar(currentPage))
			{
				if (_toolbar == null)
				{
					_toolbar = ConfigureToolbar();
					NSApplication.SharedApplication.MainWindow.Toolbar = _toolbar;

					_toolbar.InsertItem(NavigationGroupIdentifier, 0);
					_toolbar.InsertItem(NSToolbar.NSToolbarFlexibleSpaceItemIdentifier, 1);
					_toolbar.InsertItem(HasTabs ? TabbedGroupIdentifier : TitleGroupIdentifier, 2);
					_toolbar.InsertItem(NSToolbar.NSToolbarFlexibleSpaceItemIdentifier, 3);
					_toolbar.InsertItem(ToolbarItemsGroupIdentifier, 4);
				}

				_toolbar.Visible = true;
				UpdateToolbarItems();
				UpdateTitle();
				UpdateNavigationItems();
				if (HasTabs)
					UpdateTabbedItems();
				UpdateBarBackgroundColor();
			}
			else
			{
				if (_toolbar != null)
				{
					_toolbar.Visible = false;
				}
			}
		}

		void UpdateBarBackgroundColor()
		{
			// I'm sorry. I'm so so sorry.
			// When the user has Graphite appearance set in System Preferences on El Capitan
			// and they enter fullscreen mode, Cocoa doesn't respect the VibrantDark appearance
			// making the toolbar background white instead of black, however the toolbar items do still respect
			// the dark appearance, making them white on white.
			//
			// So, an absolute hack is to go through the toolbar hierarchy and make all the views background colours
			// be the dark grey we wanted them to be in the first place.
			//
			// https://bugzilla.xamarin.com/show_bug.cgi?id=40160
			//
			//if (_toolbar.Window == null)
			//{
			//	if (_toolbarSuperview != null)
			//	{
			//		Superview.WantsLayer = false;

			//		if (Superview.Superview != null)
			//		{
			//			Superview.Superview.WantsLayer = false;
			//		}
			//	}
			//	return;
			//}
			//var bgColor = _getBackgroundColor().CGColor;

			////// NSToolbarItemViewer
			//if (Superview != null)
			//{
			//	Superview.WantsLayer = true;
			//	Superview.Layer.BackgroundColor = bgColor;

			//	if (Superview.Superview != null)
			//	{
			//		// _NSToolbarViewClipView
			//		Superview.Superview.WantsLayer = true;
			//		Superview.Superview.Layer.BackgroundColor = bgColor;

			//		if (Superview.Superview.Superview != null && Superview.Superview.Superview.Superview != null)
			//		{
			//			// NSTitlebarView
			//			Superview.Superview.Superview.Superview.WantsLayer = true;
			//			Superview.Superview.Superview.Superview.Layer.BackgroundColor = bgColor;
			//		}
			//	}
			//}
		}

		void NavigationPagePropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName.Equals(NavigationPage.BarTextColorProperty.PropertyName) || e.PropertyName.Equals(NavigationPage.BarBackgroundColorProperty.PropertyName))
				UpdateToolBar();
		}

		void ToolbarTrackerOnCollectionChanged(object sender, EventArgs eventArgs)
		{
			UpdateToolbarItems();
		}

		async Task NavigateBackFrombackButton()
		{
			await NavigationController?.PopAsyncInner(true, true);
		}

		bool ShowBackButton()
		{
			if (_navigation == null)
				return false;

			return NavigationPage.GetHasBackButton(_navigation.CurrentPage) && !IsRootPage();
		}

		bool IsRootPage()
		{
			if (NavigationController == null)
				return true;
			return NavigationController.StackDepth <= 1;
		}

		NSColor GetBackgroundColor()
		{
			var backgroundNSColor = NSColor.Clear;
			if (Navigation != null && Navigation.BarBackgroundColor != Color.Default)
				backgroundNSColor = Navigation.BarBackgroundColor.ToNSColor();
			return backgroundNSColor;
		}

		NSColor GetTitleColor()
		{
			var titleNSColor = NSColor.Black;
			if (Navigation != null && Navigation?.BarTextColor != Color.Default)
				titleNSColor = Navigation.BarTextColor.ToNSColor();

			return titleNSColor;
		}

		string GetCurrentPageTitle()
		{
			if (NavigationController == null)
				return string.Empty;
			return NavigationController.StackCopy.Peek().Title ?? "";
		}

		string GetPreviousPageTitle()
		{
			if (NavigationController == null || NavigationController.StackDepth <= 1)
				return string.Empty;

			return NavigationController.StackCopy.ElementAt(NavigationController.StackDepth - 1).Title ?? _defaultBackButtonTitle;
		}

		List<ToolbarItem> GetToolbarItems()
		{
			return _toolbarTracker.ToolbarItems.ToList();
		}

		void UpdateTitle()
		{
			if (_toolbar == null || _navigation == null || _titleGroup == null)
				return;

			var title = GetCurrentPageTitle();
			var item = new NSToolbarItem(title);

			var titleField = new NSTextField
			{
				AllowsEditingTextAttributes = true,
				Bordered = false,
				DrawsBackground = false,
				Bezeled = false,
				Editable = false,
				Selectable = false,
				Cell = new VerticallyCenteredTextFieldCell(0f, NSFont.TitleBarFontOfSize(18)),
				StringValue = title
			};
			titleField.Cell.TextColor = GetTitleColor();
			titleField.SizeToFit();
			titleField.Layout();
			titleField.SetNeedsDisplay();
			_titleGroup.Group.Subitems = new NSToolbarItem[] { item };
			_titleGroup.Group.View = titleField;
		}

		void UpdateToolbarItems()
		{
			if (_toolbar == null || _navigation == null || _toolbarGroup == null)
				return;

			var currentPage = NavigationController.StackCopy.Peek();
			UpdateGroup(_toolbarGroup, currentPage.ToolbarItems, ToolbarItemWidth, ToolbarItemSpacing);
		}

		void UpdateNavigationItems()
		{
			if (_toolbar == null || _navigation == null || _navigationGroup == null)
				return;
			var items = new List<ToolbarItem>();
			if (ShowBackButton())
			{
				var backButtonItem = new ToolbarItem
				{
					Text = GetPreviousPageTitle(),
					Command = new Command(async () => await NavigateBackFrombackButton())
				};
				items.Add(backButtonItem);
			}

			UpdateGroup(_navigationGroup, items, BackButtonItemWidth, -1);

			var navItemBack = _navigationGroup.Items.FirstOrDefault();
			if (navItemBack != null)
			{
				navItemBack.Button.Image = NSImage.ImageNamed(NSImageName.GoLeftTemplate);
				navItemBack.Button.SizeToFit();
				navItemBack.Button.AccessibilityTitle = "NSBackButton";
			}
		}

		void UpdateTabbedItems()
		{
			if (_toolbar == null || _navigation == null || _tabbedGroup == null)
				return;

			UpdateGroup(_tabbedGroup, _navigation.ToolbarItems, ToolbarItemWidth, ToolbarItemSpacing);
		}

		static void UpdateGroup(NativeToolbarGroup group, IList<ToolbarItem> toolbarItems, double itemWidth, double itemSpacing)
		{
			int count = toolbarItems.Count;
			group.Items.Clear();
			if (count > 0)
			{
				var subItems = new NSToolbarItem[count];
				var view = new NSView();
				nfloat totalWidth = 0;
				var currentX = 0.0;
				for (int i = 0; i < toolbarItems.Count; i++)
				{
					var element = toolbarItems[i];

					var item = new NSToolbarItem(element.Text);
					item.Activated += (sender, e) => (element as IMenuItemController)?.Activate();

					var button = new NSButton();
					button.Title = element.Text;
					button.SizeToFit();
					var buttonWidth = itemWidth;
					if (button.FittingSize.Width > itemWidth)
					{
						buttonWidth = button.FittingSize.Width + 10;
					}
					button.Frame = new CGRect(currentX + i * itemSpacing, 0, buttonWidth, ToolbarItemHeight);
					currentX += buttonWidth;
					totalWidth += button.Frame.Width;
					button.Activated += (sender, e) => (element as IMenuItemController)?.Activate();

					button.BezelStyle = NSBezelStyle.TexturedRounded;
					if (!string.IsNullOrEmpty(element.Icon))
						button.Image = new NSImage(element.Icon);

					button.SizeToFit();
					view.AddSubview(button);

					item.Label = item.PaletteLabel = item.ToolTip = button.ToolTip = element.Text;

					subItems[i] = item;

					group.Items.Add(new NativeToolbarGroup.Item { ToolbarItem = item, Button = button });
				}
				view.Frame = new CGRect(0, 0, totalWidth + (itemSpacing * (count - 1)), ToolbarItemHeight);

				group.Group.Subitems = subItems;
				group.Group.View = view;
			}
			else {
				group.Group.Subitems = new NSToolbarItem[] { };
				group.Group.View = new NSView();
			}
		}
	}
}