using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Xamarin.Forms.Platform.WinRT
{
	internal class FormsVisualStateManager : Windows.UI.Xaml.VisualStateManager
	{
		protected override bool GoToStateCore(Control control, FrameworkElement templateRoot, string stateName, Windows.UI.Xaml.VisualStateGroup @group,
			Windows.UI.Xaml.VisualState state, bool useTransitions)
		{
			// We'll use this custom VSM for the Forms versions of the controls; it can hijack the
			// GoToState calls and, if the control is in "use native VSM" mode, it'll just pass them through
			// If it's in "use Forms VSM" mode, it'll redirect the state to "Forms" + stateName
			return base.GoToStateCore(control, templateRoot, stateName, @group, state, useTransitions);
		}
	}
}