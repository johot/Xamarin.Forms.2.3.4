using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppKit;
using Xamarin.Forms.Internals;

namespace Xamarin.Forms.Platform.MacOS
{
	internal class NativeToolbarTracker
	{
		INavigationPageController NavigationController => _navigation as INavigationPageController;

		internal ToolbarTracker _toolbarTracker;

		NSToolbar _toolbar;

		NavigationPage _navigation;
		MainToolBarDelegate ToolbarDelegate => _toolbar.Delegate as MainToolBarDelegate;

		public NativeToolbarTracker()
		{
			_toolbarTracker = new ToolbarTracker();
			_toolbarTracker.CollectionChanged += ToolbarTrackerOnCollectionChanged;
		}

		void ToolbarTrackerOnCollectionChanged(object sender, EventArgs eventArgs)
		{
			UpdateToolbarItems();
		}

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
			var toolbar = new NSToolbar("MainToolbar")
			{
				Delegate = new MainToolBarDelegate(GetCurrentPageTitle, GetPreviousPageTitle, GetToolbarItems, async () => await NavigateBackFrombackButton())
			};
			toolbar.DisplayMode = NSToolbarDisplayMode.Icon;

			return toolbar;
		}

		async Task NavigateBackFrombackButton()
		{
			await NavigationController?.PopAsyncInner(true, true);
		}

		string GetCurrentPageTitle()
		{
			return NavigationController.StackCopy.Peek().Title ?? "";
		}

		string GetPreviousPageTitle()
		{
			if (NavigationController.StackDepth <= 1)
				return "";
			else
				return NavigationController.StackCopy.ElementAt(NavigationController.StackDepth - 1).Title;
		}

		List<ToolbarItem> GetToolbarItems()
		{
			return _toolbarTracker.ToolbarItems.ToList();
		}

		internal void UpdateToolBar()
		{
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
				_toolbar.InsertItem(MainToolBarDelegate.BackButtonIdentifier, 0);
				_toolbar.InsertItem(NSToolbar.NSToolbarFlexibleSpaceItemIdentifier, 1);
				_toolbar.InsertItem(MainToolBarDelegate.TitleIdentifier, 2);
				_toolbar.InsertItem(NSToolbar.NSToolbarFlexibleSpaceItemIdentifier, 3);
				_toolbar.InsertItem(MainToolBarDelegate.ToolbarItemsIdentifier, 4);
			}
			else
			{
				ToolbarDelegate?.UpdateItems();
			}
		}
	}
}