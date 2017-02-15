#if FORMS
namespace Xamarin.Forms.Flex
#else
namespace Xamarin.FlexLayout
#endif
{
	public delegate object MeasureFunc(IFlexNode node, float width, FlexMeasureMode widthMode, float height, FlexMeasureMode heightMode);
}
