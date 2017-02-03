#if WINDOWS_PHONE_APP
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace Xamarin.Forms.ControlGallery
{
	/// <summary>
	/// Provides application-specific behavior to supplement the default Application class.
	/// </summary>
	public sealed partial class App
	{
		TransitionCollection _transitions;

		/// <summary>
		/// Initializes the singleton application object.  This is the first line of authored code
		/// executed, and as such is the logical equivalent of main() or WinMain().
		/// </summary>
		public App()
		{
			InitializeComponent();
			Suspending += OnSuspending;
		}

		/// <summary>
		/// Invoked when the application is launched normally by the end user.  Other entry points
		/// will be used when the application is launched to open a specific file, to display
		/// search results, and so forth.
		/// </summary>
		/// <param name="e">Details about the launch request and process.</param>
		protected override void OnLaunched(LaunchActivatedEventArgs e)
		{
#if DEBUG
			if (System.Diagnostics.Debugger.IsAttached)
			{
				DebugSettings.EnableFrameRateCounter = true;
			}
#endif

			var rootFrame = Window.Current.Content as global::Windows.UI.Xaml.Controls.Frame;

			// Do not repeat app initialization when the Window already has content,
			// just ensure that the window is active
			if (rootFrame == null)
			{
				// Create a Frame to act as the navigation context and navigate to the first page
				rootFrame = new global::Windows.UI.Xaml.Controls.Frame();

				// TODO: change this value to a cache size that is appropriate for your application
				rootFrame.CacheSize = 1;

				Forms.Init (e);
				FormsMaps.Init (Controls.App.Config["WinPhoneMapsAuthKey"]);

				if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
				{
					// TODO: Load state from previously suspended application
				}

				// Place the frame in the current Window
				Window.Current.Content = rootFrame;
			}

			if (rootFrame.Content == null)
			{
				// Removes the turnstile navigation for startup.
				if (rootFrame.ContentTransitions != null)
				{
					_transitions = new TransitionCollection();
					foreach (var c in rootFrame.ContentTransitions)
					{
						_transitions.Add(c);
					}
				}

				rootFrame.ContentTransitions = null;
				rootFrame.Navigated += RootFrame_FirstNavigated;

				// When the navigation stack isn't restored navigate to the first page,
				// configuring the new page by passing required information as a navigation
				// parameter
				if (!rootFrame.Navigate(typeof(MainPage), e.Arguments))
				{
					throw new Exception("Failed to create initial page");
				}
			}

			// Ensure the current window is active
			Window.Current.Activate();
		}

		/// <summary>
		/// Restores the content transitions after the app has launched.
		/// </summary>
		/// <param name="sender">The object where the handler is attached.</param>
		/// <param name="e">Details about the navigation event.</param>
		void RootFrame_FirstNavigated(object sender, global::Windows.UI.Xaml.Navigation.NavigationEventArgs e)
		{
			var rootFrame = sender as global::Windows.UI.Xaml.Controls.Frame;
			rootFrame.ContentTransitions = _transitions ?? new TransitionCollection() { new NavigationThemeTransition() };
			rootFrame.Navigated -= RootFrame_FirstNavigated;
		}

		/// <summary>
		/// Invoked when application execution is being suspended.  Application state is saved
		/// without knowing whether the application will be terminated or resumed with the contents
		/// of memory still intact.
		/// </summary>
		/// <param name="sender">The source of the suspend request.</param>
		/// <param name="e">Details about the suspend request.</param>
		void OnSuspending(object sender, SuspendingEventArgs e)
		{
			var deferral = e.SuspendingOperation.GetDeferral();

			// TODO: Save application state and stop any background activity
			deferral.Complete();
		}
	}
}
#endif
﻿#if WINDOWS_UWP
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=402347&clcid=0x409
namespace Xamarin.Forms.ControlGallery
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            InitializeComponent();
            Suspending += OnSuspending;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {

#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
             //   DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            var rootFrame = Window.Current.Content as Windows.UI.Xaml.Controls.Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Windows.UI.Xaml.Controls.Frame();
                // Set the default language
                rootFrame.Language = Windows.Globalization.ApplicationLanguages.Languages[0];

                rootFrame.NavigationFailed += OnNavigationFailed;

				Forms.Init (e);
				FormsMaps.Init (Controls.App.Config["UWPMapsAuthKey"]);

				// Place the frame in the current Window
				Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                rootFrame.Navigate(typeof(MainPage), e.Arguments);
            }

			//// Uncomment to test overriding the status bar color
			//if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
			//{
			//	var statusBar = StatusBar.GetForCurrentView();
			//	if (statusBar != null)
			//	{
			//		statusBar.BackgroundOpacity = 1;
			//		statusBar.BackgroundColor = Colors.Black;
			//		statusBar.ForegroundColor = Colors.White;
			//	}
			//}

			// Ensure the current window is active
			Window.Current.Activate();
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}
#endif
﻿#if WINDOWS_APP
using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

namespace Xamarin.Forms.ControlGallery
{
	public sealed partial class App
	{
		public App()
		{
			InitializeComponent();
		}

		protected override void OnLaunched(LaunchActivatedEventArgs e)
		{

#if DEBUG
			if (System.Diagnostics.Debugger.IsAttached)
			{
				DebugSettings.EnableFrameRateCounter = true;
			}
#endif

			var rootFrame = Window.Current.Content as global::Windows.UI.Xaml.Controls.Frame;

			// Do not repeat app initialization when the Window already has content,
			// just ensure that the window is active
			if (rootFrame == null)
			{
				// Create a Frame to act as the navigation context and navigate to the first page
				rootFrame = new global::Windows.UI.Xaml.Controls.Frame();
				// Set the default language
				rootFrame.Language = global::Windows.Globalization.ApplicationLanguages.Languages[0];

				rootFrame.NavigationFailed += OnNavigationFailed;

				Forms.Init (e);
				FormsMaps.Init (Controls.App.Config["Win8MapsAuthKey"]);

				// Place the frame in the current Window
				Window.Current.Content = rootFrame;
			}

			if (rootFrame.Content == null)
			{
				// When the navigation stack isn't restored navigate to the first page,
				// configuring the new page by passing required information as a navigation
				// parameter
				rootFrame.Navigate(typeof(MainPage), e.Arguments);
			}
			// Ensure the current window is active
			Window.Current.Activate();
		}

		/// <summary>
		/// Invoked when Navigation to a certain page fails
		/// </summary>
		/// <param name="sender">The Frame which failed navigation</param>
		/// <param name="e">Details about the navigation failure</param>
		void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
		{
			throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
		}
	}
}
#endif
