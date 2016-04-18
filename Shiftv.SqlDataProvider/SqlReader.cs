using System;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Shiftv.SqlDataProvider;

namespace Quizzle.SqlDataProviders
{
    public class SqlReader : IReader
    {
        private readonly SqlDataReader _dr;
        private readonly SqlDatabase _db;

        public SqlReader(SqlDatabase db, SqlDataReader dr)
        {
            _db = db;
            _dr = dr;
        }

        public void Close()
        {
            _dr.Close();
            _db.Close();
        }

        public bool Read()
        {
            return _dr.Read();
        }

        public int GetInt(int i)
        {
            if (_dr.IsDBNull(i)) return 0;
            return _dr.GetInt32(i);
        }

        public int? GetIntNull(int i)
        {
            if (_dr.IsDBNull(i)) return null;
            return _dr.GetInt32(i);
        }

        public double GetDouble(int i)
        {
            if (_dr.IsDBNull(i)) return 0;
            return _dr.GetDouble(i);
        }

        public double? GetDoubleNull(int i)
        {
            if (_dr.IsDBNull(i)) return null;
            return _dr.GetDouble(i);
        }

        public decimal GetDecimal(int i)
        {
            if (_dr.IsDBNull(i)) return 0;
            return _dr.GetDecimal(i);
        }

        public decimal? GetDecimalNull(int i)
        {
            if (_dr.IsDBNull(i)) return null;
            return _dr.GetDecimal(i);
        }

        public string GetString(int i)
        {
            if (_dr.IsDBNull(i)) return "";
            return _dr.GetString(i);
        }

        public DateTime GetDateTime(int i)
        {
            if (_dr.IsDBNull(i)) return new DateTime(1800, 1, 1);
            return _dr.GetDateTime(i);
        }

        public DateTime? GetDateTimeNull(int i)
        {
            if (_dr.IsDBNull(i)) return null;
            return _dr.GetDateTime(i);
        }

        public bool GetBool(int i)
        {
            if (_dr.IsDBNull(i)) return false;
            return _dr.GetBoolean(i);
        }

        public bool? GetBoolNull(int i)
        {
            if (_dr.IsDBNull(i)) return null;
            return _dr.GetBoolean(i);
        }

        internal object GetObject(int p)
        {
            return null;
        }

        public Guid GetGuid(int i)
        {
            if (_dr.IsDBNull(i)) return new Guid();
            return _dr.GetGuid(i);
        }

        public Guid GetGuid(string fieldName)
        {
            return GetGuid(_dr.GetOrdinal(fieldName));
        }

        public Guid? GetGuidNull(int i)
        {
            if (_dr.IsDBNull(i)) return null;
            return _dr.GetGuid(i);
        }

        public int? GetIntNull(string fieldName)
        {
            return GetIntNull(_dr.GetOrdinal(fieldName));
        }

        public decimal? GetDecimalNull(string fieldName)
        {
            return GetDecimalNull(_dr.GetOrdinal(fieldName));
        }

        public decimal GetDecimal(string fieldName)
        {
            return GetDecimal(_dr.GetOrdinal(fieldName));
        }

        public int GetInt(string fieldName)
        {
            return GetInt(_dr.GetOrdinal(fieldName));
        }

        public string GetString(string fieldName)
        {
            return GetString(_dr.GetOrdinal(fieldName));
        }

        public byte[] GetBytes(int i, int bytesNo)
        {
            byte[] data = new byte[bytesNo];
            var bytesRead = _dr.GetBytes(i, 0, data, 0, bytesNo);
            return data;
        }

        public byte[] GetBytes(string fieldName, int bytesNo)
        {
            return GetBytes(_dr.GetOrdinal(fieldName), bytesNo);
        }

        public SqlXml GetXml(int i)
        {
            return _dr.GetSqlXml(i);
        }

        public SqlXml GetXml(string fieldName)
        {
            return GetXml(_dr.GetOrdinal(fieldName));
        }

        public int IndexOf(string fieldName)
        {
            try
            {
                return _dr.GetOrdinal(fieldName);
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public object Get(int i)
        {
            return _dr.GetValue(i);
        }

        public bool GetBool(string fieldName)
        {
            return GetBool(_dr.GetOrdinal(fieldName));
        }

        public bool? GetBoolNull(string fieldName)
        {
            return GetBoolNull(_dr.GetOrdinal(fieldName));
        }


        public double GetDouble(string fieldName)
        {
            return GetDouble(_dr.GetOrdinal(fieldName));
        }

        public DateTime GetDateTime(string fieldName)
        {
            return GetDateTime(_dr.GetOrdinal(fieldName));
        }

        public DateTime? GetDateTimeNull(string fieldName)
        {
            return GetDateTimeNull(_dr.GetOrdinal(fieldName));
        }

        public void Dispose()
        {
            Close();
        }
    }
}
