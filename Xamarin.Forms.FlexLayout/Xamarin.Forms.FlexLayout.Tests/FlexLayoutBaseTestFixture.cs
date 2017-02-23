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
    public class FlexLayoutBaseTestFixture : BaseTestFixture
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
    }
}
