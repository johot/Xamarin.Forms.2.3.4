using NUnit.Framework;
using Xamarin.Forms.Core.UnitTests;

namespace Xamarin.Forms.FlexLayoutTests
{

    [TestFixture]
    public class FlexAlignSelfTests : FlexLayoutBaseTestFixture
    {

        [Test]
        public void TestAlignSelfCenter()
        {
            var platform = new UnitPlatform();
            var layout = new FlexLayout();
            layout.FlexDirection = Flex.FlexDirection.Column;
            layout.Platform = platform;

            var view1 = new View { IsPlatformEnabled = true };
            view1.HeightRequest = 10;
            view1.WidthRequest = 10;
            FlexLayout.SetAlignSelf(view1, Flex.Align.Center);

            layout.Children.Add(view1);

            layout.Layout(new Rectangle(0, 0, 100, 100));

            Assert.AreEqual(0, layout.X);
            Assert.AreEqual(0, layout.Y);
            Assert.AreEqual(100, layout.Width);
            Assert.AreEqual(100, layout.Height);

            Assert.AreEqual(45, view1.X);
            Assert.AreEqual(0, view1.Y);
            Assert.AreEqual(10, view1.Width);
            Assert.AreEqual(10, view1.Height);

            //layout.FlowDirection = FlowDirection.RightToLeft;
            //layout.Layout(new Rectangle(0, 0, 100, 100));

            Assert.AreEqual(0, layout.X);
            Assert.AreEqual(0, layout.Y);
            Assert.AreEqual(100, layout.Width);
            Assert.AreEqual(100, layout.Height);

            Assert.AreEqual(45, view1.X);
            Assert.AreEqual(0, view1.Y);
            Assert.AreEqual(10, view1.Width);
            Assert.AreEqual(10, view1.Height);
        }

        [Test]
        public void TestAlignSelfFlexEnd()
        {
            var platform = new UnitPlatform();
            var layout = new FlexLayout();
            layout.FlexDirection = Flex.FlexDirection.Column;
            layout.Platform = platform;

            var view1 = new View { IsPlatformEnabled = true };
            view1.HeightRequest = 10;
            view1.WidthRequest = 10;
            FlexLayout.SetAlignSelf(view1, Flex.Align.FlexEnd);

            layout.Children.Add(view1);

            layout.Layout(new Rectangle(0, 0, 100, 100));

            Assert.AreEqual(0, layout.X);
            Assert.AreEqual(0, layout.Y);
            Assert.AreEqual(100, layout.Width);
            Assert.AreEqual(100, layout.Height);

            Assert.AreEqual(90, view1.X);
            Assert.AreEqual(0, view1.Y);
            Assert.AreEqual(10, view1.Width);
            Assert.AreEqual(10, view1.Height);

            //layout.FlowDirection = FlowDirection.RightToLeft;
            //layout.Layout(new Rectangle(0, 0, 100, 100));

            //Assert.AreEqual(0, layout.X);
            //Assert.AreEqual(0, layout.Y);
            //Assert.AreEqual(100, layout.Width);
            //Assert.AreEqual(100, layout.Height);

            //Assert.AreEqual(0, view1.X);
            //Assert.AreEqual(0, view1.Y);
            //Assert.AreEqual(10, view1.Width);
            //Assert.AreEqual(10, view1.Height);          
        }

        [Test]
        public void TestAlignSelfFlexStart()
        {
            var platform = new UnitPlatform();
            var layout = new FlexLayout();
            layout.FlexDirection = Flex.FlexDirection.Column;
            layout.Platform = platform;

            var view1 = new View { IsPlatformEnabled = true };
            view1.HeightRequest = 10;
            view1.WidthRequest = 10;
            FlexLayout.SetAlignSelf(view1, Flex.Align.FlexStart);

            layout.Children.Add(view1);

            layout.Layout(new Rectangle(0, 0, 100, 100));

            Assert.AreEqual(0, layout.X);
            Assert.AreEqual(0, layout.Y);
            Assert.AreEqual(100, layout.Width);
            Assert.AreEqual(100, layout.Height);

            Assert.AreEqual(0, view1.X);
            Assert.AreEqual(0, view1.Y);
            Assert.AreEqual(10, view1.Width);
            Assert.AreEqual(10, view1.Height);

            //layout.FlowDirection = FlowDirection.RightToLeft;
            //layout.Layout(new Rectangle(0, 0, 100, 100));

            //Assert.AreEqual(0, layout.X);
            //Assert.AreEqual(0, layout.Y);
            //Assert.AreEqual(100, layout.Width);
            //Assert.AreEqual(100, layout.Height);

            //Assert.AreEqual(90, view1.X);
            //Assert.AreEqual(0, view1.Y);
            //Assert.AreEqual(10, view1.Width);
            //Assert.AreEqual(10, view1.Height);   
        }

