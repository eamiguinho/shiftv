using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ShiftvAPI.Contracts.Data;
using ShiftvAPI.Contracts.Data.Converters;
using ShiftvAPI.Contracts.Data.Sync;
using ShiftvAPI.Contracts.Infrastucture.Shiftv;
using ShiftvAPI.Infrastucture.Implementation.Helpers;

namespace ShiftvAPI.Infrastucture.Implementation.Shows
{
    public class ShowShiftvDataService : IShowShiftvDataService
    {
        public async Task<Show> GetShowById(int id)
        {
            try
            {
                using (var db = new SqlHandler())
                {
                    var query = string.Format("Select TraktId, JsonData, LastUpdate from tbl_Shows with(nolock) where TraktId='{0}'", id);
                    SqlCommand myCommand = new SqlCommand(query,
                        db.MyConnection);
                    using (var myReader = myCommand.ExecuteReader())
                    {
                        while (myReader.Read())
                        {
                            var json = myReader["JsonData"].ToString();
                            if (!string.IsNullOrEmpty(json))
                            {
                                return JsonConvert.DeserializeObject<Show>(json);
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

        public  void SaveShow(Show showData)
        {
            try
            {
                using (var db = new SqlHandler())
                {
                    SqlCommand cmd = new SqlCommand(@"
                    IF (NOT EXISTS(SELECT null FROM tbl_Shows WHERE TraktId = @TraktId)) 
                    BEGIN 
                        INSERT INTO tbl_Shows (TraktId, Title, JsonData, MiniJsonData, LastUpdate, Genres, Status) 
                            VALUES (@TraktId,@Title, @JsonData, @MiniJsonData, @LastUpdate, @Genres, @Status)
                    END 
                    ELSE 
                    BEGIN 
                        UPDATE tbl_Shows 
                        SET Title = @Title, JsonData = @JsonData, LastUpdate = @LastUpdate, MiniJsonData = @MiniJsonData, Status = @Status, Genres = @Genres
                        WHERE TraktId = @TraktId
                    END 
              ", db.MyConnection);
                    cmd.Parameters.AddWithValue("@TraktId", showData.Ids.TraktId);
                    cmd.Parameters.AddWithValue("@Title", showData.Title);
                    cmd.Parameters.AddWithValue("@JsonData", JsonConvert.SerializeObject(showData));
                    cmd.Parameters.AddWithValue("@MiniJsonData", JsonConvert.SerializeObject(ShowMiniConverter.Convert(showData)));
                    cmd.Parameters.AddWithValue("@LastUpdate", DateTime.Parse(showData.UpdatedAt));
                    cmd.Parameters.AddWithValue("@Genres", showData.Genres != null ? string.Join(",", showData.Genres.ToArray()) : "");
                    cmd.Parameters.AddWithValue("@Status", showData.Status != "ended");
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void SaveSeason(int traktId, List<Season> seasons)
        {
            using (var db = new SqlHandler())
            {
                foreach (var season in seasons)
                {
                    SqlCommand cmd = new SqlCommand(@"
                    IF (NOT EXISTS(SELECT null FROM tbl_Seasons WHERE ShowTraktId = @ShowTraktId and [SeasonNumber] = @SeasonNumber)) 
                    BEGIN 
                        INSERT INTO tbl_Seasons (ShowTraktId, JsonData, LastUpdate,[SeasonNumber]) 
                            VALUES (@ShowTraktId, @JsonData, @LastUpdate,@SeasonNumber)
                    END 
                    ELSE 
                    BEGIN 
                        UPDATE tbl_Seasons 
                        SET JsonData = @JsonData, LastUpdate = @LastUpdate
                        WHERE ShowTraktId= @ShowTraktId and [SeasonNumber] = @SeasonNumber
                    END 
              ", db.MyConnection);
                    cmd.Parameters.AddWithValue("@ShowTraktId", traktId);
                    cmd.Parameters.AddWithValue("@JsonData", JsonConvert.SerializeObject(season));
                    cmd.Parameters.AddWithValue("@LastUpdate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@SeasonNumber", season.Number);
                    cmd.ExecuteNonQuery();
                }
            
            }
        }

        public async Task<List<MiniShow>> GetTrending(int page, int limit)
        {
            try
            {
                using (var db = new SqlHandler())
                {
                    var query = string.Format("Select JsonData, LastUpdate from tbl_Lists with(nolock) where [id]='{0}' AND [user]='{1}' AND [type]='{2}'", "trending", "global", "show");
                    SqlCommand myCommand = new SqlCommand(query,
                        db.MyConnection);
                    using (                                                                                                                                                                                 var myReader = myCommand.ExecuteReader())
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
                                return JsonConvert.DeserializeObject<List<MiniShow>>(jsonData);
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

        public async Task SaveTrending(List<MiniShow> listMini)
        {
            await SaveList(listMini, "trending", "global", "show");
        }

        public async Task<List<MiniShow>> GetPopular(int page, int limit)
        {
            try
            {
                using (var db = new SqlHandler())
                {
                    var query = string.Format("Select JsonData, LastUpdate from tbl_Lists with(nolock) where [id]='{0}' AND [user]='{1}' AND [type]='{2}'", "popular", "global", "show");
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
                                return JsonConvert.DeserializeObject<List<MiniShow>>(jsonData);
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

        public async Task SavePopular(List<MiniShow> listMini)
        {
            await SaveList(listMini, "popular", "global", "show");
        }

        public DateTime? GetLastUpdate()
        {
            try
            {
                using (var db = new SqlHandler())
                {
                    var query = string.Format("Select LastUpdate from tbl_Updates with(nolock) where [type]='{0}'", "show");
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
            try
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
                    cmd.Parameters.AddWithValue("@type", "show");
                    cmd.Parameters.AddWithValue("@LastUpdate", date);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
        }
        }

        public async Task<People> GetPeople(int showId)
        {
            try
            {
                using (var db = new SqlHandler())
                {
                    var query = string.Format("Select JsonData, LastUpdate from tbl_People with(nolock) where [TraktId]='{0}'", showId);
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

        public async Task SavePeople(People peopleData, int showId)
        {
            using (var db = new SqlHandler())
            {
                var filename =
                    await
                        AzureBlobStorageHandler.SaveFileToAzure(JsonConvert.SerializeObject(peopleData),
                            string.Format("{0}", showId), BackupContainerTypes.People);
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
                cmd.Parameters.AddWithValue("@TraktId", showId);
                cmd.Parameters.AddWithValue("@JsonData", filename);
                cmd.Parameters.AddWithValue("@LastUpdate", DateTime.Now);
                cmd.ExecuteNonQuery();
            }
        }

        public async Task<List<MiniShow>> Search(string key)
        {
            try
            {
                using (var db = new SqlHandler())
                {
                    var query = string.Format("Select JsonData from tbl_Shows with(nolock) where [Title] like '%{0}%'", key);
                    var myCommand = new SqlCommand(query,
                        db.MyConnection);
                    var list = new List<MiniShow>();
                    using (var myReader = myCommand.ExecuteReader())
                    {
                        while (myReader.Read())
                        {
                            var filename = myReader["JsonData"].ToString();
                            if (string.IsNullOrEmpty(filename)) return null;
                            var jsonData =
                                await AzureBlobStorageHandler.GetFileFromAzure(filename, BackupContainerTypes.Shows);
                            var x = JsonConvert.DeserializeObject<Show>(jsonData);
                            list.Add(new MiniShow
                            {
                                Fanart = x.Images.Fanart,
                                Ids = x.Ids,
                                Network = x.Network,
                                Rating = x.Rating,
                                Title = x.Title,
                                Votes = x.Votes,
                                Year = x.Year,
                                Genres = x.Genres,
                                Status = x.Status,
                                FirstAired = x.FirstAired,
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

        public List<Season> GetSeasons(int id)
        {
            try
            {
                using (var db = new SqlHandler())
                {
                    var query = string.Format("Select JsonData, LastUpdate from tbl_Seasons with(nolock) where [ShowTraktId]='{0}'", id);
                    var myCommand = new SqlCommand(query,
                        db.MyConnection);
                    using (var myReader = myCommand.ExecuteReader())
                    {
                        var list = new List<Season>();
                        while (myReader.Read())
                        {
                            var data = myReader["JsonData"].ToString();
                            var season = JsonConvert.DeserializeObject<Season>(data);
                            list.Add(season);
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

        public async Task<List<Comment>> GetComments(int showId)
        {
            try
            {
                using (var db = new SqlHandler())
                {
                    var query = string.Format(@"select CommentId, LastUpdated, Comment, IsSpoil, IsReview, u.JsonData from tbl_UserShowCommentsQueue usc
join tbl_Users u on u.Id = usc.UserId
where usc.ShowTraktId = {0} and TraktSynced = 0", showId);
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

        public Season GetLastSeason(int showId)
        {
            try
            {
                using (var db = new SqlHandler())
                {
                    var query = string.Format("select TOP 1 JsonData, LastUpdate from tbl_Seasons with(nolock) where [ShowTraktId]='{0}' order by SeasonNumber desc  ", showId);
                    var myCommand = new SqlCommand(query,
                        db.MyConnection);
                    using (var myReader = myCommand.ExecuteReader())
                    {
                        while (myReader.Read())
                        {
                            var data = myReader["JsonData"].ToString();
                            var season = JsonConvert.DeserializeObject<Season>(data);
                            return season;
                        }
                    }
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<List<MiniShow>> GetUserShows(string token, List<WatchedEpisodes> userShowsIds)
        {
            try
            {
                using (var db = new SqlHandler())
                {
                    var query = string.Format(@"select MiniJsonData from tbl_Shows where TraktId in ({0})", string.Join(",", userShowsIds.Select(x => x.TraktShowId).Distinct()));
                    SqlCommand myCommand = new SqlCommand(query,
                        db.MyConnection);
                    var list = new List<MiniShow>();
                    using (var myReader = myCommand.ExecuteReader())
                    {
                        while (myReader.Read())
                        {
                            var showTitle = myReader.GetString(0);
                            if (!string.IsNullOrEmpty(showTitle))
                            {
                                list.Add(JsonConvert.DeserializeObject<MiniShow>(showTitle));
                            }
                        }
                    }
                    return list;
                }
            }
            catch (Exception e)
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
                            string.Format("{0}{1}{2}", id, user, type), BackupContainerTypes.Lists);
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

    public class ShowDb
    {
        public string ImdbId { get; set; }
        public string JsonData { get; set; }
        public string LastUpdate { get; set; }
    }
}
