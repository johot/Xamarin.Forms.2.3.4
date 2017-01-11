using AppKit;

namespace Xamarin.Forms.Platform.MacOS
{
	public class FormsNSView : NSView
	{
		public override void DrawRect(CoreGraphics.CGRect dirtyRect)
		{
			if (!Equals(BackgroundColor, NSColor.Clear) && BackgroundColor != null)
			{
				BackgroundColor.Set();
				NSGraphics.RectFill(Bounds);
			}

			base.DrawRect(dirtyRect);
		}

		public NSColor BackgroundColor
		{
			get;
			set;
		}
	}
}

