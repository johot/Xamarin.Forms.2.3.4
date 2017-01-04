using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Xamarin.Forms
{

	public static class Watcher
	{
		private static Stack<Stopwatch> s_w = new Stack<Stopwatch>();
		private static Stack<string> s_current = new Stack<string>();
		public static void Start(string current){
			string indent = new string('\t', s_w.Count);
			Debug.WriteLine($"{indent}{current} >>>");
			var sw = new Stopwatch();
			sw.Start();
			s_w.Push(sw);
			s_current.Push(current);
		}

		public static void Stop(){
			var sw = s_w.Pop();
			sw.Stop();
			string current = s_current.Pop();
			string indent = new string('\t', s_w.Count);
			Debug.WriteLine($"{indent}{current} : {sw.ElapsedMilliseconds} ({sw.Elapsed})");
		}

	}
}