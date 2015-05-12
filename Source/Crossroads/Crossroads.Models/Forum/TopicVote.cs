using System;
using System.ComponentModel.DataAnnotations;

namespace Crossroads.Models.Forum
{
    public class TopicVote
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }

        public bool? IsVotePositive { get; set; }

        public int TopicId { get; set; }

        public virtual Topic Topic { get; set; }
    }
}
