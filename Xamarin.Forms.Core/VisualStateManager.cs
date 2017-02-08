using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.Forms
{
	public class VisualStateManager
	{
		public static readonly BindableProperty VisualStateGroupsProperty =
			BindableProperty.CreateAttached("VisualStateGroups", typeof(IList<VisualStateGroup>), typeof(VisualElement), new List<VisualStateGroup>());

		public static IList<VisualStateGroup> GetVisualStateGroups(VisualElement visualElement)
		{
			return (IList<VisualStateGroup>)visualElement.GetValue(VisualStateGroupsProperty);
		}

		public static void SetVisualStateGroups(VisualElement visualElement, IList<VisualStateGroup> value)
		{
			visualElement.SetValue(VisualStateGroupsProperty, value);
		}

		public static bool GoToState(VisualElement visualElement, string name)
		{
			return false;
		}
	}

	[ContentProperty("VisualStates")]
	public class VisualStateGroup
	{
		public VisualStateGroup()
		{
			VisualStates = new List<VisualState>();
		}

		public IList<VisualState> VisualStates { get; }
	}

	public class VisualState
	{
		public ICollection<Setter> Setters { get; set; }
		public Type TargetType { get; }
	}

}
