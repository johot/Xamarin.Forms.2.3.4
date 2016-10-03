using System;
using Foundation;

namespace Xamarin.Forms.Platform.MacOS
{
	internal class NSPageContainer : NSObject
	{
		public NSPageContainer(Page element, int index)
		{
			Page = element;
			Index = index;
			var render = Platform.GetRenderer(element);
			if (render == null)
				render = Platform.CreateRenderer(element);

		}

		public Page Page { get; }

		public int Index
		{
			get;
			set;
		}
	}

}
