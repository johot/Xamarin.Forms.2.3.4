using System.Collections.Generic;
using System.Diagnostics;

namespace Xamarin.Forms
{
	public static class Watcher
	{
		static readonly Stack<WatchData> s_watches = new Stack<WatchData>();
		static int s_counter;

		static string Indent => new string('\t', s_watches.Count);

		public static void Start(string current)
		{
			var indent = new string('\t', s_watches.Count);
			string message = $"w:{s_counter++:D8} {indent}{current} >>>";
			Debug.WriteLine(message);
			Log.Warning("Watcher", message);
			var sw = new WatchData(current);
			s_watches.Push(sw);
			sw.Stopwatch.Start();
		}

		public static void Stop()
		{
			WatchData sw = s_watches.Pop();
			Stopwatch watch = sw.Stopwatch;
			watch.Stop();
			string message = $"w:{s_counter++:D8} {Indent}{sw.Label} : {watch.ElapsedMilliseconds} ({watch.Elapsed})";
			Debug.WriteLine(message);
			Log.Warning("Watcher", message);

			foreach (KeyValuePair<string, long> data in sw.InternalData)
			{
				if (watch.ElapsedMilliseconds == 0)
				{
					continue;
				}

				double percent = data.Value / (double)watch.ElapsedMilliseconds;
				string msg = $"w:{s_counter++:D8} {Indent} ---> {data.Key} : {percent:P}";
				Debug.WriteLine(msg);
				Log.Warning("Watcher", msg);
			}

			if (s_watches.Count == 0)
			{
				return;
			}

			WatchData parent = s_watches.Peek();

			if (parent.InternalData.ContainsKey(sw.Label))
			{
				parent.InternalData[sw.Label] += watch.ElapsedMilliseconds;
			}
			else
			{
				parent.InternalData.Add(sw.Label, watch.ElapsedMilliseconds);
			}
		}

		class WatchData
		{
			public WatchData(string label)
			{
				Stopwatch = new Stopwatch();
				InternalData = new Dictionary<string, long>();
				Label = label;
			}

			public Dictionary<string, long> InternalData { get; }

			public string Label { get; }

			public Stopwatch Stopwatch { get; }
		}
	}
}