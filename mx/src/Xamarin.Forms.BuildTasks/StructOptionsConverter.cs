namespace Xamarin.Forms.Design {
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.ComponentModel;
	using System.Threading.Tasks;
	using System.Reflection;
	using System.Globalization;

	internal class StructOptionsConverter<T> : TypeConverter {
		private static Lazy<string[]> StandardValues = new Lazy<string[]>(() =>
			ReflectionExtensions.GetFields(typeof(T))
				.Where(fi => fi.IsStatic && fi.IsPublic &&
					!fi.CustomAttributes.Any(a => a.AttributeType == typeof(ObsoleteAttribute)))
				.Select(fi => fi.Name).ToArray());

		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) {
			return sourceType == typeof(string);
		}

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value) {
			if (value != null && StandardValues.Value.Contains(value.ToString())) {
				// It doesn't matter what we return here as long as we don't throw
				return null;
			}

			return base.ConvertFrom(context, culture, value);
		}

		public override bool IsValid(ITypeDescriptorContext context, object value) {
			if (value == null) {
				return false;
			}

			return StandardValues.Value.Contains(value.ToString()) ||
				value.GetType().FullName.StartsWith("Xamarin.Forms.OnPlatform");
		}

		public override bool GetStandardValuesSupported(ITypeDescriptorContext context) {
			return true;
		}

		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context) {
			return true;
		}

		public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context) {
			return new StandardValuesCollection(StandardValues.Value);
		}
	}

	internal static class ReflectionExtensions {
		private static IEnumerable<T> GetParts<T>(Type type, Func<TypeInfo, IEnumerable<T>> selector) {
			TypeInfo ti;
			for (Type t = type; t != null; t = ti.BaseType) {
				ti = IntrospectionExtensions.GetTypeInfo(t);
				foreach (T obj in selector(ti))
					yield return obj;
			}
		}

		public static IEnumerable<FieldInfo> GetFields(this Type type) {
			return ReflectionExtensions.GetParts<FieldInfo>(type, (Func<TypeInfo, IEnumerable<FieldInfo>>)(i => i.DeclaredFields));
		}
	}

}
