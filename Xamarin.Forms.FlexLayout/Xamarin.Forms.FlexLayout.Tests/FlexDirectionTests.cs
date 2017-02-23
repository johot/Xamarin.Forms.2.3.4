using System;
using NUnit.Framework;
using Xamarin.Forms;
using Xamarin.Forms.Core.UnitTests;

namespace Xamarin.Forms.FlexLayoutTests
{
	[TestFixture]
	public class YGFlexDirectionTest : BaseTestFixture
	{

		[SetUp]
		public override void Setup()
		{
			base.Setup();
			global::Xamarin.Forms.FlexLayout.RegisterEngine(typeof(Xamarin.FlexLayoutEngine.Yoga.YogaEngine));
			Device.PlatformServices = new MockPlatformServices();
		}

		[TearDown]
		public override void TearDown()
		{
			base.TearDown();
			global::Xamarin.Forms.FlexLayout.RegisterEngine(null);
			Device.PlatformServices = null;
		}

		[Test]
		public void TestFlexDirectionColumnWithoutHeight()
		{
			var platform = new UnitPlatform();
			var layout = new FlexLayout();
			layout.FlexDirection = Flex.FlexDirection.Column;
			layout.Platform = platform;

			var label1 = new View { Platform = platform, IsPlatformEnabled = true };
			label1.HeightRequest = 10;
			var label2 = new View { Platform = platform, IsPlatformEnabled = true };
			label2.HeightRequest = 10;
			var label3 = new View { Platform = platform, IsPlatformEnabled = true };
			label3.HeightRequest = 10;

			layout.Children.Add(label1);
			layout.Children.Add(label2);
			layout.Children.Add(label3);


			var measure = layout.Measure(100, double.PositiveInfinity);
			layout.Layout(new Rectangle(0, 0, 100, double.PositiveInfinity));

			Assert.AreEqual(0f, layout.X);
			Assert.AreEqual(0f, layout.Y);
			Assert.AreEqual(100f, layout.Width);
			//Assert.AreEqual(30f, layout.Height);

			Assert.AreEqual(0f, label1.X);
			Assert.AreEqual(0f, label1.Y);
			Assert.AreEqual(100f, label1.Width);
			Assert.AreEqual(10f, label1.Height);

			Assert.AreEqual(0f, label2.X);
			Assert.AreEqual(10f, label2.Y);
			Assert.AreEqual(100f, label2.Width);
			Assert.AreEqual(10f, label2.Height);

			Assert.AreEqual(0f, label3.X);
			Assert.AreEqual(20f, label3.Y);
			Assert.AreEqual(100f, label3.Width);
			Assert.AreEqual(10f, label3.Height);

			//TODO: change direction
			//root.StyleDirection = YogaDirection.RTL;
			layout.Layout(new Rectangle(0, 0, 100, double.PositiveInfinity));

			Assert.AreEqual(0f, layout.X);
			Assert.AreEqual(0f, layout.Y);
			Assert.AreEqual(100f, layout.Width);
			//Assert.AreEqual(30f, layout.Height);

			Assert.AreEqual(0f, label1.X);
			Assert.AreEqual(0f, label1.Y);
			Assert.AreEqual(100f, label1.Width);
			Assert.AreEqual(10f, label1.Height);

			Assert.AreEqual(0f, label2.X);
			Assert.AreEqual(10f, label2.Y);
			Assert.AreEqual(100f, label2.Width);
			Assert.AreEqual(10f, label2.Height);

			Assert.AreEqual(0f, label3.X);
			Assert.AreEqual(20f, label3.Y);
			Assert.AreEqual(100f, label3.Width);
			Assert.AreEqual(10f, label3.Height);
		}

