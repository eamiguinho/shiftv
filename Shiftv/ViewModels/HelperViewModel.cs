using System;
using System.Collections.Generic;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml.Media.Imaging;
using GalaSoft.MvvmLight.Messaging;
using Shiftv.Common;
using Shiftv.Contracts.Domain.Results;
using Shiftv.Contracts.Services;
using Shiftv.Global;
using Shiftv.Helpers;

namespace Shiftv.ViewModels
{
    public class HelperViewModel : ViewModelBase
    {
        private RelayCommand _loginPressed;
        private bool _isLoading;
        private bool _errorMessage;
        private string _welcome;
        private BitmapImage _image1;
        private BitmapImage _image4;
        private BitmapImage _image3;
        private BitmapImage _image2;
        private BitmapImage _image5;
        public Random A = new Random(DateTime.Now.Ticks.GetHashCode());
        public List<int> RandomList = new List<int>();
        private string _createUsername;
        private string _createPassword;
        private string _createEmail;
        private RelayCommand _createPressed;
        private bool _createErrorMessage;
        private string _createErrorMessageText;
        private bool _canImageShow;
        private bool _canImageMovie;
        private RelayCommand _enterWithoutLogin;
        private string _errorMessageLogin;


        public HelperViewModel()
        {
            LoadImages();
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            localSettings.Values["firstTime"] = true;
        }

        private async void LoadImages()
        {
            Image5 = await ImageHelper.GetShowImageInCache();
            Image4 = await ImageHelper.GetMovieImageInCache();
            var showsReq = await CoreServices.Show.GetTrending();
            var moviesReq = await CoreServices.Movie.GetTrending();
            _canImageShow = showsReq.Result == StandardResults.Ok && showsReq.Data != null && showsReq.Data.Count > 10;
            _canImageMovie = moviesReq.Result == StandardResults.Ok && moviesReq.Data != null && moviesReq.Data.Count > 10;
            RandomList.Clear();
            if (_canImageShow) Image1 = await ImageHelper.GetShowImageAsync(new Uri(showsReq.Data[NewNumber()].Fanart.Full));
            if (_canImageShow) Image3 = await ImageHelper.GetShowImageAsync(new Uri(showsReq.Data[NewNumber()].Fanart.Full));
            if (Image5 == null && _canImageShow) Image5 = await ImageHelper.GetShowImageAsync(new Uri(showsReq.Data[NewNumber()].Fanart.Full));
            RandomList.Clear();
            if (_canImageMovie) Image2 = await ImageHelper.GetMovieImageAsync(new Uri(moviesReq.Data[NewNumber()].Fanart.Full));
            if (Image4 == null && _canImageMovie) Image4 = await ImageHelper.GetMovieImageAsync(new Uri(moviesReq.Data[NewNumber()].Fanart.Full));
        }



        private int NewNumber()
        {
            var mynumber = A.Next(0, 10);
            while (RandomList.Contains(mynumber))
            {
                mynumber = A.Next(0, 10);
            }
            RandomList.Add(mynumber);
            return mynumber;
        }


    
     
      

        public BitmapImage Image1
        {
            get { return _image1; }
            set { SetProperty(ref _image1, value); }
        }
        public BitmapImage Image2
        {
            get { return _image2; }
            set { SetProperty(ref _image2, value); }
        }
        public BitmapImage Image3
        {
            get { return _image3; }
            set { SetProperty(ref _image3, value); }
        }
        public BitmapImage Image4
        {
            get { return _image4; }
            set { SetProperty(ref _image4, value); }
        }
        public BitmapImage Image5
        {
            get { return _image5; }
            set { SetProperty(ref _image5, value); }
        }



    }
}
