using System;

using NUnit.Framework;
using Windows.UI.Xaml;

namespace UniversalPlatforms.Tests
{
	[TestFixture]
	public class DependencyPropertyTests
{
		[Test]
		public void RegisterWithoutMetadata()
		{
			var dp = DependencyProperty.Register("Foo", typeof(string), typeof(MockDependencyObject), null);
			Assert.That(dp, Is.Not.Null);
			Assert.That(dp, Is.TypeOf<DependencyProperty>());
			Assert.That(dp.GetMetadata(typeof(MockDependencyObject)), Is.Null);
		}

		[Test]
		public void Register()
		{
			var dp = DependencyProperty.Register("Foo", typeof(string), typeof(MockDependencyObject), new PropertyMetadata("FooBar"));
			Assert.That(dp, Is.Not.Null);
			Assert.That(dp, Is.TypeOf<DependencyProperty>());
			Assert.That(dp.GetMetadata(typeof(MockDependencyObject)).DefaultValue, Is.EqualTo("FooBar"));
		}

		[Test]
		public void RegisterAttachedWithoutMetadata()
		{
			var dp = DependencyProperty.RegisterAttached("Foo", typeof(string), typeof(MockDependencyObject), null);
			Assert.That(dp, Is.Not.Null);
			Assert.That(dp, Is.TypeOf<DependencyProperty>());
			Assert.That(dp.GetMetadata(typeof(MockDependencyObject)), Is.Null);
		}

		[Test]
		public void RegisterAttached()
		{
			var dp = DependencyProperty.RegisterAttached("Foo", typeof(string), typeof(MockDependencyObject), new PropertyMetadata("FooBar"));
			Assert.That(dp, Is.Not.Null);
			Assert.That(dp, Is.TypeOf<DependencyProperty>());
			Assert.That(dp.GetMetadata(typeof(MockDependencyObject)).DefaultValue, Is.EqualTo("FooBar"));
		}
	}
}