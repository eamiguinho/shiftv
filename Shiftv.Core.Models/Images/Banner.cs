using Shiftv.Contracts.Domain.Images;

namespace Shiftv.Core.Models.Images
{
    public class Banner : IBanner
    {
        public string Full { get; set; }
        public string Medium { get; set; }
        public string Thumb { get; set; }
    }
}