using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.ApplicationModel.Store;
using Shiftv.Contracts.Services;

namespace Shiftv.Common
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        private bool _errorGettingData;
        private bool _noDataAvailable;
        private bool _isBuyPageVisible;
        private RelayCommand _showBuyPage;
        private RelayCommand _hideBuyPage;
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (object.Equals(storage, value)) return false;

            storage = value;
            this.OnPropertyChanged(propertyName);

            return true;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            var eventHandler = this.PropertyChanged;
            if (eventHandler != null)
            {
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public bool IsUserLogged
        {
            get
            {
                var user = CoreServices.User.GetCurrentUser();
                return user != null;
            }
        }



        public bool ErrorGettingData
        {
            get { return _errorGettingData; }
            set { SetProperty(ref _errorGettingData, value); }
        }
        public bool NoDataAvailable
        {
            get { return _noDataAvailable; }
            set { SetProperty(ref _noDataAvailable, value); }
        }

        public bool IsToShowAds
        {
            get
            {
                var inAppPurchase = CurrentApp.LicenseInformation;
                if (inAppPurchase != null && (inAppPurchase.ProductLicenses["NoAds"].IsActive || inAppPurchase.ProductLicenses["NoAds2"].IsActive || inAppPurchase.ProductLicenses["NoAds3"].IsActive))
                {
                    return false;
                }
                return true;
            }
        }

        public bool IsBuyPageVisible
        {
            get { return _isBuyPageVisible; }
            set { SetProperty(ref _isBuyPageVisible, value); }
        }

        public RelayCommand ShowBuyPage
        {
            get { return _showBuyPage ?? (_showBuyPage = new RelayCommand(ShowBuyPageAction)); }
        }

        public void ShowBuyPageAction()
        {
            IsBuyPageVisible = true;
            OnPropertyChanged("IsBuyPageVisible");
        } 
        
        public RelayCommand HideBuyPage
        {
            get { return _hideBuyPage ?? (_hideBuyPage = new RelayCommand(HideBuyPageAction)); }
        }

        private void HideBuyPageAction()
        {
            IsBuyPageVisible = false;
        }
    }
}