		[Test]
		public void TestFlexDirectionRowNoWidth()
		{
			var platform = new UnitPlatform();
			var layout = new FlexLayout();
			layout.FlexDirection = Flex.FlexDirection.Row;
			layout.Platform = platform;

			var label1 = new View { Platform = platform, IsPlatformEnabled = true };
			label1.WidthRequest = 10;
			var label2 = new View { Platform = platform, IsPlatformEnabled = true };
			label2.WidthRequest = 10;
			var label3 = new View { Platform = platform, IsPlatformEnabled = true };
			label3.WidthRequest = 10;

			layout.Children.Add(label1);
			layout.Children.Add(label2);
			layout.Children.Add(label3);

			var measure = layout.Measure(double.PositiveInfinity, 100);
			layout.Layout(new Rectangle(0, 0, double.PositiveInfinity, 100));

			Assert.AreEqual(0f, layout.X);
			Assert.AreEqual(0f, layout.Y);
			//	Assert.AreEqual(30f, layout.Width);
			Assert.AreEqual(100f, layout.Height);

			Assert.AreEqual(0f, label1.X);
			Assert.AreEqual(0f, label1.Y);
			Assert.AreEqual(10f, label1.Width);
			Assert.AreEqual(100f, label1.Height);

			Assert.AreEqual(10f, label2.X);
			Assert.AreEqual(0f, label2.Y);
			Assert.AreEqual(10f, label2.Width);
			Assert.AreEqual(100f, label2.Height);

			Assert.AreEqual(20f, label3.X);
			Assert.AreEqual(0f, label3.Y);
			Assert.AreEqual(10f, label3.Width);
			Assert.AreEqual(100f, label3.Height);

			//TODO: change direction
			//root.StyleDirection = YogaDirection.RTL;
			layout.Layout(new Rectangle(0, 0, double.PositiveInfinity, 100));

			Assert.AreEqual(0f, label1.X);
			Assert.AreEqual(0f, label1.Y);
			Assert.AreEqual(10f, label1.Width);
			Assert.AreEqual(100f, label1.Height);

			Assert.AreEqual(10f, label2.X);
			Assert.AreEqual(0f, label2.Y);
			Assert.AreEqual(10f, label2.Width);
			Assert.AreEqual(100f, label2.Height);

			Assert.AreEqual(20f, label3.X);
			Assert.AreEqual(0f, label3.Y);
			Assert.AreEqual(10f, label3.Width);
			Assert.AreEqual(100f, label3.Height);
		}

		[Test]
		public void TestFlexDirectionColumn()
		{
			//	YogaNode root = new YogaNode();
			//	root.Width = 100;
			//	root.Height = 100;

			//	YogaNode root_child0 = new YogaNode();
			//	root_child0.Height = 10;
			//	root.Insert(0, root_child0);

			//	YogaNode root_child1 = new YogaNode();
			//	root_child1.Height = 10;
			//	root.Insert(1, root_child1);

			//	YogaNode root_child2 = new YogaNode();
			//	root_child2.Height = 10;
			//	root.Insert(2, root_child2);
			//	root.StyleDirection = YogaDirection.LTR;
			//	root.CalculateLayout();

			//	Assert.AreEqual(0f, root.LayoutX);
			//	Assert.AreEqual(0f, root.LayoutY);
			//	Assert.AreEqual(100f, root.LayoutWidth);
			//	Assert.AreEqual(100f, root.LayoutHeight);

			//	Assert.AreEqual(0f, root_child0.LayoutX);
			//	Assert.AreEqual(0f, root_child0.LayoutY);
			//	Assert.AreEqual(100f, root_child0.LayoutWidth);
			//	Assert.AreEqual(10f, root_child0.LayoutHeight);

			//	Assert.AreEqual(0f, root_child1.LayoutX);
			//	Assert.AreEqual(10f, root_child1.LayoutY);
			//	Assert.AreEqual(100f, root_child1.LayoutWidth);
			//	Assert.AreEqual(10f, root_child1.LayoutHeight);

			//	Assert.AreEqual(0f, root_child2.LayoutX);
			//	Assert.AreEqual(20f, root_child2.LayoutY);
			//	Assert.AreEqual(100f, root_child2.LayoutWidth);
			//	Assert.AreEqual(10f, root_child2.LayoutHeight);

			//	root.StyleDirection = YogaDirection.RTL;
			//	root.CalculateLayout();

			//	Assert.AreEqual(0f, root.LayoutX);
			//	Assert.AreEqual(0f, root.LayoutY);
			//	Assert.AreEqual(100f, root.LayoutWidth);
			//	Assert.AreEqual(100f, root.LayoutHeight);

			//	Assert.AreEqual(0f, root_child0.LayoutX);
			//	Assert.AreEqual(0f, root_child0.LayoutY);
			//	Assert.AreEqual(100f, root_child0.LayoutWidth);
			//	Assert.AreEqual(10f, root_child0.LayoutHeight);

			//	Assert.AreEqual(0f, root_child1.LayoutX);
			//	Assert.AreEqual(10f, root_child1.LayoutY);
			//	Assert.AreEqual(100f, root_child1.LayoutWidth);
			//	Assert.AreEqual(10f, root_child1.LayoutHeight);

			//	Assert.AreEqual(0f, root_child2.LayoutX);
			//	Assert.AreEqual(20f, root_child2.LayoutY);
			//	Assert.AreEqual(100f, root_child2.LayoutWidth);
			//	Assert.AreEqual(10f, root_child2.LayoutHeight);
		}

