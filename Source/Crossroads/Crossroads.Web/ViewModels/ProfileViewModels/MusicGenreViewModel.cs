using System;
using Crossroads.Models.UserProfile;
using Crossroads.Web.Infrastructure.Mappings;

namespace Crossroads.Web.ViewModels.ProfileViewModels
{
    public class MusicGenreViewModel : IMapFrom<MusicGenre>
    {
        public string Name { get; set; }
    }
}
