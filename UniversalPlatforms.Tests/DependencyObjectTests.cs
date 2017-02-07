using System;

using NUnit.Framework;
using Windows.UI.Xaml;

namespace UniversalPlatforms.Tests
{
	[TestFixture]
	public class DependencyObjectTests
	{
		class Fish : DependencyObject
		{
			public static readonly DependencyProperty SpeciesProperty = DependencyProperty.Register(nameof(Species), typeof(string), typeof(Fish), new PropertyMetadata("Goldfish", OnSpeciesChanged));

			public string Species {
				get { return (string)GetValue(SpeciesProperty); }
				set { SetValue(SpeciesProperty, value); }
			}

			static void OnSpeciesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
			{
				((Fish)d).OnSpeciesChanged(e);
			}

			void OnSpeciesChanged(DependencyPropertyChangedEventArgs e)
			{
				SpeciesChanged?.Invoke(this, e);
			}

			public event EventHandler<DependencyPropertyChangedEventArgs> SpeciesChanged;
		}

		abstract class AquariumServices : DependencyObject
		{
			public enum Buoyancy { Floats, Sinks, Drifts }

			public static readonly DependencyProperty BuoyancyProperty = DependencyProperty.RegisterAttached("Buoyancy", typeof(Buoyancy), typeof(AquariumServices), new PropertyMetadata(Buoyancy.Floats));

			public static void SetBuoyancy(DependencyObject element, Buoyancy value)
			{
				element.SetValue(BuoyancyProperty, value);
			}

			public static Buoyancy GetBuoyancy(DependencyObject element)
			{
				return (Buoyancy)element.GetValue(BuoyancyProperty);
			}
		}

		[Test]
		public void GetValueForUnsetProperty()
		{
			var fish = new Fish();
			Assert.That(fish.Species, Is.EqualTo("Goldfish"));
		}

		[Test]
		public void GetValueForSetProperty()
		{
			var fish = new Fish();
			fish.Species = "Clownfish";
			Assert.That(fish.Species, Is.EqualTo("Clownfish"));
		}

		[Test]
		public void GetValueForClearedProperty()
		{
			var fish = new Fish();
			fish.Species = "Clownfish";
			Assume.That(fish.Species, Is.EqualTo("Clownfish"));
			fish.ClearValue(Fish.SpeciesProperty);
			Assert.That(fish.Species, Is.EqualTo("Goldfish"));
		}

		[Test]
		public void ReadLocalValueForUnsetProperty()
		{
			var fish = new Fish();
			Assert.That(fish.ReadLocalValue(Fish.SpeciesProperty), Is.EqualTo(DependencyProperty.UnsetValue));
		}

		[Test]
		public void ReadLocalValueForSetProperty()
		{
			var fish = new Fish();
			fish.Species = "Clownfish";
			Assert.That(fish.ReadLocalValue(Fish.SpeciesProperty), Is.EqualTo("Clownfish"));
		}

		[Test]
		public void ReadLocalValueForClearedProperty()
		{
			var fish = new Fish();
			fish.Species = "Clownfish";
			Assume.That(fish.ReadLocalValue(Fish.SpeciesProperty), Is.EqualTo("Clownfish"));
			fish.ClearValue(Fish.SpeciesProperty);
			Assert.That(fish.ReadLocalValue(Fish.SpeciesProperty), Is.EqualTo(DependencyProperty.UnsetValue));
		}

		[Test]
		public void MetadataPropertyChangedCallbackIsFired()
		{
			var fish = new Fish();
			int status = 0;
			int fired = 0;
			EventHandler<DependencyPropertyChangedEventArgs> speciesChanged = (sender, e) => {
				Assert.That(sender, Is.EqualTo(fish));
				Assert.That(e.Property, Is.EqualTo(Fish.SpeciesProperty));
				Assert.That(e.NewValue, Is.Not.EqualTo(e.OldValue));
				if (status != 1)
					Assert.Fail();
				fired++;
			};

			status = 0; //shouldn't fire
			fish.Species = "foo";
			fish.ClearValue(Fish.SpeciesProperty);

			status = 1; //fire!
			Assume.That(fired, Is.EqualTo(0));
			fish.SpeciesChanged += speciesChanged;
			fish.Species = "foo";
			fish.ClearValue(Fish.SpeciesProperty);
			Assert.That(fired, Is.EqualTo(2));

			fish.SpeciesChanged -= speciesChanged;

			fish.Species = "foo";
			fish.ClearValue(Fish.SpeciesProperty);
			Assert.That(fired, Is.EqualTo(2));

		}

		[Test]
		public void RegisterPropertyChangedCallback()
		{
			var fish = new Fish();
			int status = 0;
			int fired = 0;
			DependencyPropertyChangedCallback cb = (sender, dp) => {
				Assert.That(sender, Is.EqualTo(fish));
				Assert.That(dp, Is.EqualTo(Fish.SpeciesProperty));
				if (status != 1)
					Assert.Fail();
				fired++;
			};

			status = 0; //shouldn't fire
			fish.Species = "foo";
			fish.ClearValue(Fish.SpeciesProperty);

			status = 1; //fire!
			Assume.That(fired, Is.EqualTo(0));
			var token = fish.RegisterPropertyChangedCallback(Fish.SpeciesProperty, cb);
			fish.Species = "foo";
			fish.ClearValue(Fish.SpeciesProperty);
			Assert.That(fired, Is.EqualTo(2));

			fish.UnregisterPropertyChangedCallback(Fish.SpeciesProperty, token);

			fish.Species = "foo";
			fish.ClearValue(Fish.SpeciesProperty);
			Assert.That(fired, Is.EqualTo(2));
		}
	}
}