		[Test]
		public void TestFlexDirectionRow()
		{
			//	YogaNode root = new YogaNode();
			//	root.FlexDirection = YogaFlexDirection.Row;
			//	root.Width = 100;
			//	root.Height = 100;

			//	YogaNode root_child0 = new YogaNode();
			//	root_child0.Width = 10;
			//	root.Insert(0, root_child0);

			//	YogaNode root_child1 = new YogaNode();
			//	root_child1.Width = 10;
			//	root.Insert(1, root_child1);

			//	YogaNode root_child2 = new YogaNode();
			//	root_child2.Width = 10;
			//	root.Insert(2, root_child2);
			//	root.StyleDirection = YogaDirection.LTR;
			//	root.CalculateLayout();

			//	Assert.AreEqual(0f, root.LayoutX);
			//	Assert.AreEqual(0f, root.LayoutY);
			//	Assert.AreEqual(100f, root.LayoutWidth);
			//	Assert.AreEqual(100f, root.LayoutHeight);

			//	Assert.AreEqual(0f, root_child0.LayoutX);
			//	Assert.AreEqual(0f, root_child0.LayoutY);
			//	Assert.AreEqual(10f, root_child0.LayoutWidth);
			//	Assert.AreEqual(100f, root_child0.LayoutHeight);

			//	Assert.AreEqual(10f, root_child1.LayoutX);
			//	Assert.AreEqual(0f, root_child1.LayoutY);
			//	Assert.AreEqual(10f, root_child1.LayoutWidth);
			//	Assert.AreEqual(100f, root_child1.LayoutHeight);

			//	Assert.AreEqual(20f, root_child2.LayoutX);
			//	Assert.AreEqual(0f, root_child2.LayoutY);
			//	Assert.AreEqual(10f, root_child2.LayoutWidth);
			//	Assert.AreEqual(100f, root_child2.LayoutHeight);

			//	root.StyleDirection = YogaDirection.RTL;
			//	root.CalculateLayout();

			//	Assert.AreEqual(0f, root.LayoutX);
			//	Assert.AreEqual(0f, root.LayoutY);
			//	Assert.AreEqual(100f, root.LayoutWidth);
			//	Assert.AreEqual(100f, root.LayoutHeight);

			//	Assert.AreEqual(90f, root_child0.LayoutX);
			//	Assert.AreEqual(0f, root_child0.LayoutY);
			//	Assert.AreEqual(10f, root_child0.LayoutWidth);
			//	Assert.AreEqual(100f, root_child0.LayoutHeight);

			//	Assert.AreEqual(80f, root_child1.LayoutX);
			//	Assert.AreEqual(0f, root_child1.LayoutY);
			//	Assert.AreEqual(10f, root_child1.LayoutWidth);
			//	Assert.AreEqual(100f, root_child1.LayoutHeight);

			//	Assert.AreEqual(70f, root_child2.LayoutX);
			//	Assert.AreEqual(0f, root_child2.LayoutY);
			//	Assert.AreEqual(10f, root_child2.LayoutWidth);
			//	Assert.AreEqual(100f, root_child2.LayoutHeight);
		}

		[Test]
		public void TestFlexDirectionColumnReverse()
		{
			//	YogaNode root = new YogaNode();
			//	root.FlexDirection = YogaFlexDirection.ColumnReverse;
			//	root.Width = 100;
			//	root.Height = 100;

			//	YogaNode root_child0 = new YogaNode();
			//	root_child0.Height = 10;
			//	root.Insert(0, root_child0);

			//	YogaNode root_child1 = new YogaNode();
			//	root_child1.Height = 10;
			//	root.Insert(1, root_child1);

			//	YogaNode root_child2 = new YogaNode();
			//	root_child2.Height = 10;
			//	root.Insert(2, root_child2);
			//	root.StyleDirection = YogaDirection.LTR;
			//	root.CalculateLayout();

			//	Assert.AreEqual(0f, root.LayoutX);
			//	Assert.AreEqual(0f, root.LayoutY);
			//	Assert.AreEqual(100f, root.LayoutWidth);
			//	Assert.AreEqual(100f, root.LayoutHeight);

			//	Assert.AreEqual(0f, root_child0.LayoutX);
			//	Assert.AreEqual(90f, root_child0.LayoutY);
			//	Assert.AreEqual(100f, root_child0.LayoutWidth);
			//	Assert.AreEqual(10f, root_child0.LayoutHeight);

			//	Assert.AreEqual(0f, root_child1.LayoutX);
			//	Assert.AreEqual(80f, root_child1.LayoutY);
			//	Assert.AreEqual(100f, root_child1.LayoutWidth);
			//	Assert.AreEqual(10f, root_child1.LayoutHeight);

			//	Assert.AreEqual(0f, root_child2.LayoutX);
			//	Assert.AreEqual(70f, root_child2.LayoutY);
			//	Assert.AreEqual(100f, root_child2.LayoutWidth);
			//	Assert.AreEqual(10f, root_child2.LayoutHeight);

			//	root.StyleDirection = YogaDirection.RTL;
			//	root.CalculateLayout();

			//	Assert.AreEqual(0f, root.LayoutX);
			//	Assert.AreEqual(0f, root.LayoutY);
			//	Assert.AreEqual(100f, root.LayoutWidth);
			//	Assert.AreEqual(100f, root.LayoutHeight);

			//	Assert.AreEqual(0f, root_child0.LayoutX);
			//	Assert.AreEqual(90f, root_child0.LayoutY);
			//	Assert.AreEqual(100f, root_child0.LayoutWidth);
			//	Assert.AreEqual(10f, root_child0.LayoutHeight);

			//	Assert.AreEqual(0f, root_child1.LayoutX);
			//	Assert.AreEqual(80f, root_child1.LayoutY);
			//	Assert.AreEqual(100f, root_child1.LayoutWidth);
			//	Assert.AreEqual(10f, root_child1.LayoutHeight);

			//	Assert.AreEqual(0f, root_child2.LayoutX);
			//	Assert.AreEqual(70f, root_child2.LayoutY);
			//	Assert.AreEqual(100f, root_child2.LayoutWidth);
			//	Assert.AreEqual(10f, root_child2.LayoutHeight);
		}

