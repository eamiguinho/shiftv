using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ShiftvAPI.Contracts.Data;
using ShiftvAPI.Contracts.Infrastucture.Shiftv;
using ShiftvAPI.Infrastucture.Implementation.Helpers;

namespace ShiftvAPI.Infrastucture.Implementation.Login
{
    class LoginShiftvDataService : ILoginShiftvDataService
    {
        public async Task<string> SaveUserSettings(TokenResult userTraktInfo)
        {
            try
            {  

               var userExists = false;
                string shiftvAccessToken = null;
                using (var db = new SqlHandler())
                {
                    var query =
                        string.Format(
                            "Select username, [AccessToken] from tbl_Users with(nolock) where [Username]='{0}'",
                            userTraktInfo.UserSettings.User.Username);
                    SqlCommand myCommand = new SqlCommand(query,
                        db.MyConnection);
                  
                    using (var myReader = myCommand.ExecuteReader())
                    {
                        while (myReader.Read())
                        {
                            userExists = true;
                            shiftvAccessToken = myReader["AccessToken"].ToString();
                        }
                    }
                }
                if (userExists)
                    {
                        using (var db2 = new SqlHandler())
                        {
                                  var filename =
                     await
                         AzureBlobStorageHandler.SaveFileToAzure(JsonConvert.SerializeObject(userTraktInfo),
                             string.Format("{0}", userTraktInfo.UserSettings.User.Username), BackupContainerTypes.UserSettings);
                        SqlCommand cmd = new SqlCommand(@"
                        UPDATE tbl_Users 
                        SET TraktAccessToken = @TraktAccessToken, JsonData = @JsonData, LastUpdate = @LastUpdate, ExpireDate = @ExpireDate
                        WHERE [username] = @Username
              ", db2.MyConnection);
                        cmd.Parameters.AddWithValue("@Username", userTraktInfo.UserSettings.User.Username);
                        cmd.Parameters.AddWithValue("@TraktAccessToken", userTraktInfo.AccessToken);
                        cmd.Parameters.AddWithValue("@JsonData", filename);
                        cmd.Parameters.AddWithValue("@LastUpdate", DateTime.Now);
                        cmd.Parameters.AddWithValue("@ExpireDate", userTraktInfo.ExpiresAt);
                        cmd.ExecuteNonQuery();
                        return shiftvAccessToken;
                        }
                  
                    }
                    else
                    {
                        using (var db3 = new SqlHandler())
                        {
                              var newShiftvToken = Guid.NewGuid();
                                var filename =
                     await
                         AzureBlobStorageHandler.SaveFileToAzure(JsonConvert.SerializeObject(userTraktInfo),
                             string.Format("{0}", userTraktInfo.UserSettings.User.Username), BackupContainerTypes.UserSettings);
                        SqlCommand cmd = new SqlCommand(@"
                        INSERT INTO tbl_Users ([Username], [TraktAccessToken], [JsonData], [LastUpdate], [ExpireDate], [AccessToken]) 
                            VALUES (@Username, @TraktAccessToken, @JsonData, @LastUpdate, @ExpireDate, @AccessToken)
              ", db3.MyConnection);
                        cmd.Parameters.AddWithValue("@Username", userTraktInfo.UserSettings.User.Username);
                        cmd.Parameters.AddWithValue("@TraktAccessToken", userTraktInfo.AccessToken);
                        cmd.Parameters.AddWithValue("@JsonData", filename);
                        cmd.Parameters.AddWithValue("@LastUpdate", DateTime.Now);
                        cmd.Parameters.AddWithValue("@ExpireDate", userTraktInfo.ExpiresAt);
                        cmd.Parameters.AddWithValue("@AccessToken", newShiftvToken);
                        cmd.ExecuteNonQuery();
                              return newShiftvToken.ToString();
                        }
                      
                    }
                  
                
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}