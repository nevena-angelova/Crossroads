using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crossroads.Models.Profile
{
    public class ProfileMessage
    {
        public ProfileMessage()
        {
            this.IsRead = false;
            this.DateCreated = DateTime.Now;
        }

        public int Id { get; set; }

        [Required]
        [StringLength(80, MinimumLength = 3)]
        public string Title { get; set; }

        [Required]
        [StringLength(2000, MinimumLength = 10)]
        public string Content { get; set; }

        public virtual UserProfile AuthorProfile { get; set; }

        [InverseProperty("Messages")]
        public virtual UserProfile Profile { get; set; }

        public DateTime DateCreated { get; set; }

        public bool IsRead { get; set; }
    }
}