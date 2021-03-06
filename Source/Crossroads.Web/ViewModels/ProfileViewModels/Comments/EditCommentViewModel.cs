﻿using AutoMapper;
using Crossroads.Models.Profile;
using Crossroads.Web.Infrastructure.Mappings;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Crossroads.Web.ViewModels.ProfileViewModels.Comments
{
    public class EditCommentViewModel: IMapFrom<ProfileComment>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        [AllowHtml]
        [UIHint("TinyMCE")]
        [Required(ErrorMessage = "Съдържанието е задължително.")]
        [StringLength(2500, ErrorMessage = "Коментарът трябва да е до {1} символа.")]
        public string Content { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<ProfileComment, EditCommentViewModel>()
                .ForMember(m => m.UserName, opt => opt.MapFrom(t => t.AuthorProfile.ProfileUser.UserName));
        }
    }
}