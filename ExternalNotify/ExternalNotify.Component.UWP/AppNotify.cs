using System.Diagnostics;
using Windows.Foundation.Metadata;

namespace ExternalNotify.Component
{
    public delegate void NotifyAppHandler(string aState, string aValue);

    /// <summary>
    /// Sample native object for injecting to the WebView.
    /// </summary>
    [AllowForWeb]
	public sealed class AppNotify
	{
		public event NotifyAppHandler NotifyAppEvent;

		public void notify(string aState, string aValue)
		{
            Debug.WriteLine($"Called from WebView! state={aState}\nvalue:\n{aValue}");
            OnNotifyAppEvent(aState, aValue);
        }

        private void OnNotifyAppEvent(string aState, string aValue)
        {
            NotifyAppEvent?.Invoke(aState, aValue);
        }
    }
}