using System;
using System.Runtime.CompilerServices;

namespace Windows.UI.Xaml
{
	public sealed class DependencyProperty
	{
		string _name;
		Type _propertyType, _ownerType;
		PropertyMetadata _typeMetadata;

		DependencyProperty()
		{
		}

		public static object UnsetValue { get; } = new object();

		public PropertyMetadata GetMetadata(Type forType)
		{
			return _typeMetadata;
		}

		public static DependencyProperty Register(string name, Type propertyType, Type ownerType, PropertyMetadata typeMetadata)
		{
			return new DependencyProperty {
				_name = name,
				_propertyType = propertyType,
				_ownerType = ownerType,
				_typeMetadata = typeMetadata,
			};
		}

		public static DependencyProperty RegisterAttached(string name, Type propertyType, Type ownerType, PropertyMetadata defaultMetadata)
		{
			return new DependencyProperty {
				_name = name,
				_propertyType = propertyType,
				_ownerType = ownerType,
				_typeMetadata = defaultMetadata,
			};
		}
	}
}