using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNet.Identity.EntityFramework;
using Crossroads.Data.Common.Models;
using System.Web;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;

namespace Crossroads.Data.Models
{
    public class ApplicationUser : IdentityUser, IAuditInfo, IDeletableEntity
    {
        public ApplicationUser()
        {
            // This will prevent UserManager.CreateAsync from causing exception
            this.CreatedOn = DateTime.Now;
        }

        public DateTime CreatedOn { get; set; }

        public bool PreserveCreatedOn { get; set; }

        public System.DateTime? ModifiedOn { get; set; }

        [Index]
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public bool isOnline { get; set; }

        [ForeignKey("Image")]
        public int? ImageId { get; set; }

        public virtual Image Image { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}
