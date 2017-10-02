using AutoMapper;
using Crossroads.Data;
using Crossroads.Web.Infrastructure.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Crossroads.Web.ViewModels.ForumViewModels.Comments
{
    public class DisplayCommentViewModel : IMapFrom<Comment>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public string AuthorUserName { get; set; }

        public int AuthorPoints { get; set; }

        public ICollection<Role> AuthorRoles { get; set; }

        public string AuthorImage { get; set; }

        public int Flags { get; set; }

        public bool? Flagged { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? DateEdited { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Comment, DisplayCommentViewModel>()
                .ForMember(m => m.AuthorUserName, opt => opt.MapFrom(c => c.User.UserName))
                .ForMember(m => m.AuthorRoles, opt => opt.MapFrom(c => c.User.Roles))
                .ForMember(m => m.AuthorPoints, opt => opt.MapFrom(a => a.User.ForumPoints))
                .ForMember(m => m.AuthorImage, opt => opt.MapFrom(c => c.User.Image))
                .ForMember(m => m.Flagged, opt => opt.MapFrom(c =>
                    c.CommentFlags.Where(f => f.User.UserName == HttpContext.Current.User.Identity.Name).FirstOrDefault().Flagged));
        }
    }
}