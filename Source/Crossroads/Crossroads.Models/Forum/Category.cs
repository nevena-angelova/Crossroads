using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Crossroads.Models.Forum
{
    public class Category
    {
        private ICollection<Topic> topics;

        public Category()
        {
            this.topics = new HashSet<Topic>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(90, MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        [StringLength(130, MinimumLength = 3)]
        public string Description { get; set; }

        public virtual ICollection<Topic> Topics
        {
            get { return this.topics; }
            set { this.topics = value; }
        }
    }
}
