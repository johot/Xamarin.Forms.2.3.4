using System;
namespace Windows.Foundation
{
	public delegate void AsyncActionCompletedHandler(IAsyncAction asyncInfo, AsyncStatus asyncStatus);

	public interface IAsyncAction : IAsyncInfo
	{
		void GetResults();
		AsyncActionCompletedHandler Completed { get; set; }
	}
}
