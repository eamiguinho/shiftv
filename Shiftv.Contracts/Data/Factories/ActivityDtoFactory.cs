using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Shiftv.Contracts.Data.Activity;
using Shiftv.Contracts.Domain.Activity;
using Shiftv.Global;

namespace Shiftv.Contracts.Data.Factories
{
   public class ActivityDtoFactory
    {
        public static IActivity Create(ActivityDto dto)
        {
            if (dto == null) return null;
            var activity = Ioc.Container.Resolve<IActivity>();
            activity.ActivityItems = dto.ActivityItems.Select(dto1 => ActivityItemDtoFactory.Create(dto1)).ToList();
            activity.Timestamps = ActivityTimestampsDtoFactory.Create(dto.Timestamps);
            return activity;
        }
    }
}
