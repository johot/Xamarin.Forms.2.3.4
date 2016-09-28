using System;
using AppKit;
using Xamarin.Forms;

namespace Xamarin.Forms.Platform.MacOS
{
	public class NativeViewWrapper : View
	{
		public NativeViewWrapper(NSView nativeView, GetDesiredSizeDelegate getDesiredSizeDelegate = null, SizeThatFitsDelegate sizeThatFitsDelegate = null, LayoutSubviewsDelegate layoutSubviews = null)
		{
			GetDesiredSizeDelegate = getDesiredSizeDelegate;
			SizeThatFitsDelegate = sizeThatFitsDelegate;
			LayoutSubviews = layoutSubviews;
			NativeView = nativeView;

			nativeView.TransferbindablePropertiesToWrapper(this);
		}

		public GetDesiredSizeDelegate GetDesiredSizeDelegate { get; }

		public LayoutSubviewsDelegate LayoutSubviews { get; set; }

		public NSView NativeView { get; }

		public SizeThatFitsDelegate SizeThatFitsDelegate { get; set; }

		protected override void OnBindingContextChanged()
		{
			NativeView.SetBindingContext(BindingContext, nv => nv.Subviews);
			base.OnBindingContextChanged();
		}
	}
}

