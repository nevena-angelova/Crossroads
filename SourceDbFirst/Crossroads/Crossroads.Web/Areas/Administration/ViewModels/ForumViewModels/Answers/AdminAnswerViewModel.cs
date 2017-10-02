using AutoMapper;
using Crossroads.Data;
using Crossroads.Web.Infrastructure.Mappings;
using System;
using System.Collections.Generic;

namespace Crossroads.Web.Areas.Administration.ViewModels.ForumViewModels.Answers
{
    public class AdminAnswerViewModel : IMapFrom<Answer>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public int TopicId { get; set; }

        public string Topic { get; set; }

        public string Author { get; set; }

        public int AuthorPoints { get; set; }

        public string AuthorImage { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? DateEdited { get; set; }

        public int Votes { get; set; }

        public int Flags { get; set; }

        public ICollection<AnswerFlag> AnswerFlags { get; set; }

        public int CommentsCount { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Answer, AdminAnswerViewModel>()
              .ForMember(m => m.Topic, opt => opt.MapFrom(a => a.Topic.Title))
              .ForMember(m => m.Author, opt => opt.MapFrom(a => a.User.UserName))
              .ForMember(m => m.AuthorPoints, opt => opt.MapFrom(a => a.User.ForumPoints))
              .ForMember(m => m.AuthorImage, opt => opt.MapFrom(a => a.User.Image))
              .ForMember(m => m.CommentsCount, opt => opt.MapFrom(a => a.Comments.Count));
        }
    }
}