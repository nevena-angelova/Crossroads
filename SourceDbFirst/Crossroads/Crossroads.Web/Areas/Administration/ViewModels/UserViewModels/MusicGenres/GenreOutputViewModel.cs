using Crossroads.Data;
using Crossroads.Web.Infrastructure.Mappings;

namespace Crossroads.Web.Areas.Administration.ViewModels.UserViewModels.MusicGenres
{
    public class GenreOutputViewModel : IMapFrom<MusicGenre>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}