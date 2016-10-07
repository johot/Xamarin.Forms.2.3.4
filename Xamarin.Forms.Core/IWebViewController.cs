namespace Xamarin.Forms
{
	public interface IWebViewController
	{
		void SendNavigated(WebNavigatedEventArgs args);

		void SendNavigating(WebNavigatingEventArgs args);

		void UpdateCanGoBack(bool canGoBack);

		void UpdateCanGoForward(bool canGoForward);
	}
}
