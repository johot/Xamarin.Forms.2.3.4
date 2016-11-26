using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Design;

namespace Console {
	class Program {
		static void Main(string[] args) {
			if (args.Count() < 2)
				throw new ArgumentException("XAML Design requires the path of the target dll and output directory as arguments");

			var fileName = args[0];
			if (!File.Exists(fileName))
				throw new FileNotFoundException("Target DLL not found", fileName);

			var outputDirectory = args[1].TrimEnd(new[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar });
			if (!Directory.Exists(outputDirectory))
				Directory.CreateDirectory(outputDirectory);

			var runner = new DesignGenerator();
			var registrationFileBuilder = runner.GenerateFor(fileName);
			if (registrationFileBuilder.Length > 0) {
				File.WriteAllText(Path.Combine(outputDirectory, Path.GetFileNameWithoutExtension(fileName) + ".AttributeTableBuilder.cs"), registrationFileBuilder.ToString());

				using (var reader = Assembly.GetExecutingAssembly().GetManifestResourceStream("Xamarin.Forms.Design.StructOptionsConverter.cs"))
					File.WriteAllText(Path.Combine(outputDirectory, "StructOptionsConverter.cs"), reader.ReadAll());
			}
		}
	}
}
