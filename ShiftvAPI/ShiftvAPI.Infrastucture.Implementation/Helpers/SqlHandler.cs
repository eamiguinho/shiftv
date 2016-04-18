using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Formatting = System.Xml.Formatting;

namespace ShiftvAPI.Infrastucture.Implementation.Helpers
{
    public class SqlHandler : IDisposable
    {
        public SqlConnection MyConnection = new SqlConnection("Server=tcp:v06528hyiz.database.windows.net,1433;Database=shiftvstaging;User ID=amiguinho@v06528hyiz;Password=25713423_Ee;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;");

        public SqlHandler()
        {
            Connect();
        }
        public void Connect()
        {
            try
            {
                MyConnection.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void ExecuteNonQuery(string query)
        {
            Connect();
            var myCommand = new SqlCommand(query, MyConnection);
            myCommand.ExecuteNonQueryAsync();
        }
        public IEnumerable<Dictionary<string, object>> Serialize(SqlDataReader reader)
        {
            var results = new List<Dictionary<string, object>>();
            var cols = new List<string>();
            for (var i = 0; i < reader.FieldCount; i++)
                cols.Add(reader.GetName(i));

            while (reader.Read())
                results.Add(SerializeRow(cols, reader));

            return results;
        }
        private Dictionary<string, object> SerializeRow(IEnumerable<string> cols,
                                                        SqlDataReader reader)
        {
            var result = new Dictionary<string, object>();
            foreach (var col in cols)
                result.Add(col, reader[col]);
            return result;
        }
       

        public void Dispose()
        {
            try
            {
                MyConnection.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}