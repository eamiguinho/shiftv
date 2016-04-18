using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Shiftv.Contracts.Data.Factories;
using Shiftv.Contracts.Data.Movies;
using Shiftv.Contracts.Data.Peoples;
using Shiftv.Contracts.Domain.Categories;
using Shiftv.Contracts.Domain.Movies;
using Shiftv.Contracts.Domain.Peoples;
using Shiftv.Contracts.Domain.Results;
using Shiftv.Contracts.Domain.Shows;
using Shiftv.Contracts.Services.Movies;
using Shiftv.Global;

namespace Shiftv.DesignServices.Implementation
{
    public class MovieDesignService : IMovieService
    {
        public Task<DataResult<List<IMiniMovie>>> GetTrending(bool b)
        {
            return Task.Run(() =>
            {
                var manifestResourceStream = Assembly.Load(new AssemblyName("Shiftv.DesignServices.Implementation")).GetManifestResourceStream(@"Shiftv.DesignServices.Implementation.Data.GetMoviesTrending.json");
                var streamReader = new StreamReader(manifestResourceStream);
                var jsonString = streamReader.ReadToEnd();
                var tracksCollection = JsonConvert.DeserializeObject<List<MiniMovieDto>>(jsonString);
                return new DataResult<List<IMiniMovie>>(tracksCollection.Select(MiniMovieDtoFactory.Create).ToList());
            });
        }

        public Task<DataResult<List<IMiniMovie>>> SearchMoviesByKey(string key)
        {
            return Task.Run(() =>
            {
                var manifestResourceStream = Assembly.Load(new AssemblyName("Shiftv.DesignServices.Implementation")).GetManifestResourceStream(@"Shiftv.DesignServices.Implementation.Data.GetMoviesTrending.json");
                var streamReader = new StreamReader(manifestResourceStream);
                var jsonString = streamReader.ReadToEnd();
                var tracksCollection = JsonConvert.DeserializeObject<List<MiniMovieDto>>(jsonString);
                return new DataResult<List<IMiniMovie>>(tracksCollection.Select(MiniMovieDtoFactory.Create).ToList());
            });
        }

        public Task<DataResult<List<IMovie>>> GetRecommendations()
        {
            return Task.Run(() =>
            {
                var manifestResourceStream = Assembly.Load(new AssemblyName("Shiftv.DesignServices.Implementation")).GetManifestResourceStream(@"Shiftv.DesignServices.Implementation.Data.GetMoviesTrending.json");
                var streamReader = new StreamReader(manifestResourceStream);
                var jsonString = streamReader.ReadToEnd();
                var tracksCollection = JsonConvert.DeserializeObject<List<MovieDto>>(jsonString);
                return new DataResult<List<IMovie>>(tracksCollection.Select(MovieDtoFactory.Create).ToList());
            });
        }

        public Task<DataResult<List<IMovie>>> GetByCategory(ICategory category)
        {
            throw new System.NotImplementedException();
        }

        public IMovie GetCurrentMovie()
        {
                var manifestResourceStream = Assembly.Load(new AssemblyName("Shiftv.DesignServices.Implementation")).GetManifestResourceStream(@"Shiftv.DesignServices.Implementation.Data.GetMovieDetails.json");
                var streamReader = new StreamReader(manifestResourceStream);
                var jsonString = streamReader.ReadToEnd();
                var tracksCollection = JsonConvert.DeserializeObject<MovieDto>(jsonString);
                return MovieDtoFactory.Create(tracksCollection);
        }

        public Task<DataResult<ICheckinResult>> CheckIn()
        {
            throw new System.NotImplementedException();
        }

        public Task<DataResult<List<IMiniMovie>>> GetMoviesWatchlistByUser(string username)
        {
            throw new System.NotImplementedException();
        }

        public Task<DataResult> SetCurrent(IMovie toModel)
        {
            throw new System.NotImplementedException();
        }

        public Task<DataResult> SetCurrent(IMiniMovie toModel)
        {
            throw new System.NotImplementedException();
        }

        public Task<DataResult<List<IMovie>>> GetLovedByUser(string username)
        {
            return Task.Run(() =>
            {
                var manifestResourceStream = Assembly.Load(new AssemblyName("Shiftv.DesignServices.Implementation")).GetManifestResourceStream(@"Shiftv.DesignServices.Implementation.Data.GetMoviesTrending.json");
                var streamReader = new StreamReader(manifestResourceStream);
                var jsonString = streamReader.ReadToEnd();
                var tracksCollection = JsonConvert.DeserializeObject<List<MovieDto>>(jsonString);
                return new DataResult<List<IMovie>>(tracksCollection.Select(MovieDtoFactory.Create).ToList());
            });
        }

