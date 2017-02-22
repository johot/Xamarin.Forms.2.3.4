using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms.Core.UnitTests;

namespace Xamarin.Forms.FlexLayoutTests
{
    [TestFixture]
    public class FlexLayoutTests : BaseTestFixture
    {
        [SetUp]
        public override void Setup()
        {
            base.Setup();
            global::Xamarin.Forms.FlexLayout.RegisterEngine(typeof(Xamarin.FlexLayoutEngine.Yoga.YogaEngine));
            //   Device.PlatformServices = new MockPlatformServices();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
            global::Xamarin.Forms.FlexLayout.RegisterEngine(null);
            //Device.PlatformServices = null;
        }


        [Test]
        public void ThrowsOnNullAdd()
        {
            var layout = new FlexLayout();

            Assert.Throws<ArgumentNullException>(() => layout.Children.Add(null));
        }

        [Test]
        public void ThrowsOnNullRemove()
        {
            var layout = new FlexLayout();

            Assert.Throws<ArgumentNullException>(() => layout.Children.Remove(null));
        }

        [Test]
        public void TestBasicLayout()
        {
            var platform = new UnitPlatform();
            var layout = new FlexLayout();
            layout.Platform = platform;

            var label1 = new Label { Platform = platform, IsPlatformEnabled = true };
            var label2 = new Label { Platform = platform, IsPlatformEnabled = true };

            layout.Children.Add(label1);
            layout.Children.Add(label2);

            layout.Layout(new Rectangle(0, 0, 912, 912));

            Assert.AreEqual(912, layout.Width);
            Assert.AreEqual(912, layout.Height);

            Assert.AreEqual(new Rectangle(0, 0, 912, 20), label1.Bounds);
            Assert.AreEqual(new Rectangle(0, 20, 912, 20), label2.Bounds);
        }

        [Test]
        public void TestBasicLayoutWithElementsWidth()
        {
            var platform = new UnitPlatform();
            var layout = new FlexLayout();
            layout.Platform = platform;

            var label1 = new Label { Platform = platform, IsPlatformEnabled = true, WidthRequest = 100 };
            var label2 = new Label { Platform = platform, IsPlatformEnabled = true, WidthRequest = 100 };

            layout.Children.Add(label1);
            layout.Children.Add(label2);

            layout.Layout(new Rectangle(0, 0, 912, 912));

            Assert.AreEqual(912, layout.Width);
            Assert.AreEqual(912, layout.Height);

            Assert.AreEqual(new Rectangle(0, 0, 100, 20), label1.Bounds);
            Assert.AreEqual(new Rectangle(0, 20, 100, 20), label2.Bounds);
        }

        [Test]
        public void TestBasicLayoutWithElementsWidthAndMargin()
        {
            var platform = new UnitPlatform();
            var layout = new FlexLayout();
            layout.Platform = platform;

            var label1 = new Label { Platform = platform, IsPlatformEnabled = true, WidthRequest = 100, Margin = new Thickness(5, 0, 0, 0) };
            var label2 = new Label { Platform = platform, IsPlatformEnabled = true, WidthRequest = 100, Margin = new Thickness(5, 0, 0, 0) };

            layout.Children.Add(label1);
            layout.Children.Add(label2);

            layout.Layout(new Rectangle(0, 0, 912, 912));

            Assert.AreEqual(912, layout.Width);
            Assert.AreEqual(912, layout.Height);

            Assert.AreEqual(new Rectangle(5, 0, 100, 20), label1.Bounds);
            Assert.AreEqual(new Rectangle(5, 20, 100, 20), label2.Bounds);
        }

        [Test]
        public void TestAttachingViews()
        {
            var platform = new UnitPlatform();
            var layout = new FlexLayout();
            layout.Platform = platform;
            layout.FlexDirection = Flex.FlexDirection.Column;

            var subView1 = new FlexLayout();
            subView1.Platform = platform;
            subView1.IsPlatformEnabled = true;
            subView1.FlexDirection = Flex.FlexDirection.Column;
            subView1.WidthRequest = 100;
            FlexLayout.SetGrow(subView1, 1);
            layout.Children.Add(subView1);

            var subView2 = new FlexLayout();
            subView2.Platform = platform;
            subView1.IsPlatformEnabled = true;
            subView2.FlexDirection = Flex.FlexDirection.Column;
            subView2.WidthRequest = 150;
            FlexLayout.SetGrow(subView2, 1);
            layout.Children.Add(subView2);

            foreach (var view in new[] { subView1, subView2 })
            {
                var someView = new Label { Text = "Hello", Platform = platform, IsPlatformEnabled = true, WidthRequest = 100 };
                view.Children.Add(someView);
            }
            layout.Layout(new Rectangle(0, 0, 912, 912));


            foreach (var view in new[] { subView1, subView2 })
            {
                var someView = new Label { Text = "Hello", Platform = platform, IsPlatformEnabled = true, WidthRequest = 100 };
                view.Children.Add(someView);
            }

            layout.Layout(new Rectangle(0, 0, 912, 912));

            Assert.AreEqual(subView1.Bounds.Size.Width, 100);
            Assert.AreEqual(subView1.Bounds.Size.Width, 25);

            foreach (var subview in subView1.Children)
            {
                var subviewSize = subview.Bounds.Size;
                Assert.AreNotEqual(subviewSize.Width, 0);
                Assert.AreNotEqual(subviewSize.Height, 0);
                Assert.IsFalse(double.IsNaN(subviewSize.Width));
                Assert.IsFalse(double.IsNaN(subviewSize.Height));
            }

            foreach (var subview in subView2.Children)
            {
                var subviewSize = subview.Bounds.Size;
                Assert.AreNotEqual(subviewSize.Width, 0);
                Assert.AreNotEqual(subviewSize.Height, 0);
                Assert.IsFalse(double.IsNaN(subviewSize.Width));
                Assert.IsFalse(double.IsNaN(subviewSize.Height));
            }
        }

        [Test]
        public void TestIsLeafFlag()
        {
            var platform = new UnitPlatform();
            var layout = new FlexLayout();
            layout.Platform = platform;

            Assert.True(layout.IsLeaf());

            for (int i = 0; i < 10; i++)
            {
                var children = new Label();
                FlexLayout.SetIsIncluded(children, false);
                layout.Children.Add(children);

            }

            Assert.True(layout.IsLeaf());

            var subview = layout.Children[0];
            FlexLayout.SetIsIncluded(subview, true);

            Assert.False(layout.IsLeaf());
            Assert.True(subview.IsLeaf());

            layout.Children.Clear();

            Assert.True(layout.IsLeaf());
        }

    }

    public class BaseTestFixture
    {
        [SetUp]
        public virtual void Setup()
        {
#if !WINDOWS_PHONE
            var culture = Environment.GetEnvironmentVariable("UNIT_TEST_CULTURE");

            if (!string.IsNullOrEmpty(culture))
            {
                var thead = Thread.CurrentThread;
                thead.CurrentCulture = new CultureInfo(culture);
            }
#endif
        }

        [TearDown]
        public virtual void TearDown()
        {

        }
    }

}