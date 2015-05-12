using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crossroads.Models.Profile
{
    public class ProfileComment
    {
        public ProfileComment()
        {
            this.DateCreated = DateTime.Now;
        }

        public int Id { get; set; }

        [Required]
        [StringLength(2500)]
        public string Content { get; set; }

        public virtual UserProfile AuthorProfile { get; set; }

        [InverseProperty("ProfileComments")]
        public virtual UserProfile Profile { get; set; }

        public DateTime DateCreated { get; set; }
    }
}