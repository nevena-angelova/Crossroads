using Crossroads.Web.ViewModels.ForumViewModels.Answers;
using MvcPaging;
namespace Crossroads.Web.ViewModels.ForumViewModels.Topics
{
    public class DisplayTopicViewModel
    {
        public TopicContentViewModel Topic { get; set; }

        public IPagedList<DisplayAnswerViewModel> Answers { get; set; }
    }
}