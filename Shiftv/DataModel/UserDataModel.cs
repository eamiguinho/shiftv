using Shiftv.Common;
using Shiftv.Contracts.Domain.Comments;
using Shiftv.Contracts.Domain.Users;
using Shiftv.Contracts.Services;

namespace Shiftv.DataModel
{
    public class UserDataModel : ViewModelBase
    {
        private bool _isGold;
        private bool _isSilver;

        public UserDataModel(IUser user)
        {
            Username = user.Username;
            Avatar = user.Images.Avatar.Full;
            Model = user;
            LoadStatus(user);
        }

        public IUser Model { get; set; }

        private async void LoadStatus(IUser user)
        {
            var status = await CoreServices.User.GetUserStats(user.Username);
            if (status.IsOk && status.Data != null)
            {
                IsGold = status.Data.IsGold;
                IsSilver = status.Data.IsSilver;
            }
        }

        public string Avatar { get; set; }

        public string Username { get; set; }

        public bool IsGold
        {
            get { return _isGold; }
            set { SetProperty(ref _isGold, value); }
        }

        public bool IsSilver
        {
            get { return _isSilver; }
            set { SetProperty(ref _isSilver, value); }
        }

        public string FullName { get { return Model.Name; } }
        public string Location { get { return Model.Location; } }
        public string Gender { get { return Model.Gender; } }
        public string About { get { return Model.About; } }
        public bool IsVip { get { return Model.Vip; } }

        public bool IsMale
        {
            get { return Model.Gender.ToLower() == "male"; }
        }

        public bool IsFemale { get { return Model.Gender.ToLower() == "female"; } }
        public bool IsProtected { get { return Model.Private; } }
    }

    public class NewUserDataModel : ViewModelBase
    {
        private bool _isGold;
        private bool _isSilver;

        public NewUserDataModel(IUser user)
        {
            Username = user.Username;
            Avatar = user.Images != null ? user.Images.Avatar.Full : null;
           // LoadStatus(user);
        }

        //private async void LoadStatus(IUser user)
        //{
        //    var status = await CoreServices.User.GetUserStats(user.Username);
        //    if (status.IsOk && status.Data != null)
        //    {
        //        IsGold = status.Data.IsGold;
        //        IsSilver = status.Data.IsSilver;
        //    }
        //}

        public string Avatar { get; set; }

        public string Username { get; set; }

        public bool IsGold
        {
            get { return _isGold; }
            set { SetProperty(ref _isGold, value); }
        }

        public bool IsSilver
        {
            get { return _isSilver; }
            set { SetProperty(ref _isSilver, value); }
        }
    }
}