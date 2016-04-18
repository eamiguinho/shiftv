using Shiftv.Contracts.Domain.Categories;

namespace Shiftv.Core.Models.Categories
{
    class Category : ICategory
    {
        public string Name { get; set; }
        public string Slug { get; set; }
    }
}
