using AutoMapper;
using Crossroads.Models.Forum;
using Crossroads.Web.Infrastructure.Mappings;
using System;
using System.Linq;

namespace Crossroads.Web.ViewModels.ForumViewModels
{
    public class DisplayCategoryViewModel : IMapFrom<Category>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int TopicsCount { get; set; }

        public int AnswersCount { get; set; }

        public int? LastTopicId { get; set; }

        public string LastTopicTitle { get; set; }

        public string LastTopicAuthor { get; set; }

        public string LastTopicAuthorImage { get; set; }

        public DateTime? LastTopicDateCreated { get; set; }

        public int? CategoryLastAnswerTopicId { get; set; }

        public string CategoryLastAnswerTopicTitle { get; set; }

        public string CategoryLastAnswerAuthor { get; set; }

        public string CategoryLastAnswerAuthorImage { get; set; }

        public DateTime? CategoryLastAnswerDateCreated { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Category, DisplayCategoryViewModel>()
                 .ForMember(m => m.LastTopicId, opt =>
                    opt.MapFrom(c => (int?)c.Topics.OrderByDescending(t => t.DateCreated).FirstOrDefault().Id ?? 0))
                .ForMember(m => m.LastTopicTitle, opt =>
                    opt.MapFrom(c => c.Topics.OrderByDescending(t => t.DateCreated).FirstOrDefault().Title))
                .ForMember(m => m.LastTopicAuthor, opt =>
                    opt.MapFrom(c => c.Topics.OrderByDescending(t => t.DateCreated).FirstOrDefault().AuthorProfile.ProfileUser.UserName))
                .ForMember(m => m.LastTopicAuthorImage, opt =>
                    opt.MapFrom(c => c.Topics.OrderByDescending(t => t.DateCreated).FirstOrDefault().AuthorProfile.Image))
                .ForMember(m => m.LastTopicDateCreated, opt =>
                    opt.MapFrom(c => (DateTime?)c.Topics.OrderByDescending(t => t.DateCreated).FirstOrDefault().DateCreated))
                .ForMember(m => m.TopicsCount, opt => opt.MapFrom(c => (int?)c.Topics.Count ?? 0))
                .ForMember(m => m.AnswersCount, opt => opt.MapFrom(c => (int?)c.Topics.Sum(a => a.Answers.Count) ?? 0))
                //when adding a new answer, its topic DateUpdated is updated so the last updated topic's last answer is taken
                .ForMember(m => m.CategoryLastAnswerTopicId, opt =>
                    opt.MapFrom(c => (int?)c.Topics.OrderByDescending(t => t.DateUpdated).FirstOrDefault()
                        .Answers.OrderByDescending(a => a.DateCreated).FirstOrDefault().Topic.Id ?? 0))
                .ForMember(m => m.CategoryLastAnswerTopicTitle, opt =>
                    opt.MapFrom(c => c.Topics.OrderByDescending(t => t.DateUpdated).FirstOrDefault()
                        .Answers.OrderByDescending(a => a.DateCreated).FirstOrDefault().Topic.Title))
                .ForMember(m => m.CategoryLastAnswerAuthor, opt =>
                    opt.MapFrom(c => c.Topics.OrderByDescending(t => t.DateUpdated).FirstOrDefault()
                        .Answers.OrderByDescending(a => a.DateCreated).FirstOrDefault().AuthorProfile.ProfileUser.UserName))
                .ForMember(m => m.CategoryLastAnswerAuthorImage, opt =>
                    opt.MapFrom(c => c.Topics.OrderByDescending(t => t.DateUpdated).FirstOrDefault()
                        .Answers.OrderByDescending(a => a.DateCreated).FirstOrDefault().AuthorProfile.Image))
                .ForMember(m => m.CategoryLastAnswerDateCreated, opt =>
                    opt.MapFrom(c => (DateTime?)c.Topics.OrderByDescending(t => t.DateUpdated).FirstOrDefault()
                    .Answers.OrderByDescending(a => a.DateCreated).FirstOrDefault().DateCreated));
        }
    }
}