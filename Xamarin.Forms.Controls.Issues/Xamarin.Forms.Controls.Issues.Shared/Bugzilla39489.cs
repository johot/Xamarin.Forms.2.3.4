using Xamarin.Forms.CustomAttributes;
using Xamarin.Forms.Internals;
using System.Threading;
using System;
using System.Threading.Tasks;
using Xamarin.Forms.Maps;

#if UITEST
using Xamarin.UITest;
using NUnit.Framework;
#endif

namespace Xamarin.Forms.Controls
{
	[Preserve(AllMembers = true)]
	[Issue(IssueTracker.Bugzilla, 39489, "Memory leak when using NavigationPage with Maps")]
	public class Bugzilla39489 : TestNavigationPage
	{
		protected override void Init()
		{
			PushAsync(new Bz39489Content());
		}

		#if UITEST
		[Test]
		public async void Bugzilla39458Test()
		{
			RunningApp.WaitForElement(q => q.Marked("New Page"));
			RunningApp.Tap(q => q.Marked("New Page"));
			RunningApp.WaitForElement(q => q.Marked("New Page"));
			await Task.Delay(3000);
			RunningApp.Back();
		}
		#endif
	}

	public class MyXFMap : Map
	{
		static int s_count;

		public MyXFMap()
		{
			Interlocked.Increment(ref s_count);
			System.Diagnostics.Debug.WriteLine($"++++++++ {nameof(MyXFMap)} : {s_count}");
		}

		~MyXFMap()
		{
			Interlocked.Decrement(ref s_count);
			System.Diagnostics.Debug.WriteLine($"-------- {nameof(MyXFMap)} : {s_count}");
		}
	}

	[Preserve(AllMembers = true)]
	public class Bz39489Content : ContentPage
	{
		static int s_count;

		public Bz39489Content()
		{
			Interlocked.Increment(ref s_count);
			System.Diagnostics.Debug.WriteLine($"++++++++ {nameof(Bz39489Content)} : {s_count}");

			var button = new Button { Text = "New Page" };

			var gcbutton = new Button { Text = "GC" };

			var map = new Map();

			button.Clicked += Button_Clicked;
			gcbutton.Clicked += GCbutton_Clicked;

			Content = new StackLayout { Children = { button, gcbutton, map } };
		}

		void GCbutton_Clicked(object sender, EventArgs e)
		{
			System.Diagnostics.Debug.WriteLine(">>>>>>>> Running Garbage Collection");
			GC.Collect();
			GC.WaitForPendingFinalizers();
			System.Diagnostics.Debug.WriteLine($">>>>>>>> GC.GetTotalMemory = {GC.GetTotalMemory(true):n0}");
		}

		void Button_Clicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new Bz39489Content());
		}

		~Bz39489Content()
		{
			Interlocked.Decrement(ref s_count);
			System.Diagnostics.Debug.WriteLine($"-------- {nameof(Bz39489Content)} : {s_count}");
		}
	}
}