using AutoMapper;
using AutoMapper.QueryableExtensions;
using Crossroads.Data;
using Crossroads.Web.ViewModels.ProfileViewModels.Messages;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Crossroads.Web.Infrastructure.Constants;
using Crossroads.Web.Infrastructure.Sanitizer;
using System;
using System.Net.Mail;
using System.Net;
using MvcPaging;

namespace Crossroads.Web.Controllers
{
    public class ProfileMessagesController : BaseController
    {
        private readonly ISanitizer sanitizer;
        private const int DefaultPageSize = 10;

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
                UserId = profileId
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
                var autor = this.Data.Users.All()
                    .Where(u => u.UserName == this.CurrentUser.UserName)
                    .FirstOrDefault();

                if (autor == null)
                {
                    return HttpNotFound("User not found!");
                }

                ProfileMessage dbMessaege = new ProfileMessage
                {
                    User1 = autor,
                    Title = message.Title,
                    Content = sanitizer.Sanitize(message.Content),
                    DateCreated = DateTime.Now
                };

                var profile = this.Data.Users.GetById(message.UserId);
                if (profile == null)
                {
                    return HttpNotFound("User not found!");
                }

                if (profile.EmailMsgNotify == true)
                {
                    SmtpClient smtpServer = new SmtpClient("hefes.icnhost.net");
                    smtpServer.Credentials = new NetworkCredential(Constants.AdminEmail, Constants.AdminEmailPass);
                    smtpServer.Port = 25;

                    MailMessage mail = new MailMessage();
                    mail.From = new MailAddress(Constants.AdminEmail);
                    mail.To.Add(profile.Email);
                    mail.Subject = "Crossroads: Ново съобщение";
                    mail.Body = "Здравейте, имате ново съобщение от " + autor.UserName + ".";

                    smtpServer.Send(mail);
                }

                profile.ProfileMessages.Add(dbMessaege);
                this.Data.SaveChanges();

                return Content("Съобщението е изпратено успешно.");
            }

            throw new HttpException(400, "Message not valid!");
        }

        [Authorize]
        public ActionResult DisplayMessages(int id, int? page)
        {
            var messages = this.Data.Messages.All()
                .Where(m => m.User.Id == id)
                .OrderByDescending(m => m.DateCreated)
                .Project()
                .To<MessageViewModel>()
                .ToList();

            if (messages.Count > 0)
            {
                int currentPageIndex = page.HasValue ? page.Value - 1 : 0;
                return PartialView(Partials.Messages, messages.ToPagedList(currentPageIndex, DefaultPageSize));
            }

            return Content("Нямате съобщения.");
        }

        [Authorize]
        public ActionResult ReadMessage(int id)
        {
            ProfileMessage message = this.Data.Messages.GetById(id);

            if (!(message.IsRead == true))
            {
                message.IsRead = true;
                this.Data.SaveChanges();
            }

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