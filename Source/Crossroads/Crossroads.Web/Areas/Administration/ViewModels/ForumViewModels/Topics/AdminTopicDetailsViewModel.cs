using AutoMapper;
using Crossroads.Models.Forum;
using Crossroads.Web.Infrastructure.Mappings;
using System;

namespace Crossroads.Web.Areas.Administration.ViewModels.ForumViewModels.Topics
{
    public class AdminTopicDetailsViewModel : IMapFrom<Topic>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string Category { get; set; }

        public string Author { get; set; }

        public int AuthorPoints { get; set; }

        public string AuthorImage { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? DateEdited { get; set; }

        public int Votes { get; set; }

        public int Views { get; set; }

        public int Flags { get; set; }

        public int AnswerCount { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Topic, AdminTopicDetailsViewModel>()
               .ForMember(m => m.Category, opt => opt.MapFrom(t => t.Category.Name))
               .ForMember(m => m.AuthorPoints, opt => opt.MapFrom(a => a.AuthorProfile.ForumPoints))
               .ForMember(m => m.AuthorImage, opt => opt.MapFrom(t => t.AuthorProfile.Image))
               .ForMember(m => m.Author, opt => opt.MapFrom(t => t.AuthorProfile.ProfileUser.UserName))
               .ForMember(m => m.AnswerCount, opt => opt.MapFrom(t => (int?)t.Answers.Count ?? 0));
        }
    }
}