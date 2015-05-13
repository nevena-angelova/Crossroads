using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Crossroads.Models.Profile
{
    public class Town
    {
        private ICollection<UserProfile> profiles;

        public Town()
        {
            this.profiles = new HashSet<UserProfile>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(25, MinimumLength = 2)]
        public string Name { get; set; }

        public virtual ICollection<UserProfile> Profiles
        {
            get { return this.profiles; }
            set { this.profiles = value; }
        }
    }
}
