using Crossroads.Models.Forum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crossroads.Models.Profile
{
    public class UserProfile
    {
        private ICollection<ProfileInterest> interests;
        private ICollection<MusicGenre> musicGenres;
        private ICollection<ProfileComment> profileComments;
        private ICollection<ProfileMessage> messages;
        private ICollection<Topic> topics;

        public UserProfile()
        {
            this.interests = new HashSet<ProfileInterest>();
            this.musicGenres = new HashSet<MusicGenre>();
            this.profileComments = new HashSet<ProfileComment>();
            this.messages = new HashSet<ProfileMessage>();
            this.topics = new HashSet<Topic>();
        }

        [Key]
        public int Id { get; set; }

        [StringLength(40, MinimumLength = 2)]
        public string FirstName { get; set; }

        [StringLength(40, MinimumLength = 2)]
        public string LastName { get; set; }

        [ForeignKey("Town")]
        public int? TownId { get; set; }

        public virtual Town Town { get; set; }

        [StringLength(600)]
        public string Bands { get; set; }

        public bool? IsMale { get; set; }

        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }

        [StringLength(1500)]
        public string About { get; set; }

        public string ProfileUserId { get; set; }

        public virtual User ProfileUser { get; set; }

        public string Image { get; set; }

        public DateTime DateCreated { get; set; }

        public int ForumPoints { get; set; }

        public virtual ICollection<ProfileInterest> Interests
        {
            get { return this.interests; }
            set { this.interests = value; }
        }

        public virtual ICollection<MusicGenre> MusicGenres
        {
            get { return this.musicGenres; }
            set { this.musicGenres = value; }
        }

        public virtual ICollection<ProfileComment> ProfileComments
        {
            get { return this.profileComments; }
            set { this.profileComments = value; }
        }

        public virtual ICollection<ProfileMessage> Messages
        {
            get { return this.messages; }
            set { this.messages = value; }
        }
    }
}