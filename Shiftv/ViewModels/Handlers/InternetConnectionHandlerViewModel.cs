using System;
using Windows.ApplicationModel;
using Windows.Networking.Connectivity;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Shiftv.Common;
using Shiftv.Contracts.Services;

namespace Shiftv.ViewModels.Handlers
{
    public class InternetConnectionHandlerViewModel : ViewModelBase, IDisposable
    {
        private bool _isInternetErrorVisible;
        private bool _isApiErrorConnection;
        private readonly CoreDispatcher _coreDispatcher;
        private DispatcherTimer _timer1;

        public InternetConnectionHandlerViewModel()
        {
            if (DesignMode.DesignModeEnabled)
            {
                IsInternetErrorVisible = false;
                IsApiErrorConnection = true;
            }
            else
            {
                _coreDispatcher = Window.Current.CoreWindow.Dispatcher;
                NetworkInformation.NetworkStatusChanged += NetworkInformation_NetworkStatusChanged; // Listen to connectivity changes
                _timer1 = new DispatcherTimer { Interval = TimeSpan.FromSeconds(30) };
                _timer1.Tick += timer_Tick;
                _timer1.Start();
                CheckPing();
            }
        }

        public void Dispose()
        {
            _timer1.Stop();
            _timer1 = null;
        }

        public bool IsInternetErrorVisible
        {
            get { return _isInternetErrorVisible; }
            set { SetProperty(ref _isInternetErrorVisible, value); }
        }

        public bool IsApiErrorConnection
        {
            get { return _isApiErrorConnection; }
            set { SetProperty(ref _isApiErrorConnection, value); }
        }


        private void timer_Tick(object sender, object e)
        {
            CheckPing();
        }


        public static bool IsConnectedToInternet()
        {
            var connectionProfile = NetworkInformation.GetInternetConnectionProfile();
            return (connectionProfile != null && connectionProfile.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess);
        }

        private async void NetworkInformation_NetworkStatusChanged(object sender)
        {
            var profile = IsConnectedToInternet();
            if (profile == false)
            {
                await _coreDispatcher.RunAsync(
                    CoreDispatcherPriority.Normal,
                    () =>
                    {
                        IsInternetErrorVisible = true;
                    });
            }
            else
            {
                await _coreDispatcher.RunAsync(
                    CoreDispatcherPriority.Normal, () =>
                    {
                        IsInternetErrorVisible = false;
                    });
            }
        }

        private async void CheckPing()
        {
            if (IsInternetErrorVisible) return;
            var ping = await CoreServices.Stats.PingServer();
            IsApiErrorConnection = !ping.IsOk;
        }
    }
}
