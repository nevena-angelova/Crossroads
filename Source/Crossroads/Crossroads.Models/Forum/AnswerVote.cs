using System;
using System.ComponentModel.DataAnnotations;

namespace Crossroads.Models.Forum
{
    public class AnswerVote
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }

        public bool? IsVotePositive { get; set; }

        public int AnswerId { get; set; }

        public virtual Answer Answer { get; set; }
    }
}
