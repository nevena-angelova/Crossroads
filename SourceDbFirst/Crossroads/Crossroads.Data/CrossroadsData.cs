using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace Crossroads.Data
{
    public class CrossroadsData : ICrossroadsData
    {
        private readonly DbContext context;

        private readonly Dictionary<Type, object> repositories = new Dictionary<Type, object>();

        public CrossroadsData(DbContext context)
        {
            this.context = context;
        }

        public DbContext Context
        {
            get
            {
                return this.context;
            }
        }

        public IRepository<User> Users
        {
            get
            {
                return this.GetRepository<User>();
            }
        }

        public IRepository<ProfileInterest> Interests
        {
            get
            {
                return this.GetRepository<ProfileInterest>();
            }
        }

        public IRepository<MusicGenre> MusicGenres
        {
            get
            {
                return this.GetRepository<MusicGenre>();
            }
        }

        public IRepository<ProfileComment> ProfileComments
        {
            get
            {
                return this.GetRepository<ProfileComment>();
            }
        }

        public IRepository<ProfileMessage> Messages
        {
            get
            {
                return this.GetRepository<ProfileMessage>();
            }
        }

        public IRepository<Town> Towns
        {
            get
            {
                return this.GetRepository<Town>();
            }
        }

        public IRepository<Answer> Answers
        {
            get
            {
                return this.GetRepository<Answer>();
            }
        }

        public IRepository<AnswerVote> AnswerVotes
        {
            get
            {
                return this.GetRepository<AnswerVote>();
            }
        }

        public IRepository<AnswerFlag> AnswerFlags
        {
            get
            {
                return this.GetRepository<AnswerFlag>();
            }
        }

        public IRepository<Category> Categories
        {
            get
            {
                return this.GetRepository<Category>();
            }
        }

        public IRepository<Comment> Comments
        {
            get
            {
                return this.GetRepository<Comment>();
            }
        }

        public IRepository<CommentFlag> CommentFlags
        {
            get
            {
                return this.GetRepository<CommentFlag>();
            }
        }

        public IRepository<Topic> Topics
        {
            get
            {
                return this.GetRepository<Topic>();
            }
        }

        public IRepository<TopicVote> TopicVotes
        {
            get
            {
                return this.GetRepository<TopicVote>();
            }
        }

        public IRepository<TopicFlag> TopicFlags
        {
            get
            {
                return this.GetRepository<TopicFlag>();
            }
        }

        public IRepository<Role> Roles
        {
            get
            {
                return this.GetRepository<Role>();
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.context != null)
                {
                    this.context.Dispose();
                }
            }
        }

        public int SaveChanges()
        {
            return this.context.SaveChanges();
        }

        private IRepository<T> GetRepository<T>() where T : class
        {
            if (!this.repositories.ContainsKey(typeof(T)))
            {
                var type = typeof(EFRepository<T>);

                this.repositories.Add(typeof(T), Activator.CreateInstance(type, this.context));
            }

            return (IRepository<T>)this.repositories[typeof(T)];
        }
    }
}