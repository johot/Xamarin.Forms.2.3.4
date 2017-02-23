using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Core.UnitTests;
using Xamarin.Forms.FlexLayoutTests;

namespace Xamarin.Forms.FlexLayoutTests
{
    [TestFixture]

    public class FlexTest : FlexLayoutBaseTestFixture
    {
        [Test]
        public void TestFlexBasisFlexGrowColumn()
        {
            var platform = new UnitPlatform((view, width, height) => { return new SizeRequest(new Size(0, 0)); });
            var layout = new FlexLayout();
            layout.FlexDirection = Flex.FlexDirection.Column;
            layout.Platform = platform;

            var view1 = new View { IsPlatformEnabled = true, Platform = platform };
            FlexLayout.SetGrow(view1, 1);
            FlexLayout.SetBasis(view1, 50);

            var view2 = new View { IsPlatformEnabled = true, Platform = platform };
            FlexLayout.SetGrow(view2, 1);

            layout.Children.Add(view1);
            layout.Children.Add(view2);

            layout.Layout(new Rectangle(0, 0, 100, 100));

            Assert.AreEqual(0, layout.X);
            Assert.AreEqual(0, layout.Y);
            Assert.AreEqual(100, layout.Width);
            Assert.AreEqual(100, layout.Height);

            Assert.AreEqual(0, view1.X);
            Assert.AreEqual(0, view1.Y);
            Assert.AreEqual(100, view1.Width);
            Assert.AreEqual(75, view1.Height);

            Assert.AreEqual(0, view2.X);
            Assert.AreEqual(75, view2.Y);
            Assert.AreEqual(100, view2.Width);
            Assert.AreEqual(25, view2.Height);

            //layout.FlowDirection = FlowDirection.RightToLeft;
            //layout.Layout(new Rectangle(0, 0, 100, 100));

            //Assert.AreEqual(0, layout.X);
            //Assert.AreEqual(0, layout.Y);
            //Assert.AreEqual(100, layout.Width);
            //Assert.AreEqual(100, layout.Height);

            //Assert.AreEqual(0, view1.X);
            //Assert.AreEqual(0, view1.Y);
            //Assert.AreEqual(100, view1.Width);
            //Assert.AreEqual(75, view1.Height);

            //Assert.AreEqual(0, view2.X);
            //Assert.AreEqual(75, view2.Y);
            //Assert.AreEqual(100, view2.Width);
            //Assert.AreEqual(25, view2.Height);
        }

        [Test]
        public void TestFlexBasisflexGrowRow()
        {

            var platform = new UnitPlatform((view, width, height) => { return new SizeRequest(new Size(0, 0)); });
            var layout = new FlexLayout();
            layout.FlexDirection = Flex.FlexDirection.Row;
            layout.Platform = platform;

            var view1 = new View { IsPlatformEnabled = true, Platform = platform };
            FlexLayout.SetGrow(view1, 1);
            FlexLayout.SetBasis(view1, 50);

            var view2 = new View { IsPlatformEnabled = true, Platform = platform };
            FlexLayout.SetGrow(view2, 1);

            layout.Children.Add(view1);
            layout.Children.Add(view2);

            layout.Layout(new Rectangle(0, 0, 100, 100));

            Assert.AreEqual(0, layout.X);
            Assert.AreEqual(0, layout.Y);
            Assert.AreEqual(100, layout.Width);
            Assert.AreEqual(100, layout.Height);

            Assert.AreEqual(0, view1.X);
            Assert.AreEqual(0, view1.Y);
            Assert.AreEqual(75, view1.Width);
            Assert.AreEqual(100, view1.Height);

            Assert.AreEqual(75, view2.X);
            Assert.AreEqual(0, view2.Y);
            Assert.AreEqual(25, view2.Width);
            Assert.AreEqual(100, view2.Height);

            //layout.FlowDirection = FlowDirection.RightToLeft;
            //layout.Layout(new Rectangle(0, 0, 100, 100));

            //Assert.AreEqual(0, layout.X);
            //Assert.AreEqual(0, layout.Y);
            //Assert.AreEqual(100, layout.Width);
            //Assert.AreEqual(100, layout.Height);

            //Assert.AreEqual(25, view1.X);
            //Assert.AreEqual(0, view1.Y);
            //Assert.AreEqual(75, view1.Width);
            //Assert.AreEqual(100, view1.Height);

            //Assert.AreEqual(0, view2.X);
            //Assert.AreEqual(0, view2.Y);
            //Assert.AreEqual(25, view2.Width);
            //Assert.AreEqual(100, view2.Height);

        }


