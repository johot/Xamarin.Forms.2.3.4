using System;
using AppKit;
using Foundation;

namespace Xamarin.Forms.Platform.MacOS
{
	internal class PageControllerDelegate : NSPageControllerDelegate
	{

		public override string GetIdentifier(NSPageController pageController, NSObject targetObject)
		{
			return nameof(PageRenderer);
		}

		public override NSViewController GetViewController(NSPageController pageController, string identifier)
		{
			return new PageRenderer();
		}

		public override void PrepareViewController(NSPageController pageController, NSViewController viewController, NSObject targetObject)
		{
			var pageContainer = targetObject as NSPageContainer;
			if (pageContainer != null)
			{
				var page = pageContainer.Page;
				var pageRenderer = (viewController as PageRenderer);
				pageRenderer.SetElement(page);
				Platform.SetRenderer(page, pageRenderer);
			}
		}

	}
}
