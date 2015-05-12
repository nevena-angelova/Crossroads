using System.ComponentModel.DataAnnotations;

namespace Crossroads.Models.Forum
{
    public class TopicFlag
    {
        public TopicFlag()
        {
            this.Flagged = true;
        }

        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }

        public bool Flagged { get; set; }

        public int TopicId { get; set; }

        public virtual Topic Topic { get; set; }
    }
}
