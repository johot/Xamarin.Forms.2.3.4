using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreLocation;

#if __MOBILE__
using AddressBookUI;
#endif

#if __MOBILE__
namespace Xamarin.Forms.Maps.iOS
#else
namespace Xamarin.Forms.Maps.MacOS
#endif
{
	internal class GeocoderBackend
	{
		public static void Register()
		{
			Geocoder.GetPositionsForAddressAsyncFunc = GetPositionsForAddressAsync;
			Geocoder.GetAddressesForPositionFuncAsync = GetAddressesForPositionAsync;
		}

		static Task<IEnumerable<string>> GetAddressesForPositionAsync(Position position)
		{
			var location = new CLLocation(position.Latitude, position.Longitude);
			var geocoder = new CLGeocoder();
			var source = new TaskCompletionSource<IEnumerable<string>>();
			geocoder.ReverseGeocodeLocation(location, (placemarks, error) =>
			{
				if (placemarks == null)
					placemarks = new CLPlacemark[0];
#if __MOBILE__
				IEnumerable<string> addresses = placemarks.Select(p => ABAddressFormatting.ToString(p.AddressDictionary, false));
				source.SetResult(addresses);
#else
				source.SetResult(null);
#endif

			});
			return source.Task;
		}

		static Task<IEnumerable<Position>> GetPositionsForAddressAsync(string address)
		{
			var geocoder = new CLGeocoder();
			var source = new TaskCompletionSource<IEnumerable<Position>>();
			geocoder.GeocodeAddress(address, (placemarks, error) =>
			{
				if (placemarks == null)
					placemarks = new CLPlacemark[0];
				IEnumerable<Position> positions = placemarks.Select(p => new Position(p.Location.Coordinate.Latitude, p.Location.Coordinate.Longitude));
				source.SetResult(positions);
			});
			return source.Task;
		}
	}
}