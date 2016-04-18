namespace Shiftv.Contracts.Domain.Peoples
{
    public interface IActor
    {
         string Name { get; set; }
         string Character { get; set; }
         IPeopleImage Image { get; set; }
    }
}
