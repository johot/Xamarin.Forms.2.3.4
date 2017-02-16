using System.Collections.Generic;

#if FORMS
using NativeView = Xamarin.Forms.View;

namespace Xamarin.Forms
#else
namespace Xamarin.FlexLayout
#endif
{
    internal static class FlexLayoutExtensions
    {
        internal static Dictionary<IFlexNode, NativeView> Bridges = new Dictionary<IFlexNode, NativeView>();

        public static bool IsLeaf(this NativeView view)
        {
            if (view.IsEnabled)
            {
                foreach (NativeView subview in GetChildren(view))
                {
                    if (subview.IsEnabled && FlexLayout.GetIsIncluded(subview))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static IEnumerable<NativeView> GetChildren(NativeView view)
        {
            var result = new List<NativeView>();
            if (view is Xamarin.Forms.Layout<Xamarin.Forms.View>)
            {
                result.AddRange((view as Xamarin.Forms.Layout<Xamarin.Forms.View>).Children);
            }

            return result;
        }
    }
}
