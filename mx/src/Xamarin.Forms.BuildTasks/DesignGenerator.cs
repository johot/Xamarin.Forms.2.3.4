using Mono.Cecil;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace Xamarin.Forms.Design {
	public class DesignGenerator {
		private const string ViewTypeName = "Xamarin.Forms.VisualElement";
		private const string MarkupExtensionTypeName = "Xamarin.Forms.Xaml.IMarkupExtension";
		private const string FormsXamlDllName = "Xamarin.Forms.Xaml.dll";
		private const string FormsCoreName = "Xamarin.Forms.Core.dll";

		public DesignGenerator() {
		}

		public StringBuilder GenerateFor(string fileName) {
			StringBuilder newClassBuilder = new StringBuilder();
			var hasMetadata = false;

			var assembly = GetAssembly(fileName);
			var generatedTypes = new List<TypeDefinition>();

			GenerateProlog(fileName, newClassBuilder);
			newClassBuilder.Append(
@"		private void AddAttributesForTypes()
		{
			Type type;
");

			foreach (var type in assembly.Modules.SelectMany(m => m.Types).OrderBy(t => t.Name)) {
				var generatedMetadata = GenerateClassMethod(newClassBuilder, type) ||
					GenerateOptionMethod(newClassBuilder, type) ||
					GenerateMarkupMethod(newClassBuilder, type);

				if (generatedMetadata)
					generatedTypes.Add(type);

				hasMetadata |= generatedMetadata;
			}
			newClassBuilder.Append(
@"		}
");

			// Use first public type to get at the assembly.
			var publicType = assembly.Modules.SelectMany(m => m.Types).OrderBy(t => t.FullName).First(t => t.IsPublic);
			var namespaces = generatedTypes.Select(t => t.Namespace).GroupBy(ns => ns).Select(g => string.Format(
@"
			AddAssemblyAttributes(assembly, new XmlnsDefinitionAttribute(""http://xamarin.com/schemas/2014/forms"", ""{0}""));", g.Key));

			newClassBuilder.Append(
@"
		public AttributeTableBuilder()
		{
			var assembly = typeof(" + publicType.FullName + @").Assembly;
" + string.Join(Environment.NewLine, namespaces) + @"

			AddAttributesForTypes();
		}

");

			GenerateEpilog(newClassBuilder);

			return hasMetadata ? newClassBuilder : new StringBuilder();
		}

		private static AssemblyDefinition GetAssembly(string fileName) {
			var rs = new DefaultAssemblyResolver();
			rs.AddSearchDirectory(Path.GetDirectoryName(fileName));

			var assembly = AssemblyDefinition.ReadAssembly(fileName, new ReaderParameters { AssemblyResolver = rs });
			return assembly;
		}

		private bool GenerateMarkupMethod(StringBuilder newClassBuilder, TypeDefinition type) {
			if (!type.IsAbstract && type.Interfaces.Any(i => i.FullName == MarkupExtensionTypeName)) {
				newClassBuilder.AppendFormat(
@"			type = typeof ({0});
			AddTypeAttributes (type, new MarkupExtensionReturnTypeAttribute (), new EditorBrowsableAttribute(EditorBrowsableState.Always));

", type.FullName);

				return true;
			}

			return false;
		}

		private bool GenerateOptionMethod(StringBuilder newClassBuilder, TypeDefinition type) {
			if (!type.IsEnum && type.IsPublic && !type.IsAttribute() &&
				type.Fields.Any(f => f.IsStatic && f.IsInitOnly && f.FieldType.FullName == type.FullName)) {
				newClassBuilder.AppendFormat(
@"			type = typeof({0});
			AddTypeAttributes(type, new TypeConverterAttribute(typeof(StructOptionsConverter<{0}>)));

", type.FullName);
				return true;
			}

			return false;
		}

		private bool GenerateClassMethod(StringBuilder newClassBuilder, TypeDefinition type) {
			if (!type.IsAbstract && type.IsPublic && type.InheritsFrom(ViewTypeName)) {
				newClassBuilder.AppendFormat(
@"			type = typeof({0});
			AddTypeAttributes(type, new EditorBrowsableAttribute(EditorBrowsableState.Always));
			AddAttributesForType(type);
", type.FullName);

				var contentAttribute = type.GetContentPropertyAttribute();

				if (contentAttribute != null) {
					newClassBuilder.AppendFormat(
@"			AddTypeAttributes(type, new ContentPropertyAttribute(""{0}""));
", contentAttribute.ConstructorArguments.First().Value);
				}

				if (!type.Methods.Any(m => m.Name == ".ctor" && m.Parameters.Count == 0 && m.IsPublic)) {
					// If there isn't a public parameterless constructor, provide a dummy type converter 
					// to satisfy the intellisense
					newClassBuilder.AppendLine(
@"			AddTypeAttributes(type, new TypeConverterAttribute(typeof(StringConverter)));");
				}

				newClassBuilder.AppendLine();
				return true;
			}

			return false;
		}

		private void GenerateProlog(string fileName, StringBuilder newClassBuilder) {
			var ns = Path.GetFileNameWithoutExtension(fileName);
			newClassBuilder.Append(
@"
[assembly: Microsoft.Windows.Design.Metadata.ProvideMetadata(typeof(").Append(ns).Append(@".RegisterMetadata))]

namespace ").Append(ns).Append(@"
{
	using System;
	using System.ComponentModel;
	using System.Reflection;
	using System.Windows.Markup;
	using Microsoft.Windows.Design.Metadata;
	using Xamarin.Forms.Design;

	internal class RegisterMetadata : IProvideAttributeTable
	{
		AttributeTable IProvideAttributeTable.AttributeTable
		{
			get
			{
				AttributeTableBuilder builder = new AttributeTableBuilder();
				return builder.CreateTable();
			}
		}
	}

	internal partial class AttributeTableBuilder : Microsoft.Windows.Design.Metadata.AttributeTableBuilder
	{
");
		}

		private void GenerateEpilog(StringBuilder newClassBuilder) {
			newClassBuilder.Append(
@"		private void AddTypeAttributes(Type type, params Attribute[] attribs)
		{
			base.AddCallback(type, builder => builder.AddCustomAttributes(attribs));
		}

		private void AddMemberAttributes(Type type, string memberName, params Attribute[] attribs)
		{
			base.AddCallback(type, builder => builder.AddCustomAttributes(memberName, attribs));
		}

		private void AddAssemblyAttributes(Assembly assembly, params Attribute[] attribs)
		{
			base.AddCustomAttributes(assembly, attribs);
		}

		partial void AddAttributesForType(Type type);
	}
}
");
		}
	}
}