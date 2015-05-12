using AutoMapper;
using Crossroads.Models.Forum;
using Crossroads.Web.Infrastructure.Mappings;
using System.Linq;

namespace Crossroads.Web.Areas.Administration.ViewModels.ForumViewModels.Categories
{
    public class CategoryOutputViewModel : IMapFrom<Category>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int? TopicsCount { get; set; }

        public int? AnswersCount { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Category, CategoryOutputViewModel>()
               .ForMember(m => m.TopicsCount, opt => opt.MapFrom(c => (int?)c.Topics.Count ?? 0))
               .ForMember(m => m.AnswersCount, opt => opt.MapFrom(c => (int?)c.Topics.Sum(a => a.Answers.Count) ?? 0));
        }
    }
}