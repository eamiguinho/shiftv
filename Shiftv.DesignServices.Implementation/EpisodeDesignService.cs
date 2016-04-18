using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Newtonsoft.Json;
using Shiftv.Contracts.Data.Factories;
using Shiftv.Contracts.Data.Shows;
using Shiftv.Contracts.Domain.Results;
using Shiftv.Contracts.Domain.Shows;
using Shiftv.Contracts.Domain.Users;
using Shiftv.Contracts.Services.Episodes;
using Shiftv.Contracts.Services.Shows;
using Shiftv.Global;

namespace Shiftv.DesignServices.Implementation
{
    public class EpisodeDesignService : IEpisodeService
    {

        public IEpisode GetNextEpisode(IShow show)
        {
            throw new NotImplementedException();
        }

        public Task<DataResult<List<IEpisode>>> GetEpisodesBySeason(int season)
        {
            return Task.Run(() =>
            {
                var manifestResourceStream = Assembly.Load(new AssemblyName("Shiftv.DesignServices.Implementation")).GetManifestResourceStream(@"Shiftv.DesignServices.Implementation.Data.GetEpisodesBySeason.json");
                var streamReader = new StreamReader(manifestResourceStream);
                var jsonString = streamReader.ReadToEnd();
                var tracksCollection = JsonConvert.DeserializeObject<List<EpisodeDto>>(jsonString);
                return new DataResult<List<IEpisode>>(tracksCollection.Select(x=> EpisodeDtoFactory.Create(x,null)).ToList());
            });
        }

        public Task<DataResult<List<IUser>>> GetWatchingNow(int season, int episode)
        {
            throw new NotImplementedException();
        }

        public Task<DataResult<IGenericPostResult>> SetAsSeen(int season, int episode)
        {
            throw new NotImplementedException();
        }

        public Task<DataResult<List<ISetAsSeenResult>>> SetAsSeen(List<IEpisode> episodes, IMiniShow selectedShow)
        {
            throw new NotImplementedException();
        }

        public Task<DataResult<ISetAsSeenResult>> SetAsSeen(IEpisode episode, IMiniShow miniShow = null)
        {
            throw new NotImplementedException();
        }

 
        public Task<DataResult<IGenericPostResult>> SetAsSeen(List<IEpisode> episodes, bool episode)
        {
            throw new NotImplementedException();
        }

        public Task<DataResult<IGenericPostResult>> SetAsUnseen(int season, int episode)
        {
            throw new NotImplementedException();
        }

        public Task<DataResult<IRateResult>> RateEpisode(bool love, int season, int episode)
        {
            throw new NotImplementedException();
        }


        public Task<DataResult<ICheckinResult>> CheckIn(int season, int episode)
        {
            throw new NotImplementedException();
        }

        public Task<DataResult<IMediaStream>> GetEpisodeLink(int season, int episode, DateTime episodeName, string subtitlesLanguages, int? numberAbs)
        {
            return Task.Run(() =>
            {
                var x = MediaStreamDtoFactory.Create(null);
                return new DataResult<IMediaStream>(x);
            });
        }

        public Task<DataResult<IMediaStream>> GetEpisodeLink(int season, int episode)
        {
            return Task.Run(() =>
            {
                var x = MediaStreamDtoFactory.Create(null);
                return new DataResult<IMediaStream>(x);
            });
        }

        public Task<DataResult<IGenericPostResult>> AddToWatchlist(IEpisode episode)
        {
            throw new NotImplementedException();
        }

        public Task<DataResult<IGenericPostResult>> AddEpisodesToWatchlist(List<IEpisode> episodes)
        {
            throw new NotImplementedException();
        }

        public Task<DataResult<IGenericPostResult>> RemoveAllFromWatchlist(List<IEpisode> episodes)
        {
            throw new NotImplementedException();
        }

        public Task<DataResult<List<ISetAsSeenResult>>> SetAsUnseen(List<IEpisode> episodes)
        {
            throw new NotImplementedException();
        }

        public Task<DataResult<List<IRateResult>>> RateEpisodes(int love, List<IEpisode> episodes)
        {
            throw new NotImplementedException();
        }

        public Task<DataResult<IGenericPostResult>> RemoveFromWatchlist(IEpisode season)
        {
            throw new NotImplementedException();
        }

