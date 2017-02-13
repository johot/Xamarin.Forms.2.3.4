using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Xamarin.Forms.Controls.GalleryPages.VisualStateManagerGalleries
{
	public partial class EntryDisabledStatesGallery : ContentPage
	{
		public EntryDisabledStatesGallery()
		{
			InitializeComponent();

			Button0.Text = $"Toggle IsEnabled (Currently {Entry0.IsEnabled})";
			Button1.Text = $"Toggle IsEnabled (Currently {Entry1.IsEnabled})";
			Button2.Text = $"Toggle IsEnabled (Currently {Entry2.IsEnabled})";
		}

		void Button0_OnClicked(object sender, EventArgs e)
		{
			var button = sender as Button;
			ToggleIsEnabled(Entry0, button);
		}

		void Button1_OnClicked(object sender, EventArgs e)
		{
			var button = sender as Button;
			ToggleIsEnabled(Entry1, button);
		}

		void Button2_OnClicked(object sender, EventArgs e)
		{
			var button = sender as Button;
			ToggleIsEnabled(Entry2, button);
		}

		void ToggleIsEnabled(Entry entry, Button button)
		{
			entry.IsEnabled = !entry.IsEnabled;
			
			if (button != null)
			{
				button.Text = $"Toggle IsEnabled (Currently {entry.IsEnabled})";
			}
		}
	}
}
