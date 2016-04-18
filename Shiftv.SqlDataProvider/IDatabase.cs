using System;
using Quizzle.SqlDataProviders;

namespace Shiftv.SqlDataProvider
{
    public interface IDatabase : IDisposable
    {
        string ConnectionString { get; set; }
        string Text { get; set; }
        string StoreProcedure { get; set; }
        void Open();
        void Close();
        IReader ExecuteReader();
        void ExecuteNonQuery();
        object ExecuteScalar();
        void Param(string name, object value);
        void Param(string name, Guid value);
        void BeginTransaction();
        void Commit();
        void Rollback();
        void NewCommand();
    }
}

