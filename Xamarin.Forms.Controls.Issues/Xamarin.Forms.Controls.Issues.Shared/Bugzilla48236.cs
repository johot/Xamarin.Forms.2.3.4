using Xamarin.Forms.CustomAttributes;
using Xamarin.Forms.Internals;

#if UITEST
using Xamarin.UITest;
using NUnit.Framework;
#endif

namespace Xamarin.Forms.Controls.Issues
{
	[Preserve(AllMembers = true)]
	[Issue(IssueTracker.Bugzilla, 48236, "[WinRT/UWP] BackgroundColor for Stepper behaves differently compared to iOS to Android", PlatformAffected.WinRT)]
	public class Bugzilla48236 : TestContentPage
	{
		protected override void Init()
		{
			// Refer to https://bugzilla.xamarin.com/show_bug.cgi?id=48236 for this issue. WinRT/UWP had
			// an issue where the background color would be used on the area containing the buttons, 
			// potentially causing the color to run the width of the screen. Only the buttons should have
			// a background color.
			var stepper = new Stepper
			{
				BackgroundColor = Color.Green,
				Minimum = 0,
				Maximum = 10
			};

			Content = new StackLayout
			{
				Children =
				{
					stepper,
					new Button
					{
						BackgroundColor = Color.Aqua,
						Text = "Change Stepper Color to Yellow",
						Command = new Command(() =>
						{
							stepper.BackgroundColor = Color.Yellow;
						})
					}
				}
			};
		}
	}
}