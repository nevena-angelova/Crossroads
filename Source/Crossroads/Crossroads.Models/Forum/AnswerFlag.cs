using System.ComponentModel.DataAnnotations;

namespace Crossroads.Models.Forum
{
    public class AnswerFlag
    {
        public AnswerFlag()
        {
            this.Flagged = true;
        }

        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }

        public bool Flagged { get; set; }

        public int AnswerId { get; set; }

        public virtual Answer Answer { get; set; }
    }
}
