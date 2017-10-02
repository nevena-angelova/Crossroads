using AutoMapper;
using Crossroads.Data;
using Crossroads.Web.Infrastructure.Mappings;
using Crossroads.Web.ViewModels.ForumViewModels.Comments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Crossroads.Web.ViewModels.ForumViewModels.Answers
{
    public class DisplayAnswerViewModel : IMapFrom<Answer>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public string AuthorUserName { get; set; }

        public int AuthorPoints { get; set; }

        public ICollection<Role> AuthorRoles { get; set; }

        public string AuthorImage { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? DateEdited { get; set; }

        public int Votes { get; set; }

        public int Flags { get; set; }

        public bool? Flagged { get; set; }

        public ICollection<DisplayCommentViewModel> Comments { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Answer, DisplayAnswerViewModel>()
                .ForMember(m => m.AuthorUserName, opt => opt.MapFrom(a => a.User.UserName))
                .ForMember(m => m.AuthorPoints, opt => opt.MapFrom(a => a.User.ForumPoints))
                .ForMember(m => m.AuthorRoles, opt => opt.MapFrom(a => a.User.Roles))
                .ForMember(m => m.AuthorImage, opt => opt.MapFrom(a => a.User.Image))
                .ForMember(m => m.Flagged, opt => opt.MapFrom(a =>
                    a.AnswerFlags.Where(f => f.User.UserName == HttpContext.Current.User.Identity.Name).FirstOrDefault().Flagged))
                .ForMember(m => m.Comments, opt => opt.MapFrom(a => a.Comments.OrderBy(c => c.DateCreated)));
        }
    }
}