using System;
using System.Threading.Tasks;

namespace Xamarin.Forms
{
	public interface IApplicationController
	{
		void SetCurrentApplication(Application app);
		void SetAppIndexingProvider(IAppIndexingProvider appIndexing);
		void SendOnAppLinkRequestReceived(Uri uri);
		void SendResume();
		Task SendSleepAsync();
		void SendStart();
		NavigationProxy NavigationProxy { get; }
	}
}