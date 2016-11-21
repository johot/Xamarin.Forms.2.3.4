using System.Reflection;
using Microsoft.Windows.Design.Metadata;
using Xamarin.Forms.Xaml.Design;

#if !MXBUILD
[assembly: AssemblyTitle("Xamarin.Forms.Xaml.Design")]
[assembly: AssemblyDescription("Provides the design-time metadata for the XAML language service.")]
#endif
[assembly: ProvideMetadata(typeof (RegisterMetadata))]