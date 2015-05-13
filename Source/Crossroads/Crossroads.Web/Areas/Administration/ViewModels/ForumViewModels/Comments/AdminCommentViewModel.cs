using AutoMapper;
using Crossroads.Models.Forum;
using Crossroads.Web.Infrastructure.Mappings;
using System;

namespace Crossroads.Web.Areas.Administration.ViewModels.ForumViewModels.Comments
{
    public class AdminCommentViewModel : IMapFrom<Comment>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? DateEdited { get; set; }

        public int TopicId { get; set; }

        public string Topic { get; set; }

        public int AnswerId { get; set; }

        public string Author { get; set; }

        public int AuthorPoints { get; set; }

        public string AuthorImage { get; set; }

        public int Flags { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Comment, AdminCommentViewModel>()
              .ForMember(m => m.Author, opt => opt.MapFrom(a => a.AuthorProfile.ProfileUser.UserName))
              .ForMember(m => m.AuthorPoints, opt => opt.MapFrom(a => a.AuthorProfile.ForumPoints))
              .ForMember(m => m.AuthorImage, opt => opt.MapFrom(a => a.AuthorProfile.Image))
              .ForMember(m => m.TopicId, opt => opt.MapFrom(a => a.Answer.TopicId))
              .ForMember(m => m.Topic, opt => opt.MapFrom(a => a.Answer.Topic.Title));
        }
    }
}