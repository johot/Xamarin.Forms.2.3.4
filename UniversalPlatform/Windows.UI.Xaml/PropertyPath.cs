namespace Windows.UI.Xaml
{
	public sealed class PropertyPath : DependencyObject
	{
		public PropertyPath(string path)
		{
			Path = path;
		}

		public string Path { get; }
	}
}