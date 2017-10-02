using AutoMapper;
using Crossroads.Data;
using Crossroads.Web.Infrastructure.AuthorizeAttributes;
using Crossroads.Web.Infrastructure.Constants;
using Crossroads.Web.Infrastructure.Sanitizer;
using Crossroads.Web.ViewModels.ForumViewModels.Answers;
using System;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Crossroads.Web.Controllers.Forum
{
    public class AnswersController : BaseController
    {
        private readonly ISanitizer sanitizer;

        public AnswersController(ICrossroadsData data, ISanitizer sanitizer)
            : base(data)
        {
            this.sanitizer = sanitizer;
        }

        [AjaxAuthorize]
        public ActionResult AddAnswer(int topicId)
        {
            if (!Request.IsAjaxRequest())
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return this.Content("This action can be invoke only by AJAX call");
            }

            AddAnswerViewModel answer = new AddAnswerViewModel();
            answer.TopicId = topicId;
            return this.PartialView(Partials.AddAnswer, answer);
        }

        [HttpPost]
        [AjaxAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult AddAnswer(AddAnswerViewModel answer)
        {
            if (!Request.IsAjaxRequest())
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return this.Content("This action can be invoke only by AJAX call");
            }

            if (answer != null && ModelState.IsValid)
            {
                Topic topic = this.Data.Topics.GetById(answer.TopicId);

                Category category = topic.Category;
                topic.Category = category;
                topic.DateUpdated = DateTime.Now;

                User User = this.Data.Users.All()
                                    .Where(u => u.UserName == this.CurrentUser.UserName)
                                    .FirstOrDefault();

                Answer dbAnswer = new Answer
                {
                    Topic = topic,
                    User = User,
                    DateCreated = DateTime.Now,
                    Content = sanitizer.Sanitize(answer.Content)
                };

                this.Data.Answers.Add(dbAnswer);
                this.Data.SaveChanges();

                DisplayAnswerViewModel answerModel = Mapper.Map<DisplayAnswerViewModel>(dbAnswer);

                return this.PartialView(Partials.DisplayAnswer, answerModel);
            }

            throw new HttpException(400, "Not valid answer!");
        }

        [MyOrInRoleAuthorize]
        public ActionResult EditAnswer(int answerId, string userName)
        {
            if (!Request.IsAjaxRequest())
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return this.Content("This action can be invoke only by AJAX call");
            }

            Answer dbAnswer = this.Data.Answers.GetById(answerId);
            if (dbAnswer == null)
            {
                return HttpNotFound("Answer not found!");
            }

            EditAnswerViewModel answer = Mapper.Map<EditAnswerViewModel>(dbAnswer);

            return this.PartialView(Partials.EditAnswer, answer);
        }

        [HttpPost]
        [MyOrInRoleAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult EditAnswer(EditAnswerViewModel answer)
        {
            if (!Request.IsAjaxRequest())
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return this.Content("This action can be invoke only by AJAX call");
            }

            if (answer != null && ModelState.IsValid)
            {
                Answer dbAnswer = this.Data.Answers.GetById(answer.Id);

                if (dbAnswer == null)
                {
                    return HttpNotFound("Answer not found!");
                }

                dbAnswer.Content = sanitizer.Sanitize(answer.Content);
                dbAnswer.DateEdited = DateTime.Now;

                this.Data.SaveChanges();

                DisplayAnswerViewModel answerModel = Mapper.Map<DisplayAnswerViewModel>(dbAnswer);

                return this.PartialView(Partials.DisplayAnswer, answerModel);
            }

            throw new HttpException(400, "Answer not valid!");
        }

        [HttpPost]
        [AjaxAuthorize]
        public ActionResult AnswerFlag(int answerId)
        {
            if (!Request.IsAjaxRequest())
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return this.Content("This action can be invoke only by AJAX call!");
            }

            Answer answer = this.Data.Answers.GetById(answerId);
            if (answer == null)
            {
                return HttpNotFound("Answer not found!");
            }

            if (!(answer.User.Id == this.CurrentUser.Id))
            {
                bool isUserInCollection = false;

                foreach (var flag in answer.AnswerFlags)
                {
                    if (flag.UserId == this.CurrentUser.Id)
                    {
                        if (flag.Flagged == true || flag.Flagged == null)
                        {
                            answer.Flags--;
                            flag.Flagged = false;
                        }
                        else
                        {
                            answer.Flags++;
                            flag.Flagged = true;
                        }

                        isUserInCollection = true;
                        break;
                    }
                }

                if (!isUserInCollection)
                {
                    AnswerFlag newFlag = new AnswerFlag
                    {
                        User = this.CurrentUser,
                    };

                    answer.Flags++;
                    answer.AnswerFlags.Add(newFlag);
                }

                this.Data.SaveChanges();

                return this.Content(answer.Flags.ToString());
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }


        [HttpPost]
        [AjaxAuthorize]
        public ActionResult AnswerVote(int answerId, bool isVotePositive)
        {
            if (!Request.IsAjaxRequest())
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return this.Content("This action can be invoke only by AJAX call");
            }

            Answer answer = this.Data.Answers.GetById(answerId);
            if (answer == null)
            {
                return HttpNotFound("Answer not found!");
            }

            if (!(answer.User.Id == this.CurrentUser.Id))
            {
                bool isUserInCollection = false;

                if (answer.AnswerVotes.Count > 0)
                {
                    foreach (var vote in answer.AnswerVotes)
                    {
                        if (vote.UserId == this.CurrentUser.Id)
                        {
                            if (vote.IsVotePositive == isVotePositive)
                            {
                                return this.Content("<strong class=\"vote-error\">Не може да давате повече от един глас на отговор!<a href=\"javascript:void(0)\">OK</a></strong>" + answer.Votes.ToString());
                            }
                            else if (vote.IsVotePositive == null)
                            {
                                vote.IsVotePositive = isVotePositive;
                            }
                            else
                            {
                                vote.IsVotePositive = null;
                            }

                            isUserInCollection = true;
                            break;
                        }
                    }
                }

                if (!isUserInCollection)
                {
                    AnswerVote newVote = new AnswerVote
                    {
                        User = this.CurrentUser,
                        IsVotePositive = isVotePositive
                    };

                    answer.AnswerVotes.Add(newVote);
                }

                if (isVotePositive)
                {
                    answer.Votes++;
                    answer.User.ForumPoints += 1;
                }
                else
                {
                    answer.Votes--;
                    answer.User.ForumPoints -= 1;
                }

                try
                {
                    this.Data.SaveChanges();
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException ex)
                {
                    string result = "";

                    foreach (var error in ex.EntityValidationErrors)
                    {
                        foreach (var err in error.ValidationErrors)
                        {
                            result += err.ErrorMessage + "; ";
                        }   
                    }

                    return Content(result);
                }

                return this.Content(answer.Votes.ToString());
            }

            return this.Content("<strong class=\"vote-error\">Не може да гласувате за собствените си отговори!<a href=\"javascript:void(0)\">OK</a></strong>" + answer.Votes.ToString());
        }
    }
}