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
		}

		void Button0_OnClicked(object sender, EventArgs e)
		{
			Entry0.IsEnabled = !Entry0.IsEnabled;
		}

		void Button1_OnClicked(object sender, EventArgs e)
		{
			Entry1.IsEnabled = !Entry1.IsEnabled;
		}

		void Button2_OnClicked(object sender, EventArgs e)
		{
			Entry2.IsEnabled = !Entry2.IsEnabled;
		}
	}
}
