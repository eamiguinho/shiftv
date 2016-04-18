using Shiftv.Contracts.Domain.Images;

namespace Shiftv.Core.Models.Images
{
    public class Logo : ILogo
    {
        public string Full { get; set; }
        public string Medium { get; set; }
        public string Thumb { get; set; }
    }
}