namespace Windows.UI.Xaml
{
	public class PropertyMetadata
	{
		PropertyChangedCallback _propertyChangedCallback;

		PropertyMetadata()
		{
		}

		public PropertyMetadata(object defaultValue)
		{
			DefaultValue = defaultValue;
		}

		public PropertyMetadata(object defaultValue, PropertyChangedCallback propertyChangedCallback)
		{
			DefaultValue = defaultValue;
			_propertyChangedCallback = propertyChangedCallback;
		}

		public CreateDefaultValueCallback CreateDefaultValueCallback { get; private set; }
		public object DefaultValue { get; private set; }

		public static PropertyMetadata Create(object defaultValue)
		{
			return new PropertyMetadata(defaultValue);
		}

		public static PropertyMetadata Create(CreateDefaultValueCallback createDefaultValueCallback)
		{
			return new PropertyMetadata() {
				CreateDefaultValueCallback = createDefaultValueCallback,
			};
		}

		public static PropertyMetadata Create(object defaultValue, PropertyChangedCallback propertyChangedCallback)
		{
			return new PropertyMetadata(defaultValue, propertyChangedCallback);
		}

		public static PropertyMetadata Create(CreateDefaultValueCallback createDefaultValueCallback, PropertyChangedCallback propertyChangedCallback)
		{
			return new PropertyMetadata() {
				CreateDefaultValueCallback = createDefaultValueCallback,
			};
		}

		internal void OnPropertyChanged(DependencyObject dependencyObject, object oldValue, object newValue, DependencyProperty property)
		{
			_propertyChangedCallback?.Invoke(dependencyObject, new DependencyPropertyChangedEventArgs(oldValue, newValue, property));
		}
	}
}