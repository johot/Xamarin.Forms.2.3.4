using System;
using System.Collections.Generic;
using System.Linq;
using AppKit;
using CoreGraphics;
using Foundation;

namespace Xamarin.Forms.Platform.MacOS
{
	//Ported from Xamarin Studio
	public class FormsAwesomeBar : NSView
	{
		const float _buttonsMaxWidth = 120.0f;
		const float _buttonsMaxHeight = 50.0f;
		const float toolbarPadding = 8.0f;
		const float maxSearchBarWidth = 270.0f;
		const float minSearchBarWidth = 150.0f;
		const float maxStatusBarWidth = 700.0f;
		const float minStatusBarWidth = 220.0f;
		const float runButtonWidth = 60.0f;
		internal static float ToolbarHeight => 24.0f;

		NSButton _backButton;
		NSTextField _titleField;
		NSObject _superviewFrameChangeObserver;
		NSView _toolbarItemsContainer;

		Func<NSColor> _getBackgroundColor;
		Func<NSColor> _getTitleColor;
		Func<string> _getBackText;
		Func<string> _getTitle;
		Func<List<ToolbarItem>> _getToolbarItems;

		public event EventHandler BackButtonPressed;

		public FormsAwesomeBar(Func<NSColor> getBackgroundColor, Func<string> getBackText, Func<string> getTitle, Func<NSColor> getTitleColor, Func<List<ToolbarItem>> getToolbarItems)
		{
			_getBackgroundColor = getBackgroundColor;
			_getBackText = getBackText;
			_getTitle = getTitle;
			_getToolbarItems = getToolbarItems;
			_getTitleColor = getTitleColor;

			_backButton = NSButtonExtensions.CreateButton(_getBackText(), NSImage.ImageNamed(NSImageName.GoLeftTemplate), () => BackButtonPressed?.Invoke(this, new EventArgs()));
			_backButton.ImagePosition = NSCellImagePosition.ImageLeft;
			_backButton.SizeToFit();
			_backButton.Layout();

			AddSubview(_backButton);

			_titleField = new NSTextField
			{
				AllowsEditingTextAttributes = true,
				Bordered = false,
				DrawsBackground = false,
				Bezeled = false,
				Editable = false,
				Selectable = false,
				Cell = new FormsVerticallyCenteredTextFieldCell(0f, NSFont.TitleBarFontOfSize(18))
			};

			AddSubview(_titleField);

			_toolbarItemsContainer = GenerateToolbarItems();
			AddSubview(_toolbarItemsContainer);

			UpdateItems();
		}

		public void UpdateItems()
		{
			var backButtonText = _backButton.Title = _getBackText();
			if (string.IsNullOrEmpty(backButtonText))
				_backButton.Hidden = true;
			else
				_backButton.Hidden = false;

			_titleField.Cell.StringValue = _getTitle();
			_titleField.Cell.TextColor = _getTitleColor();
			_titleField.SetNeedsDisplay();

			_toolbarItemsContainer = GenerateToolbarItems();
			UpdateLayout();
		}

		public override void ViewDidMoveToWindow()
		{
			base.ViewDidMoveToWindow();

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
			if (Window == null)
			{
				if (Superview != null)
				{
					Superview.WantsLayer = false;

					if (Superview.Superview != null)
					{
						Superview.Superview.WantsLayer = false;
					}
				}
				return;
			}

			var bgColor = _getBackgroundColor().CGColor;

			//// NSToolbarItemViewer
			if (Superview != null)
			{
				Superview.WantsLayer = true;
				Superview.Layer.BackgroundColor = bgColor;

				if (Superview.Superview != null)
				{
					// _NSToolbarViewClipView
					Superview.Superview.WantsLayer = true;
					Superview.Superview.Layer.BackgroundColor = bgColor;

					if (Superview.Superview.Superview != null && Superview.Superview.Superview.Superview != null)
					{
						// NSTitlebarView
						Superview.Superview.Superview.Superview.WantsLayer = true;
						Superview.Superview.Superview.Superview.Layer.BackgroundColor = bgColor;
					}
				}
			}
		}

		public override void ViewWillMoveToSuperview(NSView newSuperview)
		{
			if (Superview != null && _superviewFrameChangeObserver != null)
			{
				NSNotificationCenter.DefaultCenter.RemoveObserver(_superviewFrameChangeObserver);
				_superviewFrameChangeObserver = null;

				Superview.PostsFrameChangedNotifications = false;
			}

			base.ViewWillMoveToSuperview(newSuperview);
		}

		public override void ViewDidMoveToSuperview()
		{
			base.ViewDidMoveToSuperview();

			if (Superview != null)
			{
				Superview.PostsFrameChangedNotifications = true;
				_superviewFrameChangeObserver = NSNotificationCenter.DefaultCenter.AddObserver(NSView.FrameChangedNotification, (note) =>
				{
					// Centre vertically in superview frame
					Frame = new CGRect(0, Superview.Frame.Y + (Superview.Frame.Height - ToolbarHeight) / 2, Superview.Frame.Width, ToolbarHeight);
				}, Superview);
			}
		}

		public override CGRect Frame
		{
			get
			{
				return base.Frame;
			}
			set
			{
				base.Frame = value;
				UpdateLayout();
			}
		}

		void UpdateLayout()
		{
			_backButton.Frame = new CGRect(toolbarPadding, 0, runButtonWidth, ToolbarHeight);

			var statusbarWidth = Math.Max(Math.Min(Math.Round(Frame.Width * 0.3), maxStatusBarWidth), minStatusBarWidth);
			var searchbarWidth = maxSearchBarWidth;
			if (statusbarWidth < searchbarWidth)
			{
				searchbarWidth = minSearchBarWidth;
			}

			//// We only need to work out the width on the left side of the window because the statusbar is centred
			//// Gap + RunButton.Width + Gap + ButtonBar.Width + Gap + Half of StatusBar.Width
			var spaceLeft = (Frame.Width / 2) - (toolbarPadding + runButtonWidth + toolbarPadding + toolbarPadding + (statusbarWidth / 2));

			var realTitleBarWidth = Math.Min(_titleField.FittingSize.Width, statusbarWidth - 2);
			_titleField.Frame = new CGRect(Math.Round((Frame.Width - realTitleBarWidth) / 2), 0, realTitleBarWidth, ToolbarHeight);

			nfloat elcapYOffset = 0;
			nfloat elcapHOffset = 0;

			nfloat scaleFactor = 1;

			if (Window != null && Window.Screen != null)
			{
				scaleFactor = Window.Screen.BackingScaleFactor;
			}
			elcapYOffset = scaleFactor == 2 ? -0.5f : -1;
			elcapHOffset = 1.0f;
			_toolbarItemsContainer.Frame = new CGRect(Frame.Width - searchbarWidth, 0 + elcapYOffset, searchbarWidth, ToolbarHeight + elcapHOffset);
		}

		NSView GenerateToolbarItems()
		{
			var tbItems = _getToolbarItems();
			var tbItemsCount = tbItems.Count;

			if (tbItemsCount <= 0)
				return new NSView(new CGRect(0, 0, _buttonsMaxWidth, _buttonsMaxHeight));

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
			return seg;

		}
	}
}