        public Task<DataResult<IEpisode>> GetFullEpisodeInfo(int imdbId, int season, int episode)
        {
            throw new NotImplementedException();
        }

        public List<ISubtitlesLanguage> GetListSubtitlesLanguage()
        {
            var list = new List<ISubtitlesLanguage>();
            var sub1 = Ioc.Container.Resolve<ISubtitlesLanguage>();
            sub1.Language = "English";
            sub1.LanguageId = "eng";
            list.Add(sub1); 
            var sub2 = Ioc.Container.Resolve<ISubtitlesLanguage>();
            sub2.Language = "Portuguese";
            sub2.LanguageId = "por";
            list.Add(sub2);
            var sub3 = Ioc.Container.Resolve<ISubtitlesLanguage>();
            sub3.Language = "German";
            sub3.LanguageId = "ger";
            list.Add(sub3);  
            var sub4 = Ioc.Container.Resolve<ISubtitlesLanguage>();
            sub4.Language = "Spanish";
            sub4.LanguageId = "spa";
            list.Add(sub4);  
            var sub5 = Ioc.Container.Resolve<ISubtitlesLanguage>();
            sub5.Language = "Italian";
            sub5.LanguageId = "ita";
            list.Add(sub5);  
            var sub6 = Ioc.Container.Resolve<ISubtitlesLanguage>();
            sub6.Language = "French";
            sub6.LanguageId = "fra";
            list.Add(sub6);
            var sub7 = Ioc.Container.Resolve<ISubtitlesLanguage>();
            sub7.Language = "Polish";
            sub7.LanguageId = "pol";
            list.Add(sub7);    
            var sub8 = Ioc.Container.Resolve<ISubtitlesLanguage>();
            sub8.Language = "Russian";
            sub8.LanguageId = "rus";
            list.Add(sub8);   
            var sub9 = Ioc.Container.Resolve<ISubtitlesLanguage>();
            sub9.Language = "Chinese";
            sub9.LanguageId = "chi";
            list.Add(sub9);
            var sub10 = Ioc.Container.Resolve<ISubtitlesLanguage>();
            sub10.Language = "Dutch";
            sub10.LanguageId = "dut";
            list.Add(sub10);
            var sub11 = Ioc.Container.Resolve<ISubtitlesLanguage>();
            sub11.Language = "Swedish";
            sub11.LanguageId = "swe";
            list.Add(sub11);
            var sub12 = Ioc.Container.Resolve<ISubtitlesLanguage>();
            sub12.Language = "Greek";
            sub12.LanguageId = "gre";
            list.Add(sub12); 
            var sub13 = Ioc.Container.Resolve<ISubtitlesLanguage>();
            sub13.Language = "Czech";
            sub13.LanguageId = "cze";
            list.Add(sub13);  
            var sub14 = Ioc.Container.Resolve<ISubtitlesLanguage>();
            sub14.Language = "Slovak";
            sub14.LanguageId = "slo";
            list.Add(sub14);  
            var sub15 = Ioc.Container.Resolve<ISubtitlesLanguage>();
            sub15.Language = "Icelandic";
            sub15.LanguageId = "ice";
            list.Add(sub15);
            return list;
        }

        public Task<DataResult<int?>> GetAbsoluteNusmberFromTvDb(int showTvDbId, DateTime episodeAirDate)
        {
            throw new NotImplementedException();
        }

        public Task<DataResult<int?>> GetAbsoluteNumberFromTvDb(int showTvDbId, DateTime episodeAirDate)
        {
            throw new NotImplementedException();
        }

        public Task<DataResult<ICheckinResult>> CancelCheckIn()
        {
            throw new NotImplementedException();
        }

        public Task<DataResult<IMediaStream>> GetEpisodeSubtitles(string subtitlesLanguages, int season, int number)
        {
            throw new NotImplementedException();
        }

        public Task<DataResult<IGenericPostResult>> SetAsSeen(int season, int episode, int tvdbId, string imdbid, string title, int year)
        {
            throw new NotImplementedException();
        }

        public Task<DataResult<IGenericPostResult>> SetAsSeen(List<IEpisode> episodes, int tvdbId, string imdbid, string title, int year)
        {
            throw new NotImplementedException();
        }

        public Task<DataResult<IRateResult>> RateEpisode(int newValue, IEpisode season)
        {
            throw new NotImplementedException();
        }
    }
}
