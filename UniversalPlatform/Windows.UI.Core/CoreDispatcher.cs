using System;
using Windows.Foundation;

namespace Windows.UI.Core
{
	public sealed class CoreDispatcher
	{
		private CoreDispatcher()
		{
		}

		public IAsyncAction RunAsync(CoreDispatcherPriority priority, DispatchedHandler agileCallback)
		{
			throw new NotImplementedException();
		}

		public IAsyncAction RunIdleAsync(IdleDispatchedHandler agileCallback)
		{
			throw new NotImplementedException();
		}

		public CoreDispatcherPriority CurrentPriority { get; set; }
		public bool HasThreadAccess { get; } = false;
	}
}