using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Rs317.Tools.AOT.LinkFileGenerator
{
	class Program
	{
		static void Main(string[] args)
		{
			//Autofac requires some stuff from Core to be remembered.
			//See: https://issuetracker.unity3d.com/issues/il2cpp-notsupportedexceptions-exception-is-thrown-in-build-with-newtonsoft-dot-json-plugin
			StringBuilder builder = new StringBuilder(5000);
			builder.Append($@"<linker>");
			foreach (var name in Directory.GetFiles(Directory.GetCurrentDirectory())
				.Where(n => n.Contains($".dll")))
			{
				Console.WriteLine($"Writing: {name}");
				builder.Append($"\n\t");
				string assemblyName = Path.GetFileNameWithoutExtension(name);
				builder.Append($"<assembly fullname=\"{assemblyName}\" preserve=\"all\"/>");
			}

			builder.Append($"\n\t<assembly fullname=\"System.Core\">");
			builder.Append("\n\t\t<type fullname=\"System.Linq.Expressions.Interpreter.LightLambda\" preserve=\"all\" />");
			builder.Append($"\n\t</assembly>");

			builder.Append("\n");
			builder.Append($@"</linker>");

			File.WriteAllText("link.xml", builder.ToString());
		}
	}
}
