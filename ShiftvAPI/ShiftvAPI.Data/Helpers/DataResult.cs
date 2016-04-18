namespace ShiftvAPI.Contracts.Helpers
{
    public class DataResult
    {
        public DataResult(StandardResults result)
        {
            Result = result;
        }

        public StandardResults Result { get; set; }

        public bool IsOk
        {
            get { return Result == StandardResults.Ok; }
        }

    }

    public class DataResult<TData> : DataResult
    {
        public DataResult(StandardResults result)
            : base(result)
        {
        }

        public DataResult(TData data)
            : base(StandardResults.Ok)
        {
            Data = data;
        }

        public TData Data { get; set; }
    }
}