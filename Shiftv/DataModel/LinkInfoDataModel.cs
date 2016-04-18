using Autofac;
using Shiftv.Common;
using Shiftv.Contracts.Domain.Shows;
using Shiftv.Global;
using Shiftv.Helpers;

namespace Shiftv.DataModel
{
    public class LinkInfoDataModel : ViewModelBase
    {
        private ILinkInfo _model;
        private int _streamN;
        private bool _isPlayingNow;

        public LinkInfoDataModel(ILinkInfo linkInfo, int streamN)
        {
            _model = linkInfo;
            _streamN = streamN;
        }

        public LinkInfoDataModel()
        {
            var x = Ioc.Container.Resolve<ILinkInfo>();
            x.Quality = StreamQuality.HD;
            x.FileSizeFormatted = "400 MB";
            _model = x;
        }

        public string FullNameLink
        {
            get { return string.Format("{2} {0} ({1})", _streamN, _model.FileSizeFormatted , ShiftvHelpers.GetTranslation("Stream_Upper")); }
        }

        public ILinkInfo Model
        {
            get { return _model; }
        }

        public bool IsHD { get { return _model.Quality == StreamQuality.HD; } }
        public bool IsMD { get { return _model.Quality == StreamQuality.MD; } }
        public bool IsSD { get { return _model.Quality == StreamQuality.SD; } }

        public string FileSize
        {
            get { return _model.FileSizeFormatted; }
        }

        public bool IsPlayingNow
        {
            get { return _isPlayingNow; }
            set { SetProperty(ref _isPlayingNow, value); }
        }

        public string Quality
        {
            get { return string.Format("{0}", _model.Quality); }
        }  
        
        public string Speed
        {
            get { return string.Format("{0} / {1}", _model.Velocity.ToString().ToLower(), StreamName); }
        }

        public string StreamName
        {
            get
            {
                var host = new System.Uri(string.IsNullOrEmpty(_model.EmbbedLink) ? _model.StreamLink : _model.EmbbedLink).Host;
                var domain = host.Substring(host.LastIndexOf('.', host.LastIndexOf('.') - 1) + 1);
                return domain;
            }
        }
    }
}