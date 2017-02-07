namespace Windows.UI.Xaml
{
	public sealed class DataContextChangedEventArgs
	{
		public bool Handled { get; set; }
		public object NewValue { get; }
	}
}