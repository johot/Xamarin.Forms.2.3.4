using NUnit.Framework;

namespace Xamarin.Forms.Core.UnitTests
{
	[TestFixture]
	public class InheritableBindablePropertyTests
	{
		[Test]
		public void BasicInheritancePostAdd()
		{
			var layout = new StackLayout();
			var child = new Label();

			layout.Children.Add(child);
			layout.FlowDirection = FlowDirection.RightToLeft;

			Assert.AreEqual(FlowDirection.RightToLeft, child.FlowDirection);
		}

		[Test]
		public void BasicInheritancePreAdd()
		{
			var layout = new StackLayout();
			var child = new Label();

			layout.FlowDirection = FlowDirection.RightToLeft;
			layout.Children.Add(child);

			Assert.AreEqual(FlowDirection.RightToLeft, child.FlowDirection);
		}

		[Test]
		public void ManualSetBlocks()
		{
			var layout = new StackLayout();
			var innerLayout = new StackLayout();
			var child = new Label();

			layout.Children.Add(innerLayout);
			innerLayout.Children.Add(child);

			layout.FlowDirection = FlowDirection.RightToLeft;
			child.FlowDirection = FlowDirection.LeftToRight;

			Assert.AreEqual(FlowDirection.LeftToRight, child.FlowDirection);
		}

		[Test]
		public void ManualSetBlocksPost()
		{
			var layout = new StackLayout();
			var innerLayout = new StackLayout();
			var child = new Label();

			layout.FlowDirection = FlowDirection.RightToLeft;
			child.FlowDirection = FlowDirection.LeftToRight;

			layout.Children.Add(innerLayout);
			innerLayout.Children.Add(child);

			Assert.AreEqual(FlowDirection.LeftToRight, child.FlowDirection);
		}

	}
}