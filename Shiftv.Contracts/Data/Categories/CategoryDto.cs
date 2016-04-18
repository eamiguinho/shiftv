using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Newtonsoft.Json;
using Shiftv.Contracts.Domain.Categories;
using Shiftv.Global;

namespace Shiftv.Contracts.Data.Categories
{
    public class CategoryDto
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "slug")]
        public string Slug { get; set; }
    }

    public static class CategoryDtoFactory
    {
        public static ICategory Create(CategoryDto dto)
        {
            var x = Ioc.Container.Resolve<ICategory>();
            x.Name = dto.Name;
            x.Slug = dto.Slug;
            return x;
        }
    }
}
