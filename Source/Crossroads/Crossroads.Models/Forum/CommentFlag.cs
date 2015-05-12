using System.ComponentModel.DataAnnotations;

namespace Crossroads.Models.Forum
{
    public class CommentFlag
    {
        public CommentFlag()
        {
            this.Flagged = true;
        }

        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }

        public bool Flagged { get; set; }

        public int CommentId { get; set; }

        public virtual Comment Comment { get; set; }
    }
}
