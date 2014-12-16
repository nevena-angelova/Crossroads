using Crossroads.Data.Models;
using Crossroads.Web.Infrastructure.Mapping;

namespace Crossroads.Web.ViewModels
{
    public class PostViewModel : IMapFrom<Post>
    {
        public string Title { get; set; }
    }
}