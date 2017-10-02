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
    
    public partial class Answer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Answer()
        {
            this.AnswerFlags = new HashSet<AnswerFlag>();
            this.AnswerVotes = new HashSet<AnswerVote>();
            this.Comments = new HashSet<Comment>();
        }
    
        public int Id { get; set; }
        public string Content { get; set; }
        public Nullable<int> UserId { get; set; }
        public int TopicId { get; set; }
        public System.DateTime DateCreated { get; set; }
        public Nullable<System.DateTime> DateEdited { get; set; }
        public int Votes { get; set; }
        public int Flags { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AnswerFlag> AnswerFlags { get; set; }
        public virtual Topic Topic { get; set; }
        public virtual User User { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AnswerVote> AnswerVotes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Comment> Comments { get; set; }
    }
}