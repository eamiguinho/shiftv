namespace Shiftv.Contracts.Domain.Peoples
{
    public interface ICast
    {
        string Character { get; set; }

        IPerson Person { get; set; }
    }
}