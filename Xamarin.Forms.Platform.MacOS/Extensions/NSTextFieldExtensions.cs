using System;
using AppKit;
using CoreGraphics;

namespace Xamarin.Forms.Platform.MacOS
{
	internal static class NSTextFieldExtensions
	{
		public static NSTextField CreateLabel(string text)
		{
			var textField = new NSTextField();
			textField.StringValue = text;
			textField.DrawsBackground = false;
			textField.Editable = false;
			textField.Bezeled = false;
			textField.Selectable = false;
			textField.SizeToFit();
			textField.CenterTextVertically();
			return textField;
		}

		public static NSTextFieldCell CreateLabelCentered(string text)
		{
			var textField = new VerticallyCenteredTextFieldCell(0);
			textField.StringValue = text;
			textField.DrawsBackground = false;
			textField.Editable = false;
			textField.Bezeled = false;
			textField.Selectable = false;
			return textField;
		}

		public static void CenterTextVertically(this NSTextField self)
		{
			self.CenterTextVertically(self.Frame);
		}

		public static void CenterTextVertically(this NSTextField self, CGRect frame)
		{
			var stringHeight = self.Cell.AttributedStringValue.Size.Height;
			var titleRect = self.Cell.TitleRectForBounds(frame);
			var newTitleRect = new CGRect(titleRect.X, frame.Y + (frame.Height - stringHeight) / 2.0, titleRect.Width, stringHeight);
			self.Frame = newTitleRect;
		}
	}

	class VerticallyCenteredTextFieldCell : NSTextFieldCell
	{
		nfloat offset;
		public VerticallyCenteredTextFieldCell(nfloat yOffset)
		{
			offset = yOffset;
		}

		public override CGRect DrawingRectForBounds(CGRect theRect)
		{
			// Get the parent's idea of where we should draw.
			CGRect newRect = base.DrawingRectForBounds(theRect);

			// Ideal size for the text.
			CGSize textSize = CellSizeForBounds(theRect);

			// Center in the rect.
			nfloat heightDelta = newRect.Size.Height - textSize.Height;
			if (heightDelta > 0)
			{
				newRect.Size = new CGSize(newRect.Width, newRect.Height - heightDelta);
				newRect.Location = new CGPoint(newRect.X, newRect.Y + heightDelta / 2 + offset);
			}
			return newRect;
		}
	}
}

