using System.Reflection;
using Microsoft.Windows.Design.Metadata;

#if !MXBUILD
[assembly: AssemblyTitle ("Xamarin.Forms.Core.Design")]
[assembly: AssemblyDescription ("Provides the design-time metadata for the XAML language service.")]
#endif

[assembly: ProvideMetadata (typeof (Xamarin.Forms.Core.Design.RegisterMetadata))]