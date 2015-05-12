using Crossroads.Models.Profile;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crossroads.Models.Forum
{
    public class Topic
    {
        private ICollection<Answer> answers;
        private ICollection<TopicVote> userVotes;
        private ICollection<TopicFlag> userFlags;

        public Topic()
        {
            this.answers = new HashSet<Answer>();
            this.userVotes = new HashSet<TopicVote>();
            this.userFlags = new HashSet<TopicFlag>();
            this.Votes = 0;
            this.Flags = 0;
            this.Views = 0;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Title { get; set; }

        [Required]
        [StringLength(6000, MinimumLength = 10)]
        public string Content { get; set; }

        [Required]
        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }

        [ForeignKey("AuthorProfile")]
        public int AuthorProfileId { get; set; }

        public virtual UserProfile AuthorProfile { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime? DateEdited { get; set; }

        public int Votes { get; set; }

        public int Views { get; set; }

        public int Flags { get; set; }

        public virtual ICollection<Answer> Answers
        {
            get { return this.answers; }
            set { this.answers = value; }
        }

        public virtual ICollection<TopicVote> UserVotes
        {
            get { return this.userVotes; }
            set { this.userVotes = value; }
        }

        public virtual ICollection<TopicFlag> UserFlags
        {
            get { return this.userFlags; }
            set { this.userFlags = value; }
        }
    }
}