        [Test]
        public void TestFlexBasisFlexShrinkColumn()
        {
            var platform = new UnitPlatform((view, width, height) => { return new SizeRequest(new Size(width, height)); });
            var layout = new FlexLayout();
            layout.FlexDirection = Flex.FlexDirection.Column;
            layout.Platform = platform;

            var view1 = new View { IsPlatformEnabled = true, Platform = platform };
            FlexLayout.SetShrink(view1, 1);
            FlexLayout.SetBasis(view1, 100);

            var view2 = new View { IsPlatformEnabled = true, Platform = platform };
            FlexLayout.SetBasis(view2, 50);

            layout.Children.Add(view1);
            layout.Children.Add(view2);

            layout.Layout(new Rectangle(0, 0, 100, 100));


            Assert.AreEqual(0, layout.X);
            Assert.AreEqual(0, layout.Y);
            Assert.AreEqual(100, layout.Width);
            Assert.AreEqual(100, layout.Height);

            Assert.AreEqual(0, view1.X);
            Assert.AreEqual(0, view1.Y);
            Assert.AreEqual(100, view1.Width);
            Assert.AreEqual(50, view1.Height);

            //Assert.AreEqual(0, view2.X);
            //Assert.AreEqual(50, view2.Y);
            //Assert.AreEqual(100, view2.Width);
            //Assert.AreEqual(50, view2.Height);

            //layout.FlowDirection = FlowDirection.RightToLeft;
            //layout.Layout(new Rectangle(0, 0, 100, 100));

            //Assert.AreEqual(0, layout.X);
            //Assert.AreEqual(0, layout.Y);
            //Assert.AreEqual(100, layout.Width);
            //Assert.AreEqual(100, layout.Height);

            //Assert.AreEqual(25, view1.X);
            //Assert.AreEqual(0, view1.Y);
            //Assert.AreEqual(75, view1.Width);
            //Assert.AreEqual(100, view1.Height);

            //Assert.AreEqual(0, view2.X);
            //Assert.AreEqual(0, view2.Y);
            //Assert.AreEqual(25, view2.Width);
            //Assert.AreEqual(100, view2.Height);
        }


        


        //    Assert.AreEqual(0f, root_child0.LayoutX);

        //    Assert.AreEqual(0f, root_child0.LayoutY);

        //    Assert.AreEqual(100f, root_child0.LayoutWidth);

        //    Assert.AreEqual(50f, root_child0.LayoutHeight);



        //    Assert.AreEqual(0f, root_child1.LayoutX);

        //    Assert.AreEqual(50f, root_child1.LayoutY);

        //    Assert.AreEqual(100f, root_child1.LayoutWidth);

        //    Assert.AreEqual(50f, root_child1.LayoutHeight);



        //    root.StyleDirection = YogaDirection.RTL;

        //    root.CalculateLayout();



        //    Assert.AreEqual(0f, root.LayoutX);

        //    Assert.AreEqual(0f, root.LayoutY);

        //    Assert.AreEqual(100f, root.LayoutWidth);

        //    Assert.AreEqual(100f, root.LayoutHeight);



        //    Assert.AreEqual(0f, root_child0.LayoutX);

        //    Assert.AreEqual(0f, root_child0.LayoutY);

        //    Assert.AreEqual(100f, root_child0.LayoutWidth);

