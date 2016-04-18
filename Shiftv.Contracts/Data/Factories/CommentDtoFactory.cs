using Autofac;
using Shiftv.Contracts.Data.Comments;
using Shiftv.Contracts.Domain.Comments;
using Shiftv.Contracts.Domain.Users;
using Shiftv.Global;

namespace Shiftv.Contracts.Data.Factories
{
    public static class CommentDtoFactory
    {
        public static IComment Create(CommentDto dto)
        {
            if (dto == null) return null;
            var x = Ioc.Container.Resolve<IComment>();
            x.Id = dto.Id;
            x.Likes = dto.Likes;
            x.Replies = dto.Replies;
            x.CommentText = dto.CommentText;
            x.CreatedAt = dto.CreatedAt;
            x.ParentId = dto.ParentId;
            x.Replies = dto.Replies;
            x.Review = dto.Review;
            x.Spoiler = dto.Spoiler;
            x.User = UserDtoFactory.Create(dto.User);
            return x;
        }

        public static CommentDto GetDto(IComment dto)
        {
            if (dto == null) return null;
            var x = new CommentDto();
            x.Id = dto.Id;
            x.Likes = dto.Likes;
            x.Replies = dto.Replies;
            x.CommentText = dto.CommentText;
            x.CreatedAt = dto.CreatedAt;
            x.ParentId = dto.ParentId;
            x.Replies = dto.Replies;
            x.Review = dto.Review;
            x.Spoiler = dto.Spoiler;
            x.User = x.User != null ? UserDtoFactory.GetDto(dto.User) : null;
            return x;
        }
    }

    public class UserDtoFactory
    {
        public static IUser Create(UserDto dto)
        {
            if (dto == null) return null;
            var x = Ioc.Container.Resolve<IUser>();
            x.About = dto.About;
            x.Age = dto.Age;
            x.Gender = dto.Gender;
            x.Images = ImageDtoFactory.Create(dto.Images);
            x.JoinedAt = dto.JoinedAt;
            x.Location = dto.Location;
            x.Name = dto.Name;
            x.Private = dto.Private;
            x.Username = dto.Username;
            x.Vip = dto.Vip;
            return x;
        }

        public static UserDto GetDto(IUser dto)
        {
            if (dto == null) return null;
            var x = new UserDto();
            x.About = dto.About;
            x.Age = dto.Age;
            x.Gender = dto.Gender;
            x.Images = ImageDtoFactory.GetDto(dto.Images);
            x.JoinedAt = dto.JoinedAt;
            x.Location = dto.Location;
            x.Name = dto.Name;
            x.Private = dto.Private;
            x.Username = dto.Username;
            x.Vip = dto.Vip;
            return x;
        }
    }
}