using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Xamarin.Forms.FlexLayoutTests
{
    [TestFixture]
    public class FlexLayoutTests : BaseTestFixture
    {
        [SetUp]
        public override void Setup()
        {
            base.Setup();
         //   Device.PlatformServices = new MockPlatformServices();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
            //Device.PlatformServices = null;
        }

        [Test]
        public void ThrowsOnInitWithNoEngine()
        {
            Assert.Throws<InvalidOperationException>(() => new FlexLayout());
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