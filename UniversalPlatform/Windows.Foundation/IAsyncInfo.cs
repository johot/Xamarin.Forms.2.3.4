using System;
namespace Windows.Foundation
{
	public interface IAsyncInfo
	{
		void Cancel();
		void Close();
		Exception ErrorCode { get; }
		uint Id { get; }
		AsyncStatus Status { get; }
	}
}
