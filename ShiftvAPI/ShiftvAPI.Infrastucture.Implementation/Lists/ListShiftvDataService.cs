using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ShiftvAPI.Contracts.Data;
using ShiftvAPI.Contracts.Infrastucture.Shiftv;
using ShiftvAPI.Infrastucture.Implementation.Helpers;

namespace ShiftvAPI.Infrastucture.Implementation.Lists
{
    class ListShiftvDataService : IListShiftvDataService
    {
        public async Task<TraktList> GetList(string username, string id)
        {
            try
            {
                using (var db = new SqlHandler())
                {
                    var query = string.Format("Select JsonData, LastUpdate from tbl_Lists with(nolock) where [id]='{0}' AND [user]='{1}' AND [type]='{2}'", id, username, "list");
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
                                return JsonConvert.DeserializeObject<TraktList>(jsonData);
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

        public async Task SaveList(TraktList listData, string username, string id)
        {
            using (var db = new SqlHandler())
            {
                var filename =
                    await
                        AzureBlobStorageHandler.SaveFileToAzure(JsonConvert.SerializeObject(listData),
                            string.Format("{0}{1}", id, username), BackupContainerTypes.Lists);
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
                cmd.Parameters.AddWithValue("@user", username);
                cmd.Parameters.AddWithValue("@type", "list");
                cmd.Parameters.AddWithValue("@JsonData", filename);
                cmd.Parameters.AddWithValue("@LastUpdate", DateTime.Now);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
