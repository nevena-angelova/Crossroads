using System;
using Crossroads.Models;
using Crossroads.Models.Profile;
using System.Data.Entity;
using Crossroads.Models.Forum;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Crossroads.Data
{
    public interface ICrossroadsData
    {
        DbContext Context { get; }

        IRepository<User> Users { get; }

        IRepository<ProfileInterest> Interests { get; }

        IRepository<MusicGenre> MusicGenres { get; }

        IRepository<UserProfile> Profiles { get; }

        IRepository<ProfileComment> ProfileComments { get; }

        IRepository<ProfileMessage> Messages { get; }

        IRepository<Town> Towns { get; }

        IRepository<Answer> Answers { get; }

        IRepository<AnswerVote> AnswerVotes { get; }

        IRepository<AnswerFlag> AnswerFlags { get; }

        IRepository<Category> Categories { get; }

        IRepository<Comment> Comments { get; }

        IRepository<CommentFlag> CommentFlags { get; }

        IRepository<Topic> Topics { get; }

        IRepository<TopicVote> TopicVotes { get; }

        IRepository<TopicFlag> TopicFlags { get; }

        void Dispose();

        int SaveChanges();

    }
}