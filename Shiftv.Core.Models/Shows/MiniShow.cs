using System;
using System.Collections.Generic;
using Shiftv.Contracts.Domain.Images;
using Shiftv.Contracts.Domain.Movies;
using Shiftv.Contracts.Domain.Shows;

namespace Shiftv.Core.Models.Shows
{
    class MiniShow : IMiniShow
    {
        public string Title { get; set; }
        public IIds Ids { get; set; }
        public string Network { get; set; }
        public double? Rating { get; set; }
        public int? Votes { get; set; }
        public int? Year { get; set; }
        public IFanart Fanart { get; set; }
        public string FirstAired { get; set; }

        public DateTime? FirstAiredData
        {
            get
            {
                try
                {
                    return !string.IsNullOrEmpty(FirstAired) ? DateTime.Parse(FirstAired).ToLocalTime() : (DateTime?)null;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public int? UserRating { get; set; }
        public List<string> Genres { get; set; }
        public string Status { get; set; }
    }
}