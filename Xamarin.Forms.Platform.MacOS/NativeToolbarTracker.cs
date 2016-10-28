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
	internal class NativeToolbarTracker
	{
		const string ToolBarId = "AwesomeBarToolbar";
		const string AwesomeBarId = "AwesomeBarToolbarItem";

		INavigationPageController NavigationController => _navigation as INavigationPageController;

		ToolbarTracker _toolbarTracker;
		NSToolbar _toolbar;
		AwesomeBar _awesomeBar;
		NavigationPage _navigation;
		MainToolBarDelegate ToolbarDelegate => _toolbar.Delegate as MainToolBarDelegate;

		public NativeToolbarTracker()
		{
			_toolbarTracker = new ToolbarTracker();
			_toolbarTracker.CollectionChanged += ToolbarTrackerOnCollectionChanged;
			_awesomeBar = new AwesomeBar(GetBackgroundColor, GetPreviousPageTitle, GetCurrentPageTitle, GetTitleColor, GetToolbarItems);
			_awesomeBar.BackButtonPressed += async (sender, e) => await NavigateBackFrombackButton();
		}

		public static bool IsFullscreen { get; private set; }

		public NavigationPage Navigation
		{
			get { return _navigation; }
			set
			{
				if (_navigation == value)
					return;

				_navigation = value;
				UpdateToolBar();
			}
		}

		public void TryHide(NavigationPage navPage)
		{
			if (navPage == _navigation)
			{
				Navigation = null;
			}
		}

		protected virtual NSToolbar ConfigureToolbar()
		{
			var toolbar = new NSToolbar(ToolBarId)
			{
				DisplayMode = NSToolbarDisplayMode.Icon
			};

			toolbar.WillInsertItem = (tool, id, send) =>
			{
				switch (id)
				{
					case AwesomeBarId:
						return new NSToolbarItem(AwesomeBarId)
						{
							View = _awesomeBar,
							MinSize = new CGSize(1024, AwesomeBar.ToolbarHeight),
							MaxSize = new CGSize(float.PositiveInfinity, AwesomeBar.ToolbarHeight)
						};

					default:
						throw new NotImplementedException();
				}
			};

			Action<NSNotification> resizeAction = notif =>
			{
				var win = _awesomeBar.Window;
				if (win == null)
				{
					return;
				}

				var item = _toolbar.Items[0];

				var abFrameInWindow = _awesomeBar.ConvertRectToView(_awesomeBar.Frame, null);
				var awesomebarHeight = AwesomeBar.ToolbarHeight;
				var size = new CGSize(win.Frame.Width - abFrameInWindow.X - 4, awesomebarHeight);

				if (item.MinSize != size)
				{
					item.MinSize = size;
				}
			};

			// We can't use the events that Xamarin.Mac adds for delegate methods as they will overwrite
			// the delegate that Gtk has added
			NSWindow nswin = NSApplication.SharedApplication.MainWindow;
			NSNotificationCenter.DefaultCenter.AddObserver(NSWindow.DidResizeNotification, resizeAction, nswin);
			NSNotificationCenter.DefaultCenter.AddObserver(NSWindow.DidEndLiveResizeNotification, resizeAction, nswin);

			NSNotificationCenter.DefaultCenter.AddObserver(NSWindow.WillEnterFullScreenNotification, (note) => IsFullscreen = true, nswin);
			NSNotificationCenter.DefaultCenter.AddObserver(NSWindow.WillExitFullScreenNotification, (note) => IsFullscreen = false, nswin);

			return toolbar;
		}

		void ToolbarTrackerOnCollectionChanged(object sender, EventArgs eventArgs)
		{
			UpdateToolbarItems();
		}

		async Task NavigateBackFrombackButton()
		{
			await NavigationController?.PopAsyncInner(true, true);
		}

		NSColor GetBackgroundColor()
		{
			return Navigation?.BarBackgroundColor.ToNSColor() ?? NSColor.Clear;
		}

		NSColor GetTitleColor()
		{
			return Navigation?.BarTextColor.ToNSColor() ?? NSColor.Black;
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

			return NavigationController.StackCopy.ElementAt(NavigationController.StackDepth - 1).Title;
		}

		List<ToolbarItem> GetToolbarItems()
		{
			return _toolbarTracker.ToolbarItems.ToList();
		}

		void UpdateToolBar()
		{
			if (NSApplication.SharedApplication.MainWindow == null)
				return;

			if (NavigationController == null)
			{
				if (_toolbar != null)
					_toolbar.Visible = false;
				return;
			}

			var currentPage = NavigationController.StackCopy.Peek();

			if (NavigationPage.GetHasNavigationBar(currentPage))
			{
				if (_toolbar == null)
				{
					_toolbar = ConfigureToolbar();
					NSApplication.SharedApplication.MainWindow.Toolbar = _toolbar;
				}
				_toolbar.Visible = true;

				UpdateToolbarItems();
			}
			else
			{
				if (_toolbar == null)
				{
					_toolbar.Visible = false;
				}
			}
		}

		internal void UpdateToolbarItems()
		{
			if (_toolbar == null || _navigation == null)
				return;

			var nItems = _toolbar.Items.Length;
			if (nItems == 0)
			{
				_toolbar.InsertItem(AwesomeBarId, 0);

			}
			else
			{
				_awesomeBar?.UpdateItems();
			}
		}
	}
}