        //    Assert.AreEqual(50f, root_child0.LayoutHeight);



        //    Assert.AreEqual(0f, root_child1.LayoutX);

        //    Assert.AreEqual(50f, root_child1.LayoutY);

        //    Assert.AreEqual(100f, root_child1.LayoutWidth);

        //    Assert.AreEqual(50f, root_child1.LayoutHeight);

        //}



        //[Test]

        //public void Test_flex_basis_flex_shrink_row()

        //{

        //    YogaNode root = new YogaNode();

        //    root.FlexDirection = YogaFlexDirection.Row;

        //    root.Width = 100;

        //    root.Height = 100;



        //    YogaNode root_child0 = new YogaNode();

        //    root_child0.FlexShrink = 1;

        //    root_child0.FlexBasis = 100;

        //    root.Insert(0, root_child0);



        //    YogaNode root_child1 = new YogaNode();

        //    root_child1.FlexBasis = 50;

        //    root.Insert(1, root_child1);

        //    root.StyleDirection = YogaDirection.LTR;

        //    root.CalculateLayout();



        //    Assert.AreEqual(0f, root.LayoutX);

        //    Assert.AreEqual(0f, root.LayoutY);

        //    Assert.AreEqual(100f, root.LayoutWidth);

        //    Assert.AreEqual(100f, root.LayoutHeight);



        //    Assert.AreEqual(0f, root_child0.LayoutX);

        //    Assert.AreEqual(0f, root_child0.LayoutY);

        //    Assert.AreEqual(50f, root_child0.LayoutWidth);

        //    Assert.AreEqual(100f, root_child0.LayoutHeight);



        //    Assert.AreEqual(50f, root_child1.LayoutX);

        //    Assert.AreEqual(0f, root_child1.LayoutY);

        //    Assert.AreEqual(50f, root_child1.LayoutWidth);

        //    Assert.AreEqual(100f, root_child1.LayoutHeight);



        //    root.StyleDirection = YogaDirection.RTL;

        //    root.CalculateLayout();



        //    Assert.AreEqual(0f, root.LayoutX);

        //    Assert.AreEqual(0f, root.LayoutY);

        //    Assert.AreEqual(100f, root.LayoutWidth);

        //    Assert.AreEqual(100f, root.LayoutHeight);



        //    Assert.AreEqual(50f, root_child0.LayoutX);

        //    Assert.AreEqual(0f, root_child0.LayoutY);

        //    Assert.AreEqual(50f, root_child0.LayoutWidth);

        //    Assert.AreEqual(100f, root_child0.LayoutHeight);



        //    Assert.AreEqual(0f, root_child1.LayoutX);

        //    Assert.AreEqual(0f, root_child1.LayoutY);

        //    Assert.AreEqual(50f, root_child1.LayoutWidth);

        //    Assert.AreEqual(100f, root_child1.LayoutHeight);

        //}



        //[Test]

        //public void Test_flex_shrink_to_zero()

        //{

        //    YogaNode root = new YogaNode();

        //    root.Height = 75;



        //    YogaNode root_child0 = new YogaNode();

        //    root_child0.Width = 50;

        //    root_child0.Height = 50;

        //    root.Insert(0, root_child0);



        //    YogaNode root_child1 = new YogaNode();

        //    root_child1.FlexShrink = 1;

        //    root_child1.Width = 50;

        //    root_child1.Height = 50;

        //    root.Insert(1, root_child1);



        //    YogaNode root_child2 = new YogaNode();

        //    root_child2.Width = 50;

        //    root_child2.Height = 50;

        //    root.Insert(2, root_child2);

        //    root.StyleDirection = YogaDirection.LTR;

        //    root.CalculateLayout();



        //    Assert.AreEqual(0f, root.LayoutX);

        //    Assert.AreEqual(0f, root.LayoutY);

        //    Assert.AreEqual(50f, root.LayoutWidth);

        //    Assert.AreEqual(75f, root.LayoutHeight);



