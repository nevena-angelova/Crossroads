namespace Crossroads.Web.Infrastructure.Constants
{
    public class Views
    {
        //Profile
        public const string DisplayProfile = "~/Views/Profile/DisplayProfile.cshtml";
        public const string EditProfile = "~/Views/Profile/EditProfile.cshtml";

        //Forum
        public const string TopTopics = "~/Views/Forum/Topics/TopTopics.cshtml";
        public const string DisplayTopics = "~/Views/Forum/Topics/DisplayTopics.cshtml";
        public const string DisplayFoundTopics = "~/Views/Forum/Topics/DisplayFoundTopics.cshtml";
        public const string DisplayTopic = "~/Views/Forum/Topics/DisplayTopic.cshtml";
        public const string AddTopic = "~/Views/Forum/Topics/AddTopic.cshtml";
        public const string EditTopic = "~/Views/Forum/Topics/EditTopic.cshtml";

        //Administration

        public const string ListUsers = "~/Areas/Administration/Views/UsersAdmin/Users/ListUsers.cshtml";
        public const string DisplayUser = "~/Areas/Administration/Views/UsersAdmin/Users/DisplayUser.cshtml";
        public const string EditUser = "~/Areas/Administration/Views/UsersAdmin/Users/EditUser.cshtml";
        public const string ListInterests = "~/Areas/Administration/Views/UsersAdmin/Interests/ListInterests.cshtml";
        public const string ListGenres = "~/Areas/Administration/Views/UsersAdmin/MusicGenres/ListGenres.cshtml";
        public const string ListTowns = "~/Areas/Administration/Views/UsersAdmin/Towns/ListTowns.cshtml";

        public const string ListCategories = "~/Areas/Administration/Views/ForumAdmin/Categories/ListCategories.cshtml";
        public const string ListTopics = "~/Areas/Administration/Views/ForumAdmin/Topics/ListTopics.cshtml";
        public const string AdminDisplayTopic = "~/Areas/Administration/Views/ForumAdmin/Topics/AdminDisplayTopic.cshtml";
        public const string AdminEditTopic = "~/Areas/Administration/Views/ForumAdmin/Topics/AdminEditTopic.cshtml";
        public const string ListAnswers = "~/Areas/Administration/Views/ForumAdmin/Answers/ListAnswers.cshtml";
        public const string ListComments = "~/Areas/Administration/Views/ForumAdmin/Comments/ListComments.cshtml";
    }
}