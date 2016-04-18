namespace Shiftv.Contracts.Domain.Peoples
{
    public class Crew : ICrew
    {
        public string Job { get; set; }
        public IPerson Person { get; set; }
    }
}