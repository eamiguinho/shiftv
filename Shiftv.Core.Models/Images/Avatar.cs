﻿using Shiftv.Contracts.Domain.Images;

namespace Shiftv.Core.Models.Images
{
    public class Avatar : IAvatar
    {
        public string Full { get; set; }
        public string Medium { get; set; }
        public string Thumb { get; set; }
    }
}