using System;
namespace Windows.Foundation
{
	public enum AsyncStatus
	{
		Canceled = 2,
		canceled = Canceled,
		Completed = 1,
		completed = Completed,
		Error = 3,
		error = Error,
		Started = 0,
		started = Started,
	}
}
