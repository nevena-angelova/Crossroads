using Crossroads.Models;
using Crossroads.Models.Forum;
using Crossroads.Models.Profile;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity;

namespace Crossroads.Data
{
    public class CrossroadsDbContext : IdentityDbContext<User>
    {
        public CrossroadsDbContext()
            : base("Crossroads", throwIfV1Schema: false)
        {
        }

        public virtual IDbSet<ProfileInterest> Interests { get; set; }

        public virtual IDbSet<MusicGenre> MusicGenres { get; set; }

        public virtual IDbSet<UserProfile> Profiles { get; set; }

        public virtual IDbSet<ProfileComment> ProfileComments { get; set; }

        public virtual IDbSet<ProfileMessage> Messages { get; set; }

        public virtual IDbSet<Town> Towns { get; set; }

        public virtual IDbSet<Answer> Answers { get; set; }

        public virtual IDbSet<AnswerVote> AnswerVotes { get; set; }

        public virtual IDbSet<AnswerFlag> AnswerFlags { get; set; }

        public virtual IDbSet<Category> Categories { get; set; }

        public virtual IDbSet<Comment> Comments { get; set; }

        public virtual IDbSet<CommentFlag> CommentFlags { get; set; }

        public virtual IDbSet<Topic> Topics { get; set; }

        public virtual IDbSet<TopicVote> TopicVotes { get; set; }

        public virtual IDbSet<TopicFlag> TopicFlags { get; set; }

        public static CrossroadsDbContext Create()
        {
            return new CrossroadsDbContext();
        }
    }
}
