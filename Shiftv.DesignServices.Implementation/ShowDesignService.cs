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
using Shiftv.Contracts.Data.Peoples;
using Shiftv.Contracts.Data.Shows;
using Shiftv.Contracts.Domain.Movies;
using Shiftv.Contracts.Domain.Peoples;
using Shiftv.Contracts.Domain.Results;
using Shiftv.Contracts.Domain.Shows;
using Shiftv.Contracts.Services.Shows;
using Shiftv.Global;

namespace Shiftv.DesignServices.Implementation
{
    public class ShowDesignService : IShowService
    {
        public Task<DataResult<List<IMiniShow>>> GetTrending(bool b)
        {
            return Task.Run(() =>
            {
                var manifestResourceStream = Assembly.Load(new AssemblyName("Shiftv.DesignServices.Implementation")).GetManifestResourceStream(@"Shiftv.DesignServices.Implementation.Data.TrendingShows.json");
                var streamReader = new StreamReader(manifestResourceStream);
                var jsonString = streamReader.ReadToEnd();
                var tracksCollection = JsonConvert.DeserializeObject<List<MiniShowDto>>(jsonString);
                return new DataResult<List<IMiniShow>>(tracksCollection.Select(MiniShowDtoFactory.CreateShow).ToList());
            });
        }

        public Task<DataResult<List<IMiniShow>>> GetTop()
        {
            return Task.Run(() =>
            {
                var manifestResourceStream = Assembly.Load(new AssemblyName("Shiftv.DesignServices.Implementation")).GetManifestResourceStream(@"Shiftv.DesignServices.Implementation.Data.TrendingShows.json");
                var streamReader = new StreamReader(manifestResourceStream);
                var jsonString = streamReader.ReadToEnd();
                var tracksCollection = JsonConvert.DeserializeObject<List<MiniShowDto>>(jsonString);
                return new DataResult<List<IMiniShow>>(tracksCollection.Select(MiniShowDtoFactory.CreateShow).ToList());
            });
        }

        public Task<DataResult<List<IMiniShow>>> GetFresh()
        {
            return Task.Run(() =>
            {
                var manifestResourceStream = Assembly.Load(new AssemblyName("Shiftv.DesignServices.Implementation")).GetManifestResourceStream(@"Shiftv.DesignServices.Implementation.Data.TrendingShows.json");
                var streamReader = new StreamReader(manifestResourceStream);
                var jsonString = streamReader.ReadToEnd();
                var tracksCollection = JsonConvert.DeserializeObject<List<MiniShowDto>>(jsonString);
                return new DataResult<List<IMiniShow>>(tracksCollection.Select(MiniShowDtoFactory.CreateShow).ToList());
            });
        }

        public Task<DataResult<IShow>> GetByImdbId(IIds imdbId)
        {
            return Task.Run(() =>
            {
                var manifestResourceStream = Assembly.Load(new AssemblyName("Shiftv.DesignServices.Implementation")).GetManifestResourceStream(@"Shiftv.DesignServices.Implementation.Data.GetShowById.json");
                var streamReader = new StreamReader(manifestResourceStream);
                var jsonString = streamReader.ReadToEnd();
                var showDto = JsonConvert.DeserializeObject<ShowDto>(jsonString);
                return new DataResult<IShow>(ShowDtoFactory.CreateShow(showDto));
            });
        }



        public Task<DataResult> SetCurrent(IShow toModel)
        {
            throw new NotImplementedException();
        }

        public Task<DataResult> SetCurrent(IMiniShow toModel)
        {
            throw new NotImplementedException();
        }

        public IShow GetCurrentShow()
        {
            var manifestResourceStream = Assembly.Load(new AssemblyName("Shiftv.DesignServices.Implementation")).GetManifestResourceStream(@"Shiftv.DesignServices.Implementation.Data.GetShowById.json");
            var streamReader = new StreamReader(manifestResourceStream);
            var jsonString = streamReader.ReadToEnd();
            var showDto = JsonConvert.DeserializeObject<ShowDto>(jsonString);
            return ShowDtoFactory.CreateShow(showDto);
        }

