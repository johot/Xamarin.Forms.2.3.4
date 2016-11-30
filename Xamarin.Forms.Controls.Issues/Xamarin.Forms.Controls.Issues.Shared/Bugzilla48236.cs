using Xamarin.Forms.CustomAttributes;
using Xamarin.Forms.Internals;

#if UITEST
using Xamarin.UITest;
using NUnit.Framework;
#endif

// Apply the default category of "Issues" to all of the tests in this assembly
// We use this as a catch-all for tests which haven't been individually categorized
#if UITEST
[assembly: NUnit.Framework.Category("Issues")]
#endif

namespace Xamarin.Forms.Controls.Issues
{
	[Preserve(AllMembers = true)]
	[Issue(IssueTracker.Bugzilla, 48236, "[WinRT/UWP] BackgroundColor for Stepper behaves differently compared to iOS to Android", PlatformAffected.WinRT)]
	public class Bugzilla48236 : TestContentPage
	{
		protected override void Init()
		{
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