#if __MOBILE__
using UIKit;
using Xamarin.Forms.Maps.iOS;
#else
using Xamarin.Forms.Maps.MacOS;
#endif

namespace Xamarin
{
	public static class FormsMaps
	{
		static bool s_isInitialized;
#if __MOBILE__
		static bool? s_isiOs8OrNewer;

		internal static bool IsiOs8OrNewer
		{
			get
			{
				if (!s_isiOs8OrNewer.HasValue)
					s_isiOs8OrNewer = UIDevice.CurrentDevice.CheckSystemVersion(8, 0);
				return s_isiOs8OrNewer.Value;
			}
		}
#endif
		public static void Init()
		{
			if (s_isInitialized)
				return;
			GeocoderBackend.Register();
			s_isInitialized = true;
		}
	}
}