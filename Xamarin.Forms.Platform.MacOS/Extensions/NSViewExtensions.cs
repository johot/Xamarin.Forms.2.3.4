using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using AppKit;
using static System.String;

namespace Xamarin.Forms.Platform.MacOS
{
	public static class NSViewExtensions
	{

		public static IEnumerable<NSView> Descendants(this NSView self)
		{
			if (self.Subviews == null)
				return Enumerable.Empty<NSView>();
			return self.Subviews.Concat(self.Subviews.SelectMany(s => s.Descendants()));
		}

		public static SizeRequest GetSizeRequest(this NSView self, double widthConstraint, double heightConstraint, double minimumWidth = -1, double minimumHeight = -1)
		{
			CoreGraphics.CGSize s;
			var control = self as NSControl;
			if (control != null)
				s = control.SizeThatFits(new CoreGraphics.CGSize(widthConstraint, heightConstraint));
			else
				s = self.FittingSize;
			var request = new Size(s.Width == float.PositiveInfinity ? double.PositiveInfinity : s.Width, s.Height == float.PositiveInfinity ? double.PositiveInfinity : s.Height);
			var minimum = new Size(minimumWidth < 0 ? request.Width : minimumWidth, minimumHeight < 0 ? request.Height : minimumHeight);
			return new SizeRequest(request, minimum);
		}

		internal static T FindDescendantView<T>(this NSView view) where T : NSView
		{
			var queue = new Queue<NSView>();
			queue.Enqueue(view);

			while (queue.Count > 0)
			{
				var descendantView = queue.Dequeue();

				var result = descendantView as T;
				if (result != null)
					return result;

				for (var i = 0; i < descendantView.Subviews.Length; i++)
					queue.Enqueue(descendantView.Subviews[i]);
			}

			return null;
		}

		public static void SetBinding(this NSView view, string propertyName, BindingBase bindingBase, string updateSourceEventName = null)
		{
			var binding = bindingBase as Binding;
			//This will allow setting bindings from Xaml by reusing the MarkupExtension
			updateSourceEventName = updateSourceEventName ?? binding?.UpdateSourceEventName;

			if (!IsNullOrEmpty(updateSourceEventName))
			{
				NativeBindingHelpers.SetBinding(view, propertyName, bindingBase, updateSourceEventName);
				return;
			}

			NativeViewPropertyListener nativePropertyListener = null;
			if (bindingBase.Mode == BindingMode.TwoWay)
			{
				nativePropertyListener = new NativeViewPropertyListener(propertyName);
				view.AddObserver(nativePropertyListener, propertyName, 0, IntPtr.Zero);
			}

			NativeBindingHelpers.SetBinding(view, propertyName, bindingBase, nativePropertyListener);
		}

		public static void SetBinding(this NSView self, BindableProperty targetProperty, BindingBase binding)
		{
			NativeBindingHelpers.SetBinding(self, targetProperty, binding);
		}

		public static void SetValue(this NSView target, BindableProperty targetProperty, object value)
		{
			NativeBindingHelpers.SetValue(target, targetProperty, value);
		}

		public static void SetBindingContext(this NSView target, object bindingContext, Func<NSView, IEnumerable<NSView>> getChildren = null)
		{
			NativeBindingHelpers.SetBindingContext(target, bindingContext, getChildren);
		}

		internal static void TransferbindablePropertiesToWrapper(this NSView target, View wrapper)
		{
			NativeBindingHelpers.TransferBindablePropertiesToWrapper(target, wrapper);
		}

		//internal static NSView FindFirstResponder(this NSView view)
		//{
		//	if (view.IsFirstResponder)
		//		return view;

		//	foreach (var subView in view.Subviews)
		//	{
		//		var firstResponder = subView.FindFirstResponder();
		//		if (firstResponder != null)
		//			return firstResponder;
		//	}

		//	return null;
		//}
	}
}

