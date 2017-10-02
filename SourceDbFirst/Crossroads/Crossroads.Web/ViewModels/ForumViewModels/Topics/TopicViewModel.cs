﻿using AutoMapper;
using Crossroads.Data;
using Crossroads.Web.Infrastructure.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Crossroads.Web.ViewModels.ForumViewModels.Topics
{
    public class TopicViewModel : IMapFrom<Topic>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string AuthorUserName { get; set; }

        public int AuthorPoints { get; set; }

        public ICollection<Role> AuthorRoles { get; set; }

        public DateTime DateCreated { get; set; }

        public int Answers { get; set; }

        public int Votes { get; set; }

        public int Views { get; set; }

        public string LastAnswerAuthorUserName { get; set; }

        public ICollection<Role> LastAnswerAuthorRoles { get; set; }

        public int? LastAnswerAuthorPionts { get; set; }

        public DateTime? LastAnswerDateCreated { get; set; }

        public bool IsPriority { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Topic, TopicViewModel>()
                .ForMember(m => m.AuthorUserName, opt => opt.MapFrom(t => t.User.UserName))
                .ForMember(m => m.AuthorPoints, opt => opt.MapFrom(t => t.User.ForumPoints))
                .ForMember(m => m.AuthorRoles, opt => opt.MapFrom(t => t.User.Roles))
                .ForMember(m => m.Answers, opt => opt.MapFrom(t => t.Answers.Count))
                .ForMember(m => m.LastAnswerAuthorUserName, opt =>
                    opt.MapFrom(t => t.Answers.OrderByDescending(a => a.DateCreated).FirstOrDefault().User.UserName))
                .ForMember(m => m.LastAnswerAuthorRoles, opt =>
                    opt.MapFrom(t => t.Answers.OrderByDescending(a => a.DateCreated).FirstOrDefault().User.Roles))
                .ForMember(m => m.LastAnswerAuthorPionts, opt =>
                    opt.MapFrom(t => (int?)t.Answers.OrderByDescending(a => a.DateCreated).FirstOrDefault().User.ForumPoints))
                .ForMember(m => m.LastAnswerDateCreated, opt =>
                    opt.MapFrom(t => (DateTime?)t.Answers.OrderByDescending(a => a.DateCreated).FirstOrDefault().DateCreated))
                .ForMember(m => m.IsPriority, opt => opt.MapFrom(t => t.Priority != null ? true : false));
        }
    }
}