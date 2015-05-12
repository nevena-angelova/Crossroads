using AutoMapper;
using Crossroads.Models.Forum;
using Crossroads.Web.Infrastructure.Mappings;
using System;

namespace Crossroads.Web.Areas.Administration.ViewModels.ForumViewModels.Topics
{
    public class TopicOutputViewModel : IMapFrom<Topic>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string Category { get; set; }

        public string Author { get; set; }

        public DateTime DateCreated { get; set; }

        public int Votes { get; set; }

        public int Views { get; set; }

        public int AnswerCount { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Topic, TopicOutputViewModel>()
               .ForMember(m => m.Category, opt => opt.MapFrom(t => t.Category.Name))
               .ForMember(m => m.Author, opt => opt.MapFrom(t => t.AuthorProfile.ProfileUser.UserName))
               .ForMember(m => m.AnswerCount, opt => opt.MapFrom(t => (int?)t.Answers.Count ?? 0));
        }
    }
}