using ManagementSystem.Data;
using ManagementSystem.Data.Models;
using ManagementSystem.Web.Infrastructure.Sanitizer;
using ManagementSystem.Web.ViewModels;
using System;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace ManagementSystem.Web.Controllers
{
    [Authorize]
    public class CommentsController : BaseController
    {
        private readonly ISanitizer sanitizer;

        public CommentsController(ManagementSystemData data, ISanitizer sanitizer)
            : base(data)
        {
            this.sanitizer = sanitizer;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Add(int taskId, CommentViewModel comment)
        {
            if (comment.ReminderDate!=null)
            {
                if (comment.ReminderDate < DateTime.Now)
                {
                    return this.JsonError("Reminder date can not be in the past.");
                }

                var task = this.Data.Tasks.GetById(comment.TaskId);
                var requiredByDate = task.RequiredByDate;

                if (!ValidateReminderDate(comment.ReminderDate.Value, requiredByDate))
                {
                    return this.JsonError("Reminder date can not be after Required By Date.");
                }
            }
           
            
            if (comment != null && ModelState.IsValid)
            {
                if (this.sanitizer.Sanitize(comment.Content) != comment.Content)
                {
                    return this.JsonError("Your comment contains potentially dangerous code. Edit it.");
                }

                var newComment = new Comment();

                newComment.DateAdded = DateTime.Now;
                newComment.Type = CommentType.Note;
                newComment.Author = this.UserProfile;
                newComment.TaskId = taskId;
                newComment.Content = comment.Content;
                if (comment.ReminderDate != null)
                {
                    newComment.ReminderDate = comment.ReminderDate;
                    var task = this.Data.Tasks.GetById(comment.TaskId);
                    task.NextActionDate = comment.ReminderDate;
                    this.Data.Tasks.Update(task);
                }

                this.Data.Comments.Add(newComment);
                this.Data.SaveChanges();

                comment.AuthorName = this.UserProfile.UserName;
                comment.DateAdded = DateTime.Now;
                comment.Id = newComment.Id;

                return PartialView("_CommentPartialView", comment);
            }

            return this.JsonError("Unexpexted error");
        }

        //GET Edit comment
        public ActionResult Edit(int commentId)
        {
            var existingCommentAsModel=this.Data.Comments
                .All()
                .Where(c => c.Id == commentId)
                .Project()
                .To<CommentViewModel>()
                .FirstOrDefault();

            return PartialView("_EditComment", existingCommentAsModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(int taskId, CommentViewModel comment)
        {

            if (comment != null && ModelState.IsValid)
            {
                if (this.sanitizer.Sanitize(comment.Content) != comment.Content)
                {
                    return this.JsonError("Your comment contains potentially dangerous code. Edit it.");
                }

                var existingComment = this.Data.Comments
                    .All()
                    .FirstOrDefault(c => c.Id == comment.Id);

                existingComment.Content = comment.Content;
                existingComment.Type = comment.Type;
                existingComment.ReminderDate = comment.ReminderDate;

                this.Data.Comments.Update(existingComment);
                this.Data.SaveChanges();

                return PartialView("_CommentPartialView", comment);
            }

            return this.JsonError("Unexpexted error");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Delete(int commentId)
        {
            var existingComment = this.Data.Comments
                   .All()
                   .FirstOrDefault(c => c.Id == commentId);

            if (existingComment != null && ModelState.IsValid)
            {
                this.Data.Comments.Delete(existingComment);
                this.Data.SaveChanges();

                return new EmptyResult();
            }

            return this.JsonError("Unexpexted error");
        }

        public bool ValidateReminderDate(DateTime reminderDate, DateTime requiredByDate){
            if (reminderDate>requiredByDate)
            {
                return false;
            }
            return true;
        }
    }
}