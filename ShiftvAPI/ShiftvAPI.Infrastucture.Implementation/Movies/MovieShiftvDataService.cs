using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ShiftvAPI.Contracts.Data;
using ShiftvAPI.Contracts.Infrastucture.Shiftv;
using ShiftvAPI.Infrastucture.Implementation.Helpers;

namespace ShiftvAPI.Infrastucture.Implementation.Movies
{
    class MovieShiftvDataService : IMovieShiftvDataService
    {
        public async Task<Movie> GetMovieById(int id)
        {
            try
            {
                using (var db = new SqlHandler())
                {
                    var query = string.Format("Select TraktId, JsonData, LastUpdate from tbl_Movies with(nolock) where TraktId='{0}'", id);
                    SqlCommand myCommand = new SqlCommand(query,
                        db.MyConnection);
                    using (var myReader = myCommand.ExecuteReader())
                    {
                        while (myReader.Read())
                        {
                            var filename = myReader["JsonData"].ToString();
                            if (!string.IsNullOrEmpty(filename))
                            {
                                var jsonData =
                                     await AzureBlobStorageHandler.GetFileFromAzure(filename, BackupContainerTypes.Movies);
                                return JsonConvert.DeserializeObject<Movie>(jsonData);
                            }
                            else
                            {
                                return null;
                            }
                        }
                    }
                }
                return null;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task SaveMovie(Movie movieData)
        {
            using (var db = new SqlHandler())
            {
                var filename = await AzureBlobStorageHandler.SaveFileToAzure(JsonConvert.SerializeObject(movieData), movieData.Ids.ImdbId, BackupContainerTypes.Movies);
                SqlCommand cmd = new SqlCommand(@"
                    IF (NOT EXISTS(SELECT null FROM tbl_Movies WHERE TraktId = @TraktId)) 
                    BEGIN 
                        INSERT INTO tbl_Movies (TraktId, Title, JsonData, LastUpdate) 
                            VALUES (@TraktId,@Title, @JsonData, @LastUpdate)
                    END 
                    ELSE 
                    BEGIN 
                        UPDATE tbl_Movies 
                        SET Title = @Title, JsonData = @JsonData, LastUpdate = @LastUpdate
                        WHERE TraktId = @TraktId
                    END 
              ", db.MyConnection);
                cmd.Parameters.AddWithValue("@TraktId", movieData.Ids.TraktId);
                cmd.Parameters.AddWithValue("@Title", movieData.Title);
                cmd.Parameters.AddWithValue("@JsonData", filename);
                cmd.Parameters.AddWithValue("@LastUpdate", DateTime.Parse(movieData.UpdatedAt));

                cmd.ExecuteNonQuery();
            }
        }

        public async Task<List<MiniMovie>> GetTrending(int page, int limit)
        {
            try
            {
                using (var db = new SqlHandler())
                {
                    var query = string.Format("Select JsonData, LastUpdate from tbl_Lists with(nolock) where [id]='{0}' AND [user]='{1}' AND [type]='{2}'", "trending", "global", "movie");
                    SqlCommand myCommand = new SqlCommand(query,
                        db.MyConnection);
                    using (var myReader = myCommand.ExecuteReader())
                    {
                        while (myReader.Read())
                        {
                            var filename = myReader["JsonData"].ToString();
                            var lastUpdate = myReader["LastUpdate"].ToString();
                            var parsedData = DateTime.Parse(lastUpdate);
                            if ((DateTime.Now - parsedData).TotalDays > 1) return null;
                            if (!string.IsNullOrEmpty(filename))
                            {
                                var jsonData =
                                     await AzureBlobStorageHandler.GetFileFromAzure(filename, BackupContainerTypes.Lists);
                                return JsonConvert.DeserializeObject<List<MiniMovie>>(jsonData);
                            }
                            else
                            {
                                return null;
                            }
                        }
                    }
                }
                return null;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task SaveTrending(List<MiniMovie> listMini)
        {
            await SaveList(listMini, "trending", "global", "movie");
        }

        public async Task<List<MiniMovie>> GetPopular(int page, int limit)
        {
            try
            {
                using (var db = new SqlHandler())
                {
                    var query = string.Format("Select JsonData, LastUpdate from tbl_Lists with(nolock) where [id]='{0}' AND [user]='{1}' AND [type]='{2}'", "popular", "global", "movie");
                    SqlCommand myCommand = new SqlCommand(query,
                        db.MyConnection);
                    using (var myReader = myCommand.ExecuteReader())
                    {
                        while (myReader.Read())
                        {
                            var filename = myReader["JsonData"].ToString();
                            if (!string.IsNullOrEmpty(filename))
                            {
                                var jsonData =
                                     await AzureBlobStorageHandler.GetFileFromAzure(filename, BackupContainerTypes.Lists);
                                return JsonConvert.DeserializeObject<List<MiniMovie>>(jsonData);
                            }
                            else
                            {
                                return null;
                            }
                        }
                    }
                }
                return null;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task SavePopular(List<MiniMovie> listMini)
        {
            await SaveList(listMini, "popular", "global", "movie");
        }

        public DateTime? GetLastUpdate()
        {
            try
            {
                using (var db = new SqlHandler())
                {
                    var query = string.Format("Select LastUpdate from tbl_Updates with(nolock) where [type]='{0}'", "movie");
                    SqlCommand myCommand = new SqlCommand(query,
                        db.MyConnection);
                    using (var myReader = myCommand.ExecuteReader())
                    {
                        while (myReader.Read())
                        {
                            var lastDate = myReader["LastUpdate"].ToString();
                            var parsed = DateTime.Parse(lastDate);
                            return parsed;
                        }
                    }
                }
                return null;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public void SaveLastUpdate(DateTime date)
        {
            using (var db = new SqlHandler())
            {
                SqlCommand cmd = new SqlCommand(@"
                    IF (NOT EXISTS(SELECT null FROM tbl_Updates WHERE [type]=@type)) 
                    BEGIN 
                        INSERT INTO tbl_Updates ([type], LastUpdate) 
                            VALUES (@type, @LastUpdate)
                    END 
                    ELSE 
                    BEGIN 
                        UPDATE tbl_Updates 
                        SET LastUpdate = @LastUpdate
                        WHERE [type] = @type
                    END 
              ", db.MyConnection);
                cmd.Parameters.AddWithValue("@type", "movie");
                cmd.Parameters.AddWithValue("@LastUpdate", date);
                cmd.ExecuteNonQuery();
            }
        }

        public async Task<People> GetPeople(int movieId)
        {
            try
            {
                using (var db = new SqlHandler())
                {
                    var query = string.Format("Select JsonData, LastUpdate from tbl_People with(nolock) where [TraktId]='{0}'", movieId);
                    var myCommand = new SqlCommand(query,
                        db.MyConnection);
                    using (var myReader = myCommand.ExecuteReader())
                    {
                        while (myReader.Read())
                        {
                            var filename = myReader["JsonData"].ToString();
                            if (string.IsNullOrEmpty(filename)) return null;
                            var jsonData =
                                await AzureBlobStorageHandler.GetFileFromAzure(filename, BackupContainerTypes.People);
                            return JsonConvert.DeserializeObject<People>(jsonData);
                        }
                    }
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task SavePeople(People peopleData, int movieId)
        {
            using (var db = new SqlHandler())
            {
                var filename =
                    await
                        AzureBlobStorageHandler.SaveFileToAzure(JsonConvert.SerializeObject(peopleData),
                            string.Format("{0}", movieId), BackupContainerTypes.People);
                SqlCommand cmd = new SqlCommand(@"
                    IF (NOT EXISTS(SELECT null FROM tbl_People WHERE [TraktId] = @TraktId)) 
                    BEGIN 
                        INSERT INTO tbl_People ([TraktId], JsonData, LastUpdate) 
                            VALUES (@TraktId, @JsonData, @LastUpdate)
                    END 
                    ELSE 
                    BEGIN 
                        UPDATE tbl_People 
                        SET JsonData = @JsonData, LastUpdate = @LastUpdate
                        WHERE [TraktId] = @TraktId
                    END 
              ", db.MyConnection);
                cmd.Parameters.AddWithValue("@TraktId", movieId);
                cmd.Parameters.AddWithValue("@JsonData", filename);
                cmd.Parameters.AddWithValue("@LastUpdate", DateTime.Now);
                cmd.ExecuteNonQuery();
            }
        }

        public async Task<List<MiniMovie>> Search(string key)
        {
            try
            {
                using (var db = new SqlHandler())
                {
                    var query = string.Format("Select JsonData from tbl_Movies with(nolock) where [Title] like '%{0}%'", key);
                    var myCommand = new SqlCommand(query,
                        db.MyConnection);
                    var list = new List<MiniMovie>();
                    using (var myReader = myCommand.ExecuteReader())
                    {
                        while (myReader.Read())
                        {
                            var filename = myReader["JsonData"].ToString();
                            if (string.IsNullOrEmpty(filename)) return null;
                            var jsonData =
                                await AzureBlobStorageHandler.GetFileFromAzure(filename, BackupContainerTypes.Movies);
                            var x = JsonConvert.DeserializeObject<Movie>(jsonData);
                            list.Add(new MiniMovie
                            {
                                Fanart = x.Images.Fanart,
                                Ids = x.Ids,
                                Rating = x.Rating,
                                Title = x.Title,
                                Votes = x.Votes,
                                Genres = x.Genres,
                                Runtime = x.Runtime
                            });
                        }
                        return list.Count > 0 ? list : null;
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<List<Comment>> GetComments(int movieId)
        {
            try
            {
                using (var db = new SqlHandler())
                {
                    var query = string.Format(@"select CommentId, LastUpdated, Comment, IsSpoil, IsReview, u.JsonData from tbl_UserMovieCommentsQueue usc
join tbl_Users u on u.Id = usc.UserId
where usc.MovieTraktId = {0} and TraktSynced = 0", movieId);
                    var myCommand = new SqlCommand(query,
                        db.MyConnection);
                    var listComments = new List<Comment>();
                    using (var myReader = myCommand.ExecuteReader())
                    {
                        while (myReader.Read())
                        {
                            var filename = myReader["JsonData"].ToString();
                            var commentText = myReader["Comment"].ToString();
                            var commentId = myReader.GetInt32(0);
                            var commentDate = myReader["LastUpdated"].ToString();
                            var isReview = myReader.GetBoolean(4);
                            var isSpoil = myReader.GetBoolean(3);
                            if (string.IsNullOrEmpty(filename)) return null;
                            var jsonData =
                               await AzureBlobStorageHandler.GetFileFromAzure(filename, BackupContainerTypes.UserSettings);
                            var user = JsonConvert.DeserializeObject<TokenResult>(jsonData);
                            var comment = new Comment
                            {
                                CommentText = commentText,
                                CreatedAt = commentDate,
                                Id = commentId,
                                Review = isReview,
                                Spoiler = isSpoil,
                                User = user.UserSettings.User
                            };
                            listComments.Add(comment);
                        }
                        return listComments;
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void UpdateWatchlist(List<MiniMovie> list, string token)
        {
            try
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
                    IF (NOT EXISTS(SELECT null FROM tbl_UserMovieWatchlist WHERE [UserId] = @UserId)) 
                    BEGIN 
                        INSERT INTO tbl_UserMovieWatchlist (UserId, LastUpdate, JsonData) 
                            VALUES (@UserId, @LastUpdate,@JsonData)
                    END 
                    ELSE 
                    BEGIN 
                        UPDATE tbl_UserMovieWatchlist
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
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public List<MiniMovie> GetUserWatchlist(string token)
        {
            try
            {
                using (var db = new SqlHandler())
                {
                    var query = string.Format(@"
    declare @UserId int 
                    Select @UserId = Id from tbl_Users Where [AccessToken] = '{0}'
select JsonData from tbl_UserMovieWatchlist
where UserId =  @UserId", token);
                    var myCommand = new SqlCommand(query,
                        db.MyConnection);
                    var watchlist = new List<MiniMovie>();
                    using (var myReader = myCommand.ExecuteReader())
                    {
                        while (myReader.Read())
                        {
                            var filename = myReader["JsonData"].ToString();
                            var parsed = JsonConvert.DeserializeObject<List<MiniMovie>>(filename);
                            watchlist.AddRange(parsed);
                        }
                        return watchlist;
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
        }


        private static async Task SaveList(object listMini, string id, string user, string type)
        {
            using (var db = new SqlHandler())
            {
                var filename =
                    await
                        AzureBlobStorageHandler.SaveFileToAzure(JsonConvert.SerializeObject(listMini),
                            string.Format("{0}{1}{2}", id, user,type), BackupContainerTypes.Lists);
                SqlCommand cmd = new SqlCommand(@"
                    IF (NOT EXISTS(SELECT null FROM tbl_Lists WHERE [id] = @id and [user] = @user and [type]=@type)) 
                    BEGIN 
                        INSERT INTO tbl_Lists ([id], [user], [type], JsonData, LastUpdate) 
                            VALUES (@id, @user, @type, @JsonData, @LastUpdate)
                    END 
                    ELSE 
                    BEGIN 
                        UPDATE tbl_Lists 
                        SET JsonData = @JsonData, LastUpdate = @LastUpdate
                        WHERE [id] = @id AND [user] = @user AND [type] = @type
                    END 
              ", db.MyConnection);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@user", user);
                cmd.Parameters.AddWithValue("@type", type);
                cmd.Parameters.AddWithValue("@JsonData", filename);
                cmd.Parameters.AddWithValue("@LastUpdate", DateTime.Now);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
