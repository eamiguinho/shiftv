namespace Shiftv.Contracts.Domain.Images
{
    public interface IGlobalImageData
    {
        string Full { get; set; }

        string Medium { get; set; }

        string Thumb { get; set; }
    }
}