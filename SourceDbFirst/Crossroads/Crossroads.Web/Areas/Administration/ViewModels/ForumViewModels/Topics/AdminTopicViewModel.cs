﻿using AutoMapper;
using Crossroads.Data;
using Crossroads.Web.Infrastructure.Mappings;
using System;

namespace Crossroads.Web.Areas.Administration.ViewModels.ForumViewModels.Topics
{
    public class AdminTopicViewModel : IMapFrom<Topic>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Category { get; set; }

        public string Author { get; set; }

        public int AuthorPoints { get; set; }

        public DateTime DateCreated { get; set; }

        public int Votes { get; set; }

        public int Views { get; set; }

        public int Flags { get; set; }

        public int AnswerCount { get; set; }

        public bool IsPriority { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Topic, AdminTopicViewModel>()
               .ForMember(m => m.Category, opt => opt.MapFrom(t => t.Category.Name))
               .ForMember(m => m.Author, opt => opt.MapFrom(t => t.User.UserName))
               .ForMember(m => m.AuthorPoints, opt => opt.MapFrom(a => a.User.ForumPoints))
               .ForMember(m => m.AnswerCount, opt => opt.MapFrom(t => (int?)t.Answers.Count ?? 0))
               .ForMember(m => m.IsPriority, opt => opt.MapFrom(t => t.Priority != null ? true : false));
        }
    }
}