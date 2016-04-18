using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Shiftv.Contracts.Data.Comments;
using Shiftv.Contracts.Data.Factories;
using Shiftv.Contracts.Domain.Comments;
using Shiftv.Contracts.Domain.Shows;
using Shiftv.Contracts.Services.Comments;
using Shiftv.Global;

namespace Shiftv.DesignServices.Implementation
{
    public class CommentDesignService : ICommentService
    {
        public Task<DataResult<List<IComment>>> GetCommentsShowById()
        {
            return Task.Run(() =>
            {
                var manifestResourceStream = Assembly.Load(new AssemblyName("Shiftv.DesignServices.Implementation")).GetManifestResourceStream(@"Shiftv.DesignServices.Implementation.Data.GetCommentsByShow.json");
                var streamReader = new StreamReader(manifestResourceStream);
                var jsonString = streamReader.ReadToEnd();
                var tracksCollection = JsonConvert.DeserializeObject<List<CommentDto>>(jsonString);
                return new DataResult<List<IComment>>(tracksCollection.Select(dto => CommentDtoFactory.Create(dto)).ToList());
            });
        }

        public Task<DataResult<ICommentResult>> CommentsShow(string comment, bool isSpoiler, bool isReview)
        {
            throw new NotImplementedException();
        }

        public Task<DataResult<ICommentResult>> CommentEpisode(string comment, int season, int episode, bool isSpoiler, bool isReview)
        {
            throw new NotImplementedException();
        }

        public Task<DataResult<List<IComment>>> GetCommentsMovie()
        {
            return Task.Run(() =>
            {
                var manifestResourceStream = Assembly.Load(new AssemblyName("Shiftv.DesignServices.Implementation")).GetManifestResourceStream(@"Shiftv.DesignServices.Implementation.Data.GetCommentsByShow.json");
                var streamReader = new StreamReader(manifestResourceStream);
                var jsonString = streamReader.ReadToEnd();
                var tracksCollection = JsonConvert.DeserializeObject<List<CommentDto>>(jsonString);
                return new DataResult<List<IComment>>(tracksCollection.Select(dto => CommentDtoFactory.Create(dto)).ToList());
            });
        }

        public Task<DataResult<ICommentResult>> CommentsMovie(string comment, bool isSpoiler, bool isReview)
        {
            throw new NotImplementedException();
        }

        public Task<DataResult<List<IComment>>> GetEpisodeComments(IShow show, int season, int episode)
        {
            return Task.Run(() =>
            {
                var manifestResourceStream = Assembly.Load(new AssemblyName("Shiftv.DesignServices.Implementation")).GetManifestResourceStream(@"Shiftv.DesignServices.Implementation.Data.GetEpisodeComments.json");
                var streamReader = new StreamReader(manifestResourceStream);
                var jsonString = streamReader.ReadToEnd();
                var tracksCollection = JsonConvert.DeserializeObject<List<CommentDto>>(jsonString);
                return new DataResult<List<IComment>>(tracksCollection.Select(dto => CommentDtoFactory.Create(dto)).ToList());
            });
        }
    }
}
