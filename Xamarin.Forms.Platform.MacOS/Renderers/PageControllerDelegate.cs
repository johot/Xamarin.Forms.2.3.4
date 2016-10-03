using System;
using AppKit;
using Foundation;

namespace Xamarin.Forms.Platform.MacOS
{
	internal class PageControllerDelegate : NSPageControllerDelegate
	{
		readonly CarouselPageRenderer _carouselPageRenderer;

		public PageControllerDelegate(CarouselPageRenderer pageController)
		{
			_carouselPageRenderer = pageController;
		}

		public override string GetIdentifier(NSPageController pv, NSObject obj)
		{
			return nameof(PageRenderer);
		}

		public override NSViewController GetViewController(NSPageController pageController, string identifier)
		{
			return new PageRenderer();
		}

		public override void PrepareViewController(NSPageController pageController, NSViewController viewController, NSObject targetObject)
		{
			var pContainer = targetObject as NSPageContainer;
			if (pContainer != null)
			{
				var page = pContainer.Page;
				var pageRenderer = (viewController as PageRenderer);
				pageRenderer.SetElement(page);
				Platform.SetRenderer(page, pageRenderer);
			}
		}

	}
}
