using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using AppKit;
using Foundation;

namespace Xamarin.Forms.Platform.MacOS
{
	[Register("CarouselPageRenderer")]
	public class CarouselPageRenderer : NSPageController, IVisualElementRenderer
	{
		bool _appeared;
		bool _disposed;
		EventTracker _events;
		bool _ignoreNativeScrolling;
		VisualElementTracker _tracker;

		public CarouselPageRenderer() { View = new FormsNSView { BackgroundColor = NSColor.Clear }; }
		public CarouselPageRenderer(IntPtr handle) : base(handle) { }

		IElementController ElementController => Element as IElementController;
		IPageController PageController => (IPageController)Element;

		public override nint SelectedIndex
		{
			get
			{
				return base.SelectedIndex;
			}
			set
			{
				if (base.SelectedIndex == value)
					return;
				base.SelectedIndex = value;
				if (Carousel != null)
					Carousel.CurrentPage = (ContentPage)ElementController.LogicalChildren[(int)SelectedIndex];
			}
		}

		public VisualElement Element { get; private set; }

		public event EventHandler<VisualElementChangedEventArgs> ElementChanged;

		public SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint)
		{
			return NativeView.GetSizeRequest(widthConstraint, heightConstraint);
		}

		public NSView NativeView => View;

		public void SetElement(VisualElement element)
		{
			VisualElement oldElement = Element;
			Element = element;

			Init();

			OnElementChanged(new VisualElementChangedEventArgs(oldElement, element));
		}

		public void SetElementSize(Size size)
		{
			Element.Layout(new Rectangle(Element.X, Element.Y, size.Width, size.Height));
		}

		public NSViewController ViewController
		{
			get { return this; }
		}

		public override void ViewDidAppear()
		{
			base.ViewDidAppear();
			if (_appeared || _disposed)
				return;

			_appeared = true;
			PageController.SendAppearing();
		}

		public override void ViewDidDisappear()
		{
			base.ViewDidDisappear();

			if (!_appeared || _disposed)
				return;

			_appeared = false;
			PageController.SendDisappearing();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && !_disposed)
			{
				if (Carousel != null)
				{
					Carousel.PropertyChanged -= OnPropertyChanged;
					Carousel.PagesChanged -= OnPagesChanged;
				}

				Platform.SetRenderer(Element, null);

				if (_appeared)
				{
					_appeared = false;
					PageController?.SendDisappearing();
				}

				if (_events != null)
				{
					_events.Dispose();
					_events = null;
				}

				if (_tracker != null)
				{
					_tracker.Dispose();
					_tracker = null;
				}

				Element = null;
				_disposed = true;
			}

			base.Dispose(disposing);
		}

		protected virtual void OnElementChanged(VisualElementChangedEventArgs e)
		{
			EventHandler<VisualElementChangedEventArgs> changed = ElementChanged;
			if (changed != null)
				changed(this, e);
		}

		protected virtual void ConfigureNSPageController()
		{
			TransitionStyle = NSPageControllerTransitionStyle.HorizontalStrip;
		}

		protected CarouselPage Carousel => Element as CarouselPage;

		void Init()
		{
			Delegate = new PageControllerDelegate(this);

			_tracker = new VisualElementTracker(this);
			_events = new EventTracker(this);
			_events.LoadEvents(View);

			ConfigureNSPageController();

			UpdateBackground();
			UpdateSource();
			UpdateCurrentPage(false);

			Carousel.PropertyChanged += OnPropertyChanged;
			Carousel.PagesChanged += OnPagesChanged;
		}

		void UpdateSource()
		{
			var pages = new List<NSPageContainer>();
			for (var i = 0; i < ElementController.LogicalChildren.Count; i++)
			{
				Element element = ElementController.LogicalChildren[i];
				var child = element as ContentPage;
				if (child != null)
					pages.Add(new NSPageContainer(child, i));
			}

			ArrangedObjects = pages.ToArray();
		}

		void OnPagesChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			_ignoreNativeScrolling = true;

			UpdateSource();

			_ignoreNativeScrolling = false;
		}

		void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == nameof(TabbedPage.CurrentPage))
				UpdateCurrentPage();
			else if (e.PropertyName == VisualElement.BackgroundColorProperty.PropertyName)
				UpdateBackground();
			else if (e.PropertyName == Page.BackgroundImageProperty.PropertyName)
				UpdateBackground();
		}

		void UpdateBackground()
		{
			string bgImage = ((Page)Element).BackgroundImage;
			var formsBackgroundView = View as FormsNSView;
			if (!string.IsNullOrEmpty(bgImage))
			{
				formsBackgroundView.BackgroundColor = NSColor.FromPatternImage(NSImage.ImageNamed(bgImage));
				return;
			}
			Color bgColor = Element.BackgroundColor;
			if (bgColor.IsDefault)
				formsBackgroundView.BackgroundColor = NSColor.White;
			else
				formsBackgroundView.BackgroundColor = bgColor.ToNSColor();
		}

		void UpdateCurrentPage(bool animated = true)
		{
			ContentPage current = Carousel.CurrentPage;
			if (current != null)
			{
				int index = Carousel.CurrentPage != null ? CarouselPage.GetIndex(Carousel.CurrentPage) : 0;
				if (index < 0)
					index = 0;

				if (SelectedIndex == index)
					return;

				if (animated)
				{
					NSAnimationContext.RunAnimation((NSAnimationContext context) =>
					{
						((NSPageController)Animator).SelectedIndex = index;
					}, CompleteTransition);
				}
				else
				{
					SelectedIndex = index;
				}
			}
		}
	}
}
