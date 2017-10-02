using Crossroads.Data;
using Crossroads.Web.Infrastructure.Mappings;
using System.ComponentModel.DataAnnotations;

namespace Crossroads.Web.Areas.Administration.ViewModels.UserViewModels.Towns
{
    public class TownInputViewModel : IMapFrom<Town>
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Полето е задължително")]
        [StringLength(25, MinimumLength = 2, ErrorMessage = "Дължината трябва да е от {2} до {1} символа.")]
        public string Name { get; set; }
    }
}