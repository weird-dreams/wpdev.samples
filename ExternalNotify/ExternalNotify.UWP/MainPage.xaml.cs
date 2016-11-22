using System;
using System.Diagnostics;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ExternalNotify.Component;

namespace ExternalNotify
{
	public sealed partial class MainPage
	{
		public MainPage()
		{
			InitializeComponent();

        }

		private void Web_OnNavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
		{
			// WebView native object must be inserted in the OnNavigationStarting event handler
			AppNotify appNotify = new AppNotify();
		    appNotify.NotifyAppEvent += OnNotified;
			// Expose the native WinRT object on the page's global object
			Web.AddWebAllowedObject("AppNotify", appNotify);
 }

	    private async void OnNotified(string aState, string aValue)
	    {
	        await new MessageDialog($"Content:\n{aValue}", $"Notified by WebView with state '{aState}'").ShowAsync();
	    }

	    private async void Web_OnDOMContentLoaded(WebView sender, WebViewDOMContentLoadedEventArgs args)
		{
			//try
			//{
			//	// inject event handler to arbitrary page once the DOM is loaded
			//	// in this case add event handlet to click on the main <table> element
			//	await Web.InvokeScriptAsync("eval",
			//		new[] { "document.getElementById(\"hnmain\").addEventListener(\"click\", function () { NotifyApp.setKeyCombination(43); }); "});
			//		//new[] { "document.getElementById(\"hnmain\").addEventListener(\"click\", function () { window.external.notify(\"43\"); }); " });
			//}
			//catch (Exception e)
			//{
			//	Debug.WriteLine(e);
			//}
		}

        private void Web_OnScriptNotify(object sender, NotifyEventArgs e)
        {
            Debug.WriteLine("Called from ScriptNotify! {0}", new[] { e.Value });
        }

	    private void OnNavigate(object aSender, RoutedEventArgs aE)
	    {
            mProgressRing.IsActive = true;
            mProgressRing.Visibility = Visibility.Visible;
            mNavigateButton.IsEnabled = false;

            Web.Navigate(new Uri(mUrl.Text));
        }

	    private void Web_OnNavigationCompleted(WebView aSender, WebViewNavigationCompletedEventArgs aArgs)
	    {
            mProgressRing.Visibility = Visibility.Collapsed;
            mNavigateButton.IsEnabled = true;
        }

	    private void Web_OnNavigationFailed(object aSender, WebViewNavigationFailedEventArgs aE)
	    {
            mProgressRing.Visibility = Visibility.Collapsed;
            mNavigateButton.IsEnabled = true;
        }

	    private void Web_OnContentLoading(WebView aSender, WebViewContentLoadingEventArgs aArgs)
	    {
            mProgressRing.IsActive = true;
            mProgressRing.Visibility = Visibility.Visible;
            mNavigateButton.IsEnabled = false;
        }

	    private void Web_OnFrameNavigationCompleted(WebView aSender, WebViewNavigationCompletedEventArgs aArgs)
	    {
	        mError.Text = aArgs.IsSuccess ? "" : aArgs.WebErrorStatus.ToString();
	    }
	}
}