        [Test]
        public void TestAlignSelfFlexEndOverrideFlexStart()
        {
            var platform = new UnitPlatform();
            var layout = new FlexLayout();
            layout.AlignItems = Flex.Align.FlexStart;
            layout.FlexDirection = Flex.FlexDirection.Column;
            layout.Platform = platform;

            var view1 = new View { IsPlatformEnabled = true };
            view1.HeightRequest = 10;
            view1.WidthRequest = 10;
            FlexLayout.SetAlignSelf(view1, Flex.Align.FlexEnd);

            layout.Children.Add(view1);

            layout.Layout(new Rectangle(0, 0, 100, 100));

            Assert.AreEqual(0, layout.X);
            Assert.AreEqual(0, layout.Y);
            Assert.AreEqual(100, layout.Width);
            Assert.AreEqual(100, layout.Height);

            Assert.AreEqual(90, view1.X);
            Assert.AreEqual(0, view1.Y);
            Assert.AreEqual(10, view1.Width);
            Assert.AreEqual(10, view1.Height);

            //layout.FlowDirection = FlowDirection.RightToLeft;
            //layout.Layout(new Rectangle(0, 0, 100, 100));

            //Assert.AreEqual(0, layout.X);
            //Assert.AreEqual(0, layout.Y);
            //Assert.AreEqual(100, layout.Width);
            //Assert.AreEqual(100, layout.Height);

            //Assert.AreEqual(0, view1.X);
            //Assert.AreEqual(0, view1.Y);
            //Assert.AreEqual(10, view1.Width);
            //Assert.AreEqual(10, view1.Height);   

        }

        [Test]
        public void TestAlignSelfBaseline()
        {
            var platform = new UnitPlatform();
            var layout = new FlexLayout();
            layout.FlexDirection = Flex.FlexDirection.Row;
            layout.Platform = platform;

            var view1 = new View { IsPlatformEnabled = true , Platform = platform };
            view1.HeightRequest = 50;
            view1.WidthRequest = 50;
            FlexLayout.SetAlignSelf(view1, Flex.Align.Baseline);

            var view2 = new FlexLayout { IsPlatformEnabled = true, Platform = platform };
            view2.FlexDirection = Flex.FlexDirection.Column;
            view2.Platform = platform;

            view2.HeightRequest = 20;
            view2.WidthRequest = 50;
            FlexLayout.SetAlignSelf(view2, Flex.Align.Baseline);

            var view3 = new View { IsPlatformEnabled = true, Platform = platform };

            view3.HeightRequest = 10;
            view3.WidthRequest = 50;
            view2.Children.Add(view3);

            layout.Children.Add(view1);
            layout.Children.Add(view2);

            layout.Layout(new Rectangle(0, 0, 100, 100));

            Assert.AreEqual(0, layout.X);
            Assert.AreEqual(0, layout.Y);
            Assert.AreEqual(100, layout.Width);
            Assert.AreEqual(100, layout.Height);

            Assert.AreEqual(0, view1.X);
            Assert.AreEqual(0, view1.Y);
            Assert.AreEqual(50, view1.Width);
            Assert.AreEqual(50, view1.Height);

            Assert.AreEqual(50, view2.X);
            Assert.AreEqual(40, view2.Y);
            Assert.AreEqual(50, view2.Width);
            Assert.AreEqual(20, view2.Height);

            Assert.AreEqual(0, view3.X);
            Assert.AreEqual(0, view3.Y);
            Assert.AreEqual(50, view3.Width);
            Assert.AreEqual(10, view3.Height);

            //layout.FlowDirection = FlowDirection.RightToLeft;
            //layout.Layout(new Rectangle(0, 0, 100, 100));

            //Assert.AreEqual(0, layout.X);
            //Assert.AreEqual(0, layout.Y);
            //Assert.AreEqual(100, layout.Width);
            //Assert.AreEqual(100, layout.Height);

            //Assert.AreEqual(50, view1.X);
            //Assert.AreEqual(0, view1.Y);
            //Assert.AreEqual(50, view1.Width);
            //Assert.AreEqual(50, view1.Height);

            //Assert.AreEqual(0, view2.X);
            //Assert.AreEqual(40, view2.Y);
            //Assert.AreEqual(50, view2.Width);
            //Assert.AreEqual(20, view2.Height);

            //Assert.AreEqual(0, view3.X);
            //Assert.AreEqual(0, view3.Y);
            //Assert.AreEqual(50, view3.Width);
            //Assert.AreEqual(10, view3.Height);           
        }
    }
}
