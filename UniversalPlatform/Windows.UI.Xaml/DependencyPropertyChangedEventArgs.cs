namespace Windows.UI.Xaml
{
	public sealed class DependencyPropertyChangedEventArgs
	{
		internal DependencyPropertyChangedEventArgs(object oldValue, object newValue, DependencyProperty property)
		{
			NewValue = newValue;
			OldValue = oldValue;
			Property = property;
		}

		public object NewValue { get; }
		public object OldValue { get; }
		public DependencyProperty Property { get; }
	}
}