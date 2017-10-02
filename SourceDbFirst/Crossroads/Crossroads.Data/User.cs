//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Crossroads.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            this.AnswerFlags = new HashSet<AnswerFlag>();
            this.Answers = new HashSet<Answer>();
            this.AnswerVotes = new HashSet<AnswerVote>();
            this.CommentFlags = new HashSet<CommentFlag>();
            this.Comments = new HashSet<Comment>();
            this.ProfileComments = new HashSet<ProfileComment>();
            this.ProfileComments1 = new HashSet<ProfileComment>();
            this.ProfileMessages = new HashSet<ProfileMessage>();
            this.ProfileMessages1 = new HashSet<ProfileMessage>();
            this.TopicFlags = new HashSet<TopicFlag>();
            this.Topics = new HashSet<Topic>();
            this.TopicVotes = new HashSet<TopicVote>();
            this.MusicGenres = new HashSet<MusicGenre>();
            this.ProfileInterests = new HashSet<ProfileInterest>();
            this.Roles = new HashSet<Role>();
        }
    
        public int Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string UserName { get; set; }
        public Nullable<System.DateTime> LastActionTime { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Nullable<int> TownId { get; set; }
        public string Bands { get; set; }
        public Nullable<bool> IsMale { get; set; }
        public Nullable<System.DateTime> BirthDate { get; set; }
        public string About { get; set; }
        public string Image { get; set; }
        public System.DateTime DateCreated { get; set; }
        public int ForumPoints { get; set; }
        public string Skype { get; set; }
        public string Facebook { get; set; }
        public Nullable<bool> EmailConfirmed { get; set; }
        public Nullable<bool> EmailMsgNotify { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AnswerFlag> AnswerFlags { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Answer> Answers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AnswerVote> AnswerVotes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CommentFlag> CommentFlags { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Comment> Comments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProfileComment> ProfileComments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProfileComment> ProfileComments1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProfileMessage> ProfileMessages { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProfileMessage> ProfileMessages1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TopicFlag> TopicFlags { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Topic> Topics { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TopicVote> TopicVotes { get; set; }
        public virtual Town Town { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MusicGenre> MusicGenres { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProfileInterest> ProfileInterests { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Role> Roles { get; set; }
    }
}
