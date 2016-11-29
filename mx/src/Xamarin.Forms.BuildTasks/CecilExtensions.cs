using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.Forms.Design {
	public static class CecilExtensions {
		public static bool InheritsFrom(this TypeDefinition type, string fullName) {
			if (type.BaseType == null)
				return false;

			return type.BaseType.FullName == fullName || type.BaseType.Resolve().InheritsFrom(fullName);
		}

		public static bool IsGeneric(this TypeReference type, string fullName) {
			if (!type.IsGenericInstance)
				return false;

			var instance = type as GenericInstanceType;
			if (instance == null)
				return false;

			return instance.GenericArguments.Any(g => g.FullName == fullName);
		}

		public static bool IsAttribute(this TypeDefinition type) {
			return type.InheritsFrom("System.Attribute");
		}

		public static CustomAttribute GetContentPropertyAttribute(this TypeDefinition type) {
			var attribute = type.CustomAttributes.FirstOrDefault(a => a.AttributeType != null && a.AttributeType.FullName == "Xamarin.Forms.ContentPropertyAttribute");

			if (attribute == null && type.BaseType != null)
				attribute = type.BaseType.Resolve().GetContentPropertyAttribute();

			return attribute;
		}
	}
}
