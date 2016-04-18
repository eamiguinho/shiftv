using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ShiftvAPI.Contracts.Data;
using ShiftvAPI.Contracts.Data.PostObjects;
using ShiftvAPI.Contracts.Data.Sync;
using ShiftvAPI.Contracts.Infrastucture.Shiftv;
using ShiftvAPI.Infrastucture.Implementation.Helpers;

namespace ShiftvAPI.Infrastucture.Implementation.Sync
{
    internal class SyncShiftvDataService : ISyncShiftvDataService
    {
        public void SaveRatingsShowsMovies(List<RatingSync> ratingSyncs, string token, RequestType requestType)
        {

            try
            {


                var list = new List<UserRating>();


                foreach (var ratingSync in ratingSyncs)
                {
                    switch (requestType)
                    {
                        case RequestType.Shows:
                            if (ratingSync.Show.Ids.TraktId != null)
                                list.Add(new UserRating
                                {
                                    TraktId = ratingSync.Show.Ids.TraktId.Value,
                                    Rating = ratingSync.Rating
                                });
                            break;
                        case RequestType.Seasons:
                            if (ratingSync.Show.Ids.TraktId != null)
                                list.Add(new UserRating
                                {
                                    TraktId = ratingSync.Show.Ids.TraktId.Value,
                                    Rating = ratingSync.Rating,
                                    Season = ratingSync.Season.Number
                                });
                            break;
                        case RequestType.Episodes:
                            if (ratingSync.Show.Ids.TraktId != null)
                                list.Add(new UserRating
                                {
                                    TraktId = ratingSync.Show.Ids.TraktId.Value,
                                    Rating = ratingSync.Rating,
                                    Season = ratingSync.Episode.Season,
                                    Episode = ratingSync.Episode.Number
                                });
                            break;
                        case RequestType.Movies:
                            if (ratingSync.Movie.Ids.TraktId != null)
                                list.Add(new UserRating
                                {
                                    TraktId = ratingSync.Movie.Ids.TraktId.Value,
                                    Rating = ratingSync.Rating
                                });
                            break;
                    }

                }
                SaveUserRatingDb(token, list, requestType);

            }
            catch (Exception e)
            {
                return;
            }

        }

