using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Media.Imaging;
using Shiftv.Common;
using Shiftv.Contracts.Services;
using Shiftv.DataModel;
using Shiftv.Global;
using Shiftv.Helpers;

namespace Shiftv.ViewModels
{
    public class ChooseSectionViewModel : ViewModelBase
    {
        private BitmapImage _imageShow1;
        private BitmapImage _imageShow2;
        private BitmapImage _imageShow3;
        private BitmapImage _imageShow4;
        private BitmapImage _imageShow5;
        private BitmapImage _imageMovie1;
        private BitmapImage _imageMovie2;
        private BitmapImage _imageMovie3;
        private BitmapImage _imageMovie4;
        private BitmapImage _imageMovie5;

        public ChooseSectionViewModel()
        {
            LoadImages();
        }

        private async void LoadImages()
        {
            ImageShow5 = await ImageHelper.GetShowImageInCache();
            ImageMovie5 =await ImageHelper.GetMovieImageInCache();
            var user = CoreServices.User.GetCurrentUser();
            if (user != null)
            {
                LoggedUser = new UserDataModel(user.UserSettings.User);
                UserAvatar = await ImageHelper.GetOtherImageAsync(new Uri(user.UserSettings.User.Images.Avatar.Full));
            }
            var res = await CoreServices.Show.GetTrending();
            var shows = res.Data;
            var moviesReq = await CoreServices.Movie.GetTrending();
            var movies = moviesReq.Data;
            _canLoadShows = res.Result == StandardResults.Ok && shows != null && shows.Count > 0;
            _canLoadMovies = moviesReq.Result == StandardResults.Ok && movies != null && movies.Count > 0;

            RandomList.Clear();
            RandomList2.Clear();

            if (_canLoadShows && ImageShow5 == null)
            {
                try
                {
                    ImageShow5 = await ImageHelper.GetShowImageAsync(new Uri(shows[NewNumber(RandomList)].Fanart.Full));

                }
                catch (Exception)
                {

                }
            }
            if (_canLoadMovies && ImageMovie5 == null)
            {
                try
                {
                    ImageMovie5 = await ImageHelper.GetMovieImageAsync(new Uri(movies[NewNumber(RandomList2)].Fanart.Full));

                }
                catch (Exception)
                {

                }
            }
            if (_canLoadShows)
            {
                try
                {
                    ImageShow4 = await ImageHelper.GetShowImageAsync(new Uri(shows[NewNumber(RandomList)].Fanart.Full));

                }
                catch (Exception)
                {

                }
            }
            if (_canLoadMovies)
            {
                try
                {
                    ImageMovie4 = await ImageHelper.GetMovieImageAsync(new Uri(movies[NewNumber(RandomList2)].Fanart.Full));
                }
                catch (Exception)
                {
                    
                }
            }
            if (_canLoadShows)
            {
                try
                {
                    ImageShow3 = await ImageHelper.GetShowImageAsync(new Uri(shows[NewNumber(RandomList)].Fanart.Full));

                }
                catch (Exception)
                {

                }
            }
            if (_canLoadMovies)
            {
                try
                {
                    ImageMovie3 = await ImageHelper.GetMovieImageAsync(new Uri(movies[NewNumber(RandomList2)].Fanart.Full));

                }
                catch (Exception)
                {

                }
            }
            if (_canLoadShows)
            {
                try
                {
                    ImageShow2 = await ImageHelper.GetShowImageAsync(new Uri(shows[NewNumber(RandomList)].Fanart.Full));

                }
                catch (Exception)
                {

                }
            }
            if (_canLoadMovies)
            {
                try
                {
                    ImageMovie2 = await ImageHelper.GetMovieImageAsync(new Uri(movies[NewNumber(RandomList2)].Fanart.Full));
                }
                catch (Exception)
                {
                    
                    throw;
                }
            }
            if (_canLoadShows)
            {
                try
                {
                    ImageShow1 = await ImageHelper.GetShowImageAsync(new Uri(shows[NewNumber(RandomList)].Fanart.Full));

                }
                catch (Exception)
                {

                }
            }
            if (_canLoadMovies)
            {
                try
                {
                    ImageMovie1 = await ImageHelper.GetMovieImageAsync(new Uri(movies[NewNumber(RandomList2)].Fanart.Full));

                }
                catch (Exception)
                {

                }
            }
        }

        public UserDataModel LoggedUser
        {
            get { return _loggedUser; }
            set { SetProperty(ref _loggedUser, value); }
        }


        public Random A = new Random(DateTime.Now.Ticks.GetHashCode());
        public List<int> RandomList = new List<int>();
        public List<int> RandomList2 = new List<int>();
        private BitmapImage _userAvatar;
        private bool _isLoadingData;
        private bool _canLoadShows;
        private bool _canLoadMovies;
        private UserDataModel _loggedUser;

        private int NewNumber(List<int> list)
        {
            var mynumber = A.Next(0, 15);
            while (RandomList.Contains(mynumber))
            {
                mynumber = A.Next(0, 15);
            }
            list.Add(mynumber);
            return mynumber;
        }


        public BitmapImage ImageShow1
        {
            get { return _imageShow1; }
            set { SetProperty(ref _imageShow1, value); }
        }
        public BitmapImage ImageShow2
        {
            get { return _imageShow2; }
            set { SetProperty(ref _imageShow2, value); }
        }
        public BitmapImage ImageShow3
        {
            get { return _imageShow3; }
            set { SetProperty(ref _imageShow3, value); }
        }

        public BitmapImage ImageShow4
        {
            get { return _imageShow4; }
            set { SetProperty(ref _imageShow4, value); }
        }
        public BitmapImage ImageShow5
        {
            get { return _imageShow5; }
            set { SetProperty(ref _imageShow5, value); }
        }

        public BitmapImage ImageMovie1
        {
            get { return _imageMovie1; }
            set { SetProperty(ref _imageMovie1, value); }
        }
        public BitmapImage ImageMovie2
        {
            get { return _imageMovie2; }
            set { SetProperty(ref _imageMovie2, value); }
        }
        public BitmapImage ImageMovie3
        {
            get { return _imageMovie3; }
            set { SetProperty(ref _imageMovie3, value); }
        }
        public BitmapImage ImageMovie4
        {
            get { return _imageMovie4; }
            set { SetProperty(ref _imageMovie4, value); }
        }
        public BitmapImage ImageMovie5
        {
            get { return _imageMovie5; }
            set { SetProperty(ref _imageMovie5, value); }
        }

        public BitmapImage UserAvatar
        {
            get { return _userAvatar; }
            set { SetProperty(ref _userAvatar, value); }
        }

        public bool IsLoadingData
        {
            get { return _isLoadingData; }
            set { SetProperty(ref _isLoadingData, value); }
        }
    }
}
