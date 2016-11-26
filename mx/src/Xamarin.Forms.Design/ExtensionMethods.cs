using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.Forms.Design {
	internal static class ExtensionMethods {
		public static string ReadAll(this Stream reader) {
			var bytes = new byte[reader.Length];
			reader.Read(bytes, 0, (int)reader.Length);
			return Encoding.UTF8.GetString(bytes);
		}
	}
}
