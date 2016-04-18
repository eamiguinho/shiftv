namespace Shiftv.Contracts.Domain.Peoples
{
    class Cast : ICast
    {
        public string Character { get; set; }
        public IPerson Person { get; set; }
    }
}