        //    Assert.AreEqual(0f, root_child0.LayoutX);

        //    Assert.AreEqual(0f, root_child0.LayoutY);

        //    Assert.AreEqual(50f, root_child0.LayoutWidth);

        //    Assert.AreEqual(50f, root_child0.LayoutHeight);



        //    Assert.AreEqual(0f, root_child1.LayoutX);

        //    Assert.AreEqual(50f, root_child1.LayoutY);

        //    Assert.AreEqual(50f, root_child1.LayoutWidth);

        //    Assert.AreEqual(0f, root_child1.LayoutHeight);



        //    Assert.AreEqual(0f, root_child2.LayoutX);

        //    Assert.AreEqual(50f, root_child2.LayoutY);

        //    Assert.AreEqual(50f, root_child2.LayoutWidth);

        //    Assert.AreEqual(50f, root_child2.LayoutHeight);



        //    root.StyleDirection = YogaDirection.RTL;

        //    root.CalculateLayout();



        //    Assert.AreEqual(0f, root.LayoutX);

        //    Assert.AreEqual(0f, root.LayoutY);

        //    Assert.AreEqual(50f, root.LayoutWidth);

        //    Assert.AreEqual(75f, root.LayoutHeight);



        //    Assert.AreEqual(0f, root_child0.LayoutX);

        //    Assert.AreEqual(0f, root_child0.LayoutY);

        //    Assert.AreEqual(50f, root_child0.LayoutWidth);

        //    Assert.AreEqual(50f, root_child0.LayoutHeight);



        //    Assert.AreEqual(0f, root_child1.LayoutX);

        //    Assert.AreEqual(50f, root_child1.LayoutY);

        //    Assert.AreEqual(50f, root_child1.LayoutWidth);

        //    Assert.AreEqual(0f, root_child1.LayoutHeight);



        //    Assert.AreEqual(0f, root_child2.LayoutX);

        //    Assert.AreEqual(50f, root_child2.LayoutY);

        //    Assert.AreEqual(50f, root_child2.LayoutWidth);

        //    Assert.AreEqual(50f, root_child2.LayoutHeight);

        //}



        //[Test]

        //public void Test_flex_basis_overrides_main_size()

        //{

        //    YogaNode root = new YogaNode();

        //    root.Width = 100;

        //    root.Height = 100;



        //    YogaNode root_child0 = new YogaNode();

        //    root_child0.FlexGrow = 1;

        //    root_child0.FlexBasis = 50;

        //    root_child0.Height = 20;

        //    root.Insert(0, root_child0);



        //    YogaNode root_child1 = new YogaNode();

        //    root_child1.FlexGrow = 1;

        //    root_child1.Height = 10;

        //    root.Insert(1, root_child1);



        //    YogaNode root_child2 = new YogaNode();

        //    root_child2.FlexGrow = 1;

        //    root_child2.Height = 10;

        //    root.Insert(2, root_child2);

        //    root.StyleDirection = YogaDirection.LTR;

        //    root.CalculateLayout();



        //    Assert.AreEqual(0f, root.LayoutX);

        //    Assert.AreEqual(0f, root.LayoutY);

        //    Assert.AreEqual(100f, root.LayoutWidth);

        //    Assert.AreEqual(100f, root.LayoutHeight);



        //    Assert.AreEqual(0f, root_child0.LayoutX);

        //    Assert.AreEqual(0f, root_child0.LayoutY);

        //    Assert.AreEqual(100f, root_child0.LayoutWidth);

        //    Assert.AreEqual(60f, root_child0.LayoutHeight);



        //    Assert.AreEqual(0f, root_child1.LayoutX);

        //    Assert.AreEqual(60f, root_child1.LayoutY);

        //    Assert.AreEqual(100f, root_child1.LayoutWidth);

        //    Assert.AreEqual(20f, root_child1.LayoutHeight);



        //    Assert.AreEqual(0f, root_child2.LayoutX);

