using Crossroads.Models.Profile;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crossroads.Models.Forum
{
    public class Answer
    {
        private ICollection<Comment> comments;
        private ICollection<AnswerVote> userVotes;
        private ICollection<AnswerFlag> userFlags;

        public Answer()
        {
            this.comments = new HashSet<Comment>();
            this.userVotes = new HashSet<AnswerVote>();
            this.userFlags = new HashSet<AnswerFlag>();
            this.Votes = 0;
            this.Flags = 0;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(6000, MinimumLength = 10)]
        public string Content { get; set; }

        // not required because of the EF cascade DELETE
        [ForeignKey("AuthorProfile")]
        public int? AuthorProfileId { get; set; }

        public virtual UserProfile AuthorProfile { get; set; }

        [ForeignKey("Topic")]
        public int TopicId { get; set; }

        public virtual Topic Topic { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? DateEdited { get; set; }

        public int Votes { get; set; }

        public int Flags { get; set; }

        public virtual ICollection<Comment> Comments
        {
            get { return this.comments; }
            set { this.comments = value; }
        }

        public virtual ICollection<AnswerVote> UserVotes
        {
            get { return this.userVotes; }
            set { this.userVotes = value; }
        }

        public virtual ICollection<AnswerFlag> UserFlags
        {
            get { return this.userFlags; }
            set { this.userFlags = value; }
        }
    }
}