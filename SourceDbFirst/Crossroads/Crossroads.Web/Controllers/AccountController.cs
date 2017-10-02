using CaptchaMvc.Attributes;
using Crossroads.Data;
using Crossroads.Web.Infrastructure.Account;
using Crossroads.Web.Infrastructure.Constants;
using Crossroads.Web.ViewModels.AccountViewModels;
using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;

namespace Crossroads.Web.Controllers
{
    public class AccountController : BaseController
    {
        public AccountController(ICrossroadsData data)
            : base(data)
        {
        }

        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                MyMembershipProvider provider = new MyMembershipProvider();
                if (provider.ValidateUser(model.UserName, model.Password))
                {
                    var dbUser = this.Data.Users.All().Where(u => u.UserName == model.UserName).FirstOrDefault();

                    if (dbUser.EmailConfirmed == true)
                    {
                        FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);

                        if (Url.IsLocalUrl(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Непотвърден email.");
                        ViewBag.ReturnUrl = returnUrl;
                        return View(model);
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "Неуспешен опит за влизане.");
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        public ActionResult Register()
        {
            return View();
        }


        [HttpPost, CaptchaVerify("Грешен код за валидация")]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (this.Data.Users.All().Where(u => u.UserName == model.UserName).Any())
            {
                ModelState.AddModelError("", "Съществува потребител със същото име.");
                return View(model);
            }

            if (this.Data.Users.All().Where(u => u.Email == model.Email).Any())
            {
                ModelState.AddModelError("", "Съществува потребител със същия email.");
                return View(model);
            }

            string securityStamp = Guid.NewGuid().ToString();

            var user = new User
            {
                UserName = model.UserName,
                Email = model.Email,
                SecurityStamp = securityStamp,
                PasswordHash = Crypto.SHA1(model.UserName.Substring(3) + model.Password),
                ForumPoints = 0,
                DateCreated = DateTime.Now,
                EmailMsgNotify = true
            };

            FormsAuthentication.SetAuthCookie(model.UserName, false);

            this.Data.Users.Add(user);
            this.Data.SaveChanges();

            string token = Crypto.SHA1(user.UserName.Substring(3) + user.PasswordHash + securityStamp);

            SmtpClient smtpServer = new SmtpClient("hefes.icnhost.net");
            smtpServer.Credentials = new NetworkCredential(Constants.AdminEmail, Constants.AdminEmailPass);
            smtpServer.Port = 25;

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(Constants.AdminEmail);
            mail.To.Add(user.Email);
            mail.Subject = "Потвърждаване на регистрацията в Crossroads";
            mail.Body = String.Format("За да потвърдите регистрацията си, моля кликнете на http://crossroads.bg/Account/ConfirmEmail?userName={0}&secretKey={1}", user.UserName, token);

            mail.IsBodyHtml = true;

            smtpServer.Send(mail);

            return View(Views.DisplayConfirmEmailMsg);

        }

        public ActionResult ConfirmEmail(string userName, string secretKey)
        {
            var dbUser = this.Data.Users.All().Where(u => u.UserName == userName).FirstOrDefault();
            if (dbUser == null)
            {
                return HttpNotFound("User not found!");
            }

            string dbUserToken = Crypto.SHA1(dbUser.UserName.Substring(3) + dbUser.PasswordHash + dbUser.SecurityStamp);

            if (dbUserToken == secretKey)
            {
                dbUser.EmailConfirmed = true;
                this.Data.SaveChanges();

                return RedirectToAction("EditUser", "Profile", new { userName = dbUser.UserName });
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Bad Request");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            var dbUser = this.Data.Users.All()
                .Where(u => u.UserName == this.CurrentUser.UserName)
                .FirstOrDefault();

            if (dbUser == null)
            {
                return HttpNotFound("User not found!");
            }

            dbUser.LastActionTime = dbUser.LastActionTime.Value.AddMinutes(-5);

            this.Data.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        public ActionResult ResetPassword()
        {
            return View(new ResetPasswordViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var dbUser = this.Data.Users.All()
                .Where(u => u.Email == model.Email)
                .FirstOrDefault();

            if (dbUser != null)
            {
                var newPassword = Membership.GeneratePassword(16, 3);

                dbUser.PasswordHash = Crypto.SHA1(dbUser.UserName.Substring(3) + newPassword);

                MailMessage mail = new MailMessage();

                SmtpClient smtpServer = new SmtpClient("hefes.icnhost.net");
                smtpServer.Credentials = new NetworkCredential(Constants.AdminEmail, Constants.AdminEmailPass);
                smtpServer.Port = 25;

                mail.From = new MailAddress(Constants.AdminEmail);
                mail.To.Add(dbUser.Email);
                mail.Subject = "Забравена парола";
                mail.Body = "Здравейте, Новата Ви парола е: " + newPassword;

                smtpServer.Send(mail);

                this.Data.SaveChanges();

                return RedirectToAction("Login", "Account");
            }
            else
            {
                ModelState.AddModelError("", "Няма намерен Email.");
            }

            return View(model);
        }


        //public ActionResult NewPassword(ResetPasswordViewModel model)
        //{

        //}
    }
}