        private static void SaveUserRatingDb(string token, List<UserRating> list, RequestType requestType)
        {
            var tblName = "";
            switch (requestType)
            {
                case RequestType.Shows:
                    tblName = "tbl_UserShowRating";
                    break;
                case RequestType.Seasons:
                    tblName = "tbl_UserSeasonRating";
                    break;
                case RequestType.Episodes:
                    tblName = "tbl_UserEpisodeRating";
                    break;
                case RequestType.Movies:
                    tblName = "tbl_UserMovieRating";
                    break;
            }
            using (var db = new SqlHandler())
            {

                var listParsed = JsonConvert.SerializeObject(list);
                SqlCommand cmd = new SqlCommand(string.Format(@"
                    declare @UserId int 
                    Select @UserId = Id from tbl_Users Where [TraktAccessToken] = @AccessToken
IF @UserId is null
  BEGIN 
     Select @UserId = Id from tbl_Users Where [AccessToken] = @AccessToken
  END 
                    IF (NOT EXISTS(SELECT null FROM {0} WHERE [UserId] = @UserId)) 
                    BEGIN 
                        INSERT INTO {0} ([UserId], [LastUpdate], [JsonData]) 
                            VALUES (@UserId, @LastUpdate, @JsonData)
                    END 
                    ELSE 
                    BEGIN 
                        UPDATE {0} 
                        SET LastUpdate = @LastUpdate, JsonData = @JsonData
                        WHERE [UserId] = @UserId
                    END 
              ", tblName), db.MyConnection);
                cmd.Parameters.AddWithValue("@AccessToken", token);
                cmd.Parameters.AddWithValue("@JsonData", listParsed);
                cmd.Parameters.AddWithValue("@LastUpdate", DateTime.Now);
                cmd.ExecuteNonQuery();
            }
        }


        public void UpdateRatingUser(List<UserRating> userRatedShows, string token, RequestType type)
        {
            SaveUserRatingDb(token, userRatedShows, type);
        }



        public void SaveRatingsEpisodes(List<RatingSync> ratingSyncs, string token, RequestType requestType)
        {

            try
            {
                using (var db = new SqlHandler())
                {
                    foreach (var ratingSync in ratingSyncs)
                    {
                        SqlCommand cmd = new SqlCommand(string.Format(@"
                    declare @UserId int 
                    Select @UserId = Id from tbl_Users Where [TraktAccessToken] = @AccessToken

                    IF (NOT EXISTS(SELECT null FROM {0} WHERE [UserId] = @UserId and [ShowTraktId] = @ShowTraktId and [Season] = @Season and [Episode] = @Episode)) 
                    BEGIN 
                        INSERT INTO {0} ([UserId], [Rating], [LastUpdate], [ShowTraktId], [Episode], [Season]) 
                            VALUES (@UserId, @Rating, @LastUpdate, @ShowTraktId, @Episode, @Season)
                    END 
                    ELSE 
                    BEGIN 
                        UPDATE {0} 
                        SET Rating = @Rating, LastUpdate = @LastUpdate
                        WHERE [UserId] = @UserId and [ShowTraktId] = @ShowTraktId and [Season] = @Season and [Episode] = @Episode
                    END 
              ", "tbl_UserEpisodeRatingQueue"), db.MyConnection);
                        cmd.Parameters.AddWithValue("@ShowTraktId", ratingSync.Show.Ids.TraktId);
                        cmd.Parameters.AddWithValue("@AccessToken", token);
                        cmd.Parameters.AddWithValue("@Rating", ratingSync.Rating);
                        cmd.Parameters.AddWithValue("@Season", ratingSync.Rating);
                        cmd.Parameters.AddWithValue("@Episode", ratingSync.Rating);
                        cmd.Parameters.AddWithValue("@LastUpdate", DateTime.Now);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception)
            {
                return;
            }
        }


        public void SaveWatchedEpisode(List<SyncWatched> watchedSyncs, string token, List<SetAsSeenJson> getWatchedEpisodesQueue)
        {
            try
            {
                var list = new List<WatchedEpisodes>();
              
                foreach (var watchedsync in watchedSyncs)
                {
                    foreach (var season in watchedsync.Seasons)
                    {
                        foreach (var episode in season.Episodes)
                        {
                            if (watchedsync.Show.Ids.TraktId != null)
                                if (season.Number != null)
                                    list.Add(new WatchedEpisodes
                                    {
                                        TraktShowId = watchedsync.Show.Ids.TraktId.Value,
                                        EpisodeNumber = episode.Number,
                                        SeasonNumber = season.Number.Value
                                    });
                        }
                    }

                }

                foreach (var setAsSeenJson in getWatchedEpisodesQueue)
                {
                        var epiInList = list.FirstOrDefault(
                            x =>
                                x.TraktShowId == setAsSeenJson.Show.Ids.TraktId.Value &&
                                x.EpisodeNumber == setAsSeenJson.Episode.Number);
                    if (epiInList != null)
                    {
                        if (!setAsSeenJson.Watched) list.Remove(epiInList);
                    }
                    else
                    {
                        list.Add(new WatchedEpisodes
                        {
                            TraktShowId = setAsSeenJson.Show.Ids.TraktId.Value,
                            EpisodeNumber = setAsSeenJson.Episode.Number,
                            SeasonNumber = setAsSeenJson.Episode.Season
                        });
                    }
                }
                SaveDbWatchedEpisodes(token, list);
            }
            catch (Exception)
            {
                return;
            }
        }

        private static void SaveDbWatchedEpisodes(string token, List<WatchedEpisodes> list)
        {
            using (var db = new SqlHandler())
            {
                var jsonData = JsonConvert.SerializeObject(list);
                SqlCommand cmd = new SqlCommand(string.Format(@"
                    declare @UserId int 
                    Select @UserId = Id from tbl_Users Where [TraktAccessToken] = @AccessToken
IF @UserId is null
  BEGIN 
     Select @UserId = Id from tbl_Users Where [AccessToken] = @AccessToken
  END 
                    IF (NOT EXISTS(SELECT null FROM tbl_UserShowWatched WHERE UserId = @UserId)) 
                    BEGIN 
                        INSERT INTO tbl_UserShowWatched (UserId, LastUpdate, JsonData) 
                            VALUES (@UserId, @LastUpdate, @JsonData)
                    END 
                    ELSE 
                    BEGIN 
                        UPDATE tbl_UserShowWatched
                        SET  LastUpdate = @LastUpdate, JsonData = @JsonData
                        WHERE UserId = @UserId
                    END 
              ", "tbl_UserEpisodeRating"), db.MyConnection);
                cmd.Parameters.AddWithValue("@JsonData", jsonData);
                cmd.Parameters.AddWithValue("@AccessToken", token);
                cmd.Parameters.AddWithValue("@LastUpdate", DateTime.Now);
                cmd.ExecuteNonQuery();
            }
        }


        public void UpdateWatchedEpisode(List<WatchedEpisodes> userWatchedEpisodes, string token)
        {
            SaveDbWatchedEpisodes(token, userWatchedEpisodes);
        }



        public void UpdateWatchedMovies(List<WatchedMovie> userWatchedMovies, string token)
        {
            UpdateMoviesWatchedDb(token, userWatchedMovies);
        }



        public void SaveWatchedMovies(List<SyncWatched> watchedSyncs, string token, List<SetAsSeenJson> getWatchedMovieQueue)
        {
            try
            {
                var list = new List<WatchedMovie>();
                foreach (var watchedSync in watchedSyncs)
                {
                    if (watchedSync.Movie.Ids.TraktId != null)
                    {
                        list.Add(new WatchedMovie {TraktId = watchedSync.Movie.Ids.TraktId.Value});

                    }
                }
                foreach (var setAsSeenJson in getWatchedMovieQueue)
                {
                    var epiInList = list.FirstOrDefault(
                        x =>
                            x.TraktId == setAsSeenJson.Movie.Ids.TraktId.Value);
                    if (epiInList != null)
                    {
                        if (!setAsSeenJson.Watched) list.Remove(epiInList);
                    }
                    else
                    {
                        list.Add(new WatchedMovie
                        {
                            TraktId = setAsSeenJson.Movie.Ids.TraktId.Value
                        });
                    }
                }

                UpdateMoviesWatchedDb(token, list);

            }
            catch (Exception)
            {
                return;
            }
        }

        private static void UpdateMoviesWatchedDb(string token, List<WatchedMovie> list)
        {
            using (var db = new SqlHandler())
            {

                var listParsed = JsonConvert.SerializeObject(list);

                SqlCommand cmd = new SqlCommand(string.Format(@"
                    declare @UserId int 
                    Select @UserId = Id from tbl_Users Where [TraktAccessToken] = @AccessToken
IF @UserId is null
  BEGIN 
     Select @UserId = Id from tbl_Users Where [AccessToken] = @AccessToken
  END 
                    IF (NOT EXISTS(SELECT null FROM tbl_UserMovieWatched WHERE [UserId] = @UserId)) 
                    BEGIN 
                        INSERT INTO tbl_UserMovieWatched (UserId, LastUpdate, JsonData) 
                            VALUES (@UserId, @LastUpdate,@JsonData)
                    END 
                    ELSE 
                    BEGIN 
                        UPDATE tbl_UserMovieWatched
                        SET  LastUpdate = @LastUpdate, JsonData = @JsonData
                        WHERE [UserId] = @UserId
                    END 
              ", "tbl_UserEpisodeRating"), db.MyConnection);
                cmd.Parameters.AddWithValue("@JsonData", listParsed);
                cmd.Parameters.AddWithValue("@AccessToken", token);
                cmd.Parameters.AddWithValue("@LastUpdate", DateTime.Now);
                cmd.ExecuteNonQuery();
            }
        }


        public List<WatchedEpisodes> GetEpisodeWatchedByUser(string token)
        {
            try
            {
                using (var db = new SqlHandler())
                {
                    var query = string.Format(@"
declare @UserId int
Select @UserId = Id from tbl_Users where AccessToken = '{0}'
Select JsonData from tbl_UserShowWatched where UserId=@UserId", token);
                    var myCommand = new SqlCommand(query,
                        db.MyConnection);
                    var list = new List<WatchedEpisodes>();
                    using (var myReader = myCommand.ExecuteReader())
                    {
                        while (myReader.Read())
                        {
                            var jsonData = (string) myReader["JsonData"];
                            list = JsonConvert.DeserializeObject<List<WatchedEpisodes>>(jsonData);
                            return list;
                        }
                        return list;
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool SaveShowRating(RateRequestJsonDto rateRequest, string token)
        {
            try
            {
                using (var db = new SqlHandler())
                {
                    SqlCommand cmd = new SqlCommand(@"
                    declare @UserId int 
                    Select @UserId = Id from tbl_Users Where [AccessToken] = @AccessToken
                    IF (NOT EXISTS(SELECT null FROM tbl_UserShowRatingQueue WHERE [TraktId] = @TraktId and [UserId] = @UserId)) 
                    BEGIN 
                        INSERT INTO tbl_UserShowRatingQueue ([TraktId], [UserId], [Rating], [TraktSynced],[LastUpdate],[CommitTraktJson]) 
                            VALUES (@TraktId, @UserId, @Rating, 0, @LastUpdate,@CommitTraktJson)
                    END 
                    ELSE 
                    BEGIN 
                        UPDATE tbl_UserShowRatingQueue
                        SET Rating = @Rating, LastUpdate = @LastUpdate, TraktSynced = 0, CommitTraktJson = @CommitTraktJson
                        WHERE [TraktId] = @TraktId and [UserId] = @UserId
                    END 
              ", db.MyConnection);
                    cmd.Parameters.AddWithValue("@TraktId", rateRequest.Show.Ids.TraktId);
                    cmd.Parameters.AddWithValue("@AccessToken", token);
                    cmd.Parameters.AddWithValue("@Rating", rateRequest.Rating);
                    cmd.Parameters.AddWithValue("@LastUpdate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@CommitTraktJson", JsonConvert.SerializeObject(rateRequest));

                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool SaveShowComment(CommentRequestJsonDto commentRequest, string token)
        {
            try
            {
                using (var db = new SqlHandler())
                {
                    SqlCommand cmd = new SqlCommand(@"
                    declare @UserId int 
                    Select @UserId = Id from tbl_Users Where [AccessToken] = @AccessToken
                        INSERT INTO tbl_UserShowCommentsQueue (UserId, ShowTraktId, IsReview, IsSpoil, Comment, LastUpdated, TraktCommitJson) 
                            VALUES (@UserId,@TraktId, @IsReview , @IsSpoil, @Comment, @LastUpdate, @TraktCommitJson)
                
              ", db.MyConnection);
                    cmd.Parameters.AddWithValue("@TraktId", commentRequest.Show.Ids.TraktId);
                    cmd.Parameters.AddWithValue("@AccessToken", token);
                    cmd.Parameters.AddWithValue("@IsReview", commentRequest.Review);
                    cmd.Parameters.AddWithValue("@IsSpoil", commentRequest.Spoiler);
                    cmd.Parameters.AddWithValue("@Comment", commentRequest.Comment);
                    cmd.Parameters.AddWithValue("@LastUpdate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@TraktCommitJson", JsonConvert.SerializeObject(commentRequest));
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool SaveMovieRating(RateRequestJsonDto rateRequest, string token)
        {
            try
            {
                using (var db = new SqlHandler())
                {
                    SqlCommand cmd = new SqlCommand(@"
                    declare @UserId int 
                    Select @UserId = Id from tbl_Users Where [AccessToken] = @AccessToken
                    IF (NOT EXISTS(SELECT null FROM tbl_UserMovieRatingQueue WHERE [TraktId] = @TraktId and [UserId] = @UserId)) 
                    BEGIN 
                        INSERT INTO tbl_UserMovieRatingQueue ([TraktId], [UserId], [Rating], [TraktSynced],[LastUpdate], [TraktCommitJson]) 
                            VALUES (@TraktId, @UserId, @Rating, 0, @LastUpdate, @TraktCommitJson)
                    END 
                    ELSE 
                    BEGIN 
                        UPDATE tbl_UserMovieRatingQueue
                        SET Rating = @Rating, LastUpdate = @LastUpdate, TraktSynced = 0, TraktCommitJson =  @TraktCommitJson
                        WHERE [TraktId] = @TraktId and [UserId] = @UserId
                    END 
              ", db.MyConnection);
                    cmd.Parameters.AddWithValue("@TraktId", rateRequest.Movie.Ids.TraktId);
                    cmd.Parameters.AddWithValue("@AccessToken", token);
                    cmd.Parameters.AddWithValue("@Rating", rateRequest.Rating);
                    cmd.Parameters.AddWithValue("@LastUpdate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@TraktCommitJson", JsonConvert.SerializeObject(rateRequest));
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool SaveMovieComment(CommentRequestJsonDto commentRequest, string token)
        {
            try
            {
                using (var db = new SqlHandler())
                {
                    SqlCommand cmd = new SqlCommand(@"
                    declare @UserId int 
                    Select @UserId = Id from tbl_Users Where [AccessToken] = @AccessToken
                        INSERT INTO tbl_UserMovieCommentsQueue (UserId, MovieTraktId, IsReview, IsSpoil, Comment, LastUpdated, TraktCommitJson) 
                            VALUES (@UserId,@TraktId, @IsReview , @IsSpoil, @Comment, @LastUpdate, @TraktCommitJson)
                
              ", db.MyConnection);
                    cmd.Parameters.AddWithValue("@TraktId", commentRequest.Movie.Ids.TraktId);
                    cmd.Parameters.AddWithValue("@AccessToken", token);
                    cmd.Parameters.AddWithValue("@IsReview", commentRequest.Review);
                    cmd.Parameters.AddWithValue("@IsSpoil", commentRequest.Spoiler);
                    cmd.Parameters.AddWithValue("@Comment", commentRequest.Comment);
                    cmd.Parameters.AddWithValue("@LastUpdate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@TraktCommitJson", JsonConvert.SerializeObject(commentRequest));
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool SetMovieAsSeen(SetAsSeenJson setAsSeenRequest, string token)
        {
            try
            {
                using (var db = new SqlHandler())
                {
                    SqlCommand cmd = new SqlCommand(@"
                   declare @UserId int 
                    Select @UserId = Id from tbl_Users Where [AccessToken] = @AccessToken
                    IF (NOT EXISTS(SELECT null FROM tbl_UserMovieWatchedQueue WHERE [MovieTraktId] = @MovieTraktId and [UserId] = @UserId)) 
                    BEGIN 
                        INSERT INTO tbl_UserMovieWatchedQueue (MovieTraktId, UserId, LastUpdate, TraktSynced, Watched, TraktCommitJson) 
                            VALUES (@MovieTraktId, @UserId, @LastUpdate, 0, @Watched, @TraktCommitJson)
                    END 
                    ELSE 
                    BEGIN 
                        UPDATE tbl_UserMovieWatchedQueue
                        SET Watched = @Watched, LastUpdate = @LastUpdate, TraktSynced = 0, TraktCommitJson =  @TraktCommitJson
                        WHERE [MovieTraktId] = @MovieTraktId and [UserId] = @UserId
                    END 
              ", db.MyConnection);
                    cmd.Parameters.AddWithValue("@MovieTraktId", setAsSeenRequest.Movie.Ids.TraktId);
                    cmd.Parameters.AddWithValue("@AccessToken", token);
                    cmd.Parameters.AddWithValue("@Watched", setAsSeenRequest.Watched);
                    cmd.Parameters.AddWithValue("@LastUpdate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@TraktCommitJson", JsonConvert.SerializeObject(setAsSeenRequest));
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public List<SetAsSeenResultJson> SetEpisodesAsSeen(List<SetAsSeenJson> setAsSeenRequest, string token)
        {
            var list = new List<SetAsSeenResultJson>();
            using (var db = new SqlHandler())
            {
                foreach (var setAsSeenJson in setAsSeenRequest)
                {
                    var result = new SetAsSeenResultJson {Request = setAsSeenJson, Success = false};
                    try
                    {
                        SqlCommand cmd = new SqlCommand(@"
                   declare @UserId int 
                    Select @UserId = Id from tbl_Users Where [AccessToken] = @AccessToken
                    IF (NOT EXISTS(SELECT null FROM tbl_UserShowWatchedQueue WHERE [ShowTraktId] = @ShowTraktId and [UserId] = @UserId  and [Season] = @Season and [Episode] = @Episode)) 
                    BEGIN 
                        INSERT INTO tbl_UserShowWatchedQueue (ShowTraktId, Season, Episode, UserId, LastUpdate, TraktSynced, Watched, TraktCommitJson) 
                            VALUES (@ShowTraktId, @Season, @Episode,@UserId,@LastUpdate, 0, @Watched, @TraktCommitJson)
                    END 
                    ELSE 
                    BEGIN 
                        UPDATE tbl_UserShowWatchedQueue
                        SET Watched = @Watched, LastUpdate = @LastUpdate, TraktSynced = 0, TraktCommitJson =  @TraktCommitJson
                        WHERE [ShowTraktId] = @ShowTraktId and [UserId] = @UserId and [Season] = @Season and [Episode] = @Episode
                    END 
              ", db.MyConnection);
                        cmd.Parameters.AddWithValue("@ShowTraktId", setAsSeenJson.Show.Ids.TraktId);
                        cmd.Parameters.AddWithValue("@AccessToken", token);
                        cmd.Parameters.AddWithValue("@Season", setAsSeenJson.Episode.Season);
                        cmd.Parameters.AddWithValue("@Episode", setAsSeenJson.Episode.Number);
                        cmd.Parameters.AddWithValue("@Watched", setAsSeenJson.Watched);
                        cmd.Parameters.AddWithValue("@LastUpdate", DateTime.Now);
                        cmd.Parameters.AddWithValue("@TraktCommitJson",
                            JsonConvert.SerializeObject(setAsSeenJson));
                        cmd.ExecuteNonQuery();
                        result.Success = true;
                    }
                    catch (Exception e)
                    {
                        result.Success = false;
                    }
                    finally
                    {
                        list.Add(result);
                    }
                }
            }
            return list;


        }



        public List<WatchedMovie> GetMoviesWatchedByUser(string token)
        {
            try
            {
                using (var db = new SqlHandler())
                {
                    var query = string.Format(@"
declare @UserId int
Select @UserId = Id from tbl_Users where AccessToken = '{0}'
Select JsonData from tbl_UserMovieWatched where UserId=@UserId", token);
                    var myCommand = new SqlCommand(query,
                        db.MyConnection);
                    var list = new List<WatchedMovie>();
                    using (var myReader = myCommand.ExecuteReader())
                    {
                        while (myReader.Read())
                        {
                            var jsonData = (string) myReader["JsonData"];
                            list = JsonConvert.DeserializeObject<List<WatchedMovie>>(jsonData);
                        }
                        return list;
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<RateResultJsonDto> RateEpisodes(List<RateRequestJsonDto> rateRequests, string token)
        {
            var list = new List<RateResultJsonDto>();
            using (var db = new SqlHandler())
            {
                foreach (var rateRequest in rateRequests)
                {
                    var result = new RateResultJsonDto {Request = rateRequest, Success = false};
                    try
                    {
                        SqlCommand cmd = new SqlCommand(@"
                   declare @UserId int 
                    Select @UserId = Id from tbl_Users Where [AccessToken] = @AccessToken
                    IF (NOT EXISTS(SELECT null FROM tbl_UserEpisodeRatingQueue WHERE [ShowTraktId] = @ShowTraktId and [UserId] = @UserId  and [Season] = @Season and [Episode] = @Episode)) 
                    BEGIN 
                        INSERT INTO tbl_UserEpisodeRatingQueue (ShowTraktId, Season, Episode, UserId, Rating, TraktSynced, LastUpdate, TraktCommitJson) 
                            VALUES (@ShowTraktId, @Season, @Episode,@UserId, @Rating,0, @LastUpdate, @TraktCommitJson)
                    END 
                    ELSE 
                    BEGIN 
                        UPDATE tbl_UserEpisodeRatingQueue
                        SET Rating = @Rating, LastUpdate = @LastUpdate, TraktSynced = 0, TraktCommitJson =  @TraktCommitJson
                        WHERE [ShowTraktId] = @ShowTraktId and [UserId] = @UserId and [Season] = @Season and [Episode] = @Episode
                    END 
              ", db.MyConnection);
                        cmd.Parameters.AddWithValue("@ShowTraktId", rateRequest.Show.Ids.TraktId);
                        cmd.Parameters.AddWithValue("@AccessToken", token);
                        cmd.Parameters.AddWithValue("@Season", rateRequest.Episode.Season);
                        cmd.Parameters.AddWithValue("@Episode", rateRequest.Episode.Number);
                        cmd.Parameters.AddWithValue("@Rating", rateRequest.Rating);
                        cmd.Parameters.AddWithValue("@LastUpdate", DateTime.Now);
                        cmd.Parameters.AddWithValue("@TraktCommitJson", JsonConvert.SerializeObject(rateRequest));
                        cmd.ExecuteNonQuery();
                        result.Success = true;
                    }
                    catch (Exception e)
                    {
                        result.Success = false;
                    }
                    finally
                    {
                        list.Add(result);
                    }
                }
            }
            return list;
        }



        public List<UserRating> GetShowRatingsByUser(string token)
        {
            try
            {
                using (var db = new SqlHandler())
                {
                    var query = string.Format(@"
declare @UserId int
Select @UserId = Id from tbl_Users where AccessToken = '{0}'
Select JsonData from tbl_UserShowRating where UserId=@UserId", token);
                    var myCommand = new SqlCommand(query,
                        db.MyConnection);
                    using (var myReader = myCommand.ExecuteReader())
                    {
                        while (myReader.Read())
                        {
                            var listShowRating = (string) myReader["JsonData"];
                            var listShowRatingParsed = JsonConvert.DeserializeObject<List<UserRating>>(listShowRating);
                            if (listShowRatingParsed != null)
                            {
                                return listShowRatingParsed;

                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
            return null;
        }

        public UserRating GetShowRatingByUser(string token, Show showData)
        {
            try
            {
                using (var db = new SqlHandler())
                {
                    var query = string.Format(@"
declare @UserId int
Select @UserId = Id from tbl_Users where AccessToken = '{0}'
Select JsonData from tbl_UserShowRating where UserId=@UserId", token);
                    var myCommand = new SqlCommand(query,
                        db.MyConnection);
                    using (var myReader = myCommand.ExecuteReader())
                    {
                        while (myReader.Read())
                        {
                            var listShowRating = (string) myReader["JsonData"];
                            var listShowRatingParsed = JsonConvert.DeserializeObject<List<UserRating>>(listShowRating);
                            if (listShowRatingParsed != null)
                            {
                                return
                                    listShowRatingParsed.FirstOrDefault(
                                        x => showData.Ids.TraktId != null && x.TraktId == showData.Ids.TraktId.Value);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
            return null;
        }

        public List<UserRating> GetMovieRatingsByUser(string token)
        {
            try
            {
                using (var db = new SqlHandler())
                {
                    var query = string.Format(@"
declare @UserId int
Select @UserId = Id from tbl_Users where AccessToken = '{0}'
Select JsonData from tbl_UserMovieRating where UserId=@UserId", token);
                    var myCommand = new SqlCommand(query,
                        db.MyConnection);
                    var list = new List<UserRating>();
                    using (var myReader = myCommand.ExecuteReader())
                    {
                        while (myReader.Read())
                        {
                            var jsonData = (string) myReader["JsonData"];
                            list = JsonConvert.DeserializeObject<List<UserRating>>(jsonData);
                        }
                        return list;
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public UserRating GetMovieRatingByUser(string token, Movie movieData)
        {
            try
            {
                using (var db = new SqlHandler())
                {
                    var query = string.Format(@"
declare @UserId int
Select @UserId = Id from tbl_Users where AccessToken = '{0}'
Select JsonData from tbl_UserMovieRating where UserId=@UserId", token);
                    var myCommand = new SqlCommand(query,
                        db.MyConnection);
                    using (var myReader = myCommand.ExecuteReader())
                    {
                        while (myReader.Read())
                        {
                            var jsonData = (string) myReader["JsonData"];
                            var jsonDataConv = JsonConvert.DeserializeObject<UserRating>(jsonData);
                            return jsonDataConv;
                        }
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
            return null;
        }


        public List<UserRating> GetEpisodeRatingsByUser(string token)
        {
            try
            {
                using (var db = new SqlHandler())
                {
                    var query = string.Format(@"
declare @UserId int
Select @UserId = Id from tbl_Users where AccessToken = '{0}'
Select JsonData from tbl_UserEpisodeRating where UserId=@UserId", token);
                    var myCommand = new SqlCommand(query,
                        db.MyConnection);
                    var list = new List<UserRating>();
                    using (var myReader = myCommand.ExecuteReader())
                    {
                        while (myReader.Read())
                        {
                            var rating = (string) myReader["JsonData"];
                            list = JsonConvert.DeserializeObject<List<UserRating>>(rating);
                        }
                        return list;
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        #region GetQueueMethods

        public Dictionary<UserRating, string> GetShowRatingsQueue(string token)
        {
            try
            {
                using (var db = new SqlHandler())
                {
                    var query = string.Format(@"
declare @UserId int
Select @UserId = Id from tbl_Users where TraktAccessToken = '{0}'
IF @UserId is null
  BEGIN 
     Select @UserId = Id from tbl_Users Where [AccessToken] = '{0}'
  END 
select CommitTraktJson,TraktId,Rating from tbl_UserShowRatingQueue where UserId =@UserId", token);
                    var myCommand = new SqlCommand(query,
                        db.MyConnection);
                    var list = new Dictionary<UserRating, string>();
                    using (var myReader = myCommand.ExecuteReader())
                    {
                        while (myReader.Read())
                        {
                            var commit = (string) myReader["CommitTraktJson"];
                            var traktId = (int) myReader["TraktId"];
                            var rating = (int) myReader["Rating"];
                            var userRating = new UserRating {TraktId = traktId, Rating = rating};
                            list.Add(userRating, commit);
                        }
                        return list;
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Dictionary<UserRating, string> GetEposideRatingsQueue(string token)
        {
            try
            {
                using (var db = new SqlHandler())
                {
                    var query = string.Format(@"
declare @UserId int
Select @UserId = Id from tbl_Users where TraktAccessToken = '{0}'
IF @UserId is null
  BEGIN 
     Select @UserId = Id from tbl_Users Where [AccessToken] = '{0}'
  END 
select TraktCommitJson,ShowTraktId,Rating,Season,Episode from tbl_UserEpisodeRatingQueue where UserId =@UserId", token);
                    var myCommand = new SqlCommand(query,
                        db.MyConnection);
                    var list = new Dictionary<UserRating, string>();
                    using (var myReader = myCommand.ExecuteReader())
                    {
                        while (myReader.Read())
                        {
                            var commit = (string) myReader["TraktCommitJson"];
                            var traktId = (int) myReader["ShowTraktId"];
                            var rating = (int) myReader["Rating"];
                            var season = (int) myReader["Season"];
                            var episode = (int) myReader["Episode"];
                            var userRating = new UserRating
                            {
                                TraktId = traktId,
                                Rating = rating,
                                Episode = episode,
                                Season = season
                            };
                            list.Add(userRating, commit);
                        }
                        return list;
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Dictionary<UserRating, string> GetMovieRatingsQueue(string token)
        {
            try
            {
                using (var db = new SqlHandler())
                {
                    var query = string.Format(@"
declare @UserId int
IF @UserId is null
  BEGIN 
     Select @UserId = Id from tbl_Users Where [AccessToken] = '{0}'
  END 
Select @UserId = Id from tbl_Users where TraktAccessToken = '{0}'
select TraktCommitJson,TraktId,Rating from tbl_UserMovieRatingQueue where UserId =@UserId", token);
                    var myCommand = new SqlCommand(query,
                        db.MyConnection);
                    var list = new Dictionary<UserRating, string>();
                    using (var myReader = myCommand.ExecuteReader())
                    {
                        while (myReader.Read())
                        {
                            var commit = (string) myReader["TraktCommitJson"];
                            var traktId = (int) myReader["TraktId"];
                            var rating = (int) myReader["Rating"];
                            var userRating = new UserRating {TraktId = traktId, Rating = rating};
                            list.Add(userRating, commit);
                        }
                        return list;
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void DeleteRatingQueue(string token)
        {
            try
            {
                using (var db = new SqlHandler())
                {
                    var query = string.Format(@"
declare @UserId int
Select @UserId = Id from tbl_Users where TraktAccessToken = '{0}'
Delete from tbl_UserShowRatingQueue where UserId = @UserId
", token);
                    var myCommand = new SqlCommand(query,
                        db.MyConnection);
                    myCommand.ExecuteNonQuery();

                     query = string.Format(@"
declare @UserId int
Select @UserId = Id from tbl_Users where TraktAccessToken = '{0}'
Delete from tbl_UserMovieRatingQueue where UserId = @UserId
", token);

                    myCommand = new SqlCommand(query,
                            db.MyConnection);
                    myCommand.ExecuteNonQuery();

                    query = string.Format(@"
declare @UserId int
Select @UserId = Id from tbl_Users where TraktAccessToken = '{0}'
Delete from tbl_UserEpisodeRatingQueue where UserId = @UserId
", token);

                    myCommand = new SqlCommand(query,
                            db.MyConnection);
                    myCommand.ExecuteNonQuery();
                }

            }
            catch (Exception)
            {
            }
        }

        public List<SetAsSeenJson> GetWatchedEpisodesQueue(string token)
        {
            try
            {
                using (var db = new SqlHandler())
                {
                    var query = string.Format(@"
declare @UserId int
Select @UserId = Id from tbl_Users where TraktAccessToken = '{0}'
select TraktCommitJson from tbl_UserShowWatchedQueue where UserId =@UserId", token);
                    var myCommand = new SqlCommand(query,
                        db.MyConnection);
                    var list = new List<SetAsSeenJson>();
                    using (var myReader = myCommand.ExecuteReader())
                    {
                        while (myReader.Read())
                        {
                            var commit = (string) myReader["TraktCommitJson"];
                            var commitParsed = JsonConvert.DeserializeObject<SetAsSeenJson>(commit);
                            list.Add(commitParsed);
                        }
                        return list;
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }

            #endregion

        }

        public List<SetAsSeenJson> GetWatchedMoviesQueue(string token)
        {
            try
            {
                using (var db = new SqlHandler())
                {
                    var query = string.Format(@"
declare @UserId int
Select @UserId = Id from tbl_Users where TraktAccessToken = '{0}'
select TraktCommitJson from tbl_UserMovieWatchedQueue where UserId =@UserId", token);
                    var myCommand = new SqlCommand(query,
                        db.MyConnection);
                    var list = new List<SetAsSeenJson>();
                    using (var myReader = myCommand.ExecuteReader())
                    {
                        while (myReader.Read())
                        {
                            var commit = (string) myReader["TraktCommitJson"];
                            var commitParsed = JsonConvert.DeserializeObject<SetAsSeenJson>(commit);
                            list.Add(commitParsed);
                        }
                        return list;
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void DeleteWatchedQueue(string token)
        {
    
                using (var db = new SqlHandler())
                {
                    var query = string.Format(@"
declare @UserId int
Select @UserId = Id from tbl_Users where TraktAccessToken = '{0}'
Delete from tbl_UserShowWatchedQueue where UserId = @UserId and Watched = 1
", token);
                    var myCommand = new SqlCommand(query,
                        db.MyConnection);
                    myCommand.ExecuteNonQuery();

                }
                using (var db2 = new SqlHandler())
                {
                    var query2 = string.Format(@"
declare @UserId int
Select @UserId = Id from tbl_Users where TraktAccessToken = '{0}'
Delete from tbl_UserMovieWatchedQueue where UserId = @UserId and Watched = 1
", token);
                    var myCommand2 = new SqlCommand(query2,
                        db2.MyConnection);
                    myCommand2.ExecuteNonQuery();
                }
            

        }

        public List<CommentRequestJsonDto> GetMovieComments(string token)
        {
            try
            {
                using (var db = new SqlHandler())
                {
                    var query = string.Format(@"
declare @UserId int
Select @UserId = Id from tbl_Users where TraktAccessToken = '{0}'
select CommentId, TraktCommitJson from tbl_UserMovieCommentsQueue where UserId =@UserId and TraktSynced = 0", token);
                    var myCommand = new SqlCommand(query,
                        db.MyConnection);
                    var list = new List<CommentRequestJsonDto>();
                    using (var myReader = myCommand.ExecuteReader())
                    {
                        while (myReader.Read())
                        {
                            var commit = (string)myReader["TraktCommitJson"];
                            var commentId = (int)myReader["CommentId"];
                            var commitParsed = JsonConvert.DeserializeObject<CommentRequestJsonDto>(commit);
                            commitParsed.CommentId = commentId;
                            list.Add(commitParsed);
                        }
                        return list;
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<CommentRequestJsonDto> GetShowComments(string token)
        {
            try
            {
                using (var db = new SqlHandler())
                {
                    var query = string.Format(@"
declare @UserId int
Select @UserId = Id from tbl_Users where TraktAccessToken = '{0}'
select  CommentId, TraktCommitJson from tbl_UserShowCommentsQueue where UserId =@UserId  and TraktSynced = 0", token);
                    var myCommand = new SqlCommand(query,
                        db.MyConnection);
                    var list = new List<CommentRequestJsonDto>();
                    using (var myReader = myCommand.ExecuteReader())
                    {
                        while (myReader.Read())
                        {
                            var commit = (string)myReader["TraktCommitJson"];
                            var commentId = (int)myReader["CommentId"];
                            var commitParsed = JsonConvert.DeserializeObject<CommentRequestJsonDto>(commit);
                            commitParsed.CommentId = commentId;
                            list.Add(commitParsed);
                        }
                        return list;
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public string GetTraktTokenByShiftvToken(string token)
        {
            try
            {
                using (var db = new SqlHandler())
                {
                    var query = string.Format(@"
Select TraktAccessToken from tbl_Users where AccessToken = '{0}'", token);
                    var myCommand = new SqlCommand(query,
                        db.MyConnection);
                    using (var myReader = myCommand.ExecuteReader())
                    {
                        while (myReader.Read())
                        {
                            var commit = (string)myReader["TraktAccessToken"];
                            return commit;
                        }
                        return null;
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void DeleteMovieComment(CommentRequestJsonDto commentRequestJsonDto, string token)
        {
   
            try
            {
                using (var db = new SqlHandler())
                {
                    var query = string.Format(@"
declare @UserId int
Select @UserId = Id from tbl_Users where TraktAccessToken = '{0}'
update tbl_UserMovieCommentsQueue set TraktSynced = 1  where UserId = @UserId and MovieTraktId = '{1}' and CommentId = '{2}'
", token, commentRequestJsonDto.Movie.Ids.TraktId.Value, commentRequestJsonDto.CommentId.Value);
                    var myCommand = new SqlCommand(query,
                        db.MyConnection);
                    myCommand.ExecuteNonQuery();

                }
            }
            catch (Exception)
            {
            }
        }

        public void DeleteShowComment(CommentRequestJsonDto commentRequestJsonDto, string token)
        {
            try
            {
                using (var db = new SqlHandler())
                {
                    var query = string.Format(@"
declare @UserId int
Select @UserId = Id from tbl_Users where TraktAccessToken = '{0}'
update tbl_UserShowCommentsQueue set TraktSynced = 1 where  UserId = @UserId and ShowTraktId = '{1}' and CommentId = '{2}'
", token, commentRequestJsonDto.Show.Ids.TraktId.Value, commentRequestJsonDto.CommentId.Value);
                    var myCommand = new SqlCommand(query,
                        db.MyConnection);
                    myCommand.ExecuteNonQuery();

                }
            }
            catch (Exception)
            {
            }
        }

        public void DeleteUnwatchedQueue(string token)
        {
            try
            {
                using (var db = new SqlHandler())
                {
                    var query = string.Format(@"
declare @UserId int
Select @UserId = Id from tbl_Users where TraktAccessToken = '{0}'
Delete from tbl_UserShowWatchedQueue where UserId = @UserId and Watched = 0
", token);
                    var myCommand = new SqlCommand(query,
                        db.MyConnection);
                    myCommand.ExecuteNonQuery();

                }
                using (var db2 = new SqlHandler())
                {
                    var query2 = string.Format(@"
declare @UserId int
Select @UserId = Id from tbl_Users where TraktAccessToken = '{0}'
Delete from tbl_UserMovieWatchedQueue where UserId = @UserId and Watched = 0
", token);
                    var myCommand2 = new SqlCommand(query2,
                        db2.MyConnection);
                    myCommand2.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
            }
        }
    }
}