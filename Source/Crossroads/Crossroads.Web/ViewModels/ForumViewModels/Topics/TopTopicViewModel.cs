using AutoMapper;
using Crossroads.Models.Forum;
using Crossroads.Web.Infrastructure.Mappings;
using System;
using System.Linq;

namespace Crossroads.Web.ViewModels.ForumViewModels.Topics
{
    public class TopTopicViewModel : IMapFrom<Topic>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Category { get; set; }

        public string AuthorUserName { get; set; }

        public int AuthorPoints { get; set; }

        public DateTime DateCreated { get; set; }

        public int Answers { get; set; }

        public int Votes { get; set; }

        public int Views { get; set; }

        public string LastAnswerAuthorUserName { get; set; }

        public int? LastAnswerAuthorPionts { get; set; }

        public DateTime? LastAnswerDateCreated { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Topic, TopTopicViewModel>()
                .ForMember(m => m.Category, opt => opt.MapFrom(t => t.Category.Name))
                .ForMember(m => m.AuthorUserName, opt => opt.MapFrom(t => t.AuthorProfile.ProfileUser.UserName))
                .ForMember(m => m.AuthorPoints, opt => opt.MapFrom(t => t.AuthorProfile.ForumPoints))
                .ForMember(m => m.Answers, opt => opt.MapFrom(t => t.Answers.Count))
                .ForMember(m => m.LastAnswerAuthorUserName, opt =>
                    opt.MapFrom(t => t.Answers.OrderByDescending(a => a.DateCreated).FirstOrDefault().AuthorProfile.ProfileUser.UserName))
                .ForMember(m => m.LastAnswerAuthorPionts, opt =>
                    opt.MapFrom(t => (int?)t.Answers.OrderByDescending(a => a.DateCreated).FirstOrDefault().AuthorProfile.ForumPoints))
                .ForMember(m => m.LastAnswerDateCreated, opt =>
                    opt.MapFrom(t => (DateTime?)t.Answers.OrderByDescending(a => a.DateCreated).FirstOrDefault().DateCreated));
        }
    }
}