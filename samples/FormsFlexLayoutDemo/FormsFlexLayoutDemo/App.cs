using Xamarin.Forms;


namespace FormsFlexLayoutDemo
{
    public class App : Application
    {
        public App()
        {
            //MainPage = new MasterDetailPage
            //{
            //    Master = new FlexOptionsPage(),
            //    Detail = new NavigationPage(new FlexDemoPage()),
            //    BindingContext = new FlexLayoutViewModel()
            //};
            var layout = new FlexLayout();

            var label1 = new Label { Text = "Label 2" };
            var label2 = new Label { Text = "Label 1" };

            layout.Children.Add(label1);
            layout.Children.Add(label2);

            MainPage = new ContentPage { Content = layout };
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
