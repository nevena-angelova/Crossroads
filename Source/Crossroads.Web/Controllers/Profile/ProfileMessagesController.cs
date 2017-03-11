using AutoMapper;
using AutoMapper.QueryableExtensions;
using Crossroads.Data;
using Crossroads.Models.Profile;
using Crossroads.Web.ViewModels.ProfileViewModels.Messages;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Crossroads.Web.Infrastructure.Constants;
using Crossroads.Web.Infrastructure.Sanitizer;

namespace Crossroads.Web.Controllers
{
    public class ProfileMessagesController : BaseController
    {
        private readonly ISanitizer sanitizer;

        public ProfileMessagesController(ICrossroadsData data, ISanitizer sanitizer)
            : base(data)
        {
            this.sanitizer = sanitizer;
        }

        [Authorize]
        public ActionResult PostMessage(int profileId, int? answerMsgId)
        {
            AddMessageViewModel message = new AddMessageViewModel
            {
                ProfileId = profileId
            };

            if (answerMsgId != null)
            {
                message.AnswerMsgId = answerMsgId;
            }

            return this.PartialView(Partials.PostMessage, message);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult PostMessage(AddMessageViewModel message)
        {
            if (message != null && ModelState.IsValid)
            {
                var autorProfile = this.Data.Profiles.All()
                    .Where(p => p.ProfileUser.UserName == this.CurrentUser.UserName)
                    .FirstOrDefault();

                if (autorProfile == null)
                {
                    return HttpNotFound("Profile not found!");
                }             

                ProfileMessage dbMessaege = new ProfileMessage
                {
                    AuthorProfile = autorProfile,
                    Title = message.Title,
                    Content = sanitizer.Sanitize(message.Content)
                };

                var profile = this.Data.Profiles.GetById(message.ProfileId);
                if (profile == null)
                {
                    return HttpNotFound("Profile not found!");
                }

                profile.Messages.Add(dbMessaege);
                this.Data.SaveChanges();

                return Content("Съобщението е изпратено успешно.");
            }

            throw new HttpException(400, "Message not valid!");
        }

        [Authorize]
        public ActionResult DisplayMessages(int id)
        {
            var messages = this.Data.Messages.All()
                .Where(m => m.Profile.Id == id)
                .OrderByDescending(m => m.DateCreated)
                .Project()
                .To<MessageViewModel>()
                .ToList();

            if (messages.Count > 0)
            {
                return PartialView(Partials.Messages, messages);
            }

            return Content("Нямате съобщения.");
        }

        [Authorize]
        public ActionResult ReadMessage(int id)
        {
            ProfileMessage message = this.Data.Messages.GetById(id);
            message.IsRead = true;
            this.Data.SaveChanges();

            MessageContentViewModel content = Mapper.Map<MessageContentViewModel>(message);

            return PartialView(Partials.MessageContent, content);
        }

        [Authorize]
        public ActionResult DeleteMessage(int id)
        {
            ProfileMessage message = this.Data.Messages.GetById(id);

            if (message == null)
            {
                return HttpNotFound("Message not found!");
            }

            this.Data.Messages.Delete(message);
            this.Data.SaveChanges();

            return Content("");
        }
    }
}