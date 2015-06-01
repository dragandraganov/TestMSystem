using System;
using System.Linq;
using System.Web.Mvc;
using ManagementSystem.Data;
using AutoMapper.QueryableExtensions;
using ManagementSystem.Data.Models;
using ManagementSystem.Web.ViewModels;
using AutoMapper;
using System.Collections.Generic;
using ManagementSystem.Common;

namespace ManagementSystem.Web.Controllers
{
    [Authorize]
    public class TasksController : BaseController
    {
        public TasksController(ManagementSystemData data)
            : base(data)
        {
        }

        // GET: Tasks
        public ActionResult Index(string query, DateTime? startDate, DateTime? endDate)
        {
            var allTasks = this.Data.Tasks
                .All();

            if (!this.User.IsInRole(GlobalConstants.ManagerRole))
            {
                allTasks = allTasks.Where(t => t.AssignedToUsers.Any(u => u.Id == this.UserProfile.Id));
            }

            if (!(query == null || query.Trim() == String.Empty))
            {
                allTasks = allTasks.Where(t => t.Comments.Any(c => c.Content.Contains(query)));
            }

            if (startDate != null || endDate != null)
            {
                if (startDate == null)
                {
                    startDate = DateTime.Now;
                }
                else if (endDate == null)
                {
                    endDate = DateTime.Now;
                }
                allTasks = allTasks.Where(t => t.RequiredByDate >= startDate && t.RequiredByDate <= endDate);
            }

            var allTasksModel = allTasks.Project()
                .To<TaskViewModel>()
                .ToList();

            return View(allTasksModel);
        }

        //GET: Create task
        [Authorize(Roles = GlobalConstants.ManagerRole)]
        public ActionResult Create()
        {
            var complexModel = new ComplexViewModel();
            //TODO Write as SQL query
            complexModel.AllUsers = this.Data.Users
                .All()
                .Project()
                .To<UserViewModel>()
                .ToList();

            complexModel.TaskViewModel = new TaskViewModel();

            return View(complexModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = GlobalConstants.ManagerRole)]
        public ActionResult Create(ComplexViewModel model)
        {
            if (model != null && ModelState.IsValid)
            {
                var newTask = Mapper.Map<Task>(model.TaskViewModel);

                newTask.CreatedOnDate = DateTime.Now;

                foreach (var selectedUserId in model.SelectedUsersId)
                {
                    var user = this.Data.Users
                        .All()
                        .FirstOrDefault(u => u.Id == selectedUserId);

                    if (user != null)
                    {
                        newTask.AssignedToUsers.Add(user);
                    }
                }

                this.Data.Tasks.Add(newTask);
                this.Data.SaveChanges();
                TempData["Success"] = "A new task was created";
                return RedirectToAction("Index", "Tasks");
            }
            model.AllUsers = this.Data.Users
               .All()
               .Project()
               .To<UserViewModel>()
               .ToList();
            return View(model);
        }

        //GET: Edit task
        [Authorize(Roles = GlobalConstants.ManagerRole)]
        public ActionResult Edit(int id)
        {
            var existingTaskModel = this.Data.Tasks
                .All()
                .Where(t => t.Id == id)
                .Project()
                .To<TaskViewModel>()
                .FirstOrDefault();

            if (existingTaskModel == null)
            {
                return new HttpNotFoundResult("Task not found");
            }

            return View(existingTaskModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = GlobalConstants.ManagerRole)]
        public ActionResult Edit(TaskViewModel taskModel)
        {
            if (taskModel != null && ModelState.IsValid)
            {
                var existingTask = this.Data
                   .Tasks
                   .GetById(taskModel.Id);

                existingTask.Description = taskModel.Description;
                existingTask.NextActionDate = taskModel.NextActionDate;
                existingTask.Type = taskModel.Type;
                existingTask.Status = taskModel.Status;

                this.Data.Tasks.Update(existingTask);
                this.Data.SaveChanges();
                TempData["Success"] = "The task was changed successfuly";
                return RedirectToAction("Index", "Tasks");
            }

            return View(taskModel);
        }


        //GET: Delete task
        [Authorize(Roles = GlobalConstants.ManagerRole)]
        public ActionResult Delete(int id)
        {
            var existingTaskModel = this.Data.Tasks
                .All()
                .Where(t => t.Id == id)
                .Project()
                .To<TaskViewModel>()
                .FirstOrDefault();

            if (existingTaskModel == null)
            {
                return new HttpNotFoundResult("Task not found");
            }

            return View(existingTaskModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = GlobalConstants.ManagerRole)]
        public ActionResult Delete(TaskViewModel taskModel)
        {
            if (taskModel != null && ModelState.IsValid)
            {
                var existingTask = this.Data
                   .Tasks
                   .GetById(taskModel.Id);

                this.Data.Tasks.Delete(existingTask);
                this.Data.SaveChanges();
                TempData["Success"] = "The task was deleted successfuly";
                return RedirectToAction("Index", "Tasks");
            }

            return View(taskModel);
        }

        //GET: Edit task
        public ActionResult Details(int id)
        {
            var existingTask = this.Data.Tasks
                .All()
                .Where(t => t.Id == id)
                .FirstOrDefault();

            if (existingTask == null)
            {
                return new HttpNotFoundResult("Task not found");
            }

            if (!(existingTask.AssignedToUsers.Contains(this.UserProfile) || this.User.IsInRole(GlobalConstants.ManagerRole)))
            {
                return new HttpNotFoundResult("It is not your task");
            }

            var taskModel = Mapper.Map<Task, TaskViewModel>(existingTask);

            taskModel.Comments = Mapper.Map<ICollection<Comment>, IList<CommentViewModel>>(existingTask.Comments);


            return View(taskModel);
        }
    }
}