		[Test]
		public void TestFlexDirectionRowReverse()
		{
			//	YogaNode root = new YogaNode();
			//	root.FlexDirection = YogaFlexDirection.RowReverse;
			//	root.Width = 100;
			//	root.Height = 100;

			//	YogaNode root_child0 = new YogaNode();
			//	root_child0.Width = 10;
			//	root.Insert(0, root_child0);

			//	YogaNode root_child1 = new YogaNode();
			//	root_child1.Width = 10;
			//	root.Insert(1, root_child1);

			//	YogaNode root_child2 = new YogaNode();
			//	root_child2.Width = 10;
			//	root.Insert(2, root_child2);
			//	root.StyleDirection = YogaDirection.LTR;
			//	root.CalculateLayout();

			//	Assert.AreEqual(0f, root.LayoutX);
			//	Assert.AreEqual(0f, root.LayoutY);
			//	Assert.AreEqual(100f, root.LayoutWidth);
			//	Assert.AreEqual(100f, root.LayoutHeight);

			//	Assert.AreEqual(90f, root_child0.LayoutX);
			//	Assert.AreEqual(0f, root_child0.LayoutY);
			//	Assert.AreEqual(10f, root_child0.LayoutWidth);
			//	Assert.AreEqual(100f, root_child0.LayoutHeight);

			//	Assert.AreEqual(80f, root_child1.LayoutX);
			//	Assert.AreEqual(0f, root_child1.LayoutY);
			//	Assert.AreEqual(10f, root_child1.LayoutWidth);
			//	Assert.AreEqual(100f, root_child1.LayoutHeight);

			//	Assert.AreEqual(70f, root_child2.LayoutX);
			//	Assert.AreEqual(0f, root_child2.LayoutY);
			//	Assert.AreEqual(10f, root_child2.LayoutWidth);
			//	Assert.AreEqual(100f, root_child2.LayoutHeight);

			//	root.StyleDirection = YogaDirection.RTL;
			//	root.CalculateLayout();

			//	Assert.AreEqual(0f, root.LayoutX);
			//	Assert.AreEqual(0f, root.LayoutY);
			//	Assert.AreEqual(100f, root.LayoutWidth);
			//	Assert.AreEqual(100f, root.LayoutHeight);

			//	Assert.AreEqual(0f, root_child0.LayoutX);
			//	Assert.AreEqual(0f, root_child0.LayoutY);
			//	Assert.AreEqual(10f, root_child0.LayoutWidth);
			//	Assert.AreEqual(100f, root_child0.LayoutHeight);

			//	Assert.AreEqual(10f, root_child1.LayoutX);
			//	Assert.AreEqual(0f, root_child1.LayoutY);
			//	Assert.AreEqual(10f, root_child1.LayoutWidth);
			//	Assert.AreEqual(100f, root_child1.LayoutHeight);

			//	Assert.AreEqual(20f, root_child2.LayoutX);
			//	Assert.AreEqual(0f, root_child2.LayoutY);
			//	Assert.AreEqual(10f, root_child2.LayoutWidth);
			//	Assert.AreEqual(100f, root_child2.LayoutHeight);
		}

	}
}

