using System;
using Crossroads.Models.UserProfile;
using Crossroads.Web.Infrastructure.Mappings;

namespace Crossroads.Web.ViewModels.ProfileViewModels
{
    public class ProfileInterestViewModel : IMapFrom<ProfileInterest>
    {
        public string Name { get; set; }
    }
}