        public Task<IShow> GetCurrentShowWithoutCache()
        {
            throw new NotImplementedException();
        }

        public Task<DataResult<List<IShow>>> GetRecommendations()
        {
            return Task.Run(() =>
            {
                var manifestResourceStream = Assembly.Load(new AssemblyName("Shiftv.DesignServices.Implementation")).GetManifestResourceStream(@"Shiftv.DesignServices.Implementation.Data.TrendingShows.json");
                var streamReader = new StreamReader(manifestResourceStream);
                var jsonString = streamReader.ReadToEnd();
                var tracksCollection = JsonConvert.DeserializeObject<List<ShowDto>>(jsonString);
                return new DataResult<List<IShow>>(tracksCollection.Select(ShowDtoFactory.CreateShow).ToList());
            });
        }

        public Task<DataResult<List<IMiniShow>>> SearchShowsByKey(string key)
        {
            return Task.Run(() =>
            {
                var manifestResourceStream = Assembly.Load(new AssemblyName("Shiftv.DesignServices.Implementation")).GetManifestResourceStream(@"Shiftv.DesignServices.Implementation.Data.TrendingShows.json");
                var streamReader = new StreamReader(manifestResourceStream);
                var jsonString = streamReader.ReadToEnd();
                var tracksCollection = JsonConvert.DeserializeObject<List<MiniShowDto>>(jsonString);
                return new DataResult<List<IMiniShow>>(tracksCollection.Select(MiniShowDtoFactory.CreateShow).ToList());
            });
        }

        public void UpdateCurrentShow()
        {
            throw new NotImplementedException();
        }

        public Task<DataResult<IRateResult>> RateShow(int rate)
        {
            throw new NotImplementedException();
        }

        public Task<DataResult<IGenericPostResult>> AddToWatchlist()
        {
            throw new NotImplementedException();
        }

        public Task<DataResult<IGenericPostResult>> RemoveFromWatchlist()
        {
            throw new NotImplementedException();
        }

        public Task<DataResult<List<IShow>>> GetShowsWithEpisodesWatchlistByUser(string username)
        {
            return Task.Run(() =>
            {
                var manifestResourceStream = Assembly.Load(new AssemblyName("Shiftv.DesignServices.Implementation")).GetManifestResourceStream(@"Shiftv.DesignServices.Implementation.Data.TrendingShows.json");
                var streamReader = new StreamReader(manifestResourceStream);
                var jsonString = streamReader.ReadToEnd();
                var tracksCollection = JsonConvert.DeserializeObject<List<ShowDto>>(jsonString);
                return new DataResult<List<IShow>>(tracksCollection.Select(ShowDtoFactory.CreateShow).ToList());
            });
        }

        public Task<DataResult<List<IShow>>> GetShowsWatchlistByUser(string username)
        {
            return Task.Run(() =>
            {
                var manifestResourceStream = Assembly.Load(new AssemblyName("Shiftv.DesignServices.Implementation")).GetManifestResourceStream(@"Shiftv.DesignServices.Implementation.Data.TrendingShows.json");
                var streamReader = new StreamReader(manifestResourceStream);
                var jsonString = streamReader.ReadToEnd();
                var tracksCollection = JsonConvert.DeserializeObject<List<ShowDto>>(jsonString);
                return new DataResult<List<IShow>>(tracksCollection.Select(ShowDtoFactory.CreateShow).ToList());
            });
        }

        public Task<DataResult<List<IMiniShow>>> GetAnime()
        {
            return Task.Run(() =>
            {
                var manifestResourceStream = Assembly.Load(new AssemblyName("Shiftv.DesignServices.Implementation")).GetManifestResourceStream(@"Shiftv.DesignServices.Implementation.Data.TrendingShows.json");
                var streamReader = new StreamReader(manifestResourceStream);
                var jsonString = streamReader.ReadToEnd();
                var tracksCollection = JsonConvert.DeserializeObject<List<MiniShowDto>>(jsonString);
                return new DataResult<List<IMiniShow>>(tracksCollection.Select(MiniShowDtoFactory.CreateShow).ToList());
            });
        }

