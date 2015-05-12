using Crossroads.Models.Profile;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crossroads.Models.Forum
{
    public class Comment
    {
        private ICollection<CommentFlag> userFlags;

        public Comment()
        {
            this.userFlags = new HashSet<CommentFlag>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(1000, MinimumLength = 10)]
        public string Content { get; set; }

        public int? AuthorProfileId { get; set; }

        public virtual UserProfile AuthorProfile { get; set; }

        public int Flags { get; set; }

        [ForeignKey("Answer")]
        public int AnswerId { get; set; }

        public virtual Answer Answer { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? DateEdited { get; set; }

        public virtual ICollection<CommentFlag> UserFlags
        {
            get { return this.userFlags; }
            set { this.userFlags = value; }
        }
    }
}