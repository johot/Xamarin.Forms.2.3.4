namespace Windows.UI.Xaml.Data
{
	public class Binding : BindingBase
	{
		public IValueConverter Converter { get; set; }
		public string ConverterLanguage { get; set; }
		public object ConverterParameter { get; set; }
		public string ElementName { get; set; }
		public bool FallbackValue { get; set; }
		public BindingMode Mode { get; set; }
		public PropertyPath Path { get; set; }
		public RelativeSource RelativeSource { get; set; }
		public object TargetNullValue { get; set; }
		public UpdateSourceTrigger UpdateSourceTrigger { get; set; }
	}
}