        //    Assert.AreEqual(80f, root_child2.LayoutY);

        //    Assert.AreEqual(100f, root_child2.LayoutWidth);

        //    Assert.AreEqual(20f, root_child2.LayoutHeight);



        //    root.StyleDirection = YogaDirection.RTL;

        //    root.CalculateLayout();



        //    Assert.AreEqual(0f, root.LayoutX);

        //    Assert.AreEqual(0f, root.LayoutY);

        //    Assert.AreEqual(100f, root.LayoutWidth);

        //    Assert.AreEqual(100f, root.LayoutHeight);



        //    Assert.AreEqual(0f, root_child0.LayoutX);

        //    Assert.AreEqual(0f, root_child0.LayoutY);

        //    Assert.AreEqual(100f, root_child0.LayoutWidth);

        //    Assert.AreEqual(60f, root_child0.LayoutHeight);



        //    Assert.AreEqual(0f, root_child1.LayoutX);

        //    Assert.AreEqual(60f, root_child1.LayoutY);

        //    Assert.AreEqual(100f, root_child1.LayoutWidth);

        //    Assert.AreEqual(20f, root_child1.LayoutHeight);



        //    Assert.AreEqual(0f, root_child2.LayoutX);

        //    Assert.AreEqual(80f, root_child2.LayoutY);

        //    Assert.AreEqual(100f, root_child2.LayoutWidth);

        //    Assert.AreEqual(20f, root_child2.LayoutHeight);

        //}



        //[Test]

        //public void Test_flex_grow_shrink_at_most()

        //{

        //    YogaNode root = new YogaNode();

        //    root.Width = 100;

        //    root.Height = 100;



        //    YogaNode root_child0 = new YogaNode();

        //    root.Insert(0, root_child0);



        //    YogaNode root_child0_child0 = new YogaNode();

        //    root_child0_child0.FlexGrow = 1;

        //    root_child0_child0.FlexShrink = 1;

        //    root_child0.Insert(0, root_child0_child0);

        //    root.StyleDirection = YogaDirection.LTR;

        //    root.CalculateLayout();



        //    Assert.AreEqual(0f, root.LayoutX);

        //    Assert.AreEqual(0f, root.LayoutY);

        //    Assert.AreEqual(100f, root.LayoutWidth);

        //    Assert.AreEqual(100f, root.LayoutHeight);



        //    Assert.AreEqual(0f, root_child0.LayoutX);

        //    Assert.AreEqual(0f, root_child0.LayoutY);

        //    Assert.AreEqual(100f, root_child0.LayoutWidth);

        //    Assert.AreEqual(0f, root_child0.LayoutHeight);



        //    Assert.AreEqual(0f, root_child0_child0.LayoutX);

        //    Assert.AreEqual(0f, root_child0_child0.LayoutY);

        //    Assert.AreEqual(100f, root_child0_child0.LayoutWidth);

        //    Assert.AreEqual(0f, root_child0_child0.LayoutHeight);



        //    root.StyleDirection = YogaDirection.RTL;

        //    root.CalculateLayout();



        //    Assert.AreEqual(0f, root.LayoutX);

        //    Assert.AreEqual(0f, root.LayoutY);

        //    Assert.AreEqual(100f, root.LayoutWidth);

        //    Assert.AreEqual(100f, root.LayoutHeight);



        //    Assert.AreEqual(0f, root_child0.LayoutX);

        //    Assert.AreEqual(0f, root_child0.LayoutY);

        //    Assert.AreEqual(100f, root_child0.LayoutWidth);

        //    Assert.AreEqual(0f, root_child0.LayoutHeight);



        //    Assert.AreEqual(0f, root_child0_child0.LayoutX);

        //    Assert.AreEqual(0f, root_child0_child0.LayoutY);

        //    Assert.AreEqual(100f, root_child0_child0.LayoutWidth);

        //    Assert.AreEqual(0f, root_child0_child0.LayoutHeight);

        //}



    }
}
