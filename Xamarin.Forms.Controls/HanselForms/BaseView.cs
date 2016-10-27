namespace Xamarin.Forms.Controls
{
	public class BaseView : ContentPage
	{
		public BaseView()
		{
			SetBinding(Page.TitleProperty, new Binding(HBaseViewModel.TitlePropertyName));
			SetBinding(Page.IconProperty, new Binding(HBaseViewModel.IconPropertyName));
		}
	}

}