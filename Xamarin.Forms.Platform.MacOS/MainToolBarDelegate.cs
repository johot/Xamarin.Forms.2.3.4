using System;
using System.Collections.Generic;
using System.Linq;
using AppKit;
using Xamarin.Forms.Internals;

namespace Xamarin.Forms.Platform.MacOS
{
	internal class MainToolBarDelegate : NSToolbarDelegate
	{
		const int _buttonsMaxWidth = 120;
		const int _buttonsMaxHeight = 50;
		Func<string> _getPageTitle;
		Func<string> _getPreviousPageTitle;
		Func<List<ToolbarItem>> _getToolbarItems;
		Action _goBackAction;
		NSToolbar _toolbar;

		bool _disposed;

		public MainToolBarDelegate(Func<string> getPageTitle, Func<string> getPreviousPageTitle, Func<List<ToolbarItem>> getToolbarItems, Action goBackAction)
		{
			_getPageTitle = getPageTitle;
			_getPreviousPageTitle = getPreviousPageTitle;
			_getToolbarItems = getToolbarItems;
			_goBackAction = goBackAction;
		}

		public static string TitleIdentifier = "Title";
		public static string BackButtonIdentifier = "BackButton";
		public static string ToolbarItemsIdentifier = "ToolbarItems";

		public override string[] SelectableItemIdentifiers(NSToolbar toolbar)
		{
			return new string[] { };
		}

		public override string[] AllowedItemIdentifiers(NSToolbar toolbar)
		{
			return new string[] { NSToolbar.NSToolbarCustomizeToolbarItemIdentifier, NSToolbar.NSToolbarSpaceItemIdentifier };
		}

		public override string[] DefaultItemIdentifiers(NSToolbar toolbar)
		{
			return new string[] { };
		}

		//TODO: Make sure we remove any handlers from buttons
		public override NSToolbarItem WillInsertItem(NSToolbar toolbar, string itemIdentifier, bool willBeInserted)
		{
			_toolbar = toolbar;
			var tb = new NSToolbarItem(itemIdentifier);
			if (itemIdentifier == TitleIdentifier)
			{
				tb.View = NSTextField.CreateLabel(_getPageTitle());
			}
			if (itemIdentifier == BackButtonIdentifier)
			{
				var title = _getPreviousPageTitle();
				tb.MinSize = tb.MaxSize = new CoreGraphics.CGSize(_buttonsMaxWidth, _buttonsMaxHeight);

				if (!string.IsNullOrEmpty(title))
				{
					var btn = NSButton.CreateButton(title, NSImage.ImageNamed(NSImageName.GoLeftTemplate), _goBackAction);
					btn.BezelStyle = NSBezelStyle.TexturedRounded;
					if (btn.FittingSize.Width > _buttonsMaxWidth)
					{
						//TODO: This should be translated .. how? 
						btn.Title = "Back";
					}
					tb.View = btn;
				}
				else
				{
					tb.View = new NSView(new CoreGraphics.CGRect(0, 0, _buttonsMaxWidth, _buttonsMaxHeight));
				}
			}

			if (itemIdentifier == ToolbarItemsIdentifier)
			{
				var tbItems = _getToolbarItems();
				var tbItemsCount = tbItems.Count;

				tb.MinSize = tb.MaxSize = new CoreGraphics.CGSize(_buttonsMaxWidth, _buttonsMaxHeight);

				if (tbItemsCount > 0)
				{
					var seg = new NSSegmentedControl { SegmentStyle = NSSegmentStyle.TexturedRounded };
					seg.SegmentCount = tbItemsCount;

					for (int i = 0; i < tbItemsCount; i++)
					{
						var tbi = tbItems[i];
						seg.SetLabel(tbi.Text, i);
						if (!string.IsNullOrEmpty(tbi.Icon))
							seg.SetImage(new NSImage(tbi.Icon), i);

						seg.SetEnabled(tbi.IsEnabled, i);
					}
					seg.Activated += (sender, e) => (tbItems.ElementAt((int)seg.SelectedSegment) as IMenuItemController)?.Activate();
					tb.View = seg;
				}
				else
				{
					tb.View = new NSView(new CoreGraphics.CGRect(0, 0, _buttonsMaxWidth, _buttonsMaxHeight));
				}
			}

			return tb;
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if (disposing && !_disposed)
			{
				_disposed = true;
			}
		}
	}
}
