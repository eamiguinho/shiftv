using System.Collections.ObjectModel;
using System.Linq;
using Windows.Storage;
using Shiftv.Common;
using Shiftv.Contracts.Services;
using Shiftv.DataModel;

namespace Shiftv.ViewModels.Settings
{
    class ShiftvSettingsViewModel : ViewModelBase
    {
        private ObservableCollection<SubtitleLanguageDataModel> _primaryLanguages;
        private ObservableCollection<SubtitleLanguageDataModel> _secondaryLanguages;
        private SubtitleLanguageDataModel _selectedPrimaryLanguage;
        private SubtitleLanguageDataModel _selectedSecondaryLanguage;

        public ShiftvSettingsViewModel()
        {
            var lan = CoreServices.Episode.GetListSubtitlesLanguage();
            foreach (var subtitlesLanguage in lan)
            {
                PrimaryLanguages.Add(new SubtitleLanguageDataModel(subtitlesLanguage));
                SecondaryLanguages.Add(new SubtitleLanguageDataModel(subtitlesLanguage));
            }
            var localSettings = ApplicationData.Current.LocalSettings;
            if (localSettings.Values["PrimaryLanguageSubtitles"] != null)
            {
                SelectedPrimaryLanguage =
                    PrimaryLanguages.FirstOrDefault(
                        x => x.LanguageId == localSettings.Values["PrimaryLanguageSubtitles"].ToString());
            }
            else
            {
                SelectedPrimaryLanguage = PrimaryLanguages.FirstOrDefault(
                           x => x.Language == "Disabled");
            }

            if (localSettings.Values["SecondaryLanguageSubtitles"] != null)
            {
                SelectedSecondaryLanguage =     SecondaryLanguages.FirstOrDefault(
                        x => x.LanguageId == localSettings.Values["SecondaryLanguageSubtitles"].ToString());
            }
            else
            {
                SelectedSecondaryLanguage = SecondaryLanguages.FirstOrDefault(
                    x => x.Language == "Disabled");
            }
        }

        public SubtitleLanguageDataModel SelectedSecondaryLanguage
        {
            get { return _selectedSecondaryLanguage; }
            set
            {
                SetProperty(ref _selectedSecondaryLanguage, value);
                if (value != null)
                {
                    var localSettings = ApplicationData.Current.LocalSettings;
                    localSettings.Values["SecondaryLanguageSubtitles"] = value.LanguageId;
                }
            }
        }

        public SubtitleLanguageDataModel SelectedPrimaryLanguage
        {
            get { return _selectedPrimaryLanguage; }
            set
            {
                SetProperty(ref _selectedPrimaryLanguage, value);
                if (value != null)
                {
                    var localSettings = ApplicationData.Current.LocalSettings;
                    localSettings.Values["PrimaryLanguageSubtitles"] = value.LanguageId;
                }
            }
        }


        public ObservableCollection<SubtitleLanguageDataModel> PrimaryLanguages
        {
            get { return _primaryLanguages ?? (_primaryLanguages = new ObservableCollection<SubtitleLanguageDataModel>()); }
        }

        public ObservableCollection<SubtitleLanguageDataModel> SecondaryLanguages
        {
            get { return _secondaryLanguages ?? (_secondaryLanguages = new ObservableCollection<SubtitleLanguageDataModel>()); }
        }

        public bool AutoCheckIn
        {
            get {  
                var localSettings = ApplicationData.Current.LocalSettings;
                if (localSettings.Values["AutoCheckIn"] == null)
                    return false;
                bool isAutoCheckInOn;
                var b = bool.TryParse(localSettings.Values["AutoCheckIn"].ToString(), out  isAutoCheckInOn);
                return b && isAutoCheckInOn;
            }
            set
            { 
                var localSettings = ApplicationData.Current.LocalSettings;
                localSettings.Values["AutoCheckIn"] = value;
            }
        }
    }
}
