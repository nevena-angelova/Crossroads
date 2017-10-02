using System.Data.Entity;

namespace Crossroads.Data
{
    public interface ICrossroadsData
    {
        DbContext Context { get; }

        IRepository<User> Users { get; }

        IRepository<ProfileInterest> Interests { get; }

        IRepository<MusicGenre> MusicGenres { get; }

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

        IRepository<Role> Roles { get; }

        void Dispose();

        int SaveChanges();

    }
}