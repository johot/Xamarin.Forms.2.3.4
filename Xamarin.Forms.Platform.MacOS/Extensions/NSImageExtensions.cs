using AppKit;

namespace Xamarin.Forms.Platform.MacOS
{
	public static class NSImageExtensions
	{
		public static NSImage ResizeTo(this NSImage self, CoreGraphics.CGSize newSize)
		{
			if (self == null)
				return self;
			self.ResizingMode = NSImageResizingMode.Stretch;
			var smallImage = new NSImage(newSize);
			smallImage.LockFocus();
			self.Size = newSize;
			NSGraphicsContext.CurrentContext.ImageInterpolation = NSImageInterpolation.High;
			self.Draw(CoreGraphics.CGPoint.Empty, new CoreGraphics.CGRect(0, 0, newSize.Width, newSize.Height), NSCompositingOperation.Copy, 1.0f);
			smallImage.UnlockFocus();
			self.Dispose();
			return smallImage;
		}
	}
}