        public Task<DataResult<double?>> GetImdbRanting(string imdbId)
        {
            return Task.FromResult(new DataResult<double?>(10.0));
        }

        public Task<DataResult<List<IShowProgress>>> GetShowProgress()
        {
            return Task.Run(() =>
            {
                var manifestResourceStream = Assembly.Load(new AssemblyName("Shiftv.DesignServices.Implementation")).GetManifestResourceStream(@"Shiftv.DesignServices.Implementation.Data.GetShowProgress.json");
                var streamReader = new StreamReader(manifestResourceStream);
                var jsonString = streamReader.ReadToEnd();
                var tracksCollection = JsonConvert.DeserializeObject<List<ShowProgressDto>>(jsonString);
                return new DataResult<List<IShowProgress>>(tracksCollection.Select(ShowProgressDtoFactory.Create).ToList());
            });
        }

        public Task<DataResult<List<IShowProgress>>> GetShowProgress(string imdbId)
        {
            return Task.Run(() =>
            {
                var manifestResourceStream = Assembly.Load(new AssemblyName("Shiftv.DesignServices.Implementation")).GetManifestResourceStream(@"Shiftv.DesignServices.Implementation.Data.GetShowProgress.json");
                var streamReader = new StreamReader(manifestResourceStream);
                var jsonString = streamReader.ReadToEnd();
                var tracksCollection = JsonConvert.DeserializeObject<List<ShowProgressDto>>(jsonString);
                return new DataResult<List<IShowProgress>>(tracksCollection.Select(ShowProgressDtoFactory.Create).ToList());
            });
        }

        public Task<DataResult<List<IShow>>> GetLovedByUser(string username)
        {
            return Task.Run(() =>
            {
                var manifestResourceStream = Assembly.Load(new AssemblyName("Shiftv.DesignServices.Implementation")).GetManifestResourceStream(@"Shiftv.DesignServices.Implementation.Data.TrendingShows.json");
                var streamReader = new StreamReader(manifestResourceStream);
                var jsonString = streamReader.ReadToEnd();
                var tracksCollection = JsonConvert.DeserializeObject<List<ShowDto>>(jsonString);
                return new DataResult<List<IShow>>(tracksCollection.Select(ShowDtoFactory.CreateShow).ToList());
            });
        }

        public Task<DataResult> SetCurrent(IIds imdbId, string title)
        {
            throw new NotImplementedException();
        }
        public Task<DataResult> SetCurrentAzure(IShow toModel)
        {
            throw new NotImplementedException();
        }

        public Task<DataResult<IPeople>> GetPeople()
        {
            return Task.Run(() =>
             {
                 var manifestResourceStream = Assembly.Load(new AssemblyName("Shiftv.DesignServices.Implementation")).GetManifestResourceStream(@"Shiftv.DesignServices.Implementation.Data.GetPeople.json");
                 var streamReader = new StreamReader(manifestResourceStream);
                 var jsonString = streamReader.ReadToEnd();
                 var showDto = JsonConvert.DeserializeObject<PeopleDto>(jsonString);
                 return new DataResult<IPeople>(PeopleDtoFactory.Create(showDto));
             });
        }

        public Task<DataResult<List<IMiniShow>>> GetPopular()
        {
            throw new NotImplementedException();
        }

        public Task<DataResult> ForceUpdate()
        {
            throw new NotImplementedException();
        }

        public Task<DataResult<List<IMiniShow>>> GetTopImdb()
        {
            throw new NotImplementedException();
        }

        public void ClearTrending()
        {
            throw new NotImplementedException();
        }

        public void UpdateProgress()
        {
            throw new NotImplementedException();
        }

        public void UpdateWatchlist()
        {
            throw new NotImplementedException();
        }


    }
}
