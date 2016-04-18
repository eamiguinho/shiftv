using System;
using System.Data.SqlTypes;

namespace Shiftv.SqlDataProvider
{
    public interface IReader : IDisposable
    {
        void Close();
        bool Read();

        bool GetBool(int i);
        bool GetBool(string fieldName);

        bool? GetBoolNull(int i);
        bool? GetBoolNull(string fieldName);

        DateTime GetDateTime(int i);
        DateTime GetDateTime(string fieldName);

        DateTime? GetDateTimeNull(int i);
        DateTime? GetDateTimeNull(string fieldName);

        decimal GetDecimal(int i);
        decimal GetDecimal(string fieldName);

        decimal? GetDecimalNull(int i);
        decimal? GetDecimalNull(string fieldName);

        double GetDouble(int i);
        double GetDouble(string fieldName);

        double? GetDoubleNull(int i);

        Guid GetGuid(int i);
        Guid GetGuid(string fieldName);
        Guid? GetGuidNull(int i);

        int GetInt(int i);
        int GetInt(string fieldName);

        int? GetIntNull(int i);
        int? GetIntNull(string fieldName);

        string GetString(int i);
        string GetString(string fieldName);

        byte[] GetBytes(int i, int bytesNo);
        byte[] GetBytes(string p, int bytesNo);

        SqlXml GetXml(int i);
        SqlXml GetXml(string fieldName);

        int IndexOf(string fieldName);

        object Get(int i);
    }
}