        public Task<DataResult<IMediaStream>> GetMovieLink()
        {
            throw new System.NotImplementedException();
        }

        public Task<DataResult<IMediaStream>> GetMovieLink(string subtitlesLanguages)
        {
            throw new System.NotImplementedException();
        }

        public Task<DataResult<List<IMiniMovie>>> GetTop()
        {
            return Task.Run(() =>
            {
                var manifestResourceStream = Assembly.Load(new AssemblyName("Shiftv.DesignServices.Implementation")).GetManifestResourceStream(@"Shiftv.DesignServices.Implementation.Data.GetMoviesTrending.json");
                var streamReader = new StreamReader(manifestResourceStream);
                var jsonString = streamReader.ReadToEnd();
                var tracksCollection = JsonConvert.DeserializeObject<List<MiniMovieDto>>(jsonString);
                return new DataResult<List<IMiniMovie>>(tracksCollection.Select(MiniMovieDtoFactory.Create).ToList());
            });
        }

        public Task<DataResult<List<IMiniMovie>>> GetFresh()
        {
            return Task.Run(() =>
            {
                var manifestResourceStream = Assembly.Load(new AssemblyName("Shiftv.DesignServices.Implementation")).GetManifestResourceStream(@"Shiftv.DesignServices.Implementation.Data.GetMoviesTrending.json");
                var streamReader = new StreamReader(manifestResourceStream);
                var jsonString = streamReader.ReadToEnd();
                var tracksCollection = JsonConvert.DeserializeObject<List<MiniMovieDto>>(jsonString);
                return new DataResult<List<IMiniMovie>>(tracksCollection.Select(MiniMovieDtoFactory.Create).ToList());
            });
        }

        public Task<DataResult<IGenericPostResult>> AddToWatchlist()
        {
            throw new System.NotImplementedException();
        }

        public Task<DataResult<IGenericPostResult>> RemoveFromWatchlist()
        {
            throw new System.NotImplementedException();
        }

        public Task<DataResult<IGenericPostResult>> SetAsSeen()
        {
            throw new System.NotImplementedException();
        }

        public Task<DataResult<IGenericPostResult>> SetAsUnseen()
        {
            throw new System.NotImplementedException();
        }

        public Task<DataResult<IRateResult>> RateMovie(int rate)
        {
            throw new System.NotImplementedException();
        }

        public Task<DataResult<List<IMiniMovie>>> GetTopImdb()
        {
            return Task.Run(() =>
            {
                var manifestResourceStream = Assembly.Load(new AssemblyName("Shiftv.DesignServices.Implementation")).GetManifestResourceStream(@"Shiftv.DesignServices.Implementation.Data.GetMoviesTrending.json");
                var streamReader = new StreamReader(manifestResourceStream);
                var jsonString = streamReader.ReadToEnd();
                var tracksCollection = JsonConvert.DeserializeObject<List<MiniMovieDto>>(jsonString);
                return new DataResult<List<IMiniMovie>>(tracksCollection.Select(MiniMovieDtoFactory.Create).ToList());
            });
        }

        public Task<DataResult<List<IMiniMovie>>> GetAnimationMovies()
        {
            throw new System.NotImplementedException();
        }

        public Task<DataResult<IMediaStream>> GetMovieSubtitles(string subtitlesLanguages)
        {
            throw new System.NotImplementedException();
        }

        public void ClearTrending()
        {
            throw new System.NotImplementedException();
        }

        public Task<DataResult<List<IMiniMovie>>> GetPopular()
        {
            throw new System.NotImplementedException();
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

        public Task<DataResult<List<IMiniMovie>>> GetOscarsMovies()
        {
            throw new System.NotImplementedException();
        }

        public Task<DataResult<List<IMiniMovie>>> GetChristmasMovies()
        {
            throw new System.NotImplementedException();
        }

        public Task<DataResult<double?>> GetImdbRanting(string imdbId)
        {
            return Task.FromResult(new DataResult<double?>(10.0));
        }

        public Task<DataResult<ICheckinResult>> CancelCheckIn()
        {
            throw new System.NotImplementedException();
        }
    }
}