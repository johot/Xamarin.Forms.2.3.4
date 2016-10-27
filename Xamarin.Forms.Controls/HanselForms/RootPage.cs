using System;
using System.Collections.Generic;

namespace Xamarin.Forms.Controls
{
	public enum HMenuType
	{
		About,
		Blog,
		Twitter,
		Hanselminutes,
		Ratchet,
		DeveloperLife
	}

	public class HomeMenuItem : BaseModel
	{
		public HomeMenuItem()
		{
			MenuType = HMenuType.About;
		}
		public string Icon { get; set; }
		public HMenuType MenuType { get; set; }
	}

	public class BaseModel
	{
		public string Title { get; set; }
		public string Details { get; set; }
		public int Id { get; set; }
	}

	public class RootPage : MasterDetailPage
	{
		public static bool IsUWPDesktop { get; set; }
		Dictionary<HMenuType, NavigationPage> Pages { get; set; }
		public RootPage()
		{
			Pages = new Dictionary<HMenuType, NavigationPage>();
			Master = new MenuPage1(this);

			BindingContext = new HBaseViewModel
			{
				Title = "Hanselman",
				Icon = "slideout.png"
			};
			Navigate(HMenuType.About);
		}

		public void Navigate(HMenuType id)
		{
			Page newPage;
			if (!Pages.ContainsKey(id))
			{
				switch (id)
				{
					case HMenuType.About:
						Pages.Add(id, new NavigationPage(new MyAbout()));
						break;
					case HMenuType.Blog:
						Pages.Add(id, new NavigationPage(new BlogPage()));
						break;
					case HMenuType.DeveloperLife:
						Pages.Add(id, new NavigationPage(new AboutPage() { Title = "Page 3" }));
						break;
					case HMenuType.Hanselminutes:
						Pages.Add(id, new NavigationPage(new AboutPage() { Title = "Page 4" }));
						break;
					case HMenuType.Ratchet:
						Pages.Add(id, new NavigationPage(new AboutPage() { Title = "Page 5" }));
						break;
					case HMenuType.Twitter:
						Pages.Add(id, new NavigationPage(new AboutPage() { Title = "Page 6" }));
						break;
				}
			}

			newPage = Pages[id];
			if (newPage == null)
				return;

			Detail = newPage;
		}
	}

	public class MenuPage1 : ContentPage
	{
		RootPage mdp;
		ListView ListViewMenu;
		List<HomeMenuItem> menuItems;
		public MenuPage1(RootPage page)
		{
			Title = "Master";
			mdp = page;
			ListViewMenu = new ListView { ItemTemplate = new DataTemplate(typeof(MenuViewCell)) };
			ListViewMenu.ItemsSource = menuItems = new List<HomeMenuItem>
				{
					new HomeMenuItem { Title = "About", MenuType = HMenuType.About, Icon ="about.png" },
					new HomeMenuItem { Title = "Blog", MenuType = HMenuType.Blog, Icon = "blog.png" },
					new HomeMenuItem { Title = "Twitter", MenuType = HMenuType.Twitter, Icon = "twitternav.png" },
					new HomeMenuItem { Title = "Hanselminues", MenuType = HMenuType.Hanselminutes, Icon = "hm.png" },
					new HomeMenuItem { Title = "Ratchet", MenuType = HMenuType.Ratchet, Icon = "ratchet.png" },
					new HomeMenuItem { Title = "Developers Life", MenuType = HMenuType.DeveloperLife, Icon = "tdl.png"},
				};

			ListViewMenu.SelectedItem = menuItems[0];

			ListViewMenu.ItemSelected += (sender, e) =>
			   {
				   if (ListViewMenu.SelectedItem == null)
					   return;

				   mdp.Navigate(((HomeMenuItem)e.SelectedItem).MenuType);
			   };
			Content = ListViewMenu;
		}
	}

	class MenuViewCell : ViewCell
	{
		public MenuViewCell()
		{
			var grd = new Grid { Padding = new Thickness(5) };
			grd.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
			grd.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
			var img = new Image { HeightRequest = 25, WidthRequest = 25 };
			img.SetBinding(Image.SourceProperty, nameof(HomeMenuItem.Icon));
			var lbl = new Label { FontSize = 24, VerticalOptions = LayoutOptions.Center };
			lbl.SetBinding(Label.TextProperty, nameof(HomeMenuItem.Title));
			Grid.SetColumn(lbl, 1);
			grd.Children.Add(img);
			grd.Children.Add(lbl);
			View = grd;
		}
	}
}
